namespace Sparta_Dungeon_TeamProject
{
    public class Item
    {
        public string Name { get; }
        public int Type { get; }

        // �ΰ��� �̻� �ɷ�ġ�� �ִ� �������� �⺻ �ɷ�ġ�� ����.
        public int AtkBonus { get; }
        public int DefBonus { get; }
        public int HpBonus { get; }
        public int MpBonus { get; }

        //��ȭ�� �ʿ��� ��
        public int TotalValue { get; set; } // �� �ɷ�ġ / ��ȭ�� set �ʿ�
        public int MaxValue { get; set; } = 50; // ��ȭ �ִ�ġ - �׽�Ʈ��. ���� ����.

        // ������ ����, ����
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
                parts.Add($"���ݷ�{(AtkBonus > 0 ? "+" : "")}{AtkBonus}");
            }
            if (DefBonus != 0)
            {
                parts.Add($"����{(DefBonus > 0 ? "+" : "")}{DefBonus}");
            }
            if (HpBonus != 0)
            {
                parts.Add($"�ִ�HP{(HpBonus > 0 ? "+" : "")}{HpBonus}");
            }
            if (MpBonus != 0)
            {
                parts.Add($"�ִ�MP{(MpBonus > 0 ? "+" : "")}{MpBonus}");
            }
            return parts.Count > 0 ? string.Join("  |  ", parts) : "��Ÿ";
        }

        public string ItemInfoText() // ����/��� ���� �����ϰ� �� �� ����
        {
            string enhanceText = TotalValue == MaxValue ? " (�ִ�ġ)" : "";
            string atkInfo = AtkBonus > 0 ? $"���ݷ� +{AtkBonus}" : "";
            string defInfo = DefBonus > 0 ? $"���� +{DefBonus}" : "";
            string hpInfo = HpBonus > 0 ? $"���� HP +{HpBonus}" : "";
            string mpInfo = MpBonus > 0 ? $"���� MP +{MpBonus}" : "";

            return $"{Name}  |  {DisplayTypeText} +{TotalValue}{enhanceText}  |  {Desc}";
        }

        public string ItemEnhanceText() // �������� ���� / ��ȭ O ������
        {
            // ��ŻValue - AtkBonus Ȥ�� DefBonus�� ���� �ٸ��� ǥ��
            

            //int enhanceValue = TotalValue - BaseValue;
            //string enhanceText = TotalValue >= MaxValue ? " (�ִ�ġ)" : "";
            //string valueText = enhanceValue > 0 ? $"+({enhanceValue})" : "";
            //string typeText = Type == 0 ? "���ݷ�" : "����";

            return $"{Name}  |  �����ʿ�  |  {Desc}";
        }

        // ȸ�� ������ ��� ����
        public void UseItem(Player player)
        {
            if (Type != 2)
            {
                return;
            }
            // ��) ȸ�� �����̶�� MaxHpBonus ��ŭ HP ȸ��
            player.Heal(HpBonus);
            player.Mp = Math.Min(player.Mp + MpBonus, player.MaxMp);
            player.RemoveFromInventory(this);
        }
    }
}