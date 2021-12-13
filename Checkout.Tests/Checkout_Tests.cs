namespace Checkout.Tests
{
  using System;
  using FluentAssertions;
  using NUnit.Framework;

  public sealed class Checkout_Tests
  {
    [TestCase("")]
    [TestCase(null)]
    public void Add_InvalidSku_ThrowsException(string sku)
    {
      var checkout = new Checkout();

      Action act = () => checkout.Add(sku);

      act.Should().Throw<ArgumentNullException>();
    }

    [TestCase("")]
    [TestCase(null)]
    public void Add_InvalidSku_DoesNotAddToBasket(string sku)
    {
      var checkout = new Checkout();

      try
      {
        checkout.Add(sku);
      }
      catch (Exception)
      {
      }

      checkout.Basket.Should().BeEmpty();
    }
  }
}
