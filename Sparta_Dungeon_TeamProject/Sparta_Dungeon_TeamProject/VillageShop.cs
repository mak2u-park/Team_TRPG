﻿using System;
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
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("┏━━━━━━━━━━━━━━━━<< 상점 >>━━━━━━━━━━━━━━━━┓");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("  필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"  [보유 골드] {player.Gold} G");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("  [아이템 목록]");
                Console.ResetColor();
                Console.WriteLine();

                // 아이템 리스트 출력
                ItemExt.PrintShop(Item.ShopItemsDb, player, inventory);
                Console.WriteLine();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
                Console.ResetColor();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("  [1] 아이템 구매");
                Console.WriteLine("  [2] 아이템 판매");
                Console.WriteLine("  [~`] 나가기");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("  원하시는 행동을 입력해주세요.");
                Console.ResetColor();
                Console.Write("\n  >> ");

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
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("┏━━━━━━━━━━━━━━<< 상점 - 아이템 구매 >>━━━━━━━━━━━━━━┓");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("  필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"  [보유 골드] {player.Gold} G");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("  [아이템 목록]");
                Console.ResetColor();
                Console.WriteLine();

                // 아이템 리스트 출력
                ItemExt.PrintShop(itemDb.ToList(), player, inventory);
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("  [~`] 나가기");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("  구매하실 아이템 번호를 입력하세요.");
                Console.ResetColor();

                Console.Write("\n  >> ");

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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  이미 구매한 아이템입니다.");
                Console.ResetColor();
                Utils.WaitForEnter();
                return;
            }
            else if (player.Gold < targetItem.Price)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  골드가 부족합니다!");
                Console.ResetColor();
                Utils.WaitForEnter();
                return;
            }
            else
            {
                player.Gold -= targetItem.Price;
                inventory.AddItem(targetItem);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n  {targetItem.Name}을(를) 구매했습니다!");
                Console.ResetColor();
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
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("┏━━━━━━━━━━━━━━<< 상점 - 아이템 판매 >>━━━━━━━━━━━━━━┓");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("  필요 없는 아이템을 판매할 수 있는 곳입니다.");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"  [보유 골드] {player.Gold} G");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("  [보유 아이템 목록]");
                Console.ResetColor();
                Console.WriteLine();

                ItemExt.PrintInventory(items, player);
                Console.WriteLine();

                if (items.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("  판매 가능한 아이템이 없습니다. [Enter]");
                    Console.ResetColor();
                    Utils.WaitForEnter();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("  [~`] 나가기");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("  판매하실 아이템 번호를 입력하세요.");
                Console.ResetColor();
                Console.Write("\n  >> ");

                int result = Utils.CheckInput(0, items.Count);

                if (result == -1)
                {
                    return;
                }

                Item targetItem = items[result - 1];
                int sellPrice = (int)(targetItem.Price * 0.85);

                player.Gold += sellPrice;
                inventory.RemoveItem(targetItem);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n  {targetItem.Name}을(를) {sellPrice} G에 판매했습니다. [Enter]");
                Console.ResetColor();
                Utils.WaitForEnter();
                return;
            }
        }
    }
}

