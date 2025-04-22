using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparta_Dungeon_TeamProject
{
    internal class Skill
    {
        // 스킬명, 피해 값, 소모 값, 쿨타임, 스킬 설명
        public string Name { get; }
        public int Damage { get; }
        public int Cost { get; }
        public int CoolTime { get; }
        public string Desc { get; }

        public Skill(string name, int damage, int cost, int coolTime, string desc)
        {
            Name = name;
            Damage = damage;
            Cost = cost;
            CoolTime = coolTime;
            Desc = desc;
        }



    }
}
