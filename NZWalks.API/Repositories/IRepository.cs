using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

/*
 * The repository pattern is used to abstract the data access logic and provide a clean separation between the data
 * access layer and the business logic layer. By using the repository pattern, we can centralize data access logic,
 * making it easier to manage and maintain. It also allows for better unit testing by enabling the use of mock
 * repositories. In the provided code, the IRepository interface defines the contract for data operations, and the
 * SqlRegionRepository class implements this interface to interact with the database using Entity Framework. This
 * approach promotes a more modular and testable codebase.
 */
public interface IRepository<T>
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task<T> AddAsync(T item);
    Task<T?> UpdateAsync(Guid id, T item);
    Task<T?> DeleteAsync(Guid id);
}