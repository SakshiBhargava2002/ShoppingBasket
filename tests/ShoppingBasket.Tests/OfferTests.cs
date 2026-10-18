using ShoppingBasket.Offers;

namespace ShoppingBasket.Tests;

public class OfferTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(4, 4)]
    public void Full_price_charges_every_unit(int quantity, int chargeable)
    {
        Assert.Equal(chargeable, NoOffer.Instance.ChargeableUnits(quantity));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    [InlineData(4, 2)]
    [InlineData(5, 3)]
    [InlineData(9, 5)]
    public void Buy_one_get_one_free_charges_one_item_in_each_pair(int quantity, int chargeable)
    {
        Assert.Equal(chargeable, BuyOneGetOneFreeOffer.Instance.ChargeableUnits(quantity));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 2)]
    [InlineData(4, 3)]
    [InlineData(5, 4)]
    [InlineData(6, 4)]
    [InlineData(7, 5)]
    [InlineData(9, 6)]
    public void Three_for_two_charges_two_units_for_each_group_of_three(int quantity, int chargeable)
    {
        Assert.Equal(chargeable, ThreeForTwoOffer.Instance.ChargeableUnits(quantity));
    }

    [Fact]
    public void Buy_one_get_one_free_rejects_a_quantity_that_cannot_be_paired_safely()
    {
        Assert.Throws<OverflowException>(() =>
            BuyOneGetOneFreeOffer.Instance.ChargeableUnits(int.MaxValue));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void Offers_reject_a_negative_quantity(int quantity)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => NoOffer.Instance.ChargeableUnits(quantity));
        Assert.Throws<ArgumentOutOfRangeException>(() => BuyOneGetOneFreeOffer.Instance.ChargeableUnits(quantity));
        Assert.Throws<ArgumentOutOfRangeException>(() => ThreeForTwoOffer.Instance.ChargeableUnits(quantity));
    }
}
