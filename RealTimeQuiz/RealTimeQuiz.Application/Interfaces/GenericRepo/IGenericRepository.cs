using RealTimeQuiz.Application.Interfaces.Specifications;
using RealTimeQuiz.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.Interfaces.GenericRepo;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    IQueryable<T> GetAll();
    Task<T> AddAsync(T entity);
    Task<int> SaveChangesAsync();
    void Update(T entity);
    void Delete(T entity);
    Task<T?> GetEntityWithSpec(ISpecification<T> spec);
    IQueryable<T> GetQueryWithSpec(ISpecification<T> spec);
}
