using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using static Sparta_Dungeon_TeamProject.Program;

namespace Sparta_Dungeon_TeamProject
{
    // �÷��̾� Ŭ����
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

        // �⺻ ���� �� �ִ� Hp/Mp
        public int Atk { get; private set; }
        public int Cri { get; private set; }
        public int Def { get; private set; }
        public int Acc { get; private set; }
        public int CriDmg { get; private set; } // ġ��Ÿ ����

        public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public int MaxMp { get; private set; }
        public int Mp { get; private set; }
        public int Gold { get; set; }

        // �������ο� ���� �߰�����
        public int ExtraAtk { get; private set; }
        public int ExtraDef { get; private set; }

        // �� ���� �հ�
        public int FinalAtk => Atk + inventory.GetTotalAtkBonus();
        public int FinalDef => Def + inventory.GetTotalDefBonus();
        public int FinalMaxHp => MaxHp + inventory.GetTotalHpBonus();
        public int FinalMaxMp => MaxMp + inventory.GetTotalMpBonus();

        public int Chapter { get; set; } = 0;
        public int Stage { get; set; } = 0;

        // ���� ��ų ��� (�ʱ⺸��ų�� Job����)
        public List<SkillLibrary> Skills { get; private set; } = new List<SkillLibrary>();

        private static Random rand = new Random();

