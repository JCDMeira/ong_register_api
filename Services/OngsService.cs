using OngResgisterApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace RestaurantApi.Services;

public class OngsService
{
    private readonly IMongoCollection<Ong> _ongsCollection;

    public OngsService(
        IOptions<DatabaseSettings> ongsDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            ongsDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            ongsDatabaseSettings.Value.DatabaseName);

        _ongsCollection = mongoDatabase.GetCollection<Ong>(
            ongsDatabaseSettings.Value.CollectionName);
    }

    public async Task<List<Ong>> GetAsync() =>
        await _ongsCollection.Find(_ => true).ToListAsync();

    public async Task<Ong?> GetAsync(string id) =>
        await _ongsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<Ong?> GetByNameAsync(string name) =>
     await _ongsCollection.Find(x => x.Name == name).FirstOrDefaultAsync();

    public async Task CreateAsync(Ong newOng) =>
        await _ongsCollection.InsertOneAsync(newOng);

    public async Task UpdateAsync(string id, Ong updatedOng) =>
        await _ongsCollection.ReplaceOneAsync(x => x.Id == id, updatedOng);

    public async Task RemoveAsync(string id) =>
        await _ongsCollection.DeleteOneAsync(x => x.Id == id);
}