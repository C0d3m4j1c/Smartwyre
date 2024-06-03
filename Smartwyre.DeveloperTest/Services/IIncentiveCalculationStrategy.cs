using System;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{
	public interface IIncentiveCalculationStrategy
	{
        bool IsApplicable(Rebate rebate, Product product, CalculateRebateRequest request);
        decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request);
    }
}

