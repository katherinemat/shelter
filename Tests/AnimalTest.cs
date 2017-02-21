using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using Shelter.Objects;

namespace Shelter
{
  public class AnimalTest : IDisposable
  {
    public AnimalTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=shelter_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Animal.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfNamesAreTheSame()
    {
      Animal firstAnimal = new Animal("bubbles", 2, "penguin");
      Animal secondAnimal = new Animal("bubbles", 2, "penguin");

      Assert.Equal(firstAnimal, secondAnimal);
    }

    public void Dispose()
    {
      Animal.DeleteAll();
    }
  }
}
