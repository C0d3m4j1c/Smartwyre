using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;
    private readonly IEnumerable<IIncentiveCalculationStrategy> _calculationStrategies;

    public RebateService(IRebateDataStore rebateDataStore, IProductDataStore productDataStore, IEnumerable<IIncentiveCalculationStrategy> calculationStrategies)
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
        _calculationStrategies = calculationStrategies;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        var rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        var product = _productDataStore.GetProduct(request.ProductIdentifier);

        var result = new CalculateRebateResult();

        foreach (var strategy in _calculationStrategies)
        {
            if (!strategy.IsApplicable(rebate, product, request))
            {
                continue;
            }
            var rebateAmount = strategy.Calculate(rebate, product, request);
            _rebateDataStore.StoreCalculationResult(rebate, rebateAmount);
            result.Success = true;
            return result;
        }

        result.Success = false;
        return result;
    }
}
