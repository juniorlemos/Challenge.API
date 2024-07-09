
using Coding.Challenge.Dependencies.Models;

namespace Coding.Challenge.insfrastructure.DataAccess.Interfaces;

public interface IGenericWriteOnlyRepository <T> where T : class
{
    Task<T?> CreateContent(T entity);
}
