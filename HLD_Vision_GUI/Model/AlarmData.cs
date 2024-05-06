using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HLD_Vision_GUI.Model
{
    public class AlarmData
    {
        static uint Count = 0;
        static public System.Collections.Generic.Dictionary<string, AlarmData> AlarmList = new System.Collections.Generic.Dictionary<string, AlarmData>();

        public enum Icons { ERROR, QUESTION, WARNING, INFORMATAION };

        public uint No { get { return no; } }
        uint no;

        public string errCaption;
        public string errContent;

        public Icons icons;
        public MessageBoxButtons buttons;

        AlarmData(string errCaption, string errContent, Icons icons, MessageBoxButtons buttons)
        {
            no = Count++;
            this.errCaption = errCaption;
            this.errContent = errContent;
            this.icons = icons;
            this.buttons = buttons;
            AlarmList.Add(errCaption, this);
        }

        static AlarmData()
        {
            new AlarmData("Err#0001", "System", Icons.INFORMATAION, MessageBoxButtons.OK);
        }
    }
}
