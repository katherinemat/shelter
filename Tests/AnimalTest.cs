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

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Animal testAnimal = new Animal("bubbles", 2, "penguin");

      //Act
      testAnimal.Save();
      List<Animal> result = Animal.GetAll();
      List<Animal> testList = new List<Animal>{testAnimal};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      Animal testAnimal = new Animal("bubbles", 2, "penguin");

      testAnimal.Save();
      Animal savedAnimal = Animal.GetAll()[0];

      int result = savedAnimal.GetId();
      int testId = testAnimal.GetId();
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_FindFindsAnimalInDatabase()
    {
      //Arrange
      Animal testAnimal = new Animal("bubbles", 2, "penguin");
      testAnimal.Save();

      //Act
      Animal foundAnimal = Animal.Find(testAnimal.GetId());

      //Assert
      Assert.Equal(testAnimal, foundAnimal);
    }

    public void Dispose()
    {
      Animal.DeleteAll();
    }
  }
}
