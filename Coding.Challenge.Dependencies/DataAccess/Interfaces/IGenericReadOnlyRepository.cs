using Coding.Challenge.Dependencies.Models;

namespace Coding.Challenge.Dependencies.DataAccess.Interfaces;

public interface IGenericReadOnlyRepository<T> where T : class
{
    Task<T?> GetContentById(Guid id);
    Task<IEnumerable<Content?>> GetManyContents();

}
