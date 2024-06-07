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
                if (!decimal.TryParse(priceInput, out var price))
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

                // Hey this are the types of incentives
                // Press 1 to select FixedRateRebate
                // ...

                //castFunctionToPrintMenuOfIncentives();

                //Function that will iterate all the enums to create the menu
                //foreach(value of values)
                //Console.WriteLine($"Press {value.toInt} to select {value.name})

                Console.WriteLine("These are the types of Rebate incentives, select one");
                Console.WriteLine("Press 0 if you want to FixedRateRebate");
                Console.WriteLine("Press 1 if you want to AmountPerUom");
                Console.WriteLine("Press 2 if you want to FixedCashAmount");
                var incentiveInput = Console.ReadLine();
                if (!int.TryParse(incentiveInput, out var incentiveType))
                {
                    Console.WriteLine("Invalid type. Please enter a valid {0..2} value.");
                    return;
                }

                //Hardcoding the Incentive for simplicity
                var rebate = new Rebate
                {
                    Incentive = (IncentiveType) incentiveType,
                    Percentage = percentaje,
                    Amount = amount
                };

                //Hardcoding the Incentive for simplicity  0 -- Bitwise 1 -- next 0 << 2 2 
                SupportedIncentiveType supportedIncentiveType = SupportedIncentiveType.FixedRateRebate;
                switch(incentiveType)
                {
                    case 0: supportedIncentiveType = SupportedIncentiveType.FixedRateRebate;
                        break;
                    case 1: supportedIncentiveType = SupportedIncentiveType.AmountPerUom;
                        break;
                    case 2: supportedIncentiveType = SupportedIncentiveType.FixedCashAmount;
                        break;
                }

                var product = new Product
                {
                    SupportedIncentives = supportedIncentiveType,
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