        public Player(string name, IJob job)
        {
            Name = name;
            Job = job.Type;
            Level = 1;
            Exp = 0; // ����ġ �ʱ�ȭ

            // ������ �⺻ ���� �ʱ�ȭ
            MaxExp = job.ExpToLevelUp; // ù ������ ����ġ - ������ϵ� ����. ���� ����� ����.
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

        // ��� ����
        public void EquipItem(Item item)
        {
            if (IsEquipped(item))
                return;

            equippedItems.Add(item);
            RetrunEquipStats();
        }

        // ��� ����
        public void UnequipItem(Item item)
        {
            if (!IsEquipped(item))
                return;

            equippedItems.Remove(item);
            RetrunEquipStats();
        }

        // ��� ���� ���� Ȯ��
        public bool IsEquipped(Item item)
        {
            return equippedItems.Contains(item);
        }

        // ���� Ȯ�� ��, ���� �ݿ�
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

        // ������ ����
        public void GainExp()
        {
            while (Exp >= MaxExp) // ������
            {

                Exp -= MaxExp;
                MaxExp = JobDatas[Job].ExpToLevelUp;
                MaxExp += 25;
                Level++;
                Hp = MaxHp; // ������ �� ü�� ����
                ExtraAtk += 1;
                ExtraDef += 1;

                Console.Clear();
                Console.WriteLine();
                string levelUpMessage = "\n\n\n\n\n    �׿��� ������ ����� ���� �� ������׽��ϴ�.\n\n\n\n\n";

                messages.StartSkipListener(); // ��ŵ ���� ����

                Console.ForegroundColor = ConsoleColor.Yellow;
                messages.PrintMessageWithSkip(levelUpMessage, 80); // ��ŵ ������ ���
                if (messages.Skip)
                {
                    Console.Clear(); // ��ŵ�Ǿ��� ��� ȭ�� ����
                }
                Console.ResetColor();

                if (!messages.Skip)
                {
                    Thread.Sleep(800); // ��ŵ���� ���� ��쿡�� �ణ ����
                }

                messages.Skip = false; // �ʱ�ȭ
                Console.WriteLine();
                Console.WriteLine();

               /*if (Level % 5 == 0) // 5�������� ���ο� ��ų ȹ��
                {
                    SkillManager.LearnSkill(this);
                }*/
            }
        }

        // ����ġ ȹ�� - ����/�̺�Ʈ ���� �޴¿�
        public void GainReward(int gold, int exp)
        {
            Gold += gold;
            Exp += exp;
            GainExp();
        }

        // ü�� ȸ�� (����, ����, �̺�Ʈ ��)
        public bool Heal(int cost, int amount)
        {
            if (cost > 0)
            {
                if (Gold < cost || Hp >= MaxHp) return false;
                Gold -= cost;
            }

            //int newHp = Hp + amount;
            int healAmount = amount;

            if (Job == JobType.���Ż�) // ���Ż�� ȸ���� ����
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

        // ���� ȸ�� (����, ����, �̺�Ʈ ��)
        public bool GainMp(int cost, int amount)
        {
            if (Gold >= cost && Mp < MaxMp)
            {
                Gold -= cost;
                Mp += amount;
                if (Mp > MaxMp) Mp = MaxMp; // �ִ� ���� �ʰ� ����
                return true;
            }
            return false;
        }


        // �÷��̾� ����
        public void PlayerAttack(Monster target, double power)
        {
            if (IsHit(target))
            {
                bool isCritical = IsCritical();
                double multiplier = DamageSpread();

                double attackDamage = isCritical ? FinalAtk * power * (CriDmg / 100.0) : FinalAtk * power;
                int finalAttackDamage;

                if (Job == JobType.������) // ������� ���� ���� ����
                {
                    int ignoredDef = target.Def / 2; // ������ ���� ���
                    finalAttackDamage = (int)Math.Ceiling(attackDamage * multiplier - ignoredDef);

                    Console.WriteLine($"\n\n\n{"",10}�������� �ֹ�! {ignoredDef} ��ŭ�� ������ �����߽��ϴ�!");
                }
                else
                {
                    finalAttackDamage = (int)Math.Ceiling(attackDamage * multiplier - target.Def);
                }

                if (Job == JobType.��������) // ���������� ������ ������ ������ ���� ���� ����
                {
                    Random random = new Random();
                    int armorBreak = random.Next(0, 4); // 0 ~ 3 ���� ����

                    if (armorBreak > 0)
                    {
                        int beforeDef = target.Def;
                        target.Def -= armorBreak;

                        if (target.Def < 0)
                        {
                            target.Def = 0; // ������ 0 ������ �������� �ʰ�
                        }

                        int decreasedAmount = beforeDef - target.Def;

                        Console.WriteLine($"\n\n\n{"",10}�������̰� [Lv.{target.Level}][{target.Name}]�� ������ {armorBreak}��ŭ ���� ���׽��ϴ�!");
                        Console.WriteLine($"\n\n\n{"",10}����  [Lv.{target.Level}][{target.Name}]�� ������ {target.Def}�Դϴ�!");
                    }
                }

                if (Job == JobType.����) // ���� Ư��
                {
                    if (target.Name == "ī�ǹٶ�" || target.Name == "��ȸ�ϴ� ���谡" || target.Name == "��� ī�ǹٶ�" || target.Name == "���� �����")
                    {
                        int originalDamage = finalAttackDamage; // �⺻ ���� ������

                        finalAttackDamage = (int)(finalAttackDamage * 1.3); // �������� 30% �߰� ����

                        int addedDamage = finalAttackDamage - originalDamage; // �߰��� ���ط� ���

                        Console.WriteLine($"\n\n\n{"",10}������ ������! �⺻ ���� {originalDamage} + �߰� ���� {addedDamage}�� ���ظ� ������!");
                    }
                }
                if (Job == JobType.���ݼ���) // ���ݼ��� Ư��
                {
                    int HpDamage = (int)(target.CurrentHp * 0.2); // ���� ü���� 20% �߰� ����
                    int originalDamage = finalAttackDamage; // �⺻ ����

                    finalAttackDamage += HpDamage; // ���� ������ ���

                    Console.WriteLine($"\n\n\n{"",10}���ݼ����� ����! �⺻ ���� {originalDamage} + �߰� ���� {HpDamage}�� ���ظ� ������!");
                }

                finalAttackDamage = Math.Max(1, finalAttackDamage); // �ּ� ������ 1 ����
                target.CurrentHp -= finalAttackDamage;

                Console.WriteLine();

                if (isCritical)
                {
                    messages.CriticalMes(this);
                }
                Console.WriteLine($"\n\n{"",10}[Lv.{target.Level}][{target.Name}] ���� {finalAttackDamage}��ŭ ���ظ� ������!");
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"\n\n\n{"",10}[Lv.{target.Level}][{target.Name}] ���� ������ ��������!");
            }

            Console.WriteLine($"\n\n\n{"",10}�� [Enter] Ű�� ���� �������� �Ѿ����.");
            Utils.WaitForEnter();
            Console.Clear();
        }

        // ���Ż� ���� ����
        public void SpiritAttack(Monster target)
        {
            Random random = new Random();
            int SpiritCount = random.Next(1, 4); // 1 ~ 3���� ���� ��ȯ

            Console.WriteLine($"\n\n\n{"",10}{SpiritCount}���� ��ȥ�� ����� �����ϴ�.");

            int totalDamage = 0; // ��ü ���� ������

            for (int i = 0; i < SpiritCount; i++)
            {
                int SpiritDamage = Level * 1;

                if (target.CurrentHp - SpiritDamage <= 0)
                {
                    SpiritDamage = (int)target.CurrentHp - 1;
                    if (SpiritDamage < 0) SpiritDamage = 0; // ü���� 1 ���ϸ� 0 ������
                }

                target.CurrentHp -= SpiritDamage;
                totalDamage += SpiritDamage; // ����
                if (target.CurrentHp <= 1) break; // ü�� 1 ���ϸ� �߰� ���� ����
            }

            Console.WriteLine($"\n{"",10}�ǹ��� ��ȥ {SpiritCount}���� [Lv.{target.Level}][{target.Name}] ���� �� {totalDamage}��ŭ ���ظ� �־���!");

            Console.WriteLine($"\n\n\n{"",10}�� [Enter] Ű�� ���� �������� �Ѿ����.");
            Utils.WaitForEnter();
            Console.Clear();
        }
        public bool IsHit(Monster target)
        {
            int hit = Acc - target.Dodge;
            return rand.Next(0, 100) <= hit;
        }
        public bool IsCritical() // �÷��̾� ġ��Ÿ ����
        {
            return rand.Next(0, 100) < Cri; // ġ��Ÿ Ȯ��
        }

        public double DamageSpread() // ���� �� ���ط� 0.9 ~ 1.1 ���� ����
        {
            return rand.NextDouble() * 0.2 + 0.9;
        }

        //�ǰ� ���ط� ���
        public void EnemyDamage(int amount)
        {
            int damage = amount - Def;
            damage = damage < 0 ? 1 : damage;
            Hp -= damage;

            Console.WriteLine();
            Console.WriteLine($"    {damage}�� �������� �޾ҽ��ϴ�!");
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
                battles.BattleFailUI(); // Battles Ŭ������ BattleFailUI ȣ��
            }
        }

