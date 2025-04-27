using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Sparta_Dungeon_TeamProject
{
    internal class VillageBS
    {
        Player player = Player.Instance;
        Inventory inventory = Inventory.Instance;
        // 강화 제련소
        public void UpgradeItemUI()
        {
            // 고정 UI
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[속죄의 제련소]");
            Starter starter = new Starter();
            
            
            if (!firstVisitFlags["강화"]) // 첫 방문
            {
               
                Thread.Sleep(1000);
                Console.WriteLine("대장장이: 오, 반갑네! 이 동네에선 처음 보는 얼굴인데?"); // 아재 대장장이
                Console.WriteLine("         나는 아이템을 강화시키는 걸 도와주고 있다네~!");
                Thread.Sleep(1000);
                Console.WriteLine("         골드가 좀 들긴 하지만, 능력치는 확실히 올라가지.");
                Console.WriteLine("         물론… 실패할 때도 있어! 하하하, 내 탓하진 말게나~");
                Program.firstVisitFlags["강화"] = true;
            }
            else // 재방문
            {
                Console.WriteLine("대장장이: 이번엔 어떤걸 강화해줄까?");
            }

            showTitle = true; //UI 고정/ 아래는 변경 값으로 재반영

            Thread.Sleep(500);
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            var upgradable = items.Where(i => i.Type == 0 || i.Type == 1).ToList();
            upgradable.PrintEquipStatus(player); // 무기, 방어구

            Console.WriteLine("\n\n");
            int guideLine = Console.CursorTop; // 하단 지우기용, 위치 저장

            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G");
            Console.WriteLine();
            Console.WriteLine("[~`] 다음에 다시 올게요.");
            Console.WriteLine();
            Console.WriteLine("대장장이에게 보여줄 아이템 번호를 입력하세요.");
            Console.Write(">>");

            int result = Utils.CheckInput(1, upgradable.Count);
            if (result == -1)
            {
                Console.WriteLine("대장장이: 그래 다음에 또 봅세!");
                Thread.Sleep(500);
                return;
            }

            // 선택한 아이템
            var targetItem = upgradable[result - 1];
            int cost = player.GetUpgradeCost(targetItem);
            int valueUp = player.GetUpgradeValue(targetItem);

            DisplayUpgradeResult(targetItem, cost, valueUp, guideLine);
        }
    }

}
