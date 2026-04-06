using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.MicroLoan
{
    public class CheckLeadRequest
    {
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
    }
    public class CreateLoanRequest
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string PAN { get; set; }
        public string Gender { get; set; }
        public int BusinessAge { get; set; }
        public string LoanType { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public string LoanAmount { get; set; }
    }
    public class UploadLoanDocsRequest
    {
        public string ApplicationId { get; set; }

        public IFormFile Pan { get; set; }
        public IFormFile Gst { get; set; }
        public IFormFile Bank { get; set; }
        public IFormFile Aadhar { get; set; }
        public IFormFile Photo { get; set; }
    }
    public class LoanStatusRequest
    {
        public string ApplicationId { get; set; }
    }
    public class LoanTermsRequest
    {
        public string ApplicationId { get; set; }
    }
    public class LoanDisbursalRequest
    {
        public string ApplicationId { get; set; }
    }
    public class ReUploadLoanDocRequest
    {
        public string ApplicationId { get; set; }
        public IFormFile File { get; set; }
    }
    public class LoanApplicationsByUserRequest
    {
        public string UserId { get; set; }
    }
    public class UpdateLoanApplicationRequest
    {
        public string ApplicationId { get; set; }
        public string FieldName { get; set; }
        public string Value { get; set; }
    }
}
