using BankUAPI.SharedKernel.Request.MicroLoan;
using BankUAPI.SharedKernel.Response;
using BankUAPI.SharedKernel.Response.BankAccount;
using BankUAPI.SharedKernel.Response.MicroLoan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.MicroLoan
{
    public interface ILoanService
    {
        Task<ApiResponse<CheckLeadResponse>> CheckLeadAsync(CheckLeadRequest request, CancellationToken cn);

        Task<ApiResponse<CreateLoanResponse>> CreateApplicationAsync(CreateLoanRequest request, CancellationToken cn);

        Task<ApiResponse<string>> UploadDocumentsAsync(UploadLoanDocsRequest request, CancellationToken cn);

        Task<ApiResponse<LoanStatusResponse>> GetLoanStatusAsync(LoanStatusRequest request, CancellationToken cn);

        Task<ApiResponse<LoanTermsResponse>> GetLoanTermsAsync(LoanTermsRequest request, CancellationToken cn);

        Task<ApiResponse<LoanDisbursalResponse>> GetDisbursalAsync(LoanDisbursalRequest request, CancellationToken cn);
    }
}
