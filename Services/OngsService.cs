using OngResgisterApi.Models;
using API.Infra;
using restaurant_api.Utils;
using X.PagedList;
using Microsoft.AspNetCore.Mvc;

namespace OngApi.Services;

public class OngsService
{
    private readonly IMongoRepository<Ong> _ong;

    public OngsService(
       IMongoRepository<Ong> ong)
    {
        _ong = ong;
    }

    public async Task<IPagedList<Ong>> GetAsync(int page, int count, string? name,
             string? purpose,
            string? search,
             string? how_to_assist) {
        var result = await _ong.Get();
        var pagedResult = result
        .Where(ong =>
                   EntintyFilters.HasSearchString(ong.Name, name) &&
                   EntintyFilters.HasSearchString(ong.Purpose, purpose) &&
                   ong.HowToAssist.Any(s => EntintyFilters.HasSearchString(s, how_to_assist)) &&
                       (
                       EntintyFilters.HasSearchString(ong.Name, search) ||
                       EntintyFilters.HasSearchString(ong.Purpose, search) ||
                       ong.HowToAssist.Any(s => EntintyFilters.HasSearchString(s, search))
                       )
                   )
                   .ToPagedList(page,count);
        return pagedResult;
    }

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