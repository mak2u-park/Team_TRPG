using System.Reflection.Metadata.Ecma335;

namespace Sparta_Dungeon_TeamProject
{
    public partial class Program
    {
        // 인벤토리
        static void DisplayInventoryUI()
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            player.InventoryItemList(false);

            Console.WriteLine();
            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("2. 속죄의 제련소");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, 2);

            switch (result)
            {
                case 0:
                    DisplayMainUI();
                    break;
                case 1:
                    DisplayEquipUI();
                    break;
                case 2:
                    UpgradeItemUI();
                    break;
            }
        }

        // 장착관리
        static void DisplayEquipUI()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("인벤토리 - 장착관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            player.InventoryItemList(true);

            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("장착하실 아이템 번호를 입력하세요");
            Console.Write(">>");

            int result = CheckInput(0, player.InventoryCount);

            switch (result)
            {
                case 0:
                    DisplayInventoryUI();
                    break;

                default:

                    int itemIdx = result - 1;

                    List<Item> inventory = player.GetInventoryItems();
                    Item targetItem = inventory[itemIdx];
                    player.EquipItem(targetItem);

                    DisplayEquipUI();
                    break;
            }
        }

        // 강화 제련소
        static void UpgradeItemUI(bool showTitle = false)
        {
            // 고정 UI
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[속죄의 제련소]");

            if (!firstVisitFlags["강화"]) // 첫 방문
            {
                Thread.Sleep(1000);
                Console.WriteLine("대장장이: 오, 반갑네! 이 동네에선 처음 보는 얼굴인데?"); // 아재 대장장이
                Console.WriteLine("         나는 아이템을 강화시키는 걸 도와주고 있다네~!");
                Thread.Sleep(1000);
                Console.WriteLine("         골드가 좀 들긴 하지만, 능력치는 확실히 올라가지.");
                Console.WriteLine("         물론… 실패할 때도 있어! 하하하, 내 탓하진 말게나~");
                firstVisitFlags["강화"] = true;
            }
            else // 재방문
            {
                Console.WriteLine("대장장이: 이번엔 어떤걸 강화해줄까?");
            }
            showTitle = true; //UI 고정/ 아래는 변경 값으로 재반영

            Thread.Sleep(500);
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            player.InventoryItemList(true);

            Console.WriteLine("\n\n");
            int guideLine = Console.CursorTop; // 하단 지우기용, 위치 저장

            Console.WriteLine("0. 다음에 다시 올게요.");
            Console.WriteLine();
            Console.WriteLine("대장장이에게 보여줄 아이템 번호를 입력하세요.");
            Console.Write(">>");

            int result = CheckInput(0, player.InventoryCount);
            if (result == 0)
            {
                Console.WriteLine("대장장이: 그래 다음에 또 봅세!");
                Thread.Sleep(500);
                DisplayInventoryUI();
                return;
            }

            Item targetItem = player.GetInventoryItems()[result - 1]; // 선택한 아이템
            int cost = player.GetUpgradeCost(targetItem); // 강화 비용
            int valueUp = player.GetUpgradeValue(targetItem); // 강화 능력치 증가량

            DisplayUpgradeResult(targetItem, cost, valueUp, guideLine); // 강화 결과 출력
        }


        // 강화 결과 출력
        static void DisplayUpgradeResult(Item targetItem, int cost, int valueUp, int guidLine)
        {
            ClearBottom(guidLine, 10);
            Console.SetCursorPosition(0, guidLine);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{targetItem.Name}] 이걸 강화하겠다고?");
            Console.WriteLine($"비용은 {cost} G 면, 충분하네");
            Console.WriteLine($"잘 되면 {targetItem.DisplayTypeText}이 {valueUp}만큼 올라가고, \n망하면… 뭐, 나도 먹고 살아야하지 않겠나.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1. 강화해주세요!");
            Console.WriteLine("2. 음... 이거 말고, 다른 거 고를게요.");
            Console.WriteLine("0. 다음에 다시 올게요.");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력하세요.");
            Console.Write(">>");

            int input = CheckInput(1, 2);

            ClearBottom(guidLine, 10);
            Console.SetCursorPosition(0, guidLine);

            if (input == 1) // 강화 성공
            {
                bool isSuccess = player.UpgradeItem(targetItem); // 강화 성공 여부

                Console.ForegroundColor = ConsoleColor.Yellow;
                if (isSuccess) // 강화 성공!
                {
                    Console.WriteLine($"[{targetItem.Name}] 강화 성공! 이야~ 잘 터졌구만!");
                    Console.WriteLine($"{targetItem.DisplayTypeText}이 {targetItem.Value} 상승했다네!");
                }
                else if (targetItem.Value == targetItem.MaxValue) // 강화 실패!
                {
                    Console.WriteLine($"[{targetItem.Name}] 아이템은 이미 최대로 강화됐어.");
                    Console.WriteLine($"현재 능력치 : {targetItem.Value}");
                }
                else
                {
                    Console.WriteLine($"현재 골드: {player.Gold} G");
                    Console.WriteLine("아이고, 골드가 부족한데? 준비 좀 더 하고 오게.");
                }
                Console.WriteLine("Loding...");
                Thread.Sleep(500);
                UpgradeItemUI();
            }
            else if (input == 2) // 뒤로가기 
            {
                Console.WriteLine("대장장이: 고민이 많구만~! 그럼 이건 돌려주겠네");
                Console.WriteLine("Loding...");
                Thread.Sleep(500);
                UpgradeItemUI();
            }
            else if (input == -1) // 나가기
            {
                Console.WriteLine("대장장이: 그래, 다음에 또 봅세!");
                Console.WriteLine("Loding...");
                Thread.Sleep(500);
                DisplayInventoryUI();
            }
        }
    }
}
