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
        public int Cri { get; private set; }
        public int Def { get; private set; }
        public int Hp { get; private set; }
        public int MaxHp { get; private set; } = 100;
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
        public List<SkillLibrary> EquipSkillList = new List<SkillLibrary>();

        public int InventoryCount
        {
            get
            {
                return Inventory.Count;
            }
        }

        public Player(int level, int exp, int maxExp, string name, JobType job, int atk, int cri, int def, int hp, int maxHp, int mp, int maxMp, int gold)
        {
            Level = level;
            Exp = exp;
            MaxExp = maxExp;
            Name = name;
            Job = job;
            Atk = atk;
            Cri = cri;
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
            Console.Clear();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("┏━━━━━━━━━━━━━━<< 여정의 기록 >>━━━━━━━━━━━━━━┓");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"  Lv. {Level:D2}  {{ {Exp}/{MaxExp} }}");
            Console.ResetColor();

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"  {{ {Job} }}  {Name}");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("  공격력 : ");
            if (ExtraAtk == 0)
                Console.WriteLine($"{Atk}");
            else
                Console.WriteLine($"{Atk + ExtraAtk} (+{ExtraAtk})");

            Console.WriteLine();

            Console.Write("  방어력 : ");
            if (ExtraDef == 0)
                Console.WriteLine($"{Def}");
            else
                Console.WriteLine($"{Def + ExtraDef} (+{ExtraDef})");
            Console.ResetColor();

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"  체력 : {Hp}/{MaxHp}");
            Console.ResetColor();

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"  마나 : {Mp}/{MaxMp}");
            Console.ResetColor();

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"  Gold : {Gold} G");
            Console.ResetColor();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.ResetColor();
            Console.WriteLine();
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
                string levelUpMessage = "\n\n\n\n\n    쌓여온 경험이 당신을 한층 더 성장시켰습니다.\n\n\n\n\n";

                Messages.StartSkipListener(); // 스킵 감지 시작

                Console.ForegroundColor = ConsoleColor.Yellow;
                Messages.PrintMessageWithSkip(levelUpMessage, 80); // 스킵 가능한 출력
                if (Messages.Skip)
                {
                    Console.Clear(); // 스킵되었을 경우 화면 정리
                }
                Console.ResetColor();

                if (!Messages.Skip)
                {
                    Thread.Sleep(800); // 스킵되지 않은 경우에만 약간 멈춤
                }

                Messages.Skip = false; // 초기화
                Console.WriteLine();
                Console.WriteLine();

                if (Level % 5 == 0) // 5레벨마다 새로운 스킬 획득
                {
                    SkillManager.LearnSkill(this);
                }
            }
        }

        public void PlayerAttack(Monster target, double power)
        {
            bool isCritical = IsCritical();
            double multiplier = DamageSpread();

            double attackDamage = isCritical ? FinalAtk * power * 1.5: FinalAtk * power;
            int finalAttackDamage = (int)Math.Ceiling(attackDamage * multiplier - target.Def); // 몬스터 방어력만큼 최종 데미지 감소
            finalAttackDamage = Math.Max(1, finalAttackDamage); // 최소 데미지 1

            target.Hp -= finalAttackDamage;

            if (this.IsCritical())
            {
                Messages.CriticalMes(this);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"{"",10}[Lv.{target.Level}][{target.Name}] 에게 {finalAttackDamage}만큼 피해를 입혔다!");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"{"",10}▶ 아무 키나 눌러 다음으로 넘어가세요.");
            Console.ReadKey();
            Console.Clear();
        }

        private static Random rand = new Random();

        public bool IsCritical() // 플레이어 치명타
        {
            return rand.Next(0, 100) < Cri; // 치명타 확률
        }

        public double DamageSpread() // 공격 시 피해량 0.9 ~ 1.1 랜덤 설정
        {
            return rand.NextDouble() * 0.2 + 0.9; 
        }

        public void DisplaySkillUI()
        {
            Console.Clear();
            Console.WriteLine("[스킬 목록]");

            ShowSkillList();

            Console.WriteLine("\n0. 나가기");
            Console.Write("\n원하시는 행동을 입력해주세요 >> ");

            int choice = Program.CheckInput(0, 1);
            switch (choice)
            {
                case 0:
                    Program.DisplayMainUI();
                    break;
            }
        }

        // 스킬 목록 출력 # Program.cs
        public void ShowSkillList()
        {
            for (int i = 0; i < Skills.Count; i++)
            {
                SkillLibrary targetSkill = Skills[i];

                string displayIdx = $"{i + 1}";
                Console.WriteLine($"- {displayIdx} {targetSkill.Name}" +
                $" : {targetSkill.Desc} (소모 값: {targetSkill.Cost} / 쿨타임: {targetSkill.Cool})");

            }
        }

        // 보유 스킬 카운팅 # Program.cs
        public int SkillListCount
        {
            get
            {
                return Skills.Count;
            }
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
            Hp = MaxHp;
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

            Console.WriteLine();
            Console.WriteLine($"    {damage}의 데미지를 받았습니다!");
            Console.WriteLine();


            if (Hp <= 0)
            {
                Hp = 0;

                Program.BattleFailUI();
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