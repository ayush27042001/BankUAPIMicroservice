using BankUAPI.Application.Interface;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel.AddFund;
using BankUAPI.SharedKernel.Request;
using BankUAPI.SharedKernel.Response;
using BankUAPI.SharedKernel.Response.BankAccount;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZstdSharp.Unsafe;
using static System.Net.Mime.MediaTypeNames;

namespace BankUAPI.Application.Implementation
{
    public class AgreementSignService : IAgreementSignService
    {

        private readonly AppDbContext _db;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CashfreeSetting _cashfree;
        private readonly IHostEnvironment _env;
        private readonly TicketSetting _TicketSetting;
        private readonly AgreementSetting _agreementSetting;

        public AgreementSignService(
            AppDbContext db,
            IHttpClientFactory httpClientFactory,
            IOptions<CashfreeSetting> cashfree,
            IOptions<TicketSetting> ticketSetting,
            IOptions<AgreementSetting> agreementSetting,
            IHostEnvironment env)
        {
            _db = db;
            _httpClientFactory = httpClientFactory;
            _cashfree = cashfree.Value;
            _TicketSetting = ticketSetting.Value;
            _agreementSetting = agreementSetting.Value;
            _env = env;
        }

        // SEND OTP
        public async Task<ApiResponse<AadhaarOtpResponse>> SendAadhaarOtpAsync(
   AadhaarRequest request,
   CancellationToken cn)
        {
            try
            {
                //  VALIDATE USER ID
                if (!long.TryParse(request.UserId.ToString(), out long uid))
                {
                    return new ApiResponse<AadhaarOtpResponse>
                    {
                        Status = "ERR",
                        Message = "Invalid UserId"
                    };
                }

                var user = await _db.Registrations
                    .FirstOrDefaultAsync(x => x.RegistrationId == uid, cn);

                if (user == null)
                {
                    return new ApiResponse<AadhaarOtpResponse>
                    {
                        Status = "ERR",
                        Message = "User not found"
                    };
                }

                if (string.IsNullOrEmpty(user.AadharNo))
                {
                    return new ApiResponse<AadhaarOtpResponse>
                    {
                        Status = "ERR",
                        Message = "Aadhaar not found"
                    };
                }

                // ✅ CALL CASHFREE
                var client = _httpClientFactory.CreateClient();

                var body = new
                {
                    aadhaar_number = user.AadharNo
                };

                var httpRequest = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"{_cashfree.BaseUrl}verification/offline-aadhaar/otp"
                );

                httpRequest.Headers.Add("x-client-id", _cashfree.ClientId);
                httpRequest.Headers.Add("x-client-secret", _cashfree.ClientSecret);

                httpRequest.Content = new StringContent(
                    JsonSerializer.Serialize(body),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.SendAsync(httpRequest, cn);
                var content = await response.Content.ReadAsStringAsync(cn);

                var json = JObject.Parse(content);

                return new ApiResponse<AadhaarOtpResponse>
                {
                    Status = "SUCCESS",
                    Message = "OTP Sent",
                    Data = new AadhaarOtpResponse
                    {
                        RefId = json["ref_id"]?.ToString(),
                        Status = json["status"]?.ToString()
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<AadhaarOtpResponse>
                {
                    Status = "ERR",
                    Message = ex.Message
                };
            }
        }

        // ✅ VERIFY + SIGN
        public async Task<ApiResponse<AgreementSignResult>> VerifyAndSignAsync(
       VerifyOtpRequest request,
       CancellationToken cn)
        {
            try
            {
                var agreement = await _db.UserAgreements
                    .FirstOrDefaultAsync(x => x.AgreementId == request.AgreementId, cn);

                if (agreement == null)
                    return new ApiResponse<AgreementSignResult>
                    {
                        Status = "ERR",
                        Message = "Agreement not found"
                    };

                if (agreement.Status == "Signed" || agreement.Status == "Approved")
                    return new ApiResponse<AgreementSignResult>
                    {
                        Status = "ERR",
                        Message = "Already signed"
                    };

                if (string.IsNullOrWhiteSpace(agreement.FilePath))
                    return new ApiResponse<AgreementSignResult>
                    {
                        Status = "ERR",
                        Message = "File path missing"
                    };

                string name = "";

                // ✅ TEST BYPASS
                //if (request.AgreementId == "12345")
                //{
                //    name = "TEST USER";
                //}
                //else
                //{
                    var client = _httpClientFactory.CreateClient();

                    var body = new
                    {
                        otp = request.Otp,
                        ref_id = request.RefId
                    };

                    var httpRequest = new HttpRequestMessage(
                        HttpMethod.Post,
                        $"{_cashfree.BaseUrl}verification/offline-aadhaar/verify"
                    );

                    httpRequest.Headers.Add("x-client-id", _cashfree.ClientId);
                    httpRequest.Headers.Add("x-client-secret", _cashfree.ClientSecret);

                    httpRequest.Content = new StringContent(
                        JsonSerializer.Serialize(body),
                        Encoding.UTF8,
                        "application/json");

                    var response = await client.SendAsync(httpRequest, cn);
                    var content = await response.Content.ReadAsStringAsync(cn);

                    var json = JObject.Parse(content);

                    if (json["status"]?.ToString() != "VALID")
                        return new ApiResponse<AgreementSignResult>
                        {
                            Status = "ERR",
                            Message = "Invalid OTP"
                        };

                    name = json["name"]?.ToString()?.ToUpper();
                //}

                // FILE DOWNLOAD (FIXED)

                string inputPath = agreement.FilePath.Replace("~/", "").Trim();

                if (!Uri.IsWellFormedUriString(inputPath, UriKind.Absolute))
                {
                    inputPath = $"{_agreementSetting.BaseUrl.TrimEnd('/')}/{inputPath.TrimStart('/')}";
                }

                var httpClient = _httpClientFactory.CreateClient();

                // REQUIRED HEADERS (avoid 403)
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
                httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
                httpClient.DefaultRequestHeaders.Referrer = new Uri(_agreementSetting.BaseUrl);

                HttpResponseMessage fileResponse = null;

                // RETRY (3 times)
                for (int i = 0; i < 3; i++)
                {
                    fileResponse = await httpClient.GetAsync(inputPath, cn);

                    if (fileResponse.IsSuccessStatusCode)
                        break;

                    await Task.Delay(500, cn);
                }

                if (fileResponse == null || !fileResponse.IsSuccessStatusCode)
                {
                    return new ApiResponse<AgreementSignResult>
                    {
                        Status = "ERR",
                        Message = $"Download failed: {fileResponse?.StatusCode}"
                    };
                }

                byte[] fileBytes = await fileResponse.Content.ReadAsByteArrayAsync(cn);

                string tempFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "Temp");

                if (!Directory.Exists(tempFolder))
                    Directory.CreateDirectory(tempFolder);

                string tempFilePath = Path.Combine(tempFolder, Guid.NewGuid() + ".pdf");

                try
                {
                    await File.WriteAllBytesAsync(tempFilePath, fileBytes, cn);

                    // SIGN PDF
                    string relativePath = AddSignatureToPdf(tempFilePath, name);

                    string fullUrl = $"{_TicketSetting.BaseUrl.TrimEnd('/')}/{relativePath}";

                    // ✅ SAVE DB
                    agreement.Status = "Signed";
                    agreement.FullName = name;
                    agreement.UserSignedPath = fullUrl;

                    await _db.SaveChangesAsync(cn);

                    return new ApiResponse<AgreementSignResult>
                    {
                        Status = "SUCCESS",
                        Message = "Document signed successfully",
                        Data = new AgreementSignResult
                        {
                            FullName = name,
                            SignedFilePath = fullUrl
                        }
                    };
                }
                finally
                {
                    // 🧹 CLEAN TEMP FILE ALWAYS
                    if (File.Exists(tempFilePath))
                        File.Delete(tempFilePath);
                }
            }
            catch (Exception)
            {
                return new ApiResponse<AgreementSignResult>
                {
                    Status = "ERR",
                    Message = "Something went wrong"
                };
            }
        }

        // SIGN PDF FUNCTION
        private string AddSignatureToPdf(string inputPdfPath, string signerName)
        {
            string folder = Path.Combine(_env.ContentRootPath, "wwwroot", "SignedDocs");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fileName = "Signed_" + Guid.NewGuid() + ".pdf";
            string outputPdfPath = Path.Combine(folder, fileName);

            string timeStamp = DateTime.Now.ToString("dd MMM yyyy 'at' HH:mm:ss");

            using (PdfReader reader = new PdfReader(inputPdfPath))
            using (FileStream fs = new FileStream(outputPdfPath, FileMode.Create))
            using (PdfStamper stamper = new PdfStamper(reader, fs))
            {
                int totalPages = reader.NumberOfPages;

                var nameFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD);
                var smallFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL);

                for (int i = 1; i <= totalPages; i++)
                {
                    PdfContentByte cb = stamper.GetOverContent(i);

                    float xRight = reader.GetPageSize(i).Right - 40;
                    float yBottom = reader.GetPageSize(i).Bottom + 50;

                    ColumnText.ShowTextAligned(cb, Element.ALIGN_RIGHT,
                        new Phrase(signerName, nameFont),
                        xRight, yBottom + 40, 0);

                    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT,
                        new Phrase(timeStamp, smallFont),
                        xRight - 150, yBottom + 25, 0);

                    cb.MoveTo(xRight - 150, yBottom + 23);
                    cb.LineTo(xRight, yBottom + 23);
                    cb.Stroke();

                    ColumnText.ShowTextAligned(cb, Element.ALIGN_RIGHT,
                        new Phrase("eSign", smallFont),
                        xRight, yBottom + 25, 0);
                }
            }

            return "SignedDocs/" + fileName;
        }
    }
}

