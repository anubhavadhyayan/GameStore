using System;
using GameStore.Client.Models;
namespace GameStore.Client.Clients;

public class GenresClient(HttpClient httpClient)
{

    public async Task<Genre[]> GetGenresAsync()
        => await httpClient.GetFromJsonAsync<Genre[]>("genres") ?? [];
}
