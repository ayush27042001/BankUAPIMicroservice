using BankUAPI.Application.Interface.KYC;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel.AddFund;
using BankUAPI.SharedKernel.Response.KYC;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.KYC
{
    public class GetKycService : IGetKycService
    {
        private readonly AppDbContext _db;
        private readonly AgreementSetting _agreementSettings;
        public GetKycService(AppDbContext db, IOptions<AgreementSetting> agreementSettings)
        {
            _db = db;
            _agreementSettings = agreementSettings.Value;
        }

        public async Task<GetKycResponse> GetKycAsync(string registrationId, CancellationToken cn)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(registrationId))
                {
                    return new GetKycResponse
                    {
                        Status = "ERR",
                        Message = "RegistrationId is required",
                        KycStatus = "ERR",
                        Documents = null
                    };
                }

                if (!long.TryParse(registrationId, out long regId))
                {
                    return new GetKycResponse
                    {
                        Status = "ERR",
                        Message = "Invalid RegistrationId",
                        KycStatus = "ERR",
                        Documents = null
                    };
                }

                var user = await _db.Registrations
                    .FirstOrDefaultAsync(x => x.RegistrationId == regId, cn);

                if (user == null)
                {
                    return new GetKycResponse
                    {
                        Status = "ERR",
                        Message = "User not found",
                        KycStatus = "ERR",
                        Documents = null
                    };
                }

                var documents = new List<KycDocument>();

                if (!string.IsNullOrEmpty(user.aadharUpload))
                    documents.Add(new KycDocument
                    {
                        DocumentType = "Aadhar",
                        FileUrl = ResolveFileUrl(user.aadharUpload)
                    });

                if (!string.IsNullOrEmpty(user.panUpload))
                    documents.Add(new KycDocument
                    {
                        DocumentType = "PAN",
                        FileUrl = ResolveFileUrl(user.panUpload)
                    });

                if (!string.IsNullOrEmpty(user.photoUpload))
                    documents.Add(new KycDocument
                    {
                        DocumentType = "Photo",
                        FileUrl = ResolveFileUrl(user.photoUpload)
                    });

                if (!string.IsNullOrEmpty(user.gstUpload))
                    documents.Add(new KycDocument
                    {
                        DocumentType = "GST",
                        FileUrl = ResolveFileUrl(user.gstUpload)
                    });

                if (!string.IsNullOrEmpty(user.ShopFrontupload))
                    documents.Add(new KycDocument
                    {
                        DocumentType = "ShopFront",
                        FileUrl = ResolveFileUrl(user.ShopFrontupload)
                    });

                if (!string.IsNullOrEmpty(user.ShopInupload))
                    documents.Add(new KycDocument
                    {
                        DocumentType = "ShopInside",
                        FileUrl = ResolveFileUrl(user.ShopInupload)
                    });

                if (!string.IsNullOrEmpty(user.KycApplication))
                    documents.Add(new KycDocument
                    {
                        DocumentType = "KycApplication",
                        FileUrl = ResolveFileUrl(user.KycApplication)
                    });

                return new GetKycResponse
                {
                    Status = "SUCCESS",
                    Message = "KYC fetched successfully",
                    KycStatus = user.KycStatus,
                    Documents = documents
                };
            }
            catch (Exception ex)
            {
                return new GetKycResponse
                {
                    Status = "ERR",
                    Message = ex.Message,
                    KycStatus = "ERR",
                    Documents = null
                };
            }
        }
        private string ResolveFileUrl(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return null;

            // If already full URL, return as-is
            if (filePath.StartsWith("http://") || filePath.StartsWith("https://"))
                return filePath;

            // Remove "~/" if present
            filePath = filePath.Replace("~/", "");

            // Attach BaseUrl
            return $"{_agreementSettings.BaseUrl.TrimEnd('/')}/{filePath.TrimStart('/')}";
        }
    }
}
