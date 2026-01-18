using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DinosaursApi;

public static class DinosaursEndpoints
{
    private static readonly List<Dinosaur> _dinosaurs =
    [
        new Dinosaur(1, "Rexy", "Tyrannosaurus Rex"),
        new Dinosaur(2, "Blue", "Velociraptor")
    ];

    private static int _nextId = 3;

    public static void MapDinosaursEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/dinosaurs")
            .WithTags("Dinosaurs");

        group.MapGet("/", GetAllDinosaurs)

            .WithSummary("Get all dinosaurs")
            .WithDescription("Returns a list of all dinosaurs in the system.");

        group.MapGet("/{id:int}", GetDinosaurById)
            .WithName("GetDinosaurById")
            .WithSummary("Get a dinosaur by ID")
            .WithDescription("Returns a single dinosaur by their ID.");

        group.MapPost("/", CreateDinosaur)
            .WithName("CreateDinosaur")
            .WithSummary("Create a new dinosaur")
            .WithDescription("Creates a new dinosaur with the provided details.");

        group.MapPut("/{id:int}", UpdateDinosaur)
            .WithName("UpdateDinosaur")
            .WithSummary("Update an existing dinosaur")
            .WithDescription("Updates an existing dinosaur's details.");

        group.MapDelete("/{id:int}", DeleteDinosaur)
            .WithName("DeleteDinosaur")
            .WithSummary("Delete a dinosaur")
            .WithDescription("Deletes a dinosaur from the system.");
    }

    /// <summary>
    /// Returns all dinosaurs.
    /// </summary>
    private static Ok<List<Dinosaur>> GetAllDinosaurs()
    {
        return TypedResults.Ok(_dinosaurs);
    }

    /// <summary>
    /// Returns a dinosaur by ID.
    /// </summary>
    /// <param name="id">The ID of the dinosaur to retrieve.</param>
    private static Results<Ok<Dinosaur>, NotFound<ProblemDetails>> GetDinosaurById(int id)
    {
        var dinosaur = _dinosaurs.FirstOrDefault(d => d.Id == id);

        if (dinosaur is null)
        {
            return TypedResults.NotFound(new ProblemDetails
            {
                Title = "Dinosaur not found",
                Detail = $"Dinosaur with ID {id} was not found.",
                Status = StatusCodes.Status404NotFound
            });
        }

        return TypedResults.Ok(dinosaur);
    }

    /// <summary>
    /// Creates a new dinosaur.
    /// </summary>
    /// <param name="request">The dinosaur creation request.</param>
    private static Results<Created<Dinosaur>, BadRequest<ProblemDetails>, Conflict<ProblemDetails>> CreateDinosaur(CreateDinosaurRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Species))
        {
            return TypedResults.BadRequest(new ProblemDetails
            {
                Title = "Invalid request",
                Detail = "Name and Species are required fields.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        if (_dinosaurs.Any(d => d.Species.Equals(request.Species, StringComparison.OrdinalIgnoreCase) && d.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)))
        {
            return TypedResults.Conflict(new ProblemDetails
            {
                Title = "Dinosaur already exists",
                Detail = $"A dinosaur with name '{request.Name}' and species '{request.Species}' already exists.",
                Status = StatusCodes.Status409Conflict
            });
        }

        var dinosaur = new Dinosaur(_nextId++, request.Name, request.Species);
        _dinosaurs.Add(dinosaur);

        return TypedResults.Created($"/dinosaurs/{dinosaur.Id}", dinosaur);
    }

    /// <summary>
    /// Updates an existing dinosaur.
    /// </summary>
    /// <param name="id">The ID of the dinosaur to update.</param>
    /// <param name="request">The dinosaur update request.</param>
    private static Results<Ok<Dinosaur>, NotFound<ProblemDetails>, BadRequest<ProblemDetails>, Conflict<ProblemDetails>> UpdateDinosaur(int id, UpdateDinosaurRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Species))
        {
            return TypedResults.BadRequest(new ProblemDetails
            {
                Title = "Invalid request",
                Detail = "Name and Species are required fields.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var existingDinosaurIndex = _dinosaurs.FindIndex(d => d.Id == id);

        if (existingDinosaurIndex == -1)
        {
            return TypedResults.NotFound(new ProblemDetails
            {
                Title = "Dinosaur not found",
                Detail = $"Dinosaur with ID {id} was not found.",
                Status = StatusCodes.Status404NotFound
            });
        }

        if (_dinosaurs.Any(d => d.Id != id && d.Species.Equals(request.Species, StringComparison.OrdinalIgnoreCase) && d.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)))
        {
            return TypedResults.Conflict(new ProblemDetails
            {
                Title = "Dinosaur already exists",
                Detail = $"A dinosaur with name '{request.Name}' and species '{request.Species}' already exists.",
                Status = StatusCodes.Status409Conflict
            });
        }

        var updatedDinosaur = new Dinosaur(id, request.Name, request.Species);
        _dinosaurs[existingDinosaurIndex] = updatedDinosaur;

        return TypedResults.Ok(updatedDinosaur);
    }

    /// <summary>
    /// Deletes a dinosaur.
    /// </summary>
    /// <param name="id">The ID of the dinosaur to delete.</param>
    private static Results<NoContent, NotFound<ProblemDetails>> DeleteDinosaur(int id)
    {
        var dinosaur = _dinosaurs.FirstOrDefault(d => d.Id == id);

        if (dinosaur is null)
        {
            return TypedResults.NotFound(new ProblemDetails
            {
                Title = "Dinosaur not found",
                Detail = $"Dinosaur with ID {id} was not found.",
                Status = StatusCodes.Status404NotFound
            });
        }

        _dinosaurs.Remove(dinosaur);
        return TypedResults.NoContent();
    }
}
