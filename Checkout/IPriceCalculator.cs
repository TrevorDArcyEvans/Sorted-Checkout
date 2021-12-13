namespace Checkout
{
  public interface IPriceCalculator
  {
    decimal TotalPrice(IBasket basket);
  }
}
