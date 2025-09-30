
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
                    continue;
                case 2:
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
        Console.Clear();
        var world = new World();

        while (true) {
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

            switch (choice) {
                case 1:
                    world.Nations.Add(CreateNation(world));
                    continue;
                case 2:
                    world.Settlements.Add(CreateSettlement(world));
                    continue;
                case 3:
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
            if (settlement.Nation != null) {
                Console.WriteLine($"[Id:{settlement.Id}] [Nation:{settlement.Nation.Name}], [Name:{settlement.Name}]");
            } else {
                Console.WriteLine($"[Id:{settlement.Id}] [Nation:None], [Name:{settlement.Name}]");
            }
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

    public static Nation CreateNation(World world) {
        Console.Clear();
        string defaultName = Nation.GetRandomName();

        Console.Write($"Leave blank for Name: {defaultName}\n" +
            $"Enter name: ");

        string? name = Console.ReadLine();
        if (name == string.Empty) name = defaultName;

        Console.Clear();
        int id = world.Nations.Count;
        return new Nation(id++, name!);
    }

    public static Settlement CreateSettlement(World world) {
        Console.Clear();
        string defaultName = Nation.GetRandomName();// To do: create a random settlement name

        Console.Write($"Leave blank for Name: {defaultName}\n" +
            $"Enter name: ");
        string? name = Console.ReadLine();
        if (name == string.Empty) name = defaultName;

        Console.Clear();
        int id = world.Settlements.Count;
        return new Settlement(id++, name!);
    }

    public static void ChangeSettlementNation(World world) {
        int settlementId, nationId;
        Console.Clear();

        do {
            SettlementLookUp(world);
            Console.Write("Type Settlement ID: ");
            settlementId = GetButton();
            Console.Clear();
        } while (0 <= settlementId && settlementId > world.Settlements.Count);

        if (world.Settlements[settlementId].Nation != null) {
            Console.WriteLine($">[Id:{world.Settlements[settlementId].Id}] " +
                $"[Nation:{world.Settlements[settlementId].Nation.Name}], " +
                $"[Name:{world.Settlements[settlementId].Name}]");
        } else {
            Console.WriteLine($">[Id:{world.Settlements[settlementId].Id}] " +
                $"[Nation:None], " +
                $"[Name:{world.Settlements[settlementId].Name}]");
        }

        do {
            NationLookUp(world);
            Console.Write("Type Nation ID: ");
            nationId = GetButton();
            Console.Clear();
        } while (0 <= nationId && nationId > world.Nations.Count);
        Console.WriteLine($">[Id:{world.Nations[nationId].Id}] [Name:{world.Nations[nationId].Name}]");


        world.Settlements[settlementId].Nation = world.Nations[nationId];
        Console.Clear();
        return;

    }
}
