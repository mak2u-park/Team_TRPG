namespace Sparta_Dungeon_TeamProject
{
    // 플레이어 클래스
    public class Player
    {
        public int Level { get; }
        public string Name { get; }
        public JobType Job { get; }
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; private set; }
        public int Gold { get; private set; }

        public int ExtraAtk { get; private set; }
        public int ExtraDef { get; private set; }

        private List<Item> Inventory = new List<Item>();
        private List<Item> EquipList = new List<Item>();

        public int InventoryCount
        {
            get
            {
                return Inventory.Count;
            }
        }

        public Player(int level, string name, JobType job, int atk, int def, int hp, int gold)
        {
            Level = level;
            Name = name;
            Job = job;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
        }

        // 1. 상태보기 # Program.cs
        public void DisplayPlayerInfo()
        {
            Console.WriteLine($"Lv. {Level:D2}");
            Console.WriteLine($"{Name} {{ {Job} }}");
            Console.WriteLine(ExtraAtk == 0 ? $"공격력 : {Atk}" : $"공격력 : {Atk + ExtraAtk} (+{ExtraAtk})");
            Console.WriteLine(ExtraDef == 0 ? $"방어력 : {Def}" : $"방어력 : {Def + ExtraDef} (+{ExtraDef})");
            Console.WriteLine($"체력 : {Hp}");
            Console.WriteLine($"Gold : {Gold} G");
        }

        // 인벤토리 # Inventory.cs
        public void DisplayInventory(bool showIdx)
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                Item targetItem = Inventory[i];

                string displayIdx = showIdx ? $"{i + 1} " : "";
                string displayEquipped = IsEquipped(targetItem) ? "[E]" : "";
                Console.WriteLine($"- {displayIdx}{displayEquipped} {targetItem.ItemInfoText()}");
            }
        }

        // 장비 착용여부 # Inventory.cs
        public void EquipItem(Item item)
        {
            if (IsEquipped(item))
            {
                EquipList.Remove(item);
                if (item.Type == 0)
                    ExtraAtk -= item.Value;
                else
                    ExtraDef -= item.Value;
            }
            else
            {
                EquipList.Add(item);
                if (item.Type == 0)
                    ExtraAtk += item.Value;
                else
                    ExtraDef += item.Value;
            }
        }

        public bool IsEquipped(Item item)
        {
            return EquipList.Contains(item);
        }

        // 인벤토리 아이템 목록 반환(조회) # Inventory.cs
        public List<Item> GetInventoryItems()
        {
            return Inventory;
        }

        // 구매 및 판매 아이템 관련 # Shop.cs
        public void BuyItem(Item item)
        {
            Gold -= item.Price;
            Inventory.Add(item);
        }

        public bool HasItem(Item item)
        {
            return Inventory.Contains(item);
        }

        public void SellItem(Item item)
        {
            Gold += (int)(item.Price * 0.85);
            Inventory.Remove(item);
            EquipList.Remove(item);
            if (item.Type == 0) ExtraAtk -= item.Value;
            else ExtraDef -= item.Value;
        }

        // 휴식기능 관련 # Program.cs
        public void Rest()
        {
            Gold -= 500;
            Hp = 100;
        }

        // 아이템 강화 - 오류 발생 bool값으로 변경 시도 # Inventory.cs
        public bool UpgradeItem(Item item)
        {
            const int upgrageCost = 50; // 기본 강화 비용

            if (item.Type < 30) // 능력치 30 이하일 때
            {
                Gold -= 50;
                item.Value += 1;
                return true;
            }
            else if (item.Type >= 30 && item.Type < 50) // 30이상 50미만
            {
                Gold -= 100;
                item.Value += 3;
                return true;
            }
            else if (item.Type >= 50 && item.Type < 100) // 50이상 100미만
            {
                Gold -= 200;
                item.Value += 5;
                return true;
            }

            //강화 실패
            else if (item.Type >= 100)
            {
                return false;
            }
            else if (Gold < upgrageCost)
            {
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}