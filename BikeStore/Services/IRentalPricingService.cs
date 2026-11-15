namespace BikeStore.Services;
public interface IRentalPricingService
{
    decimal Calculate(decimal dailyRate, DateTime startDate, DateTime endDate);
}
