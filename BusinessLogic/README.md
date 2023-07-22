BusinessLogic Project


Contains the business logic of our application, which is all about WHAT our system does, 
not HOW it does it. In here you'll find code concerned with two innermost circles (entities, 
use cases) of the Clean Architecture (CA) diagram.

The 'Interfaces' folder provides
	- access to mechanisms like data persistence & notification, and
	- abstractions to the use cases used by the application (WebApi, ConsoleApp)

The 'Exceptions' folder contains
	- SPECIFIC exceptions, pertaining to specific problem situations (e.g. MissingEmailAddress), and
	- ABSTRACT exceptions, that encompass a category of problems (e.g. ClientInputException).

The 'Services' folder holds
	- Simple default implementations for the ICustomerRepository interface:
	- An in-memory customer database (basically a list of customers), and
	- An implementation that reads/writes to a JSON file. 

Furthermore,
	- In this CA system, we THROW SPECIFIC exceptions and CATCH & HANDLE ABSTRACT exceptions.
	  For example, we throw a MissingEmailAddress exception, and catch that as a ClientInputException
	  at the application boundary (Here the controller action). 
	- In this way, ALL our custom exceptions get handled in the correct way without us having to worry 
	  about unhandled application exceptions.
	- All specific exceptions must derive from an abstract exception for the exception handling to work.

Notice how this project has NO DEPENDENCIES on other projects. It's an indication that this project lives 
at the centre of CA and other circles depend on it (Interface Adapters).

