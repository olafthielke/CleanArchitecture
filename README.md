# CleanArchitecture
A slice of Clean Architecture


# GETTING STARTED:

1. Clone the repo to your computer.
   $ git clone https://github.com/olafthielke/CleanArchitecture.git

2. If you want to try SQL Server as a persistence mechanism, then open and run the CreateDatabase.sql script in the Data.SqlServer.Specific project.

3. Try and build the solution.

4. Set Presentation.WebApi as the solution's start up project.

5. In the Presentation.WebApi project, ensure that only the 1. option is uncommented. 
   The line will be 
		services.AddSingleton<ICustomerRepository, InMemoryCustomerDatabase>(); 
   and it will enable the in-memory database option only. This is an easy way to get started.
   
6. Run the WebApi project using the IIS Express option. A browser window should pop up for address https://localhost:44365/customers. You should be able to see an empty array denoting meaning there are no customers in the in-memory database. 

7. Use a tool like Postman (https://www.postman.com/) to POST JSON data to https://localhost:44365/customers/register. 
   The posted data should be in the form:
		{
			"firstName" : "Fred",
			"lastName" : "Flintstone",
			"emailAddress" : "fred@flintstones.net"
		}

8. The POST should succeed and if you refresh the originial browser window pointed at https://localhost:44365/customers you'll see the new record in the in-memory database.

Great! Your Clean Architecture Web API is working.


# NOTES/RECOMMENDATIONS

I recommend you place breakpoints in the CustomerController.Register() method and post the same customer info again from Postman and follow the request. You should see the call entering the RegisterCustomerUseCase.RegisterCustomer() method. This is where the business logic workflow is executed. Please note that RegisterCustomerUseCase is entirely independent of any mechanisms: Web, Database, or anything else. Isn't that cool? It just knows WHAT must happen, but not in fine detail (the HOW).

To demonstrate the extreme pluggability of this solution, in the WebApi project's Startup.cs file comment out the 1. configuration settings and uncomment the ones for 2., saving and retrieving customer data to and from a JSON file. All changed over only using configuration!

Feel free to try out all 6 modular and pluggable data access option. 

Note: I hope the Redis option works for you. If it doesn't you may need to install an updated StackExchange Redis client.

Good luck!

PS: Please let me know what you think.