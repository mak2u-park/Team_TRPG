namespace Sparta_Dungeon_TeamProject
{
    public partial class Program
    {
        // ������DB
        static void InitItemDb()
        {
            itemDb = new Item[]//Ÿ�� 0: ����, 1: ��, 2: �Ҹ�ǰ, 3: ��Ÿ
            {    // ������ �̸� / Ÿ�� / �ɷ�ġ / ���� / ����
            new Item("�������� ����", 1, 5,"���ÿ� ������ �ִ� �����Դϴ�. ",1000),
            new Item("���谩��", 1, 9,"����� ������� ưư�� �����Դϴ�. ",2000),
            new Item("���ĸ�Ÿ�� ����", 1, 15,"���ĸ�Ÿ�� ������� ����ߴٴ� ������ �����Դϴ�. ",3500),
            new Item("���� ��", 0, 2,"���� �� �� �ִ� ���� �� �Դϴ�. ",600),
            new Item("û�� ����", 0, 5,"��𼱰� ���ƴ��� ���� �����Դϴ�. ",1500),
            new Item("���ĸ�Ÿ�� â", 0, 7,"���ĸ�Ÿ�� ������� ����ߴٴ� ������ â�Դϴ�. ",2500),
            new Item("ü�� ����", 2, 0,"ü���� ȸ���ϴ� �����Դϴ�. ",100),
            new Item("���� ����", 2, 0,"������ ȸ���ϴ� �����Դϴ�. ",100),
            new Item("�� ����", 3, 0,"���� �������ִ� ���̴�.",0),
            };
        }
    }

    // ������ Ŭ����
    public class Item
    {
        public string Name { get; }
        public int Type { get; }
        public int BaseValue { get; } // �⺻ �ɷ�ġ
        public int Value { get; set; } // �� �ɷ�ġ / ��ȭ�� set �ʿ�
        public int MaxValue { get; set; } = 50; // ��ȭ �ִ�ġ - �׽�Ʈ��. ���� ����.
        public string Desc { get; }
        public int Price { get; }

        public string DisplayTypeText
        {
            get
            {
                switch(Type)
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

        public Item(string name, int type, int baseValue, string desc, int price)
        {
            Name = name;
            Type = type;
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