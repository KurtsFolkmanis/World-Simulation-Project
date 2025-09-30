using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldSim.Domain.Buildings;

public abstract class Building(Action<SettlementEvent> sendSettlementEvent) {
    public string Name {
        get {
            return GetType().Name.ToString();
        }
    }

    protected Action<SettlementEvent> SendSettlementEvent { get; set; } = sendSettlementEvent;
    public abstract void Action();

}

public enum BuildingBlueprint {
    House, Mine, LumberMill, Armory
}
