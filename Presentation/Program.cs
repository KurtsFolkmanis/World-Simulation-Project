using WorldSim.Data;

namespace WorldSim.Presentation;
class Program {
    // Default number of turns to simulate when none is specified
    private const int DefaultTurnCount = 1337;

    public static Task Main(string[] args) {
        Console.ForegroundColor = ConsoleColor.Yellow;

        SaveLoadManager fileManager = new();
        var world = Screen.StartMenu(fileManager);

        for (int i = 0; i < DefaultTurnCount; i++) {
            world.Turn();
        }
        Screen.Menu(world);

        fileManager.Save(world);
        return Task.CompletedTask;
    }
}
