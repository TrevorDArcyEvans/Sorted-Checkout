namespace Checkout
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public sealed class PriceCalculator : IPriceCalculator
  {
    // [SKU] --> [qty, total-price]
    private readonly IDictionary<string, IDictionary<int, decimal>> _pricing;

    public PriceCalculator(IDictionary<string, IDictionary<int, decimal>> pricing)
    {
      _pricing = pricing;
    }

    public decimal TotalPrice(IBasket basket)
    {
      if (basket.SKUs.Any(sku => !_pricing.ContainsKey(sku)))
      {
        throw new ArgumentOutOfRangeException("Unknown SKU");
      }

      return basket.SKUs.Sum(sku => LineItemPrice(sku, basket.Quantity(sku)));
    }

    private decimal LineItemPrice(string sku, int qty)
    {
      var itemPrices = _pricing[sku];

      if (itemPrices.ContainsKey(qty))
      {
        // found an exact match for this qty
        return itemPrices[qty];
      }

      // get closest exact match
      var closestQty = itemPrices.Keys
        .OrderByDescending(x => x)
        .First(x => x < qty);

      // work out how many items left to price from exact match qty
      var diffQty = qty - closestQty;

      // recursively calculate price for remaining items
      // until there are no more items left to price
      // NOTE:  assumes that at least a single item can be bought
      //        ie we have pricing for a single item
      var price = itemPrices[closestQty] + LineItemPrice(sku, diffQty);

      return price;
    }
  }
}
