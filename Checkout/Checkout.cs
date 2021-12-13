namespace Checkout
{
  using System;
  using System.Collections.Generic;

  public sealed class Checkout
  {
    // [SKU] --> [qty]
    private readonly Dictionary<string, int> _basket = new();

    // [SKU] --> [qty, total-price]
    private readonly PriceCalculator _priceCalculator;

    public Checkout(
      PriceCalculator priceCalculator)
    {
      _priceCalculator = priceCalculator;
    }

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

    public decimal TotalPrice => _priceCalculator.TotalPrice(_basket);
  }
}
