namespace WorldSim.Domain;

class Nation(int id, string? name = null) {
    public int Id { get; private set; } = id;
    public string Name { get; private set; } = name ?? GetRandomName();

    internal static string GetRandomName() {
        Random rnd        = new Random();
        string randomName;

        var prefixes = new List<string>{"Eld", "Nor", "Val", "Ther", "Kra", "Zan"};
        var roots    = new List<string>{"dria", "gorn", "thar", "vyn", "mor", "lithe"};
        var suffixes = new List<string>{"ia", "land", "dor", "heim", "vale", "reach"};

        randomName = prefixes[rnd.Next(prefixes.Count)];
        randomName += roots[rnd.Next(roots.Count)];
        randomName += suffixes[rnd.Next(suffixes.Count)];

        return randomName;
    }
}
