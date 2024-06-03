namespace Smartwyre.DeveloperTest.Types
{
    public interface IProductDataStore
    {
        Product GetProduct(string productIdentifier);
        void AddProduct(string productIdentifier, Product product);
    }
}
