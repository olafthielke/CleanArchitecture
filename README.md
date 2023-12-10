# CleanArchitecture
A slice of Clean Architecture

I recommend you check out the general simplicity of the code, especially in the Business Logic project.


# GETTING STARTED:

1. Clone the repo to your computer.
   $ git clone https://github.com/olafthielke/CleanArchitecture.git

2. Set the docker-compose project as the Startup Project.

3. At a terminal, run command 'docker compose up --build'

4. Once the Web Api and Postgres containers are up and running - which will take a little while on the first time - open your browser navigate to https://localhost:8081/customers. You should see the JSON data for one customer record, Fred Flintstone.

5. You can now use a tool like Postman (https://www.postman.com/) to POST JSON data to https://localhost:44365/customers/register. 
   The POST'd data should be of the form:
		{
			"firstName" : "Barney",
			"lastName" : "Rubble",
			"emailAddress" : "barney@rubbles.rock"
		}

6. Assuming there were no errors, when you refresh the https://localhost:8081/customers browser window you should see your new customer appear in the list. 

7. It's working as expected! DONE!



NOTE: I haven't got docker-compose support for SQL Server yet. 

- If you want to try SQL Server as a persistence mechanism, then open and run the CreateDatabase.sql script in the Data.SqlServer.Specific project.

- Try and build the solution.

- Set Presentation.WebApi as the solution's start up project.

- In the Presentation.WebApi project, ensure that only the 1. option is uncommented. 
   The line will be 
		services.AddSingleton<ICustomerRepository, InMemoryCustomerDatabase>(); 
   and it will enable the in-memory database option only. This is an easy way to get started.
   
- Run the WebApi project using the IIS Express option. A browser window should pop up for address https://localhost:44365/customers. You should be able to see an empty array denoting meaning there are no customers in the in-memory database. 

- The POST should succeed and if you refresh the originial browser window pointed at https://localhost:44365/customers you'll see the new record in the in-memory database.

Great! Your Clean Architecture Web API is working.


# NOTES/RECOMMENDATIONS

I recommend you place breakpoints in the CustomerController.Register() method and post the same customer info again from Postman and follow the request. You should see the call entering the RegisterCustomerUseCase.RegisterCustomer() method. This is where the business logic workflow is executed. Please note that RegisterCustomerUseCase is entirely independent of any mechanisms: Web, Database, or anything else. Isn't that cool? It just knows WHAT must happen, but not in fine detail (the HOW).

To demonstrate the extreme pluggability of this solution, in the WebApi project's Startup.cs file comment out the 1. configuration settings and uncomment the ones for 2., saving and retrieving customer data to and from a JSON file. All changed over only using configuration!

Feel free to try out all 6 modular and pluggable data access option. 

Note: I hope the Redis option works for you. If it doesn't you may need to install an updated StackExchange Redis client.

Good luck!

PS: Please let me know what you think.