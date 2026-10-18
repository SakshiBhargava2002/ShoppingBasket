namespace ShoppingBasket;

/// <summary>
/// Thrown when a basket contains a name that is not in the price catalogue.
/// </summary>
public sealed class UnknownProductException : Exception
{
    public UnknownProductException(string productName)
        : base($"'{productName}' is not a known product.")
    {
        ProductName = productName;
    }

    public string ProductName { get; }
}
