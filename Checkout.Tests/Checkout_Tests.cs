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

    [Test]
    public void Add_ValidSku_AddsToBasket()
    {
      var checkout = new Checkout();

      checkout.Add("sku");

      checkout.Basket
        .Keys
        .Should()
        .Contain("sku");
    }

    [Test]
    public void Add_ValidSku_UpdatesQuantity()
    {
      var checkout = new Checkout();

      checkout.Add("sku");

      checkout.Basket["sku"]
        .Should()
        .Be(1);
    }

    [Test]
    public void Add_ValidSkuTwice_AddsToBasketOnce()
    {
      var checkout = new Checkout();

      checkout.Add("sku");
      checkout.Add("sku");

      checkout.Basket
        .Keys
        .Should()
        .OnlyContain(key => key == "sku");
    }

    [Test]
    public void Add_ValidSkuTwice_IncrementsQuantity()
    {
      var checkout = new Checkout();

      checkout.Add("sku");
      checkout.Add("sku");

      checkout.Basket["sku"]
        .Should()
        .Be(2);
    }
  }
}
