namespace ShoppingBasket.Basket;

/// <summary>
/// One product on a receipt: how many were scanned, how many are charged, and the line total.
/// </summary>
public sealed class BasketLine
{
    public BasketLine(
        string productName,
        int quantity,
        int unitPriceInPence,
        string offerDescription,
        int chargeableUnits,
        int lineTotalInPence)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productName);
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);
        ArgumentOutOfRangeException.ThrowIfNegative(unitPriceInPence);
        ArgumentException.ThrowIfNullOrWhiteSpace(offerDescription);
        ArgumentOutOfRangeException.ThrowIfNegative(chargeableUnits);

        if (chargeableUnits > quantity)
        {
            throw new ArgumentOutOfRangeException(
                nameof(chargeableUnits),
                "An offer cannot charge for more units than were bought.");
        }

        int expectedTotal = checked(chargeableUnits * unitPriceInPence);
        if (lineTotalInPence != expectedTotal)
        {
            throw new ArgumentException(
                "Line total does not match the chargeable units and unit price.",
                nameof(lineTotalInPence));
        }

        ProductName = productName;
        Quantity = quantity;
        UnitPriceInPence = unitPriceInPence;
        OfferDescription = offerDescription;
        ChargeableUnits = chargeableUnits;
        LineTotalInPence = lineTotalInPence;
    }

    public string ProductName { get; }

    public int Quantity { get; }

    public int UnitPriceInPence { get; }

    public string OfferDescription { get; }

    public int ChargeableUnits { get; }

    public int LineTotalInPence { get; }
}
