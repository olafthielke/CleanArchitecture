using System;
using Xunit;
using FluentAssertions;
using BusinessLogic.Entities;
using Notification.Email.Services;

namespace Tests.Email
{
    public class PlaceholderReplacerTests
    {
        [Theory]
        [InlineData("hello world!")]
        [InlineData("hi there!")]
        [InlineData("ABC 123")]
        public void Given_Object_Is_Null_When_Call_Replace_Then_Return_Input(string input)
        {
            var replacer = new PlaceholderReplacer();
            var result = replacer.Replace(input, null);
            result.Should().Be(input);
        }

        [Theory]
        [InlineData("hello world!")]
        [InlineData("hi there!")]
        [InlineData("ABC 123")]
        public void Given_No_Placeholders_When_Call_Replace_Then_Return_Input(string input)
        {
            var customer = new Customer(Guid.NewGuid(), "Fred", "Flintstone", "fred@flintstones.com");
            var replacer = new PlaceholderReplacer();
            var result = replacer.Replace(input, customer);
            result.Should().Be(input);
        }

        [Theory]
        [InlineData("Hi [[SocialSecurityNumber]]!")]
        [InlineData("You live at [[PostCode]]")]
        [InlineData("Number of nodes: [[NumberOfNodes]]")]
        public void Given_NonMatching_Placeholders_When_Call_Replace_Then_Return_Input(string input)
        {
            var customer = new Customer(Guid.NewGuid(), "Fred", "Flintstone", "fred@flintstones.com");
            var replacer = new PlaceholderReplacer();
            var result = replacer.Replace(input, customer);
            result.Should().Be(input);
        }

        [Theory]
        [InlineData("Hi [[FirstName]]!", "Fred", "Hi Fred!")]
        [InlineData("Hi [[FirstName]]!", "Wilma", "Hi Wilma!")]
        [InlineData("Hi [[FirstName]]!", "Pebbles", "Hi Pebbles!")]
        [InlineData("Hello [[FirstName]]. How are you?", "Fred", "Hello Fred. How are you?")]
        [InlineData("Sayonara [[FirstName]]! Wazzup?", "Wilma", "Sayonara Wilma! Wazzup?")]
        [InlineData("How are you [[FirstName]]?", "Pebbles", "How are you Pebbles?")]
        [InlineData("[[FirstName]] & [[FirstName]]!", "Fred", "Fred & Fred!")]
        [InlineData("[[FirstName]], [[FirstName]] and one more [[FirstName]]!", "Wilma", "Wilma, Wilma and one more Wilma!")]
        public void Given_FirstName_Placeholder_When_Call_Replace_Then_Replace_With_FirstName_Property_Value(
            string input, string firstName, string output)
        {
            var customer = new Customer(Guid.NewGuid(), firstName, "Flintstone", "fred@flintstones.com");
            var replacer = new PlaceholderReplacer();
            var result = replacer.Replace(input, customer);
            result.Should().Be(output);
        }

        [Theory]
        [InlineData("Hi [[FirstName]] [[LastName]]!", "12345678-DF26-4D03-8378-1B6501C109E9", "Fred", "Flintstone", "fred@flintstones.com", "Hi Fred Flintstone!")]
        [InlineData("The email is '[[EmailAddress]]' for Customer with Id: ([[Id]])", "abcdef00-1a29-44f4-b22b-7c509461503c", "Barney", "Rubble", "barney@rubbles.com", "The email is 'barney@rubbles.com' for Customer with Id: (abcdef00-1a29-44f4-b22b-7c509461503c)")]
        public void Given_Multiple_Placeholders_When_Call_Replace_Then_Replace_All_Placeholder_With_Property_Values(
            string input, string id, string firstName, string lastName, string emailAddress, string output)
        {
            var customer = new Customer(new Guid(id), firstName, lastName, emailAddress);
            var replacer = new PlaceholderReplacer();
            var result = replacer.Replace(input, customer);
            output.Should().Be(result);
        }
    }
}
