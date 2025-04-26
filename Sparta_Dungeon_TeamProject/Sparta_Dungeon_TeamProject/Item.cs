using System;
using System.Collections.Generic;
using System.Linq;

namespace Sparta_Dungeon_TeamProject
{
    public class Item
    {
        public Dictionary<JobType, List<Item>> GiftItemDb { get; private set; }
        public List<Item> EventItemDb { get; private set; }
        public List<Item> ShopItemDb { get; private set; }

        public ItemManager()
        {
            GiftItemDb = InitializeGiftItems();
            EventItemDb = InitializeEventItems();
            ShopItemDb = InitializeShopItems();
        }

        private Dictionaty<JobType, List<Item>> InitializeGiftItems()
        {
            return new Dictionary<JobType, List<Item>>
            {
                { JobType.����, new List<Item> { new Item("pp", 0, 0, 0, 0, 0, "1", 0) } },
                { JobType.������, new List<Item> { new Item("mm", 0, 0, 0, 0, 00, "3", 0) } },
                { JobType.������, new List<Item> { new Item("zz", 0, 0, 0, 0, 15, "5", 0) } },
                { JobType.��������, new List<Item> { new Item("dd", 0, 5, 0, 0, 0, "7", 0),
                                                     new Item("ss", 1, 0, 3, 5, 0, "8", 0),
                                                     new Item("ee", 1, 0, 3, 5, 0, "9", 0) } },
                { JobType.���Ż�, new List<Item> { new Item("bb", 3, 0, 0, 0, 0,"9", 0) } }
            };
        }

        public string Name { get; }
        public int Type { get; }

        // �ΰ��� �̻� �ɷ�ġ�� �ִ� �������� �⺻ �ɷ�ġ�� ����.
        public int AtkBonus { get; }
        public int DefBonus { get; }
        public int HpBonus { get; }
        public int MpBonus { get; }

        // ��ȭ �ý���
        public int TotalValue { get; set; } = 0; // ��ȭ ��ġ
        public int MaxValue { get; set; } = 50; // �ִ� ��ȭ ��ġ

        // ������ ����, ����
        public string Desc { get; }
        public int Price { get; }

        public Item(string name, int type, int atkBonus, int defBonus,
            int hpBonus, int mpBonus, string desc, int price)
        {
            Name = name;
            Type = type;
            AtkBonus = atkBonus;
            DefBonus = defBonus;
            HpBonus = hpBonus;
            MpBonus = mpBonus;
            Desc = desc;
            Price = price;
        }

        // 1. ������ �ʱ� ���� ������
        public static readonly Dictionary<JobType, Item[]> GifttemDb = new()
        {           // �̸�, type, atk, def, hp, mp, ����, price
            { JobType.����, new[] {
                new Item("pp", 0, 0, 0, 0, 0, "1", 0),
            }},
            { JobType.������, new[] {
                new Item("mm", 0, 0, 0, 0, 00, "3", 0),
            }},
            { JobType.������, new[] {
                new Item("zz", 0, 0, 0, 0, 15, "5", 0),
            }},
            { JobType.��������, new[] {
                new Item("dd", 0, 5, 0, 0, 0, "7", 0),
                new Item("ss", 1, 0, 3, 5, 0, "8", 0),
                new Item("ee", 1, 0, 3, 5, 0, "9", 0),
            }},
            { JobType.���Ż�, new[] {
                new Item("bb", 3, 0, 0, 0, 0, "9", 0),
            }}
        };

        // 2. �̺�Ʈ/���� ȹ��� ������ <Type == 5>
        public static readonly Item[] EventItemDb = new Item[]
        {           // �̸�, type, atk, def, hp, mp, ����, price     // index
            new Item("�η��� ��", 5, 5, 0, 0, 0, "������ ������ ���̴� �η��� �� �Դϴ�.", 0), // 0
            new Item("�� ������ ��", 5, 20, 0, 0, 0, "�� ������ ��", 0),  // 1
            new Item("���ֹ��� ��", 5, 15, 0, 0, 0, "��г��� �� �Դϴ�.", 0),
            new Item("�����", 5, 0, 0, 2, 0, "���� �̽��غ��̴� ������̴�.", 0),
        };

        // 3. ������ ������ <Type: 0=����, 1=��, 2=ȸ��������, 3=��Ÿ>
        public static Item[] InitializeItemDb() => new Item[]
        {           // �̸�, type, atk, def, hp, mp, ����, price
            new Item("ö��",   0, 5, 0, 5, 0, "�⺻���� ö���Դϴ�.", 100),
            new Item("ö����", 1, 0, 5, 0, 0, "�⺻���� ö�� �����Դϴ�.", 120),
            new Item("HP ����",2, 0, 0, 20, 0, "�ִ� ü���� 20 ȸ���մϴ�.", 50),
            new Item("MP ����",2, 0, 0, 0, 20, "�ִ� ������ 20 ȸ���մϴ�.", 50),
        };

        // Ÿ�Ժ� �׷�ȭ
        public static readonly Dictionary<int, List<Item>> ItemTypeD
            = InitializeItemDb().GroupBy(item => item.Type).ToDictionary(g => g.Key, g => g.ToList());


        //Ÿ�Ժ� ������ ��������
        public static Item[] GetItemTypeD(int type)
        {
            return ItemTypeD.TryGetValue(type, out var list) ? list.ToArray()
                : Array.Empty<Item>();
        }

        // 0 �ƴ� �ɷ�ġ ǥ�õȰ͸� ���
        public string ItemInfoText()
        {
            var parts = new List<string>();
            if (AtkBonus != 0) parts.Add($"���ݷ� +{AtkBonus}");
            if (DefBonus != 0) parts.Add($"���� +{DefBonus}");
            if (HpBonus != 0) parts.Add($"ü�� +{HpBonus}");
            if (MpBonus != 0) parts.Add($"���� +{MpBonus}");

            string statInfo = parts.Any()
                ? " (" + string.Join(" ", parts) + ")"
                : string.Empty;

            return Name + statInfo;
        }

        // ȸ�� ������ ���
        public void UseItem(Player player)
        {
            if (Type != 2) return;

            player.Heal(0, HpBonus);
            player.GainMp(0, MpBonus);
            Inventory.RemoveItem(this);
        }
    }

    public static class ItemExt
    {
        // ������ �ε���, ���� ���� ���� ���
        public static void PrintEquipStatus(this IEnumerable<Item> items, Player player)
        {
            int idx = 1;
            foreach (var item in items)
            {
                bool equipped = player.IsEquipped(item);
                Console.WriteLine($"-{idx}. {(equipped ? "(E)" : string.Empty)} {item.ItemInfoText()}");
                idx++;
            }
        }

        // �⺻ ������ �ε��� ���
        public static void PrintBasicList(this IEnumerable<Item> items)
        {
            int idx = 1;
            foreach (var item in items)
            {
                Console.WriteLine($"-{idx}. {item.ItemInfoText()}");
                idx++;
            }
        }

        // ��Ͽ��� ������ ����
        public static void RemoveItem(this List<Item> items, Item item)
        {
            items.Remove(item);
        }
    }
}