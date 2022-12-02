using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Share
{
    public interface IMainWindow
    {
        void UpdateNodity(string info);
        void ShowProcessing(string msg = "Processing");
        void ShowProcesed(string msg = "Ready");

        void UpdateFinalStatus();

        BlockChain.Share.MainWindowHelper GetHelper();

        bool GetIsRunning();
    }

}
