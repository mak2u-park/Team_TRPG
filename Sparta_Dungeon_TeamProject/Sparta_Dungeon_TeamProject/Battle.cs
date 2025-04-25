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
            Console.WriteLine($"Chapter. {Chapter + 1} - Stage ({Stage + 1})");
            Console.WriteLine();
            Console.WriteLine($"**던전입장 - {ChapterInfo.ChapterTitle[Chapter]}**");
            Console.WriteLine();
            ChapterInfo.ChapterDesc(Chapter);
            Console.WriteLine();

            // 최종 보스 만날 시 붉은 글씨 출력
            if (Chapter == 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine("1. 앞으로 나아가기");
            Console.ResetColor();
            Console.WriteLine("0. 마을로 돌아가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

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
            Console.WriteLine($"Chapter. {Chapter + 1} - Stage ({Stage+1})");
            Console.WriteLine();
            Console.WriteLine($"**보스 등장 - {ChapterInfo.ChapterTitle[Chapter]}**");
            Console.WriteLine();
            BossInfo.BossDesc(Chapter);
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"    [Lv.{m.Level}][{m.Name}] {GetMonsterStatus(m)}");           
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("1. 전투 시작");
            Console.WriteLine("2. 아이텝 사용");
            Console.WriteLine("3. 상태 보기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(1, 3);
            switch (result)
            {
                case 1:
                    HandleNextStage(++Stage); // 보스 전투가 아직 미구현이라서 바로 다음 스테이지로 넘어감
                    // BossBattlechap1();
                    break;
                case 2:
                    break; // 아이템 사용 미구현
                case 3:
                    Console.WriteLine("\n\n\n");
                    player.DisplayPlayerInfo();
                    Console.WriteLine();
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine();
                    Console.WriteLine("원하시는 행동을 입력해주세요.");

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
                    PlayerTurn();
                }

                else // 만약 플레이어의 턴이 아니라면
                {
                    MonsterTurn();
                }
            }
        }


        static void BossBattlechap1()
        {
            BattleTurn = 1;
            // 보스몬스터 초기화
            Playerturn = true;

            while (true)
            {
                if (Playerturn) // 만약 플레이어의 턴이라면
                {

                }

                else // 만약 플레이어의 턴이 아니라면
                {

                }
            }

        }

        static void PlayerTurn()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"┏━━━━━━━[ 현재 스테이지 {Stage + 1} ]━━━━━━━┓");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("        ! 당신의 턴입니다 !");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"   ▶ 현재 턴 수 : {BattleTurn}");
            Console.WriteLine();
            Console.WriteLine($"   ▶ 현재 HP : {player.Hp}/{player.MaxHp}");
            Console.WriteLine($"   ▶ 현재 MP : {player.Mp}/{player.MaxMp}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("   > 전투가 계속되고 있습니다! <");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"┖━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("┏━━━━━━━━━━<< 등장 몬스터 목록 >>━━━━━━━━━━┓");
            Console.ResetColor();

            PrintMonsters(); // 몬스터 출력

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.ResetColor();

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
                            PlayerTurn(); break;
                    }
                    break;
            }
        }

        static void MonsterTurn()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"┏━━━━━━━[ 현재 스테이지 {Stage + 1} ]━━━━━━━┓");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("        ! 상대의 턴입니다 !");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"   ▶ 현재 턴 수 : {BattleTurn}");
            Console.WriteLine();
            Console.WriteLine($"   ▶ 현재 HP : {player.Hp}/{player.MaxHp}");
            Console.WriteLine($"   ▶ 현재 MP : {player.Mp}/{player.MaxMp}");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("   > 전투가 계속되고 있습니다! <");
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
                    Console.WriteLine($"    [Lv.{m.Level}][{m.Name}] (이)가 공격을 시도 합니다!");
                    Console.ResetColor();
                    Console.WriteLine();
                    player.EnemyDamage(m.Atk);
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"    ▶ 현재 HP : {player.Hp}/{player.MaxHp}");
                    Console.ResetColor();
                    Console.WriteLine();
                    Thread.Sleep(300);
                }
            }

            Console.WriteLine();
            Console.WriteLine("    ▶ 아무 키나 눌러 다음으로 넘어가세요.");
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
            Console.WriteLine("   > 일반 공격을 시도하고 있습니다. . . <");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"   ▶ 현재 턴 수 : {BattleTurn}");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("┏━━━━━━━━━━<< 등장 몬스터 목록 >>━━━━━━━━━━┓");
            Console.ResetColor();

            PrintMonsters(); // 몬스터 출력

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("    ▶ 0. 취소");
            Console.WriteLine();
            Console.WriteLine("    대상을 선택해주세요.");

            int result = CheckInput(0, battleMonsters.Count);

            if (result == 0) return;

            Monster target = battleMonsters[result - 1];

            if (!target.IsAlive)
            {
                Console.WriteLine();
                Console.WriteLine("    [!] 이미 사망한 몬스터는 공격할 수 없습니다.");
                Console.WriteLine();
                Thread.Sleep(500);
                Playerturn = true;
                return;
            }

            if (DamageCalculation.Evasion()) // 몬스터가 회피했다면
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine($"    [Lv.{target.Level}][{target.Name}] (은)는 공격을 손쉽게 회피했다!");
                Console.ResetColor();
                Thread.Sleep(700);
                Playerturn = false;
                return;
            }
            else // 몬스터가 회피하지 못했을 경우 
            {
                bool isCritical = DamageCalculation.IsCritical();
                double multiplier = DamageCalculation.GetRandomMultiplier();

                double baseDamage = isCritical ? player.FinalAtk * 1.5 : player.FinalAtk;
                int finalDamage = (int)Math.Ceiling((baseDamage * multiplier - target.Def * 0.5)); // 몬스터 방어력의 절반 만큼 최종 데미지 감소
                finalDamage = Math.Max(1, finalDamage); // 최소 데미지 1

                target.Hp -= finalDamage;

                Console.Clear();
                Console.WriteLine();

                if (isCritical) // 먄약 크리티컬 이라면
                {
                    JobType job = player.Job;

                    string[] impactLines = Messages.CriticalDamageMessage.ContainsKey(job)
                        ? Messages.CriticalDamageMessage[job]
                        : new[] {
            "\n\n\n\n    ...정적 속에", // 직업 목록에 존재하지 않으면 뜨는 기본 메시지
            "\n\n    강렬한 일격이 내려친다!" };

                    string criticalImpact = Messages.CriticalDamageFinalMessage.ContainsKey(job)
                        ? Messages.CriticalDamageFinalMessage[job][0]
                        : "──  그대의 일격은 어둠을 가르며, 찰나의 빛이 번뜩였다!!"; // 직업 목록에 존재하지 않으면 뜨는 기본 메시지

                    Console.ForegroundColor = ConsoleColor.Red;
                    Messages.PrintLinesWithSkip(impactLines, 30, 800); // 스킵 가능한 연출 메시지 출력
                    if (Messages.Skip)
                    {
                        Console.Clear(); // 스킵되었을 경우 화면 정리
                    }
                    Console.ResetColor();

                    // 흔들림 2회 (화면 깜빡임)
                    for (int i = 0; i < 2; i++)
                    {
                        if (Messages.Skip) break;

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        string prefix = new string(' ', 4 + i % 2);
                        Console.WriteLine("\n\n\n\n" + prefix + criticalImpact);
                        Console.ResetColor();
                        Thread.Sleep(120);
                    }

                    // 마지막 1회는 클리어 없이 그대로 출력 (최종 상태)
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\n\n\n    " + criticalImpact);
                    Console.ResetColor();
                    Console.WriteLine("\n\n\n");
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine($"    [Lv.{target.Level}][{target.Name}] 에게 {finalDamage} 만큼 피해를 입혔다!");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("    ▶ 아무 키나 눌러 다음으로 넘어가세요.");
                Console.ReadKey();
                Console.Clear();

                if (target.Hp <= 0)
                {
                    KillMon++; // 몬스터 처치 수 증가
                    target.IsAlive = false;
                    DisplayKillMessage(target);
                    player.GainReward(target.DropGold, target.DropExp);
                    ExpGoldCheck();
                    Console.WriteLine();
                    Console.WriteLine("    ▶ 아무 키나 눌러 다음으로 넘어가세요.");
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
                    Console.WriteLine($"    [Lv.{target.Level}][{target.Name}] (은)는 일격을 맞고 사망했다!");
                    Thread.Sleep(700);
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"    {target.DropGold} G 를 획득했다.");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"    {target.DropExp} 만큼 경험치를 획득했다.");
                    Console.ResetColor();
                    Console.WriteLine();
                }

                static void ExpGoldCheck()
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"    보유 골드 {player.Gold}");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"    현재 경험치 {player.Exp}/{player.MaxExp}");
                    Console.ResetColor();
                    Console.WriteLine();
                }
            }
        }


        public static void BattleSuccessUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("┏━━━━━━━━━━━━━━━<< 전투 결과 >>━━━━━━━━━━━━━━━┓");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("              ★ 전투에 승리하셨습니다! ★");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"    ▶ 현재 스테이지에서 {KillMon}마리의 몬스터를 처치하셨습니다.");
            Console.WriteLine();
            Console.WriteLine($"    ▶ Lv.{player.Level} [{player.Name}]");
            Console.WriteLine($"    ▶ 현재 HP : {player.Hp}/{player.MaxHp}");
            Console.WriteLine($"    ▶ 현재 MP : {player.Mp}/{player.MaxMp}");
            Console.WriteLine();
            Console.WriteLine($"    ▶ 현재 경험치 : {player.Exp}/{player.MaxExp}");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.WriteLine("    ▶ 1. 다음 스테이지로 이동하기");
            Console.WriteLine("    ▶ 0. 복귀하기");
            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.WriteLine("    원하시는 행동을 입력해주세요.");

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

        public static void BattleFailureUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("┏━━━━━━━━━━━━━━━<< 전투 결과 >>━━━━━━━━━━━━━━━┓");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("              ◆ 전투에 패배하셨습니다! ◆");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine($"    ▶ Lv.{player.Level} [{player.Name}]");
            Console.WriteLine();
            Console.WriteLine($"    ▶ 현재 경험치 : {player.Exp}/{player.MaxExp}");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.WriteLine("    ▶ 0. 복귀하기");
            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
            Console.WriteLine("    원하시는 행동을 입력해주세요.");

            if (CheckInput(0, 0) == 0)
            {
                Stage = 0; // 사망하고 돌아가면서 스테이지 값 초기화
                DisplayMainUI();
            }
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
                    Console.WriteLine($"    [{i + 1}] [Lv.{m.Level}][{m.Name}] {GetMonsterStatus(m)}");
                    Console.ResetColor();
                    Console.WriteLine();
                }
                else
                {
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"    [{i + 1}] [Lv.{m.Level}][{m.Name}] {GetMonsterStatus(m)}");
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
                return new List<Monster> {MonsterFactory.CreateMonster(bossname)};
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



    public class DamageCalculation // 데미지 계산식
    {
        private static Random rand = new Random();

        public static bool Evasion() // 몬스터 회피 
        {
            return rand.Next(0, 100) < 10; // 10% 확률
        }

        public static bool IsCritical() // 플레이어 치명타
        {
            return rand.Next(0, 100) < 20; // 20% 확률
        }

        public static double GetRandomMultiplier() // 공격 시 피해량 0.9 ~ 1.1 랜덤 설정
        {
            return rand.NextDouble() * 0.2 + 0.9; // 0.9 ~ 1.1 사이 배율
        }
    }


    // 챕터를 구분하는 클래스 생성, 4챕터까지 존재
    public static class ChapterInfo
    {
        public static string[] ChapterTitle = { "어두운 숲", "깊은 동굴", "던전 상층", "던전 하층" };

        public static void ChapterDesc(int Chapter)
        {
            switch (Chapter)
            {
                case 0:
                    Console.WriteLine("당신은 수많은 사람들의 발자국이 남은 길을 따라 어둑한 숲 속으로 서서히 스며듭니다.");
                    Console.WriteLine("저편의 어둠 속에서는 이름 모를 짐승의 울음소리가 들려오는 듯 합니다.");
                    Console.WriteLine("진흙 위 발자국들은 어느새 더 이상 사람의 것으로 보이지 않습니다.");
                    break;
                case 1:
                    Console.WriteLine("동굴 입구에 다다른 당신은, 그 앞에 남겨진 몇몇 발자국에서 묘한 기운을 느낍니다.");
                    Console.WriteLine("누군가의 두려움, 그리고 되돌아가고 싶었던 마음이 고스란히 전해지는 듯합니다.");
                    Console.WriteLine("당신 역시 잠시 발걸음을 멈추고, 깊은 숨을 내쉽니다.");
                    break;
                case 2:
                    Console.WriteLine("당신은 길을 가로막던 모든 적들을 쓰러뜨리고, 마침내 던전의 입구에 도달했습니다만 그 마음은 한없이 무겁습니다.");
                    Console.WriteLine("질척이는 탐욕과 후회의 그림자가 발끝을 물고 늘어지는 듯 합니다.");
                    Console.WriteLine("멀리서 빛나는 고양이의 눈빛을 애써 피하며 당신은 발걸음을 옮깁니다.");
                    break;
                case 3:
                    Console.WriteLine("던전에 처음 들어섰을 때, 당신은 당신의 고통과 투지가 누군가의 오락거리로 소비되고 있음에 불쾌감을 느꼈습니다.");
                    Console.WriteLine("그러나 시간이 지날수록 이 기묘한 분위기에 점점 익숙해졌고, 당신도 모르는 새즐거움을 느끼기 시작합니다. ");
                    Console.WriteLine("이제는 이 던전을 하나의 놀이로 받아들이며, 이 놀이의 일부가 되고 싶다는 욕망을 느낍니다.");
                    break;
            }

        }

    }

    public static class BossInfo
    {
        public static void BossDesc(int Chapter)
        {
            switch (Chapter)
            {
                case 0:
                    Console.WriteLine("숲이 한차례 더 어두워집니다. 짙은 안개와 어둠이 뒤엉킨 공간에서 갑작스레 정적이 찾아옵니다.");
                    Console.WriteLine("바스락거리는 풀잎 소리마저 멎은 순간, 거대한 형체가 모습을 드러냅니다.");
                    Console.WriteLine("북슬북슬한 털과 느긋한 기운에도 불구하고, 그 존재를 마주한 순간 당신은 깨닫습니다.");
                    Console.WriteLine("이 숲을 지나기 위해서는 이 알 수 없는 거대한 존재와 맞서야 한다는 것을 말입니다.");
                    break;
                case 1:
                    Console.WriteLine("동굴의 어둠을 뚫고 가장 깊은 곳에 들어서자, 희미한 등불을 조명삼아 한 사람이 웅크리고 있습니다.");
                    Console.WriteLine("이곳에서, 당신은 또 다른 자신을 마주한 듯한 기분에 사로잡힙니다.");
                    Console.WriteLine("후회와 두려움에 사로잡힌 모험가.");
                    Console.WriteLine("그는 이 동굴의 어둠만큼이나 무거운 감정을 짊어진 채, 당신의 앞을 가로막고 있습니다.");
                    Console.WriteLine("어디선가 고양이의 울음소리가 들리는 듯 합니다.");
                    break;
                case 2:
                    Console.WriteLine("머리에 왕관을 쓴 이름 모를 짐승이 다시 등장했습니다.");
                    Console.WriteLine("어두운 숲에서 만났던 존재와는 달리 조용하지만 결연한 눈빛으로 당신을 바라봅니다.");
                    Console.WriteLine("그 눈빛에는 흔들림 없는 확신과 사명감이 담겨있는듯 합니다.");
                    Console.WriteLine("당신은 이유 모를 답답함과 불쾌함에 휩싸입니다.");
                    Console.WriteLine("결국, 당신은 그 답답함을 긁어 내려는듯 전투 자세를 취합니다.");
                    break;
                case 3:
                    Console.WriteLine("어둠이 잡초보다 짙게 깔린 던전의 하층부,");
                    Console.WriteLine("당신은 마침내 그 존재와 마주합니다.");
                    Console.WriteLine("");
                    Console.WriteLine("검은 고양이는 호기심 어린 눈빛으로 당신을 바라봅니다.");
                    Console.WriteLine("그 눈동자에는 어떠한 적의도, 악의도 없습니다.");
                    Console.WriteLine("오히려 당신을 새로운 장난감으로 인식하는 듯.");
                    Console.WriteLine("살랑이는 꼬리와 느긋한 몸짓에서 순수한 기쁨만이 묻어납니다.");
                    Console.WriteLine("");
                    Console.WriteLine("바로 그 순수함이 당신의 마음을 천천히, 그러나 확실하게 사로잡습니다.");
                    Console.WriteLine("당신은 알 수 없는 설렘과 기대, 그리고 벗어날 수 없는 매혹에 휩싸여,");
                    Console.WriteLine("검은 고양이와의 마지막 놀이를 시작합니다.\r\n");
                    break;
            }

        }
    }

    public static class Messages
    {

        public static bool Skip = false; // 메시지 스킵 기능
        public static Thread inputThread; // 메시지 스킵 기능

        // 직업별 크리티컬 데미지 메시지

        public static Dictionary<JobType, string[]> CriticalDamageMessage = new Dictionary<JobType, string[]>
{
    { JobType.전사, new[]
    {   "\n\n\n\n    고요한 전장의 메아리.",
        "\n\n    녹슨 검이 다시 한 번, 적의 심장을 겨눈다!" }},

    { JobType.마법사, new[]
    {   "\n\n\n\n    비틀린 마법진이 휘청인다.",
        "\n\n    그 틈을 찔러, 한 줄기 마력이 폭주한다!" }},

    { JobType.영매사, new[]
    {   "\n\n\n\n    말을 잃은 존재의 눈이 빛난다.",
        "\n\n    고양이의 울음 대신, 사념이 퍼져간다…" }},

    { JobType.대장장이, new[]
    {   "\n\n\n\n    녹슨 망치가 손에서 덜컹거린다.",
        "\n\n    마지막 불꽃이 담긴 한 방이 울려 퍼진다!" }},

    { JobType.과학자, new[]
    {   "\n\n\n\n    금기된 공식이 마침내 완성된다.",
        "\n\n    불안정한 빛이 공기를 찢으며 발산된다!" }},
    };


        public static Dictionary<JobType, string[]> CriticalDamageFinalMessage = new Dictionary<JobType, string[]>
{ 
    // 직업별 크리티컬 데미지 마지막 메시지
    
    { JobType.전사, new[]
    {"──  잊힌 전사의 일격이, 다시 역사의 피를 흐르게 했다."}},

    { JobType.마법사, new[]
    {"──  허황된 주문조차 진실의 순간엔 검이 된다."}},

    { JobType.영매사, new[]
    {"──  침묵의 일격은 소리 없이, 그러나 확실히 끝을 남긴다."}},

    { JobType.대장장이, new[]
    {"──  무너진 기억과 함께, 대지에 새긴 죄의 불꽃."}},

    {JobType.과학자, new[]
    {"──  이단의 실험은 성공했고, 희생은 검증되었다."} },
    };

        public static void StartSkipListener()
        {
            Skip = false;
            inputThread = new Thread(() =>
            {
                Console.ReadKey(true);
                Skip = true;
            });
            inputThread.IsBackground = true;
            inputThread.Start();
        }

        public static void PrintMessageWithSkip(string message, int delay = 100)
        {
            foreach (char c in message)
            {
                if (Skip) break;
                Console.Write(c);

                int elapsed = 0;
                while (elapsed < delay)
                {
                    if (Skip) break;
                    Thread.Sleep(10);
                    elapsed += 10;
                }
            }
        }

        public static void PrintLinesWithSkip(string[] lines, int charDelay = 30, int lineDelay = 800)
        {
            StartSkipListener();

            foreach (string line in lines)
            {
                PrintMessageWithSkip(line, charDelay);
                if (Skip) break;
                Thread.Sleep(lineDelay);

                int elapsed = 0;
                while (elapsed < lineDelay)
                {
                    if (Skip) break;
                    Thread.Sleep(10);
                    elapsed += 10;
                }
            }

            Skip = false; // 다음 출력을 위해 초기화

        }
    }
}

