using System.ComponentModel;
using System.ComponentModel.Design;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Xml.Linq;
using Sparta_Dungeon_TeamProject;
using static Sparta_Dungeon_TeamProject.Monster;

namespace Sparta_Dungeon_TeamProject
{
    public partial class Battles
    {
        // player, Stage, Chapter, Inventory, Village 인스턴스
        private Player player;
        private Inventory inventory;
        private Village village;
        Messages messages = new Messages();
        private MonsterSpawner monsterSpawner = new MonsterSpawner();
        private BattleManager battleManager = new BattleManager();

        private List<Monster> monsters = new List<Monster>(); // 몬스터 리스트
        private List<Monster> bossMonsters = new List<Monster>(); // 보스 몬스터 리스트
        private List<Monster> normalMonsters = new List<Monster>(); // 일반 몬스터 리스트

        public List<Monster> battleMonsters = new List<Monster>(); // 전투용 몬스터 리스트
        public HashSet<int> selectedOptions = new HashSet<int>() { }; // 4스테이지 보스 선택지 중복 제거용 리스트
        public Battles() { } // 빈 파라미터 생성자
        public Battles(Player player, Inventory inventory, Village village)
        {
            this.player = player;
            this.inventory = inventory;
            this.village = village;
        }


        public int KillMon = 0; // 몬스터 처치 횟수 값
        public int BattleTurn = 1; // 전투 턴 변수
        public int Stage = 0; // 스테이지 변수
        public int Chapter => Stage / 3; // 읽기 전용 프로퍼티
        public int GimmickReady = 0; // 보스 기믹 컨트롤용 변수

        public int bossAtk;
        public int bossDef;
        public int bossDodge;

