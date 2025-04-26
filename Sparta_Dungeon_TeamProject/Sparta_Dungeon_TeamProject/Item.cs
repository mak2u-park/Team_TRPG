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
                { JobType.전사, new List<Item> { new Item("pp", 0, 0, 0, 0, 0, "1", 0) } },
                { JobType.마법사, new List<Item> { new Item("mm", 0, 0, 0, 0, 00, "3", 0) } },
                { JobType.과학자, new List<Item> { new Item("zz", 0, 0, 0, 0, 15, "5", 0) } },
                { JobType.대장장이, new List<Item> { new Item("dd", 0, 5, 0, 0, 0, "7", 0),
                                                     new Item("ss", 1, 0, 3, 5, 0, "8", 0),
                                                     new Item("ee", 1, 0, 3, 5, 0, "9", 0) } },
                { JobType.영매사, new List<Item> { new Item("bb", 3, 0, 0, 0, 0,"9", 0) } }
            };
        }

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

        // 1. 직업별 초기 보상 아이템
        public static readonly Dictionary<JobType, Item[]> GifttemDb = new()
        {           // 이름, type, atk, def, hp, mp, 설명, price
            { JobType.전사, new[] {
                new Item("pp", 0, 0, 0, 0, 0, "1", 0),
            }},
            { JobType.마법사, new[] {
                new Item("mm", 0, 0, 0, 0, 00, "3", 0),
            }},
            { JobType.과학자, new[] {
                new Item("zz", 0, 0, 0, 0, 15, "5", 0),
            }},
            { JobType.대장장이, new[] {
                new Item("dd", 0, 5, 0, 0, 0, "7", 0),
                new Item("ss", 1, 0, 3, 5, 0, "8", 0),
                new Item("ee", 1, 0, 3, 5, 0, "9", 0),
            }},
            { JobType.영매사, new[] {
                new Item("bb", 3, 0, 0, 0, 0, "9", 0),
            }}
        };

        // 2. 이벤트/전투 획득용 아이템 <Type == 5>
        public static readonly Item[] EventItemDb = new Item[]
        {           // 이름, type, atk, def, hp, mp, 설명, price     // index
            new Item("부러진 검", 5, 5, 0, 0, 0, "세월의 흔적이 보이는 부러진 검 입니다.", 0), // 0
            new Item("옛 영웅의 검", 5, 20, 0, 0, 0, "옛 영웅의 검", 0),  // 1
            new Item("저주받은 검", 5, 15, 0, 0, 0, "기분나쁜 검 입니다.", 0),
            new Item("물고기", 5, 0, 0, 2, 0, "아주 싱싱해보이는 물고기이다.", 0),
        };

        // 3. 상점용 아이템 <Type: 0=무기, 1=방어구, 2=회복아이템, 3=기타>
        public static Item[] InitializeItemDb() => new Item[]
        {           // 이름, type, atk, def, hp, mp, 설명, price
            new Item("철검",   0, 5, 0, 5, 0, "기본적인 철검입니다.", 100),
            new Item("철방패", 1, 0, 5, 0, 0, "기본적인 철제 방패입니다.", 120),
            new Item("HP 포션",2, 0, 0, 20, 0, "최대 체력을 20 회복합니다.", 50),
            new Item("MP 포션",2, 0, 0, 0, 20, "최대 마나를 20 회복합니다.", 50),
        };

        // 타입별 그룹화
        public static readonly Dictionary<int, List<Item>> ItemTypeD
            = InitializeItemDb().GroupBy(item => item.Type).ToDictionary(g => g.Key, g => g.ToList());


        //타입별 아이템 가져오기
        public static Item[] GetItemTypeD(int type)
        {
            return ItemTypeD.TryGetValue(type, out var list) ? list.ToArray()
                : Array.Empty<Item>();
        }

        // 0 아닌 능력치 표시된것만 출력
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

        // 회복 아이템 사용
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
        // 아이템 인덱스, 장착 상태 포함 출력
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

        // 기본 아이템 인덱스 출력
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
        public static void RemoveItem(this List<Item> items, Item item)
        {
            items.Remove(item);
        }
    }
}