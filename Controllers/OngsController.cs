using Microsoft.AspNetCore.Mvc;
using OngResgisterApi.Models;
using RestaurantApi.Services;

namespace OngResgisterApi.Controllers
{
    [Route("/api/ongs")]
    [ApiController]
    public class OngsController : Controller
    {
        private OngsService _ongsService;

        public OngsController(OngsService service) => _ongsService = service;

        [HttpGet]
        public async Task<List<Ong>> Get() => await _ongsService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Ong>> Get(string id)
        {
            var ong = await _ongsService.GetAsync(id);
            if(ong == null) return NotFound();
            return Ok(ong);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Ong newOng)
        {
            var ong = await _ongsService.GetByNameAsync(newOng.Name);
            if(ong != null) return BadRequest();
            await _ongsService.CreateAsync(newOng);
            return CreatedAtAction(nameof(Get), new {id= newOng.Id}, newOng);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Ong updatedOng)
        {
            var ong = await _ongsService.GetAsync(id);
            if (ong == null) return NotFound();

            var existingOng = await _ongsService.GetByNameAsync(updatedOng.Name);
            if (existingOng != null) return BadRequest();

            await _ongsService.UpdateAsync(id, updatedOng);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ong = await _ongsService.GetAsync(id);
            if(ong == null) return NotFound();
            await _ongsService.RemoveAsync(id);
            return NoContent();
        }
    }
}
