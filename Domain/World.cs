using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WorldSim.Domain.Buildings;

namespace WorldSim.Domain;

class World {
    public Random Random = new Random();
    public List<Nation> Nations { get; set; } = new List<Nation>();
    public List<Settlement> Settlements { get; private set; } = new List<Settlement>();
    public List<Trader> Traders { get; private set; } = new List<Trader>();

    public void WorldStart() {
        Nations.Add(new Nation(0));
        Nations.Add(new Nation(1));
        Nations.Add(new Nation(2));

        Settlements.Add(new Settlement(0, "SettlementTest1"));
        Settlements.Add(new Settlement(1, "SettlementTest2"));
        Settlements.Add(new Settlement(2, "SettlementTest3"));
        Settlements.Add(new Settlement(3, "SettlementTest4"));
        Settlements.Add(new Settlement(4, "SettlementTest5"));

        Settlements[0].Nation = Nations[0];
        Settlements[1].Nation = Nations[1];
        Settlements[2].Nation = Nations[2];
        Settlements[3].Nation = Nations[1];
        Settlements[4].Nation = Nations[2];

    }

    public void Turn() {
        int worldPop = 0;

        foreach (var settlement in Settlements) {
            worldPop += settlement.Population;
        }

        int maxTrader = worldPop / 4;
        if (maxTrader > Traders.Count) {
            do {
                Settlement settlement = Settlements[Random.Next(Settlements.Count)];
                Traders.Add(new Trader(Traders.Count + 1, Settlements, settlement, 10));
            }
            while (maxTrader > Traders.Count);
        }

        foreach (var settlement in Settlements) {
            settlement.TurnAction();
        }

        foreach (var trader in Traders) {
            trader.TurnAction();
        }
    }
}
