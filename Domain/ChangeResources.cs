namespace WorldSim.Domain;

public class ChangeResources(MaterialType type, int amount) : SettlementEvent{
    public MaterialType Type { get; private set; } = type;
    public int Amount { get; private set; } = amount;
}