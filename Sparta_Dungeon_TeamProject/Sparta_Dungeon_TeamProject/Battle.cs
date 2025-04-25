using System.ComponentModel;
using System.ComponentModel.Design;
using System.Numerics;
using System.Threading;
using System.Xml.Linq;
using Sparta_Dungeon_TeamProject;
using static Sparta_Dungeon_TeamProject.Monster;
using static Sparta_Dungeon_TeamProject.Monster.MonsterFactory;

namespace Sparta_Dungeon_TeamProject
{
    public partial class Program
    {
        public static string GetMonsterStatus(Monster mon) => mon.IsAlive ? $"[HP:{mon.Hp}]" : "[사망]";

        public static int KillMon = 0; // 몬스터 처치 횟수 값
        public static bool Playerturn = true; // 플레이어의 턴 여부
        public static List<Monster> battleMonsters = new List<Monster>(); // 전투용 몬스터 리스트
        public static int BattleTurn = 1; // 전투 턴 변수
        public static int Stage = 0; // 스테이지 변수
        public static int Chapter = 0; // 챕터 변수

        // 4. 던전

        // 던전 입장 스크립트

        static void DisplayDungeonUI(int Chapter)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"{"",10}Chapter. {Chapter + 1}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{"",3}┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            Console.WriteLine($"{"",3}                                                ");
            Console.WriteLine($"{"",10}Chapter. {Chapter + 1} - Stage {Stage + 1}");
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
            Console.WriteLine($"{"",3}┃         0. 마을로 돌아가기            ┃");
            Console.WriteLine($"{"",3}┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine($"{"",7}원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, 1);
            switch (result)
            {
                case 0:
                    DisplayMainUI();
                    break;
                case 1:
                    Battle(Stage);

                    break;
            }

        }

        // 보스 등장 스크립트 (3번째 스테이지마다 등장 예정)
        static void EnterBossUI()
        {
            battleMonsters = MonsterSpawner.SpawnMonsters(Stage);
            var m = battleMonsters[0];

            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{"",3}┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            Console.WriteLine($"{"",12}Chapter. {Chapter + 1} - {Stage + 1}");
            Console.WriteLine($"{"",3}┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{"",10}>> 보스 등장: {Messages.ChapterTitle[Chapter]} <<");
            Console.ResetColor();
            Console.WriteLine();

            Messages.BossDesc(Chapter);
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

            int result = CheckInput(1, 2);
            switch (result)
            {
                case 1:
                    HandleNextStage(++Stage); // 보스 전투가 아직 미구현이라서 바로 다음 스테이지로 넘어감
                    // BossBattlechap(Chapter);
                    break;
                case 2:
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    player.DisplayPlayerInfo();
                    Console.WriteLine();
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine();
                    Console.WriteLine($"{"",7}원하시는 행동을 입력해주세요.");
                    break;
                case 3:
                    Console.WriteLine("\n\n\n");
                    player.DisplayPlayerInfo();
                    Console.WriteLine();
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine();
                    Console.WriteLine($"{"",7}원하시는 행동을 입력해주세요.");

                    switch (CheckInput(0, 0))
                    {
                        case 0:
                            EnterBossUI();
                            break;
                    }
                    break;
            }
        }
        static void Battle(int stage)
        {
            KillMon = 0; // 몬스터 킬 수 초기화
            BattleTurn = 1; // 전투 턴 수 초기화
            battleMonsters = MonsterSpawner.SpawnMonsters(Stage);
            Playerturn = true;

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
        static void BossBattlechap(int chapter)
        {
            BattleTurn = 1;
            battleMonsters = MonsterSpawner.SpawnMonsters(Stage);
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
        static void PlayerTurnUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{"",7}┏━━━━━━━[ Chapter. {Chapter + 1} - {Stage + 1} ]━━━━━━━┓");
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
                    PlayerActionNormal();
                    // PlayerActionBoss1(); 구현 X 임시로 일반 선택지 넣어두었음
                    break;
                case 5:
                    PlayerActionNormal();
                    // PlayerActionBoss2(); 구현 X 임시로 일반 선택지 넣어두었음
                    break;
                case 8:
                    PlayerActionNormal();
                    // PlayerActionBoss3(); 구현 X 임시로 일반 선택지 넣어두었음
                    break;
                case 11:
                    PlayerActionNormal();
                    // PlayerActionBoss4(); 구현 X 임시로 일반 선택지 넣어두었음
                    break;
                default:
                    PlayerActionNormal();
                    break;
            }
        }

