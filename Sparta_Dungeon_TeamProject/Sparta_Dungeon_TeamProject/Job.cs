using System;

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
        JobType Type { get; } // 타입
        string DisplayName { get; } // 출력될 직업명
        string Story { get; } // 서사 (선택 후 출력)
        string Description { get; } // 간단설명 (선택 전 출력)

        // 기본 스탯
        int ExpToLevelUp { get; }
        int Atk { get; } 
        int Acc { get; }
        int Cri { get; } // 치명타 확률
        int CriDmg { get; }
        int Def { get; }
        int MaxHp { get; } // 최대 체력

        //int MaxMp { get; } // 최대 마나
        int DefaultGold { get; } // 초기 보상 골드
        string Trait { get; } // 직업 특성

       // List<string> InitialSkills { get; } // 초기 보상 스킬
        List<Item> InitialItems { get; } // 초기 보상 아이템
    };

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
        public int ExpToLevelUp => 100;
        public int Atk => 20;
        public int Acc => 75;
        public int Cri => 40;
        public int CriDmg => 200;
        public int Def => 8;
        public int MaxHp => 150;

        //public int MaxMp => 30;
        public int DefaultGold => 10000;      // 구현: 직업별 시작 골드
        public string Trait => "높은 치명타 확률, 낮은 명중률.";

        //public List<string> InitialSkills => new List<string>() { $"스킬1", $"스킬2" };
        public List<Item> InitialItems => new List<Item>(Item.GifttemDb[JobType.전사]);
    }

    public class Mage : IJob
    {
        public JobType Type => JobType.마법사;
        public string DisplayName => "뜨내기 마법사";
        public string Story => "재능이 미천해 마법사의 탑에서 쫒겨난 후," +
            "\n무시당하기 싫어 허풍만 늘었습니다.\n" +
            "\n뛰어난 사람들에게 질투를 느끼며," +
            "\n던전에서 발견되는 마법서를 얻기 위해" +
            "\n가벼운 마음을 갖고 전장으로 향합니다.";
        public string Description => "떠돌이 마법사로서 세계를 돌아다니며," +
            "\n       스킬 위주의 전투를 펼치는 마법 중심의 직업입니다.";
        public int ExpToLevelUp => 100;
        public int Atk => 10;
        public int Acc => 85;
        public int Cri => 10;
        public int CriDmg => 150;
        public int Def => 3;
        public int MaxHp => 80;

        //public int MaxMp => 120;
        public int DefaultGold => 10000;      // 구현: 직업별 시작 골드
        public string Trait => "방어력 무시 공격.\n보스에게 가하는 피해 감소.";

        // public List<string> InitialSkills => new List<string>() { $"스킬1", $"스킬2" };
        public List<Item> InitialItems => new List<Item>(Item.GifttemDb[JobType.마법사]);
    }

    public class Scientist : IJob
    {
        public JobType Type => JobType.과학자;
        public string DisplayName => "불법 과학자";
        public string Story => "마을에서 추방당한 이단 과학자로서" +
            "\n금기된 재료들을 가지고 위험한 연구를 계속합니다.\n" +
            "\n자신만의 독특한 기술로 세상에 맞서려 합니다.\n";
        public string Description => "금기된 독성 중심의 스킬을 사용하며," +
            "\n       상대적으로 마나 소모가 잦고 체력 소모가 적습니다.";
        public int ExpToLevelUp => 100;
        public int Atk => 10;
        public int Acc => 100;
        public int Cri => 15;
        public int CriDmg => 140;
        public int Def => 5;
        public int MaxHp => 100;

        //public int MaxMp => 100;
        public int DefaultGold => 15000;      // 구현: 직업별 시작 골드
        public string Trait => "전투 중 체력 재생, 매우 높은 명중률.";

        // public List<string> InitialSkills => new List<string>() { $"스킬1", $"스킬2" };
        public List<Item> InitialItems => new List<Item>(Item.GifttemDb[JobType.과학자]);
    }

    public class Smith : IJob
    {
        public JobType Type => JobType.대장장이;
        public string DisplayName => "노쇄한 대장장이";
        public string Story => "주변 사람들로부터 죽은 줄 알았던 제자를" +
            "\n던전에서 봤다는 소문을 듣게 되어 제자를 찾아 나섭니다.\n" +
            "\n제자의 비극 이후 제련을 멈췄지만" +
            "\n그의 무기에는 상처와 함께 이야기가 남아있습니다.\n";
        public string Description => "약한 능력치를 극복하는 강력한 장비 기반의 직업입니다.";
        public int ExpToLevelUp => 130;
        public int Atk => 14;
        public int Acc => 85;
        public int Cri => 10;
        public int CriDmg => 160;
        public int Def => 10;
        public int MaxHp => 130;

        //public int MaxMp => 40;
        public int DefaultGold => 10000;      // 구현: 직업별 시작 골드
        public string Trait => "전용 장비 지급, .\n";

        // public List<string> InitialSkills => new List<string>() { $"스킬1", $"스킬2" };
        public List<Item> InitialItems => new List<Item>(Item.GifttemDb[JobType.대장장이]);
    }

    public class Medium : IJob
    {
        public JobType Type => JobType.영매사;
        public string DisplayName => "저주받은 영매사";
        public string Story => "영매사는 선천적으로 말을 할 수 없지만," +
            "\n그녀의 특별한 능력으로 죽은 이들의 이야기를" +
            "\n세상에 들려줄 수는 있습니다.\n" +
            "\n그녀가 풀어내는 이야기들은 잔혹하지만 어딘가 애잔합니다.";
        public string Description => "저 너머의 존재들과 소통하는 영매사는," +
            "\n      그들의 호의를 받으며, 더 많은 경험치 또한 얻습니다.";
        public int ExpToLevelUp => 105;
        public int Atk => 9;
        public int Acc => 90;
        public int Cri => 25;
        public int CriDmg => 125;
        public int Def => 4;
        public int MaxHp => 90;

        //public int MaxMp => 110;
        public int DefaultGold => 10000;      // 구현: 직업별 시작 골드
        public string Trait => "추가 경험치 획득, 영혼들이 당신을 돕습니다.\n 회복 효율이 감소합니다.";

        // public List<string> InitialSkills => new List<string>() { $"스킬1", $"스킬2" };
        public List<Item> InitialItems => new List<Item>(Item.GifttemDb[JobType.영매사]);
    }
}
