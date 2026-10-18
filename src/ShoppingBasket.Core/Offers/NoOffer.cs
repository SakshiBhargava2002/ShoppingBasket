namespace ShoppingBasket.Offers;

/// <summary>Charges every unit at the full unit price.</summary>
public sealed class NoOffer : IOffer
{
    public static NoOffer Instance { get; } = new();

    private NoOffer()
    {
    }

    public string Description => "full price";

    public int ChargeableUnits(int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);
        return quantity;
    }
}
