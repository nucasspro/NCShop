namespace NCShop.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}