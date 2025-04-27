using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Sparta_Dungeon_TeamProject;
using static Sparta_Dungeon_TeamProject.Program;

namespace Sparta_Dungeon_TeamProject
{
    public class Messages
    {
        public bool Skip = false; // 메시지 스킵 기능
        public Thread inputThread; // 메시지 스킵 기능

        

        // **메인메뉴**
        //public void ShowMainMenu(Player player, Inventory inventory)
        //{
            
        //}

        public void BossDesc(int Chapter)
        {
            switch (Chapter)
            {
                case 0:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"{"",3}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}숲이 한차례 더 어두워집니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}짙은 안개와 어둠이 뒤엉킨 공간에서 정적이 찾아옵니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}바스락거리는 풀잎 소리마저 멎은 순간, 거대한 형체가 모습을 드러냅니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}북슬북슬한 털과 느긋한 기운. 그러나, 그 존재를 마주한 순간 본능적으로 깨닫습니다.");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{"",7}이 숲을 지나기 위해서는, 이 알 수 없는 존재와 맞서야만 합니다.");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"{"",3}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                    Console.WriteLine();
                    Console.ResetColor();
                    break;

                case 1:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"{"",3}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}동굴의 어둠을 뚫고 가장 깊은 곳.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}희미한 등불 아래, 한 사람이 조용히 웅크리고 있습니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}그의 실루엣은 마치, 또 다른 '당신' 같습니다.");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{"",7}후회와 두려움을 짊어진 모험가가 당신의 앞을 가로막습니다.");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}어디선가 고양이의 울음소리가 조용히 들려옵니다.");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine();
                    Console.WriteLine($"{"",3}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                    Console.WriteLine();
                    Console.ResetColor();
                    break;

                case 2:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"{"",3}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}머리에 왕관을 쓴 이름 모를 짐승이 다시 등장했습니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}그 눈빛은 조용하지만 결연합니다. 흔들림 없는 사명감이 담겨 있습니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}당신은 이유 모를 불쾌함과 답답함에 사로잡힙니다.");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine();
                    Console.WriteLine($"{"",7}그 감정은 칼끝으로 이어지고, 전투는 피할 수 없습니다.");
                    Console.WriteLine();
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"{"",3}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                    Console.WriteLine();
                    Console.ResetColor();
                    break;

                case 3:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"{"",3}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                    Console.ResetColor();
                    Console.WriteLine();

