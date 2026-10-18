namespace ShoppingBasket.Basket;

/// <summary>
/// The priced basket: one line per distinct product, plus the total in pence.
/// </summary>
public sealed class BasketReceipt
{
    public BasketReceipt(IReadOnlyList<BasketLine> lines, int totalInPence)
    {
        ArgumentNullException.ThrowIfNull(lines);
        ArgumentOutOfRangeException.ThrowIfNegative(totalInPence);

        int summed = 0;
        foreach (BasketLine line in lines)
        {
            ArgumentNullException.ThrowIfNull(line);
            checked
            {
                summed += line.LineTotalInPence;
            }
        }

        if (summed != totalInPence)
        {
            throw new ArgumentException(
                "Line totals do not match the basket total.",
                nameof(totalInPence));
        }

        Lines = lines;
        TotalInPence = totalInPence;
    }

    public IReadOnlyList<BasketLine> Lines { get; }

    public int TotalInPence { get; }

    public string TotalFormatted => Money.FormatPence(TotalInPence);
}
