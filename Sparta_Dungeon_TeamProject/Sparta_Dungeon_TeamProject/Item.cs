using System;
using System.Collections.Generic;
using System.Linq;

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

        // ������ ��� (�Ҹ�ǰ, ȸ�������� ����)
        public void UseItem(Player player)
        {
            if (Type != 2) return;

            player.Heal(0, HpBonus); // HP ȸ��
            player.GainMp(0, MpBonus); // MP ȸ��
            Inventory.RemoveItem(this);
        }


        // --- �Ʒ����� static �����ͺ��̽� --- //
        // 1. ������ �ʱ� ���� ������
        public static readonly Dictionary<JobType, Item[]> GiftItemsDb = new()
        {
            { JobType.����, new[] { new Item("pp", 0, 0, 0, 0, 0, "1", 0) } },
            { JobType.������, new[] { new Item("mm", 0, 0, 0, 0, 0, "3", 0) } },
            { JobType.������, new[] { new Item("zz", 0, 0, 0, 0, 15, "5", 0) } },
            { JobType.��������, new[] { new Item("dd", 0, 5, 0, 0, 0, "7", 0),
                                                 new Item("ss", 1, 0, 3, 5, 0, "8", 0),
                                                 new Item("ee", 1, 0, 3, 5, 0, "9", 0) } },
            { JobType.���Ż�, new[] { new Item("bb", 3, 0, 0, 0, 0, "9", 0) } }
        };

        // 2. �̺�Ʈ/���� ȹ��� ������ <Type == 5>
        public static readonly Item[] EventItemsDb = new Item[]
        {
            new Item("�η��� ��", 5, 5, 0, 0, 0, "������ ������ ���̴� �η��� �� �Դϴ�.", 0),
            new Item("�� ������ ��", 5, 20, 0, 0, 0, "�� ������ ��", 0),
            new Item("���ֹ��� ��", 5, 15, 0, 0, 0, "��г��� �� �Դϴ�.", 0),
            new Item("�����", 5, 0, 0, 2, 0, "���� �̽��غ��̴� ������̴�.", 0)
        };

        // 3. ������ ������ <Type: 0=����, 1=��, 2=ȸ��������, 3=��Ÿ>
        public static Item[] ShopItemsDb()
        {
            return new Item[]
            {
                new Item("ö��",   0, 5, 0, 0, 0, "�⺻���� ö���Դϴ�.", 100),
                new Item("ö����", 1, 0, 5, 0, 0, "�⺻���� ö�� �����Դϴ�.", 120),
                new Item("HP ����",2, 0, 0, 20, 0, "ü���� 20 ȸ���մϴ�.", 50),
                new Item("MP ����",2, 0, 0, 0, 20, "������ 20 ȸ���մϴ�.", 50)
            };
        }

        // Ÿ�Ժ� �׷�ȭ (����, �κ� ���͸�)
        public static readonly Dictionary<int, List<Item>> ItemTypeD
        = ShopItemsDb().GroupBy(item => item.Type).ToDictionary(g => g.Key, g => g.ToList());

        public static Item[] GetItemTypeD(int type)
        {
            return ItemTypeD.TryGetValue(type, out var list) ? list.ToArray() : Array.Empty<Item>();
        }
    }


    // ������ ��� ���� ��� Ŭ����
    public static class ItemExt
    {
        // ����ǥ��
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

        // ���� ��ǥ��
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
        public void RemoveItem(this List<Item> items, Item item)
        {
            items.Remove(item);
        }
    }
}