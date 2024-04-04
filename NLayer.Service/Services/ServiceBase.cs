using NLayer.Core.Repositories;
using NLayer.Core.UnitOfWorks;

namespace NLayer.Service.Services
{
    public class ServiceBase
    {
        private readonly IGenericRepository<T> _repository;
        private readonly IUnitOfWork _unitofwork;
    }
}