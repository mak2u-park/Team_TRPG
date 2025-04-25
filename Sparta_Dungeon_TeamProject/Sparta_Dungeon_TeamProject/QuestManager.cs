using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace Sparta_Dungeon_TeamProject
{
    public partial class Program
    {
        static void DisplayQuest()
        {
            QuestManager questManager = new QuestManager(); // 퀘스트 매니저 인스턴스 생성
            questManager.QuestList(); // 퀘스트 초기화
            Console.WriteLine("퀘스트 목록:");
            foreach (var quest in questManager.questList)
            {
                Console.WriteLine($"[{questManager.Idexnum(quest)}] {quest.Name} {quest.Description} | 진행도 :{quest.Progress}/{quest.TargetCount} | 보상 :{quest.Gold}G | {(quest.IsCompleted ? "완료" : "미완료")}");
            }

        }
        public class QuestManager // internal -> public 변경
        {
            public List<Quest> questList = new List<Quest>(); // 퀘스트 리스트


            // 초기 퀘스트 설정 (외부에서 호출하여 퀘스트 추가)
            public void QuestList()
            {
                AddQuest("고블린 토벌", "고블린을 5마리 사냥해주세요!", 5, 5000);
                AddQuest("오크 토벌", "오크를 3마리 사냥해주세요!", 3, 7000);
                AddQuest("광부와 수레 조사", "일주일전 들어간 광부와 수레가 돌아오지않아 조사를 의뢰받았다.", 1, 6000);
                AddQuest("부서진 오두막 조사", "부서진 오두막을 조사해주세요!", 1, 3000);
                AddQuest("아이를 구해주세요", "며칠전 우리마을의 한 아이가 사라져 조사를 의뢰받았다.", 1, 10000);
            }
            public int Idexnum(Quest quest)
            {
                return questList.IndexOf(quest); // 퀘스트 인덱스 반환
            }
            // 외부에서 퀘스트 추가 기능
            public void AddQuest(string name, string description, int targetCount, int gold)
            {
                questList.Add(new Quest(name, description, targetCount, gold));
            }

            // 특정 이름의 퀘스트 찾기
            public Quest FindQuest(string name)
            {
                return questList.Find(quest => quest.Name == name);
            }

            // 현재 진행 가능한 퀘스트 목록 반환 (수락된 퀘스트만)
            public List<Quest> GetAcceptedQuests()
            {
                return questList.Where(quest => quest.Agree && !quest.IsCompleted).ToList();
            }

            // 모든 퀘스트 목록 반환
            public List<Quest> GetAllQuests()
            {
                return questList;
            }
            public class Quest
            {
                public string Name { get; } // 퀘스트 이름
                public string Description { get; } // 퀘스트 설명
                public int TargetCount { get; set; } // 퀘스트 목표 수치
                public int Progress { get; set; } = 0; // 퀘스트 진행도
                public int Gold { get; set; } = 0; // 퀘스트 보상 골드
                public bool IsCompleted { get; private set; } = false; // 퀘스트 완료 여부
                public bool Agree { get; set; } = false; // 퀘스트 수락 여부


                public Quest(string name, string description, int targetcount, int gold)
                {
                    Name = name;
                    Description = description;
                    TargetCount = targetcount;
                    Gold = gold;
                }

                public void CompleteQuest()
                {
                    IsCompleted = true;
                }

                public void ProgressQuest()
                {
                    if (Progress < TargetCount)
                    {
                        Progress++;
                        Console.WriteLine($"퀘스트 '{Name}' 진행중... {Progress}/{TargetCount}");
                    }
                    else
                    {
                        CompleteQuest();
                    }
                }
                public void AcceptQuest() //퀘스트 수락
                {
                    Agree = true;
                    Console.WriteLine($"퀘스트 '{Name}' 수락!");
                }

                public void RejectQuest()//퀘스트 거절
                {
                    Agree = false;
                    Console.WriteLine($"퀘스트 '{Name}' 거절!");
                }




                public void ShowQuestProgress()//퀘스트 진행도 (상시로 보여주면 좋을것같습니다)
                {
                    Console.WriteLine($"퀘스트 '{Name}' 진행도: {Progress}/{TargetCount}");
                }
                public void ProgressCount(int count)// 퀘스트 진행도 증가
                {
                    Progress += count;
                    if (Progress >= TargetCount)
                    {
                        CompleteQuest();
                    }
                }
                
            }
        }
    }
}