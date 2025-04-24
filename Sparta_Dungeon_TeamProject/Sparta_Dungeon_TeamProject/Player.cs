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
        public int Def { get; private set; }
        public int Hp { get; private set; }
        public int MaxHp { get; private set; }
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

        // 1. ���º��� # Program.cs
        public void DisplayPlayerInfo()
        {
            Console.WriteLine($"Lv. {Level:D2} {{ {Exp}/{MaxExp} }}");
            Console.WriteLine($"{Name} {{ {Job} }}");
            Console.WriteLine(ExtraAtk == 0 ? $"���ݷ� : {Atk}" : $"���ݷ� : {Atk + ExtraAtk} (+{ExtraAtk})");
            Console.WriteLine(ExtraDef == 0 ? $"���� : {Def}" : $"���� : {Def + ExtraDef} (+{ExtraDef})");
            Console.WriteLine($"ü�� : {Hp}/{MaxHp}");
            Console.WriteLine($"���� : {Mp}/{MaxMp}");
            Console.WriteLine($"Gold : {Gold} G");
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
                Console.WriteLine();
                string levelUpMessage = "�׿��� ������ ����� ���� �� ������׽��ϴ�.";

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

        // ���� DB # SetData()
        public enum JobType
        {
            ���� = 1,
            ������,
            ������,
            ��������,
            ���Ż�,
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

        // ������ �⺻ �ɷ�ġ DB # SetData()
        public static class JobDB
        {
            public static Dictionary<JobType, JobData> Jobs = new Dictionary<JobType, JobData>
            {   // ������ / ���ݷ� / ���� / �ִ�ü�� / �ִ븶��
                { JobType.����, new JobData(10, 10, 150, 100) },
                { JobType.������, new JobData(12, 5, 60, 150) },
                { JobType.������, new JobData(8, 10, 80, 200) },
                { JobType.��������, new JobData(5, 5, 60, 0) },
                { JobType.���Ż�, new JobData(10, 5, 80, 200) }
            };
        }

        // ������ �⺻ ��ų ���� # SetData()


        public void DisplaySkillUI()
        {
            Console.Clear();
            Console.WriteLine("��ų ����");
            Console.WriteLine("�̰����� ĳ������ ��ų�� ������ �� �ֽ��ϴ�.\n");
            Console.WriteLine("[��ų ���]");

            ShowSkillList(false);

            Console.WriteLine("\n1. ���� ����");
            Console.WriteLine("0. ������");
            Console.Write("\n���Ͻô� �ൿ�� �Է����ּ��� >> ");

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
            Console.WriteLine("��ų���� - ���� ����");
            Console.WriteLine("�̰����� ĳ������ ��ų�� ������ �� �ֽ��ϴ�.\n");
            Console.WriteLine("[��ų ���]");

            ShowSkillList(true);

            Console.WriteLine("\n0. ���ư���");
            Console.Write("������ ��ų ��ȣ�� �Է��ϼ��� >> ");

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
                        Console.WriteLine($"'{selectedSkill.Name}' ��ų�� �����߽��ϴ�.");
                    }
                    else
                    {
                        EquipSkillList.Add(selectedSkill);
                        Console.WriteLine($"'{selectedSkill.Name}' ��ų�� �����߽��ϴ�.");
                    }
                    Thread.Sleep(4000);
                    DisplayEquipSkill();
                    break;
            }
        }

        // ��ų ��� ��� # Program.cs
        public void ShowSkillList(bool showIdx)
        {
            for (int i = 0; i < Skills.Count; i++)
            {
                GameSkill targetSkill = Skills[i];

                string displayIdx = showIdx ? $"{i + 1} " : "";
                string displayEquipped = IsEquippedSkill(targetSkill) ? "[E]" : "";
                Console.WriteLine($"- {displayIdx} {displayEquipped} {targetSkill.Name}" +
                $" ( �Ҹ�: {targetSkill.Cost} / ��Ÿ��: {targetSkill.CoolTime} / {targetSkill.Desc} )");

            }
        }
        public bool IsEquippedSkill(GameSkill skill)
        {
            return EquipSkillList.Contains(skill);
        }

        // ���� ��ų ī���� # Program.cs
        public int SkillListCount
        {
            get
            {
                return Skills.Count;
            }
        }
        
        // ��ų ���� # Program.cs
        public void EquipSkill(GameSkill AllSkills)
        {
            if (IsEquippedSkill(AllSkills))
            {
                EquipSkillList.Remove(AllSkills);
                Console.WriteLine($"{AllSkills.Name} ��ų ���� ����");
            }
            else
            {
                EquipSkillList.Add(AllSkills);
                Console.WriteLine($"{AllSkills.Name} ��ų ���� �Ϸ�");
            }
        }

        // ��ų ���� ��� # Program.cs
        public List<GameSkill> GetListSkill()
        {
            return Skills;
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
            MaxHp += Hp;
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

            Console.WriteLine($"{damage}�� �������� �޾ҽ��ϴ�!");

            if (Hp < 0)
            {
                Hp = 0;
                Console.WriteLine("����ϼ̽��ϴ�.");
                Thread.Sleep(1000);
                Environment.Exit(0);
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