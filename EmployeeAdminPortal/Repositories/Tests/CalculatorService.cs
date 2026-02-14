namespace EmployeeAdminPortal.Repositories.Tests
{
    public class CalculatorService
    {
        // Adds two integers and returns the sum
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Subtract(int a, int b)
        {
            //Introducing some error by adding 5 to the result
            return a - b;
        }
        // Multiply two numbers
        public int Multiply(int a, int b)
        {
            return a * b;
        }
        // Divide a by b; throws DivideByZeroException if b is zero
        public int Divide(int a, int b)
        {
            if (b == 0)
                throw new DivideByZeroException("Division by zero is not allowed.");
            return a / b;
        }
    }
}
