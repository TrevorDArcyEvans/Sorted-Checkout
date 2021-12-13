namespace Checkout
{
  public sealed class Checkout
  {
    // [SKU] --> [qty]
    private readonly Basket _basket = new();

    // [SKU] --> [qty, total-price]
    private readonly IPriceCalculator _priceCalculator;

    public Checkout(
      IPriceCalculator priceCalculator)
    {
      _priceCalculator = priceCalculator;
    }

    public IBasket Basket => _basket;

    public void Add(string sku) => _basket.Add(sku);

    public decimal TotalPrice => _priceCalculator.TotalPrice(_basket);
  }
}
