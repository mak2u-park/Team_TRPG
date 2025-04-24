using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparta_Dungeon_TeamProject
{

    public class GameSkill
    {
        public static List<GameSkill> allSkills = new List<GameSkill>();

        private Player player;

        // 전체 스킬 데이터
        public static void InitSkills()
        {
            allSkills = new List<GameSkill> 
            {
                new GameSkill("전사 공용 1", 50, 50, 2, "전사 공용 스킬 1"),
                new GameSkill("전사 공용 2", 50, 50, 2, "전사 공용 스킬 2"),
                new GameSkill("마법사 공용 1", 50, 50, 2, "마법사 공용 스킬 1"),
                new GameSkill("마법사 공용 2", 50, 50, 2, "마법사 공용 스킬 2"),
                new GameSkill("궁수 공용 1", 50, 50, 2, "궁수 공용 스킬 1"),
                new GameSkill("궁수 공용 2", 50, 50, 2, "궁수 공용 스킬 2"),
                new GameSkill("도적 공용 1", 50, 50, 2, "도적 공용 스킬 1"),
                new GameSkill("도적 공용 2", 50, 50, 2, "도적 공용 스킬 2"),
                new GameSkill("성직자 공용 1", 50, 50, 2, "성직자 공용 스킬 1"),
                new GameSkill("성직자 공용 2", 50, 50, 2, "성직자 공용 스킬 2")
            };
        }

        // 스킬명, 피해 값, 소모 값, 쿨타임, 스킬 설명
        public string Name { get; }
        public int Damage { get; }
        public int Cost { get; }
        public int CoolTime { get; }
        public string Desc { get; }

        public GameSkill(string name, int damage, int cost, int coolTime, string desc)
        {
            Name = name;
            Damage = damage;
            Cost = cost;
            CoolTime = coolTime;
            Desc = desc;
        }

        public static GameSkill GetRandomSkill() // 영매사 - 랜덤 스킬 획득
        {
            Random rnd = new Random();
            int idx = rnd.Next(0, allSkills.Count);
            return allSkills[idx];
        }


    }

}
