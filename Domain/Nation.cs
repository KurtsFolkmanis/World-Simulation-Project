namespace WorldSim.Domain;

class Nation(int id, string? name = null) {
    public static int IdCounter = 0;
    public int Id { get; private set; } = id;
    public string Name { get; private set; } = name ?? GetRandomName();

    internal static string GetRandomName() {
        Random random = new Random();
        string randomName;

        var prefixes = new List<string>{"Eld", "Nor", "Val", "Ther", "Kra", "Zan"};
        var roots    = new List<string>{"dria", "gorn", "thar", "vyn", "mor", "lithe"};
        var suffixes = new List<string>{"ia", "land", "dor", "heim", "vale", "reach"};

        randomName = prefixes[random.Next(prefixes.Count)];
        randomName += roots[random.Next(roots.Count)];
        randomName += suffixes[random.Next(suffixes.Count)];

        return randomName;
    }

    public static Nation CreateNation(string nationName = "") {
        Console.Clear();
        nationName = GetRandomName();

        Console.Write($"Leave blank for Name: {nationName}\n" +             
            $"Enter name: ");

        string? name = Console.ReadLine();
        if (name == string.Empty) name = nationName;

        Console.Clear();
        IdCounter++;
        int id = IdCounter;
        return new Nation(id++, name!);
    }
}
