using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparta_Dungeon_TeamProject
{
    public partial class Program 
    {
        internal class DungeonEvent
        {
            Random random = new Random();
            int eventNum;
            int eventValue;
            int result;
            DungeonEvent()
            {
                Console.WriteLine("주변을 살펴봅니다.");
                eventNum = random.Next(0, 12);
                switch (eventNum) 
                {
                    case 0:
                        Shrine();
                        break;
                    case 1:
                        TreasureBox();
                        break;
                    case 2:
                        MerchantOrThief();
                        break;
                    case 3:
                        brokenCart();
                        break;
                    case 4:
                        hut();
                        break;
                    case 5:
                        cryingChild();
                        break;
                    case 6:
                        brokenSword();
                        break;
                    case 7:
                        fishingSpot();
                        break;
                    case 8:
                        // 고블린 조우
                        break;
                    case 9:
                        // 떠돌이 마법사 조우
                        break;
                    case 10:
                        // 고양이 조우 이벤트
                        break;
                    case 11:
                        // 검복구 이벤트
                        break;
                }
            }

            void Shrine()//신전(체력회복, 감소)-10~30
            {

                Console.WriteLine("부서진 신전에 도착했습니다.");
                Console.WriteLine("\n1.기도한다.\n2.지나간다");
                result = CheckInput(1, 2);

                switch (result)
                {
                    case 1:
                        eventNum = random.Next(1, 3);
                        eventValue = random.Next(10, 31);
                        if (eventNum == 1)
                        {
                            Console.WriteLine($"신전에서 체력이 {eventValue} 회복되었습니다.");
                            //player.Hp += eventValue;
                        }
                        else
                        {
                            Console.WriteLine($"신전에서 체력이 {eventValue} 감소하였습니다.");
                            //player.Hp -= eventValue;
                        }
                        break;
                    case 2:
                            Console.WriteLine("신전을 지나쳤습니다.");
                        break;
                }

               
                
            }

            //보물상자(아이템드랍or미믹)3:7
            void TreasureBox()
            {
                Console.WriteLine("보물상자를 발견했습니다.");
                Console.WriteLine("\n1.열어본다.\n2.지나간다");
                result = CheckInput(1, 2);
                switch (result)
                {
                    case 1:
                        eventNum = random.Next(1, 11);
                        if (eventNum <= 3)
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


            //상인or약탈자3:7
            void MerchantOrThief()
            {
                Console.WriteLine("상인과 조우했습니다.");
                Console.WriteLine("\n1.대화한다.\n2.지나간다");
                result = CheckInput(1, 2);
                switch (result)
                {
                    case 1:
                        eventNum = random.Next(1, 11);
                        if (eventNum <= 4)
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
                        Console.WriteLine("상인을 지나쳤습니다.");
                        break;
                }
            }

            //부서진 마차 이벤트
            //-> 부서진 마차 사이에서 보물발견 OR 몬스터 발견
            void brokenCart()
            {
                Console.WriteLine("부서진 마차를 발견했습니다.");
                Console.WriteLine("\n1.조사한다.\n2.지나간다");
                result = CheckInput(1, 2);
                switch (result)
                {
                    case 1:
                        eventNum = random.Next(1, 11);
                        if (eventNum <= 5)
                        {
                            Console.WriteLine("부서진 마차에서 보물을 발견했습니다.");
                            //아이템 드랍
                        }
                        else
                        {
                            Console.WriteLine("부서진 마차에서 몬스터를 발견했습니다.");
                            //몬스터와 전투
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
                result = CheckInput(1, 2);
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
                result = CheckInput(1, 2);
                switch (result)
                {
                    case 1:
                        eventNum = random.Next(1, 11);
                        if (eventNum <= 3)
                        {
                            Console.WriteLine("우는 아이를 발견했습니다.");
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
                result = CheckInput(1, 2);
                switch (result)
                {
                    case 1:
                        eventNum = random.Next(1, 11);
                        if (eventNum <= 7)
                        {
                            Console.WriteLine("부러진 검을 획득했습니다.");
                            //아이템 획득
                        }
                        else
                        {
                            Console.WriteLine("부러진 검을 지나쳤습니다.");
                            //아무것도 없음
                        }
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
                result = CheckInput(1, 2);
                switch (result)
                {
                    case 1:
                            Console.WriteLine("낚시를 통해 물고기를 잡았습니다.");
                            //물고기 획득
                        break;
                    case 2:
                        Console.WriteLine("낚시 스폿을 지나쳤습니다.");
                        break;
                }
            }

            //고블린 조우
            //-> 고블린과 대화(지능+) 고블린 굴복(힘+)

            //검복구 이벤트 상점 or 던전이벤트?
            //-> 검 복구시 좋은 검 OR 저주받은 검 OR 어이쿠 손이.. 3:4:3 (저주디버프 추가)

            //떠돌이 마법사 조우
            //-> 랜덤 스크롤 한개 획득 OR 스탯 랜덤 +1~3

            //고양이 조우 이벤트
            //-> 고양이 조우 시 인벤에 있는 물고기 하나 주고 돌려보냄
            //-> 고양이에게 물고기를 줄 시 마을로 복귀 시 고양이가 랜덤 템을 줌



        }
    }
    
}
