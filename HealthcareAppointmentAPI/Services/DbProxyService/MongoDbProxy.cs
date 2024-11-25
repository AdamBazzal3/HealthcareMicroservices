using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

using HealthcareAppointmentAPI.Settings.DbSettings;
using HealthcareAppointmentAPI.Attributes;


namespace HealthcareAppointmentAPI.Services.DbProxyService
{
    public class MongoDbProxy<T> : IDbProxy<T> where T : class
    {

        private readonly IMongoCollection<T> _instancesCollection;
        public MongoDbProxy(IOptions<HealthcareDbSettings> HealthcareDatabaseSettings)
        {
            var mongoClient = new MongoClient(Environment.GetEnvironmentVariable("ConnectionString"));

            var mongoDatabase = mongoClient.GetDatabase(Environment.GetEnvironmentVariable("DatabaseName"));

            _instancesCollection = mongoDatabase.GetCollection<T>((typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault()as BsonCollectionAttribute).CollectionName);
        }

        public async Task CreateAsync(T newInstance) =>
            await _instancesCollection.InsertOneAsync(newInstance);
        public async Task<List<T>> GetAsync()=>
            await _instancesCollection.Find(_ => true).ToListAsync();
        public async Task<List<T>> GetAsync(FilterDefinition<T> filter) =>
            await _instancesCollection.Find(filter).ToListAsync();

        public async Task<List<BsonDocument>> GetAsync(FilterDefinition<T> filter, ProjectionDefinition<T> projection)
        =>    await _instancesCollection.Find(filter).Project(projection).ToListAsync();
        
        public async Task RemoveAsync(FilterDefinition<T> filter) =>
            await _instancesCollection.DeleteOneAsync(filter);
        public async Task UpdateAsync(FilterDefinition<T> filter, T updatedInstance) =>
            await _instancesCollection.ReplaceOneAsync(filter, updatedInstance);

    }
}
