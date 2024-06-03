using System.Collections.Generic;
using Smartwyre.DeveloperTest.Types;

public class RebateDataStore : IRebateDataStore
{
    private readonly Dictionary<string, Rebate> _rebates = new Dictionary<string, Rebate>();

    public void AddRebate(string rebateIdentifier, Rebate rebate)
    {
        _rebates[rebateIdentifier] = rebate;
    }

    public Rebate GetRebate(string rebateIdentifier)
    {
        _rebates.TryGetValue(rebateIdentifier, out var rebate);
        return rebate;
    }

    public void StoreCalculationResult(Rebate rebate, decimal rebateAmount)
    {
        // Implement the logic to store the result of the rebate calculation.
    }
}
