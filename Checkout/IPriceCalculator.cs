namespace Checkout
{
  using System.Collections.Generic;

  public interface IPriceCalculator
  {
    decimal TotalPrice(IDictionary<string, int> basket);
  }
}
