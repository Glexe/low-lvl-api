using asp_net_core_web_api_v2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace asp_net_core_web_api_v2.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly IDatabaseService _dbService;

        public AnimalService(IDatabaseService dbService)
        {
            _dbService = dbService;
            dbService.Build();
        }

        public async Task<ActionResult> AddAsync(Animal animal)
        {
            SqlCommand command = new();
            command.CommandText = "INSERT INTO ANIMAL VALUES(@name, @desc, @cat, @area);";
            command.Parameters.AddWithValue("@name", animal.Name);
            command.Parameters.AddWithValue("@desc", animal.Description);
            command.Parameters.AddWithValue("@cat", animal.Category);
            command.Parameters.AddWithValue("@area", animal.Area);

            var affectedRowsCount = await _dbService.SendCommandAsync(command);
            if (affectedRowsCount == 0) return new BadRequestResult();
            return new OkObjectResult("Record has been successfully inserted");
        }

        public async Task<ActionResult> DeleteAsync(int id)
        {
            SqlCommand command = new();
            command.CommandText = "DELETE FROM Animal WHERE IdAnimal = @Id";
            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            int affectedRowsCount = await _dbService.SendCommandAsync(command);
            if (affectedRowsCount == 0) return new BadRequestObjectResult("Record with such an id wasn't found");
            return new OkObjectResult($"Record #{id} has been successfully deleted");
        }

        public async Task<IEnumerable<Animal>> GetAllAsync(string orderBy)
        {
            //cannot use order by with @parameter
            if (orderBy is null) orderBy = "Name";
            SqlCommand command = new();
            command.CommandText = "SELECT * FROM Animal ORDER BY ";
            command.CommandText += orderBy.ToLower() switch
            {
                "name" => "Name ASC;",
                "description" => "Description ASC;",
                "category" => "Category ASC;",
                "area" => "Area ASC;",
                _ => throw new ArgumentException("No such field to order by was found"),
            };


            var list = new List<Animal>();
            var dr = _dbService.SendRequestAsync(command);
            await foreach(var record in dr)
            {
                list.Add(new Animal
                {
                    IdAnimal = (int)record["IdAnimal"],
                    Name = record["Name"].ToString(),
                    Description = record["Description"].ToString(),
                    Category = record["Category"].ToString(),
                    Area = record["Area"].ToString()
                });
            }

            return list;
        }

        public async Task<ActionResult> UpdateAsync(int id, Animal animal)
        {
            SqlCommand command = new();
            command.CommandText = "UPDATE Animal SET Name = @name, Description = @desc, Category = @cat, Area = @area WHERE IdAnimal = @id";
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = animal.Name;
            command.Parameters.Add("@desc", SqlDbType.NVarChar).Value = animal.Description;
            command.Parameters.Add("@cat", SqlDbType.NVarChar).Value = animal.Category;
            command.Parameters.Add("@area", SqlDbType.NVarChar).Value = animal.Area;

            var affectedRowsCount = await _dbService.SendCommandAsync(command);
            if (affectedRowsCount == 0) return new BadRequestObjectResult("Record with such an id wasn't found");
            return new OkObjectResult($"Record #{id} has been successfully updated");
        }
    }
}
