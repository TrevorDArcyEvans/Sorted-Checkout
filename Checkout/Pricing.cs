namespace Checkout
{
  using System.Collections.Generic;

  public sealed class Pricing : IPricing
  {
    // [SKU] --> [qty, total-price]
    private readonly IDictionary<string, IItemPricing> _pricing;

    public Pricing(IDictionary<string, IItemPricing> pricing)
    {
      _pricing = pricing;
    }

    public IEnumerable<string> SKUs => _pricing.Keys;

    public IItemPricing ItemPricing(string sku) => _pricing[sku];
  }
}