        public bool Playerturn = true; // 플레이어의 턴 여부
        public static bool BossStage(int stage) // 보스스테이지 여부
        {
            if (stage % 3 == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool left = false;      // 맷돼지 기믹 회피 방향
        public bool right = false;     // 맷돼지 기믹 회피 방향
        public bool doubleAttack = false;


        public string GetMonsterStatus(Monster mon) => mon.IsAlive ? $"[HP:{mon.CurrentHp}]" : "[사망]";
        // 4. 던전

        // 던전 입장 스크립트

        public void DisplayDungeonUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"{"",10}Chapter. {Chapter + 1}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{"",3}┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            Console.WriteLine($"{"",3}                                                ");
            Console.WriteLine($"{"",10}Chapter. {Chapter + 1} - Stage {Stage % 3 + 1}");
            Console.WriteLine($"{"",3}                                                ");
            Console.WriteLine($"{"",3}┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"{"",3}┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            Console.WriteLine($"{"",10}<< 던전 입장 - {Messages.ChapterTitle[Chapter]} >>");
            Console.WriteLine($"{"",3}┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.ResetColor();
            Console.WriteLine();

            // 최종 보스 만날 시 붉은 글씨 출력
            if (Chapter == 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine($"{"",3}┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            Console.WriteLine($"{"",3}┃         1. 앞으로 나아가기            ┃");
            Console.WriteLine($"{"",3}┃         ~. 마을로 돌아가기            ┃");
            Console.WriteLine($"{"",3}┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine($"{"",7}원하시는 행동을 입력해주세요.");

            int result = Utils.CheckInput(0, 1);
            switch (result)
            {
                case -1:
                    Stage = 0;
                    return;
                case 0:
                    Stage = 0;
                    return;
                case 1:
                    Battle(Stage);
                    break;
            }
        }

        // 보스 등장 스크립트 (3번째 스테이지마다 등장 예정)
        private void EnterBossUI()
        {
            while (true)
            {
                battleMonsters = monsterSpawner.SpawnMonsters(Stage);
                var m = battleMonsters[0];
                bossAtk = m.Atk;
                bossDef = m.Def;

                Console.Clear();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"{"",3}┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
                Console.WriteLine($"{"",12}Chapter. {Chapter + 1} - {Stage % 3 + 1}");
                Console.WriteLine($"{"",3}┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{"",10}>> 보스 등장: {Messages.ChapterTitle[Chapter]} <<");
                Console.ResetColor();
                Console.WriteLine();

                messages.BossDesc(Chapter);
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{"",7}[Lv.{m.Level}] ▶▶ {m.Name} ◀◀");
                Console.ResetColor();
                Console.WriteLine(GetMonsterStatus(m));
                Console.WriteLine();

                Console.WriteLine($"{"",3}┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
                Console.WriteLine($"{"",3}┃         1. 앞으로 나아가기            ┃");
                Console.WriteLine($"{"",3}┃         2. 상태보기                   ┃");
                Console.WriteLine($"{"",3}┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
                Console.WriteLine();
                Console.WriteLine($"{"",7}원하시는 행동을 입력해주세요.");

                int result = Utils.CheckInput(1, 2);
                switch (result)
                {
                    case -1:
                        return;
                    case 1:
                        BossBattlechap(Chapter);
                        break;
                    case 2:
                        player.DisplayPlayerInfo(); //상태보기
                        Console.WriteLine();
                        Console.WriteLine("[~`] 나가기");
                        Console.WriteLine();
                        Console.WriteLine($"{"",7}원하시는 행동을 입력해주세요.\n>>");
                        break;
                }
            }
        }
        public void Battle(int stage)
        {
            KillMon = 0; // 몬스터 킬 수 초기화
            BattleTurn = 1; // 전투 턴 수 초기화
            battleMonsters = monsterSpawner.SpawnMonsters(Stage);
            Playerturn = true;

            while (true)
            {
                if (Playerturn) // 만약 플레이어의 턴이라면
                {
                    PlayerTurnUI(); // 화면 출력
                    PlayerActionNormal(); // 입력처리
                }

                else // 만약 플레이어의 턴이 아니라면
                {
                    MonsterTurnUI();
                }
            }
        }
        private void BossBattlechap(int chapter)
        {
            KillMon = 0;
            BattleTurn = 1;
            battleMonsters = monsterSpawner.SpawnMonsters(Stage);
            Playerturn = true;


            switch (chapter)
            {
                case 0:
                    // 만약 랜덤한 입장 이벤트가 발생시 이 자리에 추가
                    break;
                case 1:
                    // 만약 랜덤한 입장 이벤트가 발생시 이 자리에 추가
                    break;
                case 2:
                    // 만약 랜덤한 입장 이벤트가 발생시 이 자리에 추가
                    break;
                default:
                    // 만약 랜덤한 입장 이벤트가 발생시 이 자리에 추가
                    break;
            }

            while (true)
            {
                if (Playerturn) // 만약 플레이어의 턴이라면
                {
                    PlayerTurnUI();
                }

                else // 만약 플레이어의 턴이 아니라면
                {
                    MonsterTurnUI();
                }
            }

        }
        private void PlayerTurnUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{"",7}┏━━━━━━━[ Chapter. {Chapter + 1} - {Stage % 3 + 1} ]━━━━━━━┓");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{"",10}! 당신의 턴입니다 !");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{"",7}▶ 현재 턴 수 : {BattleTurn}");
            Console.WriteLine();
            Console.WriteLine($"{"",7}▶ 현재 HP : {player.Hp}/{player.MaxHp}");
            Console.WriteLine($"{"",7}▶ 현재 MP : {player.Mp}/{player.MaxMp}");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{"",3}> 전투가 계속되고 있습니다! <");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{"",7}┖━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{"",3}┏━━━━━━━━━━<< 등장 몬스터 목록 >>━━━━━━━━━━┓");
            Console.ResetColor();

            PrintMonsters(); // 몬스터 출력

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{"",3}┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.ResetColor();

            switch (Stage)
            {
                case 2:
                    PlayerActionBoss1();
                    break;
                case 5:
                    PlayerActionBoss2();
                    break;
                case 8:
                    PlayerActionBoss3();
                    break;
                case 11:
                    PlayerActionBoss4();
                    break;
                default:
                    break;
            }
        }

        private void PlayerActionNormal()
        {
            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.WriteLine($"{"",7}▶ 1. 일반 공격");
            //Console.WriteLine($"{"",7}▶ 2. 스킬 선택");
            Console.WriteLine($"{"",7}▶ 3. 상태 보기");
            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.WriteLine($"{"",7}원하시는 행동을 입력해주세요.");
            Console.WriteLine();

            int result = Utils.CheckInput(1, 3);

            switch (result)
            {
                case 1:
                    PlayerAttack(); return; // 플레이어 공격 불러오기 
                case 2:
                    Console.WriteLine("스킬은 준비중입니다.\n[Enter]");
                    Utils.WaitForEnter();
                    break; //스킬창 불러오기
                case 3:
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine();
                    player.DisplayPlayerInfo();
                    Console.WriteLine();
                    Console.WriteLine();
                    return;
                case -1:
                    break;
            }



        }

        private void PlayerActionBoss1()
        {
            left = false;
            right = false;

            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.WriteLine("    ▶ 1. 일반 공격");
            Console.WriteLine("    ▶ 2. 스킬 선택");
            Console.WriteLine("    ▶ 3. 왼쪽 살피기");
            Console.WriteLine("    ▶ 4. 오른쪽 살피기");
            Console.WriteLine("    ▶ 5. 상태 보기");
            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.WriteLine();

            int result = Utils.CheckInput(1, 5);

            switch (result)
            {
                case 1:
                    PlayerAttack(); break; // 플레이어 공격 불러오기 
                case 2:
                    break; //스킬창 불러오기
                case 3:
                    // 왼쪽 살피기
                    left = true;
                    Playerturn = false;
                    Console.WriteLine("당신은 왼쪽을 주의깊게 살핍니다");
                    Console.WriteLine($"{"",10}▶ 엔터를 눌러 다음으로 넘어가세요.");
                    Utils.WaitForEnter();
                    // 멧돼지가 왼쪽에서 올 경우 피하고 대신 보스가 최대체력 비례 데미지를 입음
                    break;
                case 4:
                    // 오른쪽 살피기
                    right = true;
                    Playerturn = false;
                    Console.WriteLine("당신은 오른쪽을 주의깊게 살핍니다");
                    Console.WriteLine($"{"",10}▶ 엔터를 눌러 다음으로 넘어가세요.");
                    Utils.WaitForEnter();
                    // 멧돼지가 오른쪽에서 올 경우 피하고 대신 보스가 최대체력 비례 데미지를 입음
                    break;
                case 5:
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    player.DisplayPlayerInfo();
                    Console.WriteLine();
                    Console.WriteLine("[~`] 나가기");
                    Console.WriteLine();
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    break;
                case -1:
                    return;


            }
        }
        private void PlayerActionBoss2()
        {
            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.WriteLine("    ▶ 1. 일반 공격");
            Console.WriteLine("    ▶ 2. 스킬 선택");
            Console.WriteLine("    ▶ 3. 상태 보기");
            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.WriteLine();

            int result = Utils.CheckInput(1, 3);

            switch (result)
            {
                case 1:
                    PlayerAttack(); break; // 플레이어 공격 불러오기 
                case 2:
                    break; //스킬창 불러오기
                case 3:
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    player.DisplayPlayerInfo();
                    Console.WriteLine();
                    Console.WriteLine("[~`] 나가기");
                    Console.WriteLine();
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    break;
                case -1:
                    return;
            }
        }
        private void PlayerActionBoss3()
        {
            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.WriteLine("    ▶ 1. 일반 공격");
            Console.WriteLine("    ▶ 2. 스킬 선택");
            Console.WriteLine("    ▶ 3. 상태 보기");
            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.WriteLine();

            int result = Utils.CheckInput(1, 3);
            switch (result)
            {
                case 1:
                    PlayerAttack(); break; // 플레이어 공격 불러오기 
                case 2:
                    break; //스킬창 불러오기
                case 3:
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    player.DisplayPlayerInfo();
                    Console.WriteLine();
                    Console.WriteLine("[~`] 나가기");
                    Console.WriteLine();
                    Console.WriteLine("원하시는 행동을 입력해주세요.");

                    if (result == 0) PlayerTurnUI();
                    break;
            }
        }
        private void PlayerActionBoss4()
        {
            int bossdodge = bossDodge;

            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.WriteLine("    ▶ 1. 일반 공격");
            Console.WriteLine("    ▶ 2. 스킬 선택");
            Dictionary<int, string> menuOptions = new Dictionary<int, string>()
            {
                { 3, "양초에 불 켜기"},
                { 4, "바닥에 물 엎지르기"},
                { 5, "가방 던지기"},
                { 6, "갑자기 바닥에 눕기"},
                { 7, "전시 되어 있는 접시 깨기"},
                { 8, "고양이자세 취하기"},
                { 9, "상태 보기"}
            };

            // 존재 여부 확인만을 위해 HashSet 사용

            foreach (var option in menuOptions)
            {
                if (selectedOptions.Contains(option.Key))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray; // 회색
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White; // 흰색
                }
                Console.WriteLine($"    ▶ {option.Key}. {option.Value}");
            }
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.WriteLine($"현재 검은 고양이의 회피 확률은 {battleMonsters[0].Dodge:f2}% 입니다.");
            Console.WriteLine("부디 올바른 선택을 하시기 바랍니다.");
            Console.WriteLine();

            int n = Utils.CheckInput(1, 9);


            if (selectedOptions.Contains(n))
            {
                Console.Clear();
                Console.WriteLine("한번 선택한 행동은 반복할 수 없습니다.");
                PlayerActionBoss4();
                return;
            }

            if (n > 2)
            {
                selectedOptions.Add(n);
            }

            // 검은 고양이의 기믹은 플레이어 턴 이전에 검은 고양이의 회피를 낮추는 식으로 진행
            switch (n)
            {
                case 1:
                    PlayerAttack(); break; // 플레이어 공격 불러오기 
                case 2:
                    break; //스킬창 불러오기
                case 3:
                    // 기믹 - 양초에 불 켜기, 회피 감소
                    Console.WriteLine("당신은 조심스럽게 양초에 불을 붙입니다.");
                    Console.WriteLine("은은한 불빛이 어둠을 밀어내자, 검은 고양이의 눈동자가 반짝입니다.");
                    Console.WriteLine("고양이는 불빛 주위를 빙글빙글 돌며, 호기심을 감추지 못합니다.");
                    // 회피 확률 50%로 고정
                    Console.WriteLine($"확인용 텍스트 - 현재 회피율은 {battleMonsters[0].Dodge}% 입니다.");
                    battleMonsters[0].ChangeStat(Monster.StatType.Dodge, 50);
                    Console.WriteLine($"확인용 텍스트 - 현재 회피율은 {battleMonsters[0].Dodge}% 입니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}▶ 아무 키나 눌러 다음으로 넘어가세요.");
                    Console.ReadKey();
                    Playerturn = false;
                    break;
                case 4:
                    // 기믹 - 바닥에 물 엎지르기, 즉사 기믹
                    Console.WriteLine("당신은 바닥에 물을 엎질렀습니다.");
                    Console.WriteLine("검은 고양이는 물웅덩이를 보더니, 꼬리를 부풀리고 뒤로 물러섭니다.");
                    Console.WriteLine("고양이는 더 이상 이곳에 머물 이유가 없다는 듯, 조용히 그림자 속으로 사라집니다.");
                    Console.WriteLine("동시에 방이 그림자로 가득 찹니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}▶ 아무 키나 눌러 다음으로 넘어가세요.");
                    Console.ReadKey();
                    // 즉시 사망
                    BattleFailUI();
                    break;
                case 5:
                    // 기믹 - 가방 던지기, 회피 감소
                    Console.WriteLine("당신은 가방을 힘껏 던집니다.");
                    Console.WriteLine("검은 고양이는 날렵하게 가방을 피해, 호기심 어린 눈빛으로 가방을 살핍니다.");
                    // 회피 확률 20%p 감소
                    Console.WriteLine($"확인용 텍스트 - 현재 회피율은 {battleMonsters[0].Dodge}% 입니다.");
                    battleMonsters[0].ChangeStat(Monster.StatType.Dodge, Math.Max(battleMonsters[0].Dodge - 20, 0)); // 최소 0%
                    Console.WriteLine($"확인용 텍스트 - 현재 회피율은 {battleMonsters[0].Dodge}% 입니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}▶ 아무 키나 눌러 다음으로 넘어가세요.");
                    Console.ReadKey();
                    Playerturn = false;
                    break;
                case 6:
                    // 기믹 - 갑자기 바닥에 눕기, 경계심 증가, 회피 증가
                    Console.WriteLine("당신은 갑자기 바닥에 드러눕습니다.");
                    // 회피 확률 20%p 증가
                    Console.WriteLine($"확인용 텍스트 - 현재 회피율은 {battleMonsters[0].Dodge}% 입니다.");
                    battleMonsters[0].ChangeStat(Monster.StatType.Dodge, battleMonsters[0].Dodge + 20);
                    Console.WriteLine($"확인용 텍스트 - 현재 회피율은 {battleMonsters[0].Dodge}% 입니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}▶ 아무 키나 눌러 다음으로 넘어가세요.");
                    Console.ReadKey();
                    Playerturn = false;
                    break;
                case 7:
                    // 기믹 - 전시 되어 있는 접시 깨기, 경계심 증가 회피 증가
                    Console.WriteLine("당신은 전시된 접시를 깨뜨립니다.");
                    Console.WriteLine("짤랑! 소리와 함께 조각이 흩어지자, 검은 고양이의 귀가 쫑긋 섭니다.");
                    Console.WriteLine("고양이는 깨진 조각 사이를 조심스럽게 살피며, 한동안 당신에게서 시선을 떼지 않습니다.");
                    // 회피 확률 50%p 증가
                    Console.WriteLine($"확인용 텍스트 - 현재 회피율은 {battleMonsters[0].Dodge}% 입니다.");
                    battleMonsters[0].ChangeStat(Monster.StatType.Dodge, Math.Min(battleMonsters[0].Dodge + 50, 100)); // 최대 100%
                    Console.WriteLine($"확인용 텍스트 - 현재 회피율은 {battleMonsters[0].Dodge}% 입니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}▶ 아무 키나 눌러 다음으로 넘어가세요.");
                    Console.ReadKey();
                    Playerturn = false;
                    break;
                case 8:
                    // 기믹 - 고양이자세 취하기, 회피 대폭 감소
                    Console.WriteLine("당신은 천천히 몸을 낮추고, 고양이처럼 기지개를 켭니다.");
                    Console.WriteLine("검은 고양이는 당신을 한심하게 쳐다봅니다");
                    // 회피 확률 20퍼로 고정
                    Console.WriteLine($"확인용 텍스트 - 현재 회피율은 {battleMonsters[0].Dodge}% 입니다.");
                    battleMonsters[0].ChangeStat(Monster.StatType.Dodge, 20);
                    Console.WriteLine($"확인용 텍스트 - 현재 회피율은 {battleMonsters[0].Dodge}% 입니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}▶ 아무 키나 눌러 다음으로 넘어가세요.");
                    Console.ReadKey();
                    Playerturn = false;
                    break;
                case 9:
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    player.DisplayPlayerInfo();
                    Console.WriteLine();
                    Console.WriteLine("[~`] 나가기");
                    Console.WriteLine();
                    Console.WriteLine("원하시는 행동을 입력해주세요.");

                    switch (Utils.CheckInput(0, 0))
                    {
                        case 0:
                            PlayerTurnUI(); break;
                    }
                    break;
            }
        }

        private void MonsterTurnUI()
        {
            doubleAttack = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"┏━━━━━━━[ Chapter. {Chapter + 1} - {Stage % 3 + 1}   ]━━━━━━━┓");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{"",10}! 상대의 턴입니다 !");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{"",7}▶ 현재 턴 수 : {BattleTurn,3}");
            Console.WriteLine();
            Console.WriteLine($"{"",7}▶ 현재 HP : {player.Hp,3}/{player.MaxHp,-3}");
            Console.WriteLine($"{"",7}▶ 현재 MP : {player.Mp,3}/{player.MaxMp,-3}");
            Console.ResetColor();
            Console.WriteLine();
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{"",3}> 전투가 계속되고 있습니다! <");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"┖━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.WriteLine();
            Thread.Sleep(500);
            foreach (var m in battleMonsters)
            {

                if (m.IsAlive)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{"",10}[Lv.{m.Level}][{m.Name}] (이)가 공격을 시도합니다!");
                    Console.ResetColor();
                    Console.WriteLine();


                    if (player.Job == JobType.전사) // 전사 방어 확률
                    {
                        Random random = new Random();
                        int block = random.Next(1, 6);
                        if (block == 1)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"당신은 [Lv.{m.Level}][{m.Name}]의 공격을 막았습니다!");
                        }
                        else
                        {
                            player.EnemyDamage(m.Atk);
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine($"{"",10}▶ 현재 HP : {player.Hp,3}/{player.MaxHp,-3}");
                        }
                    }
                    else if (player.Job == JobType.마법사) // 마법사 보호막
                    {
                    }
                    else
                    {
                        player.EnemyDamage(m.Atk);
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"{"",10}▶ 현재 HP : {player.Hp,3}/{player.MaxHp,-3}");
                    }
                    Console.ResetColor();
                    Console.WriteLine();
                    Thread.Sleep(300);
                }
                if (m.IsAlive && doubleAttack)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{"",10}[Lv.{m.Level}][{m.Name}] (이)가 두번째 공격을 시도합니다!");
                    Console.ResetColor();
                    Console.WriteLine();

