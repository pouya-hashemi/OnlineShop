﻿using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Domain.Common;

public static class DependencyInjections
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddTransient<HashManager>();
        services.AddTransient<IUserManager,UserManager>();
        
        return services;
    }
}