using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using WorldSim.Domain;
using WorldSim.Domain.Buildings;

namespace WorldSim.Domain;

class Settlement {
    public static int IdCounter = 0;
    public int Id { get; private set; }
    public string Name { get; set; }
    public int Population { get; private set; } = 0;
    public int MaxPopulation { get; private set; } = 0;
    public Nation? Nation { get; set; }
    public Dictionary<MaterialType, Material> Materials { get; private set; } = new Dictionary<MaterialType, Material>() {
        {MaterialType.Wood, new Material(MaterialType.Wood, 12, 4)},
        {MaterialType.Food, new Material(MaterialType.Food, 1, 2)},
        {MaterialType.Iron, new Material(MaterialType.Iron, 6, 6)},
        {MaterialType.Weapon, new Material(MaterialType.Weapon, 0, 20)},
        {MaterialType.Gold, new Material(MaterialType.Gold, 0, 1)}
    };
    public Dictionary<MaterialType, Material> MaterialsInDemand { get; private set; } = new Dictionary<MaterialType, Material>() {
        {MaterialType.Wood, new Material(MaterialType.Wood, 0, 4)},
        {MaterialType.Food, new Material(MaterialType.Food, 0, 2)},
        {MaterialType.Iron, new Material(MaterialType.Iron, 0, 6)},
        {MaterialType.Weapon, new Material(MaterialType.Weapon, 0, 20)},
        {MaterialType.Gold, new Material(MaterialType.Gold, 0, 1)}
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
            building.Action();
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
    public static Settlement CreateSettlement(string SettlementName = "") {

        if (SettlementName == "") {
            SettlementName = Nation.GetRandomName();// To do: create a random settlement name
        }

        IdCounter++;
        int id = IdCounter;
        return new Settlement(id, SettlementName); 
    }

    public void SetDemand() {
        if (buildingBlueprint == null) {
            foreach (var material in MaterialsInDemand) {
                material.Value.Amount = material.Value.BasePrice;
            }
            return;
        }
        return;
    }

    public void SetPrice() {
        SetDemand();

        foreach (var material in Materials) {
            if (MaterialsInDemand[material.Key].Amount != 0) {
                material.Value.Price = material.Value.BasePrice - 4;
            }
            material.Value.Price = material.Value.BasePrice;
        }
    }

    public override string ToString() {
        return $"Name:{Name} " +
            $"MaxPopulation:{MaxPopulation} " +
            $"Buildings:{String.Join(",", Buildings)}";
    }

}

