using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparta_Dungeon_TeamProject
{
    // 직업 DB # SetData()
    public enum JobType
    {
        전사 = 1,
        마법사,
        과학자,
        대장장이,
        영매사,
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

    // 직업별 기본 능력치 DB # SetData()
    public static class JobDB
    {
        public static Dictionary<JobType, JobData> Jobs = new Dictionary<JobType, JobData>
            {   // 직업명 / 공격력 / 방어력 / 최대체력 / 최대마나
                { JobType.전사, new JobData(5, 2, 70, 100 ) },
                { JobType.마법사, new JobData(1, 1, 50, 0) },
                { JobType.과학자, new JobData(8, 2, 50, 150) },
                { JobType.대장장이, new JobData(1, 5, 70, 120) },
                { JobType.영매사, new JobData(5, 4, 100, 75) }
            };
    }
}
