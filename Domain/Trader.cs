namespace WorldSim.Domain;

class Trader {
    private List<Settlement> settlements;
    private Random random = new Random();
    private Material gold { get; set; } = new Material(MaterialType.Gold);
    private Settlement currentSettlement { get; set; }

    public Trader(int id, List<Settlement> settlement, Settlement location, int goldAmount) {
        this.settlements = settlement;
        this.currentSettlement = location;
        gold.Amount = goldAmount;
    }

    public void TurnAction() {
        Settlement nextSettlement;
        do {
            nextSettlement = settlements[random.Next(settlements.Count)];
        } while (currentSettlement.Id == nextSettlement.Id);

        List<Material> stock = new();
        foreach (var materials in nextSettlement.Materials) {
            if (currentSettlement.Materials[materials.Key].Price > materials.Value.Price) {
                stock.Add(currentSettlement.Materials[materials.Key]);
            }
        }

        if (stock == null) {
            BuyMaterial(stock);
            Console.WriteLine($"{currentSettlement.Name} ==> {nextSettlement.Name} Gold: {gold.Amount}");
        }

        currentSettlement = nextSettlement;
    }

    private Material BuyMaterial(List<Material> Stock) {
        Material material = null;
        material = Stock[random.Next(Stock.Count)];

        Console.WriteLine($"Price: {material.Price} / {gold.Amount}");
        Console.ReadKey();
        int amount = material.Price / gold.Amount;

        return new Material(MaterialType.Weapon);
    }
}
