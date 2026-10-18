using ShoppingBasket;
using ShoppingBasket.Basket;

BasketPricer pricer = BasketPricer.ForSupermarket();

if (args.Length == 0)
{
    Demonstrate(
        pricer,
        "Brief example: two apples and a banana",
        ["Apple", "Apple", "Banana"]);

    Demonstrate(
        pricer,
        "Offers: two melons, three limes, and one apple",
        ["Melon", "Melon", "Lime", "Lime", "Lime", "Apple"]);

    Console.WriteLine("Pass item names as arguments to price your own basket.");
    Console.WriteLine("Example: dotnet run --project src/ShoppingBasket.Console -- Apple Banana Melon");
    return 0;
}

try
{
    PrintReceipt(pricer.Price(args));
    return 0;
}
catch (Exception ex) when (ex is UnknownProductException or ArgumentException or OverflowException)
{
    Console.Error.WriteLine(ex.Message);
    return 1;
}

static void Demonstrate(BasketPricer pricer, string title, string[] items)
{
    Console.WriteLine(title);
    Console.WriteLine($"Basket: {string.Join(", ", items)}");
    PrintReceipt(pricer.Price(items));
    Console.WriteLine();
}

static void PrintReceipt(BasketReceipt receipt)
{
    if (receipt.Lines.Count == 0)
    {
        Console.WriteLine("(empty basket)");
    }

    foreach (BasketLine line in receipt.Lines)
    {
        Console.WriteLine(
            $"{line.ProductName,-8} x{line.Quantity,-3} pay for {line.ChargeableUnits} at {Money.FormatPence(line.UnitPriceInPence)}  = {Money.FormatPence(line.LineTotalInPence)}  ({line.OfferDescription})");
    }

    Console.WriteLine($"Total: {receipt.TotalFormatted}");
}
