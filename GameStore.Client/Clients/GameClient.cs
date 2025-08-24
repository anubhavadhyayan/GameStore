using System;
using GameStore.Client.Models;
namespace GameStore.Client.Clients;

public class GameClient
{
    private List<GameSummary> games = [
        new() {
            Id = 1,
            Name = "Street Fighter II",
            Genre = "Fighting",
            Price = 19.99M,
            ReleaseDate = new DateOnly(1998, 8, 31)
        },
        new() {
            Id = 2,
            Name = "Street Racer II",
            Genre = "Racing",
            Price = 39.99M,
            ReleaseDate = new DateOnly(1999, 7, 11)
        },
        new() {
            Id = 3,
            Name = "Minecraft",
            Genre = "Kids and Family",
            Price = 49.99M,
            ReleaseDate = new DateOnly(2008, 10, 25)
        }
    ];
    public GameSummary[] GetGames() => [.. games];

    private readonly Genre[] genres = new GenresClient().GetGenres();

    public void AddGame(GameDetails gameDetails)
    {
        Genre genre = GetGenreById(gameDetails.GenreId);

        var gameSummary = new GameSummary
        {
            Id = games.Count + 1,
            Name = gameDetails.Name,
            Genre = genre.Name,
            Price = gameDetails.Price,
            ReleaseDate = gameDetails.ReleaseDate,
        };
        games.Add(gameSummary);
    }

    private Genre GetGenreById(string? id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        return genres.Single(genre => genre.Id == int.Parse(id));
    }

    public void UpdateGame(GameDetails updatedGame)
    {
        var genre = GetGenreById(updatedGame.GenreId);
        GameSummary existingGame = GetGameSummaryById(updatedGame.Id);
        existingGame.Name = updatedGame.Name;
        existingGame.Genre = genre.Name;
        existingGame.Price = updatedGame.Price;
        existingGame.ReleaseDate = updatedGame.ReleaseDate;
    }

    public GameDetails GetGame(int id)
    {
        GameSummary game = GetGameSummaryById(id);

        var genre = genres.Single(genre => string.Equals(
            genre.Name,
            game.Genre,
            StringComparison.OrdinalIgnoreCase));

        return new GameDetails
        {
            Id = game.Id,
            Name = game.Name,
            GenreId = genre.Id.ToString(),
            Price = game.Price,
            ReleaseDate = game.ReleaseDate,
        };

    }

    private GameSummary GetGameSummaryById(int id)
    {
        GameSummary? game = games.Find(game => game.Id == id);
        ArgumentNullException.ThrowIfNull(game);
        return game;
    }

    public void DeleteGame(int id)
    {
        var game = GetGameSummaryById(id);
        games.Remove(game);
    }
}
