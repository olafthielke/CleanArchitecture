using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Exceptions;
using BusinessLogic.Interfaces;
using Presentation.WebApi.Models;

namespace Presentation.WebApi.Controllers
{
    /// <summary>
    /// The CustomerController uses the RegisterCustomerUseCase in its Register method.
    /// This method only deals with validation of input data, conversion of the input
    /// to a canonical type that can be consumed by the use case, calling the use case
    /// and finally returning the data or error. We are keeping all web in here. The use
    /// case is unaware of web. That give us flexibility to call the use case from a
    /// non-web context, like a mobile app, a windows forms application or a console app,
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        public IGetAllCustomersUseCase GetAllUseCase { get; }
        public IRegisterCustomerUseCase RegisterUseCase { get; }


        public CustomersController(IGetAllCustomersUseCase getAllUseCase,
            IRegisterCustomerUseCase registerUseCase)
        {
            GetAllUseCase = getAllUseCase;
            RegisterUseCase = registerUseCase;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await GetAllUseCase.GetAllCustomers();
            return Ok(customers);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(ApiCustomerRegistration customerReg)
        {
            try
            {
                return await TryRegister(customerReg);
            }
            catch (Exception ex)
            {
                return await HandleException(ex);
            }
        }

        private async Task<IActionResult> TryRegister(ApiCustomerRegistration customerReg)
        {
            Validate(customerReg);
            var reg = customerReg.ToRegistration();
            var customer = await RegisterUseCase.RegisterCustomer(reg);
            return Ok(customer);
        }


        private static void Validate(ApiCustomerRegistration customerReg)
        {
            if (customerReg == null)
                throw new MissingCustomerRegistration();
        }

        private async Task<IActionResult> HandleException(Exception ex)
        {
            await Task.CompletedTask;

            return ex switch
            {
                ValidationException valEx => BadRequest(valEx.Errors),
                ClientInputException => BadRequest(new[] { ex.Message }),
                NotFoundException => NotFound(),
                ServiceException => new StatusCodeResult(502), // Bad Gateway, a call to an incoming host fails.
                _ => throw ex   // ... otherwise rethrow and generate a 500 - Internal Server Error
            };
        }
    }
}
