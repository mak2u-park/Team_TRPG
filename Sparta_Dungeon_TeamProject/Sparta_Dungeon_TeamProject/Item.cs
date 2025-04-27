using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;

namespace Sparta_Dungeon_TeamProject
{
    public class Item
    {
        public string Name { get; private set; }
        public int Type { get; private set; }

        // �ΰ��� �̻� �ɷ�ġ�� �ִ� �������� �⺻ �ɷ�ġ�� ����.
        public int AtkBonus { get; private set; }
        public int DefBonus { get; private set; }
        public int HpBonus { get; private set; }
        public int MpBonus { get; private set; }

        // ��ȭ �ý���
        public int TotalValue { get; set; } = 0; // ��ȭ ��ġ
        public int MaxValue { get; private set; } = 50; // �ִ� ��ȭ ��ġ

        // ������ ����, ����
        public string Desc { get; private set; }
        public int Price { get; private set; }

        public static Item[] ItemDb { get; private set; } = Array.Empty<Item>();
        public static Dictionary<JobType, List<Item>> GifttemDb { get; private set; } = new();

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

        // �̺�Ʈ�� ��� �� = ��������.
        public string GetSimpleInfo()
        {
            return $"{Name}  |  {Desc}";
        }

        // �� ���� ��� (���/�Ҹ�ǰ)
        public string ItemTypeTextInfo()
        {
            List<string> stats = new List<string>();

            if (AtkBonus != 0)
            {
                string atkText = $"���ݷ� {(AtkBonus > 0 ? "+" : "")}{AtkBonus}";
                if (TotalValue > 0) atkText += $" (��{TotalValue})";
                stats.Add(atkText);
            }
            if (DefBonus != 0)
            {
                string defText = $"���� {(DefBonus > 0 ? "+" : "")}{DefBonus}";
                if (TotalValue > 0) defText += $" (��{TotalValue})";
                stats.Add(defText);
            }
            if (HpBonus != 0) stats.Add($"ü�� {(HpBonus > 0 ? "+" : "")}{HpBonus}");
            if (MpBonus != 0) stats.Add($"���� {(MpBonus > 0 ? "+" : "")}{MpBonus}");

            string statInfo = stats.Count > 0 ? $" | {string.Join("  |  ", stats)}" : "";

            string typeText = Type switch
            {
                0 => "����",
                1 => "��",
                2 => "�Ҹ�ǰ",
                3 => "��ű�",
                4 => "��Ÿ",
                5 => "�̺�Ʈ",
                _ => "�˼�����",
            };

            return $"{typeText} {Name}{statInfo}  |  {Desc}";
        }

        // �����̺�Ʈ�� ������ ���
        public static readonly List<Item> EventItemsDb = new List<Item>()
        {
            new Item("�η��� ��", 5, 5, 0, 0, 0, "������ ������ ���̴� �η��� �� �Դϴ�.", 0),
            new Item("�� ������ ��", 5, 20, 0, 0, 0, "�� ������ ��", 0),
            new Item("���ֹ��� ��", 5, 15, 0, 0, 0, "��г��� �� �Դϴ�.", 0),
            new Item("�����", 5, 0, 0, 2, 0, "���� �̽��غ��̴� ������̴�.", 0)
        };

        // �ʱ� ����� ������ ������ ���
        public static readonly Dictionary<JobType, Item[]> GiftItemsDb = new()
        {
            { JobType.����, new[] { new Item("pp", 0, 0, 0, 0, 0, "1", 0) } },
            { JobType.������, new[] { new Item("mm", 0, 0, 0, 0, 0, "3", 0) } },
            { JobType.������, new[] { new Item("zz", 0, 0, 0, 0, 15, "5", 0) } },
            { JobType.��������, new[] {
                new Item("dd", 0, 5, 0, 0, 0, "7", 0),
                new Item("ss", 1, 0, 3, 5, 0, "8", 0),
                new Item("ee", 1, 0, 3, 5, 0, "9", 0)
            }},
            { JobType.���Ż�, new[] { new Item("bb", 3, 0, 0, 0, 0, "9", 0) } }
        };

        /*public static Item[] ShopItemsDb()
        {
            return new Item[]
            {
         new Item("ö��",   0, 5, 0, 0, 0, "�⺻���� ö���Դϴ�.", 100),
         new Item("ö����", 1, 0, 5, 0, 0, "�⺻���� ö�� �����Դϴ�.", 120),
         new Item("HP ����",2, 0, 0, 20, 0, "ü���� 20 ȸ���մϴ�.", 50),
         new Item("MP ����",2, 0, 0, 0, 20, "������ 20 ȸ���մϴ�.", 50)
            };
        }*/

        // ȸ�� ������ ���
        public void UseItem(Player player)
        {
            if (Type != 2) return;

            player.Heal(0, HpBonus);
            player.GainMp(0, MpBonus);
        }
    }



    // ������ ��� ��¿�
    public static class ItemExt
    {
        // �κ��丮 ���
        public static void PrintInventory(List<Item> items, Player player)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Item item = items[i];
                string equipMark = player.IsEquipped(item) ? "[E]" : "[ ]";
                Console.WriteLine($"- {i + 1}. {equipMark} {item.ItemTypeTextInfo()}");
            }
        }

        // ��������, ���尣 ��� (Type == 0,1,3��)
        public static void PrintEquipable(List<Item> items, Player player)
        {
            var filtered = items.Where(item => item.Type == 0 || item.Type == 1 || item.Type == 3).ToList();
            for (int i = 0; i < filtered.Count; i++)
            {
                Item item = filtered[i];
                string equipMark = player.IsEquipped(item) ? "[E]" : "[ ]";
                Console.WriteLine($"- {i + 1}. {equipMark} {item.ItemTypeTextInfo()}");
            }
        }

        // ���� ���ſ� ���
        public static void PrintShop(List<Item> items, Player player, Inventory inventory)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Item item = items[i];
                string equipMark = "[ ]";

                string priceText;
                if (item.Type == 2) // �Ҹ�ǰ�� ���ѱ���
                {
                    priceText = $"{item.Price} G";
                }
                else
                {
                    priceText = inventory.HasItem(item) ? "���� �Ϸ�" : $"{item.Price} G";
                }

                Console.WriteLine($"- {i + 1}. {equipMark} {item.ItemTypeTextInfo()}  |  {priceText}");
            }
        }

        // �̺�Ʈ�� ���� ��� - ��������
        public static void PrintSimple(List<Item> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Item item = items[i];
                Console.WriteLine($"- {i + 1}. {item.GetSimpleInfo()}");
            }
        }
    }
}