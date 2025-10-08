namespace WorldSim.Domain;

class Nation(int id, string? name = null) {
    public static int IdCounter = 0;
    public int Id { get; private set; } = id;
    public string Name { get; private set; } = name ?? GetRandomName();

    static string GetRandomName() {
        Random random = new Random();
        string name;

        string[] prefixes = {"Eld", "Nor", "Val", "Ther", "Kra", "Zan"};
        string[] middles = {"dria", "gorn", "thar", "vyn", "mor", "lithe"};
        string[] suffixes = {"ia", "land", "dor", "heim", "vale", "reach"};

        name = prefixes[random.Next(prefixes.Length)];
        name += middles[random.Next(middles.Length)];
        name += suffixes[random.Next(suffixes.Length)];

        return name;
    }

    public static Nation CreateNation(string nationName = "") {

        if (nationName == "") {
            nationName = GetRandomName();
        }

        IdCounter++;
        int id = IdCounter;
        return new Nation(id, nationName);
    }
}
