using System;
using System.Security.Principal;
using System.Xml.Linq;
using static Sparta_Dungeon_TeamProject.Player;

namespace Sparta_Dungeon_TeamProject
{
    public partial class Program // battle.cs
    {
        // 초기화
        public static Player player;
        public static Inventory inventory;
        public static Shop shop;
        public static Village village;
        public static Battles battles;
        public static Item[] itemDb = Array.Empty<Item>();

        private Dictionary<string, bool> firstVisitFlags = new() // 첫 방문 여부 플래그
        {  //튜토리얼로 사용도 가능
            { "강화", false },
            //{ "상점", false },
            //{ "인벤토리", false },
            //{ "던전", false },
            //{ "휴식", false },
            //{ "스킬", false },
            //{ "상태", false },
        };

        private Dictionary<JobType, IJob> JobDatas = new()
        {
            { JobType.전사, new Warrior() },
            { JobType.마법사, new Mage() },
            { JobType.과학자, new Scientist() },
            { JobType.대장장이, new Smith() },
            { JobType.영매사, new Medium() }
        };

        // ** 메인 함수 **
        static void Main(string[] args)
        {
            Program game = new Program();
            game.GameStart();
        }
        private void GameStart()
        {
            DisplayIntro(); // 인트로 UI
            SetData(); // 플레이어 / 아이템 / 스킬 초기 세팅

            // 순서 중요
            battles = new Battles(player, inventory, village);
            village = new Village(player, inventory, battles);
            
            village.MainScene(player);// 메인 메뉴 UI
        }

        // A. 인트로UI
        private void DisplayIntro()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.WriteLine($"{"",14}The Hollowed");
            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine($"{"",3}한때 무언가를 간절히 바랐던 이들이");
            Console.WriteLine($"{"",3}스스로를 비워내며, 이 도시에 도착합니다.");
            Console.WriteLine();
            Console.WriteLine($"{"",3}잊고 싶은 기억, 치유되지 않은 상처,");
            Console.WriteLine($"{"",3}돌이킬 수 없는 후회, 아니면... 그저 호기심.");
            Console.WriteLine();
            Console.WriteLine($"{"",3}그리고 누군가는 진실을 마주하기 위해,");
            Console.WriteLine($"{"",3}던전 깊은 곳을 향해 나아갑니다.");
            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{"",3}Enter를 눌러, 여정을 시작하세요.");
            Console.ResetColor();

            Utils.WaitForEnter();
        }

        // B. 플레이어 / 아이템 / 스킬 초기 세팅
        private void SetData()
        {
            JobType selectType = Prompt();
            IJob job = JobDatas[selectType];
            string name = ReadName(job);
            DisplayStory(name, job);


            // 플레이어 생성
            player = new Player(name, job);
            itemDb = Item.ItemDb; // 아이템 DB 초기화
            inventory = new Inventory(player, job.InitialItems); // 인벤토리 초기화
            shop = new Shop(player, inventory, itemDb); // 상점 초기화

            // 스킬 보상
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
            }
        }

        // B-1. 직업 선택 UI
        public void StartSelectJob(JobType? selectedJob)
        {
            Console.WriteLine($"\n{"",3}[직업]을 선택하세요.\n");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"{"",3}방법: 숫자(1-5)를 눌러 상세를 보고, Enter로 확정하세요.\n");
            Console.ResetColor();

            foreach (JobType job in Enum.GetValues(typeof(JobType)))
            {
                Console.WriteLine($"{(int)job}. {job}");

                if (selectedJob == job)
                {
                    IJob jobData = JobDatas[job];
                    Console.WriteLine($"  └ {jobData.Description}");
                    Console.WriteLine($"  └ 공격력: {jobData.Atk}  |  방어력: {jobData.Def}  |  HP: {jobData.MaxHp}  |  MP: {jobData.MaxMp}");
                    Console.WriteLine($"  └ 특성: {jobData.Trait}");
                    Console.WriteLine();
                }
            }
        }

        // B-1. 직업 선택 프롬프트 (1~5 숫자키로 정보보기)
        private JobType Prompt()
        {
            JobType? myjob = null; // 처음에 아무것도 선택 X

            while (true)
            {
                Console.Clear();
                StartSelectJob(myjob);

                var keyInfo = Console.ReadKey(true);

                if (keyInfo.Key >= ConsoleKey.D1 && keyInfo.Key <= ConsoleKey.D5)
                {
                    myjob = (JobType)(keyInfo.Key - ConsoleKey.D0);
                }
                else if (keyInfo.Key >= ConsoleKey.NumPad1 && keyInfo.Key <= ConsoleKey.NumPad5)
                {
                    myjob = (JobType)(keyInfo.Key - ConsoleKey.NumPad0);
                }

                // 선택 직업 간략한 정보 출력
                if (myjob.HasValue)
                {
                    Console.SetCursorPosition(0, (int)myjob - 1);
                }
                if (keyInfo.Key == ConsoleKey.Enter && myjob.HasValue)
                {
                    return myjob.Value;
                }
            }
        }

        

        // B-2. 이름 입력UI
        private string ReadName(IJob job)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"\n{"",3}당신의 운명은 [{job.DisplayName}](으)로 정해졌습니다.\n");
                Console.WriteLine($"{"",3}[{job.DisplayName}]의 이름을 정해주세요.\n");
                Console.Write($"{"",3}이름: ");
                string input = Console.ReadLine()!;
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine($"{"",3}올바른 이름을 입력해주세요.");
                    Thread.Sleep(500);
                    continue;
                }
                return input;
            }
        }

        // B-3. 직업 서사 / 초기 보상 UI
        private void DisplayStory(string name, IJob job)
        {
            Console.Clear();
            Console.WriteLine($"[{name}]님의 운명은 [{job.DisplayName}]입니다.\n\n");
            Console.WriteLine($"{job.Story}\n\n");
            Console.WriteLine($"공격력: {job.Atk}  |  방어력: {job.Def}  |  Hp: {job.MaxHp}  |  Mp: {job.MaxMp}  |  특성: {job.Trait}");

            Console.WriteLine($"====[The Hollowed 입성 기념 보상 지급]====");
            Console.WriteLine($"골드: {job.DefaultGold} G");
            // 초기 스킬 지급 내역이 있을 때만 출력
            if (job.InitialSkills != null && job.InitialSkills.Count > 0)
            {
                Console.WriteLine($"스킬: {string.Join(", ", job.InitialSkills)}");
            }
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

            Utils.WaitForEnter();
        }
    }


    public class Utils
    {
        // Z. 입력값 체크(숫자 입력 후, 엔터)
        public static int CheckInput(int min, int max)
        {
            int result;

            while (true)
            {
                int inputLine = Console.CursorTop;
                int column = Console.CursorLeft;

                string input = Console.ReadLine()!;

                if (input == "`" || input == "~")
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

        // Z-2. 번호 입력 받기 (ex: 장착할 아이템 번호 입력)
        public static int CheckNumberInput(int min, int max)
        {
            while (true)
            {
                string input = Console.ReadLine();

                if (input == "~" || input == "`")
                {
                    return -1; // ~` 키 입력시 나가기
                }

                if (int.TryParse(input, out int number))
                {
                    if (number >= min && number <= max)
                    {
                        return number;
                    }
                }

                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                Console.Write(">> ");
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