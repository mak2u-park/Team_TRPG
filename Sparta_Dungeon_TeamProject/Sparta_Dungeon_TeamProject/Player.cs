namespace Sparta_Dungeon_TeamProject
{
    // �÷��̾� Ŭ����
    public class Player
    {
        public int Level { get; }
        public string Name { get; }
        public JobType Job { get; }
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; private set; }
        public int Gold { get; private set; }

        public int ExtraAtk { get; private set; }
        public int ExtraDef { get; private set; }

        private List<Item> Inventory = new List<Item>();
        private List<Item> EquipList = new List<Item>();

        public int InventoryCount
        {
            get
            {
                return Inventory.Count;
            }
        }

        public Player(int level, string name, JobType job, int atk, int def, int hp, int gold)
        {
            Level = level;
            Name = name;
            Job = job;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
        }

        // 1. ���º��� # Program.cs
        public void DisplayPlayerInfo()
        {
            Console.WriteLine($"Lv. {Level:D2}");
            Console.WriteLine($"{Name} {{ {Job} }}");
            Console.WriteLine(ExtraAtk == 0 ? $"���ݷ� : {Atk}" : $"���ݷ� : {Atk + ExtraAtk} (+{ExtraAtk})");
            Console.WriteLine(ExtraDef == 0 ? $"���� : {Def}" : $"���� : {Def + ExtraDef} (+{ExtraDef})");
            Console.WriteLine($"ü�� : {Hp}");
            Console.WriteLine($"Gold : {Gold} G");
        }

        // �κ��丮 # Inventory.cs
        public void DisplayInventory(bool showIdx)
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                Item targetItem = Inventory[i];

                string displayIdx = showIdx ? $"{i + 1} " : "";
                string displayEquipped = IsEquipped(targetItem) ? "[E]" : "";
                Console.WriteLine($"- {displayIdx}{displayEquipped} {targetItem.ItemInfoText()}");
            }
        }

        // ��� ���뿩�� # Inventory.cs
        public void EquipItem(Item item)
        {
            if (IsEquipped(item))
            {
                EquipList.Remove(item);
                if (item.Type == 0)
                    ExtraAtk -= item.Value;
                else
                    ExtraDef -= item.Value;
            }
            else
            {
                EquipList.Add(item);
                if (item.Type == 0)
                    ExtraAtk += item.Value;
                else
                    ExtraDef += item.Value;
            }
        }

        public bool IsEquipped(Item item)
        {
            return EquipList.Contains(item);
        }

        // �κ��丮 ������ ��� ��ȯ(��ȸ) # Inventory.cs
        public List<Item> GetInventoryItems()
        {
            return Inventory;
        }

        // ���� �� �Ǹ� ������ ���� # Shop.cs
        public void BuyItem(Item item)
        {
            Gold -= item.Price;
            Inventory.Add(item);
        }

        public bool HasItem(Item item)
        {
            return Inventory.Contains(item);
        }

        public void SellItem(Item item)
        {
            Gold += (int)(item.Price * 0.85);
            Inventory.Remove(item);
            EquipList.Remove(item);
            if (item.Type == 0) ExtraAtk -= item.Value;
            else ExtraDef -= item.Value;
        }

        // �޽ı�� ���� # Program.cs
        public void Rest()
        {
            Gold -= 500;
            Hp = 100;
        }

        // ������ ��ȭ - ���� �߻� bool������ ���� �õ� # Inventory.cs
        public bool UpgradeItem(Item item)
        {
            int cost = item.Value < 20 ? 100 : 200; // 20�̸��� true => 100 G / 20�̸��� false => 200 G ����
            int valueUp = item.Value < 20 ? 5 : 10; // 20�̸� 5 ����, 20�̻� 10 ����

            if (item.Value >= item.MaxValue) // �ִ�ġ �̻�
            {
                return false;
            }

            if (Gold < cost) // ��� ����
            {
                return false;
            }

            Gold -= cost; // ��� ����
            item.Value += valueUp; // ������ �ɷ�ġ ����

            //���� ���� �ݿ�
            if (IsEquipped(item))
            {
                if (item.Type == 0) ExtraAtk += valueUp;
                else ExtraDef += valueUp;
            }
            return true;
        }
    }
}