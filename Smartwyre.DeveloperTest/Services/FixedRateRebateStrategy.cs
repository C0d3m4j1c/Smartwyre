using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

public class FixedRateRebateStrategy : IIncentiveCalculationStrategy
{
    public bool IsApplicable(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        return rebate.Incentive == IncentiveType.FixedRateRebate &&
               product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate) &&
               rebate.Percentage > 0 && product.Price > 0 && request.Volume > 0;
    }

    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        return product.Price * rebate.Percentage * request.Volume;
    }
}
