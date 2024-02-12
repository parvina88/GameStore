using GameStore.Server.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        List<Game> games =
            new()
            {
                new Game()
                {
                    Id = 1,
                    Name = "Minecraft",
                    Genre = "Family",
                    Price = 19.99m,
                    ReleaseDate = new DateTime(2016, 12, 10)
                },
                new Game()
                {
                    Id = 2,
                    Name = "Street fighter",
                    Genre = "Family",
                    Price = 59.99m,
                    ReleaseDate = new DateTime(2010, 12, 30)
                },
                new Game()
                {
                    Id = 3,
                    Name = "Fifa 2023",
                    Genre = "Sports",
                    Price = 69.99m,
                    ReleaseDate = new DateTime(2022, 8, 10)
                },
                new Game()
                {
                    Id = 4,
                    Name = "Final Fantasy XIV",
                    Genre = "Roleplaying",
                    Price = 59.99m,
                    ReleaseDate = new DateTime(2010, 9, 25)
                }
            };

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddCors(options => options.AddDefaultPolicy(builder => {
            builder.WithOrigins("http://localhost:5229")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            
        }));

        var app = builder.Build();

        app.UseCors();

        var group = app.MapGroup("/games")
                        .WithParameterValidation();

        // Get /games
        group.MapGet("/", () => games);

        // Get /games/{id}
        group
            .MapGet(
                "/{id}",
                (int id) =>
                {
                    Game? game = games.Find(games => games.Id == id);

                    if (game is null)
                    {
                        return Results.NotFound();
                    }

                    return Results.Ok(game);
                }
            )
            .WithName("GetGame");

        // POST /games
        group
            .MapPost(
                "/",
                (Game game) =>
                {
                    game.Id = games.Max(g => g.Id) + 1;
                    games.Add(game);

                    return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game);
                }
            );

        // PUT /games/id
        group.MapPut(
            "/{id}",
            (int id, Game updatedGame) =>
            {
                Game? existingGame = games.Find(g => g.Id == id);

                if (existingGame is null)
                {
                    updatedGame.Id = id;
                    games.Add(updatedGame);
                    return Results.CreatedAtRoute(
                        "GetGame",
                        new { id = updatedGame.Id },
                        updatedGame
                    );
                }

                existingGame.Name = updatedGame.Name;
                existingGame.Genre = updatedGame.Genre;
                existingGame.Price = updatedGame.Price;
                existingGame.ReleaseDate = updatedGame.ReleaseDate;

                return Results.NoContent();
            }
        );

        // DELETE /games/{id}
        group.MapDelete(
            "/{id}",
            (int id) =>
            {
                Game? game = games.Find(g => g.Id == id);

                if (game is null)
                {
                    return Results.NotFound();
                }
                games.Remove(game);

                return Results.NoContent();
            }
        );

        app.Run();
    }
}
