using BankUAPI.Application.Interface.Commision.CommisionDistribution;
using BankUAPI.Application.Interface.DMT.Provider;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel;
using BankUAPI.SharedKernel.Response.DMT.InstantPay;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.DMT.InstantPay
{
    public class DmtCommissionService : IDmtCommissionService
    {
        private readonly AppDbContext _db;
        private readonly ICommissionDistributionService _commisionDis;

        public DmtCommissionService(AppDbContext db, ICommissionDistributionService commissionDistributionService)
        {
            _db = db;
            _commisionDis = commissionDistributionService;
        }

        private static decimal ResolveValue(decimal baseAmount, decimal value, int type)
        {
            return type == 1
                ? value
                : Round(baseAmount * value / 100m);
        }

        public async Task<CommissionResponse> ApplyAsync(decimal txnAmount, string serviceId, string providerId, string? operatorId, string? UserId)
        {
            
            Registration retailer;
            retailer = _db.Registrations.Where(id => id.RegistrationId == Convert.ToInt32(UserId)).FirstOrDefault();


            var rate = await _commisionDis.GetDmtCommissionAsync(serviceId, providerId, operatorId, txnAmount);

            //Validate(rate);

            decimal totalCharge = ResolveValue(txnAmount, rate.RetailerValue ?? 0, rate.RetailerType ?? 0);
            decimal distributable = Round(totalCharge * 0.50m);
            decimal adminComm = 0;
            decimal distComm = 0;


            using var tx = await _db.Database.BeginTransactionAsync();

            try
            {
                var balances = await GetLatestBalancesAsync(
                    retailer?.RegistrationId,
                    retailer?.ADId,
                    retailer?.WLId
                );

                // Retailer FULL DEBIT
                AddLedger(
                    retailer.RegistrationId,
                    balances[retailer.RegistrationId],
                    totalCharge,
                    "DR",
                    "DMT_CHARGE",
                    "DMT charge debit"
                );
                decimal oldbalance = balances[retailer.RegistrationId] - totalCharge;
                AddLedger(
                    retailer.RegistrationId,
                    oldbalance,
                    txnAmount,
                    "DR",
                    "DMT_Txn",
                    "DMT Txn debit"
                );

                if (retailer.ADId != null)
                {
                    distComm = ResolveValue(distributable, rate.DistributorValue ?? 0, rate.DistributorType ?? 0);
                    adminComm = Round(distributable - distComm);

                    AddLedger(
                        retailer?.ADId ?? 0,
                        balances[retailer?.ADId ?? 0],
                        distComm,
                        "CR",
                        "DMT_COMMISSION",
                        "Distributor DMT commission"
                    );

                    AddLedger(
                        retailer?.WLId ?? 0,
                        balances[retailer?.WLId ?? 0],
                        adminComm,
                        "CR",
                        "DMT_COMMISSION",
                        "Admin DMT commission"
                    );
                }
                else
                {
                    adminComm = distributable;

                    AddLedger(
                        retailer?.WLId ?? 0,
                        balances[retailer?.WLId ?? 0],
                        adminComm,
                        "CR",
                        "DMT_COMMISSION",
                        "Admin DMT commission (Direct)"
                    );
                }

                await _db.SaveChangesAsync();
                await tx.CommitAsync();
                return new CommissionResponse()
                {
                    success = true,
                    AdminCommision = adminComm,
                    DistributerCommision = distComm,
                    RetailerCommissionCharge = totalCharge,
                    WalletAmount = balances[retailer.RegistrationId]
                };
            }
            catch
            {
                await tx.RollbackAsync();
                return new CommissionResponse()
                {
                    success = false,
                    AdminCommision = adminComm,
                    DistributerCommision = distComm,
                    RetailerCommissionCharge = totalCharge,
                    WalletAmount = 0
                };
            }
        }

        private async Task<Dictionary<int, decimal>> GetLatestBalancesAsync(
            params int?[] userIds)
        {
            var ids = userIds.Where(x => x.HasValue)
                             .Select(x => x!.Value)
                             .Distinct()
                             .ToList();

            return await _db.Tbluserbalances
                .Where(x => ids.Contains(x.UserId!.Value))
                .GroupBy(x => x.UserId!.Value)
                .Select(g => new
                {
                    UserId = g.Key,
                    Balance = g.OrderByDescending(x => x.Id)
                               .Select(x => x.NewBal ?? 0)
                               .First()
                })
                .ToDictionaryAsync(x => x.UserId, x => x.Balance);
        }

        private void AddLedger(int userId, decimal oldBal, decimal amount, string crDr, string txnType, string remarks)
        {
            if (amount <= 0) return;

            _db.Tbluserbalances.Add(new Tbluserbalance
            {
                UserId = userId,
                OldBal = oldBal,
                Amount = amount,
                NewBal = crDr == "CR" ? oldBal + amount : oldBal - amount,
                CrDrType = crDr,
                TxnType = txnType,
                Remarks = remarks,
                TxnDatetime = DateTime.UtcNow
            });
        }

        private static decimal Round(decimal v)
            => Math.Round(v, 2, MidpointRounding.AwayFromZero);
    }
}
