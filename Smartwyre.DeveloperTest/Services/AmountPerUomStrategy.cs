using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

public class AmountPerUomStrategy : IIncentiveCalculationStrategy
{
    public bool IsApplicable(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        return rebate.Incentive == IncentiveType.AmountPerUom &&
               product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom) &&
               rebate.Amount > 0 && request.Volume > 0;
    }

    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
    {
        return rebate.Amount * request.Volume;
    }
}
