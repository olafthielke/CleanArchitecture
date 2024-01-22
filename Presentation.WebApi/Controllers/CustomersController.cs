using System;
using System.Threading.Tasks;
using BusinessLogic;
using BusinessLogic.Entities;
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
    public class CustomersController(
        IGetAllCustomersUseCase getAllUseCase,
        IRegisterCustomerUseCase registerUseCase)
        : ControllerBase
    {
        public IGetAllCustomersUseCase GetAllUseCase { get; } = getAllUseCase;
        public IRegisterCustomerUseCase RegisterUseCase { get; } = registerUseCase;


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
            var validationResult = Validate(customerReg);
            if (validationResult.IsError)
                return await HandleError(validationResult.Error);
            var reg = customerReg.ToRegistration();
            var result = await RegisterUseCase.RegisterCustomer(reg);
            if (result.IsError)
                return await HandleError(result.Error);
            return Ok(result.Value);
        }


        private static Result<bool, Error> Validate(ApiCustomerRegistration customerReg)
        {
            if (customerReg == null)
                return ValidationErrors.MissingCustomerRegistration;

            return true;
        }

        private async Task<IActionResult> HandleError(Error err)
        {
            await Task.CompletedTask;

            return BadRequest(err.Message);
        }

        private async Task<IActionResult> HandleException(Exception ex)
        {
            await Task.CompletedTask;

            return ex switch
            {
                NotFoundException => NotFound(),
                ServiceException => new StatusCodeResult(502), // Bad Gateway, a call to an incoming host fails.
                _ => throw ex   // ... otherwise rethrow and generate a 500 - Internal Server Error
            };
        }
    }
}
