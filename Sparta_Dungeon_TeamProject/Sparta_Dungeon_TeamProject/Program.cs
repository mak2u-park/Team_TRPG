using System;
using System.Security.Principal;

namespace Sparta_Dungeon_TeamProject
{
    public partial class Program
    {
        private static Player player;
        private static Item[] itemDb;

        // ** 실제 구동되는 메인함수 **
        static void Main(string[] args)
        {
            // 인트로
            Console.Clear();
            Console.WriteLine("인트로");
            Console.WriteLine("6즙폭발");
            Console.WriteLine("Enter 눌러서 시작하기");
            Console.ReadLine();

            // 게임 시작
            SetData();
            DisplayMainUI();
        }

        // 기본 세팅
        static void SetData()
        {
            // 플레이어 이름, 직업 세팅 - 직업 구체화 필요.
            Console.Clear();
            Console.WriteLine("캐릭터를 생성합니다.");
            Console.WriteLine("캐릭터 이름을 입력해주세요.");
            string name = Console.ReadLine();
            Console.WriteLine("캐릭터 직업을 선택해주세요.");
            string job = Console.ReadLine();

            // 플레이어 기본 지급 - 레벨, 이름, 직업, 공격력, 방어력, 체력, 골드
            player = new Player(1, name, job, 10, 5, 100, 10000);

            // 아이템 DB
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

        // 0. 메인메뉴
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

        // 1. 상태보기
        static void DisplayStatUI()
        {
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");

            player.DisplayPlayerInfo();

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

        // 5. 휴식
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
}