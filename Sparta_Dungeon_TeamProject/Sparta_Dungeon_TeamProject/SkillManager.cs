using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Sparta_Dungeon_TeamProject.Player;

namespace Sparta_Dungeon_TeamProject
{

    public class SkillManager
    {
        //===============================[게임 전체 스킬 리스트]=================================

        public static List<SkillLibrary> AllSkills = new List<SkillLibrary>
        {
            // 전사 스킬 ( 0 ~ 3 )
            new SkillLibrary("불안정한 패링", "어색하지만 적의 공격을 방어해낼 준비를 합니다.", 30, 2),
            new SkillLibrary("어렴풋이 기억나는 동작", "몸의 기억을 되살리며, 연속 공격을 실행합니다.", 40, 3),
            new SkillLibrary("능숙한 이간질", "녹슬지 않은 그녀의 재치로 적들을 교란 시킵니다.", 20, 4),
            new SkillLibrary("그녀의 전투 철학", "전사라고 매번 싸움이 하고 싶은 것은 아닙니다.", 20, 4),
            // 마법사 스킬 ( 4 ~ 7 )
            new SkillLibrary("기초적인 화염 마법", "평범 화염 마법으로 적을 불태웁니다.", 40, 2),
            new SkillLibrary("어설픈 얼음 방패", "아담한 사이즈의 방패를 만들어 자신을 보호 합니다.", 80, 4),
            new SkillLibrary("무모한 마나 소용돌이", "비록 미완성형이지만, 그가 가진 가장 강한 기술입니다.", 0, 10),
            new SkillLibrary("마력 증폭", "다음으로 사용할 마법의 위력을 강화합니다.", 40, 3),
            // 과학자 스킬 ( 8 ~ 11 )
            new SkillLibrary("부식의 바늘", "치명적인 약물을 주입해, 적을 중독 시킵니다.", 40, 3),
            new SkillLibrary("적응형 돌연변이", "현재 상황에 적합한 형태로 몸을 변화시킵니다.", 40, 5),
            new SkillLibrary("즉효성 유전자 교란제", "투여와 동시에 유전자를 재구성 시키는 약물을 적에게 주사합니다.", 30, 4),
            new SkillLibrary("세포 분열 촉진제", "부작용을 감수하고, 자신의 세포를 촉진 시키는 약물을 복용합니다.", 60, 5),
            // 대장장이 스킬 ( 12 )
            new SkillLibrary("잃어버린 불꽃", ".", 0, 0),
            // 영매사 스킬 ( 13 ~ 16 )
            new SkillLibrary("전하고 싶었던 말", "그녀가 쓰러진 이들의 목소리를 들려줍니다.", 50, 2),
            new SkillLibrary("쇠와 민들레의 기억", "아름다운 기억은 지친 그녀를 진정시켜 줍니다.", 0, 0),
            new SkillLibrary("저편에 메아리", "저편에서 들려오는 악령들의 메아리는 모든 적를 괴롭게 합니다.", 80, 4),
            new SkillLibrary("생사의 경계", "그녀는 적에게 자신이 속한 '가운데 세계'의 모습을 잠시 보여줍니다.", 75, 3),
        };

        //===============================[직업별 스킬 분류]=================================

        public static Dictionary<JobType, List<SkillLibrary>> JobSkills = new()
        {
            { JobType.전사, AllSkills.GetRange(0, 3) },
            { JobType.마법사, AllSkills.GetRange(4, 7) },
            { JobType.과학자, AllSkills.GetRange(8, 11) },
            { JobType.대장장이, AllSkills.GetRange(12,12) },
            { JobType.영매사, AllSkills.GetRange(13, 16) }
        };

        //===============================[스킬명으로 스킬 찾기]=================================
        public static SkillLibrary GetSkillByName(string name)
        {
            return AllSkills.FirstOrDefault(s => s.Name == name);
        }

