using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldSim.Domain.Buildings;

public abstract class Building {
    private readonly List<SettlementEvent> _actionQueue = [];
    public string Name {
        get {
            return GetType().Name.ToString();
        }
    }         

    public List<SettlementEvent> TurnActions() {
        Action();
        var events = new List<SettlementEvent>(_actionQueue);
        _actionQueue.Clear();
        return events;
    }

    protected void SendSettlementEvent(SettlementEvent settlementEvent) {
        _actionQueue.Add(settlementEvent);
    }

    public abstract void Action();

}

public enum BuildingBlueprint {
    House, Mine, LumberMill, Armory
}
