using System;
using System.Collections.Generic;
using System.Text;

namespace UmhBackend.HouseholdEntity
{
    public record HouseHoldPortfolio(string HouseHoldID, decimal TaxableValue, decimal TaxAdvantagedValue) {
        // A helpful read-only property computing total wealth
        public decimal TotalValue => TaxableValue + TaxAdvantagedValue;
    }

    public interface IHouseholdService
    {
        HouseHoldPortfolio CalculateHouseHoldWealth(string HouseHoldId);
        bool Equal(HouseHoldPortfolio h1, HouseHoldPortfolio h2);

    }

    public class HouseholdService : IHouseholdService
    {
        public HouseHoldPortfolio CalculateHouseHoldWealth(string HouseHoldId) {

            var obj = new HouseHoldPortfolio(
                HouseHoldID: HouseHoldId,
                TaxableValue: 520000.50m,
                TaxAdvantagedValue: 780000.25m
            );

            // Simulating a calculation or mock database lookup
            return obj;
        }

        public bool Equal(HouseHoldPortfolio obj1, HouseHoldPortfolio obj2)
        {
            return HouseHoldPortfolio.Equals(obj1,obj2);

        }
    }
}
