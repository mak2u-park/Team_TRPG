using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Sparta_Dungeon_TeamProject.Player;

namespace Sparta_Dungeon_TeamProject
{

    public class SkillManager
    {
        //==============[게임 전체 스킬 리스트]=================================================
        public static List<SkillLibrary> AllSkills = new List<SkillLibrary>
        {
            new SkillLibrary("불안정한 패링", "굳었던 몸을 웅크리며 반격을 준비한다.", 30, 2),
            new SkillLibrary("어렴풋이 기억나는 동작", "몸의 기억을 되살린다.", 40, 3),
            new SkillLibrary("능숙한 이간질", "시간이 지나도 그녀의 말솜씨는 여전하다.", 20, 4)
        };

        //==============[스킬명으로 스킬 찾기]=================================================
        public static SkillLibrary GetSkillByName(string name)
        {
            return AllSkills.FirstOrDefault(s => s.Name == name);
        }


        //==============[직업별 기본 스킬 지급]=================================================
        public static void FirstWarriorSkill(Player player)
        {
           player.Skills.Add(GetSkillByName("불안정한 패링"));
        }
        public static void FirstWizzardSkill(Player player)
        {
            player.Skills.Add(GetSkillByName(""));
        }
        public static void FirstScientistSkill(Player player)
        {
            player.Skills.Add(GetSkillByName(""));
        }
        public static void FirstBlacksmithSkill(Player player)
        {
            player.Skills.Add(GetSkillByName(""));
        }
        public static void FirstWhispererSkill(Player player)
        {
            player.Skills.Add(GetSkillByName(""));
        }

        //==============[무작위 직업 스킬 지급]=================================================
        public static void RandomWarriorSkill(Player player)
        {
            Random random = new Random();
            int idx = random.Next(1, 4); // 1~3 랜덤
            player.Skills.Add(AllSkills[idx]);
        }
        public static void RandomWizzardSkill(Player player)
        {
            Random random = new Random();
            int idx = random.Next(5, 8); // 1~3 랜덤
            player.Skills.Add(AllSkills[idx]);
        }
        public static void RandomScientistSkill(Player player)
        {
            Random random = new Random();
            int idx = random.Next(10, 13); // 1~3 랜덤
            player.Skills.Add(AllSkills[idx]);
        }
        public static void RandomBlacksmithSkill(Player player)
        {
            Random random = new Random();
            int idx = random.Next(15, 18); // 1~3 랜덤
            player.Skills.Add(AllSkills[idx]);
        }
        public static void RandomWhispererSkill(Player player)
        {
            Random random = new Random();
            int idx = random.Next(20, 23); // 1~3 랜덤
            player.Skills.Add(AllSkills[idx]);
        }

        //==============[전사 스킬 사용]=================================================

        public void UnsteadyParry(Player player) // 기본 - 불안정한 패링
        {
            Random random = new Random();
            int chance = random.Next(0, 2); // 50% 확률

            if (chance == 1)
            {
                player.DefUP(5); // 방어력 +5

                // 반격 데미지 추가 ( 방어력 계수? )
                
                Console.WriteLine("""그리운 감각이네.""");
            }
            else
            {
                player.Heal(-20); // HP -20, 해당 피해로 사망하지 않음
                Console.WriteLine("""젠장, 나답지 못한 걸.""");
            }
        }
        public void UnsteadyStrike(Player player) // 1 - 어렴풋이 기억나는 동작
        {
            Random random = new Random();
            int Count = random.Next(1, 4); // 1~3회 타격

            for (int i = 1; i <= Count; i++) 
            {
                //int damage = player.Atk / i;
                //damage = Math.Max(damage, 10); // 최소 데미지

                if (Count == 1)
                {
                    Console.WriteLine("""처참하게도 녹슬었군.""");
                }
                if (Count == 2)
                {
                    Console.WriteLine("""옛날 생각나게 하네.""");

                }
                else if (Count == 3)
                {
                    Console.WriteLine("""나는 기억 못해도, 내 몸이 기억해.""");

                }
            }
        }
        public void TongueTwister() // 2 - 능숙한 이간질
        {

        }
    }
}
