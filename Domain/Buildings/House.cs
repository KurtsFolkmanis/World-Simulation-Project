using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WorldSim.Domain.Buildings;

class House : Building {
    public House(Action<SettlementEvent> sendSettlementEvent) : base(sendSettlementEvent) {
        SendSettlementEvent(new ChangePopulation(6));
    }

    public override void Action() {
        return;
    }

}
