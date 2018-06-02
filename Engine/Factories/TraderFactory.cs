using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    public static class TraderFactory
    {
        private static readonly List<Trader> _traders = new List<Trader>();


        static TraderFactory()
        {
            Trader susan = new Trader(1, "Susan");
            susan.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            susan.AddItemToInventory(ItemFactory.CreateGameItem(123));
            susan.AddItemToInventory(ItemFactory.CreateGameItem(69));

            Trader farmerTed = new Trader(2, "Farmer Ted");
            farmerTed.AddItemToInventory(ItemFactory.CreateGameItem(666));

            Trader peteTheHerbalist = new Trader(3, "Pete the ganja man");
            peteTheHerbalist.AddItemToInventory(ItemFactory.CreateGameItem(1001));

            AddTraderToList(susan);
            AddTraderToList(farmerTed);
            AddTraderToList(peteTheHerbalist);

        }

        public static Trader GetTraderById(int Id)
        {
            return _traders.FirstOrDefault(t => t.ID == Id);
        }

        private static void AddTraderToList(Trader trader)
        {
            if (_traders.Any(t => t.Name == trader.Name))
            {
                throw new ArgumentException($"Vec postoji trader sa {trader.Name} imenom");
            }
            _traders.Add(trader);
        }
    }
}
