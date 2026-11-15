using BikeStore.Services;
using Xunit;

namespace BikeStore.Tests;
public sealed class RentalPricingServiceTests
{
    private readonly RentalPricingService _service = new();

    [Fact]
    public void TwoDays_MultipliesDailyPrice() =>
        Assert.Equal(50m, _service.Calculate(25m, new DateTime(2026, 11, 20), new DateTime(2026, 11, 22)));

    [Fact]
    public void ZeroDayRental_Throws() =>
        Assert.Throws<ArgumentException>(() => _service.Calculate(20m, new DateTime(2026, 11, 20), new DateTime(2026, 11, 20)));

    [Fact]
    public void NegativeDailyPrice_Throws() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => _service.Calculate(-1m, new DateTime(2026, 11, 20), new DateTime(2026, 11, 21)));

    [Fact]
    public void MoreThanThirtyDays_Throws() =>
        Assert.Throws<ArgumentException>(() => _service.Calculate(20m, new DateTime(2026, 11, 20), new DateTime(2026, 12, 25)));
}
