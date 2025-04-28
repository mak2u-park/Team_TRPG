using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sparta_Dungeon_TeamProject;
using static Sparta_Dungeon_TeamProject.Program;

namespace Sparta_Dungeon_TeamProject
{
    // 인벤토리 전용 UI
    public class Inventory
    {
        private Player player; // 싱글톤 패턴으로 Player 인스턴스에 접근
        private static Inventory inventory;
        private List<Item> items = new();
        private List<Item> equippedItems = new(); // 장착된 아이템 목록
        private Dictionary<string, bool> firstVisitFlags;

        // 기본 생성자 초기화
        private Inventory() { }
        public static Inventory Instance
        {
            get
            {
                if (inventory == null)
                {
                    inventory = new Inventory();
                }
                return inventory;
            }
        }

        //program 에서 받을 생성자 추가
        public Inventory(Player player, IEnumerable<Item> initialItems)
        {
            this.player = player;
            this.items = new List<Item>(initialItems);
        }

        // 아이템 추가/제거
        public void AddItem(Item item) => items.Add(item);
        public void RemoveItem(Item item) => items.Remove(item);

        // 아이템 목록 반환
        public List<Item> GetInventoryItems()
        {
            return new List<Item>(items);
        }

        // 보유, 장착 여부 확인
        public bool HasItem(Item item) => items.Contains(item);
        public bool IsEquipped(Item item) => equippedItems.Contains(item);
        public void EquipItem(Item item)
        {
            if (item.Type != 0 && item.Type != 1 && item.Type != 3)
                return;
            if (!equippedItems.Contains(item))
                equippedItems.Add(item);
        }
        public void UnequipItem(Item item)
        {
            if (equippedItems.Contains(item))
                equippedItems.Remove(item);
        }

        public int GetTotalAtkBonus()
        {
            return equippedItems.Sum(item => item.AtkBonus + item.TotalValue);
        }

        public int GetTotalDefBonus()
        {
            return equippedItems.Sum(item => item.DefBonus + item.TotalValue);
        }

        public int GetTotalHpBonus()
        {
            return equippedItems.Sum(item => item.HpBonus + item.TotalValue);
        }

        public int GetTotalMpBonus()
        {
            return equippedItems.Sum(item => item.MpBonus + item.TotalValue);
        }
    }
}