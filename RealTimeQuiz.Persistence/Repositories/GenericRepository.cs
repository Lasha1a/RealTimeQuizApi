using Microsoft.EntityFrameworkCore;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Application.Interfaces.Specifications;
using RealTimeQuiz.Domain.Common;
using RealTimeQuiz.Persistence.Data;
using RealTimeQuiz.Persistence.Specifications.Evualator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }

    public void Delete(T entity) =>
        _context.Set<T>().Remove(entity);

    public IQueryable<T> GetAll() =>
        _context.Set<T>().AsQueryable();

    public async Task<T?> GetByIdAsync(Guid id) =>
        await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);

    public void Update(T entity) =>
        _context.Set<T>().Update(entity);

    public async Task<int> SaveChangesAsync() =>
        await _context.SaveChangesAsync();


    public async Task<T?> GetEntityWithSpec(ISpecification<T> spec) =>
        await SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec)
        .FirstOrDefaultAsync();

    public IQueryable<T> GetQueryWithSpec(ISpecification<T> spec) =>
        SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec);
}
