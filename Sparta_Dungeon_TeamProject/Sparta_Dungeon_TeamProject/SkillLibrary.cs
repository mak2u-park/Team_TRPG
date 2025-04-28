using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparta_Dungeon_TeamProject
{
    public class SkillLibrary
    {
        public string Name { get; }
        public string Desc { get; }
        public int Cost { get; }
        public int Cool { get; }



        // 스킬명, 스킬 설명, 소모값, 쿨타임
        public SkillLibrary (string name, string desc , int cost, int cool)
        {
            Name = name;
            Desc = desc;
            Cost = cost;
            Cool = cool;
        }
    }
}
