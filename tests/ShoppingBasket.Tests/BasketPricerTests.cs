using ShoppingBasket.Basket;
using ShoppingBasket.Catalogue;
using ShoppingBasket.Offers;

namespace ShoppingBasket.Tests;

public class BasketPricerTests
{
    private static readonly BasketPricer Pricer = BasketPricer.ForSupermarket();

    [Fact]
    public void Two_apples_and_one_banana_cost_90_pence()
    {
        Assert.Equal(90, Total("Apple", "Apple", "Banana"));
    }

    [Theory]
    [InlineData(1, 35)]
    [InlineData(2, 70)]
    [InlineData(3, 105)]
    public void Apples_are_35_pence_each(int count, int expectedPence)
    {
        Assert.Equal(expectedPence, Total(Repeat("Apple", count)));
    }

    [Theory]
    [InlineData(1, 20)]
    [InlineData(2, 40)]
    [InlineData(5, 100)]
    public void Bananas_are_20_pence_each(int count, int expectedPence)
    {
        Assert.Equal(expectedPence, Total(Repeat("Banana", count)));
    }

    [Theory]
    [InlineData(1, 50)]
    [InlineData(2, 50)]
    [InlineData(3, 100)]
    [InlineData(4, 100)]
    [InlineData(5, 150)]
    [InlineData(6, 150)]
    public void Melons_are_buy_one_get_one_free(int count, int expectedPence)
    {
        Assert.Equal(expectedPence, Total(Repeat("Melon", count)));
    }

    [Theory]
    [InlineData(1, 15)]
    [InlineData(2, 30)]
    [InlineData(3, 30)]
    [InlineData(4, 45)]
    [InlineData(5, 60)]
    [InlineData(6, 60)]
    [InlineData(7, 75)]
    [InlineData(8, 90)]
    [InlineData(9, 90)]
    public void Limes_are_three_for_the_price_of_two(int count, int expectedPence)
    {
        Assert.Equal(expectedPence, Total(Repeat("Lime", count)));
    }

    [Fact]
    public void One_of_each_product_has_no_multi_buy_saving()
    {
        Assert.Equal(35 + 20 + 50 + 15, Total("Apple", "Banana", "Melon", "Lime"));
    }

    [Fact]
    public void Offers_apply_to_each_product_on_its_own()
    {
        Assert.Equal(50 + 30, Total("Melon", "Lime", "Lime"));
    }

    [Fact]
    public void A_mixed_basket_applies_both_offers()
    {
        int total = Total("Melon", "Melon", "Lime", "Lime", "Lime", "Apple", "Banana");

        Assert.Equal(50 + 30 + 35 + 20, total);
    }

    [Fact]
    public void Item_order_does_not_change_the_total()
    {
        string[] first = ["Lime", "Apple", "Melon", "Lime", "Banana", "Lime", "Melon"];
        string[] second = ["Apple", "Banana", "Melon", "Melon", "Lime", "Lime", "Lime"];

        Assert.Equal(Total(first), Total(second));
        Assert.Equal(35 + 20 + 50 + 30, Total(first));
    }

    [Fact]
    public void Names_match_ignoring_case_and_surrounding_spaces()
    {
        Assert.Equal(35 + 20 + 50, Total(" apple ", "BANANA", "melon"));
    }

    [Fact]
    public void Repeated_scans_of_the_same_name_are_counted_together()
    {
        Assert.Equal(50, Total("Melon", "melon"));
        Assert.Equal(30, Total("Lime", "LIME", " lime "));
    }

    [Fact]
    public void Empty_basket_costs_nothing()
    {
        BasketReceipt receipt = Pricer.Price(Array.Empty<string>());

        Assert.Equal(0, receipt.TotalInPence);
        Assert.Empty(receipt.Lines);
        Assert.Equal("£0.00", receipt.TotalFormatted);
    }

    [Fact]
    public void Receipt_lists_scanned_products_in_catalogue_order()
    {
        BasketReceipt receipt = Pricer.Price(["Lime", "Lime", "Lime", "Melon", "Apple", "Melon"]);

        Assert.Equal(115, receipt.TotalInPence);
        Assert.Equal("£1.15", receipt.TotalFormatted);
        Assert.Equal(3, receipt.Lines.Count);

        BasketLine apple = receipt.Lines[0];
        Assert.Equal("Apple", apple.ProductName);
        Assert.Equal(1, apple.Quantity);
        Assert.Equal(1, apple.ChargeableUnits);
        Assert.Equal(35, apple.LineTotalInPence);
        Assert.Equal("full price", apple.OfferDescription);

        BasketLine melon = receipt.Lines[1];
        Assert.Equal("Melon", melon.ProductName);
        Assert.Equal(2, melon.Quantity);
        Assert.Equal(1, melon.ChargeableUnits);
        Assert.Equal(50, melon.LineTotalInPence);
        Assert.Equal("buy one get one free", melon.OfferDescription);

        BasketLine lime = receipt.Lines[2];
        Assert.Equal("Lime", lime.ProductName);
        Assert.Equal(3, lime.Quantity);
        Assert.Equal(2, lime.ChargeableUnits);
        Assert.Equal(30, lime.LineTotalInPence);
        Assert.Equal("three for the price of two", lime.OfferDescription);
    }

    [Fact]
    public void The_same_pricer_can_price_another_basket()
    {
        Assert.Equal(35, Total("Apple"));
        Assert.Equal(20, Total("Banana"));
    }

    [Fact]
    public void A_null_basket_is_rejected()
    {
        Assert.Throws<ArgumentNullException>(() => Pricer.CalculateTotalInPence(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void A_blank_item_name_is_rejected(string name)
    {
        Assert.Throws<ArgumentException>(() => Total("Apple", name));
    }

    [Fact]
    public void An_unknown_product_is_rejected()
    {
        UnknownProductException ex = Assert.Throws<UnknownProductException>(() => Total("Apple", "Pear"));

        Assert.Equal("Pear", ex.ProductName);
    }

    [Fact]
    public void A_new_product_can_be_priced_without_changing_the_pricer()
    {
        var catalogue = new PriceCatalogue(
        [
            new PricedProduct("Cherry", 10, ThreeForTwoOffer.Instance),
        ]);
        var pricer = new BasketPricer(catalogue);

        Assert.Equal(20, pricer.CalculateTotalInPence(["Cherry", "Cherry", "Cherry"]));
    }

    [Fact]
    public void A_total_that_does_not_fit_in_pence_is_rejected()
    {
        var catalogue = new PriceCatalogue(
        [
            new PricedProduct("Gold", int.MaxValue, NoOffer.Instance),
        ]);
        var pricer = new BasketPricer(catalogue);

        Assert.Throws<OverflowException>(() => pricer.CalculateTotalInPence(["Gold", "Gold"]));
    }

    [Fact]
    public void The_pricer_requires_a_catalogue()
    {
        Assert.Throws<ArgumentNullException>(() => new BasketPricer(null!));
    }

    private static int Total(params string[] items) => Pricer.CalculateTotalInPence(items);

    private static string[] Repeat(string name, int count)
    {
        var items = new string[count];
        Array.Fill(items, name);
        return items;
    }
}
