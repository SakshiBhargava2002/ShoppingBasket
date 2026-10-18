namespace ShoppingBasket;

/// <summary>
/// Formats sterling amounts that are stored as whole pence.
/// </summary>
public static class Money
{
    /// <summary>
    /// Formats a non-negative pence amount as pounds and pence, for example 90 becomes "£0.90".
    /// </summary>
    public static string FormatPence(int pence)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(pence);

        int pounds = pence / 100;
        int remainder = pence % 100;
        return $"£{pounds}.{remainder:00}";
    }
}
