using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.BL.Service
{
    class UniqueFeatureLOG
    {
        public static void GetLogAsTXT(List<String> logdata)
        {
            StreamWriter logfile = new StreamWriter("../../../BattleLogs/BattleLog" + DateTime.UtcNow.AddHours(1).ToString("yyyyMMdd_HH-mm-ss") + "." + "txt", true);
            foreach (var line in logdata)
            {
                logfile.Write(line);
                logfile.Write("\n");
            }
            logfile.Close();
            Console.WriteLine("Battle LogFile was additionally writen into .txt file.");
        }
    }
}
