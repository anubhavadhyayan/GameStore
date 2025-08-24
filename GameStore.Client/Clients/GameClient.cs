using System;
using GameStore.Client.Models;
namespace GameStore.Client.Clients;

public class GameClient(HttpClient httpClient)
{
    
    public async Task<GameSummary[]> GetGamesAsync()
        => await httpClient.GetFromJsonAsync<GameSummary[]>("games") ?? [];

    public async Task AddGameAsync(GameDetails gameDetails) =>
        await httpClient.PostAsJsonAsync("games", gameDetails);

    public async Task UpdateGameAsync(GameDetails updatedGame)
        => await httpClient.PutAsJsonAsync($"games/{updatedGame.Id}", updatedGame);


    public async Task<GameDetails> GetGameAsync(int id)
        => await httpClient.GetFromJsonAsync<GameDetails>($"games/{id}")
        ?? throw new Exception("Could not find your game!");


    public async Task DeleteGameAsync(int id)
        => await httpClient.DeleteAsync($"games/{id}");
}
