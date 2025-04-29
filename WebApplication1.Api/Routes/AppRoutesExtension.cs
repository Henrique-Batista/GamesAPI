using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.API.Domain;
using WebApplication1.API.Repositories;

namespace WebApplication1.API.Routes;

public static class AppRoutesExtension
{
    public static void MapAppRoutes(this IEndpointRouteBuilder app)
    {
        var mapper = new TypeAdapterConfig();
        mapper.NewConfig<StudioRequestDto, Studio>()
            .UseDestinationValue(member => member.Type == typeof(Guid?))
            .AfterMapping((src,
                result,
                dst) =>
            {
                var nullableProperties = src.GetType()
                    .GetProperties()
                    .Where(prop =>
                        prop.GetValue(src) is null |
                        string.IsNullOrEmpty(prop.GetValue(src)
                            ?.ToString()));
                var propertyInfos = nullableProperties.ToList();
                if (propertyInfos.Count == 0)
                    return;
                throw new ArgumentNullException(
                    $"The following properties are null or empty: {string.Join(", ", propertyInfos.Select(p => p.Name))}");
            });
        mapper.ForType<Studio, StudioResponseDto>()
            .UseDestinationValue(member => member.Type == typeof(Guid?));
        mapper.ForType<GameRequestDto, Game>()
            .UseDestinationValue(member => member.Type == typeof(Guid?))
            .AfterMapping((src,
                result,
                dst) =>
            {
                var nullableProperties = src.GetType()
                    .GetProperties()
                    .Where(prop =>
                        prop.GetValue(src) is null |
                        string.IsNullOrEmpty(prop.GetValue(src)
                            ?.ToString()));
                var propertyInfos = nullableProperties.ToList();
                if (propertyInfos.Count == 0)
                    return;
                throw new ArgumentNullException(
                    $"The following properties are null or empty: {string.Join(", ", propertyInfos.Select(p => p.Name))}");
            });
        mapper.ForType<Game, GameResponseDto>()
            .UseDestinationValue(member => member.Type == typeof(Guid?));
        mapper.Compile();
        MapStudiosRoute(app,
            mapper);
        MapGamesRoute(app,
            mapper);
    }

    private static void MapStudiosRoute(IEndpointRouteBuilder app,
        TypeAdapterConfig mapper)
    {
        app.MapGet("/studios",
            [EndpointSummary("GetAllStudios")]
            [EndpointDescription("Get all Studios in Database")]
            [Produces<IEnumerable<StudioResponseDto>>]
            async (IRepository<Studio> repository) =>
            {
                IEnumerable<Studio> studios =
                [
                ];
                try
                {
                    studios = await repository.GetAllAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                var enumerable = studios.ToList();
                return enumerable.Any()
                    ? Results.Ok(enumerable.Select(std => std.Adapt<StudioResponseDto>(mapper))
                        .ToList())
                    : Results.NotFound("No studios found.");
            });
        app.MapGet("/studios/{id}",
            [Produces<StudioResponseDto>] async (Guid id,
                IRepository<Studio> repository) =>
            {
                Studio studio;
                try
                {
                    studio = await repository.GetByIdAsync(id);
                }
                catch (KeyNotFoundException)
                {
                    return Results.NotFound($"Studio with id {id} not found.");
                }

                return Results.Ok(studio.Adapt<StudioResponseDto>(mapper));
            });
        app.MapGet("/studios/{id}/games",
            [Produces<StudioResponseDto>] async (Guid id,
                IRepository<Studio> repository) =>
            {
                Studio? studio;
                try
                {
                    studio = await repository.GetByIdAsync(id);
                    studio.Games = studio.Games?.ToList();
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e);
                }
                return Results.Ok(studio.Adapt<StudioResponseDto>(mapper));
            });
        app.MapPost("/studios",
            [Produces<StudioResponseDto>] async (StudioRequestDto studio,
                IRepository<Studio> repository) =>
            {
                Studio? createdStudio;
                try
                {
                    createdStudio = await repository.AddAsync(studio.Adapt<Studio>(mapper));
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
                
                return Results.Created($"/studios/{createdStudio?.Id}",
                    createdStudio?.Adapt<StudioResponseDto>(mapper));
            });
        app.MapPut("/studios",
            [Produces<StudioResponseDto>] async (StudioRequestWithIdDto studio,
                IRepository<Studio> repository) =>
            {
                Studio? updatedStudio;
                try
                {
                    updatedStudio = await repository.UpdateAsync(studio.Adapt<Studio>(mapper));
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }

                return Results.Ok(updatedStudio.Adapt<StudioResponseDto>(mapper));
            });
        app.MapDelete("/studios/{id}",
            [Produces<NoContentResult>] async (Guid id,
                IRepository<Studio> repository) =>
            {
                bool deleted;
                try
                {
                    deleted = await repository.DeleteAsync(id);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }

                return deleted
                    ? Results.NoContent()
                    : Results.NotFound();
            });
    }

    private static void MapGamesRoute(IEndpointRouteBuilder app,
        TypeAdapterConfig mapper)
    {
        app.MapGet("/games",
            [Produces<GameResponseDto>] async (IRepository<Game> repository) =>
            {
                IEnumerable<Game> games;
                try
                {
                    games = await repository.GetAllAsync();
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }

                var enumerable = games.ToList();
                return enumerable.Any()
                    ? Results.Ok(enumerable.Select(game => game.Adapt<GameResponseDto>(mapper)))
                    : Results.NotFound("No games found.");
            });
        app.MapGet("/games/{id}",
            [Produces<GameResponseDto>] async (Guid id,
                IRepository<Game> repository) =>
            {
                Game game;
                try
                {
                    game = await repository.GetByIdAsync(id);
                }
                catch (KeyNotFoundException)
                {
                    return Results.NotFound($"Game with id {id} not found.");
                }

                return Results.Ok(game.Adapt<GameResponseDto>(mapper));
            });
        app.MapGet("/games/{id}/studio",
            [Produces<StudioResponseDto>] async (Guid id,
                IRepository<Game> repository) =>
            {
                Studio? studio;
                try
                {
                    studio = await repository.GetGameStudioAsync(id);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }

                return Results.Ok(studio.Adapt<StudioResponseDto>(mapper));
            });
        app.MapPost("/games",
            [Produces<GameResponseDto>] async (GameRequestDto game,
                IRepository<Game> repository) =>
            {
                Game? createdGame;
                try
                {
                    createdGame = await repository.AddAsync(game.Adapt<Game>(mapper));
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }

                return Results.Created($"/games/{createdGame.Id}",
                    createdGame.Adapt<GameResponseDto>(mapper));
            });
        app.MapPut("/games/{id}",
            [Produces<GameResponseDto>] async (GameRequestDto game,
                IRepository<Game> repository) =>
            {
                Game? updatedGame;
                try
                {
                    updatedGame = await repository.UpdateAsync(game.Adapt<Game>(mapper));
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }

                return Results.Ok(updatedGame.Adapt<StudioResponseDto>(mapper));
            });
        app.MapDelete("/games/{id}",
            [Produces<NoContentResult>] async (Guid id,
                IRepository<Game> repository) =>
            {
                bool? deleted;
                try
                {
                    deleted = await repository.DeleteAsync(id);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }

                return deleted.Value
                    ? Results.NoContent()
                    : Results.NotFound();
            });
    }
}