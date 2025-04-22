using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sparta_Dungeon_TeamProject
{
    // 플레이어 클래스
    public class Player
    {
        public int Level { get; private set; }
        public int Exp { get; set; }
        public int MaxExp { get; private set; }
        public string Name { get; private set; }
        public JobType Job { get; private set; }
        public int Atk { get; private set; }
        public int Def { get; private set; }
        public int Hp { get; private set; }
        public int MaxHp { get; private set; }  
        public int Mp { get; set; }
        public int MaxMp { get; private set; }
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

        public Player(int level, int exp, int maxExp, string name, JobType job,int atk,int def, int hp, int maxHp, int mp, int maxMp, int gold)
        {
            Level = level;
            Exp = exp;
            MaxExp = maxExp;
            Name = name;
            Job = job;
            Atk = atk;
            Def = def;
            Hp = hp;
            MaxHp = maxHp;
            Mp = mp;
            MaxMp = maxMp;
            Gold = gold;

        }

        // 1. 상태보기 # Program.cs
        public void DisplayPlayerInfo()
        {
            Console.WriteLine($"Lv. {Level:D2} {{ {Exp}/{MaxExp} }}");
            Console.WriteLine($"{Name} {{ {Job} }}");
            Console.WriteLine(ExtraAtk == 0 ? $"공격력 : {Atk}" : $"공격력 : {Atk + ExtraAtk} (+{ExtraAtk})");
            Console.WriteLine(ExtraDef == 0 ? $"방어력 : {Def}" : $"방어력 : {Def + ExtraDef} (+{ExtraDef})");
            Console.WriteLine($"체력 : {Hp}/{MaxHp}");
            Console.WriteLine($"마나 : {Mp}/{MaxMp}");
            Console.WriteLine($"Gold : {Gold} G");
        }

        // 직업 DB # SetData()
        public enum JobType
        {
            전사 = 1,
            마법사,
            궁수,
            도적,
            성직자,
        }

        public class JobData
        {
            public int BaseAtk { get; }
            public int BaseDef { get; }
            public int BaseMaxHp { get; }
            public int BaseMaxMp { get; }

            public JobData(int atk, int def, int maxHp, int maxMp)
            {
                BaseAtk = atk;
                BaseDef = def;
                BaseMaxHp = maxHp;
                BaseMaxMp = maxMp;
            }
        }
        public static class JobDB
        {
            public static Dictionary<JobType, JobData> Jobs = new Dictionary<JobType, JobData>
            {   // 직업명 / 공격력 / 방어력 / 최대체력 / 최대마나
                { JobType.전사, new JobData(7, 8, 150, 50) },
                { JobType.마법사, new JobData(13, 2, 50, 150) },
                { JobType.궁수, new JobData(8, 7, 100, 100) },
                { JobType.도적, new JobData(10, 5, 80, 120) },
                { JobType.성직자, new JobData(5, 4, 125, 75) }
            };
        }


        // 경험치 획득
        public void GainExp()
        {
            while (Exp >= MaxExp)
            {
                Exp -= MaxExp;
                MaxExp += 10;
                Level++;

                if (Job == JobType.전사 || Job == JobType.궁수 || Job == JobType.도적)
                {
                    Atk += 1;
                    MaxHp += 10;
                    MaxMp += 5;
                    Hp += MaxHp;
                    Mp += MaxMp;
                }
                else
                {
                    Atk += 1;
                    MaxHp += 5;
                    MaxMp += 10;
                    Hp += MaxHp;
                    Mp += MaxMp;
                }
            }
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
            MaxHp += Hp;
        }

        // 아이템 강화 - 오류 발생 bool값으로 변경 시도 # Inventory.cs
        public bool UpgradeItem(Item item)
        {
            if (item.Value >= item.MaxValue) // 최대치 이상
            {
                return false;
            }

            if (item.Value < 10)
            {
                if (Gold < 100) return false;
                Gold -= 100;
                item.Value += 2;

                if (item.Type == 0) ExtraAtk += 2;
                else ExtraDef += 2;
                return true;
            }
            else if (item.Value >= 10) // 해당 값 이상
            {
                if (Gold < 200) return false;
                Gold -= 200;
                item.Value += 5;

                if (item.Type == 0) ExtraAtk += 5;
                else ExtraDef += 5;
                return true;
            }
            return false;
        }

        //피격 피해량 계산
        public void Damage(int amount)
        {
            int damage = amount - Def;

            damage = damage < 0 ? 0 : damage;

            Hp -= damage;

            Console.WriteLine($"{damage}의 데미지를 받았습니다!");

            if (Hp < 0)
            {
                Hp = 0;
                Console.WriteLine("게임을 종료합니다.");
                Thread.Sleep(1000);
                Environment.Exit(0);
            }
        }
    }
}