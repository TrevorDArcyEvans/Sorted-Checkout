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
      _pricing.Add("sku", new Dictionary<int, decimal>());
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

    private Checkout Create() => new(_pricing);

    private IDictionary<string, IDictionary<int, decimal>> _pricing = new Dictionary<string, IDictionary<int, decimal>>();
  }
}
