using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparta_Dungeon_TeamProject
{

    public class GameSkill
    {
        public static List<GameSkill> AllSkills = new List<GameSkill>();

        private Player player;

        // 전체 스킬 데이터
        public static void InitSkills()
        {
            AllSkills = new List<GameSkill> 
            {
                new GameSkill("전사 기본 1", 50, 50, 2, "전사 스킬 1"),
                new GameSkill("전사 2", 50, 50, 2, "전사 스킬 2"),
                new GameSkill("전사 3", 50, 50, 2, "전사 스킬 3"),
                new GameSkill("마법사 기본 1", 50, 50, 2, "마법사 스킬 1"),
                new GameSkill("마법사 2", 50, 50, 2, "마법사 스킬 2"),
                new GameSkill("마법사 3", 50, 50, 2, "마법사 스킬 3"),
                new GameSkill("과학자 기본 1", 50, 50, 2, "과학자 스킬 1"),
                new GameSkill("과학자 2", 50, 50, 2, "과학자 스킬 2"),
                new GameSkill("과학자 3", 50, 50, 2, "과학자 스킬 3"),
                new GameSkill("대장장이 기본 1", 50, 50, 2, "대장장이 스킬 1"),
                new GameSkill("대장장이 2", 50, 50, 2, "대장장이 스킬 2"),
                new GameSkill("대장장이 3", 50, 50, 2, "대장장이 스킬 3"),

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
        // 이름으로 스킬 찾기
        public static GameSkill GetSkillByName(string name)
        {
            return AllSkills.FirstOrDefault(s => s.Name == name);
        }

        public static GameSkill GetWarriorSkill() // 전사 - 랜덤 스킬 획득
        {
            Random rnd = new Random();
            int idx = rnd.Next(0, 2);
            return AllSkills[idx];
        }
        public static GameSkill GetWizzardSkill() // 마법사 - 랜덤 스킬 획득
        {
            Random rnd = new Random();
            int idx = rnd.Next(3, 5);
            return AllSkills[idx];
        }
        public static GameSkill GetScientistSkill() // 과학자 - 랜덤 스킬 획득
        {
            Random rnd = new Random();
            int idx = rnd.Next(6, 8);
            return AllSkills[idx];
        }
        public static GameSkill GetBlackSmithSkill() // 대장장이 - 랜덤 스킬 획득
        {
            Random rnd = new Random();
            int idx = rnd.Next(9, 11);
            return AllSkills[idx];
        }
        public static GameSkill GetRandomSkill() // 영매사 - 랜덤 스킬 획득
        {
            Random rnd = new Random();
            int idx = rnd.Next(0, AllSkills.Count);
            return AllSkills[idx];
        }

    }

}
