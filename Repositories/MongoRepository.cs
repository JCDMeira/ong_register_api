using MongoDB.Driver;
using OngResgisterApi.Infra;
using OngResgisterApi.Models;



namespace API.Infra
{
    public interface IMongoRepository<T>
    {
        Task<List<T>> Get();
        Task<T> Get(string id);
        Task<T> GetByName(string name);
        Task<T> Create(T entity);
        Task Update(string id, T entity);
        Task Remove(string id);
    }


    public class MongoRepository<T> : IMongoRepository<T> where T : MongoBaseEntity
    {
        private readonly IMongoCollection<T> _model;

        public MongoRepository(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _model = database.GetCollection<T>(typeof(T).Name.ToLower());
        }
        public async Task<List<T>> Get()
        {
            var result = await _model.Find(_ => true).ToListAsync();
            return result;

        }

        public async Task<T> Get(string id) => await
            _model.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<T> GetByName(string name) => await
           _model.Find(x => x.Name == name).FirstOrDefaultAsync();

        public async Task<T> Create(T entinty)
        {
            await _model.InsertOneAsync(entinty);
            return entinty;
        }

        public async Task Update(string id, T entity) => await _model.ReplaceOneAsync(x => x.Id == id, entity);

        public async Task Remove(string id)
        {
           await _model.DeleteOneAsync(x => x.Id == id);
        }

       
    }
}
