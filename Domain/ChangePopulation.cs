namespace WorldSim.Domain;
public class ChangePopulation(int amount) : SettlementEvent {
    public int Amount = amount;
}
