using System;
using System.Collections.Generic;
using System.Linq;
using Sparta_Dungeon_TeamProject;
using static Sparta_Dungeon_TeamProject.Program;

namespace Sparta_Dungeon_TeamProject
{
    public class Shop
    {
        private Player player;
        private List<Item> shopitems; // 구매용
        private Inventory inventory;
        private Item[] itemDb;

        public Shop(Player player, Inventory inventory, Item[] hereItemList)
        {
            this.player = player;
            this.inventory = inventory;
            this.itemDb = hereItemList;
        }

        // 상점
        public void DisplayShopUI()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold} G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");

                ItemExt.PrintShop(itemDb.ToList(), player, inventory); // 리스트 출력

                Console.WriteLine();
                Console.WriteLine("[1] 아이템 구매");
                Console.WriteLine("[2] 아이템 판매");
                Console.WriteLine("[~`] 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");

                int result = Utils.CheckInput(1, 2);

                switch (result)
                {
                    case -1:
                        return;
                        break;
                    case 1:
                        DisplayBuyUI();
                        break;
                    case 2:
                        DisplaySellUI();
                        break;
                }
            }

        }
        
        // 아이템-구매
        private void DisplayBuyUI()
        {
            while (true)
            {

                Console.Clear();
                Console.WriteLine("상점 - 아이템 구매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold} G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");

                ItemExt.PrintShop(itemDb.ToList(), player, inventory); // 리스트 출력

                Console.WriteLine();
                Console.WriteLine("[~`] 나가기");
                Console.WriteLine();
                Console.WriteLine("구매하실 아이템 번호를 입력하세요.");
                Console.Write(">>");

                int result = Utils.CheckInput(1, itemDb.Length);

                switch (result)
                {
                    case -1:
                        return;
                        break;

                    default:
                        int itemIdx = result - 1;
                        Item targetItem = itemDb[itemIdx];
                        BuyStatus(targetItem);
                        break;
                }
            }

        }
        private void BuyStatus(Item targetItem)
        {
            if (targetItem.Type != 2 && inventory.HasItem(targetItem))
            {
                Console.WriteLine("이미 구매한 아이템입니다.");
                Utils.WaitForEnter();
                return;
            }
            else if (player.Gold < targetItem.Price)
            {
                Console.WriteLine("골드가 부족합니다!");
                Utils.WaitForEnter();
                return;
            }
            else
            {
                player.Gold -= targetItem.Price;
                inventory.AddItem(targetItem);
                Console.WriteLine($"{targetItem.Name}을(를) 구매했습니다!");
                Utils.WaitForEnter();
            }
        }

        // 아이템판매
        private void DisplaySellUI()
        {
            while (true)
            {
                List<Item> items = inventory.GetInventoryItems();

                Console.Clear();
                Console.WriteLine("**상점 - 아이템 판매**");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine($"\n[보유 골드]\n{player.Gold} G\n");
                Console.WriteLine("[아이템 목록]");

                ItemExt.PrintInventory(items, player);

                if (items.Count == 0)
                {
                    Console.WriteLine("\n판매 가능한 아이템이 없습니다. [Enter]");
                    Utils.WaitForEnter();
                    return;
                }

                Console.WriteLine("\n[~`] 나가기");
                Console.WriteLine("판매하실 아이템 번호를 입력하세요");
                Console.Write(">>");
                int result = Utils.CheckInput(0, items.Count);

                if (result == -1)
                {
                    return;
                }

                Item targetItem = items[result - 1];
                int sellPrice = (int)(targetItem.Price * 0.85);

                player.Gold += sellPrice;
                inventory.RemoveItem(targetItem);

                Console.WriteLine($"{targetItem.Name}을(를) {sellPrice} G에 판매했습니다. [Enter]");
                Utils.WaitForEnter();
                return;
            }
        }
    }
}
