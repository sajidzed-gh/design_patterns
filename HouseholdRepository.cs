using System;
using System.Collections.Generic;
using System.Text;

namespace UmhBackend.Repository
{
    // --- THE DATA ACQUISITION LAYER (REPOSITORY PATTERN) ---

    // This represents the structure of a raw row inside table
    public record DbAccountRow(string AccountNumber, string AccountType, decimal CurrentBalance);
    
    public interface IHouseholdRepository
    { 
        List<DbAccountRow> GetRawAccountsByHouseholdId(string HouseholdId);
    }

    public class HouseholdRepository : IHouseholdRepository
    {
        public List<DbAccountRow> GetRawAccountsByHouseholdId(string HouseholdId)
        {
            return new List<DbAccountRow>
            {
                  new DbAccountRow("ACT-SUB-99", "TAXABLE", 50000.00m),
                  new DbAccountRow("ACT-SUB-88", "401K", 420000.00m)
            };
        }
    }
}
