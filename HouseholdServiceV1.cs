using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Principal;
using System.Text;
using UmhBackend.Repository;

namespace UmhBackend.FactoryPattern
{
    public record HouseholdPortfolio(string HouseholdId, decimal TotalValue, List<AccountDetails> Accounts);
    public record AccountDetails(string AccountNumber, string Type, decimal Balance, decimal EstimatedTaxOwed);

    // --- THE FACTORY PATTERN IMPLEMENTATION ---
    
    // The abstraction for our financial accounts
    public abstract class Account
    {
        public string AccountNumber { get; init; } = string.Empty;
        public decimal Balance { get; init; }

        // Each account type calculates its embedded tax liability differently
        public abstract decimal CalculateTaxLiability();

    }

    // Concrete Account 1: Taxable Brokerage (Subject to Capital Gains, e.g., 15%)
    public class TaxableAccount : Account
    {
        public override decimal CalculateTaxLiability() => Balance * 0.15m;
        
    }

    // Concrete Account 2: Tax-Deferred Traditional 401k (Subject to Income Tax, e.g., 25%)
    public class TaxDeferredAccount : Account {
        public override decimal CalculateTaxLiability() => Balance * 0.25m;
        
    }

    // The Account Factory Engine
    public static class AccountFactory
    {
        public static Account CreateAccount(string accountType, string accountNumber, decimal balance)
        {
            return accountType.ToUpper() switch
            {
                "TAXABLE" => new TaxableAccount { AccountNumber = accountNumber, Balance = balance },
                "401K" => new TaxDeferredAccount { AccountNumber = accountNumber, Balance = balance },
                _ => throw new ArgumentException($"Unsupported account type: {accountType}")


            };
        }
    }


    // Core Business Service

    public interface IHouseholdService {
        HouseholdPortfolio CalculateHouseHoldWealth(string householdId);
    }

    public class HouseholdService : IHouseholdService
    {
        private readonly IHouseholdRepository householdRepository;

        public HouseholdService(IHouseholdRepository _householdRepository)
        {
            householdRepository = _householdRepository; 
        }
        public HouseholdPortfolio CalculateHouseHoldWealth(string householdId)
        {

            var rawRows=householdRepository.GetRawAccountsByHouseholdId(householdId);

            var accountList = new List<AccountDetails>();
            decimal totalWealth = 0;

            foreach (var row in rawRows) {
                //Using our Factory to generate different account types dynamically
               var domainAccount = AccountFactory.CreateAccount(row.AccountType, row.AccountNumber, row.CurrentBalance);
                totalWealth += domainAccount.Balance;

                accountList.Add(new AccountDetails(domainAccount.AccountNumber, row.AccountType, domainAccount.Balance, domainAccount.CalculateTaxLiability()));
            }

            // Using our Factory to generate different account types dynamically
            /*var brokerage = AccountFactory.CreateAccount("TAxable", "ACT-111", 500000.00m);
            var retirement = AccountFactory.CreateAccount("401k", "ACR-222", 600000.00m);

            var accountList = new List<AccountDetails>
            { new AccountDetails(brokerage.AccountNumber, "Taxable Brokerage",brokerage.Balance,brokerage.CalculateTaxLiability()),
              new AccountDetails(retirement.AccountNumber, "Traditional 401(k)", retirement.Balance, retirement.CalculateTaxLiability())

            };

            var totalWealth = brokerage.Balance + retirement.Balance;*/

            return new HouseholdPortfolio(householdId, totalWealth, accountList);
        }
    }
}
