namespace Sparta_Dungeon_TeamProject
{
    // 던전
    public partial class Program
    {
        static void DisplayDungeonUI()
        {
            Console.Clear();
            Console.WriteLine("**던전입장 - 준비중입니다**");
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
    }
}
