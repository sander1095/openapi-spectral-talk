using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace UsersApi;

public static class UsersEndpoints
{
    private static readonly List<User> _users =
    [
        new User(1, "John Doe"),
        new User(2, "Jane Smith")
    ];

    private static int _nextId = 3;

    public static void MapUsersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/users")
            .WithTags("Users");

        group.MapGet("/", GetAllUsers)
            .WithName("GetAllUsers")
            .WithSummary("Get all users")
            .WithDescription("Returns a list of all users in the system.");

        group.MapGet("/{id:int}", GetUserById)
            .WithName("GetUserById")
            .WithSummary("Get a user by ID")
            .WithDescription("Returns a single user by their ID.");

        group.MapPost("/", CreateUser)
            .WithName("CreateUser")
            .WithSummary("Create a new user")
            .WithDescription("Creates a new user with the provided details.");

        group.MapPut("/{id:int}", UpdateUser)
            .WithName("UpdateUser")
            .WithSummary("Update an existing user")
            .WithDescription("Updates an existing user's details.");

        group.MapDelete("/{id:int}", DeleteUser)
            .WithName("DeleteUser")
            .WithSummary("Delete a user")
            .WithDescription("Deletes a user from the system.");
    }

    /// <summary>
    /// Returns all users.
    /// </summary>
    private static Ok<List<User>> GetAllUsers()
    {
        return TypedResults.Ok(_users);
    }

    /// <summary>
    /// Returns a user by ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    private static Results<Ok<User>, NotFound<ProblemDetails>> GetUserById(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);

        if (user is null)
        {
            return TypedResults.NotFound(new ProblemDetails
            {
                Title = "User not found",
                Detail = $"User with ID {id} was not found.",
                Status = StatusCodes.Status404NotFound
            });
        }

        return TypedResults.Ok(user);
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">The user creation request.</param>
    private static Results<Created<User>, BadRequest<ProblemDetails>> CreateUser(CreateUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return TypedResults.BadRequest(new ProblemDetails
            {
                Title = "Invalid request",
                Detail = "Name is a required field.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var user = new User(_nextId++, request.Name);
        _users.Add(user);

        return TypedResults.Created($"/users/{user.Id}", user);
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="request">The user update request.</param>
    private static Results<Ok<User>, NotFound<ProblemDetails>, BadRequest<ProblemDetails>> UpdateUser(int id, UpdateUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return TypedResults.BadRequest(new ProblemDetails
            {
                Title = "Invalid request",
                Detail = "Name is a required field.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var existingUserIndex = _users.FindIndex(u => u.Id == id);

        if (existingUserIndex == -1)
        {
            return TypedResults.NotFound(new ProblemDetails
            {
                Title = "User not found",
                Detail = $"User with ID {id} was not found.",
                Status = StatusCodes.Status404NotFound
            });
        }

        var updatedUser = new User(id, request.Name);
        _users[existingUserIndex] = updatedUser;

        return TypedResults.Ok(updatedUser);
    }

    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    private static Results<NoContent, NotFound<ProblemDetails>> DeleteUser(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);

        if (user is null)
        {
            return TypedResults.NotFound(new ProblemDetails
            {
                Title = "User not found",
                Detail = $"User with ID {id} was not found.",
                Status = StatusCodes.Status404NotFound
            });
        }

        _users.Remove(user);
        return TypedResults.NoContent();
    }
}
