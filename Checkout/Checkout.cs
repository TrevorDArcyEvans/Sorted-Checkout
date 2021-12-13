using System.Linq;

namespace Checkout
{
  using System;
  using System.Collections.Generic;

  public sealed class Checkout
  {
    // [SKU] --> [qty]
    private readonly Dictionary<string, int> _basket = new();

    // [SKU] --> [qty, unit-price]
    private readonly IDictionary<string, decimal> _pricing;

    public Checkout(
      IDictionary<string, decimal> pricing)
    {
      _pricing = pricing;
    }

    public IReadOnlyDictionary<string, int> Basket => _basket;

    public void Add(string sku)
    {
      if (string.IsNullOrWhiteSpace(sku))
      {
        throw new ArgumentNullException("SKU cannot be blank");
      }
      
      if (!_pricing.ContainsKey(sku))
      {
        throw new ArgumentOutOfRangeException("Unknown SKU");
      }

      if (!_basket.ContainsKey(sku))
      {
        _basket[sku] = 0;
      }

      _basket[sku]++;
    }

    public decimal TotalPrice
    {
      get
      {
        var totalPrice = _basket.Keys.Sum(key => _basket[key] * _pricing[key]);

        return totalPrice;
      }
    }
  }
}
