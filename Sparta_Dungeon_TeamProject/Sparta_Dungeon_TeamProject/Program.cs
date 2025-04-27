using System;
using System.Security.Principal;
using System.Xml.Linq;
using static Sparta_Dungeon_TeamProject.Player;

namespace Sparta_Dungeon_TeamProject
{
    public partial class Program
    {
        public static Player player;
        public static Item[] itemDb = Array.Empty<Item>(); // 임시초기값. 이후 덮어씌워짐ok
        public static Dictionary<string, bool> firstVisitFlags = new() // 첫 방문 여부 플래그
        {  //튜토리얼로 사용도 가능
            { "강화", false },
            //{ "상점", false },
            //{ "인벤토리", false },
            //{ "던전", false },
            //{ "휴식", false },
            //{ "스킬", false },
            //{ "상태", false },
        };

        // ** 메인 함수 **
        static void Main(string[] args)
        {
            DisplayIntro();
            SetData();
            Messages.ShowMainMenu();
        }

        // A. 인트로UI
        private static void DisplayIntro()
        {
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
        }

        // B. 플레이어 / 아이템 / 스킬 초기 세팅
        static void SetData()
        {
            JobType selectType = Prompt();
            IJob job = JobDatas[selectType];
            string name = ReadName(job);
            DisplayStory(name, job);


            // 플레이어 생성
            player = new Player(name, job);
            itemDb = Item.InitializeItemDb(); // 상점 초기화

            var initialItems = new List<Item>(Item.GifttemDb[job.Type]);
            Inventory.Initialize(player, initialItems); // 아이템보상 인벤에 지급

            /* 스킬 보상
            switch (selectType)
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
            }*/
        }

        // B-1. 직업 선택 프롬프트 (1~5 숫자키로 정보보기)
        private static JobType Prompt()
        {
            JobType? current = null; // 처음에 아무것도 선택 X

            while (true)
            {
                Console.Clear();
                Messages.StartSelectJob(current);

                var keyInfo = Console.ReadKey(true);

                if (keyInfo.Key >= ConsoleKey.D1 && keyInfo.Key <= ConsoleKey.D5)
                {
                    current = (JobType)(keyInfo.Key - ConsoleKey.D0);
                }
                else if (keyInfo.Key >= ConsoleKey.NumPad1 && keyInfo.Key <= ConsoleKey.NumPad5)
                {
                    current = (JobType)(keyInfo.Key - ConsoleKey.NumPad0);
                }

                // 선택 직업 간략한 정보 출력
                if (current.HasValue)
                {
                    Console.SetCursorPosition(0, (int)current - 1);
                }
                if (keyInfo.Key == ConsoleKey.Enter && current.HasValue)
                {
                    return current.Value;
                }
            }
        }

        public static readonly Dictionary<JobType, IJob> JobDatas = new()
        {
            { JobType.전사, new Warrior() },
            { JobType.마법사, new Mage() },
            { JobType.연금술사, new Scientist() },
            { JobType.대장장이, new Smith() },
            { JobType.영매사, new Medium() }
        };

        // B-2. 이름 입력UI
        private static string ReadName(IJob job)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"직업이 [{job.DisplayName}]로 정해졌습니다.");
                Console.WriteLine($"[{job.DisplayName}]의 이름을 정해주세요.");
                Console.Write("이름: ");
                string input = Console.ReadLine()!;
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("올바른 이름을 입력해주세요.");
                    Thread.Sleep(500);
                    continue;
                }
                return input;
            }
        }

        // B-3. 직업 서사 / 초기 보상 UI
        private static void DisplayStory(string name, IJob job)
        {
            Console.Clear();
            Console.WriteLine($"[{name}]님의 직업은 [{job.DisplayName}]입니다.\n\n");
            Console.WriteLine($"{job.Story}\n\n");
            Console.WriteLine($"공격력: {job.Atk}  |  방어력: {job.Def}  |  Hp: {job.MaxHp}  |  특성: {job.Trait}");

            Console.WriteLine($"====[The Hollowed 입성 기념 보상 지급]====");
            Console.WriteLine($"골드: {job.DefaultGold} G");
            // 초기 스킬 지급 내역이 있을 때만 출력
            /*if (job.InitialSkills != null && job.InitialSkills.Count > 0)
            {
                Console.WriteLine($"스킬: {string.Join(", ", job.InitialSkills)}");
            }*/
            // 초기 아이템 지급 내역이 있을 때만 출력
            if (job.InitialItems != null && job.InitialItems.Count > 0)
            {
                Console.WriteLine($"아이템: {string.Join(", ", job.InitialItems.Select(i => i.Name))}");
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Enter키를 눌러, 마을로 입장합니다.");
            Console.ResetColor();

            WaitForEnter();
        }

        // Z. 입력값 체크(숫자 입력 후, 엔터)
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

        // Z-1. 입력값 체크 (Enter만!)
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

        // Z-2. UI 일부 지우기 (제련소 참고하여 가능)
        public static void ClearBottom(int fromLine, int lineCount)
        {
            for (int i = 0; i < lineCount; i++)
            {
                Console.SetCursorPosition(0, fromLine + i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }
    }
}