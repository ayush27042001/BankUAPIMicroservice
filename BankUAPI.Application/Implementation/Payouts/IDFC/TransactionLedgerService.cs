using BankUAPI.Infrastructure.Sql.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Payouts.IDFC
{
    public sealed class TransactionLedgerService
    {
        private readonly AppDbContext _db;

        public TransactionLedgerService(AppDbContext db)
            => _db = db;

        public async Task RecordBalanceAsync(
            string clientCode,
            string internalTxnId,
            string debitAccount,
            string status,
            string requestJson,
            string responseJson,
            string TxnType,
            string BankCode = "",
            string TransactionType = "",
            string ExternalTxnId="",
            string CreditAccount="",
            string Amount="",
            string BankResponseCode="",
            string BankResponseMessage=""
            )
        {
            _db.TransactionLedger.Add(new TransactionLedger
            {
                ClientCode = clientCode,
                BankCode = BankCode,
                TransactionType = TxnType,
                ExternalTxnId= ExternalTxnId,
                InternalTxnId = internalTxnId,
                DebitAccount = debitAccount,
                CreditAccount = CreditAccount,
                Amount= Amount==""?0:Convert.ToDecimal(Amount),
                Status = status,
                BankResponseCode= BankResponseCode,
                BankResponseMessage = BankResponseMessage,
                RequestJson = requestJson,
                ResponseJson = responseJson
            });

            await _db.SaveChangesAsync();
        }
    }

}
