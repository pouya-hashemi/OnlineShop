using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Application.ApplicationServices.ProductServices.Responses;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.Common;

public static class MapsterConfiguration
{
    public static void AddMapsterConfiguration(this IServiceCollection services,IConfiguration configuration)
    {
        TypeAdapterConfig<Product, ProductDto>
            .NewConfig()
            .Map(dest => dest.ImageUrl, src =>configuration["FileDownloadBaseUrl"]+src.ImageUrl.Replace("\\","/"));
    }
}