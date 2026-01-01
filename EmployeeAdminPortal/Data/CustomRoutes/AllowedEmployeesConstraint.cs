
namespace EmployeeAdminPortal.Data.CustomRoutes
{
    public class AllowedEmployeesConstraint : IRouteConstraint
    {
        private static readonly string[] AllowedEmployees = new[]
        {
            "Alice",
            "Bob",
            "Charlie",
            "Diana"
        };

        public bool Match(HttpContext? httpContext, IRouter? route, 
            string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.TryGetValue(routeKey, out var rawValue))
                return false;

            var employeeName = rawValue?.ToString();

            return !string.IsNullOrEmpty(employeeName) &&
                AllowedEmployees.Contains(employeeName, StringComparer.OrdinalIgnoreCase);
        }
    }
}
