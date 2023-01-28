namespace OnlineShop.Domain.EntityPropertyConfigurations;

public class ProductPropertyConfiguration
{
    public const int NameMaxLength = 300;
    public const int NameMinLength = 1;
    public const int ImagePathMinLength = 1;
    public const int ImagePathMaxLength = 1000;
    public const int ImageUrlMinLength = 1;
    public const int ImageUrlMaxLength = 1000;
    public const int QuantityMinValue = 0;
    public const int PriceMinValue = 0;
}