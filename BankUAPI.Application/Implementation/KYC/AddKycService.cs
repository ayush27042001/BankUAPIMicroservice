using BankUAPI.Application.Interface.KYC;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel.AddFund;
using BankUAPI.SharedKernel.Request.KYC;
using BankUAPI.SharedKernel.Response.KYC;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.KYC
{
    public class AddKycService : IAddKycService
    {
        private readonly AppDbContext _db;
        private readonly TicketSetting _TicketSetting;
        public AddKycService(IOptions<TicketSetting> ticketSetting, AppDbContext db)
        {
            _TicketSetting = ticketSetting.Value;
            _db = db;
        }

        public async Task<KycResponse> UploadKycAsync(AddKycRequest request, CancellationToken cn)
        {
            try
            {
                if (string.IsNullOrEmpty(request.RegistrationId))
                {
                    return new KycResponse
                    {
                        Status = "ERR",
                        Message = "RegistrationId is required"
                    };
                }

                if (!long.TryParse(request.RegistrationId, out long regId))
                {
                    return new KycResponse
                    {
                        Status = "ERR",
                        Message = "Invalid RegistrationId"
                    };
                }

                var user = await _db.Registrations
                    .FirstOrDefaultAsync(x => x.RegistrationId == regId, cn);

                if (user == null)
                {
                    return new KycResponse
                    {
                        Status = "ERR",
                        Message = "User not found"
                    };
                }

                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/KYC");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string[] allowedExt = { ".jpg", ".jpeg", ".png", ".pdf" };

                List<KycFileInfo> uploadedFiles = new List<KycFileInfo>();

                async Task<string> SaveFile(IFormFile file, string fileType)
                {
                    if (file == null || file.Length == 0)
                        return null;

                    string ext = Path.GetExtension(file.FileName).ToLower();

                    if (!allowedExt.Contains(ext))
                        throw new Exception($"{fileType} must be JPG, PNG or PDF");

                    if (file.Length > 5 * 1024 * 1024)
                        throw new Exception($"{fileType} file size must be under 5MB");

                    string fileName = Guid.NewGuid().ToString() + ext;

                    string fullPath = Path.Combine(folder, fileName);

                    using var stream = new FileStream(fullPath, FileMode.Create);
                    await file.CopyToAsync(stream, cn);

                    // Generate full URL
                    string fileUrl = $"{_TicketSetting.BaseUrl.TrimEnd('/')}/KYC/{fileName}";

                    uploadedFiles.Add(new KycFileInfo
                    {
                        FileType = fileType,
                        FilePath = fileUrl,
                        FileSize = file.Length
                    });

                    return fileUrl;
                }

                user.aadharUpload = await SaveFile(request.AadharUpload, "Aadhar");
                user.panUpload = await SaveFile(request.PanUpload, "PAN");
                user.photoUpload = await SaveFile(request.PhotoUpload, "Photo");
                user.gstUpload = await SaveFile(request.GstUpload, "GST");
                user.ShopFrontupload = await SaveFile(request.ShopFrontUpload, "ShopFront");
                user.ShopInupload = await SaveFile(request.ShopInUpload, "ShopInside");
                user.KycApplication = await SaveFile(request.KycApplication, "KycApplication");

                user.BusinessProofUploadtype = request.BusinessProofUploadtype;
                user.KycStatus = "Pending";

                _db.Registrations.Update(user);

                await _db.SaveChangesAsync(cn);

                return new KycResponse
                {
                    Status = "SUCCESS",
                    Message = "KYC uploaded successfully",
                    Files = uploadedFiles
                };
            }
            catch (Exception ex)
            {
                return new KycResponse
                {
                    Status = "ERR",
                    Message = ex.Message
                };
            }
        }
    }
}
