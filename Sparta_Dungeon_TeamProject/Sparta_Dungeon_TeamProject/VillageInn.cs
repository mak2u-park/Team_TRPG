using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparta_Dungeon_TeamProject
{

    internal class VillageInn
    {
        private Inventory inventory;
        private Player player;
        QuestManager questManager = new QuestManager();

        public VillageInn(Inventory inventory, Player player)
        {
            this.inventory = inventory;
            this.player = player;
        }
        
        public void DisplayInnUI()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("여관");
                Console.WriteLine("휴식을 취할 수 있는 여관입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold} G");
                Console.WriteLine();
                Console.WriteLine("[여관 이용안내]");
                Console.WriteLine("[1] 휴식하기 (체력 회복)");
                Console.WriteLine("[2] 퀘스트 확인하기");
                Console.WriteLine("[~`] 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                int result = Utils.CheckInput(1, 2);
                switch (result)
                {
                    case -1:
                        return;
                    case 1:
                        DisplayRestUI();
                        break;
                    case 2:
                        questManager.QuestList();
                        break;
                }
            }
        }

        //여관
        public void DisplayRestUI()
        {
            Console.Clear();
            Console.WriteLine("** 여관 **");
            Console.WriteLine($"보유 골드 : {player.Gold} G\n");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("여관 주인: 안녕하세요, 여관에 오신 것을 환영합니다.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[체력 회복 메뉴]");
            Console.WriteLine("[1] 따뜻한 죽 한 그릇 (300G, +20 HP)");
            Console.WriteLine("[2] 고기 듬뿍 스튜 (700G, +50 HP)");
            Console.WriteLine("[3] 푸짐한 정식 (1200G, 전체 회복)");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("[마나 회복 메뉴]");
            Console.WriteLine("[4] 1세대 실험약 (300G, +30 MP)");
            Console.WriteLine("[5] 강화형 실험약 (700G, +80 MP)");
            Console.WriteLine("[6] 미공개 프로토타입 (1200G, 전체 회복)");
            Console.ResetColor();
            Console.WriteLine();

            int guideLine = Console.CursorTop;

            Console.WriteLine("[~`] 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");

            while (true)
            {
                Console.SetCursorPosition(0, guideLine);
                Utils.ClearBottom(guideLine, 10);
                Console.Write(">> ");
                int result = Utils.CheckInput(1, 6);

                switch (result)
                {
                    case -1:
                        return;
                    case 1:
                        if (player.Heal(300, 20))
                        {
                            Console.WriteLine("\n죽을 먹고 따뜻해졌다!");
                        }
                        else
                        {
                            Console.WriteLine("골드가 부족합니다.");
                        }
                        break;
                    case 2:
                        if (player.Heal(700, 50))
                            Console.WriteLine("\n고기 듬뿍 스튜를 먹고 힘이 솟는다!");
                        else Console.WriteLine("골드가 부족합니다.");
                        break;
                    case 3:
                        if (player.Heal(1200, player.MaxHp))
                            Console.WriteLine("\n정식을 먹고 체력이 전부 회복되었다!");
                        else Console.WriteLine("골드가 부족합니다.");
                        break;
                    case 4:
                        if (player.GainMp(300, 30))
                            Console.WriteLine("\n실험약을 마시고 정신이 또렷해진다.");
                        else Console.WriteLine("골드가 부족합니다.");
                        break;
                    case 5:
                        if (player.GainMp(700, 80))
                            Console.WriteLine("\n강화형 약물이 효과를 발휘했다!");
                        else Console.WriteLine("골드가 부족합니다.");
                        break;
                    case 6:
                        if (player.GainMp(1200, player.MaxMp))
                            Console.WriteLine("\nMP가 완전히 회복되었습니다.");
                        else Console.WriteLine("골드가 부족합니다.");
                        break;
                }
                Utils.WaitForEnter();
            }
        }
    }
}

