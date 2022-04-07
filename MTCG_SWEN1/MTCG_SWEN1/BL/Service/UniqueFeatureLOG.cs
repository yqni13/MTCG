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
        // Get LogData from Battle and write list of strings into .txt file -> saved into directory 'BattleLogs'.
        public static void GetLogAsTXT(List<String> logdata)
        {
            StreamWriter logfile = new StreamWriter("../../../BattleLogs/BattleLog" + DateTime.UtcNow.AddHours(1).ToString("yyyyMMdd_HH-mm-ss") + "." + "txt", true);
            foreach (var line in logdata)
            {
                logfile.Write(line);
                logfile.Write("\n");
            }
            logfile.Close();            
        }
    }
}
