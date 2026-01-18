using EmployeeAdminPortal.Models.EcommerceModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace EmployeeAdminPortal.Services
{
    public class CustomObjectModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.FieldName).FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                bindingContext.Result = ModelBindingResult.Failed();
                // return Task.CompletedTask;
            }

            var parts = value.Split(":");

            if (parts.Length == 3)
            {
                var product = new Product
                {
                    Name = parts[0],
                    Category = parts[1],
                    Price = decimal.TryParse(parts[2], out var price) ? price : 0
                };

                bindingContext.Result = ModelBindingResult.Success(product);
            }
            else
                bindingContext.Result = ModelBindingResult.Failed();


            try
            {
                var httpContext = bindingContext.HttpContext;
                if (httpContext.Request.Headers == null ||
                    httpContext.Request.Query == null ||
                    httpContext.Request.Body == null)
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                }

                var headerValue = httpContext.Request.Headers["X-Custom-Header"].ToString();
                var queryValue = httpContext.Request.Query["queryValue"].ToString();

                string bodyContext = string.Empty;
                try
                {
                    using (var reader = new StreamReader(httpContext.Request.Body))
                    {
                        bodyContext = await reader.ReadToEndAsync();
                    }
                }
                catch (Exception ex)
                {
                    bindingContext.ModelState.AddModelError("Body", ex.Message);
                }

                Product product;
                try
                {
                    product = JsonConvert.DeserializeObject<Product>(bodyContext);
                }
                catch (Exception ex)
                {

                }

            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError("Binding", ex.Message);
            }

            //return Task.CompletedTask;
        }
    }
}

/**
    [ModelBinder(BinderType = typeof(CustomObjectModelBinder))]
    public class CustomObject
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
    }
 */
