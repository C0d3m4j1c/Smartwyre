using System.Collections.Generic;
using Smartwyre.DeveloperTest.Types;

public class ProductDataStore : IProductDataStore
{
    private readonly Dictionary<string, Product> _products = new Dictionary<string, Product>();

    public void AddProduct(string productIdentifier, Product product)
    {
        _products[productIdentifier] = product;
    }

    public Product GetProduct(string productIdentifier)
    {
        _products.TryGetValue(productIdentifier, out var product);
        return product;
    }
}
