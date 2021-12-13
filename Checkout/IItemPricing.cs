namespace Checkout
{
  using System.Collections.Generic;

  public interface IItemPricing
  {
    IEnumerable<int> Quantities { get; }
    decimal TotalPrice(int qty);
  }
}