                    player.EnemyDamage(m.Atk);
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"{"",10}▶ 현재 HP : {player.Hp,3}/{player.MaxHp,-3}");
                    Console.ResetColor();
                    Console.WriteLine();
                    Thread.Sleep(300);
                }
            }

            Console.WriteLine();
            Console.WriteLine($"{"",10}▶ 엔터를 눌러 다음으로 넘어가세요.");
            Utils.WaitForEnter();

            // 보스 스테이지일 경우 기믹 추가
            if (BossStage(Stage))
            {
                BossGimmick(Chapter);
            }

            Playerturn = true; // 플레이어 턴으로 변경
            BattleTurn++; // 전체적인 한 턴이 끝났으니 턴 수 증가

        }

        // 보스 기믹 모음(보스 스테이지일 경우에만 실행)
        private void BossGimmick(int chapter)
        {
            switch (chapter)
            {
                // 1스테이지 보스, 카피바라의 기믹 "겁없는 멧돼지"       
                case 0:
                    BossGimmick1();
                    break;

                // 2스테이지 보스, 후회 하는 모험가의 기믹 "후회는 그림자처럼"
                case 1:
                    BossGimmick2();
                    break;

                // 3스테이지 보스, 대왕 카피바라의 기믹 "힌트"
                case 2:
                    BossGimmick3();
                    break;

                // 4스테이지 보스, 검은 고양이의 기믹은 선택지, PlayerActionBoss4()에 존재
                default:
                    break;
            }
        }

        void PlayerAttack()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{"",3}> 일반 공격을 시도하고 있습니다... <");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{"",7}▶ 현재 턴 수 : {BattleTurn}");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{"",3}┏━━━━━━━━━━<< 등장 몬스터 목록 >>━━━━━━━━━━┓");
            Console.ResetColor();

            PrintMonsters(); // 몬스터 출력

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{"",3}┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.ResetColor();
            Console.WriteLine();

            Console.WriteLine($"{"",7}▶ 0. 취소");
            Console.WriteLine();
            Console.WriteLine($"{"",7}대상을 선택해주세요.");

            int result = Utils.CheckInput(0, battleMonsters.Count);

            if (result == 0) return;

            Monster target = battleMonsters[result - 1];

            // 공격한 몬스터가 죽어있는 경우
            if (!target.IsAlive)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{"",7}[!] 이미 사망한 몬스터는 공격할 수 없습니다.");
                Console.ResetColor();
                Console.WriteLine();
                Thread.Sleep(500);
                Playerturn = true;
                return;
            }
            else
            {
                Console.Clear();
                Console.WriteLine();
                player.PlayerAttack(target, 1);
                if (player.Job == JobType.영매사)
                {
                    player.SpiritAttack(target);
                }
            }

            if (target.CurrentHp <= 0)
            {
                KillMon++; // 몬스터 처치 수 증가
                target.IsAlive = false;
                target.CurrentHp = 0; // 죽은 몬스터의 체력을 0으로 고정
                DisplayKillMessage(target);
                player.GainReward(target.DropGold, target.DropExp);
                ExpGoldCheck();
                Console.WriteLine();
                Console.WriteLine($"{"",10}▶ 엔터를 눌러 다음으로 넘어가세요.");
                Console.ReadKey();

                if (battleMonsters.All(m => !m.IsAlive))
                {
                    BattleSuccessUI();
                    return;
                }
            }

            Playerturn = false; // 몬스터에게 턴 넘김

            void DisplayKillMessage(Monster target)
            {
                Console.WriteLine();
                Console.WriteLine($"{"",10}[Lv.{target.Level}][{target.Name}] (은)는 일격을 맞고 사망했다!");
                Thread.Sleep(700);
                Console.WriteLine();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"{"",10}{target.DropGold} G 를 획득했다.");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"{"",10}{target.DropExp} 만큼 경험치를 획득했다.");
                Console.ResetColor();
                Console.WriteLine();
            }

            void ExpGoldCheck()
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"{"",10}보유 골드 {player.Gold}");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"{"",10}현재 경험치 {player.Exp}/{player.MaxExp}");
                Console.ResetColor();
                Console.WriteLine();
            }

        }

        private void BattleSuccessUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            // 타이틀
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{"",3}┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            Console.WriteLine($"{"",3}┃                ━━  전투 결과 ━━              ┃");
            Console.WriteLine($"{"",3}┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.ResetColor();
            Console.WriteLine();

            // 메시지
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"{"",7}▣ 어둠 속에서 한 줄기 생존의 숨결 ▣");
            Console.WriteLine($"{"",7}전투에서 살아남았습니다...");
            Console.ResetColor();
            Console.WriteLine();

            // 전투 요약
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.WriteLine($"{"",7}▶ Chapter. {Chapter + 1} - {Stage % 3 + 1}");
            Console.WriteLine();
            Console.WriteLine($"{"",7}▶ 처치한 몬스터 수 : {KillMon}");
            Console.WriteLine();
            Console.WriteLine($"{"",7}▶ 생존자 : Lv.{player.Level} [{player.Name}]");
            Console.WriteLine($"{"",10}체력    : {player.Hp}/{player.MaxHp}");
            Console.WriteLine($"{"",10}마나    : {player.Mp}/{player.MaxMp}");
            Console.WriteLine($"{"",10}경험치  : {player.Exp}/{player.MaxExp}");
            Console.ResetColor();
            Console.WriteLine();

            // 선택지
            Console.WriteLine($"{"",3}┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            Console.WriteLine($"{"",3}┃         1. 앞으로 나아가기            ┃");
            Console.WriteLine($"{"",3}┃         ~. 마을로 돌아가기            ┃");
            Console.WriteLine($"{"",3}┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{"",3}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine($"{"",7}원하시는 행동을 입력해주세요.");

            switch (Utils.CheckInput(1, 1))
            {
                case -1:
                    Stage = 0; // 마을로 복귀하면서 스테이지 값 초기화
                    village.MainScene(player); // 마을 메인 메뉴로 이동
                    return;
                case 1:
                    ++Stage;
                    HandleNextStage(); // 다음 층으로 이동하면서 스테이지 값 +1
                    break;
            }

        }

        // 다음스테이지가 보스스테이지인지 구분하는 메서드
        private void HandleNextStage()
        {
            int num = Stage % 3;
            switch (num)
            {
                case 0:
                    DisplayDungeonUI();
                    break;
                case 1:
                    Battle(Stage);
                    break;
                case 2:
                    EnterBossUI();
                    break;
            }
        }



        private void BossGimmick1()
        {
            if (GimmickReady++ % 2 == 0)
            {
                // 첫턴은 기믹 준비 단계, 주변에서 흥분한 멧돼지의 울음소리가 들린다
                Console.WriteLine();
                Console.WriteLine("어둠 속에서 멧돼지의 울음소리가 들려옵니다.");
                Console.WriteLine("멧돼지들은 이따금 알록달록한 버섯을 찾아 먹습니다");
                Console.WriteLine("그리고는 보이는 모든 것에 박치기를 하기 시작합니다.");
                Console.WriteLine("아마 그날따라 유난히 세상이 말랑해 보여서 그런걸지도 모릅니다.");
            }
            else
            {
                // 멧돼지가 돌진해오는 턴
                Random rand = new Random();
                bool boarRushLeft = rand.Next(2) == 0;// true면 왼쪽, false면 오른쪽

                Console.WriteLine();
                Console.WriteLine("멧돼지가 겁없는 돌진을 시작합니다.");
                Console.WriteLine(boarRushLeft ? "멧돼지가 왼쪽에서 돌진해옵니다!" : "멧돼지가 오른쪽에서 돌진해옵니다.");

                // 멧돼지의 돌진 방향을 맞춘 경우
                if ((boarRushLeft && left) || (!boarRushLeft && right))
                {
                    Console.WriteLine();
                    Console.WriteLine("멧돼지의 돌진을 완벽히 피했습니다.");
                    // 카피바라에게 최대체력의 20%의 데미지를 입힘 + 추가 예정
                    Monster target = battleMonsters[0];
                    Console.WriteLine($"확인용 텍스트 - Hp:{target.Hp:F2}, FinalHp:{target.FinalHp:F2}, CurrentHp: {target.CurrentHp:F2}, target.FinalHp / 5f: {target.FinalHp / 5f}");
                    target.CurrentHp = (target.CurrentHp) - (target.FinalHp / 5f);

                    Console.WriteLine($"최대체력:{target.FinalHp:F2}, 현재 체력: {target.CurrentHp:F2}");
                }
                // 아무런 방향을 선택하지 않은 경우
                else if (!left && !right)
                {
                    Console.WriteLine();
                    Console.WriteLine("준비되지 않은 자에게 재앙은 늘 갑작스럽습니다.");
                    Console.WriteLine("당신은 멧돼지에게 호되게 당했습니다.");
                    // 플레이어에게 최대체력의 10%의 데미지를 입힘
                    player.boarDamage(true);
                    Console.WriteLine("최대체력의 10%만큼 데미지를 받았습니다.");
                    Console.WriteLine($"확인용 텍스트 - 최대체력:{player.MaxHp:F2}, 현재 체력: {player.Hp:F2}");
                    player.CheckPlayerDead();
                }
                // 멧돼지의 돌진 방향을 틀린 경우
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("잘못된 확신은 때론 방심보다 잔혹합니다.");
                    Console.WriteLine("당신은 멧돼지의 돌진을 받아내는 데에는 재능이 없군요.");
                    // 플레이어에게 최대체력의 20%의 데미지를 입힘
                    player.boarDamage(false);
                    Console.WriteLine("최대체력의 20%만큼 데미지를 받았습니다.");
                    Console.WriteLine($"확인용 텍스트 - 최대체력:{player.MaxHp:F2}, 현재 체력: {player.Hp:F2}");
                    player.CheckPlayerDead();
                }

            }
            Console.WriteLine();
            Console.WriteLine($"{"",10}▶ 아무 키나 눌러 다음으로 넘어가세요.");
            Console.ReadKey();
        }


        public void BossGimmick2()
        {
            int def = bossDef;
            int atk = bossAtk;
            int gimmickReady = GimmickReady++;

            if (gimmickReady % 3 == 0)
            {
                // 방어력이 감소했다면 원상태로 복구, 공격력이 증가했다면 유지, 1회 공격
                Console.WriteLine();
                Console.WriteLine("모험가가 방어태세를 취합니다");
                battleMonsters[0].ChangeStat(StatType.Def, def * 2); // 원래 방어력으로 복구
                Console.WriteLine($"모험가의 방어력이 {battleMonsters[0].FinalDef:F2}(으)로 상승하였다");
            }
            else if (gimmickReady % 3 == 1)
            {
                Console.WriteLine();
                Console.WriteLine("모험가의 후회가 짙어진다...");
                // '자책' 상태 돌입
                // 2턴간 방어력 큰 폭으로 감소, 영구적인 공격력 증가,  다음턴 2회 공격
                battleMonsters[0].ChangeStat(StatType.Def, 1);
                battleMonsters[0].ChangeStat(StatType.Atk, atk + 10);
                Console.WriteLine($"모험가의 방어력이 {battleMonsters[0].FinalDef:F2}(으)로 감소하였다");
                Console.WriteLine($"모험가의 공격력이 {battleMonsters[0].FinalAtk:f2}(으)로 증가하였다");
            }
            else if (gimmickReady % 3 == 2)
            {

                // 방어력 감소된 상태, 이번 턴만 2회 공격
                Console.WriteLine();
                Console.WriteLine("모험가의 후회가 이번 공격을 강화합니다.");
                Console.WriteLine("모험가의 공격이 2번 적중합니다.");
                doubleAttack = true;
            }
            Console.WriteLine();
            Console.WriteLine($"{"",10}▶ 아무 키나 눌러 다음으로 넘어가세요.");
            Console.ReadKey();
        }

        public void BossGimmick3()
        {
            // 3스테이지 보스 대왕 카피바라는 검은 고양이 기믹에 대한 힌트를 주는 기믹으로 설계
            // 검은 고양이와의 조우시 출력되는 선택지와 유사한 행동을 하며, 긍정적, 부정적 효과를 얻음

            int def = bossDef;
            Monster target = battleMonsters[0];

            Random rand = new Random();
            int selectAction = rand.Next(0, 6);
            switch (selectAction)
            {
                case 0:
                    // 양초에 불켜기(긍정) 암시
                    Console.WriteLine();
                    Console.WriteLine("대왕 카피바라가 마른 풀에 불을 붙입니다");
                    Console.WriteLine("은은한 빛이 방 안을 따뜻하게 감싸자 대왕 카피바라가 더욱 단단해집니다.");
                    battleMonsters[0].ChangeStat(StatType.Def, 100);
                    Console.WriteLine($"카피바라의 방어력이 {target.Def}이 되었습니다.");
                    break;
                case 1:
                    // 가방 던지기(긍정) 암시
                    Console.WriteLine();
                    Console.WriteLine("대왕 카피바라가 등에 붙어 있던 이끼 뭉치를 휙 던집니다");
                    Console.WriteLine("카피바라의 몸이 가벼워져 움직임이 더욱 빨라집니다.");
                    battleMonsters[0].ChangeStat(StatType.Dodge, 50);
                    Console.WriteLine($"카피바라의 회피가 {target.Dodge}이 되었습니다.");
                    break;
                case 2:
                    // 고양이 자세 취하기(긍정) 암시
                    Console.WriteLine();
                    Console.WriteLine("대왕 카피바라가 의미불명한 자세를 취합니다.");
                    Console.WriteLine("자세히 보니 고양이 자세를 따라하는 듯 합니다...");
                    Console.WriteLine("카피바라의 체력이 온전히 회복되었다.");
                    target.CurrentHp = target.FinalHp;
                    Console.WriteLine($"카피바라의 체력이 {target.CurrentHp}로 회복되었습니다.");
                    break;
                case 3:
                    // 바닥에 물 엎지르기(부정) 암시
                    Console.WriteLine();
                    Console.WriteLine("대왕 카피바라가 웅덩이에 들어가 물장구를 칩니다.");
                    Console.WriteLine("카피바라의 털이 축축해졌습니다");
                    Console.WriteLine("대왕 카피바라가 당신을 묘한 눈으로 쳐다봅니다.");
                    Console.WriteLine("이상할 정도로 무방비해보입니다...");
                    Console.WriteLine("대왕 카피바라의 방어력이 대폭 감소했습니다.");
                    battleMonsters[0].ChangeStat(StatType.Def, 1);
                    Console.WriteLine($"카피바라의 방어력이 {target.Def}이 되었습니다.");
                    break;
                case 4:
                    // 갑자기 바닥에 눕기(부정) 암시
                    Console.WriteLine();
                    Console.WriteLine("대왕 카피바라가 갑자기 바닥에 엎드립니다.");
                    Console.WriteLine("진흙에 몸을 비비느라 당신에게 신경쓸 겨를이 없는 것 같습니다.");
                    Console.WriteLine("몸에 진흙이 가득 묻어 대왕 카피바라의 움직임이 둔해진듯 합니다");
                    battleMonsters[0].ChangeStat(StatType.Dodge, 0);
                    Console.WriteLine($"카피바라의 회피가 {target.Dodge}이 되었습니다.");
                    break;
                case 5:
                    // 전시된 접시 깨기(부정) 암시
                    Console.WriteLine();
                    Console.WriteLine("대왕 카피바라가 주변에 있는 돌을 들어 바닥에 내려칩니다.");
                    Console.WriteLine("돌맹이가 튀어 대왕 카피바라에게 약간의 피해를 입혔습니다.");
                    target.CurrentHp = (target.CurrentHp) - (target.FinalHp / 5f);
                    Console.WriteLine($"카피바라가 {target.FinalHp / 5f}만큼 피해를 입었습니다");
                    break;
            }
            Console.WriteLine();
            Console.WriteLine($"{"",10}▶ 아무 키나 눌러 다음으로 넘어가세요.");
            Console.ReadKey();
        }


        public void BattleFailUI() // Player에서 호출중이라 public해놓음. - 거기서 호출 없으면 private 추천
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"{"",3}┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            Console.WriteLine($"{"",3}┃                ━━  전투 실패 ━━              ┃");
            Console.WriteLine($"{"",3}┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{"",7}▣ 당신의 여정은 여기서 끝이 났습니다 ▣");
            Console.WriteLine($"{"",7}더 할로우드는, 또 한 명의 잊힌 자를 품었습니다.");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"{"",7}▶ 마지막 위치 : Chapter. {Chapter + 1} - {Stage % 3 + 1}");
            Console.WriteLine($"{"",7}▶ Lv.{player.Level:D2} [{player.Name}]");
            Console.WriteLine($"{"",10}체력 : {player.Hp}/{player.MaxHp}");
            Console.WriteLine($"{"",10}마나  : {player.Mp}/{player.MaxMp}");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"{"",3}┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            Console.WriteLine($"{"",3}┃             ▣ 게임이 종료됩니다 ▣          ┃");
            Console.WriteLine($"{"",3}┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{"",7}엔터를 누르면 종료됩니다...");
            Console.ResetColor();
            Utils.WaitForEnter();
            Environment.Exit(0);
        }

        private void PrintMonsters()
        {
            for (int i = 0; i < battleMonsters.Count; i++)
            {
                var m = battleMonsters[i];
                if (m.IsAlive)
                {
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}[{i + 1}] [Lv.{m.Level}][{m.Name}] {GetMonsterStatus(m)}");
                    Console.ResetColor();
                    Console.WriteLine();
                }
                else
                {
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"{"",10}[{i + 1}] [Lv.{m.Level}][{m.Name}] {GetMonsterStatus(m)}");
                    Console.ResetColor();
                    Console.WriteLine();
                }
            }
        }
    }



    public class MonsterSpawner
    {
        // 보스가 등장하는 스테이지
        private readonly Dictionary<int, string> BossStages = new()
        {
            {2, "Capybara" },
            {5, "RegretfulAdventurer" },
            {8, "GiantCapybara" },
            {11, "BlackCat" }

        };

        // 챕터별 등장하는 몬스터 분류
        private readonly Dictionary<int, Type> ChapterMonsterEnums = new()
        {
            {1, typeof(MonsterTypeChap1) },
            {2, typeof(MonsterTypeChap2) },
            {3, typeof(MonsterTypeChap3) },
            {4, typeof(MonsterTypeChap4) }
        };

        // 스테이지 별 딕셔너리의 Key로 쓰일 int 정의
        private int GetChapter(int stage)
        {
            if (stage < 3) return 1;
            if (stage < 6) return 2;
            if (stage < 9) return 3;
            return 4;
        }

        // 스테이지에 따른 몬스터 리스트 생성
        public List<Monster> SpawnMonsters(int Stage)
        {
            Random random = new Random();

            // 먼저 보스스테이지라면 보스를 소환(Guard Clause)
            // BossStages 딕셔너리의 Key에 int Stage가 포함되어 있을 경우 보스몬스터 생성 메서드 호출
            if (BossStages.ContainsKey(Stage))
            {
                string bossname = BossStages[Stage];
                return new List<Monster> { Monster.MonsterFactory.CreateMonster(bossname) };
            }

            // 일반 스테이지라면 일반 몬스터를 소환
            int chapter = GetChapter(Stage);
            Type enumType = ChapterMonsterEnums[chapter];  // 스테이지에 따라 소환되는 몬스터의 Enum
            Array allowedTypes = Enum.GetValues(enumType); // 정해진 Enum에 포함된 모든 몬스터가 들어가있는 배열

            int monsterCount = Math.Min(1 + Stage / 2, 2);

            return Enumerable.Range(0, monsterCount)
                .Select(_ =>
                {
                    var randomType = allowedTypes.GetValue(random.Next(allowedTypes.Length));
                    return Monster.MonsterFactory.CreateMonster(randomType.ToString());
                }).ToList();
        }
    }

    public class BattleManager
    {
        private static readonly Random rand = new Random();

        public bool MonEvasion(Monster target)
        {
            return rand.Next(0, 100) < target.Dodge; // 몬스터 회피 확률
        }

        public void MonEvasionMes(Monster target) // 몬스터가 회피했다면
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"{"",10}[Lv.{target.Level}][{target.Name}] (은)는 공격을 손쉽게 회피했다!");
            Console.ResetColor();
            Thread.Sleep(700);
        }
    }
}