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
    private int _speciesId;

    public Animal(string Name, int Age, int SpeciesId, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _age = Age;
      _speciesId = SpeciesId;
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
        bool speciesIdEquality = (this.GetSpeciesId() == newAnimal.GetSpeciesId());
        return (idEquality && ageEquality && nameEquality && speciesIdEquality);
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
    public void SetSpeciesId(int SpeciesId)
    {
      _speciesId = SpeciesId;
    }
    public int GetSpeciesId()
    {
      return _speciesId;
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
        int animalSpeciesId = rdr.GetInt32(3);
        Animal newAnimal = new Animal(animalName, animalAge, animalSpeciesId, animalId);
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

      SqlCommand cmd = new SqlCommand("INSERT INTO animals (name, age, species) OUTPUT INSERTED.id VALUES (@AnimalName, @AnimalAge, @AnimalSpeciesId);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@AnimalName";
      nameParameter.Value = this.GetName();

      SqlParameter ageParameter = new SqlParameter();
      ageParameter.ParameterName = "@AnimalAge";
      ageParameter.Value = this.GetAge();

      SqlParameter speciesIdParameter = new SqlParameter();
      speciesIdParameter.ParameterName = "@AnimalSpeciesId";
      speciesIdParameter.Value = this.GetSpeciesId();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(ageParameter);
      cmd.Parameters.Add(speciesIdParameter);
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
      int foundAnimalSpeciesId = 0;
      while(rdr.Read())
      {
        foundAnimalId = rdr.GetInt32(0);
        foundAnimalName = rdr.GetString(1);
        foundAnimalAge = rdr.GetInt32(2);
        foundAnimalSpeciesId = rdr.GetInt32(3);
      }
      Animal foundAnimal = new Animal(foundAnimalName, foundAnimalAge, foundAnimalSpeciesId, foundAnimalId);

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
