namespace Sparta_Dungeon_TeamProject
{
    public partial class Program
    {
        // ������DB
        static void InitItemDb()
        {
            itemDb = new Item[]
            {    // ������ �̸� / Ÿ�� / �ɷ�ġ / ���� / ����
            new Item("�������� ����", 1, 5,"���ÿ� ������ �ִ� �����Դϴ�. ",1000),
            new Item("���谩��", 1, 9,"����� ������� ưư�� �����Դϴ�. ",2000),
            new Item("���ĸ�Ÿ�� ����", 1, 15,"���ĸ�Ÿ�� ������� ����ߴٴ� ������ �����Դϴ�. ",3500),
            new Item("���� ��", 0, 2,"���� �� �� �ִ� ���� �� �Դϴ�. ",600),
            new Item("û�� ����", 0, 5,"��𼱰� ���ƴ��� ���� �����Դϴ�. ",1500),
            new Item("���ĸ�Ÿ�� â", 0, 7,"���ĸ�Ÿ�� ������� ����ߴٴ� ������ â�Դϴ�. ",2500)
            };
        }
    }

    // ������ Ŭ����
    public class Item
    {
        public string Name { get; }
        public int Type { get; }
        public int Value { get; set; }
        public string Desc { get; }
        public int Price { get; }

        public string DisplayTypeText
        {
            get
            {
                return Type == 0 ? "���ݷ�" : "����";
            }
        }

        public Item(string name, int type, int value, string desc, int price)
        {
            Name = name;
            Type = type;
            Value = value;
            Desc = desc;
            Price = price;
        }

        public string ItemInfoText()
        {
            return $"{Name}  |  {DisplayTypeText} +{Value}  |  {Desc}";
        }
    }
}