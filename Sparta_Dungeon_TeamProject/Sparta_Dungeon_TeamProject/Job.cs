using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparta_Dungeon_TeamProject
{
    // 직업 DB
    public enum JobType
    {
        전사 = 1,
        마법사,
        과학자,
        대장장이,
        영매사
    }

    public interface IJob
    {
        JobType Type { get; } // 직업 타입-코드작업용
        string DisplayName { get; } // 출력될 직업명
        string Story { get; } // 서사 - 얘만 선택 완료후 출력.
        string Description { get; } // 간단한 설명
        public int BaseAtk { get; }
        public int BaseCri { get; }
        public int BaseDef { get; }
        public int BaseHp { get; }
        public int BaseMp { get; }
        string Trait { get; } // 직업 특성
    };

    // 직업별 베이스 DB / 공격과 스킬은 Skill에서 정의
    public class Warrior : IJob
    {
        public JobType Type => JobType.전사;
        public string DisplayName => "은퇴한 전사";
        public string Story => "한 때 전장에서 목숨을 바치던 전사였습니다.\n" +
            "\n은퇴 후, 전장을 떠나 산지 벌써 10년이 지난 지금," +
            "\n아직도 세상은 어지러운 상황...전사가 부족합니다.\n" +
            "\n결국 다시 전장으로 갈 수 밖에 없습니다.";
        public string Description => "잃어버린 전투 감각으로 데미지를" +
            "\n       랜덤하게 입히는 트릭형 전사입니다.";
        public int BaseAtk => 10;
        public int BaseCri => 30;
        public int BaseDef => 2;
        public int BaseHp => 70;
        public int BaseMp => 100;
        public string Trait => "치명타 확률 +30%";
    }

    public class Mage : IJob
    {
        public JobType Type => JobType.마법사;
        public string DisplayName => "마법사";
        public string Story => "재능이 미천해 마법사의 탑에서 쫒겨난 후," +
            "\n무시당하기 싫어 허풍만 늘었습니다.\n" +
            "\n뛰어난 사람들에게 질투를 느끼며," +
            "\n던전에서 발견되는 마법서를 얻기 위해" +
            "\n가벼운 마음을 갖고 전장으로 향합니다.";
        public string Description => "떠돌이 마법사로서 세계를 돌아다니며," +
            "\n       스킬 위주의 전투를 펼치는 마법 중심의 직업입니다.";
        public int BaseAtk => 8;
        public int BaseCri => 5;
        public int BaseDef => 2;
        public int BaseHp => 50;
        public int BaseMp => 0;
        public string Trait => "마법 스킬 위주의 전투";
    }

    public class Scientist : IJob
    {
        public JobType Type => JobType.과학자;
        public string DisplayName => "과학자";
        public string Story => "마을에서 추방당한 이단 과학자로서" +
            "\n금기된 재료들을 가지고 위험한 연구를 계속합니다.\n" +
            "\n자신만의 독특한 기술로 세상에 맞서려 합니다.\n";
        public string Description => "금기된 독성 중심의 스킬을 사용하며," +
            "\n       상대적으로 마나 소모가 잦고 체력 소모가 적습니다.";
        public int BaseAtk => 2;
        public int BaseCri => 15;
        public int BaseDef => 5;
        public int BaseHp => 50;
        public int BaseMp => 150;
        public string Trait => "버프량 증가";
    }

    public class Smith : IJob
    {
        public JobType Type => JobType.대장장이;
        public string DisplayName => "대장장이";
        public string Story => "주변 사람들로부터 죽은 줄 알았던 제자를" +
            "\n던전에서 봤다는 소문을 듣게 되어 제자를 찾아 나섭니다.\n" +
            "\n제자의 비극 이후 제련을 멈췄지만" +
            "\n그의 무기에는 상처와 함께 이야기가 남아있습니다.\n";
        public string Description => "약한 능력치를 극복하는 강력한 장비 기반의 직업입니다.";
        public int BaseAtk => 1;
        public int BaseCri => 1;
        public int BaseDef => 1;
        public int BaseHp => 70;
        public int BaseMp => 0;
        public string Trait => "스킬 슬롯 +1  |  기본 장비 3개 보유";
    }

    public class Medium : IJob
    {
        public JobType Type => JobType.영매사;
        public string DisplayName => "저주받은 영매사";
        public string Story => "영매사는 선천적으로 말을 할 수 없지만," +
            "\n그녀의 특별한 능력으로 죽은 이들의 이야기를" +
            "\n세상에 들려줄 수는 있습니다.\n" +
            "\n그녀가 풀어내는 이야기들은 잔혹하지만 어딘가 애잔합니다.";
        public string Description => "저 너머의 존재들과 대화하는 영매사는," +
            "\n      그들의 호의를 받으며, 더 많은 경험치 또한 얻습니다.";
        public int BaseAtk => 4;
        public int BaseCri => 44;
        public int BaseDef => 4;
        public int BaseHp => 100;
        public int BaseMp => 75;
        public string Trait => "얻는 경험치 +20%";
    }

    public partial class Program
    {
        static JobType Prompt()
        {
            var jobs = JobDatas;
            JobType? current = null; // 처음에 아무것도 선택 X

            while (true)
            {
                Console.Clear();
                Console.WriteLine("[직업]을 선택하세요.");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("방법: 숫자(1-5)를 눌러 상세를 보고, Enter로 확정하세요.\n");
                Console.ResetColor();

                foreach (var jobk in jobs)
                {
                    bool selected = jobk.Key == current;
                    Console.Write(selected ? "> " : "");
                    Console.WriteLine($"{(int)jobk.Key}. {jobk.Value.DisplayName}");

                    if (selected)
                    {
                        Console.WriteLine($"    └ {jobk.Value.Description}");
                        Console.WriteLine($"    └ 공격력:{jobk.Value.BaseAtk}  |  " +
                            $"방어력:{jobk.Value.BaseDef}  |  " +
                            $"Hp:{jobk.Value.BaseHp}  |  " +
                            $"Mp:{jobk.Value.BaseMp}");
                        Console.WriteLine($"    └ 특성: {jobk.Value.Trait}");
                        Console.WriteLine();
                    }
                }

                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter && current.HasValue)
                {
                    return current.Value;
                }

                if ((key.Key >= ConsoleKey.D1 && key.Key <= ConsoleKey.D5) ||
                        (key.Key >= ConsoleKey.NumPad1 && key.Key <= ConsoleKey.NumPad5))
                {
                    int num = (key.Key >= ConsoleKey.D1 && key.Key <= ConsoleKey.D5)
                        ? key.Key - ConsoleKey.D0 : key.Key - ConsoleKey.NumPad0;

                    if (Enum.IsDefined(typeof(JobType), num))
                    {
                        current = (JobType)num;
                    }
                }
            }
        }
    }
}

