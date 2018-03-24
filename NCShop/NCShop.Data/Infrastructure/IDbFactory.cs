using System;

namespace NCShop.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        NCShopDbContext Init();
    }
}