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

        // 두가지 이상 능력치가 있는 아이템은 기본 능력치로 설정.
        public int AtkBonus { get; private set; }
        public int DefBonus { get; private set; }
        public int HpBonus { get; private set; }
        public int MpBonus { get; private set; }

        // 강화 시스템
        public int TotalValue { get; set; } = 0; // 강화 수치
        public int MaxValue { get; private set; } = 50; // 최대 강화 수치

        // 아이템 설명, 가격
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

        // 정보 출력 기능 ( 0 인 값은 출력 X )
        public string ItemInfoText()
        {
            string typeText = Type switch
            {
                0 => "무기",
                1 => "방어구",
                2 => "소모품",
                3 => "장신구",
                4 => "기타",
                5 => "이벤트"
            };

            List<string> stats = new List<string>();
            if (AtkBonus > 0) stats.Add($"공격력 +{AtkBonus}");
            if (DefBonus > 0) stats.Add($"방어력 +{DefBonus}");
            if (HpBonus > 0) stats.Add($"체력 +{HpBonus}");
            if (MpBonus > 0) stats.Add($"마나 +{MpBonus}");

            string statInfo = stats.Count > 0 ? $" ({string.Join("  |  ", stats)})" : string.Empty;

            return $"{typeText} {Name} - {Desc}{statInfo}";
        }

        // 회복 아이템 사용
        public void UseItem(Player player)
        {
            if (Type != 2) return;

            player.Heal(0, HpBonus);
            player.GainMp(0, MpBonus);
        }
    }



    public static class ItemExt
    {
        // 보유한 아이템 + 장착여부 : 인벤토리 (All Type)
        public static void HasItemList(List<Item> items, Player player)
        {
            int idx = 1;
            foreach (var item in items)
            {
                Console.WriteLine($"-{idx}. {item.ItemInfoText()}");
                idx++;
            }
        }

        // 장착 및 강화 가능 : 장착관리, 대장간 (Type 0, 1, 3)
        public static void EquipItemList(List<Item> items, Player player)
        {
            var inType = items.Where(Item => Item.Type == 2! || Item.Type == 5!);
            int idx = 1;
            foreach (var item in inType)
            {
                bool euipped = Inventory.IsEquipped()
            }
        }

        // 상점 아이템 (Type 1~4)
        public static void ShopItemList(this IEnumerable<Item> items, Player player)
        {
            var shopItems = items.Where(i => i.Type >= 1 && i.Type <= 4);
            int idx = 1;

            foreach (var item in shopItems)
            {
                string displayPrice;

                if (item.Type == 2) // 소모품 무한 구매 가능
                {
                    displayPrice = $"{item.Price} G";
                }
                else if (player.HasItem(item)) // 이미 구매한 장비
                {
                    displayPrice = "구매완료";
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