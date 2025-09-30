namespace WorldSim.Domain.Buildings;
class Armory : Building {
    public Armory(Action<SettlementEvent> sendSettlementEvent) : base(sendSettlementEvent) {
    }
    public override void Action() {
        SendSettlementEvent(new ChangeResources(MaterialType.Weapon, 1));
    }
}
