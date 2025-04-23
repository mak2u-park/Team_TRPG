using System.ComponentModel;
using System.ComponentModel.Design;
using System.Numerics;
using System.Threading;
using System.Xml.Linq;
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
        public static int Stage = 1; // 스테이지 변수


        // 4. 던전
        static void DisplayDungeonUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"**던전입장 - 현재 스테이지 {Stage} **");
            Console.WriteLine();
            Console.WriteLine("1. 입장");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            switch (CheckInput(0, 1))
            {
                case 0:
                    DisplayMainUI(); break;
                case 1:
                    Battle(); break;
            }
        }
        static void Battle()
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
        static void PlayerTurn()
        {
            Console.Clear();
            Console.WriteLine("당신의 턴입니다.");
            Console.WriteLine();
            Console.WriteLine($"현재 턴 수 : {BattleTurn}");
            Console.WriteLine();
            Console.WriteLine($"현재 HP : {player.Hp}/{player.MaxHp}");
            Console.WriteLine($"현재 MP : {player.Mp}/{player.MaxMp}");
            Console.WriteLine();
            Console.WriteLine("**현재 전투 중 입니다.**");
            Console.WriteLine();

            PrintMonsters(); // 몬스터 출력

            Console.WriteLine();
            Console.WriteLine("1. 일반 공격");
            Console.WriteLine("2. 스킬 선택");
            Console.WriteLine("3. 상태 보기");
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
            Console.WriteLine("상대의 턴입니다.");
            Console.WriteLine();
            Console.WriteLine($"현재 턴 수 : {BattleTurn}");
            Console.WriteLine();
            Console.WriteLine($"현재 HP : {player.Hp}/{player.MaxHp}");
            Console.WriteLine($"현재 MP : {player.Mp}/{player.MaxMp}");
            Console.WriteLine();
            Console.WriteLine("**현재 전투 중 입니다.**");
            Console.WriteLine();
            Thread.Sleep(500);
            foreach (var m in battleMonsters)
            {

                if (m.IsAlive)
                {
                    Console.WriteLine($"[Lv.{m.Level}][{m.Name}] (이)가 공격을 시도 합니다!");
                    Console.WriteLine();
                    player.Damage(m.Atk);
                    Console.WriteLine();
                    Console.WriteLine($"현재 HP : {player.Hp}/{player.MaxHp}");
                    Console.WriteLine();
                    Thread.Sleep(300);
                }
            }

            Console.WriteLine();
            Console.WriteLine("아무 키나 눌러 다음으로 넘어가세요.");
            Console.ReadKey();

            Playerturn = true; // 플레이어 턴으로 변경
            BattleTurn++; // 전체적인 한 턴이 끝났으니 턴 수 증가
        }
        static void PlayerAttack()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("**현재 일반 공격을 시도하는 중 입니다.**");
            Console.WriteLine();
            Console.WriteLine($"현재 턴 수 : {BattleTurn}");
            Console.WriteLine();

            PrintMonsters(); // 몬스터 출력

            Console.WriteLine();
            Console.WriteLine("0. 취소");
            Console.WriteLine();
            Console.WriteLine("대상을 선택해주세요.");

            int result = CheckInput(0, battleMonsters.Count);

            if (result == 0) return;

            Monster target = battleMonsters[result - 1];

            if (!target.IsAlive)
            {
                Console.WriteLine();
                Console.WriteLine("이미 사망한 몬스터는 공격할 수 없습니다.");
                Console.WriteLine();
                Thread.Sleep(500);
                Playerturn = true;
                return;
            }

            target.Hp -= player.Atk; // 몬스터를 때리는 플레이어 데미지 계산식

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"[Lv.{target.Level}][{target.Name}] 에게 {player.Atk} 만큼 피해를 입혔다!");
            Thread.Sleep(700);

            if (target.Hp <= 0)
            {
                KillMon++; // 몬스터 처치 수 증가
                target.IsAlive = false;
                DisplayKillMessage(target);
                player.GainReward(target.DropGold, target.DropExp);
                Console.WriteLine();
                Console.WriteLine("아무 키나 눌러 다음으로 넘어가세요.");
                Console.ReadKey();

                if (battleMonsters.All(m => !m.IsAlive))
                {
                    BattleSuccessUI();
                    return;
                }
            }

            Playerturn = false; // 몬스터의 턴으로 변경
        }
        static void DisplayKillMessage(Monster target)
        {
            Console.WriteLine();
            Console.WriteLine($"[Lv.{target.Level}][{target.Name}] (은)는 일격을 맞고 사망했다!");
            Thread.Sleep(700);
            Console.WriteLine();
            Console.WriteLine($"{target.DropGold} G 를 획득했다.");
            Console.WriteLine($"{target.DropExp} 만큼 경험치를 획득했다.");
            Console.WriteLine();
            Console.WriteLine($"보유 골드 {player.Gold}");
            Console.WriteLine($"현재 경험치 {player.Exp}/{player.MaxExp}");
        }

        public static void BattleSuccessUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("전투 승리!");
            Console.WriteLine();
            Console.WriteLine($"현재 층에서 몬스터를 {KillMon} 마리 만큼 처치 하셨습니다.");
            Console.WriteLine();
            Console.WriteLine($"Lv.{player.Level} [{player.Name}]");
            Console.WriteLine($"현재 HP : {player.Hp}/{player.MaxHp}");
            Console.WriteLine($"현재 MP : {player.Mp}/{player.MaxMp}");
            Console.WriteLine();
            Console.WriteLine($"현재 경험치 {player.Exp}/{player.MaxExp}");
            Console.WriteLine();
            Console.WriteLine("1. 다음 층으로 이동하기");
            Console.WriteLine("0. 복귀하기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            switch (CheckInput(0, 1))
            {
                case 0:
                    Stage = 1; // 마을로 복귀하면서 스테이지 값 초기화
                    DisplayMainUI();
                    break;
                case 1:
                    Stage++; // 다음 층으로 이동하면서 스테이지 값 +1
                    DisplayDungeonUI(); // 이건 임시로 작성해둔 것. 추후에 다른 걸로 교체해야함
                    break;

            }
        }

        public static void BattleFailureUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("전투 패배!");
            Console.WriteLine();
            Console.WriteLine($"Lv.{player.Level} [{player.Name}]");
            Console.WriteLine();
            Console.WriteLine("0. 복귀");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            if (CheckInput(0, 0) == 0)
            {
                Stage = 1; // 사망하고 돌아가면서 스테이지 값 초기화
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
                    Console.WriteLine($"[{i + 1}] [Lv.{m.Level}][{m.Name}] {GetMonsterStatus(m)}");
                    Console.WriteLine();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"[{i + 1}] [Lv.{m.Level}][{m.Name}] {GetMonsterStatus(m)}");
                    Console.ResetColor();
                    Console.WriteLine();
                }
            }
        }
    }

    public static class MonsterSpawner
    {
        public static List<Monster> SpawnMonsters(int Stage)
        {
            Random rand = new Random();
            List<MonsterType> allowedTypes;
            int monsterCount = Math.Min(1 + Program.Stage / 2, 5);

            if (Stage < 4) // 1 ~ 3 스테이지
                allowedTypes = new List<MonsterType> { MonsterType.Goblin }; // 고블린만 소환
            else if (Stage < 7) // 4 ~ 6 스테이지
                allowedTypes = new List<MonsterType> { MonsterType.Goblin, MonsterType.Orc }; // 고블린과 오크만 소환
            else // 7 ~ 스테이지
                allowedTypes = Enum.GetValues(typeof(MonsterType)).Cast<MonsterType>().ToList(); // 모든 몬스터 소환

            return Enumerable.Range(0, monsterCount)
                    .Select(_ => MonsterFactory.CreateMonster(allowedTypes[rand.Next(allowedTypes.Count)]))
                    .ToList();
        }
    }
}