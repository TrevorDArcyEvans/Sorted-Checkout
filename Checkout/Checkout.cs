namespace Checkout
{
  using System;
  using System.Collections.Generic;

  public sealed class Checkout
  {
    // [SKU] --> [qty]
    private readonly Dictionary<string, int> _basket = new();

    // [SKU] --> [qty, total-price]
    private readonly IDictionary<string, IDictionary<int, decimal>> _pricing;

    public Checkout(
      IDictionary<string, IDictionary<int, decimal>> pricing)
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
        return 0;
      }
    }
  }
}