        public void boarDamage(bool choice)
        {
            if (choice)
            {
                // �÷��̾�� �ִ�ü���� 10%�� �������� ����
                Hp -= Hp / 10;
            }
            // ������� ���� ������ Ʋ�� ���
            else
            {
                // �÷��̾�� �ִ�ü���� 20%�� �������� ����
                Hp -= Hp / 5;
            }
        }



        // ���� ��ȭ ��ų (Ȱ�뿹��:   player.AtkUP(5);   // ���ݷ� +5)
        public void AtkUP(int value)
        {
            ExtraAtk += value;
        }
        public void DefUP(int value)
        {
            ExtraDef += value;
        }



        // 1. ���º���
        public void DisplayPlayerInfo()
        {
            Console.Clear();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("������������������������������<< ������ ��� >>������������������������������");
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
            Console.WriteLine($"  ü�� : {Hp}/{MaxHp}");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("  ���ݷ� : ");
            Console.WriteLine($"{Atk}{(ExtraAtk == 0 ? "" : $" (+{ExtraAtk})")}");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("  ���� : ");
            Console.WriteLine($"{Def}{(ExtraDef == 0 ? "" : $" (+{ExtraDef})")}");
            Console.ResetColor();
            Console.WriteLine();

            Console.Write($"  ���߷� :{Acc}%");
            Console.WriteLine();

            Console.Write($"  ġ��Ÿ Ȯ�� :{Cri}%");
            Console.WriteLine();

            Console.Write($"  ġ��Ÿ ���� :{CriDmg}%");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"  ���� : {Mp}/{MaxMp}");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\n  Gold : {Gold} G");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("��������������������������������������������������������������������������������������������");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("\n[1] ��ų ����");
            Console.WriteLine("[~`] ������");
            Console.Write("\n���Ͻô� �ൿ�� �Է����ּ���\n>> ");
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

        // 2. ��ų UI �ּ�
        public void DisplaySkillUI()
        {
            Console.Clear();
            Console.WriteLine("[��ų ���]");

            //ShowSkillList();

            Console.WriteLine("\n[1] ��ų �����ϱ�");
            Console.WriteLine("[~`] ������");
            Console.Write("\n���Ͻô� �ൿ�� �Է����ּ���\n>> ");

            int choice = Utils.CheckInput(1, 1);
            switch (choice)
            {
                case -1:
                    return;
                case 1:
                    Console.Clear();
                    Console.WriteLine("��ų ��� �غ����Դϴ�.");
                    Thread.Sleep(1000);
                    break;
            }
        }

        // 2-1. ��ų��� ��°�
        public void ShowSkillList()
        {
            for (int i = 0; i < Skills.Count; i++)
            {
                SkillLibrary targetSkill = Skills[i];

                string displayIdx = $"{i + 1}";
                Console.WriteLine($"- {displayIdx} {targetSkill.Name}" +
                $" : {targetSkill.Desc} (�Ҹ� ��: {targetSkill.Cost} / ��Ÿ��: {targetSkill.Cool})");
            }
        }

        // 2-2. ��ų �� ��ȯ
        public int SkillListCount
        {
            get
            {
                return Skills.Count;
            }
        }

        // ��ȭ ��� ��� - �÷��̾� ���� �ݿ� ������ player���� ���
        public int UpgradeCost(Item item)
        {
            return item.TotalValue < 20 ? 100 : 200;
        }

        // ��ȭ ���� �� �ɷ�ġ ������
        public int UpgradeValue(Item item)
        {
            if (Job == JobType.��������)
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

        // �ִ�ġ �ʰ��� ��� ���� ����
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

        // ��ȭ �� ���� �ݿ� (ȣ���)
        public void UpgradeStat(Item item, int valueUp)
        {
            if (!IsEquipped(item))
                return;

            if (item.Type == 0)
                ExtraAtk += valueUp;
            else if (item.Type == 1)
                ExtraDef += valueUp;
        }


        //���� �̺�Ʈ
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