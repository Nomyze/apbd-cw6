using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Controllers;

using Models;

[Route("api/[controller]")]
[ApiController]
public class AnimalsController : ControllerBase {
    private readonly IConfiguration _configuration;
    public AnimalsController(IConfiguration conf) {
        _configuration = conf;
    }

    [HttpGet]
    public IActionResult GetAnimals([FromQuery] Order? orderBy) {
        List<Animal> animals = new();
        using(SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"))) {
            string query = "SELECT * FROM Animal";
            query += " ORDER BY ";
            switch(orderBy) {
                case Order.description:
                    query += "Description";
                    break;
                case Order.category:
                    query += "Category";
                    break;
                case Order.area:
                    query += "Area";
                    break;
                default:
                    query += "Name";
                    break;
            }
            query += ";";
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            using(SqlDataReader reader = command.ExecuteReader()) {
                while(reader.Read()) {
                    Animal animal = new Animal((IDataRecord) reader);
                    animals.Add(animal);
                }
            }
            connection.Close();
        }
        return Ok(animals);
    }

    [HttpPost]
    public IActionResult AddAnimal(Animal animal) {
        using(SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"))) {
            connection.Open();
            string query = "INSERT INTO Animal (Name, Category, Description, Area) VALUES (@animalName, @animalCategory, @animalDesc, @animalArea);";
            using(SqlCommand command = new SqlCommand(query, connection)) {
                command.Parameters.AddWithValue("@animalName", animal.Name);
                command.Parameters.AddWithValue("@animalCategory", animal.Category);
                command.Parameters.AddWithValue("@animalDesc", animal.Description == null ? DBNull.Value : animal.Description);
                command.Parameters.AddWithValue("@animalArea", animal.Area);
                
                Int32 added = command.ExecuteNonQuery();
                if(added == 0) 
                    Console.Error.WriteLine("Failed to add animal");
            }
            connection.Close();
        }
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateAnimal(int id, Animal animal) {
        using(SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"))) {
            connection.Open();
            string query = "UPDATE Animal SET Name=@animalName, Category=@animalCategory, Description=@animalDesc, Area=@animalArea WHERE IdAnimal=@animalId;";
            using(SqlCommand command = new SqlCommand(query, connection)) {
                command.Parameters.AddWithValue("@animalName", animal.Name);
                command.Parameters.AddWithValue("@animalCategory", animal.Category);
                command.Parameters.AddWithValue("@animalDesc", animal.Description == null ? DBNull.Value : animal.Description);
                command.Parameters.AddWithValue("@animalArea", animal.Area);
                command.Parameters.AddWithValue("@animalId", id);

                Int32 changed = command.ExecuteNonQuery();
                Console.Error.WriteLine($"Updated {changed} records");
            }
            connection.Close();
        }
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteAnimal(int id) {
        using(SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"))) {
            connection.Open();
            string query = "DELETE FROM Animal WHERE IdAnimal=@animalId;";
            using(SqlCommand command = new SqlCommand(query, connection)) {
                command.Parameters.AddWithValue("@animalId", id);

                Int32 changed = command.ExecuteNonQuery();
                Console.Error.WriteLine($"Deleted {changed} records");
            }
            connection.Close();
        }
        return Ok();
    }
}
