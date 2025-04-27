using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sparta_Dungeon_TeamProject;


/*namespace Sparta_Dungeon_TeamProject
{
    class Blacksmith_Skill
    {
        public static void LostFlameSkillEffect()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkBlue;

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



class Wizard_Skill
{
    public static void Burning()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkCyan;

        // 캐릭터 대사
        if (!Messages.Skip) // 스킵 = false 대사 출력
        {
            string[] emotionLines1 =
            {
        "\n\n\n\n    \"진짜야, 나도... 배운 적 있다고!\"",
        "\n\n    \"봐봐, 이거 하나쯤은... 아직 할 수 있어.\"",
        "\n\n    \"하긴... 뭐, 별 건 아니지만.\""
    };

            Messages.PrintLinesWithSkip(emotionLines1, 50, 150);

            if (!Messages.Skip)
            {
                // 회색 메시지 출력
                Console.ForegroundColor = ConsoleColor.DarkGray;
                string[] emotionLines2 =
            {"\n\n\n    - 허세 섞인 외침이, 짧은 순간 불꽃을 품는다.", };

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
            "\n\n\n\n\n\n    ──  스킬 [마법사의 불태우기] 발동 ──",
            "\n\n    흐릿한 마법진이 허공에 그려지고,",
            "\n\n    비뚤어진 불꽃이 느릿하게 적을 향해 날아간다.",
            "\n\n    불꽃이 스치듯 적을 태우고는, 연기만을 남긴다."
        };

        Messages.PrintLinesWithSkip(skillEffectLines, 25, 60);

        // 효과 설명
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n\n    ▶ 1명의 적에게 [피해량] 만큼의 피해를 입힙니다.");
        Console.WriteLine("    ▶ MP를 [소모량] 만큼 소모합니다.\n\n");
        Console.ResetColor();

        Thread.Sleep(600);
    }


    public static void Ice_Shield()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkCyan;

        // 캐릭터 대사
        if (!Messages.Skip) // 스킵 = false 대사 출력
        {
            string[] emotionLines1 =
            {
            "\n\n\n\n    \"마탑 들어가는데 쓴 돈이 얼만데... 이 정도는 해야지.\"",
            "\n\n    \"기초 마법이라도, 제대로 배운 건 맞거든.\"",
            "\n\n    \"하루쯤은, 나도 버틸 수 있어.\""
    };

            Messages.PrintLinesWithSkip(emotionLines1, 50, 150);

            if (!Messages.Skip)
            {
                // 회색 메시지 출력
                Console.ForegroundColor = ConsoleColor.DarkGray;
                string[] emotionLines2 =
            {"\n\n\n    - 어설픈 제스처 속에서, 차가운 기운이 응집된다.", };

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
            "\n\n\n\n\n\n    ──  스킬 [마법사의 얼음방패] 발동 ──",
            "\n\n    서툰 손끝에서 마법진이 천천히 떠오르고,",
            "\n\n    흐릿한 푸른빛이 마법사의 전신을 감싼다.",
            "\n\n    단단하진 않지만, 그 방패는 적어도 한 번은 막아낼 것이다."
        };

        Messages.PrintLinesWithSkip(skillEffectLines, 25, 60);

        // 효과 설명
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n\n    ▶ 1턴 간 적의 모든 공격을 막습니다.");
        Console.WriteLine("    ▶ MP를 [소모량] 만큼 소모합니다.\n\n");
        Console.ResetColor();

        Thread.Sleep(600);
    }
}*/
