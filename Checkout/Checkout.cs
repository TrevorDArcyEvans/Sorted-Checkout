using System.Linq;

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
      IDictionary<string, IDictionary<int,decimal>> pricing)
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
        var totalPrice = _basket.Keys.Sum(LineItemPrice);

        return totalPrice;
      }
    }

    private decimal LineItemPrice(string sku)
    {
      var qty = _basket[sku];
      var itemPrices = _pricing[sku];

      if (itemPrices.ContainsKey(qty))
      {
        return itemPrices[qty];
      }

      var closestQty = itemPrices.Keys
        .OrderBy(x => x)
        .First(x => x < qty);
      var diffQty = qty - closestQty;

      var price = itemPrices[closestQty] + diffQty * itemPrices[1];

      return price;
    }
  }
}
