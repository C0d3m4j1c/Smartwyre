using Moq;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests
{
    public class RebateServiceTests
    {
        private Mock<IRebateDataStore> rebateDataStoreMock;
        private Mock<IProductDataStore> productDataStoreMock;
        private RebateService rebateService;
        private CalculateRebateRequest request;
        private Rebate rebate;
        private Product product;

        public RebateServiceTests()
        {
            // Initialize common objects
            rebateDataStoreMock = new Mock<IRebateDataStore>();
            productDataStoreMock = new Mock<IProductDataStore>();

            request = new CalculateRebateRequest
            {
                RebateIdentifier = "Rebate1",
                ProductIdentifier = "Product1",
                Volume = 10
            };

            rebate = new Rebate();
            product = new Product();

            // Default setup
            rebateDataStoreMock.Setup(x => x.GetRebate("Rebate1")).Returns(rebate);
            productDataStoreMock.Setup(x => x.GetProduct("Product1")).Returns(product);
        }

        [Fact]
        public void Calculate_ShouldReturnSuccess_ForFixedCashAmount()
        {
            rebate.Incentive = IncentiveType.FixedCashAmount;
            rebate.Amount = 10;
            product.SupportedIncentives = SupportedIncentiveType.FixedCashAmount;

            rebateService = new RebateService(
                rebateDataStoreMock.Object,
                productDataStoreMock.Object,
                new List<IIncentiveCalculationStrategy> { new FixedCashAmountStrategy() });

            var result = rebateService.Calculate(request);

            Assert.True(result.Success);
            // Validate that StoreCalculationResult Was called and it's return value (rebate.Amount)
            rebateDataStoreMock.Verify(x => x.StoreCalculationResult(rebate, 10), Times.Once);
        }

        [Fact]
        public void Calculate_ShouldReturnSuccess_ForFixedRateRebateStrategy()
        {
            rebate.Incentive = IncentiveType.FixedRateRebate;
            rebate.Amount = 10;
            rebate.Percentage = 0.1M;
            product.SupportedIncentives = SupportedIncentiveType.FixedRateRebate;
            product.Price = 10;

            rebateService = new RebateService(
                rebateDataStoreMock.Object,
                productDataStoreMock.Object,
                new List<IIncentiveCalculationStrategy> { new FixedRateRebateStrategy() });

            var result = rebateService.Calculate(request);

            Assert.True(result.Success);
            // Validate that StoreCalculationResult Was called and it's return value (product.Price * rebate.Percentage * request.Volume)
            rebateDataStoreMock.Verify(x => x.StoreCalculationResult(rebate, 10), Times.Once);
        }

        [Fact]
        public void Calculate_ShouldReturnSuccess_ForAmountPerUomStrategy()
        {
            rebate.Incentive = IncentiveType.AmountPerUom;
            rebate.Amount = 10;
            rebate.Percentage = 0.1M;
            product.SupportedIncentives = SupportedIncentiveType.AmountPerUom;
            product.Price = 10;

            rebateService = new RebateService(
                rebateDataStoreMock.Object,
                productDataStoreMock.Object,
                new List<IIncentiveCalculationStrategy> { new AmountPerUomStrategy() });

            var result = rebateService.Calculate(request);

            Assert.True(result.Success);
            // Validate that StoreCalculationResult Was called and it's return value (product.Amount * rebate.Volume)
            rebateDataStoreMock.Verify(x => x.StoreCalculationResult(rebate, 100), Times.Once);
        }

        //TODO create the negative or False Asserts of each test and code paths
        [Fact]
        public void Calculate_ShouldReturnSuccessAsFalse_ForFixedCashAmount_WhenAmountIsNotBiggerThanZero()
        {
            rebate.Incentive = IncentiveType.FixedCashAmount;
            rebate.Amount = 0;
            product.SupportedIncentives = SupportedIncentiveType.FixedCashAmount;

            rebateService = new RebateService(
                rebateDataStoreMock.Object,
                productDataStoreMock.Object,
                new List<IIncentiveCalculationStrategy> { new FixedCashAmountStrategy() });

            var result = rebateService.Calculate(request);

            Assert.False(result.Success);
        }

        [Fact]
        public void Calculate_ShouldReturnSuccessAsFalse_ForFixedCashAmount_WhenSupportedIncentivesIsNotFixedCashAmount()
        {
            rebate.Incentive = IncentiveType.FixedCashAmount;
            rebate.Amount = 10;
            product.SupportedIncentives = SupportedIncentiveType.FixedRateRebate;

            rebateService = new RebateService(
                rebateDataStoreMock.Object,
                productDataStoreMock.Object,
                new List<IIncentiveCalculationStrategy> { new FixedCashAmountStrategy() });

            var result = rebateService.Calculate(request);

            Assert.False(result.Success);
        }

        [Fact]
        public void Calculate_ShouldReturnSuccessAsFalse_ForFixedCashAmount_WhenRebateIncentiveIsNotFixedCashAmount()
        {
            rebate.Incentive = IncentiveType.FixedRateRebate;
            rebate.Amount = 10;
            product.SupportedIncentives = SupportedIncentiveType.FixedCashAmount;

            rebateService = new RebateService(
                rebateDataStoreMock.Object,
                productDataStoreMock.Object,
                new List<IIncentiveCalculationStrategy> { new FixedCashAmountStrategy() });

            var result = rebateService.Calculate(request);

            Assert.False(result.Success);
        }


        [Fact]
        public void Calculate_ShouldReturnSuccessAsFalse_ForFixedRateRebate_WhenPercentageIsNotBiggerThanZero()
        {
            rebate.Incentive = IncentiveType.FixedRateRebate;
            rebate.Percentage = 0;
            product.Price = 10;
            product.SupportedIncentives = SupportedIncentiveType.FixedRateRebate;

            rebateService = new RebateService(
                rebateDataStoreMock.Object,
                productDataStoreMock.Object,
                new List<IIncentiveCalculationStrategy> { new FixedRateRebateStrategy() });

            var result = rebateService.Calculate(request);

            Assert.False(result.Success);
        }

        [Fact]
        public void Calculate_ShouldReturnSuccessAsFalse_ForFixedRateRebate_WhenPriceIsNotBiggerThanZero()
        {
            rebate.Incentive = IncentiveType.FixedRateRebate;
            rebate.Percentage = 10;
            product.Price = 0;
            product.SupportedIncentives = SupportedIncentiveType.FixedRateRebate;

            rebateService = new RebateService(
                rebateDataStoreMock.Object,
                productDataStoreMock.Object,
                new List<IIncentiveCalculationStrategy> { new FixedRateRebateStrategy() });

            var result = rebateService.Calculate(request);

            Assert.False(result.Success);
        }

        [Fact]
        public void Calculate_ShouldReturnSuccessAsFalse_ForFixedRateRebate_WhenVolumeIsNotBiggerThanZero()
        {
            rebate.Incentive = IncentiveType.FixedRateRebate;
            rebate.Percentage = 10;
            product.Price = 10;
            request.Volume = 0;
            product.SupportedIncentives = SupportedIncentiveType.FixedRateRebate;

            rebateService = new RebateService(
                rebateDataStoreMock.Object,
                productDataStoreMock.Object,
                new List<IIncentiveCalculationStrategy> { new FixedRateRebateStrategy() });

            var result = rebateService.Calculate(request);

            Assert.False(result.Success);
        }

        [Fact]
        public void Calculate_ShouldReturnSuccessAsFalse_ForFixedRateRebate_WhenSupportedIncentivesIsNotFixedRateRebate()
        {
            rebate.Incentive = IncentiveType.FixedRateRebate;
            rebate.Percentage = 10;
            product.Price = 10;
            product.SupportedIncentives = SupportedIncentiveType.FixedCashAmount;

            rebateService = new RebateService(
                rebateDataStoreMock.Object,
                productDataStoreMock.Object,
                new List<IIncentiveCalculationStrategy> { new FixedRateRebateStrategy() });

            var result = rebateService.Calculate(request);

            Assert.False(result.Success);
        }

        [Fact]
        public void Calculate_ShouldReturnSuccessAsFalse_ForFixedRateRebate_WhenRebateIncentiveIsNotFixedRateRebate()
        {
            rebate.Incentive = IncentiveType.FixedCashAmount;
            rebate.Percentage = 10;
            product.Price = 10;
            product.SupportedIncentives = SupportedIncentiveType.FixedRateRebate;

            rebateService = new RebateService(
                rebateDataStoreMock.Object,
                productDataStoreMock.Object,
                new List<IIncentiveCalculationStrategy> { new FixedRateRebateStrategy() });

            var result = rebateService.Calculate(request);

            Assert.False(result.Success);
        }

        [Fact]
        public void Calculate_ShouldReturnSuccessAsFalse_ForAmountPerUomStrategy_WhenAmountIsNotBiggerThanZero()
        {
            rebate.Incentive = IncentiveType.AmountPerUom;
            rebate.Amount = 0;
            product.SupportedIncentives = SupportedIncentiveType.AmountPerUom;

            rebateService = new RebateService(
                rebateDataStoreMock.Object,
                productDataStoreMock.Object,
                new List<IIncentiveCalculationStrategy> { new AmountPerUomStrategy() });

            var result = rebateService.Calculate(request);

            Assert.False(result.Success);
        }

        [Fact]
        public void Calculate_ShouldReturnSuccessAsFalse_ForAmountPerUomStrategy_WhenVolumeIsNotBiggerThanZero()
        {
            rebate.Incentive = IncentiveType.AmountPerUom;
            rebate.Amount = 10;
            product.SupportedIncentives = SupportedIncentiveType.AmountPerUom;
            request.Volume = 0;
            rebateService = new RebateService(
                rebateDataStoreMock.Object,
                productDataStoreMock.Object,
                new List<IIncentiveCalculationStrategy> { new AmountPerUomStrategy() });

            var result = rebateService.Calculate(request);

            Assert.False(result.Success);
        }

        [Fact]
        public void Calculate_ShouldReturnSuccessAsFalse_ForAmountPerUomStrategy_WhenSupportedIncentivesIsNotAmountPerUom()
        {
            rebate.Incentive = IncentiveType.AmountPerUom;
            rebate.Amount = 10;
            product.SupportedIncentives = SupportedIncentiveType.FixedRateRebate;
            request.Volume = 10;
            rebateService = new RebateService(
                rebateDataStoreMock.Object,
                productDataStoreMock.Object,
                new List<IIncentiveCalculationStrategy> { new AmountPerUomStrategy() });

            var result = rebateService.Calculate(request);

            Assert.False(result.Success);
        }

        [Fact]
        public void Calculate_ShouldReturnSuccessAsFalse_ForAmountPerUomStrategy_WhenRebateIncentiveIsNotAmountPerUom()
        {
            rebate.Incentive = IncentiveType.FixedRateRebate;
            rebate.Amount = 10;
            product.SupportedIncentives = SupportedIncentiveType.AmountPerUom;
            request.Volume = 10;
            rebateService = new RebateService(
                rebateDataStoreMock.Object,
                productDataStoreMock.Object,
                new List<IIncentiveCalculationStrategy> { new AmountPerUomStrategy() });

            var result = rebateService.Calculate(request);

            Assert.False(result.Success);
        }
    }
}