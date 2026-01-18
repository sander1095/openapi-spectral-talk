namespace DinosaursApi;

/// <summary>
/// Represents a dinosaur in the system.
/// </summary>
public record Dinosaur(int Id, string Name, string Species);

/// <summary>
/// Request to create a new dinosaur.
/// </summary>
public record CreateDinosaurRequest(string Name, string Species);

/// <summary>
/// Request to update an existing dinosaur.
/// </summary>
public record UpdateDinosaurRequest(string Name, string Species);
