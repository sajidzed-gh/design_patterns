using System;
using System.Collections.Generic;
using System.Text;
using UmhBackend.Repository;

namespace UmhBackend.StructuralPatterns.wrapper
{
    public class LoggingHouseholdRepository : IHouseholdRepository
    {

        private readonly IHouseholdRepository _innerdRepository;

        //pass the real repository into our decorator constructor
        public LoggingHouseholdRepository(IHouseholdRepository innerRepository) { 
            _innerdRepository = innerRepository;
        }
        public List<DbAccountRow> GetRawAccountsByHouseholdId(string householdId)
        {
            // 1. Add custom behavior before the real call
            Console.WriteLine($"[AUDIT LOG] Initiating Supabase fetch for household: {householdId}");
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // 2. Delegate execution to the wrapped inner repository
            var data = _innerdRepository.GetRawAccountsByHouseholdId(householdId);

            // 3. Add custom behavior after the real call
            stopwatch.Stop();
            Console.WriteLine($"[AUDIT LOG] Successfully fetched {data.Count} accounts in {stopwatch.ElapsedMilliseconds}ms");

            return data;

        }

    }
}
