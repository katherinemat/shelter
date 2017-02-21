using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Shelter.Objects
{
  public class Animal
  {
    private int _id;
    private string _name;
    private int _age;
    private string _species;

    public Animal(string Name, int Age, string Species, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _age = Age;
      _species = Species;
    }

    public override bool Equals(System.Object otherAnimal)
    {
      if (!(otherAnimal is Animal))
      {
        return false;
      }
      else
      {
        Animal newAnimal = (Animal) otherAnimal;
        bool idEquality = (this.GetId() == newAnimal.GetId());
        bool ageEquality = (this.GetAge() == newAnimal.GetAge());
        bool nameEquality = (this.GetName() == newAnimal.GetName());
        bool speciesEquality = (this.GetSpecies() == newAnimal.GetSpecies());
        return (idEquality && ageEquality && nameEquality && speciesEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }
    public void SetName(string Name)
    {
      _name = Name;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetAge(int Age)
    {
      _age = Age;
    }
    public int GetAge()
    {
      Console.WriteLine(_age);
      return _age;
    }
    public void SetSpecies(string Species)
    {
      _species = Species;
    }
    public string GetSpecies()
    {
      return _species;
    }

    public static List<Animal> GetAll()
    {
      List<Animal> allAnimals = new List<Animal>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM animals;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int animalId = rdr.GetInt32(0);
        Console.WriteLine(animalId);
        string animalName = rdr.GetString(1);
        int animalAge = rdr.GetInt32(2);
        string animalSpecies = rdr.GetString(3);
        Animal newAnimal = new Animal(animalName, animalAge, animalSpecies, animalId);
        allAnimals.Add(newAnimal);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return allAnimals;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM animals;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

  }
}
