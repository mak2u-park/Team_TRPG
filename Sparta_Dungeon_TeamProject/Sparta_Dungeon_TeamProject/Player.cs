using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using static Sparta_Dungeon_TeamProject.Program;

namespace Sparta_Dungeon_TeamProject
{
    // 플레이어 클래스
    public class Player
    {
        private static Player _instance;
        private static Inventory inventory;
        private readonly List<Item> equippedItems = new();
        private static Messages messages = new Messages();

        public Player() { }
        public static Player Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Player();
                }
                return _instance;
            }
        }
        Battles battles = new Battles();

        public string Name { get; private set; }
        public JobType Job { get; private set; }
        public int Level { get; private set; }
        public int Exp { get; private set; }
        public int MaxExp { get; private set; }

        // 기본 스탯 및 최대 Hp/Mp
        public int Atk { get; private set; }
        public int Cri { get; private set; }
        public int Def { get; private set; }
        public int Acc { get; private set; }
        public int CriDmg { get; private set; } // 치명타 피해

        public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public int MaxMp { get; private set; }
        public int Mp { get; private set; }
        public int Gold { get; set; }

        // 장착여부에 따른 추가스탯
        public int ExtraAtk { get; private set; }
        public int ExtraDef { get; private set; }

        // 총 스탯 합계
        public int FinalAtk => Atk + inventory.GetTotalAtkBonus();
        public int FinalDef => Def + inventory.GetTotalDefBonus();
        public int FinalMaxHp => MaxHp + inventory.GetTotalHpBonus();
        public int FinalMaxMp => MaxMp + inventory.GetTotalMpBonus();

        public int Chapter { get; set; } = 0;
        public int Stage { get; set; } = 0;

        // 보유 스킬 목록 (초기보상스킬은 Job에서)
        public List<SkillLibrary> Skills { get; private set; } = new List<SkillLibrary>();

        private static Random rand = new Random();

        public Player(string name, IJob job)
        {
            Name = name;
            Job = job.Type;
            Level = 1;
            Exp = 0; // 경험치 초기화

            // 직업별 기본 스탯 초기화
            MaxExp = job.ExpToLevelUp; // 첫 레벨업 경험치 - 모두통일도 가능. 수정 어렵지 않음.
            Atk = job.Atk;
            Acc = job.Acc;
            Cri = job.Cri;
            CriDmg = job.CriDmg;
            Def = job.Def;
            MaxHp = job.MaxHp;
            Hp = MaxHp;
            MaxMp = job.MaxMp;
            Mp = MaxMp;

            Gold = job.DefaultGold;
        }

        // 장비 착용
        public void EquipItem(Item item)
        {
            if (IsEquipped(item))
                return;

            equippedItems.Add(item);
            RetrunEquipStats();
        }

        // 장비 해제
        public void UnequipItem(Item item)
        {
            if (!IsEquipped(item))
                return;

            equippedItems.Remove(item);
            RetrunEquipStats();
        }

        // 장비 착용 여부 확인
        public bool IsEquipped(Item item)
        {
            return equippedItems.Contains(item);
        }

        // 장착 확인 후, 스탯 반영
        public void RetrunEquipStats()
        {
            ExtraAtk = 0;
            ExtraDef = 0;

            foreach (var item in equippedItems)
            {
                ExtraAtk += item.AtkBonus;
                ExtraDef += item.DefBonus;
            }
        }

        // 레벨업 로직
        public void GainExp()
        {
            while (Exp >= MaxExp) // 레벨업
            {

                Exp -= MaxExp;
                MaxExp = JobDatas[Job].ExpToLevelUp;
                MaxExp += 25;
                Level++;
                Hp = MaxHp; // 레벨업 시 체력 증가
                ExtraAtk += 1;
                ExtraDef += 1;

                Console.Clear();
                Console.WriteLine();
                string levelUpMessage = "\n\n\n\n\n    쌓여온 경험이 당신을 한층 더 성장시켰습니다.\n\n\n\n\n";

                messages.StartSkipListener(); // 스킵 감지 시작

                Console.ForegroundColor = ConsoleColor.Yellow;
                messages.PrintMessageWithSkip(levelUpMessage, 80); // 스킵 가능한 출력
                if (messages.Skip)
                {
                    Console.Clear(); // 스킵되었을 경우 화면 정리
                }
                Console.ResetColor();

                if (!messages.Skip)
                {
                    Thread.Sleep(800); // 스킵되지 않은 경우에만 약간 멈춤
                }

                messages.Skip = false; // 초기화
                Console.WriteLine();
                Console.WriteLine();

               /*if (Level % 5 == 0) // 5레벨마다 새로운 스킬 획득
                {
                    SkillManager.LearnSkill(this);
                }*/
            }
        }

        // 경험치 획득 - 전투/이벤트 보상 받는용
        public void GainReward(int gold, int exp)
        {
            Gold += gold;
            Exp += exp;
            GainExp();
        }

        // 체력 회복 (여관, 포션, 이벤트 등)
        public bool Heal(int cost, int amount)
        {
            if (cost > 0)
            {
                if (Gold < cost || Hp >= MaxHp) return false;
                Gold -= cost;
            }

            //int newHp = Hp + amount;
            int healAmount = amount;

            if (Job == JobType.영매사) // 영매사는 회복량 절반
            {
                healAmount /= 2;
            }

            Hp += healAmount;
            if (Hp <= 0)
            {
                Hp = 10;
            }
            else if (Hp > MaxHp)
            {
                Hp = MaxHp;
            }
            return true;
            //if (newHp <= 0)
            //{
            //    newHp = 10;
            //}
            //else if (newHp > MaxHp)
            //{
            //    newHp = MaxHp;
            //}

            //Hp = newHp;
            //return true;
        }

        // 마나 회복 (여관, 포션, 이벤트 등)
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


        // 플레이어 공격
        public void PlayerAttack(Monster target, double power)
        {
            if (IsHit(target))
            {
                bool isCritical = IsCritical();
                double multiplier = DamageSpread();

                double attackDamage = isCritical ? FinalAtk * power * (CriDmg / 100.0) : FinalAtk * power;
                int finalAttackDamage;

                if (Job == JobType.마법사) // 마법사는 방어력 절반 무시
                {
                    int ignoredDef = target.Def / 2; // 무시한 방어력 계산
                    finalAttackDamage = (int)Math.Ceiling(attackDamage * multiplier - ignoredDef);

                    Console.WriteLine($"\n\n\n{"",10}마법사의 주문! {ignoredDef} 만큼의 방어력을 무시했습니다!");
                }
                else
                {
                    finalAttackDamage = (int)Math.Ceiling(attackDamage * multiplier - target.Def);
                }

                if (Job == JobType.대장장이) // 대장장이의 공격이 명중할 때마다 적의 방어력 감소
                {
                    Random random = new Random();
                    int armorBreak = random.Next(0, 4); // 0 ~ 3 까지 감소

                    if (armorBreak > 0)
                    {
                        int beforeDef = target.Def;
                        target.Def -= armorBreak;

                        if (target.Def < 0)
                        {
                            target.Def = 0; // 방어력은 0 밑으로 떨어지지 않게
                        }

                        int decreasedAmount = beforeDef - target.Def;

                        Console.WriteLine($"\n\n\n{"",10}대장장이가 [Lv.{target.Level}][{target.Name}]에 방어력을 {armorBreak}만큼 감소 시켰습니다!");
                        Console.WriteLine($"\n\n\n{"",10}현재  [Lv.{target.Level}][{target.Name}]의 방어력은 {target.Def}입니다!");
                    }
                }

                if (Job == JobType.전사) // 전사 특성
                {
                    if (target.Name == "카피바라" || target.Name == "후회하는 모험가" || target.Name == "대왕 카피바라" || target.Name == "검은 고양이")
                    {
                        int originalDamage = finalAttackDamage; // 기본 공격 데미지

                        finalAttackDamage = (int)(finalAttackDamage * 1.3); // 보스에게 30% 추가 피해

                        int addedDamage = finalAttackDamage - originalDamage; // 추가된 피해량 계산

                        Console.WriteLine($"\n\n\n{"",10}전사의 강공격! 기본 피해 {originalDamage} + 추가 피해 {addedDamage}의 피해를 입혔다!");
                    }
                }
                if (Job == JobType.연금술사) // 연금술사 특성
                {
                    int HpDamage = (int)(target.CurrentHp * 0.2); // 현재 체력의 20% 추가 피해
                    int originalDamage = finalAttackDamage; // 기본 피해

                    finalAttackDamage += HpDamage; // 최종 데미지 계산

                    Console.WriteLine($"\n\n\n{"",10}연금술사의 공격! 기본 피해 {originalDamage} + 추가 피해 {HpDamage}의 피해를 입혔다!");
                }

                finalAttackDamage = Math.Max(1, finalAttackDamage); // 최소 데미지 1 보장
                target.CurrentHp -= finalAttackDamage;

                Console.WriteLine();

                if (isCritical)
                {
                    messages.CriticalMes(this);
                }
                Console.WriteLine($"\n\n{"",10}[Lv.{target.Level}][{target.Name}] 에게 {finalAttackDamage}만큼 피해를 입혔다!");
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"\n\n\n{"",10}[Lv.{target.Level}][{target.Name}] 에게 공격이 빗나갔다!");
            }

            Console.WriteLine($"\n\n\n{"",10}▶ [Enter] 키를 눌러 다음으로 넘어가세요.");
            Utils.WaitForEnter();
            Console.Clear();
        }

        // 영매사 유령 공격
        public void SpiritAttack(Monster target)
        {
            Random random = new Random();
            int SpiritCount = random.Next(1, 4); // 1 ~ 3명의 유령 소환

            Console.WriteLine($"\n\n\n{"",10}{SpiritCount}명의 영혼이 당신을 돕습니다.");

            int totalDamage = 0; // 전체 누적 데미지

            for (int i = 0; i < SpiritCount; i++)
            {
                int SpiritDamage = Level * 1;

                if (target.CurrentHp - SpiritDamage <= 0)
                {
                    SpiritDamage = (int)target.CurrentHp - 1;
                    if (SpiritDamage < 0) SpiritDamage = 0; // 체력이 1 이하면 0 데미지
                }

                target.CurrentHp -= SpiritDamage;
                totalDamage += SpiritDamage; // 누적
                if (target.CurrentHp <= 1) break; // 체력 1 이하면 추가 공격 금지
            }

            Console.WriteLine($"\n{"",10}의문의 영혼 {SpiritCount}명이 [Lv.{target.Level}][{target.Name}] 에게 총 {totalDamage}만큼 피해를 주었다!");

            Console.WriteLine($"\n\n\n{"",10}▶ [Enter] 키를 눌러 다음으로 넘어가세요.");
            Utils.WaitForEnter();
            Console.Clear();
        }
        public bool IsHit(Monster target)
        {
            int hit = Acc - target.Dodge;
            return rand.Next(0, 100) <= hit;
        }
        public bool IsCritical() // 플레이어 치명타 여부
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

            if (Hp <= 0)
            {
                Hp = 0;
                CheckPlayerDead();
            }
        }

        public void CheckPlayerDead()
        {
            if (Hp <= 0)
            {
                Battles battles = new Battles();
                battles.BattleFailUI(); // Battles 클래스의 BattleFailUI 호출
            }
        }

        public void boarDamage(bool choice)
        {
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



        // 스탯 강화 스킬 (활용예시:   player.AtkUP(5);   // 공격력 +5)
        public void AtkUP(int value)
        {
            ExtraAtk += value;
        }
        public void DefUP(int value)
        {
            ExtraDef += value;
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

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"  체력 : {Hp}/{MaxHp}");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("  공격력 : ");
            Console.WriteLine($"{Atk}{(ExtraAtk == 0 ? "" : $" (+{ExtraAtk})")}");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("  방어력 : ");
            Console.WriteLine($"{Def}{(ExtraDef == 0 ? "" : $" (+{ExtraDef})")}");
            Console.ResetColor();
            Console.WriteLine();

            Console.Write($"  명중률 :{Acc}%");
            Console.WriteLine();

            Console.Write($"  치명타 확률 :{Cri}%");
            Console.WriteLine();

            Console.Write($"  치명타 피해 :{CriDmg}%");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"  마나 : {Mp}/{MaxMp}");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\n  Gold : {Gold} G");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("\n[1] 스킬 관리");
            Console.WriteLine("[~`] 나가기");
            Console.Write("\n원하시는 행동을 입력해주세요\n>> ");
            Console.WriteLine();

            int result = Utils.CheckInput(-1, 1);
            switch (result)
            {
                case -1:
                    return;
                case 1:
                    DisplaySkillUI();
                    return;
            }
        }

        // 2. 스킬 UI 주석
        public void DisplaySkillUI()
        {
            Console.Clear();
            Console.WriteLine("[스킬 목록]");

            //ShowSkillList();

            Console.WriteLine("\n[1] 스킬 장착하기");
            Console.WriteLine("[~`] 나가기");
            Console.Write("\n원하시는 행동을 입력해주세요\n>> ");

            int choice = Utils.CheckInput(1, 1);
            switch (choice)
            {
                case -1:
                    return;
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

        // 2-2. 스킬 수 반환
        public int SkillListCount
        {
            get
            {
                return Skills.Count;
            }
        }

        // 강화 비용 계산 - 플레이어 정보 반영 문제로 player에서 사용
        public int UpgradeCost(Item item)
        {
            return item.TotalValue < 20 ? 100 : 200;
        }

        // 강화 성공 시 능력치 증가량
        public int UpgradeValue(Item item)
        {
            if (Job == JobType.대장장이)
            {
                return item.TotalValue < 20 ? 75 : 150;
            }
            else
            {
                return item.TotalValue < 20 ? 100 : 200;
            }
        }
        public int GetUpgradeValue(Item item)
        {
            return item.TotalValue < 20 ? 5 : 10;
        }

        // 최대치 초과시 골드 차감 방지
        public bool UpgradeItem(Item item)
        {
            if (Gold < UpgradeCost(item) || item.TotalValue >= item.MaxValue)
            {
                return false;
            }

            Gold -= UpgradeCost(item);
            item.TotalValue += UpgradeValue(item);

            if (item.TotalValue > item.MaxValue)
            {
                item.TotalValue = item.MaxValue;
            }

            if (IsEquipped(item))
            {
                ExtraAtk += item.Type == 0 ? UpgradeValue(item) : 0;
                ExtraDef += item.Type == 1 ? UpgradeValue(item) : 0;
            }

            return true;
        }

        // 강화 시 스탯 반영 (호출용)
        public void UpgradeStat(Item item, int valueUp)
        {
            if (!IsEquipped(item))
                return;

            if (item.Type == 0)
                ExtraAtk += valueUp;
            else if (item.Type == 1)
                ExtraDef += valueUp;
        }


        //던전 이벤트
        public void BuyItem(Item item)
        {
            inventory.AddItem(item);
        }

        public bool HasItem(Item item)
        {
            return inventory.HasItem(item);
        }
    }
}