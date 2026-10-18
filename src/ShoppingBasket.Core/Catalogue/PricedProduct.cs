using ShoppingBasket.Offers;

namespace ShoppingBasket.Catalogue;

/// <summary>
/// A sellable product: a display name, a unit price in pence, and one offer.
/// </summary>
public sealed class PricedProduct
{
    public PricedProduct(string name, int unitPriceInPence, IOffer offer)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegative(unitPriceInPence);
        ArgumentNullException.ThrowIfNull(offer);

        Name = name.Trim();
        UnitPriceInPence = unitPriceInPence;
        Offer = offer;
    }

    public string Name { get; }

    public int UnitPriceInPence { get; }

    public IOffer Offer { get; }
}
