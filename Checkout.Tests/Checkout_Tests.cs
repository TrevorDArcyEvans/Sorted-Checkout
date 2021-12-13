namespace Checkout.Tests
{
  using System;
  using System.Collections.Generic;
  using FluentAssertions;
  using NUnit.Framework;

  public sealed class Checkout_Tests
  {
    [TestCase("")]
    [TestCase(null)]
    public void Add_InvalidSku_ThrowsException(string sku)
    {
      var checkout = Create();

      Action act = () => checkout.Add(sku);

      act.Should().Throw<ArgumentNullException>();
    }

    [TestCase("")]
    [TestCase(null)]
    public void Add_InvalidSku_DoesNotAddToBasket(string sku)
    {
      var checkout = Create();

      try
      {
        checkout.Add(sku);
      }
      catch
      {
      }

      checkout.Basket.SKUs.Should().BeEmpty();
    }

    [Test]
    public void Add_ValidSku_AddsToBasket()
    {
      var checkout = Create();

      checkout.Add("sku");

      checkout.Basket
        .SKUs
        .Should()
        .Contain("sku");
    }

    [Test]
    public void Add_ValidSku_UpdatesQuantity()
    {
      var checkout = Create();

      checkout.Add("sku");

      checkout.Basket.Quantity("sku")
        .Should()
        .Be(1);
    }

    [Test]
    public void Add_ValidSkuTwice_AddsToBasketOnce()
    {
      var checkout = Create();

      checkout.Add("sku");
      checkout.Add("sku");

      checkout.Basket
        .SKUs
        .Should()
        .OnlyContain(key => key == "sku");
    }

    [Test]
    public void Add_ValidSkuTwice_IncrementsQuantity()
    {
      var checkout = Create();

      checkout.Add("sku");
      checkout.Add("sku");

      checkout.Basket.Quantity("sku")
        .Should()
        .Be(2);
    }

    [Test]
    public void TotalPrice_UnknownSku_ThrowsException()
    {
      var checkout = Create();

      Action act = () =>
      {
        checkout.Add("abcd");
        _ = checkout.TotalPrice;
      };

      act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public void TotalPrice_OneItem_ReturnsExpected()
    {
      var checkout = Create();

      // 1@0.50 = 0.50
      checkout.Add("A99");

      checkout.TotalPrice
        .Should()
        .Be(0.50m);
    }

    [Test]
    public void TotalPrice_TwoItem_ReturnsExpected()
    {
      var checkout = Create();

      // 1@0.50 + 1@0.50 = 1.00
      checkout.Add("A99");
      checkout.Add("A99");

      checkout.TotalPrice
        .Should()
        .Be(1.00m);
    }

    [Test]
    public void TotalPrice_ThreeItem_ReturnsExpected()
    {
      var checkout = Create();

      // 3@1.30 = 1.30
      checkout.Add("A99");
      checkout.Add("A99");
      checkout.Add("A99");

      checkout.TotalPrice
        .Should()
        .Be(1.30m);
    }

    [Test]
    public void TotalPrice_MixedItem_ReturnsExpected()
    {
      var checkout = Create();

      // 1@0.50 + 1@0.50 = 1.00
      checkout.Add("A99");
      checkout.Add("A99");

      // 2@0.45 = 0.45
      checkout.Add("B15");
      checkout.Add("B15");

      // 1@0.60 + 1@0.60 = 1.20
      checkout.Add("C40");
      checkout.Add("C40");

      // 1.00 + 0.45 + 1.20 = 2.65 
      checkout.TotalPrice
        .Should()
        .Be(2.65m);
    }

    [Test]
    public void TotalPrice_OptimumSplit_ReturnsExpected()
    {
      var checkout = Create();

      // 6@2.00 + 3@1.30 + 1@0.50 = 3.80
      checkout.Add("A99");
      checkout.Add("A99");
      checkout.Add("A99");
      checkout.Add("A99");
      checkout.Add("A99");
      checkout.Add("A99");
      checkout.Add("A99");
      checkout.Add("A99");
      checkout.Add("A99");
      checkout.Add("A99");

      // 1@0.30 + 2@0.45 = 0.75
      checkout.Add("B15");
      checkout.Add("B15");
      checkout.Add("B15");

      // 3.80 + 0.75 = 4.55
      checkout.TotalPrice
        .Should()
        .Be(4.55m);
    }

    private Checkout Create()
    {
      var skuItemPricing = new ItemPricing(new Dictionary<int, decimal>
      {
        { 1, 9.99m }
      });

      var a99ItemPricing = new ItemPricing(new Dictionary<int, decimal>
      {
        { 1, 0.50m },
        { 3, 1.30m },
        { 6, 2.00m },
      });

      var b15 = new ItemPricing(new Dictionary<int, decimal>
      {
        { 1, 0.30m },
        { 2, 0.45m }
      });

      var c40ItemPricing = new ItemPricing(new Dictionary<int, decimal>
      {
        { 1, 0.60m }
      });

      var pricing = new Pricing(new Dictionary<string, IItemPricing>
      {
        { "sku", skuItemPricing },
        { "A99", a99ItemPricing },
        { "B15", b15 },
        { "C40", c40ItemPricing }
      });

      return new(new PriceCalculator(pricing));
    }
  }
}
