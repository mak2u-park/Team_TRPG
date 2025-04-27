using System;
using System.Collections.Generic;
using static Sparta_Dungeon_TeamProject.Program;

namespace Sparta_Dungeon_TeamProject
{
    // 플레이어 클래스
    public class Player
    {
        public string Name { get; }
        public JobType Job { get; }
        public int Level { get; private set; }
        public int Exp { get; private set; }
        public int MaxExp { get; private set; }

        // 기본 스탯 및 최대 Hp/Mp
        public int Atk { get; private set; }
        public int Cri { get; private set; }
        public int Def { get; private set; }
        public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public int MaxMp { get; private set; }
        public int Mp { get; private set; }
        public int Gold { get; private set; }

        // 장비에 따라 추가되는 스탯
        public int ExtraAtk { get; set; }
        public int ExtraDef { get; set; }

        // 최종 계산된 스탯
        public int FinalAtk => Atk + ExtraAtk;
        public int FinalDef => Def + ExtraDef;

        // 보유 스킬 목록 (초기보상스킬은 Job에서)
        public List<SkillLibrary> Skills { get; private set; } = new();

        public Player(string name, IJob job)
        {
            Name = name;
            Job = job.Type;
            Level = 1;
            Exp = 0; // 경험치 초기화

            // 직업별 기본 스탯 초기화
            MaxExp = job.ExpToLevelUp; // 첫 레벨업 경험치 - 모두통일도 가능. 수정 어렵지 않음.
            Atk = job.Atk;
            Cri = job.Cri;
            Def = job.Def;
            MaxHp = job.MaxHp;
            Hp = MaxHp;
            MaxMp = job.MaxMp;
            Mp = MaxMp;

            Gold = job.DefaultGold;
        }

        // 1. 상태보기
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
            Console.WriteLine($"{Atk}{(ExtraAtk == 0 ? "" : $" (+{ExtraAtk})")}");
            Console.WriteLine();

            Console.Write("  방어력 : ");
            Console.WriteLine($"{Def}{(ExtraDef == 0 ? "" : $" (+{ExtraDef})")}");
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

            int choice = Program.CheckInput(1, 1);
            switch (choice)
            {
                case -1:
                    Messages.ShowMainMenu();
                    break;
                case 1:
                    DisplaySkillUI();
                    break;
            }
        }

        // 2. 스킬 UI
        public void DisplaySkillUI()
        {
            Console.Clear();
            Console.WriteLine("[스킬 목록]");

            ShowSkillList();

            Console.WriteLine("\n[1] 스킬 장착하기");
            Console.WriteLine("[~`] 나가기");
            Console.Write("\n원하시는 행동을 입력해주세요 >> ");

            int choice = Program.CheckInput(1, 1);
            switch (choice)
            {
                case -1:
                    Messages.ShowMainMenu();
                    break;
                case 1:
                    Console.Clear();
                    Console.WriteLine("스킬 사용 준비중입니다.");
                    Thread.Sleep(1000);
                    break;
            }
        }

        // 2-1. 스킬목록 출력값
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

        // 스킬 수 반환
        public int SkillListCount
        {
            get
            {
                return Skills.Count;
            }
        }

