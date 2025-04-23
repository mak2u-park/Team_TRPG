namespace Sparta_Dungeon_TeamProject
{
    public partial class Program
    {
        // 아이템DB
        static void InitItemDb()
        {
            itemDb = new Item[]
            {    // 아이템 이름 / 타입 / 능력치 / 설명 / 가격
            new Item("수련자의 갑옷", 1, 5,"수련에 도움을 주는 갑옷입니다. ",1000),
            new Item("무쇠갑옷", 1, 9,"무쇠로 만들어져 튼튼한 갑옷입니다. ",2000),
            new Item("스파르타의 갑옷", 1, 15,"스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ",3500),
            new Item("낣은 검", 0, 2,"쉽게 볼 수 있는 낡은 검 입니다. ",600),
            new Item("청동 도끼", 0, 5,"어디선가 사용됐던거 같은 도끼입니다. ",1500),
            new Item("스파르타의 창", 0, 7,"스파르타의 전사들이 사용했다는 전설의 창입니다. ",2500)
            };
        }
    }

    // 아이템 클래스
    public class Item
    {
        public string Name { get; }
        public int Type { get; }
        public int BaseValue { get; } // 기본 능력치
        public int Value { get; set; } // 총 능력치 / 강화시 set 필요
        public int MaxValue { get; set; } = 50; // 강화 최대치 - 테스트용. 조정 가능.
        public string Desc { get; }
        public int Price { get; }

        public string DisplayTypeText
        {
            get
            {
                return Type == 0 ? "공격력" : "방어력";
            }
        }

        public Item(string name, int type, int baseValue, string desc, int price)
        {
            Name = name;
            Type = type;
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