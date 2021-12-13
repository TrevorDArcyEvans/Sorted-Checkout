namespace Checkout
{
  using System;
  using System.Linq;

  public sealed class PriceCalculator : IPriceCalculator
  {
    // [SKU] --> [qty, total-price]
    private readonly IPricing _pricing;

    public PriceCalculator(IPricing pricing)
    {
      _pricing = pricing;
    }

    public decimal TotalPrice(IBasket basket)
    {
      if (basket.SKUs.Any(sku => !_pricing.SKUs.Contains(sku)))
      {
        throw new ArgumentOutOfRangeException("Unknown SKU");
      }

      return basket.SKUs.Sum(sku => LineItemPrice(sku, basket.Quantity(sku)));
    }

    private decimal LineItemPrice(string sku, int qty)
    {
      var itemPrices = _pricing.ItemPricing(sku);

      if (itemPrices.Quantities.Contains(qty))
      {
        // found an exact match for this qty
        return itemPrices.TotalPrice(qty);
      }

      // get closest exact match
      var closestQty = itemPrices.Quantities
        .OrderByDescending(x => x)
        .First(x => x < qty);

      // work out how many items left to price from exact match qty
      var diffQty = qty - closestQty;

      // recursively calculate price for remaining items
      // until there are no more items left to price
      // NOTE:  assumes that at least a single item can be bought
      //        ie we have pricing for a single item
      var price = itemPrices.TotalPrice(closestQty) + LineItemPrice(sku, diffQty);

      return price;
    }
  }
}
