using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparta_Dungeon_TeamProject
{

    public class GameLibrary
    {    
        // 스킬명, 피해 값, 소모 값, 쿨타임, 스킬 설명
        public string Name { get; }
        public int Cost { get; }
        public int CoolTime { get; }
        public string Desc { get; }

        public GameLibrary(string name, int cost, int coolTime, string desc)
        {
            Name = name;
            Cost = cost;
            CoolTime = coolTime;
            Desc = desc;
        }
        // 이름으로 스킬 찾기
        public static GameLibrary GetSkillByName(string name)
        {
            return AllSkills.FirstOrDefault(s => s.Name == name);
        }

        public static GameLibrary GetWarriorSkill() // 전사 - 랜덤 스킬 획득
        {
            Random rnd = new Random();
            int idx = rnd.Next(1, 3);
            return AllSkills[idx];
        }
        public static GameLibrary GetWizzardSkill() // 마법사 - 랜덤 스킬 획득
        {
            Random rnd = new Random();
            int idx = rnd.Next(5, 6);
            return AllSkills[idx];
        }
        public static GameLibrary GetScientistSkill() // 과학자 - 랜덤 스킬 획득
        {
            Random rnd = new Random();
            int idx = rnd.Next(8, 9);
            return AllSkills[idx];
        }
        public static GameLibrary GetBlacksmithSkill() // 대장장이 - 랜덤 스킬 획득
        {
            Random rnd = new Random();
            int idx = rnd.Next(11, 12);
            return AllSkills[idx];
        }
        public static GameLibrary GetWhispererSkill() // 영매사 - 랜덤 스킬 획득
        {
            Random rnd = new Random();
            int idx = rnd.Next(14, 15);
            return AllSkills[idx];
        }

        // 전사 스킬

        //===================================================================
        // 불안정한 패링
        public void UnsteadyParry()
        {
            Random random = new Random();
            int chance = random.Next(0, 2); // 0 또는 1 (50% 확률)

            if (chance == 1)
            {
                player.DefUP(5); // 방어력 +5
                Console.WriteLine("나도 아직 쓸모 있다니까.");
            }
            else
            {
                player.Heal(-20); // HP -20
                Console.WriteLine("많이 녹슬었어..");
            }
        }

        // 어렴풋이 기억나는 동작
        public void UnsteadyStrike()
        {
            Random random = new Random();
            int Count = random.Next(1, 4); // 1~3 히트 랜덤

            for (int i = 1; i <= Count; i++)
            {
                int damage = 100 - ((i - 1) * 30);
                damage = Math.Max(damage, 10); // 최소 데미지

                if (Count == 1)
                {
                    Console.WriteLine("예전엔 이렇지 않았는데..");
                }
                if (Count == 2)
                {
                    Console.WriteLine("그럭저럭 기억 하는 것 같군.");

                }
                else if (Count == 3)
                {
                    Console.WriteLine("내 몸은 전부 기억하지.");

                }
            }
        }

    }

}
