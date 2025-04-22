using System.Threading;
using System.Xml.Linq;

namespace Sparta_Dungeon_TeamProject
{
    public partial class Program
    {
        static string GetMonsterStatus(int hp, bool alive) // 살아있다면 Hp를 표시해주고 죽어있다면 [사망]을 표시해줌
        {
            return alive ? $"[HP:{hp}]" : "[사망]";
        }

        public static int Mhp1 = 10; // 임시로 만든 몬스터의 체력값
        public static int Mhp2 = 10; // 임시로 만든 몬스터의 체력값
        public static int Mhp3 = 10; // 임시로 만든 몬스터의 체력값


        public static int Mlv1 = 1; // 임시로 만든 몬스터의 레벨값
        public static int Mlv2 = 2; // 임시로 만든 몬스터의 레벨값
        public static int Mlv3 = 3; // 임시로 만든 몬스터의 레벨값


        public static int Matk1 = 3; // 임시로 만든 몬스터의 공격값
        public static int Matk2 = 5; // 임시로 만든 몬스터의 공격값
        public static int Matk3 = 50; // 임시로 만든 몬스터의 공격값

        public static int Monnum = 3; // 임시로 만든 전투용 몬스터 리스트에 남은 몬스터 값


        public static int KillMon = 0; // 임시로 만든 몬스터 처치 횟수 값


        public static bool Mlife1 = true; // 임시로 만든 몬스터의 생존 여부
        public static bool Mlife2 = true; // 임시로 만든 몬스터의 생존 여부
        public static bool Mlife3 = true; // 임시로 만든 몬스터의 생존 여부


        public static bool Playerturn = true; // 플레이어의 턴 여부


        public static string Mname1 = "슬라임"; // 임시로 만든 몬스터의 명칭
        public static string Mname2 = "고약한 고블린"; // 임시로 만든 몬스터의 명칭
        public static string Mname3 = "미니 카피바라"; // 임시로 만든 몬스터의 명칭


        public static string MonsterLifeMark1 = Mlife1 ? $"[HP:{Mhp1}]" : "[사망]"; // 임시로 만든 몬스터의 사망 여부에 따른 체력 / 사망 표시 
        public static string MonsterLifeMark2 = Mlife2 ? $"[HP:{Mhp2}]" : "[사망]"; // 임시로 만든 몬스터의 사망 여부에 따른 체력 / 사망 표시  
        public static string MonsterLifeMark3 = Mlife3 ? $"[HP:{Mhp3}]" : "[사망]"; // 임시로 만든 몬스터의 사망 여부에 따른 체력 / 사망 표시 

