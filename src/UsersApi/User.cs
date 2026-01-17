namespace UsersApi;

/// <summary>
/// Represents a user in the system.
/// </summary>
public record User(int Id, string Name);

/// <summary>
/// Request to create a new user.
/// </summary>
public record CreateUserRequest(string Name);

/// <summary>
/// Request to update an existing user.
/// </summary>
public record UpdateUserRequest(string Name);
