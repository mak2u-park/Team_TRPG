using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using Sparta_Dungeon_TeamProject;
using static System.Net.Mime.MediaTypeNames;

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
        public int Gold { get; set; }

        public int ExtraAtk { get; set; }
        public int ExtraDef { get; set; }

        public int FinalAtk => Atk + ExtraAtk; // 최종 공격력
        public int FinalDef => Def + ExtraDef; // 최종 방어력

        private List<Item> Inventory = new List<Item>();
        private List<Item> EquipList = new List<Item>();

        public List<SkillLibrary> Skills = new List<SkillLibrary>();

        public int InventoryCount
        {
            get
            {
                return Inventory.Count;
            }
        }

        public Player(int level, int exp, int maxExp, string name, JobType job, int atk, int def, int hp, int maxHp, int mp, int maxMp, int gold)
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

        // 경험치 획득 # Program.cs
        public void GainExp()
        {
            while (Exp >= MaxExp) // 레벨업
            {
                Exp -= MaxExp;
                MaxExp += 10;
                Level++;

                Console.Clear();
                Console.WriteLine();
                Console.WriteLine();
                string levelUpMessage = "쌓여온 경험이 당신을 한층 더 성장시켰습니다.";

                Console.ForegroundColor = ConsoleColor.Yellow;

                foreach (char c in levelUpMessage)
                {
                    Console.Write(c);
                    Thread.Sleep(80);
                }

                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine();
            }

        }

        // 직업 DB # SetData()
        public enum JobType
        {
            전사 = 1,
            마법사,
            과학자,
            대장장이,
            영매사,
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

        // 직업별 기본 능력치 DB # SetData()
        public static class JobDB
        {
            public static Dictionary<JobType, JobData> Jobs = new Dictionary<JobType, JobData>
            {   // 직업명 / 공격력 / 방어력 / 최대체력 / 최대마나
                { JobType.전사, new JobData(10, 10, 150, 100) },
                { JobType.마법사, new JobData(12, 5, 60, 150) },
                { JobType.과학자, new JobData(8, 10, 80, 200) },
                { JobType.대장장이, new JobData(5, 5, 60, 0) },
                { JobType.영매사, new JobData(10, 5, 80, 200) }
            };
        }

        // 직업별 기본 스킬 지급 # SetData()


        public void DisplaySkillUI()
        {
            Console.Clear();
            Console.WriteLine("스킬 관리");
            Console.WriteLine("이곳에서 캐릭터의 스킬을 관리할 수 있습니다.\n");
            Console.WriteLine("[스킬 목록]");

            ShowSkillList(false);

            Console.WriteLine("\n1. 장착 관리");
            Console.WriteLine("0. 나가기");
            Console.Write("\n원하시는 행동을 입력해주세요 >> ");

            int choice = Program.CheckInput(0, 2);
            switch (choice)
            {
                case 1:
                    DisplayEquipSkill();
                    break;
                case 0:
                    Program.DisplayMainUI();
                    break;
            }
        }

        public void DisplayEquipSkill()
        {
            Console.Clear();
            Console.WriteLine("스킬관리 - 장착 관리");
            Console.WriteLine("이곳에서 캐릭터의 스킬을 장착할 수 있습니다.\n");
            Console.WriteLine("[스킬 목록]");

            ShowSkillList(true);

            Console.WriteLine("\n0. 돌아가기");
            Console.Write("장착할 스킬 번호를 입력하세요 >> ");

            int input = Program.CheckInput(0, Skills.Count);

            switch (input)
            {

                case 0:
                    DisplaySkillUI();
                    break;
                default:
                    GameSkill selectedSkill = Skills[input - 1];

                    if (EquipSkillList.Contains(selectedSkill))
                    {
                        EquipSkillList.Remove(selectedSkill);
                        Console.WriteLine($"'{selectedSkill.Name}' 스킬을 해제했습니다.");
                    }
                    else
                    {
                        EquipSkillList.Add(selectedSkill);
                        Console.WriteLine($"'{selectedSkill.Name}' 스킬을 장착했습니다.");
                    }
                    Thread.Sleep(4000);
                    DisplayEquipSkill();
                    break;
            }
        }

        // 스킬 목록 출력 # Program.cs
        public void ShowSkillList(bool showIdx)
        {
            for (int i = 0; i < Skills.Count; i++)
            {
                GameSkill targetSkill = Skills[i];

                string displayIdx = showIdx ? $"{i + 1} " : "";
                string displayEquipped = IsEquippedSkill(targetSkill) ? "[E]" : "";
                Console.WriteLine($"- {displayIdx} {displayEquipped} {targetSkill.Name}" +
                $" ( 소모: {targetSkill.Cost} / 쿨타임: {targetSkill.CoolTime} / {targetSkill.Desc} )");

            }
        }
        public bool IsEquippedSkill(GameSkill skill)
        {
            return EquipSkillList.Contains(skill);
        }

        // 보유 스킬 카운팅 # Program.cs
        public int SkillListCount
        {
            get
            {
                return Skills.Count;
            }
        }
        
        // 스킬 장착 # Program.cs
        public void EquipSkill(GameSkill AllSkills)
        {
            if (IsEquippedSkill(AllSkills))
            {
                EquipSkillList.Remove(AllSkills);
                Console.WriteLine($"{AllSkills.Name} 스킬 장착 해제");
            }
            else
            {
                EquipSkillList.Add(AllSkills);
                Console.WriteLine($"{AllSkills.Name} 스킬 장착 완료");
            }
        }

        // 스킬 장착 목록 # Program.cs
        public List<GameSkill> GetListSkill()
        {
            return Skills;
        }

        public void AddGold(int amount) // 골드를 추가해주는 매서드
        {
            Gold += amount;
        }

        public void GainReward(int gold, int exp) // 최종적인 보상
        {
            AddGold(gold);
            Exp += exp;
            GainExp();
        }

        // 인벤토리 아이템목록 # Inventory.cs
        public void InventoryItemList(bool showIdx)
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                Item targetItem = Inventory[i];

                // 인벤토리 아이템 출력
                string displayIdx = showIdx ? $"{i + 1} " : "";
                string displayEquipped = IsEquipped(targetItem) ? "[E]" : "";

                // 아이템 강화 여부에 따른 출력
                string enhanceText = targetItem.Value == targetItem.MaxValue ? " (최대치)" : "";
                string statText = $"+({targetItem.Value})"; // 강화 수치
                string typeText = targetItem.Type == 0 ? "공격력" : "방어력";

                // 아이템 정보 출력: 이름, 타입,
                Console.WriteLine($"- {displayIdx}{displayEquipped}{targetItem.ItemEnhanceText()}");
            }
        }

        // 장비 착용여부 # Inventory.cs
        public void EquipItem(Item item)
        {
            if (IsEquipped(item))
            {
                EquipList.Remove(item);
                if (item.Type == 0) ExtraAtk -= item.Value;
                else ExtraDef -= item.Value;
            }
            else
            {
                EquipList.Add(item);
                if (item.Type == 0) ExtraAtk += item.Value;
                else ExtraDef += item.Value;
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

        public void Rest()
        {
            Gold -= 500;
            MaxHp += Hp;
        }

        // 아이템 강화 # Inventory.cs 에서 호출을 위해 분리
        public int GetUpgradeCost(Item item)
        {
            return item.Value < 20 ? 100 : 200;
        }
        public int GetUpgradeValue(Item item)
        {
            return item.Value < 20 ? 5 : 10;
        }

        // 아이템 강화 # Inventory.cs
        public bool UpgradeItem(Item item)
        {
            int cost = GetUpgradeCost(item);
            int valueUp = GetUpgradeValue(item);

            if (item.Value >= item.MaxValue) // 최대치 이상
            {
                return false;
            }

            if (Gold < cost) // 골드 부족
            {
                return false;
            }

            Gold -= cost; // 골드 차감
            item.Value += valueUp; // 아이템 능력치 증가

            //장착 스탯 반영
            if (IsEquipped(item))
            {
                if (item.Type == 0) ExtraAtk += valueUp;
                else ExtraDef += valueUp;
            }
            return true;
        }

        //피격 피해량 계산
        public void EnemyDamage(int amount)
        {
            int damage = amount - Def;

            damage = damage < 0 ? 1 : damage;

            Hp -= damage;

            Console.WriteLine($"{damage}의 데미지를 받았습니다!");

            if (Hp < 0)
            {
                Hp = 0;
                Console.WriteLine("사망하셨습니다.");
                Thread.Sleep(1000);
                Environment.Exit(0);
            }
        }

        public void Heal(int amount)//체력회복 메서드
        {
            if (Hp + amount <= 0) //계산된 체력이 0이하면 Hp10남기도록설정
            {
                Hp = 10;
            }
            else if (Hp + amount > MaxHp)//계산결과가 최대체력보다크면 최대체력으로 설정
            {
                Hp = MaxHp;
            }
            else
            {
                Hp += amount;
            }

        }

        public void DefUP(int num)
        {
            num += ExtraDef;
        }

        public void UP(int num)
        {
            num += ExtraDef;
        }

        public void SelectRemove(string name)//아이템을 찾아서 삭제하는 메서드
        {
            foreach (var item in Inventory)
            {
                if (item.Name == name)
                {
                    Inventory.Remove(item);
                    break;
                }
            }
        }

    }
}