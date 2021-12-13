namespace Checkout
{
  using System;
  using System.Collections.Generic;

  public sealed class Basket : IBasket
  {
    // [SKU] --> [qty]
    private readonly Dictionary<string, int> _basket = new();

    public IEnumerable<string> SKUs => _basket.Keys;

    public int Quantity(string sku) => _basket[sku];

    public void Add(string sku)
    {
      if (string.IsNullOrWhiteSpace(sku))
      {
        throw new ArgumentNullException("SKU cannot be blank");
      }

      if (!_basket.ContainsKey(sku))
      {
        _basket[sku] = 0;
      }

      _basket[sku]++;
    }
  }
}
