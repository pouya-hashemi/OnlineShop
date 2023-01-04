﻿using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.EntityPropertyConfigurations;

namespace OnlineShop.ApplicationTest.Common;

public class EntityGenerator
{
    public User GenerateUser =>  new User(new string(Enumerable.Repeat('a',UserPropertyConfiguration.UsernameMinLength).ToArray()),
         new HashManager().CreateHash( new string( Enumerable.Repeat('a',UserPropertyConfiguration.PasswordMinLength).ToArray())),
        new string(Enumerable.Repeat('a',UserPropertyConfiguration.UserTitleMinLength).ToArray()));
}