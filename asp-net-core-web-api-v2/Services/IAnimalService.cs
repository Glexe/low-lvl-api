using asp_net_core_web_api_v2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_net_core_web_api_v2.Services
{
    public interface IAnimalService
    {
        public abstract Task<IEnumerable<Animal>> GetAllAsync(string orderBy);
        public abstract Task<ActionResult> AddAsync(Animal animal);
        public abstract Task<ActionResult> UpdateAsync(int id, Animal animal);
        public abstract Task<ActionResult> DeleteAsync(int id);
    }
}
