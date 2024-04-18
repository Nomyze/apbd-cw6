using System.ComponentModel.DataAnnotations;
using System.Data;
namespace Models;

public class Animal {
    public int? IdAnimal {get;}
    [Required]
    public string Name {get; set;}
    public string? Description {get; set;}
    [Required]
    public string Category {get; set;}
    [Required]
    public string Area {get; set;}

    public Animal(IDataRecord fromSql) {
        IdAnimal = fromSql.GetInt32(0);
        Name = fromSql.GetString(1);
        Description = fromSql.GetValue(2).GetType() == typeof(System.DBNull) ? null : fromSql.GetString(2);
        Category = fromSql.GetString(3);
        Area = fromSql.GetString(4);
    }

    public Animal() {}

    
    override public string ToString() {
        return String.Format("Id: {0}\nName: {1}\nDescription: {2}\nCategory: {3}\nArea: {4}", IdAnimal, Name, Description, Category, Area);
    }
}
