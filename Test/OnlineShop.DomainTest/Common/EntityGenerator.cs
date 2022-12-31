using OnlineShop.Domain.Entities;

namespace OnlineShop.DomainTest.Common;

public class EntityGenerator
{
    public User GenerateUser => new User("asdf",  new string(Enumerable.Repeat('a',256).ToArray()), "asdf");
}