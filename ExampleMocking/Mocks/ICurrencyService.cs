namespace ExampleMocking.TestClasses
{
    public interface ICurrencyService
    {
        decimal GetConversionRate(string fromCurrency, string toCurrency);

        decimal GetConversionRate(object currency1, object currency2);
    }
}