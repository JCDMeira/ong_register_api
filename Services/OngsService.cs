using OngResgisterApi.Models;
using API.Infra;

namespace OngApi.Services;

public class OngsService
{
    private readonly IMongoRepository<Ong> _ong;

    public OngsService(
       IMongoRepository<Ong> ong)
    {
        _ong = ong;
    }

    public async Task<List<Ong>> GetAsync() =>
        await _ong.Get();

    public async Task<Ong?> GetAsync(string id) =>
        await _ong.Get(id);

    public async Task<Ong?> GetByNameAsync(string name) =>
     await _ong.GetByName(name);

    public async Task CreateAsync(Ong newOng) =>
        await _ong.Create(newOng);

    public async Task UpdateAsync(string id, Ong updatedOng) =>
        await _ong.Update(id, updatedOng);

    public async Task RemoveAsync(string id) =>
        await _ong.Remove(id);
}