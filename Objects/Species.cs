using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Shelter.Objects
{
  public class Species
  {
    private int _id;
    private string _name;

    public Species(string name, int id = 0)
    {
      _id = id;
      _name = name;
    }

    public string GetName()
    {
      return _name;
    }

    public void SetName(string name)
    {
      _name = name;
    }

    public int GetId()
    {
      return _id;
    }

    public void SetId(int id)
    {
      _id = id;
    }

    public override bool Equals(System.Object otherSpecies)
    {
      if (!(otherSpecies is Species))
      {
        return false;
      }
      else
      {
        Species newSpecies = (Species) otherSpecies;
        bool idEquality = (this.GetId() == newSpecies.GetId());
        bool nameEquality = (this.GetName() == newSpecies.GetName());
        return (idEquality && nameEquality);
      }
    }

    public static List<Species> GetAll()
    {
      List<Species> allSpecies = new List<Species>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM species;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int speciesId = rdr.GetInt32(0);
        string speciesName = rdr.GetString(1);
        Species newSpecies = new Species(speciesName, speciesId);
        allSpecies.Add(newSpecies);
      }
      return allSpecies;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSTER INTO species (name) OUTPUT INSTERTED.id VALUES(@SpeciesName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@SpeciesName";
      nameParameter.Value = this.GetName();

      cmd.Parameters.Add(nameParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static Species Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand ("SELECT * FROM species WHERE id=@SpeciesId;", conn);

      SqlParameter speciesId = new SqlParameter();
      speciesId.ParameterName = "@SpeciesId";
      speciesId.Value = id.ToString();

      cmd.Parameters.Add(speciesId);

      SqlDataReader rdr = cmd.ExecuteReader();

      int foundSpeciesId = 0;
      string foundSpeciesName = null;

      while(rdr.Read())
      {
        foundSpeciesId = rdr.GetInt32(0);
        foundSpeciesName = rdr.GetString(1);

      }

      Species foundSpecies = new Species(foundSpeciesName, foundSpeciesId);

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return foundSpecies;
    }
  }
}
