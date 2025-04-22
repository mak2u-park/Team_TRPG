using System;
using System.Security.Principal;

namespace Sparta_Dungeon_TeamProject
{
    public partial class Program
    {
        private static Player player;
        private static Item[] itemDb = Array.Empty<Item>(); // 임시초기값. 이후 덮어씌워짐ok

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

        // A. 기본 세팅
        static void SetData()
        {
            // 이름, 직업 세팅
            string name;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("캐릭터를 생성합니다.");
                Console.WriteLine("캐릭터 이름을 입력해주세요.");
                Console.Write("이름: ");
                name = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(name))
                {
                    break;
                }
                Console.Write("이름: ");
            }

            Console.Clear();
            Console.WriteLine("캐릭터 직업을 선택해주세요.");

            // foreach 반복문으로 직업 개수 무관하게 모두 출력됨.
            // # 맨 아래 직업 DB에서 수정하는대로 자동 반영.
            foreach (JobType job in Enum.GetValues(typeof(JobType)))
            {
                Console.WriteLine($"{(int)job}. {job}");
            }
            Console.WriteLine();
            Console.Write("번호입력: ");
            int result = CheckInput(1, 3);

            JobType jobType = (JobType)result;

            // 플레이어 기본 지급 - 레벨, 경험치, 이름, 직업, 공격력, 방어력, 체력, 골드
            player = new Player(1, 0, 100, name, jobType, 10, 5, 100, 10000);

            InitItemDb(); // 아이템 세팅 호출
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
            Console.WriteLine("0. 게임종료");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, 5);

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
                case 0:
                    Console.WriteLine("게임을 종료합니다.");
                    Thread.Sleep(1000);
                    Environment.Exit(0);
                    break;
            }
        }

        // 1. 상태보기 # Player.cs
        static void DisplayStatUI()
        {
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine();

            player.DisplayPlayerInfo(); // # Player.cs

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

        // B. 입력값 체크
        static int CheckInput(int min, int max)
        {
            int result;
            while (true)
            {
                string input = Console.ReadLine()!;
                bool isNumber = int.TryParse(input, out result);
                if (isNumber)
                {
                    if (result >= min && result <= max)
                        return result;
                }
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }

    // 직업 DB # SetData()
    public enum JobType
    {
        직업1 = 1,
        직업2,
        직업3
    }

    public class JobData
    {
        public int BaseAtk { get; }
        public int BaseDef { get; }
        public string[] Skills { get; }

        public JobData(int atk, int def, string[] skills)
        {
            BaseAtk = atk;
            BaseDef = def;
            Skills = skills;
        }
    }

    public static class JobDB
    {
        public static Dictionary<JobType, JobData> Jobs = new Dictionary<JobType, JobData>
            {   // 직업명 / 공격력 / 방어력 / 스킬
                { JobType.직업1, new JobData(10, 5, new[] { "스킬1-1", "스킬1-2" }) },
                { JobType.직업2, new JobData(8, 4, new[] { "스킬2-1", "스킬2-2" }) },
                { JobType.직업3, new JobData(6, 3, new[] { "스킬3-1", "스킬3-2" }) }
            };
    }
}