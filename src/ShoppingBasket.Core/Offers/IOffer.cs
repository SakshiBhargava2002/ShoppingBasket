namespace ShoppingBasket.Offers;

/// <summary>
/// Decides how many units of one product are charged for a purchased quantity.
/// The unit price is applied by the basket pricer after this decision.
/// </summary>
public interface IOffer
{
    /// <summary>Customer-facing offer name, for example "buy one get one free".</summary>
    string Description { get; }

    /// <summary>
    /// Returns the number of units that must be paid for.
    /// </summary>
    /// <param name="quantity">Units of this product in the basket. Zero or greater.</param>
    int ChargeableUnits(int quantity);
}
