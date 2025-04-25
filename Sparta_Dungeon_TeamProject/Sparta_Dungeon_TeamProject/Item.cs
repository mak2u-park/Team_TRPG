namespace Sparta_Dungeon_TeamProject
{
    public partial class Program
    {
        // 아이템DB
        static void InitItemDb()
        {
            itemDb = new Item[]//타입 0: 무기, 1: 방어구, 2: 소모품, 3: 기타
            {
                // 이름, 타입, 기본atk, 기본def, 기본 hp, 설명, 가격
                new Item("철검", 0, 5, 0, 5, "기본적인 검입니다.", 100),
                new Item("철갑옷", 1, 0, 5, 5, "기본적인 갑옷입니다.", 100),
                new Item("회복약", 2, 0, 0, 10, "체력을 회복하는 약입니다.", 50),
                new Item("상자", 3, 0, 0, 0, "상자입니다.", 0)

            };
        }
    }

    // 아이템 클래스
    public class Item
    {
        public string Name { get; }
        public int Type { get; }
        public int BaseAtk { get; } = 0; // 기본 공격력
        public int BaseDef { get; } = 0; // 기본 공격력
        public int BaseHp { get; } = 0; // 기본 체력
        public int BaseMp { get; } = 0; // 기본 마나

        public int BaseValue { get; } // 기본 능력치
        public int Value { get; set; } // 총 능력치 / 강화시 set 필요
        public int MaxValue { get; set; } = 50; // 강화 최대치 - 테스트용. 조정 가능.
        public string Desc { get; }
        public int Price { get; }

        public string DisplayTypeText
        {
            get
            {
                switch (Type)
                {
                    case 0:
                        return "공격력";
                    case 1:
                        return "방어력";
                    case 2:
                        return "회복력";
                    case 3:
                        return "기타";
                    default:
                        return "알 수 없음";
                }
                //return Type == 0 ? "공격력" : "방어력";
            }
        }

        public Item(string name, int type, int baseAtk, int baseDef, int baseHp, int baseMp, int baseValue, string desc, int price)
        {
            Name = name;
            Type = type;
            BaseAtk = baseAtk;
            BaseDef = baseDef;
            BaseHp = baseHp;
            BaseAtk = baseMp;
            BaseValue = baseValue;
            Value = baseValue;
            Desc = desc;
            Price = price;
        }

        public string ItemInfoText() // 장착여부 무관 / shop 아이템
        {
            string enhanceText = Value == MaxValue ? " (최대치)" : "";

            return $"{Name}  |  {DisplayTypeText} +{BaseValue}  |  {Desc}";
        }

        public string ItemEnhanceText() // 장착여부 포함 / 강화 O 아이템
        {
            int enhanceValue = Value - BaseValue;
            string enhanceText = Value >= MaxValue ? " (최대치)" : "";
            string valueText = enhanceValue > 0 ? $"+({enhanceValue})" : "";
            string typeText = Type == 0 ? "공격력" : "방어력";

            return $"{Name}  |  {typeText} {BaseValue} {valueText}{enhanceText}  |  {Desc}";
        }
    }
}