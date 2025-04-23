using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sparta_Dungeon_TeamProject
{
    // �÷��̾� Ŭ����
    public class Player
    {
        public int Level { get; private set; }
        public int Exp { get; set; }
        public int MaxExp { get; private set; }
        public string Name { get; private set; }
        public JobType Job { get; private set; }
        public int Atk { get; private set; }
        public int Def { get; private set; }
        public int Hp { get; private set; }
        public int MaxHp { get; private set; }
        public int Mp { get; set; }
        public int MaxMp { get; private set; }
        public int Gold { get; set; }

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

        public Player(int level, int exp, int maxExp, string name, JobType job, int atk, int def, int hp, int maxHp, int mp, int maxMp, int gold)
        {
            Level = level;
            Exp = exp;
            MaxExp = maxExp;
            Name = name;
            Job = job;
            Atk = atk;
            Def = def;
            Hp = hp;
            MaxHp = maxHp;
            Mp = mp;
            MaxMp = maxMp;
            Gold = gold;

        }

        // 1. ���º��� # Program.cs
        public void DisplayPlayerInfo()
        {
            Console.WriteLine($"Lv. {Level:D2} {{ {Exp}/{MaxExp} }}");
            Console.WriteLine($"{Name} {{ {Job} }}");
            Console.WriteLine(ExtraAtk == 0 ? $"���ݷ� : {Atk}" : $"���ݷ� : {Atk + ExtraAtk} (+{ExtraAtk})");
            Console.WriteLine(ExtraDef == 0 ? $"���� : {Def}" : $"���� : {Def + ExtraDef} (+{ExtraDef})");
            Console.WriteLine($"ü�� : {Hp}/{MaxHp}");
            Console.WriteLine($"���� : {Mp}/{MaxMp}");
            Console.WriteLine($"Gold : {Gold} G");
        }

        // ���� DB # SetData()
        public enum JobType
        {
            ���� = 1,
            ������,
            �ü�,
            ����,
            ������,
        }

        public class JobData
        {
            public int BaseAtk { get; }
            public int BaseDef { get; }
            public int BaseMaxHp { get; }
            public int BaseMaxMp { get; }

            public JobData(int atk, int def, int maxHp, int maxMp)
            {
                BaseAtk = atk;
                BaseDef = def;
                BaseMaxHp = maxHp;
                BaseMaxMp = maxMp;
            }
        }
        public static class JobDB
        {
            public static Dictionary<JobType, JobData> Jobs = new Dictionary<JobType, JobData>
            {   // ������ / ���ݷ� / ���� / �ִ�ü�� / �ִ븶��
                { JobType.����, new JobData(7, 8, 150, 50 ) },
                { JobType.������, new JobData(13, 2, 50, 150) },
                { JobType.�ü�, new JobData(8, 7, 100, 100) },
                { JobType.����, new JobData(10, 5, 80, 120) },
                { JobType.������, new JobData(5, 4, 125, 75) }
            };
        }


        // ����ġ ȹ��
        public void GainExp()
        {
            while (Exp >= MaxExp) // ������
            {
                Exp -= MaxExp;
                MaxExp += 10;
                Level++;

                if (Job == JobType.���� || Job == JobType.�ü� || Job == JobType.����)
                {
                    MaxHp += 10;
                    MaxMp += 5;
                    Hp += MaxHp;
                    Mp += MaxMp;
                }
                else
                {
                    MaxHp += 5;
                    MaxMp += 10;
                    Hp += MaxHp;
                    Mp += MaxMp;
                }
            }
        }

        // �κ��丮 �����۸�� # Inventory.cs
        public void InventoryItemList(bool showIdx)
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                Item targetItem = Inventory[i];

                // �κ��丮 ������ ���
                string displayIdx = showIdx ? $"{i + 1} " : "";
                string displayEquipped = IsEquipped(targetItem) ? "[E]" : "";

                // ������ ��ȭ ���ο� ���� ���
                string enhanceText = targetItem.Value == targetItem.MaxValue ? " (�ִ�ġ)" : "";
                string statText = $"+({targetItem.Value})"; // ��ȭ ��ġ
                string typeText = targetItem.Type == 0 ? "���ݷ�" : "����";

                // ������ ���� ���: �̸�, Ÿ��,
                Console.WriteLine($"- {displayIdx}{displayEquipped}{targetItem.ItemEnhanceText()}");
            }
        }

        // ��� ���뿩�� # Inventory.cs
        public void EquipItem(Item item)
        {
            if (IsEquipped(item))
            {
                EquipList.Remove(item);
                if (item.Type == 0) ExtraAtk -= item.Value;
                else ExtraDef -= item.Value;
            }
            else
            {
                EquipList.Add(item);
                if (item.Type == 0) ExtraAtk += item.Value;
                else ExtraDef += item.Value;
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
            MaxHp += Hp;
        }

        // ������ ��ȭ # Inventory.cs ���� ȣ���� ���� �и�
        public int GetUpgradeCost(Item item)
        {
            return item.Value < 20 ? 100 : 200;
        }
        public int GetUpgradeValue(Item item)
        {
            return item.Value < 20 ? 5 : 10;
        }

        public bool UpgradeItem(Item item)
        {
            int cost = GetUpgradeCost(item);
            int valueUp = GetUpgradeValue(item);

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

        //�ǰ� ���ط� ���
        public void Damage(int amount)
        {
            int damage = amount - Def;

            damage = damage < 0 ? 1 : damage;

            Hp -= damage;

            Console.WriteLine($"{damage}�� �������� �޾ҽ��ϴ�!");

            if (Hp < 0)
            {
                Hp = 0;
                Console.WriteLine("������ �����մϴ�.");
                Thread.Sleep(1000);
                Environment.Exit(0);
            }
        }
    }
}