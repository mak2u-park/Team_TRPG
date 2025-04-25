using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using Sparta_Dungeon_TeamProject;
using static System.Net.Mime.MediaTypeNames;

namespace Sparta_Dungeon_TeamProject
{
    // �÷��̾� Ŭ����
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

        public int FinalAtk => Atk + ExtraAtk; // ���� ���ݷ�
        public int FinalDef => Def + ExtraDef; // ���� ����

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

        // 1. ���º��� # Program.cs
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
            if (ExtraAtk == 0)
                Console.WriteLine($"{Atk}");
            else
                Console.WriteLine($"{Atk + ExtraAtk} (+{ExtraAtk})");

            Console.WriteLine();

            Console.Write("  ���� : ");
            if (ExtraDef == 0)
                Console.WriteLine($"{Def}");
            else
                Console.WriteLine($"{Def + ExtraDef} (+{ExtraDef})");
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
        }

        // ����ġ ȹ�� # Program.cs
        public void GainExp()
        {
            while (Exp >= MaxExp) // ������
            {
                Exp -= MaxExp;
                MaxExp += 10;
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

        public void PlayerAttack(Monster target, double power)
        {
            bool isCritical = IsCritical();
            double multiplier = DamageSpread();

            double attackDamage = isCritical ? FinalAtk * power * 1.5: FinalAtk * power;
            int finalAttackDamage = (int)Math.Ceiling(attackDamage * multiplier - target.Def); // ���� ���¸�ŭ ���� ������ ����
            finalAttackDamage = Math.Max(1, finalAttackDamage); // �ּ� ������ 1

            target.Hp -= finalAttackDamage;

            if (this.IsCritical())
            {
                Messages.CriticalMes(this);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"{"",10}[Lv.{target.Level}][{target.Name}] ���� {finalAttackDamage}��ŭ ���ظ� ������!");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"{"",10}�� �ƹ� Ű�� ���� �������� �Ѿ����.");
            Console.ReadKey();
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

        public void DisplaySkillUI()
        {
            Console.Clear();
            Console.WriteLine("[��ų ���]");

            ShowSkillList();

            Console.WriteLine("\n0. ������");
            Console.Write("\n���Ͻô� �ൿ�� �Է����ּ��� >> ");

            int choice = Program.CheckInput(0, 1);
            switch (choice)
            {
                case 0:
                    Program.DisplayMainUI();
                    break;
            }
        }

        // ��ų ��� ��� # Program.cs
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

        // ���� ��ų ī���� # Program.cs
        public int SkillListCount
        {
            get
            {
                return Skills.Count;
            }
        }

        public void AddGold(int amount) // ��带 �߰����ִ� �ż���
        {
            Gold += amount;
        }

        public void GainReward(int gold, int exp) // �������� ����
        {
            AddGold(gold);
            Exp += exp;
            GainExp();
        }

        // �κ��丮 �����۸�� # Inventory.cs
        public void InventoryItemList(bool showIdx)
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                Item targetItem = Inventory[i];

                // �κ��丮 ������ ���
                string displayIdx = showIdx ? $"{i + 1} " : "";
                string displayEquipped = IsEquipped(targetItem) ? "[E]" : "";

                // ������ ��ȭ ���ο� ���� ���
                string enhanceText = targetItem.Value == targetItem.MaxValue ? " (�ִ�ġ)" : "";
                string statText = $"+({targetItem.Value})"; // ��ȭ ��ġ
                string typeText = targetItem.Type == 0 ? "���ݷ�" : "����";

                // ������ ���� ���: �̸�, Ÿ��,
                Console.WriteLine($"- {displayIdx}{displayEquipped}{targetItem.ItemEnhanceText()}");
            }
        }

        // ��� ���뿩�� # Inventory.cs
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

        // �κ��丮 ������ ��� ��ȯ(��ȸ) # Inventory.cs
        public List<Item> GetInventoryItems()
        {
            return Inventory;
        }

        // ���� �� �Ǹ� ������ ���� # Shop.cs
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

        // ������ ��ȭ # Inventory.cs ���� ȣ���� ���� �и�
        public int GetUpgradeCost(Item item)
        {
            return item.Value < 20 ? 100 : 200;
        }
        public int GetUpgradeValue(Item item)
        {
            return item.Value < 20 ? 5 : 10;
        }

        // ������ ��ȭ # Inventory.cs
        public bool UpgradeItem(Item item)
        {
            int cost = GetUpgradeCost(item);
            int valueUp = GetUpgradeValue(item);

            if (item.Value >= item.MaxValue) // �ִ�ġ �̻�
            {
                return false;
            }

            if (Gold < cost) // ��� ����
            {
                return false;
            }

            Gold -= cost; // ��� ����
            item.Value += valueUp; // ������ �ɷ�ġ ����

            //���� ���� �ݿ�
            if (IsEquipped(item))
            {
                if (item.Type == 0) ExtraAtk += valueUp;
                else ExtraDef += valueUp;
            }
            return true;
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

        public void Heal(int amount)//ü��ȸ�� �޼���
        {
            if (Hp + amount <= 0) //���� ü���� 0���ϸ� Hp10���⵵�ϼ���
            {
                Hp = 10;
            }
            else if (Hp + amount > MaxHp)//������� �ִ�ü�º���ũ�� �ִ�ü������ ����
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

        public void SelectRemove(string name)//�������� ã�Ƽ� �����ϴ� �޼���
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