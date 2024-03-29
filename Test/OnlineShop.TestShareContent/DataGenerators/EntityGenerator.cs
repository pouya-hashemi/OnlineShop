﻿using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.EntityPropertyConfigurations;

namespace OnlineShop.TestShareContent.DataGenerators;

public class EntityGenerator
{
    public User GenerateUser =>  new User(new string(Enumerable.Repeat('a',UserPropertyConfiguration.UsernameMinLength).ToArray()),
        new string(Enumerable.Repeat('a',UserPropertyConfiguration.UserTitleMinLength).ToArray()));
    public Role GenerateRole =>  new Role(new string(Enumerable.Repeat('a',RolePropertyConfiguration.NameMaxLength).ToArray()));
    
    public Category GenerateCategory =>  new Category(new string(Enumerable.Repeat('a',CategoryPropertyConfiguration.NameMaxLength).ToArray()));
    public Product GenerateProduct =>  new Product(new string(Enumerable.Repeat('a',ProductPropertyConfiguration.NameMinLength).ToArray()),
        new string(Enumerable.Repeat('a',ProductPropertyConfiguration.ImagePathMinLength).ToArray()),ProductPropertyConfiguration.PriceMinValue,ProductPropertyConfiguration.QuantityMinValue,GenerateCategory
    ,new string(Enumerable.Repeat('a',ProductPropertyConfiguration.ImageUrlMinLength).ToArray()));

    public Cart GenerateCart =>  new Cart(Guid.NewGuid().ToString(),GenerateProduct,CartPropertyConfigurations.MinQuantity,CartPropertyConfigurations.MinPrice);


}