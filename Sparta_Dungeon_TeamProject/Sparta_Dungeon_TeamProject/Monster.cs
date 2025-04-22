using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparta_Dungeon_TeamProject
{

  
    public class Monster
    {

        public int Level { get;}
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }
        public int DropGold { get; }
        public int DropExp { get; }
        public bool IsAlive { get; set; }

        private static Random random = new Random();

        public Monster(int minLevel, int maxLevel, int minAtk, int maxAtk, int minDef, int maxDef, int minHp, int maxHp, int minDropGold, int maxDropGold, int minDropExp, int maxDropExp, bool isAlive)
        {
            Level = random.Next(minLevel, maxLevel + 1);
            Atk = random.Next(minAtk, maxAtk + 1);
            Def = random.Next(minDef, maxDef + 1);
            Hp = random.Next(minHp, maxHp + 1);
            DropGold = random.Next(minDropGold, maxDropGold + 1);
            DropExp = random.Next(minDropExp, maxDropExp + 1);
            IsAlive = isAlive;
        }
    }

    class Goblin : Monster
    {
        public Goblin() : base(
            minLevel: 1, maxLevel: 5,
            minAtk: 3, maxAtk: 5,
            minDef: 5, maxDef: 10,
            minHp: 10, maxHp: 15,
            minDropGold: 1000, maxDropGold: 3000,
            minDropExp: 50, maxDropExp: 100, 
            isAlive: true)
        { 

        }

    }


    class Orc : Monster
    {
        public Orc() : base(
            minLevel: 3, maxLevel: 5,
            minAtk: 5, maxAtk: 10,
            minDef: 10, maxDef: 20,
            minHp: 10, maxHp: 30,
            minDropGold: 2000, maxDropGold: 5000,
            minDropExp: 50, maxDropExp: 100,
            isAlive: true)
        { 

        }

    }

 
}
