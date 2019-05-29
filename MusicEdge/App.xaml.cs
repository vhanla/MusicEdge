using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;

namespace MusicEdge
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        // https://msdn.microsoft.com/en-us/library/system.windows.shell.jumplist%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396
        private void JumpList_JumpItemsRejected(object sender, System.Windows.Shell.JumpItemsRejectedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} Jump Items Rejected:\n", e.RejectionReasons.Count);
            for (int i = 0; i < e.RejectionReasons.Count; ++i)
            {
                if (e.RejectedItems[i].GetType() == typeof(JumpPath))
                    sb.AppendFormat("Reason: {0}\tItem: {1}\n", e.RejectionReasons[i], ((JumpPath)e.RejectedItems[i]).Path);
                else
                    sb.AppendFormat("Reason: {0}\tItem: {1}\n", e.RejectionReasons[i], ((JumpTask)e.RejectedItems[i]).ApplicationPath);
            }

            MessageBox.Show(sb.ToString());
        }

        private void JumpList_JumpItemsRemovedByUser(object sender, System.Windows.Shell.JumpItemsRemovedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} Jump Items Removed by the user:\n", e.RemovedItems.Count);
            for (int i = 0; i < e.RemovedItems.Count; ++i)
            {
                sb.AppendFormat("{0}\n", e.RemovedItems[i]);
            }

            MessageBox.Show(sb.ToString());
        }
    }
}
