namespace Checkout
{
  using System;
  using System.Collections.Concurrent;
  using System.Collections.Generic;

  public sealed class Checkout
  {
    // [SKU] --> [qty]
    private readonly ConcurrentDictionary<string, int> _basket = new();

    public IReadOnlyDictionary<string, int> Basket => _basket;

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
