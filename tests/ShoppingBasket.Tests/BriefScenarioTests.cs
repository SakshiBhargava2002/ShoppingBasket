using ShoppingBasket.Basket;

namespace ShoppingBasket.Tests;

/// <summary>
/// The scenario table from review: each row is one basket and its expected total in pence.
/// </summary>
public class BriefScenarioTests
{
    private static readonly BasketPricer Pricer = BasketPricer.ForSupermarket();

    public static IEnumerable<object[]> PricedBaskets()
    {
        yield return Row("Empty basket", [], 0);
        yield return Row("One apple", ["Apple"], 35);
        yield return Row("One banana", ["Banana"], 20);
        yield return Row("One melon", ["Melon"], 50);
        yield return Row("One lime", ["Lime"], 15);
        yield return Row("Multiple regular items", ["Apple", "Banana"], 55);
        yield return Row("Duplicate apples", ["Apple", "Apple"], 70);
        yield return Row("Two melons — offer applied", ["Melon", "Melon"], 50);
        yield return Row("Three melons", ["Melon", "Melon", "Melon"], 100);
        yield return Row("Four melons", ["Melon", "Melon", "Melon", "Melon"], 100);
        yield return Row("Two limes — offer not applied", ["Lime", "Lime"], 30);
        yield return Row("Three limes — offer applied", ["Lime", "Lime", "Lime"], 30);
        yield return Row("Four limes", ["Lime", "Lime", "Lime", "Lime"], 45);
        yield return Row("Six limes", ["Lime", "Lime", "Lime", "Lime", "Lime", "Lime"], 60);
        yield return Row(
            "Mixed basket",
            ["Apple", "Banana", "Melon", "Melon", "Lime", "Lime", "Lime"],
            135);
        yield return Row("Case-insensitive names", ["apple", "BANANA"], 55);
    }

    [Theory]
    [MemberData(nameof(PricedBaskets))]
    public void Basket_costs_the_expected_total(string scenario, string[] basket, int expectedPence)
    {
        int total = Pricer.CalculateTotalInPence(basket);
        Assert.True(total == expectedPence, $"{scenario}: expected {expectedPence}p but was {total}p.");
    }

    [Fact]
    public void Unknown_item_is_rejected()
    {
        UnknownProductException ex = Assert.Throws<UnknownProductException>(
            () => Pricer.CalculateTotalInPence(["Orange"]));

        Assert.Equal("Orange", ex.ProductName);
    }

    [Fact]
    public void Null_basket_is_rejected()
    {
        Assert.Throws<ArgumentNullException>(() => Pricer.CalculateTotalInPence(null!));
    }

    private static object[] Row(string scenario, string[] basket, int expectedPence)
        => [scenario, basket, expectedPence];
}
