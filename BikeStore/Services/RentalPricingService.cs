namespace BikeStore.Services;
public sealed class RentalPricingService : IRentalPricingService
{
    public decimal Calculate(decimal dailyRate, DateTime startDate, DateTime endDate)
    {
        if (dailyRate <= 0m) throw new ArgumentOutOfRangeException(nameof(dailyRate));
        int days = (endDate.Date - startDate.Date).Days;
        if (days < 1 || days > 30)
            throw new ArgumentException("Наемът трябва да бъде между 1 и 30 дни.");
        return decimal.Round(dailyRate * days, 2, MidpointRounding.AwayFromZero);
    }
}
