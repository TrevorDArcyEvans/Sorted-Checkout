namespace Checkout
{
  using System.Collections.Generic;

  public interface IBasket
  {
    IEnumerable<string> SKUs { get; }
    int Quantity(string sku);
  }
}
