

namespace WorldSim.Domain.Buildings;
class Mine : Building {
    public Mine(Action<SettlementEvent> sendSettlementEvent) : base(sendSettlementEvent) {
    }

    public override void Action() {
        SendSettlementEvent(new ChangeResources(MaterialType.Iron, 2));
    }
}
