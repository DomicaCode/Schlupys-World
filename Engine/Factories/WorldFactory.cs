using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    internal static class WorldFactory
    {
        internal static World CreateWorld()
        {
            World newWorld = new World();

            newWorld.AddLocation(-2, -1, "Farmer's Field",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse mauris.", 
                "farmfield.jpg",
                "Map-2-1.png");

            newWorld.LocationAt(-2, -1).AddMonster(2, 100);

            newWorld.AddLocation(-1, -1, "Farmer's House",
                 "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse mauris.",
                 "farmhouse.jpg", 
                 "Map-1-1.png");

            newWorld.LocationAt(-1, -1).TraderHere = TraderFactory.GetTraderById(2); // Farmer Ted Trader

            newWorld.AddLocation(0, -1, "Home", 
                "This is your house", 
                "house.jpg", "Map0-1.png");

            newWorld.AddLocation(0, 0, "Town Square",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse mauris.",
                "fountain.jpg", "Map00.png");

            newWorld.AddLocation(0, 1, "Herbalist hut",
                 "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse mauris.",
                  "herbalisthut.jpg", "Map01.png");

            newWorld.LocationAt(0, 1).TraderHere = TraderFactory.GetTraderById(3); // Pete the ganja man Trader

            newWorld.LocationAt(0, 1).QuestsAvailableHere.Add(QuestFactory.GetQuestById(1));

            newWorld.AddLocation(0, 2, "Herb Garden",
                 "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse mauris.",
                  "herbgarden.jpg", "Map02.png");

            newWorld.LocationAt(0, 2).AddMonster(1, 100);

            newWorld.AddLocation(-1, 0, "Trading shop",
                 "I got what you need",
                  "tradingshop.jpg", "Map-10.png");
            newWorld.LocationAt(-1, 0).TraderHere = TraderFactory.GetTraderById(1); // Susan Trader

            newWorld.AddLocation(1, 0, "Town gate",
                 "Are you sure you want to leave?",
                  "towngate.jpg", "Map10.png");

            newWorld.AddLocation(2, 0, "Spider forest",
                 "Oh no you got lost",
                  "spiderforest.png", "Map20.png");

            newWorld.LocationAt(2, 0).AddMonster(3, 100);

            return newWorld;
        }
    }
}
