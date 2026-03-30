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
        Task<ApiResponse<CheckLeadResponse>> CheckLead(CheckLeadRequest request, CancellationToken cn);

        Task<ApiResponse<CreateLoanResponse>> CreateApplication(CreateLoanRequest request, CancellationToken cn);

        Task<ApiResponse<string>> UploadDocuments(UploadLoanDocsRequest request, CancellationToken cn);

        Task<ApiResponse<LoanStatusResponse>> GetLoanStatus(LoanStatusRequest request, CancellationToken cn);

        Task<ApiResponse<LoanTermsResponse>> GetLoanTerms(LoanTermsRequest request, CancellationToken cn);

        Task<ApiResponse<LoanDisbursalResponse>> GetDisbursal(LoanDisbursalRequest request, CancellationToken cn);
    }
}
