using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academic_Performance_Tracking_System.Utilities
{
    class SessionManager
    {
        private static UInt16 currentProfileID;
        private static string? currentProfileName;

        public SessionManager() 
        { 

        }

        public SessionManager(string currentProfileName)
        {
            SessionManager.currentProfileName = currentProfileName;
        }

        public static void SetCurrentSessionID(UInt16 currentSessionID)
        {
            currentProfileID = currentSessionID;
        }

        public static int GetCurrentSessionID()
        {
            return currentProfileID;
        }

        public static string GetCurrentSessionName()
        {
            return currentProfileName;
        }
    }
}
