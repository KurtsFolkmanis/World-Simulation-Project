namespace WorldSim.Domain;

class Trader {
    private List<Settlement> settlements;
    private Random random = new Random();
    private const int MAX_Inventory = 30;
    public int Gold { get; set; } = 0;
    private Settlement currentSettlement { get; set; }

    public Trader(int id, List<Settlement> settlement, Settlement location, int goldAmount) {
        this.settlements = settlement;
        this.currentSettlement = location;
        Gold = goldAmount;
    }

    public void TurnAction() {
        Settlement nextSettlement;
        do {
            nextSettlement = settlements[random.Next(settlements.Count)];
        } while (currentSettlement.Id == nextSettlement.Id);

        List<Material> materialsToBuy = GetAvailableMaterialsForSettlemnt(nextSettlement);

        if (materialsToBuy.Count != 0) {
            Material boughtMaterial = BuyMaterial(materialsToBuy, nextSettlement);
            if (boughtMaterial == null) return;
            SellMaterial(boughtMaterial, nextSettlement);
            boughtMaterial = null;
        }
    }

    private Material BuyMaterial(List<Material> stock, Settlement nextSettelment) {
        Material boughtMaterial = stock[random.Next(stock.Count)];
        if (boughtMaterial.Amount == 0) return null;
        boughtMaterial.Amount = currentSettlement.buyMaterial(nextSettelment, boughtMaterial, Gold, MAX_Inventory);
        return boughtMaterial;
    }

    private void SellMaterial(Material boughtMaterial, Settlement nextSettlement) {
        Gold += nextSettlement.sellMaterial(boughtMaterial);
    }

    private List<Material> GetAvailableMaterialsForSettlemnt(Settlement nextSettlement) {
        List<Material> materials = new List<Material>();
        foreach (var material in nextSettlement.Materials) {
            if (currentSettlement.Materials[material.Key].Price < material.Value.Price) {
                materials.Add(currentSettlement.Materials[material.Key]);
            }
        }
        return materials;
    }
}
