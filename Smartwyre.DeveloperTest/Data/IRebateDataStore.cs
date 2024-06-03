namespace Smartwyre.DeveloperTest.Types
{
    public interface IRebateDataStore
    {
        Rebate GetRebate(string rebateIdentifier);
        void StoreCalculationResult(Rebate rebate, decimal rebateAmount);
        void AddRebate(string rebateIdentifier, Rebate rebate);
    }
}
