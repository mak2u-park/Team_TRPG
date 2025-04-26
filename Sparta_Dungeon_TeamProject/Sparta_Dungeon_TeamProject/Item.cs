using System;
using System.Collections.Generic;
using System.Linq;

namespace Sparta_Dungeon_TeamProject
{
    public class Item
    {
        public string Name { get; }
        public int Type { get; }

        // 두가지 이상 능력치가 있는 아이템은 기본 능력치로 설정.
        public int AtkBonus { get; }
        public int DefBonus { get; }
        public int HpBonus { get; }
        public int MpBonus { get; }

        // 강화 시스템
        public int TotalValue { get; set; } = 0; // 강화 수치
        public int MaxValue { get; set; } = 50; // 최대 강화 수치

        // 아이템 설명, 가격
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

            if (AtkBonus != 0) parts.Add($"공격력 +{AtkBonus}");
            if (DefBonus != 0) parts.Add($"방어력 +{DefBonus}");
            if (HpBonus != 0) parts.Add($"체력 +{HpBonus}");
            if (MpBonus != 0) parts.Add($"마나 +{MpBonus}");

            string statInfo = parts.Any()
                ? " (" + string.Join(" ", parts) + ")"
                : string.Empty;

            return Name + statInfo;
        }

        // 아이템 사용 (소모품, 회복아이템 전용)
        public void UseItem(Player player)
        {
            if (Type != 2) return;

            player.Heal(0, HpBonus); // HP 회복
            player.GainMp(0, MpBonus); // MP 회복
            Inventory.RemoveItem(this);
        }


        // --- 아래부터 static 데이터베이스 --- //
        // 1. 직업별 초기 보상 아이템
        public static readonly Dictionary<JobType, Item[]> GiftItemsDb = new()
        {
            { JobType.전사, new[] { new Item("pp", 0, 0, 0, 0, 0, "1", 0) } },
            { JobType.마법사, new[] { new Item("mm", 0, 0, 0, 0, 0, "3", 0) } },
            { JobType.과학자, new[] { new Item("zz", 0, 0, 0, 0, 15, "5", 0) } },
            { JobType.대장장이, new[] { new Item("dd", 0, 5, 0, 0, 0, "7", 0),
                                                 new Item("ss", 1, 0, 3, 5, 0, "8", 0),
                                                 new Item("ee", 1, 0, 3, 5, 0, "9", 0) } },
            { JobType.영매사, new[] { new Item("bb", 3, 0, 0, 0, 0, "9", 0) } }
        };

        // 2. 이벤트/전투 획득용 아이템 <Type == 5>
        public static readonly Item[] EventItemsDb = new Item[]
        {
            new Item("부러진 검", 5, 5, 0, 0, 0, "세월의 흔적이 보이는 부러진 검 입니다.", 0),
            new Item("옛 영웅의 검", 5, 20, 0, 0, 0, "옛 영웅의 검", 0),
            new Item("저주받은 검", 5, 15, 0, 0, 0, "기분나쁜 검 입니다.", 0),
            new Item("물고기", 5, 0, 0, 2, 0, "아주 싱싱해보이는 물고기이다.", 0)
        };

        // 3. 상점용 아이템 <Type: 0=무기, 1=방어구, 2=회복아이템, 3=기타>
        public static Item[] ShopItemsDb()
        {
            return new Item[]
            {
                new Item("철검",   0, 5, 0, 0, 0, "기본적인 철검입니다.", 100),
                new Item("철방패", 1, 0, 5, 0, 0, "기본적인 철제 방패입니다.", 120),
                new Item("HP 포션",2, 0, 0, 20, 0, "체력을 20 회복합니다.", 50),
                new Item("MP 포션",2, 0, 0, 0, 20, "마나를 20 회복합니다.", 50)
            };
        }

        // 타입별 그룹화 (상점, 인벤 필터링)
        public static readonly Dictionary<int, List<Item>> ItemTypeD
        = ShopItemsDb().GroupBy(item => item.Type).ToDictionary(g => g.Key, g => g.ToList());

        public static Item[] GetItemTypeD(int type)
        {
            return ItemTypeD.TryGetValue(type, out var list) ? list.ToArray() : Array.Empty<Item>();
        }
    }


    // 아이템 출력 관련 기능 클래스
    public static class ItemExt
    {
        // 장착표시
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

        // 장착 미표시
        public static void PrintBasicList(this IEnumerable<Item> items)
        {
            int idx = 1;
            foreach (var item in items)
            {
                Console.WriteLine($"-{idx}. {item.ItemInfoText()}");
                idx++;
            }
        }

        // 목록에서 아이템 삭제
        public void RemoveItem(this List<Item> items, Item item)
        {
            items.Remove(item);
        }
    }
}