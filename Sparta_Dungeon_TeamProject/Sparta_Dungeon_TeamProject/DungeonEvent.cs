using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Sparta_Dungeon_TeamProject
{
    public class DungeonEvent
    {
        public List<Item> eventitemlist = new List<Item>();
        Random random = new Random();
        int eventNum;
        int eventValue;
        int result;
        public void Start()
        {
            Console.WriteLine("주변을 살펴봅니다.");
            eventNum = random.Next(0, 9);
            switch (eventNum)
            {
                case 0:
                    statue();//석상 발견
                    break;
                case 1:
                    treasureBox();//보물상자 발견
                    break;
                case 2:
                    MerchantOrThief();//상인or약탈자 조우
                    break;
                case 3:
                    brokenCart();//부서진수레
                    break;
                case 4:
                    hut();//부서진오두막
                    break;
                case 5:
                    cryingChild();//우는아이
                    break;
                case 6:
                    wanderingMage();// 떠돌이 마법사 조우
                    break;
                case 7:
                    if (player.HasItem(Item.EventItemsDb[3]))
                    {
                        cat();// 고양이 조우 이벤트
                    }
                    else
                    {
                        fishingSpot();// 낚시 스폿 발견
                    }
                    break;
                case 8:
                    if (player.HasItem(Item.EventItemsDb[0]))
                    {
                        smith();//검복구 이벤트
                    }
                    else
                    {
                        brokenSword();//부러진 검 이벤트
                    }
                    break;

            }
        }

        void statue()//석상(체력회복, 감소)-10~30
        {

            Console.WriteLine("부서진 석상을 발견했습니다.");
            Console.WriteLine("\n1.기도한다.\n2.지나간다");
            result = Utils.CheckInput(1, 2);

            switch (result)
            {
                case 1:
                    eventNum = random.Next(1, 3);
                    eventValue = random.Next(10, 31);
                    if (eventNum == 1)
                    {
                        Console.WriteLine($"신전에서 체력이 {eventValue} 회복되었습니다.");
                        player.Heal(0, eventValue);
                    }
                    else
                    {
                        Console.WriteLine($"신전에서 체력이 {eventValue} 감소하였습니다.");
                        player.Heal(0, -eventValue);

                    }
                    break;
                case 2:
                    Console.WriteLine("석상을 지나쳤습니다.");
                    break;
            }



        }

        //보물상자(아이템드랍or미믹)3:7
        void treasureBox()
        {
            Console.WriteLine("보물상자를 발견했습니다.");
            Console.WriteLine("\n1.열어본다.\n2.지나간다");
            result = Utils.CheckInput(1, 2);
            switch (result)
            {
                case 1:
                    eventNum = random.Next(1, 11);
                    if (eventNum <= 7)
                    {
                        Console.WriteLine("보물상자에서 아이템을 획득했습니다.");
                        //아이템 드랍
                    }
                    else
                    {
                        Console.WriteLine("미믹과 조우했습니다.");
                        //미믹과 전투
                    }
                    break;
                case 2:
                    Console.WriteLine("보물상자를 지나쳤습니다.");
                    break;
            }
        }


        //상인or약탈자7:3
        void MerchantOrThief()
        {
            Console.WriteLine("상인처럼보이는 사람을 발견했습니다.");
            Console.WriteLine("\n1.대화한다.\n2.지나간다");
            result = Utils.CheckInput(1, 2);
            switch (result)
            {
                case 1:
                    eventNum = random.Next(1, 11);
                    if (eventNum <= 7)
                    {
                        Console.WriteLine("상인과 대화하여 아이템을 구매했습니다.");
                        //아이템 구매
                    }
                    else
                    {
                        Console.WriteLine("약탈자와 조우했습니다.");
                        //약탈자와 전투
                    }
                    break;
                case 2:
                    Console.WriteLine("사람몰래 지나갔습니다.");
                    break;
            }
        }

        //부서진 수레 이벤트
        //-> 부서진 수레 사이에서 보물발견 OR 함정발동
        void brokenCart()
        {
            Console.WriteLine("부서진 수레를 발견했습니다.");
            Console.WriteLine("\n1.조사한다.\n2.지나간다");
            result = Utils.CheckInput(1, 2);
            switch (result)
            {
                case 1:
                    eventNum = random.Next(1, 11);
                    if (eventNum <= 5)
                    {
                        Console.WriteLine("부서진 수레에서 아이템을 발견했습니다.");
                        //아이템 드랍
                    }
                    else
                    {
                        Console.WriteLine("수레를 만지자 누군가 남겨놓은 함정에 데미지를 받았습니다.");
                        player.Heal(0, -random.Next(1, 6));

                    }
                    break;
                case 2:
                    Console.WriteLine("부서진 마차를 지나쳤습니다.");
                    break;
            }
        }

        //오두막 발견 이벤트
        //-> 오두막 안에서 포션 발견 OR 아무것도 없음
        //-> 긍정 포션 OR 부정 포션
        void hut()
        {
            Console.WriteLine("오두막을 발견했습니다.");
            Console.WriteLine("\n1.조사한다.\n2.지나간다");
            result = Utils.CheckInput(1, 2);
            switch (result)
            {
                case 1:
                    eventNum = random.Next(1, 11);
                    if (eventNum <= 7)
                    {
                        Console.WriteLine("오두막에서 포션을 발견했습니다.");
                        //아이템 획득
                    }
                    else
                    {
                        Console.WriteLine("오두막에서 아무것도 발견하지 못했습니다.");
                        //아무것도 없음
                    }
                    break;
                case 2:
                    Console.WriteLine("오두막을 지나쳤습니다.");
                    break;
            }
        }

        //우는 아이 이벤트
        //-> 우는 아이 발견 OR 밴시 조우 전투
        void cryingChild()
        {
            Console.WriteLine("아이가 우는소리가 들립니다.");
            Console.WriteLine("\n1.조사한다.\n2.지나간다");
            result = Utils.CheckInput(1, 2);
            switch (result)
            {
                case 1:
                    eventNum = random.Next(1, 11);
                    if (eventNum <= 7)
                    {
                        Console.WriteLine("우는 아이를 발견했습니다.");
                        //퀘스트 완료판정넣기
                        //마을로 데리고가서 보상을받음
                    }
                    else
                    {
                        Console.WriteLine("밴시와 조우했습니다.");
                        //밴시와 전투
                    }
                    break;
                case 2:
                    Console.WriteLine("우는 아이를 지나쳤습니다.");
                    break;
            }
        }

        //부러진 검 이벤트
        //-> 부러진 검 습득 OR 지나친다
        //-> 향후 검 복구 이벤트 발동 트리거
        void brokenSword()
        {
            Console.WriteLine("부러진 검을 발견했습니다.");
            Console.WriteLine("\n1.조사한다.\n2.지나간다");
            result = Utils.CheckInput(1, 2);
            switch (result)
            {
                case 1:
                    Console.WriteLine("부러진 검을 획득했습니다.");
                    player.BuyItem(eventitemlist[0]);
                    //아이템 획득
                    break;
                case 2:
                    Console.WriteLine("부러진 검을 지나쳤습니다.");
                    break;
            }
        }

        //낚시 스폿 발견
        //-> 낚시한다 OR 지나친다
        void fishingSpot()
        {
            Console.WriteLine("낚시 스폿을 발견했습니다.");
            Console.WriteLine("\n1.낚시한다.\n2.지나간다");
            result = Utils.CheckInput(1, 2);
            switch (result)
            {
                case 1:
                    Console.WriteLine("낚시를 통해 물고기를 잡았습니다.");
                    player.BuyItem(eventitemlist[3]);
                    //물고기 획득
                    break;
                case 2:
                    Console.WriteLine("낚시 스폿을 지나쳤습니다.");
                    break;
            }
        }

        //검복구 
        //-> 검 복구시 좋은 검 OR 저주받은 검 OR 어이쿠 손이.. 3:4:3 (저주디버프 추가)
        void smith()
        {
            Console.WriteLine("대장간을 발견했습니다!\n대장장이 : 자네 혹시 부러진검을 갖고있나?");
            Console.WriteLine("대장장이 : 그 검을 고쳐줄수있다네.");
            Console.WriteLine("대장장이 : 하지만 고치고 나서 어떤검이 나올지는 모르지.");
            Console.WriteLine("\n1.검을 복구한다.\n2.지나간다");
            result = Utils.CheckInput(1, 2);
            switch (result)
            {
                case 1:
                    eventNum = random.Next(1, 11);
                    if (eventNum <= 3)
                    {
                        Console.WriteLine("검이 복구되었습니다.");


                        //player.SelectRemove(eventitemlist[0].Name);
                        player.BuyItem(eventitemlist[1]);

                        //아이템 드랍
                    }
                    else if (eventNum <= 7)
                    {
                        Console.WriteLine("저주받은 검이 복구되었습니다.");
                        //player.SelectRemove(eventitemlist[0].Name);
                        player.BuyItem(eventitemlist[2]);
                        //아이템 드랍
                    }
                    else
                    {
                        Console.WriteLine("어이쿠 손이 미끄러졌네...\n");
                        //player.SelectRemove(eventitemlist[0].Name);
                        //디버프
                    }
                    break;
                case 2:
                    Console.WriteLine("대장간을 지나쳤습니다.");
                    break;
            }
        }

        //떠돌이 마법사 조우
        //-> 랜덤 스크롤 한개 획득 OR 스탯 랜덤 +1~3
        void wanderingMage()
        {
            Console.WriteLine("떠돌이 마법사를 발견했습니다.");
            Console.WriteLine("\n1.대화한다.\n2.지나간다");
            result = Utils.CheckInput(1, 2);
            switch (result)
            {
                case 1:
                    eventNum = random.Next(1, 11);
                    if (eventNum <= 7)
                    {
                        Console.WriteLine("떠돌이 마법사에게서 스크롤을 획득했습니다.");
                        //아이템 드랍
                    }
                    else
                    {
                        Console.WriteLine("떠돌이 마법사에게서 지식을 얻었습니다.");
                        Console.WriteLine("스탯+@");
                        //스탯 랜덤 +1~3
                    }
                    break;
                case 2:
                    Console.WriteLine("떠돌이 마법사를 지나쳤습니다.");
                    break;
            }
        }

        //고양이 조우 이벤트
        //-> 고양이 조우 시 인벤에 있는 물고기 하나 주고 돌려보냄
        //-> 고양이에게 물고기를 줄 시 마을로 복귀 시 고양이가 랜덤 템을 줌
        void cat()
        {
            Console.WriteLine("고양이를 발견했습니다.");
            Console.WriteLine("\n1.고양이에게 물고기를 준다.\n2.지나간다");
            result = Utils.CheckInput(1, 2);
            switch (result)
            {
                case 1:
                    if (player.HasItem(Item.EventItemsDb[3]))
                    {
                        //player.SelectRemove(eventitemlist[3].Name);
                        Console.WriteLine("고양이에게 물고기를 주었습니다.");
                        //아이템 드랍
                    }
                    else
                    {
                        Console.WriteLine("고양이에게 줄 물고기가 없습니다.");
                    }
                    break;
                case 2:
                    Console.WriteLine("고양이를 지나쳤습니다.");
                    break;
            }
        }
    }

    //이벤트 몬스터 추가
    class Mimic : Monster
    {
        public Mimic() : base(
            name: "미믹",
            minLevel: 3, maxLevel: 5,
            minAtk: 5, maxAtk: 10,
            minDef: 10, maxDef: 20,
            minHp: 10, maxHp: 30,
            dodge: 0,
            minDropGold: 2000, maxDropGold: 5000,
            minDropExp: 50, maxDropExp: 100,
            isAlive: true,
            origin: 0.15f)
        {
        }

    }
    class Banshee : Monster
    {
        public Banshee() : base(
            name: "밴시",
            minLevel: 3, maxLevel: 5,
            minAtk: 5, maxAtk: 10,
            minDef: 10, maxDef: 20,
            minHp: 10, maxHp: 30,
            dodge: 20,
            minDropGold: 2000, maxDropGold: 5000,
            minDropExp: 50, maxDropExp: 100,
            isAlive: true,
            origin: 0.15f)
        {
        }
    }
}
