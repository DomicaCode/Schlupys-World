using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Potion : GameItem

    {
        public int Heal { get; set; }

        public Potion(int itemTypeID, string name, int price, int heal) : base(itemTypeID, name, price, false)
        {
            Heal = heal;
        }

        public new Potion Clone()
        {
            return new Potion(ItemTypeID, Name, Price, Heal);
        }
    }
}
