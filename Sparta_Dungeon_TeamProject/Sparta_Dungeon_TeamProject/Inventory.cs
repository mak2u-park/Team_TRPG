using System;
using System.Collections.Generic;
using System.Linq;
using Sparta_Dungeon_TeamProject;
using static Sparta_Dungeon_TeamProject.Program;

namespace Sparta_Dungeon_TeamProject
{
    // 인벤토리 전용 UI
    public class Inventory
    {
        private static Inventory _instance;
        private List<Item> items = new();
        private Player player = Player.Instance;

        // 초기화: 직업별 시작 아이템
        public Inventory(Starter starter, Player player, IEnumerable<Item> initialItems)
        {
            this.player = player;
            this.items = new List<Item>(initialItems);
        }

        private Inventory()
        {
            items = new List<Item>();
        }
        public static Inventory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Inventory();
                }
                return _instance;
            }
        }
        public void AddItem(Item item) => items.Add(item); // 아이템 추가
        public void RemoveItem(Item item) => items.Remove(item);
        public List<Item> GetItems() => new List<Item>(items); // 아이템 복사본 반환

        // 인벤토리
        public void DisplayInventoryUI()
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("==아이템 목록==");

            items.PrintEquipStatus(player); // 전체목록 + 장착상태

            Console.WriteLine();
            Console.WriteLine("[1] 장착 관리");
            Console.WriteLine("[2] 속죄의 제련소");
            Console.WriteLine("[3] 아이템 사용");
            Console.WriteLine("[~`] 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = Utils.CheckInput(1, 3);

            switch (result)
            {
                case -1:
                    return;
                    break;
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

        // 장착관리
        public void DisplayEquipUI()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[장착관리]");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("==아이템 목록==");

            var equItems = items.Where(i => i.Type == 0 || i.Type == 1 || i.Type == 3).ToList();
            equItems.PrintEquipStatus(player); // 무기, 방어구, 장신구

            Console.WriteLine();
            Console.WriteLine("[~`] 나가기");
            Console.WriteLine();
            Console.WriteLine("장착하실 아이템 번호를 입력하세요");
            Console.Write(">>");

            int result = Utils.CheckInput(1, items.Count);

            if (result == -1)
            {
                return;
            }

            var target = equItems[result - 1];
            if (player.IsEquipped(target))
            {
                player.UnequipItem(target);
            }
            else
            {
                player.EquipItem(target);
            }
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
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[1] 강화해주세요!");
            Console.WriteLine("[2] 음... 이거 말고, 다른 거 고를게요.");
            Console.WriteLine("[~`] 다음에 다시 올게요.");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력하세요.");
            Console.Write(">>");

            int input = Utils.CheckInput(1, 2);

            Utils.ClearBottom(guidLine, 10);
            Console.SetCursorPosition(0, guidLine);

            if (input == 1) // 강화 성공
            {
                bool isSuccess = player.UpgradeItem(targetItem); // 강화 성공 여부

                Console.ForegroundColor = ConsoleColor.Yellow;
                if (isSuccess) // 강화 성공!
                {
                    Console.WriteLine($"[{targetItem.Name}] 강화 성공! 이야~ 잘 터졌구만!");
                    Console.WriteLine($"{targetItem.ItemInfoText()}: {targetItem.TotalValue}");
                    if (player.IsEquipped(targetItem))
                    {
                        player.GetUpgradeStat(targetItem, valueUp);
                    }
                }
                else if (targetItem.TotalValue >= targetItem.MaxValue) // 강화 실패!
                {
                    Console.WriteLine($"[{targetItem.Name}] 아이템은 이미 최대로 강화됐어.");
                    Console.WriteLine($"{targetItem.ItemInfoText()}: {targetItem.TotalValue}");
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

        void UseItemUI()
        {
            Console.Clear();
            Console.WriteLine("인벤토리 - 아이템 사용");
            Console.WriteLine("사용 가능한 아이템 목록입니다.");

            var usable = items.Where(i => i.Type == 2).ToList();
            usable.PrintEquipStatus(player);

            Console.WriteLine();
            Console.WriteLine("[~`] 나가기\n");
            Console.Write("사용할 아이템 번호를 입력하세요: ");

            int result = Utils.CheckInput(1, usable.Count);

            if (result == -1)
            {
                DisplayInventoryUI();
                return;
            }

            var targetItem = usable[result - 1];
            targetItem.UseItem(player);

            Console.WriteLine($"{targetItem.Name}을(를) 사용했습니다!");

            Utils.WaitForEnter();
            UseItemUI();
        }
    }
}