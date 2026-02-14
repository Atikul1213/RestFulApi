using EmployeeAdminPortal.Repositories.Tests;

namespace MyAPI.XUnitTests;

public class CalculatorServiceTests
{
    private readonly CalculatorService _calculatorService;
    public CalculatorServiceTests()
    {
        _calculatorService = new CalculatorService();
    }

    [Fact]
    public void Add_WhenCalledWith2And3_Return5()
    {
        // Arrange- done in constructor
        //Act
        var result = _calculatorService.Add(2, 3);

        //Assert
        Assert.Equal(5, result);
    }


    // Test Subtract method with positive numbers
    [Fact]
    public void Substract_WhenCalledWith5And3_Return2()
    {
        // Arrange - done in construct
        //Act
        var result = _calculatorService.Subtract(5, 3);

        //Assert
        Assert.Equal(2, result);
    }


    // Parameterized test for Multiply method using Theory and InlineData
    [Theory]
    [InlineData(2, 3, 6)]
    [InlineData(-2, 3, -6)]
    [InlineData(0, 5, 0)]

    public void Multiply_WhenCalled_ReturnExpectedResult(int a, int b, int expected)
    {
        //Arrange - in constructor
        //Act
        var result = _calculatorService.Multiply(a, b);
        //Assert
        Assert.Equal(expected, result);
    }

    // Test Divide method for normal case
    [Fact]
    public void Divide_WhenCalledWith6And3_Returns2()
    {
        // Arrange - done in constructor
        // Act
        var result = _calculatorService.Divide(6, 3);
        // Assert
        Assert.Equal(2, result);
    }

    // Test Divide method to check division by zero throws exception
    [Fact]
    public void Divide_WhenDividingByZero_ThrowDivideByZeroException()
    {
        //Act and Assert
        Assert.Throws<DivideByZeroException>(() =>
            _calculatorService.Divide(10, 0));
    }
}
