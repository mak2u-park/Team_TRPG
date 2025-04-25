namespace Sparta_Dungeon_TeamProject
{
    public partial class Program
    {
        // ������DB
        static void InitItemDb()
        {
            itemDb = new Item[]//Ÿ�� 0: ����, 1: ��, 2: �Ҹ�ǰ, 3: ��Ÿ
            {
                // �̸�, Ÿ��, �⺻atk, �⺻def, �⺻ hp, ����, ����
                new Item("ö��", 0, 5, 0, 5, "�⺻���� ���Դϴ�.", 100),
                new Item("ö����", 1, 0, 5, 5, "�⺻���� �����Դϴ�.", 100),
                new Item("ȸ����", 2, 0, 0, 10, "ü���� ȸ���ϴ� ���Դϴ�.", 50),
                new Item("����", 3, 0, 0, 0, "�����Դϴ�.", 0)

            };
        }
    }

    // ������ Ŭ����
    public class Item
    {
        public string Name { get; }
        public int Type { get; }
        public int BaseAtk { get; } = 0; // �⺻ ���ݷ�
        public int BaseDef { get; } = 0; // �⺻ ���ݷ�
        public int BaseHp { get; } = 0; // �⺻ ü��
        public int BaseMp { get; } = 0; // �⺻ ����

        public int BaseValue { get; } // �⺻ �ɷ�ġ
        public int Value { get; set; } // �� �ɷ�ġ / ��ȭ�� set �ʿ�
        public int MaxValue { get; set; } = 50; // ��ȭ �ִ�ġ - �׽�Ʈ��. ���� ����.
        public string Desc { get; }
        public int Price { get; }

        public string DisplayTypeText
        {
            get
            {
                switch (Type)
                {
                    case 0:
                        return "���ݷ�";
                    case 1:
                        return "����";
                    case 2:
                        return "ȸ����";
                    case 3:
                        return "��Ÿ";
                    default:
                        return "�� �� ����";
                }
                //return Type == 0 ? "���ݷ�" : "����";
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

        public string ItemInfoText() // �������� ���� / shop ������
        {
            string enhanceText = Value == MaxValue ? " (�ִ�ġ)" : "";

            return $"{Name}  |  {DisplayTypeText} +{BaseValue}  |  {Desc}";
        }

        public string ItemEnhanceText() // �������� ���� / ��ȭ O ������
        {
            int enhanceValue = Value - BaseValue;
            string enhanceText = Value >= MaxValue ? " (�ִ�ġ)" : "";
            string valueText = enhanceValue > 0 ? $"+({enhanceValue})" : "";
            string typeText = Type == 0 ? "���ݷ�" : "����";

            return $"{Name}  |  {typeText} {BaseValue} {valueText}{enhanceText}  |  {Desc}";
        }
    }
}