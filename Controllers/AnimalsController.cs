using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnimalsController : ControllerBase {
    private readonly IConfiguration _configuration;
    public AnimalsController(IConfiguration conf) {
        _configuration = conf;
    }

    [HttpGet]
    public IActionResult GetAnimals() {
        using(SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"))) {
            string query = "SELECT * FROM Animal;";
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            using(SqlDataReader reader = command.ExecuteReader()) {
                while(reader.Read()) {
                    Console.WriteLine(String.Format("{0}, {1}",
                                reader[0], reader[1])); 
                }
            }
        }
        return Ok();
    }
}
