﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using FluentAssertions;
using BusinessLogic.Entities;
using BusinessLogic.Exceptions;
using Presentation.WebApi.Controllers;
using Presentation.WebApi.Models;
using Tests.Fakes.BusinessLogic;

namespace Tests.Presentation
{
    /// <summary>
    /// Unit tests for the CustomersController's Register method.
    /// </summary>
    public class CustomersControllerTests
    {
        [Fact]
        public async Task Given_No_CustomerRegistration_When_Call_Register_Then_Return_400_BadRequest()
        {
            var controller = SetupController();
            var result = await controller.Register(null);
            VerifyBadRequestResult(result, "Missing customer registration data.");
        }

        [Theory]
        [MemberData(nameof(GetApiRegistrationsAndRegistrations))]
        public async Task When_Call_Register_Then_Delegate_To_UseCase(ApiCustomerRegistration apiReg,
            CustomerRegistration reg)
        {
            var controller = SetupController();
            await controller.Register(apiReg);
            VerifyCallUseCase(controller, reg);
        }

        [Theory]
        [MemberData(nameof(GetApiRegistrationsAndCustomers))]
        // TODO: Should really return 201 - Created.
        public async Task Given_UseCase_Returns_Customer_When_Call_Register_Then_Return_200_OK(ApiCustomerRegistration apiReg,
            Customer customer)
        {
            var controller = SetupController(customer);
            var result = await controller.Register(apiReg);
            VerifyOkResult(result, customer);
        }

        [Theory]
        [InlineData("Client Input Error")]
        [InlineData("a different error message")]
        public async Task Given_UseCase_Throws_Simple_ClientInputException_When_Call_Register_Then_Return_400_BadRequest(string errorMsg)
        {
            var controller = SetupController(new DummyClientInputException(errorMsg));
            var result = await controller.Register(ApiRegoAdamAnt);
            VerifyBadRequestResult(result, errorMsg);
        }

        [Theory]
        [InlineData("Error 1")]
        [InlineData("Error 1", "Error 2")]
        [InlineData("Error 1", "Error 2", "Error 3")]
        public async Task Given_UseCase_Throws_ValidationException_When_Call_Register_Then_Return_400_BadRequest_With_Messages(params string[] errorMsgs)
        {
            var controller = SetupController(new ValidationException(errorMsgs));
            var result = await controller.Register(ApiRegoAdamAnt);
            VerifyBadRequestResult(result, errorMsgs);
        }

        [Fact]
        public async Task Given_UseCase_Throws_ServiceException_When_Call_Register_Then_Return_502_BadGateway()
        {
            var controller = SetupController(new ServiceException("AWS Emailing failed!", new Exception("Message rejeced exception")));
            var result = await controller.Register(ApiRegoAdamAnt);
            VerifyBadGatewayResult(result);
        }


        //[Fact]
        //public async Task Given_UseCase_Throws_NonSpecific_Exception_When_Call_Register_Then_Return_500_InternalServerError()
        //{
        //    var controller = SetupController(new Exception("unhandled exception"));
        //    var result = await controller.Register(ApiRegoAdamAnt);
        //    VerifyInternalServerErrorResult(result);
        //}


        private static readonly ApiCustomerRegistration ApiRegoAdamAnt = new ApiCustomerRegistration("Adam", "Ant", "adam@ant.co.uk");
        private static readonly ApiCustomerRegistration ApiRegoBobSmith = new ApiCustomerRegistration("Bob", "Smith", "bob@smith.com");

        private static readonly CustomerRegistration RegoAdamAnt = new CustomerRegistration("Adam", "Ant", "adam@ant.co.uk");
        private static readonly CustomerRegistration RegoBobSmith = new CustomerRegistration("Bob", "Smith", "bob@smith.com");

        private static readonly Customer CustomerAdamAnt = new Customer(Guid.NewGuid(), "Adam", "Ant", "adam@ant.co.uk");
        private static readonly Customer CustomerBobSmith = new Customer(Guid.NewGuid(), "Bob", "Smith", "bob@smith.com");

        public static IEnumerable<object[]> GetApiRegistrationsAndRegistrations()
        {
            yield return new object[] { ApiRegoAdamAnt, RegoAdamAnt };
            yield return new object[] { ApiRegoBobSmith, RegoBobSmith };
        }

        public static IEnumerable<object[]> GetApiRegistrationsAndCustomers()
        {
            yield return new object[] { ApiRegoAdamAnt, CustomerAdamAnt };
            yield return new object[] { ApiRegoBobSmith, CustomerBobSmith };
        }


        private static CustomersController SetupController(Customer customer = null)
        {
            var useCase = new MockRegisterCustomerUseCase(customer);
            return new CustomersController(null, useCase);
        }

        private static CustomersController SetupController(Exception exception)
        {
            var useCase = new MockRegisterCustomerUseCase(exception);
            return new CustomersController(null, useCase);
        }


        private static void VerifyCallUseCase(CustomersController controller, CustomerRegistration rego)
        {
            var useCase = (MockRegisterCustomerUseCase)controller.RegisterUseCase;
            useCase.WasRegisterCalled.Should().BeTrue();
            useCase.PassedInRegistration.Should().BeEquivalentTo(rego);
        }

        private static void VerifyOkResult<T>(IActionResult result, T t)
        {
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(t);
        }

        private static void VerifyBadRequestResult(IActionResult result, params string[] messages)
        {
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Value.Should().BeEquivalentTo(messages);
        }

        private static void VerifyBadGatewayResult(IActionResult result)
        {
            var statusCodeResult = result as StatusCodeResult;
            statusCodeResult.StatusCode.Should().Be(502);
        }
    }
}