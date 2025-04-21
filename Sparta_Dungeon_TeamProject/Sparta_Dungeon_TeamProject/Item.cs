namespace SpartaDungeon;

class Item
{
    public string Name { get; }
    public int Type { get; }
    public int Value { get; }
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