using ShoppingBasket.Catalogue;
using ShoppingBasket.Offers;

namespace ShoppingBasket.Tests;

public class PriceCatalogueTests
{
    [Fact]
    public void Supermarket_catalogue_uses_the_prices_and_offers_from_the_brief()
    {
        PriceCatalogue catalogue = PriceCatalogue.Supermarket();

        Assert.Equal(35, catalogue.GetRequired("Apple").UnitPriceInPence);
        Assert.IsType<NoOffer>(catalogue.GetRequired("Apple").Offer);

        Assert.Equal(20, catalogue.GetRequired("Banana").UnitPriceInPence);
        Assert.IsType<NoOffer>(catalogue.GetRequired("Banana").Offer);

        Assert.Equal(50, catalogue.GetRequired("Melon").UnitPriceInPence);
        Assert.IsType<BuyOneGetOneFreeOffer>(catalogue.GetRequired("Melon").Offer);

        Assert.Equal(15, catalogue.GetRequired("Lime").UnitPriceInPence);
        Assert.IsType<ThreeForTwoOffer>(catalogue.GetRequired("Lime").Offer);
    }

    [Fact]
    public void Lookup_trims_the_name_and_ignores_case()
    {
        PricedProduct apple = PriceCatalogue.Supermarket().GetRequired("  aPpLe  ");

        Assert.Equal("Apple", apple.Name);
    }

    [Fact]
    public void Duplicate_product_names_are_rejected()
    {
        ArgumentException ex = Assert.Throws<ArgumentException>(() => new PriceCatalogue(
        [
            new PricedProduct("Apple", 35, NoOffer.Instance),
            new PricedProduct("apple", 40, NoOffer.Instance),
        ]));

        Assert.Contains("Apple", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_product_requires_a_name()
    {
        Assert.Throws<ArgumentNullException>(() => new PricedProduct(null!, 35, NoOffer.Instance));
        Assert.Throws<ArgumentException>(() => new PricedProduct("", 35, NoOffer.Instance));
        Assert.Throws<ArgumentException>(() => new PricedProduct("   ", 35, NoOffer.Instance));
    }

    [Fact]
    public void A_product_rejects_a_negative_price_and_a_missing_offer()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new PricedProduct("Apple", -1, NoOffer.Instance));
        Assert.Throws<ArgumentNullException>(() => new PricedProduct("Apple", 35, null!));
    }

    [Fact]
    public void A_catalogue_rejects_a_null_product()
    {
        Assert.Throws<ArgumentNullException>(() => new PriceCatalogue(new PricedProduct[] { null! }));
    }
}
