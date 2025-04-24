using System;
using System.Security.Principal;
using static Sparta_Dungeon_TeamProject.Player;

namespace Sparta_Dungeon_TeamProject
{
    public partial class Program
    {
        private static Player player;
        private static Item[] itemDb = Array.Empty<Item>(); // 임시초기값. 이후 덮어씌워짐ok
        private static Dictionary<string, bool> firstVisitFlags = new() // 첫 방문 여부 플래그

        {
            { "강화", false },
            //{ 다른 곳에서도 사용가능 }
        };

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

        // 직업 데이터
        public static readonly Dictionary<JobType, IJob> JobDatas = new()
        {
            { JobType.전사, new Warrior() },
            { JobType.마법사, new Mage() },
            { JobType.과학자, new Scientist() },
            { JobType.대장장이, new smith() },
            { JobType.영매사, new Medium() }
        };

        // A. 기본 세팅
        static void SetData()
        {
            GameSkill.InitSkills(); // 스킬 세팅 호출

            GameSkill.AllSkills = new List<GameSkill>
            {
                new GameSkill("전사 기본 1", 50, 50, 2, "전사 스킬 1"),
                new GameSkill("전사 2", 50, 50, 2, "전사 스킬 2"),
                new GameSkill("전사 3", 50, 50, 2, "전사 스킬 3"),
                new GameSkill("마법사 기본 1", 50, 50, 2, "마법사 스킬 1"),
                new GameSkill("마법사 2", 50, 50, 2, "마법사 스킬 2"),
                new GameSkill("마법사 3", 50, 50, 2, "마법사 스킬 3"),
                new GameSkill("과학자 기본 1", 50, 50, 2, "과학자 스킬 1"),
                new GameSkill("과학자 2", 50, 50, 2, "과학자 스킬 2"),
                new GameSkill("과학자 3", 50, 50, 2, "과학자 스킬 3"),
                new GameSkill("대장장이 기본 1", 50, 50, 2, "대장장이 스킬 1"),
                new GameSkill("대장장이 2", 50, 50, 2, "대장장이 스킬 2"),
                new GameSkill("대장장이 3", 50, 50, 2, "대장장이 스킬 3"),
            };

            // 이름, 직업 세팅
            string name;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("캐릭터를 생성합니다.");
                Console.WriteLine("캐릭터 이름을 입력해주세요.");
                Console.Write("이름: ");
                name = Console.ReadLine()!;
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(500);
                    continue;
                }
                break;
            }

            // 직업 선택
            IJob job = JobDatas[Prompt()];
            player = new Player(
                level: 1,
                exp: 0,
                maxExp: 100,
                name: name,
                job: JobType.전사,
                hp: job.BaseHp,
                mp: job.BaseMp,
                atk: job.BaseAtk,
                def: job.BaseDef,
                maxHp: job.BaseHp,
                maxMp: job.BaseMp,
              gold: 10000
            );
            player.GetExclusiveSkill();

            Console.WriteLine();
            Console.Write("번호입력: ");
            int result = CheckInput(1, 5);

            JobType jobType = (JobType)result;

            player.GetExclusiveSkill(); // 직업별 기본 스킬 지급 
            InitItemDb(); // 아이템 세팅 호출
        }

        // 0. 메인메뉴
        public static void DisplayMainUI()
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
                    DisplayDungeonUI(Chapter);
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
            Console.WriteLine("1. 스킬 관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, 1);

            switch (result)
            {
                case 0:
                    DisplayMainUI();
                    break;
                case 1: // 스킬 관리
                    player.DisplaySkillUI();
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
        public static int CheckInput(int min, int max)
        {
            int result;

            while (true)
            {
                int inputLine = Console.CursorTop;
                int column = Console.CursorLeft;

                string input = Console.ReadLine()!;
                bool isNumber = int.TryParse(input, out result);

                if (isNumber)
                {
                    if (result >= min && result <= max)
                        return result;
                }

                int error = Console.CursorTop;
                Console.WriteLine("잘못된 입력입니다.");
                Thread.Sleep(500);

                // 에러메시지 지우고
                Console.SetCursorPosition(0, error);
                Console.Write(new string(' ', Console.WindowWidth));

                // 입력 지우기
                Console.SetCursorPosition(column, inputLine);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(column, inputLine);
            }
        }

        // B-1. 하단 지우기 (강화에서 사용중이나, 다른데서도 호출 가능)
        static void ClearBottom(int fromLine, int lineCount)
        {
            for (int i = 0; i < lineCount; i++)
            {
                Console.SetCursorPosition(0, fromLine + i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }

    }
}