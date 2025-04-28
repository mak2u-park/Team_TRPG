using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sparta_Dungeon_TeamProject
{
    public enum MonsterTypeChap1
    {
        Wolf,
        Goblin,
        Orc
    }

    public enum MonsterTypeChap2
    {
        Cavebat,
        Ghost,
        Skeleton
    }

    public enum MonsterTypeChap3
    {
        Adventurer,
        SmilingSlime,
        HeadlessSkeletion
    }

    public enum MonsterTypeChap4
    {
        Swordsman,
        Mage,
        Archer
    }

    public class Monster
    {
        // 몬스터의 이름
        public string Name { get; protected set; }

        // 몬스터 기본 스탯
        public int Level { get; protected set; }
        public int Atk { get; protected set; }
        public int Def { get; set; }
        public int Hp { get; set; }
        public int Dodge { get; protected set; }



        // 레벨 상승에 따른 최종 스탯 계산을 위한 float값 추가
        public float FinalAtk => Atk * (1 + Origin * Level);
        public float FinalDef => Def * (1 + Origin * Level);
        public float FinalHp { get; protected set; }
        public float Origin { get; protected set; }
        public float CurrentHp { get; set; }


        // 몬스터 드랍 보상 (골드, 경험치)
        public int Gold { get; protected set; }
        public int Exp { get; protected set; }
        public int DropGold { get; protected set; }
        public int DropExp { get; protected set; }


        // 몬스터 생존 여부
        public bool IsAlive { get; set; }

        private static Random random = new Random();



        public Monster(
            string name,
            int minLevel, int maxLevel,
            int minAtk, int maxAtk,
            int minDef, int maxDef,
            int minHp, int maxHp,
            int dodge,
            int minDropGold, int maxDropGold,
            int minDropExp, int maxDropExp,
            bool isAlive,
            float origin)
        {
            Name = name;
            Level = random.Next(minLevel, maxLevel + 1);
            Atk = random.Next(minAtk, maxAtk + 1);
            Def = random.Next(minDef, maxDef + 1);
            Hp = random.Next(minHp, maxHp + 1);
            FinalHp = Hp * (1 + Origin * Level);
            CurrentHp = FinalHp;
            Dodge = dodge;
            Gold = random.Next(minDropGold, maxDropGold + 1);
            Exp = random.Next(minDropExp, maxDropExp + 1);
            IsAlive = isAlive;
            DropGold = (int)(Gold * (1 + origin * Level));
            DropExp = (int)(Exp * (1 + origin * Level));
            Origin = origin;
            /*
            레벨, 공격력, 방어력, 체력, 드랍골드, 드랍경헙치는 일정 범위 이내에서 랜덤한 int값으로 정해진다.
            최종 공격력, 방어력, 체력은 레벨과 origin(태생)에 따라 영향을 받으며 좋은 태생을 가지고 있을수록
            레벨이 높을때 더 높은 최종 스탯을 갖는다.
            */

        }
        public enum StatType
        {
            Level,
            Atk,
            Def,
            Hp,
            FinalAtk,
            FinalDef,
            FinalHp,
            Dodge
        }

        public void ChangeStat(StatType stat, int value)
        {
            switch (stat)
            {
                case StatType.Level:
                    Level = value;
                    break;
                case StatType.Atk:
                    Atk = value;
                    break;
                case StatType.Def:
                    Def = value;
                    break;
                case StatType.Hp:
                    Hp = value;
                    break;
                case StatType.Dodge:
                    Dodge = value;
                    break;
                default:
                    throw new ArgumentException("존재하지 않는 스탯입니다.");
            }
        }

        public class MonsterFactory
        {
            // 문자열로 몬스터 이름을 받아 해당 몬스터 객체 생성
            public static Monster CreateMonster(string monsterName)
            {
                return monsterName switch
                {
                    "Wolf" => new Wolf(),
                    "Goblin" => new Goblin(),
                    "Orc" => new Orc(),

                    "Cavebat" => new Cavebat(),
                    "Ghost" => new Ghost(),
                    "Skeleton" => new Skeleton(),

                    "Adventurer" => new Adventurer(),
                    "SmilingSlime" => new SmilingSlime(),
                    "HeadlessSkeletion" => new HeadlessSkeletion(),

                    "Swordsman" => new Swordsman(),
                    "Mage" => new Mage(),
                    "Archer" => new Archer(),

                    "Capybara" => new Capybara(),
                    "RegretfulAdventurer" => new RegretfulAdventurer(),
                    "GiantCapybara" => new GiantCapybara(),
                    "BlackCat" => new BlackCat(),

                    _ => throw new ArgumentException($"몬스터 이름 '{monsterName}' 은(는) 존재하지 않습니다.")
                };
            }


            /*======================================[ 챕터 1 ]=====================================================*/

            public enum MonsterTypeChap1
            {
                Wolf,
                Goblin,
                Orc
            }

            // 아직 스탯은 변경하지 않음
            class Wolf : Monster
            {
                public Wolf() : base(
                    name: "늑대",
                    minLevel: 1, maxLevel: 3,
                    minAtk: 1, maxAtk: 3,
                    minDef: 5, maxDef: 5,
                    minHp: 10, maxHp: 15,
                    dodge: 20,
                    minDropGold: 100, maxDropGold: 300,
                    minDropExp: 10, maxDropExp: 20,
                    isAlive: true,
                    origin: 0.1f)
                {

                }

            }

            class Goblin : Monster
            {
                public Goblin() : base(
                    name: "고블린",
                    minLevel: 1, maxLevel: 5,
                    minAtk: 3, maxAtk: 5,
                    minDef: 5, maxDef: 10,
                    minHp: 10, maxHp: 15,
                    dodge: 10,
                    minDropGold: 200, maxDropGold: 300,
                    minDropExp: 20, maxDropExp: 30,
                    isAlive: true,
                    origin: 0.1f)
                {

                }

            }


            class Orc : Monster
            {
                public Orc() : base(
                    name: "오크",
                    minLevel: 3, maxLevel: 5,
                    minAtk: 5, maxAtk: 10,
                    minDef: 9, maxDef: 9,
                    minHp: 10, maxHp: 10,
                    dodge: 0,
                    minDropGold: 100, maxDropGold: 200,
                    minDropExp: 30, maxDropExp: 50,
                    isAlive: true,
                    origin: 0.15f)
                {

                }

            }

            /*======================================[ 챕터 2 ]=====================================================*/

            public enum MonsterTypeChap2
            {
                Cavebat,
                Ghost,
                Skeleton
            }

            // 아직 스탯은 변경하지 않음
            class Cavebat : Monster
            {
                public Cavebat() : base(
                    name: "동굴 박쥐",
                    minLevel: 5, maxLevel: 7,
                    minAtk: 5, maxAtk: 10,
                    minDef: 10, maxDef: 10,
                    minHp: 30, maxHp: 30,
                    dodge: 20,
                    minDropGold: 500, maxDropGold: 1000,
                    minDropExp: 50, maxDropExp: 100,
                    isAlive: true,
                    origin: 0.1f)
                {

                }

            }

            class Ghost : Monster
            {
                public Ghost() : base(
                    name: "유령",
                    minLevel: 7, maxLevel: 9,
                    minAtk: 10, maxAtk: 15,
                    minDef: 10, maxDef: 10,
                    minHp: 20, maxHp: 30,
                    dodge: 30,
                    minDropGold: 1000, maxDropGold: 1000,
                    minDropExp: 50, maxDropExp: 100,
                    isAlive: true,
                    origin: 0.1f)
                {

                }

            }


            class Skeleton : Monster
            {
                public Skeleton() : base(
                    name: "해골",
                    minLevel: 10, maxLevel: 15,
                    minAtk: 5, maxAtk: 10,
                    minDef: 10, maxDef: 20,
                    minHp: 20, maxHp: 30,
                    dodge: 10,
                    minDropGold: 1000, maxDropGold: 1500,
                    minDropExp: 50, maxDropExp: 100,
                    isAlive: true,
                    origin: 0.15f)
                {

                }

            }

            /*======================================[ 챕터 3 ]=====================================================*/

            public enum MonsterTypeChap3
            {
                Adventurer,
                SmilingSlime,
                HeadlessSkeletion
            }

            // 아직 스탯은 변경하지 않음
            class Adventurer : Monster
            {
                public Adventurer() : base(
                    name: "잃을 게 없는 모험가",
                    minLevel: 10, maxLevel: 15,
                    minAtk: 3, maxAtk: 5,
                    minDef: 5, maxDef: 10,
                    minHp: 30, maxHp: 30,
                    dodge: 10,
                    minDropGold: 1000, maxDropGold: 3000,
                    minDropExp: 50, maxDropExp: 100,
                    isAlive: true,
                    origin: 0.1f)
                {

                }

            }

            class SmilingSlime : Monster
            {
                public SmilingSlime() : base(
                    name: "웃는 슬라임",
                    minLevel: 10, maxLevel: 20,
                    minAtk: 3, maxAtk: 5,
                    minDef: 5, maxDef: 10,
                    minHp: 30, maxHp: 40,
                    dodge: 0,
                    minDropGold: 1000, maxDropGold: 3000,
                    minDropExp: 50, maxDropExp: 100,
                    isAlive: true,
                    origin: 0.1f)
                {

                }

            }


            class HeadlessSkeletion : Monster
            {
                public HeadlessSkeletion() : base(
                    name: "머리 없는 해골",
                    minLevel: 10, maxLevel: 25,
                    minAtk: 5, maxAtk: 10,
                    minDef: 10, maxDef: 20,
                    minHp: 30, maxHp: 50,
                    dodge: 20,
                    minDropGold: 2000, maxDropGold: 5000,
                    minDropExp: 50, maxDropExp: 100,
                    isAlive: true,
                    origin: 0.15f)
                {

                }

            }

            /*======================================[ 챕터 4 ]=====================================================*/

            public enum MonsterTypeChap4
            {
                Swordsman,
                Mage,
                Archer
            }

            // 아직 스탯은 변경하지 않음
            class Swordsman : Monster
            {
                public Swordsman() : base(
                    name: "기억 잃은 검사",
                    minLevel: 30, maxLevel: 40,
                    minAtk: 3, maxAtk: 5,
                    minDef: 5, maxDef: 10,
                    minHp: 50, maxHp: 100,
                    dodge: 20,
                    minDropGold: 1000, maxDropGold: 3000,
                    minDropExp: 50, maxDropExp: 100,
                    isAlive: true,
                    origin: 0.1f)
                {

                }

            }

            class Mage : Monster
            {
                public Mage() : base(
                    name: "기억 잃은 마법사",
                    minLevel: 30, maxLevel: 40,
                    minAtk: 3, maxAtk: 5,
                    minDef: 5, maxDef: 10,
                    minHp: 30, maxHp: 50,
                    dodge: 0,
                    minDropGold: 1000, maxDropGold: 3000,
                    minDropExp: 50, maxDropExp: 100,
                    isAlive: true,
                    origin: 0.1f)
                {

                }

            }


            class Archer : Monster
            {
                public Archer() : base(
                    name: "기억 잃은 궁수",
                    minLevel: 30, maxLevel: 40,
                    minAtk: 5, maxAtk: 10,
                    minDef: 10, maxDef: 20,
                    minHp: 50, maxHp: 70,
                    dodge: 30,
                    minDropGold: 2000, maxDropGold: 5000,
                    minDropExp: 50, maxDropExp: 100,
                    isAlive: true,
                    origin: 0.15f)
                {

                }

            }

            /*======================================[ 보스 몬스터 ]=====================================================*/
            public enum MonsterTypeBoss
            {
                Capybara,
                RegretfulAdventurer,
                GiantCapybara,
                BlackCat
            }

            class Capybara : Monster
            {
                public Capybara() : base(
                    name: "카피바라",
                    minLevel: 5, maxLevel: 5,
                    minAtk: 5, maxAtk: 10,
                    minDef: 10, maxDef: 20,
                    minHp: 50, maxHp: 50,
                    dodge: 1,
                    minDropGold: 1000, maxDropGold: 2000,
                    minDropExp: 50, maxDropExp: 100,
                    isAlive: true,
                    origin: 0.15f)
                {

                }

            }
            class RegretfulAdventurer : Monster
            {
                public RegretfulAdventurer() : base(
                    name: "후회하는 모험가",
                    minLevel: 15, maxLevel: 15,
                    minAtk: 5, maxAtk: 10,
                    minDef: 10, maxDef: 20,
                    minHp: 100, maxHp: 100,
                    dodge: 1,
                    minDropGold: 2000, maxDropGold: 4000,
                    minDropExp: 50, maxDropExp: 100,
                    isAlive: true,
                    origin: 0.15f)
                {

                }

            }
            class GiantCapybara : Monster
            {
                public GiantCapybara() : base(
                    name: "대왕 카피바라",
                    minLevel: 30, maxLevel: 30,
                    minAtk: 5, maxAtk: 10,
                    minDef: 10, maxDef: 20,
                    minHp: 300, maxHp: 300,
                    dodge: 1,
                    minDropGold: 4000, maxDropGold: 5000,
                    minDropExp: 50, maxDropExp: 100,
                    isAlive: true,
                    origin: 0.15f)
                {

                }

            }
            class BlackCat : Monster
            {
                public BlackCat() : base(
                    name: "검은 고양이",
                    minLevel: 50, maxLevel: 50,
                    minAtk: 10, maxAtk: 10,
                    minDef: 50, maxDef: 50,
                    minHp: 1000, maxHp: 1000,
                    dodge: 100,
                    minDropGold: 5000, maxDropGold: 10000,
                    minDropExp: 50, maxDropExp: 100,
                    isAlive: true,
                    origin: 0.15f)
                {

                }
            }

        }
    }
}


