using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        {                               // �̸�, Ÿ��, ���ݷ�, ����, hp, mp, 
               { JobType.����, new[] { new Item("���ε� �ܰ�", 0, 0, 0, 0, 0, "����� �������̰� �̸��� ������ �������� ��", 0) } },
               { JobType.������, new[] { new Item("������ ����������", 0, 0, 0, 0, 0, "����Ȯ�� ������ �����̴�", 0) } },
               { JobType.���ݼ���, new[] { new Item("�̿ϼ��� ������", 0, 0, 0, 0, 15, "ȣ�ſ��ε� ���� ��� �ȵȴ�", 0) } },
               { JobType.��������, new[] { new Item("������ ����(��)", 0, 40, -25, 0, 0, "7", 0),
                   new Item("������ �߰�", 1, -15, 20, 0, 0, "8", 0),
                   new Item("����� ����", 1, -15, -15, 50, 0, "9", 0) } },
               { JobType.���Ż�, new[] { new Item("����� �Ͱ���", 3, 0, 0, 0, 0, "�Ҿ���� �� ��� �Ϳ� ��� ����", 0) } }
        };

        public static readonly List<Item> ShopItemsDb = new List<Item>
        {
            new Item("ö��", 0, 5, 0, 0, 0, "�⺻���� ö���Դϴ�.", 500),
            new Item("��ī�ο� �ܰ�", 0, 7, 0, 0, 0, "������ � �� �ִ� �ܰ��Դϴ�.", 1000),
            new Item("���ſ� ����", 0, 10, -2, 0, 0, "������ �� ���� ���� �����Դϴ�.", 1500),
            new Item("��� â", 0, 8, 0, 0, 0, "������ â�Դϴ�.", 140),
            new Item("������ �ռҵ�", 0, 9, 0, 0, 0, "���� �� �� �ռҵ��Դϴ�.", 160),
            new Item("�ż��� �ְ�", 0, 6, 0, 0, 0, "���� ���ݿ� �ְ��Դϴ�.", 150),

            new Item("ö����", 1, 0, 5, 0, 0, "�⺻���� ö�� �����Դϴ�.", 1200),
            new Item("���� ����", 1, 0, 7, 0, 0, "������ ưư�� ���� �����Դϴ�.", 1400),
            new Item("���� ����", 1, 0, 4, 0, 0, "�ΰ����� �����ִ� ���� �����Դϴ�.", 1100),
            new Item("������ �κ�", 1, 0, 0, 0, 10, "������ ������Ű�� �κ��Դϴ�.", 1300),
            new Item("����� ����", 1, 0, 10, 0, 0, "������ �����Դϴ�.", 2000),
            new Item("�ż��� �尩", 1, 0, 2, 0, 0, "���� �ճ�� ���� �尩�Դϴ�.", 1600),

            new Item("���� ����", 3, 2, 2, 0, 0, "�̼��� ���� �ο��ϴ� �����Դϴ�.", 2000),
            new Item("�����̾� �����", 3, 0, 3, 0, 10, "������ ���� �����̾� ������Դϴ�.", 2200),
            new Item("���� ����", 3, 3, 0, 5, 0, "���� ������ ����� �����Դϴ�.", 2100),
            new Item("����� ��ű�", 3, 0, 0, 0, 5, "������ ��ű��Դϴ�.", 2500),

            new Item("�μ��� �ð�", 4, 0, 0, 0, 0, "�ð��� �Ҿ���� �ð��Դϴ�.", 700),
            new Item("��� ����", 4, 0, 0, 0, 0, "����� ��� �ô��� �����Դϴ�.", 3000),
            new Item("���ٷ� ����", 4, 0, 0, 0, 0, "��򰡷� ���� ���� �˷������� �𸨴ϴ�.", 2500),

            new Item("HP ����",2, 0, 0, 10, 0, "ü���� 20 ȸ���մϴ�.", 100),
            new Item("HP ����",2, 0, 0, 30, 0, "ü���� 20 ȸ���մϴ�.", 200),
            new Item("MP ����",2, 0, 0, 0, 10, "������ 20 ȸ���մϴ�.", 100),
            new Item("MP ����",2, 0, 0, 0, 30, "������ 20 ȸ���մϴ�.", 200)
        };


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