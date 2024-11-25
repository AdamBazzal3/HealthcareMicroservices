using MongoDB.Bson;
using MongoDB.Driver;

namespace HealthcareClinicAPI.Services.DbProxyService
{
    public interface IDbProxy<T>
    {
        Task<List<T>> GetAsync();
        Task<List<T>> GetAsync(FilterDefinition<T> filter);
        Task<List<T>> GetAsync(FilterDefinition<T> filter, ProjectionDefinition<T> projection);
        Task CreateAsync(T instance);
        Task UpdateAsync(FilterDefinition<T> filter, T updatedInstance);
        Task RemoveAsync(FilterDefinition<T> filter);
    }
}
