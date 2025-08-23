using System;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GameEndpoints
{
    const string GetGameEndPoint = "GetGame";

    /*private static readonly List<GameSummaryDto> gameDtos = [
        new (1, "Street Fighter", "Fighting", 199.99M, new DateOnly(1999, 7, 21)),
    new (2, "Counter Strike", "Fighting", 59.99M, new DateOnly(2000, 7, 25)),
    new (3, "GTA", "Roleplaying", 100.99M, new DateOnly(2006, 8, 21)),
    new (4, "Fifa", "Sports", 109.99M, new DateOnly(2008, 7, 21))
    ];*/

    public static RouteGroupBuilder MapGamesEndPoints(this WebApplication webApplication)
    {
        var group = webApplication.MapGroup("games").WithParameterValidation();

        //GET /games
        group.MapGet("/", async (GameStoreContext dbContext) => await dbContext.Games.
        Include(game => game.Genre)
        .Select(game => game.ToGameSummaryDto())
        .AsNoTracking()
        .ToListAsync());

        //GET /games/1
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            // GameDto? game = gameDtos.Find(game => game.Id == id);
            Game? game = await dbContext.Games.FindAsync(id);
            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        }
        ).
        WithName(GetGameEndPoint);

        //POST /games
        group.MapPost("/", async (CreateGameDto newgame, GameStoreContext dbContext) =>
        {
            // if (string.IsNullOrEmpty(newgame.Name))
            // {
            //     return Results.BadRequest("Name is Required");
            // }
            // GameDto game = new(gameDtos.Count + 1, newgame.Name, newgame.Genre, newgame.Price, newgame.ReleaseDate);
            // gameDtos.Add(game);

            /*Game game = new()
            {
                Name = newgame.Name,
                Genre = dbContext.Genres.Find(newgame.GenreId),
                GenreId = newgame.GenreId,
                Price = newgame.Price,
                ReleaseDate = newgame.ReleaseDate,
             };*/
            Game game = newgame.ToEntity();

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            /*GameDto gameDto = new(
                game.Id,
                game.Name,
                game.Genre!.Name,
                game.Price,
                game.ReleaseDate
            );*/
            return Results.CreatedAtRoute(GetGameEndPoint,
            new { id = game.Id }, game.ToGameDetailsDto());
        }
        ).WithParameterValidation();

        //PUT /games
        group.MapPut("/{id}", async (int id, UpdateGameDto updategame, GameStoreContext dbContext) =>
        {
            /*
            var index = gameDtos.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            gameDtos[index] = new GameSummaryDto(
                id,
                updategame.Name,
                updategame.Genre,
                updategame.Price,
                updategame.ReleaseDate
                );
            return Results.NoContent(); */

            var existingGame = await dbContext.Games.FindAsync(id);
            if (existingGame is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingGame).CurrentValues.SetValues(updategame.ToEntity(id));
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });

        //DELETE
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();
            // gameDtos.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        return group;

    }

}
