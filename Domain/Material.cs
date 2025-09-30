public class Material(MaterialType type, int amount = 0, int BasePrice = 1) {
    public int Amount { get; set; } = amount;
    public MaterialType Type { get; set; } = type;
    public int BasePrice { get; set; } = BasePrice;
    public int Price { get; set; } = BasePrice;
}

public enum MaterialType {
    Wood, Food, Iron, Weapon, Gold,
}

