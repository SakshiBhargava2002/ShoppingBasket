using ShoppingBasket.Catalogue;

namespace ShoppingBasket.Basket;

/// <summary>
/// Prices a basket of product names. The same instance can be reused; it keeps no basket state.
/// </summary>
public sealed class BasketPricer
{
    private readonly PriceCatalogue _catalogue;

    public BasketPricer(PriceCatalogue catalogue)
    {
        ArgumentNullException.ThrowIfNull(catalogue);
        _catalogue = catalogue;
    }

    public static BasketPricer ForSupermarket() => new(PriceCatalogue.Supermarket());

    /// <summary>
    /// Returns the total cost, in pence, of the named items.
    /// </summary>
    public int CalculateTotalInPence(IEnumerable<string> items) => Price(items).TotalInPence;

    /// <summary>
    /// Prices the basket and returns a receipt. Offers apply to the total quantity of each product.
    /// </summary>
    public BasketReceipt Price(IEnumerable<string> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        var counts = new Dictionary<string, int>(StringComparer.Ordinal);

        foreach (string raw in items)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                throw new ArgumentException("Basket items must have a name.", nameof(items));
            }

            PricedProduct product = _catalogue.GetRequired(raw);
            counts.TryGetValue(product.Name, out int current);
            checked
            {
                counts[product.Name] = current + 1;
            }
        }

        var lines = new List<BasketLine>(counts.Count);
        int total = 0;

        foreach (PricedProduct product in _catalogue.Products)
        {
            if (!counts.TryGetValue(product.Name, out int quantity))
            {
                continue;
            }

            int chargeableUnits = product.Offer.ChargeableUnits(quantity);
            int lineTotal;
            checked
            {
                lineTotal = chargeableUnits * product.UnitPriceInPence;
                total += lineTotal;
            }

            lines.Add(new BasketLine(
                product.Name,
                quantity,
                product.UnitPriceInPence,
                product.Offer.Description,
                chargeableUnits,
                lineTotal));
        }

        return new BasketReceipt(lines, total);
    }
}
