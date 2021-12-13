namespace Checkout
{
  using System.Collections.Generic;

  public sealed class ItemPricing : IItemPricing
  {
    // [qty] --> [total-price]
    private readonly IDictionary<int, decimal> _itemPricing;

    public ItemPricing(IDictionary<int, decimal> itemPricing)
    {
      _itemPricing = itemPricing;
    }

    public IEnumerable<int> Quantities => _itemPricing.Keys;

    public decimal TotalPrice(int qty) => _itemPricing[qty];
  }
}
