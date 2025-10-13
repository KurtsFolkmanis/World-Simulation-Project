using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using WorldSim.Domain;
using WorldSim.Domain.Buildings;
using static System.Net.Mime.MediaTypeNames;

namespace WorldSim.Domain;

class Settlement {
    public static int IdCounter = 0;
    public int Id { get; set; }
    public string Name { get; private set; }
    public int Population { get; private set; } = 0;
    public int MaxPopulation { get; private set; } = 0;
    public int Gold { get; set; } = 1;
    public Nation? Nation { get; set; }
    public Dictionary<MaterialType, Material> Materials { get; private set; } = new Dictionary<MaterialType, Material>() {
        {MaterialType.Wood, new Material(MaterialType.Wood, 12, 4)},
        {MaterialType.Food, new Material(MaterialType.Food, 1, 4)},
        {MaterialType.Iron, new Material(MaterialType.Iron, 6, 6)},
        {MaterialType.Weapon, new Material(MaterialType.Weapon, 0, 20)},
    };
    public List<Building> Buildings { get; private set; } = new List<Building>();

    private BuildingBlueprint? buildingBlueprint;

    public Settlement(int id, string name) {
        this.Id = id;
        this.Name = name;
        Buildings.Add(new GovernorMansion(ProcessEvent));
        Population += 2;
    }

    public void TurnAction() {
        // Add population system
        if (Population < MaxPopulation) Population += 2;

        // Get materials from buildings
        foreach (var building in Buildings) {
            foreach (var settlementEvent in building.TurnActions()) {
                ProcessEvent(settlementEvent);
            }
        }

        // Choose to create a building
        TryConstructBuilding();

        // To do: set prices for materials
        SetPrice();
    }

    public void ProcessEvent(SettlementEvent settlementEvent) {
        switch (settlementEvent) {
            case ChangeResources changeResources:
                Materials[changeResources.Type].Amount += changeResources.Amount;
                break;
            case ChangePopulation changePopulation:
                MaxPopulation += changePopulation.Amount;
                break;
            case ChangeGold changeGold:
                Gold += changeGold.Amount;
                break;
            default:
                break;
        }
    }

    public void TryConstructBuilding() {
        buildingBlueprint ??= PickBlueprint();

        if (!buildingBlueprint.Value.HasNeededMaterial(Materials)) return;

        buildingBlueprint.Value.UseBuildingMaterial(Materials);

        switch (buildingBlueprint) {
            case BuildingBlueprint.House:
                Buildings.Add(new House(ProcessEvent));
                buildingBlueprint = null;
                break;
            case BuildingBlueprint.Mine:
                Buildings.Add(new Mine(ProcessEvent));
                buildingBlueprint = null;
                break;
            case BuildingBlueprint.LumberMill:
                Buildings.Add(new LumberMill(ProcessEvent));
                buildingBlueprint = null;
                break;
            case BuildingBlueprint.Armory:
                Buildings.Add(new Armory(ProcessEvent));
                buildingBlueprint = null;
                break;
        }
    }

    public BuildingBlueprint PickBlueprint() {
        var blueprint = Enum.GetValues<BuildingBlueprint>();
        Random random = new Random();
        do {
            switch (blueprint[random.Next(blueprint.Length)]) {
                case BuildingBlueprint.House:
                    return BuildingBlueprint.House;
                case BuildingBlueprint.Mine:
                    return BuildingBlueprint.Mine;
                case BuildingBlueprint.LumberMill:
                    return BuildingBlueprint.LumberMill;
                case BuildingBlueprint.Armory:
                    return BuildingBlueprint.Armory;
                default:
                    throw new NotImplementedException();
            }
        }
        while (true);
    }

    static string GetRandomName() {
        Random random = new Random();
        string name;

        string[] prefixes = { "Raven", "Oak", "Iron", "Wolf", "Stone", "Wind", "Bright", "Frost", "Black", "Silver" };
        string[] middles = { "wood", "field", "keep", "dale", "fort", "shire", "watch", "crest", "gate", "haven" };
        string[] suffixes = { "", "ton", "burg", "stead", "port", "hold", "ford", "mouth", "ham", "hall" };

        name = prefixes[random.Next(prefixes.Length)];
        name += middles[random.Next(middles.Length)];
        name += suffixes[random.Next(suffixes.Length)];

        return name;
    }

    public static Settlement CreateSettlement(string SettlementName = "") {

        if (SettlementName == "") {
            SettlementName = GetRandomName();// To do: create a random settlement name
        }

        IdCounter++;
        int id = IdCounter;
        return new Settlement(id, SettlementName); 
    }

    public List<Material> GetDemand() {
        List<Material> demand = [];
        if(buildingBlueprint == null) {
            return demand;
        }

        foreach (var material in buildingBlueprint.Value.GetMaterials()) {
            demand.Add(material);
        }

        return demand;
    }

    public void SetPrice() {
        List<Material> MaterialsInDemand = GetDemand();
        Random random = new Random();

        foreach (var material in Materials) {
            material.Value.Price = material.Value.BasePrice + random.Next(-1,1);
        }
        if (MaterialsInDemand.Count == 0) return;

        foreach (var material in MaterialsInDemand) {
            Materials[material.Type].Price += 3;
        }
    }

    public int buyMaterial(Settlement nextSettlement ,Material material, int gold, int inventorySize) {
        int nextSettlementGold = nextSettlement.Gold;
        if (nextSettlementGold < material.Price || gold < material.Price) return 0;
        
        int traderAffordableAmount = gold / material.Price;
        int settlementAffordableAmount = nextSettlementGold / material.Price;

        int affordableAmount = Math.Min(traderAffordableAmount, settlementAffordableAmount);
        affordableAmount = Math.Min(affordableAmount, material.Amount);
        affordableAmount = Math.Min(affordableAmount, inventorySize);

        Console.WriteLine($"Trader can afford {traderAffordableAmount} of {material.Type}, settlement can afford {settlementAffordableAmount}, settlemnet Stock {material.Amount}, actual amount: {affordableAmount}");

        gold += affordableAmount * material.Price;
        material.Amount -= affordableAmount;
        return affordableAmount;

    }
    internal int sellMaterial(Material material) {
        Materials[material.Type].Amount += material.Amount;
        Gold -= material.Amount * material.Price; 
        return Materials[material.Type].Price * material.Amount;
    }

    public override string ToString() {
        return $"Name:{Name} " +
            $"MaxPopulation:{MaxPopulation} " +
            $"Buildings:{String.Join(",", Buildings)}";
    }

}

