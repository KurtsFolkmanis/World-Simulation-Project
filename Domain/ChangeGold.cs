using WorldSim.Domain;

public class ChangeGold(int amount) : SettlementEvent {
    public int Amount { get; private set; } = amount;
}
