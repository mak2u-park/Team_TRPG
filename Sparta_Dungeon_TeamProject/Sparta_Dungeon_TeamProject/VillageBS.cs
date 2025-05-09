﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Sparta_Dungeon_TeamProject
{
    public class VillageBS
    {
        private Player player;
        private Inventory inventory;
        private Dictionary<string, bool> firstVisitFlags = new()
        {
            { "강화", false },
            { "상점", false },
            { "여관", false },
            { "던전", false },
            { "휴식", false }
        };

        public VillageBS(Player player, Inventory inventory)
        {
            this.player = player;
            this.inventory = inventory;
        }

        // 인벤토리 UI
        public void DisplayInventoryUI()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("┏━━━━━━━━━━━━━━━━━━━━━━<< 인벤토리 >>━━━━━━━━━━━━━━━━━━━━━━┓");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("  보유 중인 아이템을 관리할 수 있습니다.");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("  == 아이템 목록 ==");
                Console.ResetColor();
                Console.WriteLine();

                ItemExt.PrintInventory(inventory.GetInventoryItems(), player);

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
                Console.ResetColor();
                Console.WriteLine();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("  [1] 장착 관리");
                Console.WriteLine("  [2] 속죄의 대장간");
                Console.WriteLine("  [3] 아이템 사용");
                Console.WriteLine("  [~`] 나가기");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                int result = Utils.CheckInput(1, 3);

                switch (result)
                {
                    case -1:
                        return;
                    case 1:
                        DisplayEquipUI();
                        break;
                    case 2:
                        UpgradeItemUI();
                        break;
                    case 3:
                        UseItemUI();
                        break;
                }
            }
        }

        // 장착관리 UI
        public void DisplayEquipUI()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("┏━━━━━━━━━━━━━━━━━━━━━━<< 인벤토리 - 장착 관리 >>━━━━━━━━━━━━━━━━━━━━━━┓");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("  장착 가능한 아이템을 관리할 수 있습니다.");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("  == 장착 가능한 아이템 목록 ==");
                Console.ResetColor();
                Console.WriteLine();    

                // 무기, 방어구, 장신구
                var equippableItems = inventory.GetInventoryItems().Where(x => x.Type == 0 || x.Type == 1 || x.Type == 3).ToList();
                ItemExt.PrintInventory(equippableItems, player);

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
                Console.ResetColor();
                Console.WriteLine();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("  [~`] 나가기");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("장착/해제할 아이템 번호를 입력하세요");
                Console.Write(">>");

                int result = Utils.CheckInput(1, equippableItems.Count);

                switch (result)
                {
                    case -1:
                        return;
                    default:
                        var targetItem = equippableItems[result - 1];
                        if (player.IsEquipped(targetItem))
                        {
                            equippableItems.Remove(targetItem);
                            player.UnequipItem(targetItem);
                        }
                        else if (targetItem.Type == 0 || targetItem.Type == 1 || targetItem.Type == 3)
                        {
                            equippableItems.Add(targetItem);
                            player.EquipItem(targetItem);
                        }
                        DisplayEquipUI();
                        break;
                }
            }
        }

        // 대장간
        public void UpgradeItemUI(bool showTitle = false)
        {
            // 고정 UI
            Console.Clear();
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("[속죄의 대장간]");
            Console.WriteLine();

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
            Console.WriteLine("[아이템 목록]");

            var upgradableItems = inventory.GetInventoryItems().Where(x => x.Type == 0 || x.Type == 1 || x.Type == 3).ToList();
            ItemExt.PrintInventory(upgradableItems, player);

            Console.WriteLine("\n\n");
            int guideLine = Console.CursorTop; // 하단 지우기용, 위치 저장

            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G");
            Console.WriteLine();
            Console.WriteLine("[~`] 다음에 다시 올게요.");
            Console.WriteLine();
            Console.WriteLine("대장장이에게 보여줄 아이템 번호를 입력하세요.");
            Console.Write(">>");

            int result = Utils.CheckInput(1, inventory.GetInventoryItems().Count);
            if (result == -1)
            {
                Console.WriteLine("대장장이: 그래 다음에 또 봅세!");
                Thread.Sleep(500);
                return;
            }

            // 선택한 아이템
            Item targetItem = inventory.GetInventoryItems()[result - 1];
            int cost = player.UpgradeCost(targetItem);
            int valueUp = player.UpgradeValue(targetItem);

            DisplayUpgradeResult(targetItem, cost, valueUp, guideLine);
        }

        // 강화 결과 출력
        void DisplayUpgradeResult(Item targetItem, int cost, int valueUp, int guidLine)
        {
            Utils.ClearBottom(guidLine, 10);
            Console.SetCursorPosition(0, guidLine);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{targetItem.Name}] 이걸 강화하겠다고?");
            Console.WriteLine($"비용은 {cost} G 면, 충분하네");
            Console.WriteLine($"잘 되면 능력치가 {valueUp}만큼 올라가고, \n망하면… 뭐, 나도 먹고 살아야하지 않겠나.");
            Console.WriteLine();
            Console.ResetColor();
            Console.WriteLine("[1] 강화해주세요!");
            Console.WriteLine("[2] 음... 이거 말고, 다른 거 고를게요.");
            Console.WriteLine("[~`] 다음에 다시 올게요.");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력하세요.");
            Console.Write(">>");

            int input = Utils.CheckInput(-1, 2);

            Utils.ClearBottom(guidLine, 10);
            Console.SetCursorPosition(0, guidLine);

            if (input == 1) // 강화 성공
            {
                bool isSuccess = player.UpgradeItem(targetItem); // 강화 성공 여부

                Console.ForegroundColor = ConsoleColor.Yellow;
                if (isSuccess) // 강화 성공!
                {
                    Console.WriteLine($"[{targetItem.Name}] 강화 성공! 이야~ 잘 터졌구만!");
                    Console.WriteLine($"{targetItem.ItemTypeTextInfo()}: {targetItem.TotalValue}");
                    if (player.IsEquipped(targetItem))
                    {
                        player.UpgradeStat(targetItem, valueUp);
                    }
                }
                else if (targetItem.TotalValue >= targetItem.MaxValue) // 강화 실패!
                {
                    Console.WriteLine($"[{targetItem.Name}] 아이템은 이미 최대로 강화됐어.");
                    Console.WriteLine($"{targetItem.ItemTypeTextInfo()}: {targetItem.TotalValue}");
                }
                else
                {
                    Console.WriteLine($"현재 골드: {player.Gold} G");
                    Console.WriteLine("아이고, 골드가 부족한데?");
                }
                Console.WriteLine("Loding...");
                Thread.Sleep(300);
                Console.ResetColor();
            }
            else if (input == 2) // 뒤로가기 
            {
                Console.WriteLine("대장장이: 고민이 많구만~! 그럼 이건 돌려주겠네");
                Console.WriteLine("Loding...");
                Thread.Sleep(300);
                Console.ResetColor();
            }
            else if (input == -1) // 나가기
            {
                Console.WriteLine("대장장이: 그래, 다음에 또 봅세!");
                Console.WriteLine("Loding...");
                Thread.Sleep(300);
                Console.ResetColor();
                return;
            }
            UpgradeItemUI(); // 재귀 호출 -어차피 여기 ui에서 나가면 retrun됨- 인벤으로 나가면 스택 클린됨.
        }


        // 아이템 사용 UI - 소모품만 type 2
        public void UseItemUI()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("[인벤토리 - 아이템 사용]");
                Console.WriteLine("사용 가능한 소모품 목록입니다.");
                Console.WriteLine();

                var usableItems = inventory.GetInventoryItems().Where(x => x.Type == 2).ToList();
                ItemExt.PrintInventory(usableItems, player);

                Console.WriteLine();
                Console.WriteLine("[~`] 나가기");
                Console.WriteLine();
                Console.WriteLine("사용할 아이템 번호를 입력하세요.");
                Console.Write(">> ");

                int result = Utils.CheckInput(1, usableItems.Count);

                if (result == -1)
                {
                    return;
                }

                Item targetItem = usableItems[result - 1];
                targetItem.UseItem(player);
                inventory.RemoveItem(targetItem);

                Console.WriteLine($"{targetItem.Name}을(를) 사용했습니다!");
                Thread.Sleep(800);
                continue;
            }
        }
    }
}
