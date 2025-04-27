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

        // 이벤트용 출력 폼 = 수정가능.
        public string GetSimpleInfo()
        {
            return $"{Name}  |  {Desc}";
        }

        // 상세 정보 출력 (장비/소모품)
        public string ItemTypeTextInfo()
        {
            List<string> stats = new List<string>();

            if (AtkBonus != 0)
            {
                string atkText = $"공격력 {(AtkBonus > 0 ? "+" : "")}{AtkBonus}";
                if (TotalValue > 0) atkText += $" (▲{TotalValue})";
                stats.Add(atkText);
            }
            if (DefBonus != 0)
            {
                string defText = $"방어력 {(DefBonus > 0 ? "+" : "")}{DefBonus}";
                if (TotalValue > 0) defText += $" (▲{TotalValue})";
                stats.Add(defText);
            }
            if (HpBonus != 0) stats.Add($"체력 {(HpBonus > 0 ? "+" : "")}{HpBonus}");
            if (MpBonus != 0) stats.Add($"마나 {(MpBonus > 0 ? "+" : "")}{MpBonus}");

            string statInfo = stats.Count > 0 ? $" | {string.Join("  |  ", stats)}" : "";

            string typeText = Type switch
            {
                0 => "무기",
                1 => "방어구",
                2 => "소모품",
                3 => "장신구",
                4 => "기타",
                5 => "이벤트",
                _ => "알수없음",
            };

            return $"{typeText} {Name}{statInfo}  |  {Desc}";
        }

        // 던전이벤트용 아이템 목록
        public static readonly List<Item> EventItemsDb = new List<Item>()
        {
            new Item("부러진 검", 5, 5, 0, 0, 0, "세월의 흔적이 보이는 부러진 검 입니다.", 0),
            new Item("옛 영웅의 검", 5, 20, 0, 0, 0, "옛 영웅의 검", 0),
            new Item("저주받은 검", 5, 15, 0, 0, 0, "기분나쁜 검 입니다.", 0),
            new Item("물고기", 5, 0, 0, 2, 0, "아주 싱싱해보이는 물고기이다.", 0)
        };

        // 초기 보상용 직업별 아이템 목록
        public static readonly Dictionary<JobType, Item[]> GiftItemsDb = new()
        {
            { JobType.전사, new[] { new Item("pp", 0, 0, 0, 0, 0, "1", 0) } },
            { JobType.마법사, new[] { new Item("mm", 0, 0, 0, 0, 0, "3", 0) } },
            { JobType.과학자, new[] { new Item("zz", 0, 0, 0, 0, 15, "5", 0) } },
            { JobType.대장장이, new[] {
                new Item("dd", 0, 5, 0, 0, 0, "7", 0),
                new Item("ss", 1, 0, 3, 5, 0, "8", 0),
                new Item("ee", 1, 0, 3, 5, 0, "9", 0)
            }},
            { JobType.영매사, new[] { new Item("bb", 3, 0, 0, 0, 0, "9", 0) } }
        };

        /*public static Item[] ShopItemsDb()
        {
            return new Item[]
            {
         new Item("철검",   0, 5, 0, 0, 0, "기본적인 철검입니다.", 100),
         new Item("철방패", 1, 0, 5, 0, 0, "기본적인 철제 방패입니다.", 120),
         new Item("HP 포션",2, 0, 0, 20, 0, "체력을 20 회복합니다.", 50),
         new Item("MP 포션",2, 0, 0, 0, 20, "마나를 20 회복합니다.", 50)
            };
        }*/

        // 회복 아이템 사용
        public void UseItem(Player player)
        {
            if (Type != 2) return;

            player.Heal(0, HpBonus);
            player.GainMp(0, MpBonus);
        }
    }



    // 아이템 목록 출력용
    public static class ItemExt
    {
        // 인벤토리 출력
        public static void PrintInventory(List<Item> items, Player player)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Item item = items[i];
                string equipMark = player.IsEquipped(item) ? "[E]" : "[ ]";
                Console.WriteLine($"- {i + 1}. {equipMark} {item.ItemTypeTextInfo()}");
            }
        }

        // 장착관리, 대장간 출력 (Type == 0,1,3만)
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

        // 상점 구매용 출력
        public static void PrintShop(List<Item> items, Player player, Inventory inventory)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Item item = items[i];
                string equipMark = "[ ]";

                string priceText;
                if (item.Type == 2) // 소모품은 무한구매
                {
                    priceText = $"{item.Price} G";
                }
                else
                {
                    priceText = inventory.HasItem(item) ? "구매 완료" : $"{item.Price} G";
                }

                Console.WriteLine($"- {i + 1}. {equipMark} {item.ItemTypeTextInfo()}  |  {priceText}");
            }
        }

        // 이벤트용 간단 출력 - 수정가능
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