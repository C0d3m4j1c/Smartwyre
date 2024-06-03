using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

public class FixedCashAmountStrategy : IIncentiveCalculationStrategy
{
    public bool IsApplicable(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        return rebate.Incentive == IncentiveType.FixedCashAmount &&
               product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount) &&
               rebate.Amount > 0;
    }

    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        return rebate.Amount;
    }
}
