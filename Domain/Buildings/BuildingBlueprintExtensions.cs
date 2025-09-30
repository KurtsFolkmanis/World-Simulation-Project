using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldSim.Domain.Buildings;
public static class BuildingBlueprintExtensions {
    public static Dictionary<BuildingBlueprint, List<Material>> BuildingMaterials { get; } = new() {
        {BuildingBlueprint.House, [new Material(MaterialType.Wood, 4)]},
        {BuildingBlueprint.Mine, [new Material(MaterialType.Wood, 2)]},
        {BuildingBlueprint.LumberMill, [new Material(MaterialType.Wood, 1), new Material(MaterialType.Iron, 2)]},
        {BuildingBlueprint.Armory, [new Material(MaterialType.Wood, 2), new Material(MaterialType.Iron, 4)]},
    };

    public static List<Material> GetMaterials(this BuildingBlueprint blueprint) {
        List<Material> materials = new List<Material> {};
        BuildingMaterials.TryGetValue(blueprint, out materials);
        return materials;
    }

    public static bool HasNeededMaterial(this BuildingBlueprint blueprint, Dictionary<MaterialType, Material> materials) {
        var neededMaterials = BuildingMaterials[blueprint];
        foreach (var material in neededMaterials) {
            if (material.Amount > materials[material.Type].Amount) return false;
        }
        return true;
    }

    public static Dictionary<MaterialType, Material> UseBuildingMaterial(this BuildingBlueprint blueprint, Dictionary<MaterialType, Material> materials) {
        var neededMaterials = BuildingMaterials[blueprint];
        foreach (var material in neededMaterials) {
            materials[material.Type].Amount -= material.Amount;
        }
        return materials;
    }

}

