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

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO animals (name, age, species) OUTPUT INSERTED.id VALUES (@AnimalName, @AnimalAge, @AnimalSpecies);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@AnimalName";
      nameParameter.Value = this.GetName();

      SqlParameter ageParameter = new SqlParameter();
      ageParameter.ParameterName = "@AnimalAge";
      ageParameter.Value = this.GetAge();

      SqlParameter speciesParameter = new SqlParameter();
      speciesParameter.ParameterName = "@AnimalSpecies";
      speciesParameter.Value = this.GetSpecies();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(ageParameter);
      cmd.Parameters.Add(speciesParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static Animal Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM animals WHERE id = @AnimalId;", conn);
      SqlParameter animalIdParameter = new SqlParameter();
      animalIdParameter.ParameterName = "@AnimalId";
      animalIdParameter.Value = id.ToString();
      cmd.Parameters.Add(animalIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundAnimalId = 0;
      int foundAnimalAge = 0;
      string foundAnimalName = null;
      string foundAnimalSpecies = null;
      while(rdr.Read())
      {
        foundAnimalId = rdr.GetInt32(0);
        foundAnimalName = rdr.GetString(1);
        foundAnimalAge = rdr.GetInt32(2);
        foundAnimalSpecies = rdr.GetString(3);
      }
      Animal foundAnimal = new Animal(foundAnimalName, foundAnimalAge, foundAnimalSpecies, foundAnimalId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundAnimal;
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
