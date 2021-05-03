using asp_net_core_web_api_v2.Models;
using asp_net_core_web_api_v2.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_net_core_web_api_v2.Controllers
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalService _animalService;

        public AnimalsController(IAnimalService animalService)
        {
            _animalService = animalService;
        }


        [HttpGet]
        public async Task<ActionResult> Get([FromQuery]string orderBy) //using [FromQuery] attr so @orderBy could be null
        {
            var animals = await _animalService.GetAllAsync(orderBy);
            if (!animals.Any()) return new OkObjectResult("No records were found");
            return Ok(animals);
        }

        
        [HttpPost]
        public async Task<ActionResult> Post(Animal animal)
        {
            return await _animalService.AddAsync(animal);
        }

        
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Animal animal)
        {
            return await _animalService.UpdateAsync(id, animal);
        }

        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await _animalService.DeleteAsync(id);
        }
    }
}