        static void PlayerActionNormal()
        {
            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.WriteLine($"{"",7}▶ 1. 일반 공격");
            Console.WriteLine($"{"",7}▶ 2. 스킬 선택");
            Console.WriteLine($"{"",7}▶ 3. 상태 보기");
            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.WriteLine($"{"",7}원하시는 행동을 입력해주세요.");
            Console.WriteLine();

            switch (CheckInput(1, 3))
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
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine();
                    Console.WriteLine($"{"",7}원하시는 행동을 입력해주세요.");

                    switch (CheckInput(0, 0))
                    {
                        case 0:
                            PlayerTurnUI(); break;

                    }
                    break;
            }
        }

        static void PlayerActionBoss1()
        {
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

            switch (CheckInput(1, 3))
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
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine();
                    Console.WriteLine("원하시는 행동을 입력해주세요.");

                    switch (CheckInput(0, 0))
                    {
                        case 0:
                            PlayerTurnUI(); break;
                    }
                    break;
            }
        }
        static void PlayerActionBoss2()
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

            switch (CheckInput(1, 3))
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
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine();
                    Console.WriteLine("원하시는 행동을 입력해주세요.");

                    switch (CheckInput(0, 0))
                    {
                        case 0:
                            PlayerTurnUI(); break;
                    }
                    break;
            }
        }
        static void PlayerActionBoss3()
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

            switch (CheckInput(1, 3))
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
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine();
                    Console.WriteLine("원하시는 행동을 입력해주세요.");

                    switch (CheckInput(0, 0))
                    {
                        case 0:
                            PlayerTurnUI(); break;
                    }
                    break;
            }
        }
        static void PlayerActionBoss4()
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

            switch (CheckInput(1, 3))
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
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine();
                    Console.WriteLine("원하시는 행동을 입력해주세요.");

                    switch (CheckInput(0, 0))
                    {
                        case 0:
                            PlayerTurnUI(); break;
                    }
                    break;
            }
        }

        static void MonsterTurnUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"┏━━━━━━━[ Chapter. {Chapter + 1} - {Stage + 1}   ]━━━━━━━┓");
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
            Console.WriteLine($"{"",10}▶ 아무 키나 눌러 다음으로 넘어가세요.");
            Console.ReadKey();

            Playerturn = true; // 플레이어 턴으로 변경
            BattleTurn++; // 전체적인 한 턴이 끝났으니 턴 수 증가
        }
        
        static void PlayerAttack()
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

            int result = CheckInput(0, battleMonsters.Count);

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
            // 몬스터가 회피했을 때
            if (BattleManager.MonEvasion(target) == true)
            {
                BattleManager.MonEvasionMes(target);
                Playerturn = false;
            }
            // 몬스터가 회피하지 못했을 경우
            else
            {
                player.PlayerAttack(target, 1);
                Console.Clear();
                Console.WriteLine();

                if (player.IsCritical())
                {
                    Messages.CriticalMes(player);
                }
            }

            Console.WriteLine($"{"",10}▶ 아무 키나 눌러 다음으로 넘어가세요.");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"{"",10}[Lv.{target.Level}][{target.Name}] 에게 만큼 피해를 입혔다!");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();


            if (target.Hp <= 0)
            {
                KillMon++; // 몬스터 처치 수 증가
                target.IsAlive = false;
                DisplayKillMessage(target);
                player.GainReward(target.DropGold, target.DropExp);
                ExpGoldCheck();
                Console.WriteLine();
                Console.WriteLine($"{"",10}▶ 아무 키나 눌러 다음으로 넘어가세요.");
                Console.ReadKey();

                if (battleMonsters.All(m => !m.IsAlive))
                {
                    BattleSuccessUI();
                    return;
                }
            }

            Playerturn = false; // 몬스터에게 턴 넘김

            static void DisplayKillMessage(Monster target)
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

            static void ExpGoldCheck()
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



        public static void BattleSuccessUI()
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
            Console.WriteLine($"{"",7}▶ Chapter. {Chapter + 1} - {Stage + 1}");
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
            Console.WriteLine($"{"",3}┃         0. 마을로 돌아가기            ┃");
            Console.WriteLine($"{"",3}┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{"",3}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine($"{"",7}원하시는 행동을 입력해주세요.");

            switch (CheckInput(0, 1))
            {
                case 0:
                    Stage = 0; // 마을로 복귀하면서 스테이지 값 초기화
                    DisplayMainUI();
                    break;
                case 1:
                    HandleNextStage(++Stage); // 다음 층으로 이동하면서 스테이지 값 +1
                    break;
            }

        }

        // 다음스테이지가 보스스테이지인지 구분하는 메서드
        private static void HandleNextStage(int stage)
        {
            int num = Stage % 3;
            switch (num)
            {
                case 0:
                    DisplayDungeonUI(++Chapter);
                    break;
                case 1:
                    Battle(Stage);
                    break;
                case 2:
                    EnterBossUI();
                    break;
            }
        }

        public static void BattleFailUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"{"",3}┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            Console.WriteLine($"{"",3}┃                ━━  전투 실패 ━━                ┃");
            Console.WriteLine($"{"",3}┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{"",7}▣ 당신의 여정은 여기서 끝이 났습니다 ▣");
            Console.WriteLine($"{"",7}더 할로우드는, 또 한 명의 잊힌 자를 품었습니다.");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"{"",7}▶ 마지막 위치 : 스테이지 {Stage + 1}");
            Console.WriteLine($"{"",7}▶ Lv.{player.Level:D2} [{player.Name}]");
            Console.WriteLine($"{"",10}체력 : {player.Hp}/{player.MaxHp}");
            Console.WriteLine($"{"",10}마나  : {player.Mp}/{player.MaxMp}");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"{"",3}┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            Console.WriteLine($"{"",3}┃             ▣ 게임이 종료됩니다 ▣              ┃");
            Console.WriteLine($"{"",3}┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{"",7}아무 키나 누르면 종료됩니다...");
            Console.ResetColor();
            Console.ReadKey();
            Environment.Exit(0);
        }

        static void PrintMonsters()
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



    public static class MonsterSpawner
    {
        // 보스가 등장하는 스테이지
        private static readonly Dictionary<int, string> BossStages = new()
        {
            {2, "Capybara" },
            {5, "RegretfulAdventurer" },
            {8, "GiantCapybara" },
            {11, "BlackCat" }

        };

        // 챕터별 등장하는 몬스터 분류
        private static readonly Dictionary<int, Type> ChapterMonsterEnums = new()
        {
            {1, typeof(MonsterTypeChap1) },
            {2, typeof(MonsterTypeChap2) },
            {3, typeof(MonsterTypeChap3) },
            {4, typeof(MonsterTypeChap4) }
        };

        // 스테이지 별 딕셔너리의 Key로 쓰일 int 정의
        private static int GetChapter(int stage)
        {
            if (stage < 3) return 1;
            if (stage < 6) return 2;
            if (stage < 9) return 3;
            return 4;
        }

        // 스테이지에 따른 몬스터 리스트 생성
        public static List<Monster> SpawnMonsters(int Stage)
        {
            Random random = new Random();

            // 먼저 보스스테이지라면 보스를 소환(Guard Clause)
            // BossStages 딕셔너리의 Key에 int Stage가 포함되어 있을 경우 보스몬스터 생성 메서드 호출
            if (BossStages.ContainsKey(Stage))
            {
                string bossname = BossStages[Stage];
                return new List<Monster> { MonsterFactory.CreateMonster(bossname) };
            }

            // 일반 스테이지라면 일반 몬스터를 소환
            int chapter = GetChapter(Stage);
            Type enumType = ChapterMonsterEnums[chapter];  // 스테이지에 따라 소환되는 몬스터의 Enum
            Array allowedTypes = Enum.GetValues(enumType); // 정해진 Enum에 포함된 모든 몬스터가 들어가있는 배열

            int monsterCount = Math.Min(1 + Stage / 2, 5);

            return Enumerable.Range(0, monsterCount)
                .Select(_ =>
                {
                    var randomType = allowedTypes.GetValue(random.Next(allowedTypes.Length));
                    return MonsterFactory.CreateMonster(randomType.ToString());
                }).ToList();
        }
    }

    public static class BattleManager
    {
        public static Random rand = new Random();

        public static bool MonEvasion(Monster target)
        {
            return rand.Next(0, 100) < target.Dodge; // 몬스터 회피 확률
        }

        public static void MonEvasionMes(Monster target) // 몬스터가 회피했다면
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"{"",10}[Lv.{target.Level}][{target.Name}] (은)는 공격을 손쉽게 회피했다!");
            Console.ResetColor();
            Thread.Sleep(700);
            return;
        }
    }

}












