namespace ShoppingBasket.Tests;

public class MoneyTests
{
    [Theory]
    [InlineData(0, "£0.00")]
    [InlineData(5, "£0.05")]
    [InlineData(35, "£0.35")]
    [InlineData(90, "£0.90")]
    [InlineData(100, "£1.00")]
    [InlineData(115, "£1.15")]
    [InlineData(1234, "£12.34")]
    public void Pence_are_formatted_as_pounds(int pence, string expected)
    {
        Assert.Equal(expected, Money.FormatPence(pence));
    }

    [Fact]
    public void A_negative_amount_cannot_be_formatted()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Money.FormatPence(-1));
    }
}
