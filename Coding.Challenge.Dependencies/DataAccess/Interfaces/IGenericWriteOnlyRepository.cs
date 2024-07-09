using Coding.Challenge.Dependencies.Models;

namespace Coding.Challenge.Dependencies.DataAccess.Interfaces;

public interface IGenericWriteOnlyRepository <T> where T : class
{
    Task<T?> CreateContent(T entity);
    Task<Guid> Delete(Guid id);
    Task<Content?> UpdateContent(Guid id, T content);
}
