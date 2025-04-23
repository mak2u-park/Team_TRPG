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
            Console.WriteLine("2. 강화하기");
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

        // 강화하기
        static void UpgradeItemUI()
        {
            Console.Clear();
            Console.WriteLine("인벤토리 - 아이템 강화");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            player.InventoryItemList(true);

            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("강화를 시도할 아이템 번호를 입력하세요");
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

                    int cost = targetItem.Value < 20 ? 100 : 200; // 출력용 별도 재판단 필요.
                    bool isSuccess = player.UpgradeItem(targetItem);

                    Console.WriteLine();

                    if (isSuccess) // 강화 성공!
                    {
                        Console.WriteLine($"[{targetItem.Name}] 아이템이 강화되었습니다.");
                        Console.WriteLine($"현재 능력치 : {targetItem.Value}");
                        Console.WriteLine($"현재 골드 : {player.Gold} G");
                    }
                    else // 강화 실패
                    {
                        if (targetItem.Value >= targetItem.MaxValue)
                        {
                            Console.WriteLine($"[{targetItem.Name}] 아이템은 최대 능력치에 도달했습니다.");
                            Console.WriteLine($"현재 능력치 : {targetItem.Value}");
                            Console.WriteLine($"현재 골드 : {player.Gold} G");

                        }
                        else if (player.Gold < cost)
                        {
                            Console.WriteLine($"현재 골드: {player.Gold} G");
                            Console.WriteLine("골드가 부족합니다.");
                            Console.WriteLine();
                            Console.WriteLine("Enter 를 눌러주세요.");
                            Console.ReadLine();

                            DisplayInventoryUI();
                        }
                        else
                        {
                            Console.WriteLine($"아이템 강화에 실패했습니다.");
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine("Enter 를 눌러주세요.");
                    Console.ReadLine();

                    UpgradeItemUI();
                    break;
            }
        }
    }
}
