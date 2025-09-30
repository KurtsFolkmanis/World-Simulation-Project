namespace WorldSim.Domain.Buildings;
class LumberMill : Building {
    public LumberMill(Action<SettlementEvent> sendSettlementEvent) : base(sendSettlementEvent) {
    }

    public override void Action() {
        SendSettlementEvent(new ChangeResources(MaterialType.Wood, 4));
    }

}
