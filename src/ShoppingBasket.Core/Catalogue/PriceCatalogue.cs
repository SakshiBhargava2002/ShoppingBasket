using ShoppingBasket.Offers;

namespace ShoppingBasket.Catalogue;

/// <summary>
/// The products that can be priced, looked up by name without regard to case.
/// </summary>
public sealed class PriceCatalogue
{
    private readonly Dictionary<string, PricedProduct> _byName;

    public PriceCatalogue(IEnumerable<PricedProduct> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        var ordered = new List<PricedProduct>();
        _byName = new Dictionary<string, PricedProduct>(StringComparer.OrdinalIgnoreCase);

        foreach (PricedProduct product in products)
        {
            ArgumentNullException.ThrowIfNull(product);

            if (_byName.TryGetValue(product.Name, out PricedProduct? existing))
            {
                throw new ArgumentException(
                    $"A product named '{existing.Name}' is already in the catalogue.",
                    nameof(products));
            }

            _byName.Add(product.Name, product);

            ordered.Add(product);
        }

        Products = ordered;
    }

    /// <summary>
    /// Apples, bananas, melons, and limes at the prices in the brief.
    /// </summary>
    public static PriceCatalogue Supermarket()
    {
        return new PriceCatalogue(
        [
            new PricedProduct("Apple", 35, NoOffer.Instance),
            new PricedProduct("Banana", 20, NoOffer.Instance),
            new PricedProduct("Melon", 50, BuyOneGetOneFreeOffer.Instance),
            new PricedProduct("Lime", 15, ThreeForTwoOffer.Instance),
        ]);
    }

    /// <summary>Products in catalogue order. Receipts use this order.</summary>
    public IReadOnlyList<PricedProduct> Products { get; }

    public PricedProduct GetRequired(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        string key = name.Trim();
        if (!_byName.TryGetValue(key, out PricedProduct? product))
        {
            throw new UnknownProductException(key);
        }

        return product;
    }
}
