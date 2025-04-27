using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Sparta_Dungeon_TeamProject.Program;

namespace Sparta_Dungeon_TeamProject
{
    internal class Village
    {
        bool showTitle = false;
        private Player player;
        private Inventory inventory;
        Inn inn;
        Shop shop;
        VillageBS BlackSmithShop;
        public void MainScene()
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[1] 상태 보기");
            Console.WriteLine("[2] 인벤토리");
            Console.WriteLine("[3] 상점");
            Console.WriteLine("[4] 던전입장");
            Console.WriteLine("[5] 여관");
            Console.WriteLine("[6] 대장간");
            Console.WriteLine("[~`] 게임종료");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = Utils.CheckInput(1, 6);

            switch (result)
            {
                case 1:
                    player.DisplayPlayerInfo();
                    break;
                case 2:
                    BlackSmithShop.DisplayInventoryUI();
                    break;
                case 3:
                    shop.DisplayShopUI();
                    break;
                case 4:
                    //DisplayDungeonUI(Chapter);
                    break;
                case 5:
                    inn.DisplayInnUI();
                    break;
                    case 6:
                        BlackSmithShop.UpgradeItemUI();
                    break;
                case -1:
                    Console.WriteLine("게임을 종료합니다.");
                    Thread.Sleep(1000);
                    // `키를 눌러 종료
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
