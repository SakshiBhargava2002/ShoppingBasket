namespace ShoppingBasket.Offers;

/// <summary>
/// Buy one, get one free. Items are paired, and one item in each pair is free.
/// A leftover single item is charged. Chargeable units = ceil(quantity / 2).
/// </summary>
public sealed class BuyOneGetOneFreeOffer : IOffer
{
    public static BuyOneGetOneFreeOffer Instance { get; } = new();

    private BuyOneGetOneFreeOffer()
    {
    }

    public string Description => "buy one get one free";

    public int ChargeableUnits(int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);

        checked
        {
            return (quantity + 1) / 2;
        }
    }
}