        // 4. 던전
        static void DisplayDungeonUI()
        {
            Console.Clear();
            Console.WriteLine("**던전입장 - 준비중입니다**");
            Console.WriteLine();
            Console.WriteLine("1. 입장");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, 1);
            switch (result)
            {
                case 0:
                    DisplayMainUI();
                    break;
                case 1:
                    Battle();
                    break;
            }
        }
        static void Battle()
        {
            bool BettleLoop = true; // 전투 진행 루프를 켜줌

            while (BettleLoop) // 전투 진행 루프가 켜져 있다면
            {
                if (Playerturn) // 만약 플레이어의 턴이 맞다면
                {
                    Console.Clear();
                    Console.WriteLine("당신의 턴입니다.");
                    Console.WriteLine();
                    Console.WriteLine("**현재 전투 중 입니다.**");
                    Console.WriteLine();
                    // 몬스터가 출력되는 부분은 이후에 인벤토리에서 아이템을 가져온 것 처럼 몬스터 리스트에서 전투용 몬스터 리스트로 몬스터를 출력시켜야 할 듯
                    Console.WriteLine($"[Lv.{Mlv1}][{Mname1}] {GetMonsterStatus(Mhp1, Mlife1)}");
                    Console.WriteLine($"[Lv.{Mlv2}][{Mname2}] {GetMonsterStatus(Mhp2, Mlife2)}");
                    Console.WriteLine($"[Lv.{Mlv3}][{Mname3}] {GetMonsterStatus(Mhp3, Mlife3)}");
                    Console.WriteLine();
                    Console.WriteLine("1. 일반 공격");
                    Console.WriteLine("2. 스킬 선택");
                    Console.WriteLine();
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    Console.WriteLine();

                    int result = CheckInput(1, 2);
                    switch (result)
                    {
                        case 1:
                            PlayerAttack(); // 플레이어 공격 불러오기
                            break;
                        case 2:
                            //Player.SkillUI ->     스킬창 불러오기
                            break;
                    }
                }

                else // 만약 플레이어의 턴이 아니라면
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("상대의 턴입니다.");
                    Console.WriteLine();
                    Console.WriteLine("**현재 전투 중 입니다.**");
                    Console.WriteLine();
                    Thread.Sleep(500);

                    if (Mlife1) // 이를 리스트로 불러와서 전투중인 몬스터의 수 만큼 불러와서 한 마리씩 차례대로 불러온 뒤 타격하도록 설정해야함.
                    {
                        Console.WriteLine($"[Lv.{Program.Mlv1}][{Program.Mname1}] 가 공격을 시도 합니다!");
                        player.Damage(Matk1); // 데미지 받는 것 불러오기 , 여기 수치도 리스트 값 처럼 불러와야함
                        Thread.Sleep(700);
                    }

                    if (Mlife2)
                    {
                        Console.WriteLine($"[Lv.{Program.Mlv2}][{Program.Mname2}] 가 공격을 시도 합니다!");
                        player.Damage(Matk2); // 데미지 받는 것 불러오기 , 여기 수치도 리스트 값 처럼 불러와야함
                        Thread.Sleep(700);
                    }

                    if (Mlife3)
                    {
                        Console.WriteLine($"[Lv.{Program.Mlv3}][{Program.Mname3}] 가 공격을 시도 합니다!");
                        player.Damage(Matk3); // 데미지 받는 것 불러오기 , 여기 수치도 리스트 값 처럼 불러와야함
                        Thread.Sleep(700);
                    }

                    Console.WriteLine();
                    Console.WriteLine("0. 다음");
                    Console.WriteLine();

                    CheckInput(0, 0);

                    Playerturn = true; // 턴 넘기기
                }

                if (KillMon >= 3) // 다 잡았을 때 성공으로 이동
                {
                    BattleSuccessUI();
                    BettleLoop = false;
                    break;
                }
                if (!player.PlayerAlive)
                {
                    break;
                }
            }
        }
        static void PlayerAttack()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("**현재 일반 공격을 시도하는 중 입니다.**");
            Console.WriteLine();
            Console.WriteLine($"1. [Lv.{Mlv1}][{Mname1}] {GetMonsterStatus(Mhp1, Mlife1)}");
            Console.WriteLine($"2. [Lv.{Mlv2}][{Mname2}] {GetMonsterStatus(Mhp2, Mlife2)}");
            Console.WriteLine($"3. [Lv.{Mlv3}][{Mname3}] {GetMonsterStatus(Mhp3, Mlife3)}");
            Console.WriteLine();
            Console.WriteLine("0. 취소");
            Console.WriteLine();
            Console.WriteLine("대상을 선택해주세요.");

            int result = CheckInput(0, 3); // CheckInput의 숫자를 0, [전투용 리스트에서 뽑힌 몬스터 마릿수] 로 설정해야 할 듯
            switch (result)
            {
                case 0:
                    Program.Battle();
                    break;

                case 1:

                    Program.Mhp1 -= Program.player.Atk; // 몬스터의 체력 값을 플레이어 공격력 값 만큼 뺀다.

                    if (Program.Mhp1 <= 0) // 몬스터의 체력이 0보다 작거나 같다면
                    {
                        if (Mlife1) // 몬스터가 살아있다면
                        {
                            Console.WriteLine($"[Lv.{Program.Mlv1}][{Program.Mname1}] 에게 {Program.player.Atk} 만큼 피해를 입혔다!");
                            Thread.Sleep(700);
                            Console.WriteLine();
                            Console.WriteLine($"[Lv.{Program.Mlv1}][{Program.Mname1}] 은(는) 일격을 맞고 사망했다!"); // 사망 메시지 출력
                            KillMon += 1; // 몬스터 처치 값 +1 추가
                            Mlife1 = false; // 몬스터가 사망했으니 생존 값을 false로 변경
                            Monnum--; // 몬스터 마릿수 감소 // 추후에 배틀 몬스터 리스트에서 이 몬스터를 빼는 과정으로 수정
                            Thread.Sleep(700);

                            // 몬스터 보상을 출력한다고 하면 여기에 출력하면 될 것 같음

                            if (Monnum > 0) // 필드에 존재하는 몬스터의 수가 0보다 크다면
                            {
                                Playerturn = false; // 플레이어의 공격이 끝났으니 플레이어 턴 값을 false 로 변경
                                return;
                            }
                            else // 필드에 존재하는 몬스터의 수가 0 이라면
                            {
                                BattleSuccessUI(); // 배틀 성공 창을 띄움
                                return;
                            }
                        }
                        else // 몬스터가 죽어있다면
                        {
                            Console.WriteLine();
                            Console.WriteLine("이미 사망한 몬스터는 공격할 수 없습니다.");
                            Console.WriteLine();
                            Thread.Sleep(500);
                        }
                    }
                    else // 몬스터의 체력이 0보다 크다면
                    {
                        Console.WriteLine($"당신이 휘두른 검은 바람을 가르며 목표물을 베었다!");
                        Console.WriteLine();
                        Thread.Sleep(700);
                        Console.WriteLine($"[Lv.{Program.Mlv1}][{Program.Mname1}] 에게 {Program.player.Atk} 만큼 피해를 입혔다!");
                        Console.WriteLine();
                        Thread.Sleep(700);
                        Console.WriteLine($"[Lv.{Program.Mlv1}][{Program.Mname1}] 의 현재 HP [{Program.Mhp1}]");
                        Thread.Sleep(700);
                        Playerturn = false;
                        Program.Battle();
                    }
                    break;

                case 2:

                    Program.Mhp2 -= Program.player.Atk; // 몬스터의 체력 값을 플레이어 공격력 값 만큼 뺀다.

                    if (Program.Mhp2 <= 0) // 몬스터의 체력이 0보다 작거나 같다면
                    {
                        if (Mlife2) // 몬스터가 살아있다면
                        {
                            Console.WriteLine($"[Lv.{Program.Mlv2}][{Program.Mname2}] 에게 {Program.player.Atk} 만큼 피해를 입혔다!");
                            Thread.Sleep(700);
                            Console.WriteLine();
                            Console.WriteLine($"[Lv.{Program.Mlv2}][{Program.Mname2}] 은(는) 일격을 맞고 사망했다!"); // 사망 메시지 출력
                            KillMon += 1; // 몬스터 처치 값 +1 추가
                            Mlife2 = false; // 몬스터가 사망했으니 생존 값을 false로 변경
                            Monnum--; // 몬스터 마릿수 감소 // 추후에 배틀 몬스터 리스트에서 이 몬스터를 빼는 과정으로 수정
                            Thread.Sleep(700);

                            // 몬스터 보상을 출력한다고 하면 여기에 출력하면 될 것 같음

                            if (Monnum > 0) // 필드에 존재하는 몬스터의 수가 0보다 크다면
                            {
                                Playerturn = false; // 플레이어의 공격이 끝났으니 플레이어 턴 값을 false 로 변경
                                return;
                            }
                            else // 필드에 존재하는 몬스터의 수가 0 이라면
                            {
                                return;
                            }
                        }
                        else // 몬스터가 죽어있다면
                        {
                            Console.WriteLine();
                            Console.WriteLine("이미 사망한 몬스터는 공격할 수 없습니다.");
                            Console.WriteLine();
                            Thread.Sleep(500);
                        }
                    }
                    else // 몬스터의 체력이 0보다 크다면
                    {
                        Console.WriteLine($"당신이 휘두른 검은 바람을 가르며 목표물을 베었다!");
                        Console.WriteLine();
                        Thread.Sleep(700);
                        Console.WriteLine($"[Lv.{Program.Mlv2}][{Program.Mname2}] 에게 {Program.player.Atk} 만큼 피해를 입혔다!");
                        Console.WriteLine();
                        Thread.Sleep(700);
                        Console.WriteLine($"[Lv.{Program.Mlv2}][{Program.Mname2}] 의 현재 HP [{Program.Mhp2}]");
                        Thread.Sleep(700);
                        Playerturn = false;
                        Program.Battle();
                    }
                    break;

                case 3:

                    Program.Mhp3 -= Program.player.Atk; // 몬스터의 체력 값을 플레이어 공격력 값 만큼 뺀다.

                    if (Program.Mhp3 <= 0) // 몬스터의 체력이 0보다 작거나 같다면
                    {
                        if (Mlife3) // 몬스터가 살아있다면
                        {
                            Console.WriteLine($"[Lv.{Program.Mlv3}][{Program.Mname3}] 에게 {Program.player.Atk} 만큼 피해를 입혔다!");
                            Thread.Sleep(700);
                            Console.WriteLine();
                            Console.WriteLine($"[Lv.{Program.Mlv3}][{Program.Mname3}] 은(는) 일격을 맞고 사망했다!"); // 사망 메시지 출력
                            KillMon += 1; // 몬스터 처치 값 +1 추가
                            Mlife3 = false; // 몬스터가 사망했으니 생존 값을 false로 변경
                            Monnum--; // 몬스터 마릿수 감소 // 추후에 배틀 몬스터 리스트에서 이 몬스터를 빼는 과정으로 수정
                            Thread.Sleep(700);

                            // 몬스터 보상을 출력한다고 하면 여기에 출력하면 될 것 같음

                            if (Monnum > 0) // 필드에 존재하는 몬스터의 수가 0보다 크다면
                            {
                                Playerturn = false; // 플레이어의 공격이 끝났으니 플레이어 턴 값을 false 로 변경
                                return;
                            }
                            else // 필드에 존재하는 몬스터의 수가 0 이라면
                            {
                                return;
                            }
                        }
                        else // 몬스터가 죽어있다면
                        {
                            Console.WriteLine();
                            Console.WriteLine("이미 사망한 몬스터는 공격할 수 없습니다.");
                            Console.WriteLine();
                            Thread.Sleep(500);
                        }
                    }
                    else // 몬스터의 체력이 0보다 크다면
                    {
                        Console.WriteLine($"당신이 휘두른 검은 바람을 가르며 목표물을 베었다!");
                        Console.WriteLine();
                        Thread.Sleep(700);
                        Console.WriteLine($"[Lv.{Program.Mlv3}][{Program.Mname3}] 에게 {Program.player.Atk} 만큼 피해를 입혔다!");
                        Console.WriteLine();
                        Thread.Sleep(700);
                        Console.WriteLine($"[Lv.{Program.Mlv3}][{Program.Mname3}] 의 현재 HP [{Program.Mhp3}]");
                        Thread.Sleep(700);
                        Playerturn = false;
                        Program.Battle();
                    }
                    break;
            }
        }




        public static void BattleSuccessUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("전투 승리!");
            Console.WriteLine();
            Console.WriteLine($"던전에서 몬스터를 {KillMon} 마리 만큼 처치 하셨습니다.");
            Console.WriteLine();
            Console.WriteLine($"Lv.{player.Level} [{player.Name}]");
            Console.WriteLine();
            Console.WriteLine("0. 다음");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            KillMon = 0;

            int result = CheckInput(0, 0);
            switch (result)
            {
                case 0:
                    //다음 층으로 이동하는 코드
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

            int result = CheckInput(0, 0);
            switch (result)
            {
                case 0:
                    Program.DisplayMainUI();
                    break;
            }
        }
    }
}
//
//public bool PlayerAlive = true;
//
//public void Damage(int amount)
//{
//    if (Hp > 0)
//    {
//        Hp -= amount;
//        Console.WriteLine();
//        Console.WriteLine($"[{Name} 은(는) {amount} 만큼 피해를 입었습니다!");
//        Console.WriteLine();
//        Console.WriteLine($"[현재 HP {Hp}]");
//        Console.WriteLine();

//        if (Hp <= 0)
//        {
//            PlayerAlive = false;
//            Program.BattleFailureUI();
//            return;
//        }
//    }

// Player.cs 에서 데미지를 위와 같이 코드를 추가해주면 정상 작동