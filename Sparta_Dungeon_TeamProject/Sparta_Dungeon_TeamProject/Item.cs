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
        {                               // 이름, 타입, 공격력, 방어력, hp, mp, 
               { JobType.전사, new[] { new Item("각인된 단검", 0, 0, 0, 0, 0, "노쇄한 대장장이가 이름을 각인해 선물해준 검", 0) } },
               { JobType.마법사, new[] { new Item("연습용 마법지팡이", 0, 0, 0, 0, 0, "부정확한 연습용 지팡이다", 0) } },
               { JobType.연금술사, new[] { new Item("미완성된 광선검", 0, 0, 0, 0, 15, "호신용인데 방향 제어가 안된다", 0) } },
               { JobType.대장장이, new[] { new Item("망각의 유산(검)", 0, 40, -25, 0, 0, "7", 0),
                   new Item("속죄의 견갑", 1, -15, 20, 0, 0, "8", 0),
                   new Item("배신의 갑주", 1, -15, -15, 50, 0, "9", 0) } },
               { JobType.영매사, new[] { new Item("기억의 귀걸이", 3, 0, 0, 0, 0, "잃어버린 말 대신 귀에 담긴 기억들", 0) } }
        };

        public static readonly List<Item> ShopItemsDb = new List<Item>
        {
            new Item("철검", 0, 5, 0, 0, 0, "기본적인 철검입니다.", 500),
            new Item("날카로운 단검", 0, 7, 0, 0, 0, "빠르게 찌를 수 있는 단검입니다.", 1000),
            new Item("무거운 도끼", 0, 10, -2, 0, 0, "묵직한 한 방을 가진 도끼입니다.", 1500),
            new Item("고대 창", 0, 8, 0, 0, 0, "오래된 창입니다.", 140),
            new Item("연마된 롱소드", 0, 9, 0, 0, 0, "날이 잘 선 롱소드입니다.", 160),
            new Item("신속의 쌍검", 0, 6, 0, 0, 0, "빠른 공격용 쌍검입니다.", 150),

            new Item("철방패", 1, 0, 5, 0, 0, "기본적인 철제 방패입니다.", 1200),
            new Item("가죽 갑옷", 1, 0, 7, 0, 0, "가볍고 튼튼한 가죽 갑옷입니다.", 1400),
            new Item("무쇠 투구", 1, 0, 4, 0, 0, "두개골을 지켜주는 무쇠 투구입니다.", 1100),
            new Item("마법의 로브", 1, 0, 0, 0, 10, "마법을 증폭시키는 로브입니다.", 1300),
            new Item("고대의 갑옷", 1, 0, 10, 0, 0, "오래된 갑옷입니다.", 2000),
            new Item("신속의 장갑", 1, 0, 2, 0, 0, "빠른 손놀림을 위한 장갑입니다.", 1600),

            new Item("은빛 반지", 3, 2, 2, 0, 0, "미세한 힘을 부여하는 반지입니다.", 2000),
            new Item("사파이어 목걸이", 3, 0, 3, 0, 10, "마력을 돕는 사파이어 목걸이입니다.", 2200),
            new Item("붉은 팔찌", 3, 3, 0, 5, 0, "전투 본능을 끌어내는 팔찌입니다.", 2100),
            new Item("고대의 장신구", 3, 0, 0, 0, 5, "오래된 장신구입니다.", 2500),

            new Item("부서진 시계", 4, 0, 0, 0, 0, "시간을 잃어버린 시계입니다.", 700),
            new Item("고대 동전", 4, 0, 0, 0, 0, "희귀한 고대 시대의 동전입니다.", 3000),
            new Item("빛바랜 지도", 4, 0, 0, 0, 0, "어딘가로 가는 길을 알려줄지도 모릅니다.", 2500),

            new Item("HP 포션",2, 0, 0, 10, 0, "체력을 20 회복합니다.", 100),
            new Item("HP 포션",2, 0, 0, 30, 0, "체력을 20 회복합니다.", 200),
            new Item("MP 포션",2, 0, 0, 0, 10, "마나를 20 회복합니다.", 100),
            new Item("MP 포션",2, 0, 0, 0, 30, "마나를 20 회복합니다.", 200)
        };


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