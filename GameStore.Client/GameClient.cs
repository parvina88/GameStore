using System.Net.Http.Json;
using GameStore.Client.Models;

namespace GameStore.Client;

public class GameClient
{
    private readonly HttpClient _httpClient;

    public GameClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private readonly List<Game> games =
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

    public async Task<Game[]?> GetGamesAsync()
    {
        return await _httpClient.GetFromJsonAsync<Game[]>("games");
    }

    public async Task AddGameaAsync(Game game)
    {
        await _httpClient.PostAsJsonAsync("games", game);
        
    }

    public async Task<Game> GetGameAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Game>($"games/{id}") 
                ?? throw new Exception("Could not find game!");
    }

    public async Task UpdateGameAsync(Game updatedGame)
    {
        await _httpClient.PutAsJsonAsync($"games/{updatedGame.Id}", updatedGame);
    }

    public async Task DeleteGameAsync(int id)
    {
        await _httpClient.DeleteAsync($"games/{id}");
    }
}
