using NCShop.Data.Infrastructure;
using NCShop.Data.Repositories;
using NCShop.Model.Models;

namespace NCShop.Service
{
    public interface IErrorSercive
    {
        Error Create(Error error);

        void Save();
    }

    public class ErrorService : IErrorSercive
    {
        private IErrorRepository _errorRepository;
        private IUnitOfWork _unitOfWork;

        public ErrorService(IErrorRepository errorRepository, IUnitOfWork unitOfWork)
        {
            this._errorRepository = errorRepository;
            this._unitOfWork = unitOfWork;
        }

        public Error Create(Error error)
        {
            return _errorRepository.Add(error);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}