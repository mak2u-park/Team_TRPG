namespace Sparta_Dungeon_TeamProject
{
    public class Item
    {
        public string Name { get; }
        public int Type { get; }

        // 두가지 이상 능력치가 있는 아이템은 기본 능력치로 설정.
        public int AtkBonus { get; }
        public int DefBonus { get; }
        public int HpBonus { get; }
        public int MpBonus { get; }

        //강화시 필요한 값
        public int TotalValue { get; set; } // 총 능력치 / 강화시 set 필요
        public int MaxValue { get; set; } = 50; // 강화 최대치 - 테스트용. 조정 가능.

        // 아이템 설명, 가격
        public string Desc { get; }
        public int Price { get; }

        public Item(string name, int type, int atkBonus, int defBonus, int hpBonus, int mpBonus, int totalValue, string desc, int price)
        {
            Name = name;
            Type = type;
            AtkBonus = atkBonus;
            DefBonus = defBonus;
            HpBonus = hpBonus;
            MpBonus = mpBonus;
            TotalValue = totalValue;
            Desc = desc;
            Price = price;
        }

        public string DisplayTypeText()
        {
            var parts = new List<string>();
            if (AtkBonus != 0)
            {
                parts.Add($"공격력{(AtkBonus > 0 ? "+" : "")}{AtkBonus}");
            }
            if (DefBonus != 0)
            {
                parts.Add($"방어력{(DefBonus > 0 ? "+" : "")}{DefBonus}");
            }
            if (HpBonus != 0)
            {
                parts.Add($"최대HP{(HpBonus > 0 ? "+" : "")}{HpBonus}");
            }
            if (MpBonus != 0)
            {
                parts.Add($"최대MP{(MpBonus > 0 ? "+" : "")}{MpBonus}");
            }
            return parts.Count > 0 ? string.Join("  |  ", parts) : "기타";
        }

        public string ItemInfoText() // 장착/사용 여부 무관하게 줄 수 있음
        {
            string enhanceText = TotalValue == MaxValue ? " (최대치)" : "";
            string atkInfo = AtkBonus > 0 ? $"공격력 +{AtkBonus}" : "";
            string defInfo = DefBonus > 0 ? $"방어력 +{DefBonus}" : "";
            string hpInfo = HpBonus > 0 ? $"개당 HP +{HpBonus}" : "";
            string mpInfo = MpBonus > 0 ? $"개당 MP +{MpBonus}" : "";

            return $"{Name}  |  {DisplayTypeText} +{TotalValue}{enhanceText}  |  {Desc}";
        }

        public string ItemEnhanceText() // 장착여부 포함 / 강화 O 아이템
        {
            // 토탈Value - AtkBonus 혹은 DefBonus에 따라 다르게 표시
            

            //int enhanceValue = TotalValue - BaseValue;
            //string enhanceText = TotalValue >= MaxValue ? " (최대치)" : "";
            //string valueText = enhanceValue > 0 ? $"+({enhanceValue})" : "";
            //string typeText = Type == 0 ? "공격력" : "방어력";

            return $"{Name}  |  수정필요  |  {Desc}";
        }

        // 회복 아이템 사용 로직
        public void UseItem(Player player)
        {
            if (Type != 2)
            {
                return;
            }
            // 예) 회복 포션이라면 MaxHpBonus 만큼 HP 회복
            player.Heal(HpBonus);
            player.Mp = Math.Min(player.Mp + MpBonus, player.MaxMp);
            player.RemoveFromInventory(this);
        }
    }
}