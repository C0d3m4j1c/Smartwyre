using System;
using System.Collections.Generic;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Enter Rebate Identifier: ");
                var rebateIdentifier = Console.ReadLine();

                Console.Write("Enter Rebate Amount: ");
                var amountInput = Console.ReadLine();
                if (!decimal.TryParse(amountInput, out var amount))
                {
                    Console.WriteLine("Invalid amount. Please enter a valid decimal.");
                    return;
                }

                Console.Write("Enter Rebate Percentaje: ");
                var percentajeInput = Console.ReadLine();
                if (!decimal.TryParse(percentajeInput, out var percentaje))
                {
                    Console.WriteLine("Invalid amount. Please enter a valid decimal.");
                    return;
                }

                Console.Write("Enter Product Identifier: ");
                var productIdentifier = Console.ReadLine();

                Console.Write("Enter Product Price: ");
                var priceInput = Console.ReadLine();
                if (!decimal.TryParse(percentajeInput, out var price))
                {
                    Console.WriteLine("Invalid amount. Please enter a valid decimal.");
                    return;
                }

                Console.Write("Enter Request Volume: ");
                var volumeInput = Console.ReadLine();
                if (!int.TryParse(volumeInput, out var volume))
                {
                    Console.WriteLine("Invalid volume. Please enter a valid integer.");
                    return;
                }

                // Initialize data stores and strategies
                var rebateDataStore = new RebateDataStore();
                var productDataStore = new ProductDataStore();
                var calculationStrategies = new List<IIncentiveCalculationStrategy>
                {
                    new FixedCashAmountStrategy(),
                    new FixedRateRebateStrategy(),
                    new AmountPerUomStrategy()
                };


                //Hardcoding the Incentive for simplicity
                var rebate = new Rebate
                {
                    Incentive = IncentiveType.FixedRateRebate,
                    Percentage = percentaje,
                    Amount = amount
                };

                //Hardcoding the Incentive for simplicity
                var product = new Product
                {
                    SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
                    Price = price 
                };

                productDataStore.AddProduct(productIdentifier, product);

                rebateDataStore.AddRebate(rebateIdentifier, rebate);

                var rebateService = new RebateService(rebateDataStore, productDataStore, calculationStrategies);

                var request = new CalculateRebateRequest
                {
                    RebateIdentifier = rebateIdentifier,
                    ProductIdentifier = productIdentifier,
                    Volume = volume
                };

                var result = rebateService.Calculate(request);

                Console.WriteLine($"Calculation Success: {result.Success}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
