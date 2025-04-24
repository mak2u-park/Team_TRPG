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
        string Story { get; } // 구체적인 설명
        string Description { get; } // 간단한 설명
        public int BaseAtk { get; }
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
        public string Story => "전장을 떠나 산지 벌써 10년이 지났습니다" +
            "\n전사가 은퇴하며 세상은 어지러워지고, 다시 전장으로 갈 수 밖에 없습니다.";
        public string Description => "잃어버린 전투 감각으로 일정한 데미지를 입히진 않는 트릭형 전사입니다.";
        public int BaseAtk => 5;
        public int BaseDef => 2;
        public int BaseHp => 70;
        public int BaseMp => 100;
        public string Trait => "물리 공격력 증가";
    }

    public class Mage : IJob
    {
        public JobType Type => JobType.마법사;
        public string DisplayName => "마법사";
        public string Story => "떠돌이 마법사로서 세계를 돌아다닌다.";
        public string Description => "MP 중심의 마법 공격,저주 확률 증가";
        public int BaseAtk => 8;
        public int BaseDef => 2;
        public int BaseHp => 50;
        public int BaseMp => 0;
        public string Trait => "마법 공격력 증가";
    }

    public class Scientist : IJob
    {
        public JobType Type => JobType.과학자;
        public string DisplayName => "과학자";
        public string Story => "마을에서 추방된 이단 과학자.\r\n금기된 재료들을 지니고 있다.";
        public string Description => "금기된 독성 중심의 스킬을 사용해 상대적으로 마나소모가 잦고, 체력 소모가 적음.";
        public int BaseAtk => 2;
        public int BaseDef => 5;
        public int BaseHp => 50;
        public int BaseMp => 150;
        public string Trait => "기술 사용 능력 증가";
    }

    public class smith : IJob
    {
        public JobType Type => JobType.대장장이;
        public string DisplayName => "대장장이";
        public string Story => "제자의 비극 이후 제련을 멈춘 대장장이. 무기는 남았지만 상처도 남았다.";
        public string Description => "약한 능력치를 극복하는 강력한 장비 기반의 직업";
        public int BaseAtk => 1;
        public int BaseDef => 1;
        public int BaseHp => 70;
        public int BaseMp => 0;
        public string Trait => "스킬 슬롯 +1  |  기본 장비 3개 보유";
    }

    public class Medium : IJob
    {
        public JobType Type => JobType.영매사;
        public string DisplayName => "침묵의 영매사";
        public string Story => "목소리를 잃어 세상과 단절된 인생을 살아가고 있던 영매입니다." +
            "그 대신에 생긴 고양이와의 탁월한 교감 능력으로 세상을 읽어냅니다.";
        public string Description => "고양이와 조우 시, 항상 이로운 효과를 얻을 수 있습니다." +
            "영매이기에 다른 이의 스킬을 흡수해 사용할 수 있습니다.";
        public int BaseAtk => 5;
        public int BaseDef => 4;
        public int BaseHp => 100;
        public int BaseMp => 75;
        public string Trait => "정신력 증가";
    }

    public partial class Program
    {
        static JobType Prompt()
        {
            var jobs = JobDatas;
            JobType current = jobs.Keys.First();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("직업을 선택하세요.");
                Console.WriteLine("숫자(1-5)를 눌러 상세를 보고, Enter로 확정하세요.\n");

                foreach (var jobk in jobs)
                {
                    bool selected = jobk.Key == current;
                    Console.Write(selected ? "> " : "");
                    Console.WriteLine($"{(int)jobk.Key}. {jobk.Value.DisplayName}");

                    if (selected)
                    {
                        Console.WriteLine($"    └{jobk.Value.Story}");
                        Console.WriteLine($"    └{jobk.Value.Description}");
                        Console.WriteLine($"    └ Atk:{jobk.Value.BaseAtk}  |  " +
                            $"Def:{jobk.Value.BaseDef}  |  " +
                            $"Hp:{jobk.Value.BaseHp}  |  " +
                            $"Mp:{jobk.Value.BaseMp}");
                        Console.WriteLine($"    └ Trait: {jobk.Value.Trait}");
                    }
                }

                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    return current;
                }

                if (key.Key >= ConsoleKey.D1 && key.Key <= ConsoleKey.D5)
                {
                    int num = key.Key - ConsoleKey.D0;
                    current = (JobType)num;
                }
            }
        }
    }
}