        // 경험치 획득
        public void GainExp()
        {
            while (Exp >= MaxExp) // 레벨업
            {
                Exp -= MaxExp;
                MaxExp = JobDatas[Job].ExpToLevelUp;
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

        public void GainReward(int gold, int exp)
        {
            Gold += gold;
            Exp += exp;
            GainExp();
        }

        public void PlayerAttack(Monster target, double power)
        {
            bool isCritical = IsCritical();
            double multiplier = DamageSpread();

            double attackDamage = isCritical ? FinalAtk * power * 1.5 : FinalAtk * power;
            int finalAttackDamage = (int)Math.Ceiling(attackDamage * multiplier - target.Def); // 몬스터 방어력만큼 최종 데미지 감소
            finalAttackDamage = Math.Max(1, finalAttackDamage); // 최소 데미지 1

            target.CurrentHp -= finalAttackDamage;

            Console.Clear();
            Console.WriteLine();

            if (this.IsCritical())
            {
                Messages.CriticalMes(this);
            }
            Console.WriteLine($"\n\n\n{"",10}[Lv.{target.Level}][{target.Name}] 에게 {finalAttackDamage}만큼 피해를 입혔다!");
            Console.WriteLine($"\n\n\n{"",10}▶ [Enter] 키를 눌러 다음으로 넘어가세요.");
            Program.WaitForEnter();
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

        //피격 피해량 계산
        public void EnemyDamage(int amount)
        {
            int damage = amount - Def;
            damage = damage < 0 ? 1 : damage;
            Hp -= damage;

            Console.WriteLine();
            Console.WriteLine($"    {damage}의 데미지를 받았습니다!");
            Console.WriteLine();

            CheckPlayerDead();
            
        }

        public void boarDamage(bool choice)
        {
            // 아무런 방향을 선택하지 않은 경우
            if (choice)
            {
                // 플레이어에게 최대체력의 10%의 데미지를 입힘
                Hp -= Hp / 10;
            }
            // 멧돼지의 돌진 방향을 틀린 경우
            else
            {
                // 플레이어에게 최대체력의 20%의 데미지를 입힘
                Hp -= Hp / 5;
            }
        }

        public void CheckPlayerDead()
        {
            if (Hp <= 0)
            {
                Hp = 0;
                Program.BattleFailUI();
            }
        }

        // 방어력 증가 - 스킬 사용 코드 (활용예시:   player.DefUP(5);   // 방어력 +5)
        public void DefUP(int value)
        {
            ExtraDef += value;
        }

        // 공격력 증가 - 스킬 사용 코드 (활용예시:   player.AtkUP(5);   // 공격력 +5)
        public void AtkUP(int value)
        {
            ExtraAtk += value;
        }

        // 체력 회복
        public bool Heal(int cost, int amount)
        {
            if (cost > 0)
            {
                if (Gold < cost || Hp >= MaxHp) return false;
                Gold -= cost;
            }

            int newHp = Hp + amount;

            if (newHp <= 0)
            {
                newHp = 10;
            }
            else if (newHp > MaxHp)
            {
                newHp = MaxHp;
            }

            Hp = newHp;
            return true;
        }

        // 마나 회복
        public bool GainMp(int cost, int amount)
        {
            if (Gold >= cost && Mp < MaxMp)
            {
                Gold -= cost;
                Mp += amount;
                if (Mp > MaxMp) Mp = MaxMp; // 최대 마나 초과 방지
                return true;
            }
            return false;
        }

        // 인벤토리 아이템 목록 출력
        private readonly List<Item> _equippedItems = new();

        public bool IsEquipped(Item item)
        {
            return _equippedItems.Contains(item);
        }

        // 장착 장비 기준, 스탯 재계산
        public void RefreshEquipStats()
        {
            ExtraAtk = 0;
            ExtraDef = 0;

            foreach (var item in _equippedItems)
            {
                ExtraAtk += item.AtkBonus;
                ExtraDef += item.DefBonus;
            }
        }

        // 장비 착용 + 효과 적용
        public void EquipItem(Item item)
        {
            if (_equippedItems.Contains(item))
                return;

            _equippedItems.Add(item);
            RefreshEquipStats();
        }

        // 장비 해제 + 효과 제거
        public void UnequipItem(Item item)
        {
            if (!_equippedItems.Contains(item))
                return;

            _equippedItems.Remove(item);
            RefreshEquipStats();
        }

        // 소모품 사용
        public void UseItem(Item item)
        {
            if (item.Type == 2 && item.HpBonus > 0) // 소모품
            {
                Heal(item.Price, item.HpBonus);
            }
            if (item.Type == 2 && item.MpBonus > 0) // 소모품
            {
                Heal(item.Price, item.MpBonus);
            }

            Inventory.RemoveItem(item);
        }

        // 인벤토리 아이템 목록 반환
        public List<Item> GetInventoryItems() => new List<Item>(Inventory.GetItems());

        // 구매/판매/보유 아이템 확인
        public void BuyItem(Item item)
        {
            Gold -= item.Price;
            Inventory.AddItem(item);
        }

        // 구매 아이템
        public bool HasItem(Item item)
        {
            return GetInventoryItems().Contains(item);
        }

        // 판매 아이템
        public void SellItem(Item item)
        {
            if (IsEquipped(item))
            {
                UnequipItem(item); // 장착 해제
            }

            int gainGold = (int)(item.Price * 0.85);
            Gold += gainGold;
            Inventory.RemoveItem(item); // 인벤토리에서 제거
            
            ExtraAtk -= item.AtkBonus; // 능력치 감소
            ExtraDef -= item.DefBonus;
            if (ExtraAtk < 0) ExtraAtk = 0;
            if (ExtraDef < 0) ExtraDef = 0;
        }

        // 강화 시 장착 아이템 능력치 반영
        public void GetUpgradeStat(Item item, int valueUp)
        {
            if (!IsEquipped(item))
                return;

            switch (item.Type)
            {
                case 0: // 무기
                    ExtraAtk += valueUp;
                    break;
                case 1: // 방어구
                    ExtraDef += valueUp;
                    break;
                case 2: // 소모품 (스탯X)
                    break;
                case 3: // 장신구 등 (미정)
                    break;
            }
        }

        // 아이템 강화 # Inventory.cs 에서 호출을 위해 분리
        public int GetUpgradeCost(Item item)
        {
            return item.TotalValue < 20 ? 100 : 200;
        }
        public int GetUpgradeValue(Item item)
        {
            return item.TotalValue < 20 ? 5 : 10;
        }

        // 아이템 강화 # Inventory.cs
        public bool UpgradeItem(Item item)
        {
            int cost = GetUpgradeCost(item);
            int valueUp = GetUpgradeValue(item);

            if (item.TotalValue >= item.MaxValue) // 최대치 이상
            {
                return false;
            }

            if (Gold < cost) // 골드 부족
            {
                return false;
            }

            Gold -= cost; // 골드 차감
            item.TotalValue += valueUp; // 아이템 능력치 증가

            //장착 스탯 반영
            if (IsEquipped(item))
            {
                if (item.Type == 0)
                {
                    ExtraAtk += valueUp;
                }
                else
                {
                    ExtraDef += valueUp;
                }
            }
            return true;
        }

    }
}