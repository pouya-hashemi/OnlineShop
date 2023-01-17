using System.Reflection;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineShop.Application.Common;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(typeof(ApplicationDependencyInjection));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMapsterConfiguration(configuration);

        
        return services;
    }
}