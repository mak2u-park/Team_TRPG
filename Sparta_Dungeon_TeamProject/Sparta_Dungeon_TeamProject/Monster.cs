using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparta_Dungeon_TeamProject
{


    public class Monster
    {
        // 몬스터의 이름
        public string Name { get; protected set; }

        // 몬스터 기본 스탯
        public int Level { get; protected set; }
        public int Atk { get; protected set; }
        public int Def { get; protected set; }
        public int Hp { get; set; }


        // 레벨 상승에 따른 최종 스탯 계산을 위한 float값 추가
        public float FinalAtk { get; protected set; }
        public float FinalDef { get; protected set; }
        public float FinalHp { get; protected set; }


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
            int minDropGold, int maxDropGold,
            int minDropExp, int maxDropExp,
            bool isAlive,
            float orgin)
        {
            Name = name;
            Level = random.Next(minLevel, maxLevel + 1);
            Atk = random.Next(minAtk, maxAtk + 1);
            Def = random.Next(minDef, maxDef + 1);
            Hp = random.Next(minHp, maxHp + 1);
            Gold = random.Next(minDropGold, maxDropGold + 1);
            Exp = random.Next(minDropExp, maxDropExp + 1);
            IsAlive = isAlive;
            FinalAtk = Atk * (1 + orgin * Level);
            FinalDef = Def * (1 + orgin * Level);
            FinalHp = Hp * (1 + orgin * Level);
            DropGold = (int)(Gold * (1 + orgin * Level));
            DropExp = (int)(Exp * (1 + orgin * Level));
            /*
            레벨, 공격력, 방어력, 체력, 드랍골드, 드랍경헙치는 일정 범위 이내에서 랜덤한 int값으로 정해진다.
            최종 공격력, 방어력, 체력은 레벨과 orgin(태생)에 따라 영향을 받으며 좋은 태생을 가지고 있을수록
            레벨이 높을때 더 높은 최종 스탯을 갖는다.
            */

        }

        public static class MonsterFactory
        {
            private static Random rand = new Random();

            // 각 몬스터 타입을 생성하는 메서드
            public static Monster CreateMonster(MonsterType type)
            {
                return type switch
                {
                    MonsterType.Goblin => new Goblin(),
                    MonsterType.Orc => new Orc(),
                    _ => throw new ArgumentException($"몬스터 타입 '{type}' 은(는) 존재하지 않습니다.")
                };
            }


            public enum MonsterType
            {
                Goblin,
                Orc
            }

            class Goblin : Monster
            {
                public Goblin() : base(
                    name: "고블린",
                    minLevel: 1, maxLevel: 5,
                    minAtk: 3, maxAtk: 5,
                    minDef: 5, maxDef: 10,
                    minHp: 10, maxHp: 15,
                    minDropGold: 1000, maxDropGold: 3000,
                    minDropExp: 50, maxDropExp: 100,
                    isAlive: true,
                    orgin: 0.1f)
                {           
                }

            }


            class Orc : Monster
            {
                public Orc() : base(
                    name: "오크",
                    minLevel: 3, maxLevel: 5,
                    minAtk: 5, maxAtk: 10,
                    minDef: 10, maxDef: 20,
                    minHp: 10, maxHp: 30,
                    minDropGold: 2000, maxDropGold: 5000,
                    minDropExp: 50, maxDropExp: 100,
                    isAlive: true,
                    orgin: 0.15f)
                {
                }

            }

            class Capybara : Monster
            {
                public Capybara() : base(
                    name: "카피바라",
                    minLevel: 10, maxLevel: 10,
                    minAtk: 15, maxAtk: 15,
                    minDef: 20, maxDef: 20,
                    minHp: 40, maxHp: 40,
                    minDropGold: 10000, maxDropGold: 10000,
                    minDropExp: 0, maxDropExp: 0,
                    isAlive: true,
                    orgin: 0.0f)
                { 
                }
            }
        }
    }
}

