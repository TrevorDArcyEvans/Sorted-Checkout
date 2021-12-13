using System.Collections.Generic;

namespace Checkout.Tests
{
  using System;
  using FluentAssertions;
  using NUnit.Framework;

  public sealed class Checkout_Tests
  {
    [SetUp]
    public void Setup()
    {
      _pricing.Clear();
      _pricing.Add("sku", new Dictionary<int, decimal>
      {
        { 1, 9.99m }
      });

      _pricing.Add("A99", new Dictionary<int, decimal>
      {
        { 1, 0.50m },
        { 3, 1.30m }
      });
      _pricing.Add("B15", new Dictionary<int, decimal>
      {
        { 1, 0.30m },
        { 2, 0.45m }
      });
      _pricing.Add("C40", new Dictionary<int, decimal>
      {
        { 1, 0.60m }
      });
    }

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
      catch (Exception)
      {
      }

      checkout.Basket.Should().BeEmpty();
    }

    [Test]
    public void Add_ValidSku_AddsToBasket()
    {
      var checkout = Create();

      checkout.Add("sku");

      checkout.Basket
        .Keys
        .Should()
        .Contain("sku");
    }

    [Test]
    public void Add_ValidSku_UpdatesQuantity()
    {
      var checkout = Create();

      checkout.Add("sku");

      checkout.Basket["sku"]
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
        .Keys
        .Should()
        .OnlyContain(key => key == "sku");
    }

    [Test]
    public void Add_ValidSkuTwice_IncrementsQuantity()
    {
      var checkout = Create();

      checkout.Add("sku");
      checkout.Add("sku");

      checkout.Basket["sku"]
        .Should()
        .Be(2);
    }

    [Test]
    public void Add_UnknownSku_ThrowsException()
    {
      var checkout = Create();

      Action act = () => checkout.Add("abcd");

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

      // 3@1.30 + 3@1.30 + 1@0.50 = 3.10
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

      // 3.10 + 0.75 = 3.85
      checkout.TotalPrice
        .Should()
        .Be(3.85m);
    }

    private Checkout Create() => new(_pricing);

    private IDictionary<string, IDictionary<int, decimal>> _pricing = new Dictionary<string, IDictionary<int, decimal>>();
  }
}
