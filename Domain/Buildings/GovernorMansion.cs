using System;
using WorldSim.Domain;

namespace WorldSim.Domain.Buildings;

class GovernorMansion : Building {
    public GovernorMansion(Action<SettlementEvent> sendSettlementEvent) : base(sendSettlementEvent) {
        SendSettlementEvent(new ChangePopulation(4));
    }

    public override void Action() {
        SendSettlementEvent(new ChangeResources(MaterialType.Gold, 4));
    }
}
