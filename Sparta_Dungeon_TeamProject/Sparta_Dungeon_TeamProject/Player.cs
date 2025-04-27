using System;
using System.Collections.Generic;
using System.ComponentModel;
using static Sparta_Dungeon_TeamProject.Program;

namespace Sparta_Dungeon_TeamProject
{
    // �÷��̾� Ŭ����
    public class Player
    {
        private static Player _instance;
        private Player() { }
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

        public string Name { get; private set; }
        public JobType Job { get; private set; }
        public int Level { get; private set; }
        public int Exp { get; private set; }
        public int MaxExp { get; private set; }

        // �⺻ ���� �� �ִ� Hp/Mp
        public int Atk { get; private set; }
        public int Cri { get; private set; }
        public int Def { get; private set; }
        public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public int MaxMp { get; private set; }
        public int Mp { get; private set; }
        public int Gold { get; set; }

        // �������ο� ���� �߰�����
        public int ExtraAtk { get; private set; }
        public int ExtraDef { get; private set; }

        // �� ���� �հ�
        public int FinalAtk => Atk + ExtraAtk;
        public int FinalDef => Def + ExtraDef;

        // �κ��丮
        private List<Item> inventory = new List<Item>();
        private readonly List<Item> equippedItems = new();

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
            Cri = job.Cri;
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
                MaxExp += 50;
                Level++;

                Console.Clear();
                Console.WriteLine();
                string levelUpMessage = "\n\n\n\n\n    �׿��� ������ ����� ���� �� ������׽��ϴ�.\n\n\n\n\n";

                Messages.StartSkipListener(); // ��ŵ ���� ����

                Console.ForegroundColor = ConsoleColor.Yellow;
                Messages.PrintMessageWithSkip(levelUpMessage, 80); // ��ŵ ������ ���
                if (Messages.Skip)
                {
                    Console.Clear(); // ��ŵ�Ǿ��� ��� ȭ�� ����
                }
                Console.ResetColor();

                if (!Messages.Skip)
                {
                    Thread.Sleep(800); // ��ŵ���� ���� ��쿡�� �ణ ����
                }

                Messages.Skip = false; // �ʱ�ȭ
                Console.WriteLine();
                Console.WriteLine();

                if (Level % 5 == 0) // 5�������� ���ο� ��ų ȹ��
                {
                    SkillManager.LearnSkill(this);
                }
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




        // ���� ���-�Ϲ� ����
        public void PlayerAttack(Monster target, double power)
        {
            bool isCritical = IsCritical();
            double multiplier = DamageSpread();

            double attackDamage = isCritical ? FinalAtk * power * 1.5 : FinalAtk * power;
            int finalAttackDamage = (int)Math.Ceiling(attackDamage * multiplier - target.Def); // ���� ���¸�ŭ ���� ������ ����
            finalAttackDamage = Math.Max(1, finalAttackDamage); // �ּ� ������ 1

            target.CurrentHp -= finalAttackDamage;

            Console.Clear();
            Console.WriteLine();

            if (this.IsCritical())
            {
                Messages.CriticalMes(this);
            }
            Console.WriteLine($"\n\n\n{"",10}[Lv.{target.Level}][{target.Name}] ���� {finalAttackDamage}��ŭ ���ظ� ������!");
            Console.WriteLine($"\n\n\n{"",10}�� [Enter] Ű�� ���� �������� �Ѿ����.");
            Utils.WaitForEnter();
            Console.Clear();
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
                Program.BattleFailUI();
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



        // 1. ���º��� UI
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

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("  ���ݷ� : ");
            Console.WriteLine($"{Atk}{(ExtraAtk == 0 ? "" : $" (+{ExtraAtk})")}");
            Console.WriteLine();

            Console.Write("  ���� : ");
            Console.WriteLine($"{Def}{(ExtraDef == 0 ? "" : $" (+{ExtraDef})")}");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"  ü�� : {Hp}/{MaxHp}");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"  ���� : {Mp}/{MaxMp}");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"  Gold : {Gold} G");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("��������������������������������������������������������������������������������������������");
            Console.ResetColor();
            Console.WriteLine();

            int choice = Utils.CheckInput(1, 1);
            switch (choice)
            {
                case -1:
                    return;
                    break;
                case 1:
                    DisplaySkillUI(program);
                    break;
            }
        }

        // 2. ��ų UI
        public void DisplaySkillUI(Program program)
        {
            Console.Clear();
            Console.WriteLine("[��ų ���]");

            ShowSkillList();

            Console.WriteLine("\n[1] ��ų �����ϱ�");
            Console.WriteLine("[~`] ������");
            Console.Write("\n���Ͻô� �ൿ�� �Է����ּ��� >> ");

            int choice = Utils.CheckInput(1, 1);
            switch (choice)
            {
                case -1:
                    return;
                    break;
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
    }
}