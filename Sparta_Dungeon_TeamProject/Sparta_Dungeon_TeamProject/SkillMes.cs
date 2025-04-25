using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparta_Dungeon_TeamProject
{
    class Blacksmith_Skill
    {
        public static void LostFlameSkillEffect()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;

            // 캐릭터 대사
            if (!Messages.Skip) // 스킵 = false 대사 출력
            {
                string[] emotionLines1 =
                {
        "\n\n\n\n    \"그 아이는 내 불꽃이었어... 이미 꺼진 줄만 알았지.\"",
        "\n\n    ",
        "\n\n    \"한 번 더... 마지막으로 한 번만.\""
    };

                Messages.PrintLinesWithSkip(emotionLines1, 30, 150);

                if (!Messages.Skip)
                {
                    // 회색 메시지 출력
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    string[] emotionLines2 =
                {"\n\n\n    - 그리고, 타오르는 쇳물이 그 눈동자에서 피어난다.", };

                    Messages.PrintLinesWithSkip(emotionLines2, 40, 100);
                    Console.ResetColor();
                }

                if (Messages.Skip) Console.Clear(); // 스킵 = true 넘어가면서 청소
            }

            Thread.Sleep(300);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;

            // 스킬 발동 선언
            string[] skillEffectLines =
            {
            "\n\n\n\n\n\n    ──  스킬 [잃어버린 불꽃] 발동 ──",
            "\n\n    늙은 심장에서, 마지막 불꽃이 피어난다.",
            "\n\n    아이를 지키지 못한 죄책감이,",
            "\n\n    녹슨 망치 끝에서 다시 타오른다."
        };

            Messages.PrintLinesWithSkip(skillEffectLines, 25, 60);

            // 효과 설명
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n\n    ▶ 2턴 동안 공격력이 [3배] 가 됩니다.");
            Console.WriteLine("    ▶ 앞으로 [두 번의 전투] 에서는 사용할 수 없습니다.\n\n");
            Console.ResetColor();

            Thread.Sleep(600);
        }
    }
}