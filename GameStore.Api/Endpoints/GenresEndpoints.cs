using System;
using GameStore.Api.Data;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenresEndpoints
{
    public static RouteGroupBuilder MapGenresEndpoints(this WebApplication webApplication)
    {
        var group = webApplication.MapGroup("genres");
        group.MapGet("/", async (GameStoreContext dbContext) =>
        await dbContext.Genres.Select(genre => genre.ToDto())
        .AsNoTracking()
        .ToListAsync());

        return group;
    }
}
