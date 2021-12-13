namespace Checkout
{
  using System.Collections.Generic;

  public interface IPricing
  {
    IEnumerable<string> SKUs { get; }
    IItemPricing ItemPricing(string sku);
  }
}