        //===============================[직업별 기본 스킬 추가]=================================
        public static void FirstWarriorSkill(Player player)
        {
            player.Skills.Add(GetSkillByName("불안정한 패링"));
        }
        public static void FirstWizardSkill(Player player)
        {
            player.Skills.Add(GetSkillByName("."));
        }
        public static void FirstScientistSkill(Player player)
        {
            player.Skills.Add(GetSkillByName("부식의 바늘."));
        }
        public static void FirstBlacksmithSkill(Player player)
        {
            player.Skills.Add(GetSkillByName("잃어버린 불꽃."));
        }
        public static void FirstWhispererSkill(Player player)
        {
            player.Skills.Add(GetSkillByName("영혼에 스민"));
        }

        //===============================[직업별 무작위 스킬 추가]=================================
        public static void LearnSkill(Player player)
        {
            if (!JobSkills.TryGetValue(player.Job, out var skillList))
            {
                return;
            }

            var unlearnedSkills = skillList
                .Where(skill => !player.Skills.Contains(skill))
                .ToList();

            if (unlearnedSkills.Count == 0)
            {
                Console.WriteLine("당신 모든 스킬을 배웠습니다.");
                return;
            }

            Random random = new Random();
            SkillLibrary randomSkill = unlearnedSkills[random.Next(unlearnedSkills.Count)];
            player.Skills.Add(randomSkill);

            Console.WriteLine($"당신은 {randomSkill.Name} 스킬을 배웠습니다!");
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
            int idx = random.Next(9, 12); // 1~3 랜덤
            player.Skills.Add(AllSkills[idx]);
        }
        public static void RandomBlacksmithSkill(Player player)
        {
            Random random = new Random();
            int idx = random.Next(13, 16); // 1~3 랜덤
            player.Skills.Add(AllSkills[idx]);
        }
        public static void RandomWhispererSkill(Player player)
        {
            Random random = new Random();
            int idx = random.Next(17, 20); // 1~3 랜덤
            player.Skills.Add(AllSkills[idx]);
        }

        //===============================[전사 스킬 사용]=================================

        public void UnsteadyParry(Player player) // 기본 - 불안정한 패링
        {
            Random random = new Random();
            int chance = random.Next(0, 2); // 50% 확률

            if (chance == 1)
            {
                player.DefUP(5); // 방어력 +5

                // 반격 데미지 추가? ( 공격,방어력 계수? )

                Console.WriteLine("""그리운 감각이네.""");
            }
            else
            {
                player.Heal(-20); // HP -20, 해당 피해로 사망하지 않음
                Console.WriteLine("""이런... 나답지 못한 걸.""");
            }
        }
        public void UnsteadyStrike(Player player) // 1 - 어렴풋이 기억나는 동작
        {
            Random random = new Random();
            int Count = random.Next(1, 4); // 1~3회 타격

            for (int i = 1; i <= Count; i++)
            {
                Console.WriteLine($"공격이 {Count}번 적중했습니다.");
                //int damage = player.Atk / i;
                //damage = Math.Max(damage, 10); // 최소 데미지

                if (Count == 1)
                {
                    Console.WriteLine("""나도 참 녹슬었군.""");
                }
                if (Count == 2)
                {
                    Console.WriteLine("""옛날 생각이 나는 걸.""");
                }
                else if (Count == 3)
                {
                    Console.WriteLine("""나는 기억 못해도, 내 몸은 전부 기억해.""");
                }
            }
        }
        public void TongueTwister() // 2 - 능숙한 이간질
        {

        }

        //===============================[영매사 스킬 사용]=================================

        public void BorrowedGrace(Player player)
        {
            Random random = new Random();
            int skillIndex = random.Next(0, 3); // 전체 스킬 중 하나

            switch (skillIndex)
            {
                case 0:
                    UnsteadyParry(player);
                    break;
                case 1:
                    UnsteadyStrike(player);
                    break;
                case 2:
                    TongueTwister();
                    break;
            }
        }
    }
}