                    Console.WriteLine($"{"",10}던전의 하층부. 어둠이 잡초보다 짙게 깔린 그곳.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}검은 고양이는 느긋한 몸짓으로 당신을 바라봅니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}그 눈엔 적의도, 악의도 없습니다. 단지, 순수한 호기심.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}꼬리를 살랑이며, 그는 장난감을 맞이하듯 기뻐합니다.");
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{"",7}그리고 당신은 알 수 없는 매혹에 사로잡힌 채, 마지막 '놀이'를 시작합니다.");
                    Console.ResetColor();
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"{"",3}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                    Console.ResetColor();
                    Console.WriteLine();
                    break;
            }

        }



        public string[] ChapterTitle = { "어두운 숲", "깊은 동굴", "던전 상층", "던전 하층" };

        public void ChapterDesc(int Chapter)
        {
            switch (Chapter)
            {
                case 0:
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}당신은 수많은 사람들의 발자국이 남은 길을 따라");
                    Console.WriteLine($"{"",10}어둑한 숲 속으로 서서히 스며듭니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}저편의 어둠 속에서는 이름 모를 짐승의 울음소리가");
                    Console.WriteLine($"{"",10}들려오는 듯 합니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}진흙 위 발자국들은 어느새 더 이상");
                    Console.WriteLine($"{"",10}사람의 것으로 보이지 않습니다.");
                    break;

                case 1:
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}동굴 입구에 다다른 당신은, 그 앞에 남겨진");
                    Console.WriteLine($"{"",10}몇몇 발자국에서 묘한 기운을 느낍니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}누군가의 두려움, 그리고 되돌아가고 싶었던 마음이");
                    Console.WriteLine($"{"",10}고스란히 전해지는 듯합니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}당신 역시 잠시 발걸음을 멈추고, 깊은 숨을 내쉽니다.");
                    break;

                case 2:
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}당신은 길을 가로막던 모든 적들을 쓰러뜨리고,");
                    Console.WriteLine($"{"",10}마침내 던전의 입구에 도달했습니다만");
                    Console.WriteLine($"{"",10}그 마음은 한없이 무겁습니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}질척이는 탐욕과 후회의 그림자가");
                    Console.WriteLine($"{"",10}발끝을 물고 늘어지는 듯 합니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}멀리서 빛나는 고양이의 눈빛을 애써 피하며");
                    Console.WriteLine($"{"",10}당신은 발걸음을 옮깁니다.");
                    break;

                case 3:
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}던전에 처음 들어섰을 때, 당신은 당신의 고통과 투지가");
                    Console.WriteLine($"{"",10}누군가의 오락거리로 소비되고 있음에 불쾌감을 느꼈습니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}그러나 시간이 지날수록 이 기묘한 분위기에");
                    Console.WriteLine($"{"",10}점점 익숙해졌고, 당신도 모르는 새 즐거움을 느끼기 시작합니다.");
                    Console.WriteLine();
                    Console.WriteLine($"{"",10}이제는 이 던전을 하나의 놀이로 받아들이며,");
                    Console.WriteLine($"{"",10}이 놀이의 일부가 되고 싶다는 욕망을 느낍니다.");
                    break;
            }
        }

        /*public void CriticalMes(Player player)
        {
            JobType job = player.Job;

            string[] impactLines = Messages.CriticalDamageMessage.ContainsKey(job)
                ? Messages.CriticalDamageMessage[job]
                : new[]
                {
                    "\n\n\n\n    ...정적 속에", // 직업 목록에 존재하지 않으면 뜨는 기본 메시지
                    "\n\n    강렬한 일격이 내려친다!"
                };
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

            //흔들림 2회(화면 깜빡임)
            for (int i = 0; i < 2; i++)
            {
                if (Messages.Skip) break;

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                string prefix = new string(' ', 4 + i % 2);
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n" + prefix + criticalImpact);
                Console.ResetColor();
                Thread.Sleep(120);
            }

            // 마지막 1회는 클리어 없이 그대로 출력 (최종 상태)
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n    " + criticalImpact);
            Console.ResetColor();
            Console.WriteLine("\n\n\n");
        }

        // 직업별 크리티컬 데미지 메시지

        public Dictionary<JobType, string[]> CriticalDamageMessage = new Dictionary<JobType, string[]>
        {
        { JobType.전사, new[]
        {   "\n\n\n\n\n\n\n\n    고요한 전장의 메아리.",
            "\n\n\n\n\n\n    녹슨 검이 다시 한 번, 적의 심장을 겨눈다!" }},

        { JobType.마법사, new[]
        {   "\n\n\n\n    비틀린 마법진이 휘청인다.",
            "\n\n\n\n\n\n    그 틈을 찔러, 한 줄기 마력이 폭주한다!" }},

        { JobType.영매사, new[]
        {   "\n\n\n\n\n\n\n\n    말을 잃은 존재의 눈이 빛난다.",
            "\n\n\n\n\n\n    고양이의 울음 대신, 사념이 퍼져간다…" }},

        { JobType.대장장이, new[]
        {   "\n\n\n\n\n\n\n\n    녹슨 망치가 손에서 덜컹거린다.",
            "\n\n\n\n\n\n    마지막 불꽃이 담긴 한 방이 울려 퍼진다!" }},

        { JobType.과학자, new[]
        {   "\n\n\n\n\n\n\n\n    금기된 공식이 마침내 완성된다.",
            "\n\n\n\n\n\n    불안정한 빛이 공기를 찢으며 발산된다!" }},
        };


        public Dictionary<JobType, string[]> CriticalDamageFinalMessage = new Dictionary<JobType, string[]>
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

        public void StartSkipListener()
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

        public void PrintMessageWithSkip(string message, int delay = 100)
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

        public void PrintLinesWithSkip(string[] lines, int charDelay = 30, int lineDelay = 800)
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

        public void EvasionMess(Monster target, Program Playerturn)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"{"",10}[Lv.{target.Level}][{target.Name}] (은)는 공격을 손쉽게 회피했다!");
            Console.ResetColor();
            Thread.Sleep(700);
            Sparta_Dungeon_TeamProject.Program.Playerturn = false;
            return;
        }
        
    }*/
}