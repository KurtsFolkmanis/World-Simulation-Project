
using WorldSim.Data;
using WorldSim.Domain;

namespace WorldSim.Presentation;
class Screen() {
    public static World StartMenu(SaveLoadManager fileManager) {
        Console.Clear();
        var world = new World();

        while (true) {
            Console.WriteLine($"1: Create Default World\n" +
                              $"2: Create Custom World\n" +
                              $"3: Load World from File");

            int? choice = GetButton();
            if (choice == null) { continue; }

            switch (choice) {
                case 1:
                    world.WorldStart();
                    return world;
                case 2:
                    return WorldCreation();
                case 3:
                    Console.Clear();
                    world = fileManager.Load()!;
                    if (world == null) break;
                    return world;
                default:
                    Console.Clear();
                    Console.WriteLine("Choice out of range. Please try again.");
                    break;
            }
        }
    }

    public static void Menu(World world) {
        Console.Clear();

        while (true) {
            NationLookUp(world);
            Console.WriteLine();
            SettlementLookUp(world);
            Console.WriteLine();

            Console.WriteLine($"1: Look at Nations\n" +
                              $"2: Look at Settlements\n" +
                              $"3: Exit the Simulation");

            int? choice = GetButton();
            if (choice == null) { continue; }

            switch (choice) {
                case 1:
                    Console.Clear();
                    continue;
                case 2:
                    Console.Clear();
                    continue;
                case 3:
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Choice out of range. Please try again.");
                    break;
            }
        }
    }

    public static World WorldCreation() {
        var world = new World();

        while (true) {
            Console.Clear();

            NationLookUp(world);
            Console.WriteLine();
            SettlementLookUp(world);
            Console.WriteLine();

            Console.WriteLine($"1:Create Nation\n" +
                              $"2:Create Settlements\n" +
                              $"3:Change Settlement Nation\n" +
                              $"4:Exit Creation");

            int? choice = GetButton();
            if (choice == null) { continue; }

            string? name = "";
            switch (choice) {
                case 1:
                    Console.Clear();
                    Console.Write("Type your nation name (or skip for random): ");
                    name = Console.ReadLine();
                    world.AddNation(Nation.CreateNation(name!));
                    continue;

                case 2:
                    Console.Clear();
                    Console.Write("Type your settlement name (or skip for random): ");
                    name = Console.ReadLine();
                    world.AddSettlement(Settlement.CreateSettlement(name!));
                    continue;

                case 3:
                    Console.Clear();
                    ChangeSettlementNation(world);
                    continue;

                case 4:
                    return world;

                default:
                    Console.Clear();
                    Console.WriteLine("Choice out of range. Try again.");
                    break;
            }
        }
    }

    public static void NationLookUp(World world) {
        Console.WriteLine($"Nation List:");
        foreach (var nation in world.Nations) {
            Console.WriteLine($"[Id:{nation.Id}] [Name:{nation.Name}]");
        }
    }

    public static void SettlementLookUp(World world) {
        Console.WriteLine($"Settlement List:");
        foreach (var settlement in world.Settlements) {
            string nationName = settlement.Nation != null ? settlement.Nation.Name : "None";
            Console.WriteLine($"[Id:{settlement.Id}] [Nation:{nationName}], [Name:{settlement.Name}]");
        }
    }

    public static int GetButton() {
        while (true) {
            var input = Console.ReadLine();
            if (!int.TryParse(input, out var choice)) {
                Console.Clear();
                Console.WriteLine("Invalid input — please enter the number of your choice.");
            }
            return choice;
        }
    }

    public static void ChangeSettlementNation(World world) {
        Settlement settlement;
        Nation nation;

        Console.Clear();
        do {
            try {
                SettlementLookUp(world);
                Console.Write("Type Settlement ID: ");
                settlement = world.GetSettlement(GetButton());
                break;
            } catch (Exception ex) {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
        }while (true);

        Console.Clear();
        do {
            try {
                NationLookUp(world);
                Console.Write("Type Nation ID: ");
                nation = world.GetNation(GetButton());
                break;
            } catch (Exception ex) {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
        } while (true);

        world.ChangeSettlementNation(settlement, nation);
        return;
    }
}
