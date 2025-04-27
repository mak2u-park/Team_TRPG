using System;
using System.Collections.Generic;
using static Sparta_Dungeon_TeamProject.Program;

namespace Sparta_Dungeon_TeamProject
{
    // �÷��̾� Ŭ����
    public class Player
    {
        public string Name { get; }
        public JobType Job { get; }
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
        public int Gold { get; private set; }

        // ��� ���� �߰��Ǵ� ����
        public int ExtraAtk { get; set; }
        public int ExtraDef { get; set; }

        // ���� ���� ����
        public int FinalAtk => Atk + ExtraAtk;
        public int FinalDef => Def + ExtraDef;

        // ���� ��ų ��� (�ʱ⺸��ų�� Job����)
        public List<SkillLibrary> Skills { get; private set; } = new();

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

        // 2. ��ų UI
        public void DisplaySkillUI()
        {
            Console.Clear();
            Console.WriteLine("[��ų ���]");

            ShowSkillList();

            Console.WriteLine("\n[1] ��ų �����ϱ�");
            Console.WriteLine("[~`] ������");
            Console.Write("\n���Ͻô� �ൿ�� �Է����ּ��� >> ");

            int choice = Program.CheckInput(1, 1);
            switch (choice)
            {
                case -1:
                    Messages.ShowMainMenu();
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

        // ��ų �� ��ȯ
        public int SkillListCount
        {
            get
            {
                return Skills.Count;
            }
        }

        // ����ġ ȹ��
        public void GainExp()
        {
            while (Exp >= MaxExp) // ������
            {
                Exp -= MaxExp;
                MaxExp = JobDatas[Job].ExpToLevelUp;
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
            Program.WaitForEnter();
            Console.Clear();
        }

        private static Random rand = new Random();

        public bool IsCritical() // �÷��̾� ġ��Ÿ
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

            CheckPlayerDead();
            
        }

        public void boarDamage(bool choice)
        {
            // �ƹ��� ������ �������� ���� ���
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

        public void CheckPlayerDead()
        {
            if (Hp <= 0)
            {
                Hp = 0;
                Program.BattleFailUI();
            }
        }

        // ���� ���� - ��ų ��� �ڵ� (Ȱ�뿹��:   player.DefUP(5);   // ���� +5)
        public void DefUP(int value)
        {
            ExtraDef += value;
        }

        // ���ݷ� ���� - ��ų ��� �ڵ� (Ȱ�뿹��:   player.AtkUP(5);   // ���ݷ� +5)
        public void AtkUP(int value)
        {
            ExtraAtk += value;
        }

        // ü�� ȸ��
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

        // ���� ȸ��
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

        // �κ��丮 ������ ��� ���
        private readonly List<Item> _equippedItems = new();

        public bool IsEquipped(Item item)
        {
            return _equippedItems.Contains(item);
        }

        // ���� ��� ����, ���� ����
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

        // ��� ���� + ȿ�� ����
        public void EquipItem(Item item)
        {
            if (_equippedItems.Contains(item))
                return;

            _equippedItems.Add(item);
            RefreshEquipStats();
        }

        // ��� ���� + ȿ�� ����
        public void UnequipItem(Item item)
        {
            if (!_equippedItems.Contains(item))
                return;

            _equippedItems.Remove(item);
            RefreshEquipStats();
        }

        // �Ҹ�ǰ ���
        public void UseItem(Item item)
        {
            if (item.Type == 2 && item.HpBonus > 0) // �Ҹ�ǰ
            {
                Heal(item.Price, item.HpBonus);
            }
            if (item.Type == 2 && item.MpBonus > 0) // �Ҹ�ǰ
            {
                Heal(item.Price, item.MpBonus);
            }

            Inventory.RemoveItem(item);
        }

        // �κ��丮 ������ ��� ��ȯ
        public List<Item> GetInventoryItems() => new List<Item>(Inventory.GetItems());

        // ����/�Ǹ�/���� ������ Ȯ��
        public void BuyItem(Item item)
        {
            Gold -= item.Price;
            Inventory.AddItem(item);
        }

        // ���� ������
        public bool HasItem(Item item)
        {
            return GetInventoryItems().Contains(item);
        }

        // �Ǹ� ������
        public void SellItem(Item item)
        {
            if (IsEquipped(item))
            {
                UnequipItem(item); // ���� ����
            }

            int gainGold = (int)(item.Price * 0.85);
            Gold += gainGold;
            Inventory.RemoveItem(item); // �κ��丮���� ����
            
            ExtraAtk -= item.AtkBonus; // �ɷ�ġ ����
            ExtraDef -= item.DefBonus;
            if (ExtraAtk < 0) ExtraAtk = 0;
            if (ExtraDef < 0) ExtraDef = 0;
        }

        // ��ȭ �� ���� ������ �ɷ�ġ �ݿ�
        public void GetUpgradeStat(Item item, int valueUp)
        {
            if (!IsEquipped(item))
                return;

            switch (item.Type)
            {
                case 0: // ����
                    ExtraAtk += valueUp;
                    break;
                case 1: // ��
                    ExtraDef += valueUp;
                    break;
                case 2: // �Ҹ�ǰ (����X)
                    break;
                case 3: // ��ű� �� (����)
                    break;
            }
        }

        // ������ ��ȭ # Inventory.cs ���� ȣ���� ���� �и�
        public int GetUpgradeCost(Item item)
        {
            return item.TotalValue < 20 ? 100 : 200;
        }
        public int GetUpgradeValue(Item item)
        {
            return item.TotalValue < 20 ? 5 : 10;
        }

        // ������ ��ȭ # Inventory.cs
        public bool UpgradeItem(Item item)
        {
            int cost = GetUpgradeCost(item);
            int valueUp = GetUpgradeValue(item);

            if (item.TotalValue >= item.MaxValue) // �ִ�ġ �̻�
            {
                return false;
            }

            if (Gold < cost) // ��� ����
            {
                return false;
            }

            Gold -= cost; // ��� ����
            item.TotalValue += valueUp; // ������ �ɷ�ġ ����

            //���� ���� �ݿ�
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