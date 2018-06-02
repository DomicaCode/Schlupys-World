using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    public static class MonsterFactory
    {
        public static Monster GetMonster(int MonsterID)
        {
            switch (MonsterID)
            {
                case 1:
                    Monster snake = new Monster("Snake", "snake.png", 4, 4, 1, 2, 5, 1);
                    AddLootItem(snake, 9001, 25);
                    AddLootItem(snake, 9002, 75);
                    AddLootItem(snake, 69, 50);
                    return snake;

                case 2:
                    Monster rat = new Monster("Rat", "rat.jpg", 5, 5, 1, 2, 5, 1);
                    AddLootItem(rat, 9003, 25);
                    AddLootItem(rat, 9004, 75);
                    return rat;

                case 3:
                    Monster spider = new Monster("Spider", "spider.jpg", 10, 10, 1, 4, 10, 3);
                    AddLootItem(spider, 9005, 25);
                    AddLootItem(spider, 9006, 75);
                    return spider;

                default:
                    throw new ArgumentException(string.Format("Monster type '{0}' does not exist", MonsterID));
            }
        }

        private static void AddLootItem(Monster monster, int itemID, int percentage)
        {
            if (RandomNumberGenerator.NumberBetween(1,100) <= percentage)
            {
                monster.AddItemToInventory(ItemFactory.CreateGameItem(itemID));
            }
        }
    }
}
