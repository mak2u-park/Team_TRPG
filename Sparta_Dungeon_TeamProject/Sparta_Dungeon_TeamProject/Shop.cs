using System;
using System.Collections.Generic;
using System.Linq;
using Sparta_Dungeon_TeamProject;
using static Sparta_Dungeon_TeamProject.Program;

namespace Sparta_Dungeon_TeamProject
{
    public class Shop
    {
        // 상점
        public static void DisplayShopUI()
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
            Console.WriteLine("[1] 아이템 구매");
            Console.WriteLine("[2] 아이템 판매");
            Console.WriteLine("[~`] 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(1, 2);

            switch (result)
            {
                case -1:
                    Messages.ShowMainMenu();
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
            Console.WriteLine("[~`] 나가기");
            Console.WriteLine();
            Console.WriteLine("구매하실 아이템 번호를 입력하세요.");
            Console.Write(">>");

            int result = CheckInput(1, itemDb.Length);

            switch (result)
            {
                case -1:
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

            Console.WriteLine("\n[~`] 나가기");
            Console.WriteLine("판매하실 아이템 번호를 입력하세요");
            Console.Write(">>");
            int result = CheckInput(0, owned.Count);

            if (result == -1)
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
    }
}
