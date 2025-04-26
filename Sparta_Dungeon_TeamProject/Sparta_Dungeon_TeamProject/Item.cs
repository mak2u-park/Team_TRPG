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

        // ���� ��� ��� ( 0 �� ���� ��� X )
        public string ItemInfoText()
        {
            string typeText = Type switch
            {
                0 => "����",
                1 => "��",
                2 => "�Ҹ�ǰ",
                3 => "��ű�",
                4 => "��Ÿ",
                5 => "�̺�Ʈ"
            };

            List<string> stats = new List<string>();
            if (AtkBonus > 0) stats.Add($"���ݷ� +{AtkBonus}");
            if (DefBonus > 0) stats.Add($"���� +{DefBonus}");
            if (HpBonus > 0) stats.Add($"ü�� +{HpBonus}");
            if (MpBonus > 0) stats.Add($"���� +{MpBonus}");

            string statInfo = stats.Count > 0 ? $" ({string.Join("  |  ", stats)})" : string.Empty;

            return $"{typeText} {Name} - {Desc}{statInfo}";
        }

        // ȸ�� ������ ���
        public void UseItem(Player player)
        {
            if (Type != 2) return;

            player.Heal(0, HpBonus);
            player.GainMp(0, MpBonus);
        }
    }



    public static class ItemExt
    {
        // ������ ������ + �������� : �κ��丮 (All Type)
        public static void HasItemList(List<Item> items, Player player)
        {
            int idx = 1;
            foreach (var item in items)
            {
                Console.WriteLine($"-{idx}. {item.ItemInfoText()}");
                idx++;
            }
        }

        // ���� �� ��ȭ ���� : ��������, ���尣 (Type 0, 1, 3)
        public static void EquipItemList(List<Item> items, Player player)
        {
            var inType = items.Where(Item => Item.Type == 2! || Item.Type == 5!);
            int idx = 1;
            foreach (var item in inType)
            {
                bool euipped = Inventory.IsEquipped()
            }
        }

        // ���� ������ (Type 1~4)
        public static void ShopItemList(this IEnumerable<Item> items, Player player)
        {
            var shopItems = items.Where(i => i.Type >= 1 && i.Type <= 4);
            int idx = 1;

            foreach (var item in shopItems)
            {
                string displayPrice;

                if (item.Type == 2) // �Ҹ�ǰ ���� ���� ����
                {
                    displayPrice = $"{item.Price} G";
                }
                else if (player.HasItem(item)) // �̹� ������ ���
                {
                    displayPrice = "���ſϷ�";
                }
                else
                {
                    displayPrice = $"{item.Price} G";
                }
                Console.WriteLine($"- {idx}. {displayPrice}");
            }
        }

    }
}