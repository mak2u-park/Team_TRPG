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
            //강화하기에서 true
            { "강화", false },
        };

        // ** 실제 구동되는 메인함수 **
        static void Main(string[] args)
        {
            // 인트로
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.WriteLine("The Hollowed");
            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("한때 무언가를 간절히 바랐던 이들이");
            Console.WriteLine("스스로를 비워내며, 이 도시에 도착합니다.");
            Console.WriteLine();
            Console.WriteLine("잊고 싶은 기억, 치유되지 않은 상처,");
            Console.WriteLine("돌이킬 수 없는 후회, 아니면... 그저 호기심.");
            Console.WriteLine();
            Console.WriteLine("그리고 누군가는 진실을 마주하기 위해,");
            Console.WriteLine("던전 깊은 곳을 향해 나아갑니다.");
            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Enter를 눌러, 여정을 시작하세요.");
            Console.ResetColor();

            WaitForEnter();

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
            { JobType.대장장이, new Smith() },
            { JobType.영매사, new Medium() }
        };

        // A. 기본 세팅
        static void SetData()
        {
            // 직업 선택
            JobType selectType = Prompt();
            IJob job = JobDatas[selectType];


            // 이름 입력
            string name;
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"직업이 [{job.DisplayName}]로 정해졌습니다.");
                Console.WriteLine($"[{job.DisplayName}]의 이름을 정해주세요.");
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

            // 스토리 출력
            Console.Clear();
            Console.WriteLine($"[{name}]님의 직업은 [{job.DisplayName}]입니다.\n\n");
            Console.WriteLine($"{job.Story}\n\n");
            Console.WriteLine($"{job.Description}\n");
            Console.WriteLine($"공격력: {job.BaseAtk}  |  방어력: {job.BaseDef}  |  Hp: {job.BaseHp}  |  Mp: {job.BaseMp}  |  특성: {job.Trait}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Enter키를 눌러, 마을로 입장합니다.");
            Console.ResetColor();

            WaitForEnter();

            // 플레이어 정보 세팅
            player = new Player(
                    level: 1,
                    exp: 0,
                    maxExp: 100,
                    name: name,
                    job: job.Type,
                    hp: job.BaseHp,
                    mp: job.BaseMp,
                    atk: job.BaseAtk,
                    def: job.BaseDef,
                    maxHp: job.BaseHp,
                    maxMp: job.BaseMp,
                    gold: 10000
                );

            switch (player.Job) // 기본 스킬 지급
            {
                case JobType.전사:
                    SkillManager.FirstWarriorSkill(player);
                    break;
                case JobType.마법사:
                    SkillManager.FirstWizardSkill(player);
                    break;
                case JobType.과학자:
                    SkillManager.FirstScientistSkill(player);
                    break;
                case JobType.대장장이:
                    SkillManager.FirstBlacksmithSkill(player);
                    break;
                case JobType.영매사:
                    SkillManager.FirstWhispererSkill(player);
                    break;
            }

            InitItemDb(); // 아이템 세팅 호출
        }

        // 0. 메인메뉴
        public static void DisplayMainUI()
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[1] 상태 보기");
            Console.WriteLine("[2] 인벤토리");
            Console.WriteLine("[3] 상점");
            Console.WriteLine("[4] 던전입장");
            Console.WriteLine("[5] 휴식하기");
            Console.WriteLine("[~`] 게임종료");
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
                    DisplayDungeonUI(Chapter);
                    break;
                case 5:
                    DisplayRestUI();
                    break;
                case -1:
                    Console.WriteLine("게임을 종료합니다.");
                    Thread.Sleep(1000);
                    // `키를 눌러 종료
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
            Console.WriteLine("[1] 스킬 관리");
            Console.WriteLine("[0] 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(1, 1);

            switch (result)
            {
                case -1:
                    DisplayMainUI();
                    break;
                case 1: // 스킬 관리
                    player.DisplaySkillUI();
                    break;
            }
        }

        // 5. 휴식기능 - hp, mp 회복
        static void DisplayRestUI()
        {
            Console.Clear();
            Console.WriteLine("** 여관 **");
            Console.WriteLine($"보유 골드 : {player.Gold} G\n");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("여관 주인: 안녕하세요, 여관에 오신 것을 환영합니다.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[체력 회복 메뉴]");
            Console.WriteLine("[1] 따뜻한 죽 한 그릇 (300G, +20 HP)");
            Console.WriteLine("[2] 고기 듬뿍 스튜 (700G, +50 HP)");
            Console.WriteLine("[3] 푸짐한 정식 (1200G, 전체 회복)");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("[마나 회복 메뉴]");
            Console.WriteLine("[1] 1세대 실험약 (300G, +30 MP)");
            Console.WriteLine("[2] 강화형 실험약 (700G, +80 MP)");
            Console.WriteLine("[3] 미공개 프로토타입 (1200G, 전체 회복)");

            Console.WriteLine("[1] 체력회복하기");
            Console.WriteLine("[2] 마나회복하기");
            Console.WriteLine("[~`] 나가기");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");

            int result = CheckInput(1, 2);
            switch (result)
            {
                case -1:
                    DisplayMainUI();
                    break;

                case 1:
                    if (player.HealHp(300, 20))
                        Console.WriteLine("\n죽을 먹고 따뜻해졌다!");
                    else Console.WriteLine("골드가 부족합니다.");
                    break;
                case 2:
                    if (player.HealHp(700, 50))
                        Console.WriteLine("\n고기 듬뿍 스튜를 먹고 힘이 솟는다!");
                    else Console.WriteLine("골드가 부족합니다.");
                    break;
                case 3:
                    if (player.HealHp(1200, player.MaxHp))
                        Console.WriteLine("\n정식을 먹고 체력이 전부 회복되었다!");
                    else Console.WriteLine("골드가 부족합니다.");
                    break;
                case 4:
                    if (player.GainMp(300, 30))
                        Console.WriteLine("\n실험약을 마시고 정신이 또렷해진다.");
                    else Console.WriteLine("골드가 부족합니다.");
                    break;
                case 5:
                    if (player.GainMp(700, 80))
                        Console.WriteLine("\n강화형 약물이 효과를 발휘했다!");
                    else Console.WriteLine("골드가 부족합니다.");
                    break;
                case 6:
                    if (player.GainMp(1200, player.MaxMp))
                        Console.WriteLine("\nMP가 완전히 회복되었습니다.");
                    else Console.WriteLine("골드가 부족합니다.");
                    break;
            }
            WaitForEnter();
            DisplayRestUI();
        }

        // B. 입력값 체크(숫자 입력 후, 엔터)
        public static int CheckInput(int min, int max)
        {
            int result;

            while (true)
            {
                int inputLine = Console.CursorTop;
                int column = Console.CursorLeft;

                string input = Console.ReadLine()!;

                if (input == "`")
                {
                    return -1;
                }

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

        // B-1. 입력값 받고 하단 지워 일부만 새로운 UI 덮어 씌우기 용
        static void ClearBottom(int fromLine, int lineCount)
        {
            for (int i = 0; i < lineCount; i++)
            {
                Console.SetCursorPosition(0, fromLine + i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }

        // B-2. 입력값 체크 (Enter만!)
        public static void WaitForEnter()
        {
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    return;
                }

                // 에러메시지
                Console.WriteLine("Enter를 눌러주세요.");
                Thread.Sleep(500);

                // 에러 메시지 지우기
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop);
            }
        }
    }
}