using System;
using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GameEndpoints
{
    const string GetGameEndPoint = "GetGame";

    private static readonly List<GameDto> gameDtos = [
        new (1, "Street Fighter", "Fighting", 199.99M, new DateOnly(1999, 7, 21)),
    new (2, "Counter Strike", "Fighting", 59.99M, new DateOnly(2000, 7, 25)),
    new (3, "GTA", "Roleplaying", 100.99M, new DateOnly(2006, 8, 21)),
    new (4, "Fifa", "Sports", 109.99M, new DateOnly(2008, 7, 21))
    ];

    public static RouteGroupBuilder MapGamesEndPoints(this WebApplication webApplication)
    {
        var group = webApplication.MapGroup("games").WithParameterValidation();

        //GET /games
        group.MapGet("/", () => gameDtos);

        //GET /games/1
        group.MapGet("/{id}", (int id) =>
        {
            GameDto? game = gameDtos.Find(game => game.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        }
        ).
        WithName(GetGameEndPoint);

        //POST /games
        group.MapPost("/", (CreateGameDto newgame) =>
        {
            // if (string.IsNullOrEmpty(newgame.Name))
            // {
            //     return Results.BadRequest("Name is Required");
            // }
            GameDto game = new(gameDtos.Count + 1, newgame.Name, newgame.Genre, newgame.Price, newgame.ReleaseDate);
            gameDtos.Add(game);

            return Results.CreatedAtRoute(GetGameEndPoint, new { id = game.Id }, game);
        }
        ).WithParameterValidation();

        //PUT /games
        group.MapPut("/{id}", (int id, UpdateGameDto updategame) =>
        {
            var index = gameDtos.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            gameDtos[index] = new GameDto(
                id,
                updategame.Name,
                updategame.Genre,
                updategame.Price,
                updategame.ReleaseDate
                );
            return Results.NoContent();
        });

        //DELETE
        group.MapDelete("/{id}", (int id) =>
        {
            gameDtos.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        return group;

    }

}
