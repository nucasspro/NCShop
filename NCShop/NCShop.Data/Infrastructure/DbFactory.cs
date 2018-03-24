namespace NCShop.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private NCShopDbContext dbContext;

        public NCShopDbContext Init()
        {
            return dbContext ?? (dbContext = new NCShopDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}