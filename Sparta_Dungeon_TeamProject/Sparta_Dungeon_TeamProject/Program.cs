using System;
using System.Security.Principal;
namespace SpartaDungeon;

class Program
{
    private static Player player;
    private static Item[] itemDb;

    static void Main(string[] args)
    {
        SetData();
        DisplayMainUI();
    }

    // 아이템DB
    static void SetData()
    {
        player = new Player(1, "Chad", "전사", 10, 5, 100, 10000);
        itemDb = new Item[]
        {
            new Item("수련자의 갑옷", 1, 5,"수련에 도움을 주는 갑옷입니다. ",1000),
            new Item("무쇠갑옷", 1, 9,"무쇠로 만들어져 튼튼한 갑옷입니다. ",2000),
            new Item("스파르타의 갑옷", 1, 15,"스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ",3500),
            new Item("낣은 검", 0, 2,"쉽게 볼 수 있는 낡은 검 입니다. ",600),
            new Item("청동 도끼", 0, 5,"어디선가 사용됐던거 같은 도끼입니다. ",1500),
            new Item("스파르타의 창", 0, 7,"스파르타의 전사들이 사용했다는 전설의 창입니다. ",2500)
        };
    }

    // 메인메뉴
    static void DisplayMainUI()
    {
        Console.Clear();
        Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
        Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("1. 상태 보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine("3. 상점");
        Console.WriteLine("4. 던전입장");
        Console.WriteLine("5. 휴식하기");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");

        int result = CheckInput(1, 5);

        switch (result)
        {
            case 1:
                DisplayStatUI();
                break;
            case 2:
                DisplayInventoryUI();
                break;
            case 3:
                DisplayShopUI();
                break;
            case 4:
                DisplayDungeonUI();
                break;
            case 5:
                DisplayRestUI();
                break;
        }
    }

    // 상태보기
    static void DisplayStatUI()
    {
        Console.Clear();
        Console.WriteLine("상태 보기");
        Console.WriteLine("캐릭터의 정보가 표시됩니다.");

        player.DisplayCharacterInfo();

        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");

        int result = CheckInput(0, 0);

        switch (result)
        {
            case 0:
                DisplayMainUI();
                break;
        }
    }

    // 인벤토리
    static void DisplayInventoryUI()
    {
        Console.Clear();
        Console.WriteLine("인벤토리");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        player.DisplayInventory(false);

        Console.WriteLine();
        Console.WriteLine("1. 장착 관리");
        Console.WriteLine("0. 나가기");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");

        int result = CheckInput(0, 1);

        switch (result)
        {
            case 0:
                DisplayMainUI();
                break;

            case 1:
                DisplayEquipUI();
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

        player.DisplayInventory(true);

        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");

        int result = CheckInput(0, player.InventoryCount);

        switch (result)
        {
            case 0:
                DisplayInventoryUI();
                break;

            default:

                int itemIdx = result - 1;
                Item targetItem = itemDb[itemIdx];
                player.EquipItem(targetItem);

                DisplayEquipUI();
                break;
        }
    }

    // 상점
    static void DisplayShopUI()
    {
        Console.Clear();
        Console.WriteLine("상점");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
        Console.WriteLine();
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{player.Gold} G");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < itemDb.Length; i++)
        {
            Item curItem = itemDb[i];

            string displayPrice = (player.HasItem(curItem) ? "구매완료" : $"{curItem.Price} G");
            Console.WriteLine($"- {curItem.ItemInfoText()}  |  {displayPrice}");
        }

        Console.WriteLine();
        Console.WriteLine("1. 아이템 구매");
        Console.WriteLine("2. 아이템 판매");
        Console.WriteLine("0. 나가기");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");

        int result = CheckInput(0, 1);

        switch (result)
        {
            case 0:
                DisplayMainUI();
                break;
            case 1:
                DisplayBuyUI();
                break;
            case 2:
                DisplaySellUI();
                break;
        }
    }

    // 아이템-구매
    static void DisplayBuyUI()
    {
        Console.Clear();
        Console.WriteLine("상점 - 아이템 구매");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
        Console.WriteLine();
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{player.Gold} G");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < itemDb.Length; i++)
        {
            Item curItem = itemDb[i];

            string displayPrice = (player.HasItem(curItem) ? "구매완료" : $"{curItem.Price} G");
            Console.WriteLine($"- {i + 1} {curItem.ItemInfoText()}  |  {displayPrice}");
        }

        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");

        int result = CheckInput(0, itemDb.Length);

        switch (result)
        {
            case 0:
                DisplayShopUI();
                break;

            default:
                int itemIdx = result - 1;
                Item targetItem = itemDb[itemIdx];

                if (player.HasItem(targetItem))
                {
                    Console.WriteLine("이미 구매한 아이템입니다.");
                    Console.WriteLine("Enter 를 눌러주세요.");
                    Console.ReadLine();
                }
                else // 구매가능
                {
                    if (player.Gold >= targetItem.Price)
                    {
                        Console.WriteLine("구매를 완료했습니다.");
                        player.BuyItem(targetItem);
                    }
                    else
                    {
                        Console.WriteLine("골드가 부족합니다.");
                        Console.WriteLine("Enter 를 눌러주세요.");
                        Console.ReadLine();
                    }
                }
                DisplayBuyUI();
                break;
        }
    }

    // 아이템판매
    static void DisplaySellUI()
{
    Console.Clear();
    Console.WriteLine("**상점 - 아이템 판매**");
    Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
    Console.WriteLine($"\n[보유 골드]\n{player.Gold} G\n");
    Console.WriteLine("[아이템 목록]");

    List<Item> sellableItems = player.GetInventoryItems();

    List<Item> owned = new List<Item>();
    for (int i = 0; i < sellableItems.Count; i++)
    {
        var item = sellableItems[i];
        int sellPrice = (int)(item.Price * 0.85);
        owned.Add(item);
        Console.WriteLine($"- {i + 1} {item.ItemInfoText()} | {sellPrice} G");
    }

    if (owned.Count == 0)
    {
        Console.WriteLine("\n판매 가능한 아이템이 없습니다. [Enter]");
        Console.ReadLine();
        DisplayShopUI();
        return;
    }

    Console.WriteLine("\n0. 나가기");
    Console.WriteLine("원하시는 행동을 입력해주세요.");
    int result = CheckInput(0, owned.Count);

    if (result == 0)
    {
        DisplayShopUI();
        return;
    }

    var targetItem = owned[result - 1];

    if (player.IsEquipped(targetItem))
        player.EquipItem(targetItem); // 해제

    int goldReceived = (int)(targetItem.Price * 0.85);
    player.SellItem(targetItem);
    Console.WriteLine($"{targetItem.Name}을(를) 판매하여 {goldReceived} G를 획득했습니다. [Enter]");
    Console.ReadLine();
    DisplaySellUI();
}

    static int CheckInput(int min, int max)
    {
        int result;
        while (true)
        {
            string input = Console.ReadLine();
            bool isNumber = int.TryParse(input, out result);
            if (isNumber)
            {
                if (result >= min && result <= max)
                    return result;
            }
            Console.WriteLine("잘못된 입력입니다!!!!");
        }
    }

    // 던전
    static void DisplayDungeonUI()
    {

    }

    // 휴식
    static void DisplayRestUI()
    {
        Console.Clear();
        Console.WriteLine("**휴식하기**");
        Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.Gold} G)");
        Console.WriteLine("1. 휴식하기");
        Console.WriteLine("0. 나가기");
        Console.WriteLine("\n원하시는 행동을 입력해주세요.");

        int result = CheckInput(0, 1);
        switch (result)
        {
            case 0:
                DisplayMainUI();
                break;
            case 1:
                if (player.Gold >= 500)
                {
                    player.Rest(); // 체력 회복
                    Console.WriteLine("휴식을 완료했습니다. [Enter]");
                }
                else
                {
                    Console.WriteLine("Gold 가 부족합니다. [Enter]");
                }
                Console.ReadLine();
                DisplayMainUI();
                break;
        }
    }
}