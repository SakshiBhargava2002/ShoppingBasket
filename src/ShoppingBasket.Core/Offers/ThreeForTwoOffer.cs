namespace ShoppingBasket.Offers;

/// <summary>
/// Three for the price of two. Each complete group of three is charged as two.
/// One or two leftover items are charged in full.
/// Chargeable units = (quantity / 3) * 2 + (quantity % 3).
/// </summary>
public sealed class ThreeForTwoOffer : IOffer
{
    public static ThreeForTwoOffer Instance { get; } = new();

    private ThreeForTwoOffer()
    {
    }

    public string Description => "three for the price of two";

    public int ChargeableUnits(int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);

        int groupsOfThree = quantity / 3;
        int remainder = quantity % 3;
        return groupsOfThree * 2 + remainder;
    }
}
