# Shopping basket

A small C# program that prices a basket of apples, bananas, melons, and limes, including the multi-buy offers from the brief.

Prices are calculated in whole pence so the total is exact.

| Item | Unit price | Offer |
| --- | --- | --- |
| Apple | 35p | Full price |
| Banana | 20p | Full price |
| Melon | 50p | Buy one, get one free |
| Lime | 15p | Three for the price of two |

Items are passed as a list of names. `["Apple", "Apple", "Banana"]` is two apples and one banana, and costs **90p** (£0.90).

## Run

Requires the .NET 8 SDK.

```bash
dotnet run --project src/ShoppingBasket.Console
```

With no arguments, the program prints two worked examples. Pass item names to price a basket:

```bash
dotnet run --project src/ShoppingBasket.Console -- Apple Apple Banana Melon Melon Lime Lime Lime
```

```text
Apple    x2   pay for 2 at £0.35  = £0.70  (full price)
Banana   x1   pay for 1 at £0.20  = £0.20  (full price)
Melon    x2   pay for 1 at £0.50  = £0.50  (buy one get one free)
Lime     x3   pay for 2 at £0.15  = £0.30  (three for the price of two)
Total: £1.70
```

An unknown name, such as `Pear`, is written to standard error and the process exits with code 1.

## Test

```bash
dotnet test
```

Packages are restored from nuget.org. `nuget.config` keeps that restore independent of any other feed configured on the machine.

The tests cover each price, both offers (including quantities that do and do not complete a deal), mixed baskets, item order, case and spacing, an empty basket, unknown and blank names, and a total that would overflow.

## Layout

```text
src/ShoppingBasket.Core      Pricing library
src/ShoppingBasket.Console   Command-line program
tests/ShoppingBasket.Tests   xUnit tests
docs/DESIGN.md               Design for review
```

The library entry point is `BasketPricer.CalculateTotalInPence`. `Price` returns the same total together with a line for each product.

## Design

See [docs/DESIGN.md](docs/DESIGN.md) for the pricing rules and the class model. The design also notes how products and promotions could later be loaded from configuration. That loader is not built here. The delivered scope is the class library and its tests.
