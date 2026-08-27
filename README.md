### Temporary Notes API



A small REST API for creating and sharing temporary notes.

Notes can optionally be protected with a password and configured with an expiration time or maximum number of views. Notes can also be updated or deleted.



##### Tech Stack



* .NET 10
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* MediatR
* xUnit
* Moq
* Swagger / OpenAPI





##### Features



* Create a note
* Generate a unique shareable note code
* Optional password protection
* Optional expiration time
* Optional maximum view count
* Retrieve public notes
* Unlock password-protected notes
* Update notes
* Delete notes
* Soft deletion
* Passwords stored as hashes
* Atomic view-count increment
* Server-side validation
* Database constraints
* Automated unit tests
* Swagger for API testing


##### 

##### Database Setup



The application uses SQL Server with Entity Framework Core.

Update the connection string in:



TemporaryNotes.Api/appsettings.json



Example:



{
"ConnectionStrings": {
"DefaultConnection": "Server=localhost;Database=TemporaryNotesDb;Trusted\_Connection=True;TrustServerCertificate=True"
}
}



Apply the existing migrations:



dotnet ef database update



If the EF CLI tool is not installed:



dotnet tool install --global dotnet-ef





##### Running the Application



Clone the repository and navigate to the solution directory.



Restore dependencies:



dotnet restore



Build the solution:



dotnet build



Apply database migrations:



dotnet ef database update



Run the API:



dotnet run --project TemporaryNotes.Api



Swagger is available at:



https://localhost:<port>/swagger



The exact port depends on the local launch configuration.



##### 

##### API Endpoints



##### Create a note



POST /api/Notes



Example request:



{
"content": "This is my private note.",
"expiresInMinutes": 60,
"maxViews": 5,
"password": "secret123"
}



Example response:



{
"code": "55dd042d1814",
"url": "/api/notes/55dd042d1814",
"expiresAt": "2026-08-27T18:00:00Z",
"maxViews": 5
}



##### Get a note



GET /api/Notes/{code}



For a public note, the content is returned.

For a password-protected note:



{
"requiresPassword": true
}



The password-protected note does not expose its content through the normal GET endpoint.



##### Unlock a note



POST /api/Notes/{code}/unlock



Example request:

{
"password": "secret123"
}



A successful request returns the note content.

Incorrect passwords do not consume a view.





##### Update a note



PUT /api/Notes/{code}



Example request:



{
"content": "Updated private note",
"expiresInMinutes": 60,
"maxViews": 5,
"password": "secret123",
"newPassword": "newsecret123"
}



For password-protected notes, the current password must be provided before updating the note.

`newPassword` is optional and can be used to change the password.

### Delete a note



DELETE /api/Notes/{code}



For password-protected notes, the password can be provided as a query parameter:



DELETE /api/Notes/{code}?password=secret123



A successful deletion returns:



204 No Content



##### Note Access Rules



A note cannot be accessed when:

* It does not exist
* It has been deleted
* It has expired
* Its maximum view count has been reached
* A required password is missing
* An incorrect password is provided

Only successful content retrieval increments the view count.

Incorrect passwords and failed requests do not consume views.



##### Password Security



Passwords are never stored as plain text.

The application uses ASP.NET Core `IPasswordHasher<Notes>` to hash and verify passwords.

The stored database value is the password hash rather than the original password.



##### View Count



View limits are enforced using an atomic database update.

This prevents two concurrent requests from both successfully accessing a note when only one view remains.

The view count is increased only after the note has passed all validation, expiration, deletion and password checks.



##### Expiration

Expiration is checked using UTC time.

Once a note has expired, it cannot be accessed or reactivated through the update functionality.



##### Deletion Strategy



The application uses soft deletion.

Instead of permanently removing the database record, `DeletedAt` is populated with the deletion timestamp.

This keeps the record in the database while preventing further access.

This approach also preserves useful information for potential auditing or future maintenance.



##### Validation and Database Constraints



Validation is performed on the server side.

The database also contains constraints for important data rules:

* Note code is unique
* Content has a maximum length of 1000 characters
* View count cannot be negative
* Maximum views must be greater than zero when specified

This provides an additional layer of protection instead of relying only on application-level validation.



##### Testing

The solution contains automated unit tests covering important business rules.

Run the tests with:



dotnet test



The tests focus on scenarios such as:

* Expired notes
* Deleted notes
* View limits
* Password-protected notes
* Successful note retrieval



##### Design Decisions



The application uses a simple separation between:

* API layer for HTTP requests and responses
* Application layer for business logic and MediatR handlers
* Domain layer for entities
* Infrastructure layer for EF Core and database access

MediatR keeps controller actions small and moves the main business logic into commands and queries.

The implementation intentionally keeps the architecture simple because the service is small and does not require unnecessary abstractions.



##### Error Handling



The API uses standard HTTP status codes, for example:

* `200 OK` – successful request
* `204 No Content` – successful deletion
* `400 Bad Request` – invalid input
* `401 Unauthorized` – incorrect or missing password for protected operations
* `404 Not Found` – note does not exist or is no longer accessible

Swagger/OpenAPI is enabled to make the API easy to test and explore.



##### Running Tests

From the solution directory:



dotnet test



