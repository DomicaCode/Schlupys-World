using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Factories;

namespace Engine.Models
{
    public class Location
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public List<Quest> QuestsAvailableHere { get; set; } = new List<Quest>();
        public string MapImage { get; set; }
        public List<MonsterEncounter> MonstersHere { get; set; } = new List<MonsterEncounter>();

        public Trader TraderHere { get; set; }

        public void AddMonster(int monsterID, int chanceOfEncountering)
        {
            if (MonstersHere.Exists(m => m.MonsterID == monsterID))
            {   
                //Monster je vec dodan u tu lokaciju pa overwritaj chanceofencounter
                MonstersHere.First(m => m.MonsterID == monsterID).ChanceOfEncountering = chanceOfEncountering;
            }
            else
            {
                //Monster nije na lokaciji, dodaj ga
                MonstersHere.Add(new MonsterEncounter(monsterID, chanceOfEncountering));
            }
        }
        public Monster GetMonster()
        {
            if (!MonstersHere.Any())
            {
                return null;
            }

            //Total percentage od svih monstera u lokaciji
            int totalChances = MonstersHere.Sum(m => m.ChanceOfEncountering);

            //Selectaj random number izmedu 1 i percentage monstera u lokaciji
            int randomNumber = RandomNumberGenerator.NumberBetween(1, totalChances);

            int runningTotal = 0;

            foreach (MonsterEncounter monsterEncounter in MonstersHere)
            {
                runningTotal += monsterEncounter.ChanceOfEncountering;

                if (randomNumber <= runningTotal)
                {
                    return MonsterFactory.GetMonster(monsterEncounter.MonsterID);
                }
            }

            return MonsterFactory.GetMonster(MonstersHere.Last().MonsterID);

        }

    }
}
