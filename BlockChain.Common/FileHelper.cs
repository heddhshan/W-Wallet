using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Common
{
    public static class FileHelper
    {

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void PositionFile(string sFileFullName)
        {
            try
            {
                if (!System.IO.File.Exists(sFileFullName)) return;
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                psi.Arguments = " /select," + sFileFullName;
                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex)
            {
                log.Error("PositionFile", ex);
            }
        }

    }

}
