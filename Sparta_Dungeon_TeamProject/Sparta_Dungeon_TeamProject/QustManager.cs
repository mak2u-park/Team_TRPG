using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparta_Dungeon_TeamProject
{
    internal class QustManager
    {
        List<Quest> questList = new List<Quest>(); // 퀘스트 리스트
        public void QustList()
        {
            questList.Add(new Quest("고블린 토벌", "고블린을 5마리 사냥해주세요!", 5));
            questList.Add(new Quest("오크 토벌", "오크를 3마리 사냥해주세요!", 3));
            questList.Add(new Quest("광부와 수레 조사", "일주일전 들어간 광부와 수레가 돌아오지않아 조사를 의뢰받았다.", 1));
            questList.Add(new Quest("부서진 오두막 조사", "부서진 오두막을 조사해주세요!", 1));
            questList.Add(new Quest("아이를 구해주세요", "며칠전 우리마을의 한 아이가 사라져 조사를 의뢰받았다.", 1));
        }


        
        public class Quest
        {
            public string Name { get; }// 퀘스트 이름
            public string Description { get; }// 퀘스트 설명
            public int TargetCount { get; set; } // 퀘스트 목표 수치
            public int Progress { get; set; } = 0; // 퀘스트 진행도
            public bool IsCompleted { get; private set; } = false;// 퀘스트 완료 여부
            public bool Agree { get; set; } = false;// 퀘스트 수락 여부
            public Quest(string name, string description, int targetcount)
            {
                Name = name;
                Description = description;
                TargetCount = targetcount;
            }


            public void CompleteQuest()
            {
                IsCompleted = true;
                Console.WriteLine($"퀘스트 '{Name}' 완료!");
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


            public void ShowQuestInfo()// 퀘스트 상세 정보(개별 선택할때 나오면 좋을것같습니다.)
            {
                Console.WriteLine($"퀘스트 이름: {Name}");
                Console.WriteLine($"퀘스트 설명: {Description}");
                Console.WriteLine($"목표 수치: {TargetCount}");
                Console.WriteLine($"진행도: {Progress}/{TargetCount}");
                Console.WriteLine($"완료 여부: {(IsCompleted ? "완료" : "미완료")}");
                Console.WriteLine($"수락 여부: {(Agree ? "수락" : "미수락")}");
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
