using System.Text.Json;
using WorldSim.Domain;


namespace WorldSim.Data;
class SaveLoadManager {
    string fileName = "World.json";

    public void Save(World world) {
        var options = new JsonSerializerOptions { WriteIndented = true };

        using FileStream fileStream = File.Create(fileName);
        JsonSerializer.Serialize(fileStream, world, options);
        Console.WriteLine(fileStream.Name);
    }

    public World? Load() {
        try {
            string jsonFromFile = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<World>(jsonFromFile)!;
        } catch (Exception ex) {
            Console.WriteLine($"Failed to load world: {ex.Message}");
            return null;
        }

    }
}

