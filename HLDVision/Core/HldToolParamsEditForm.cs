using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HLDVision.Core
{
    public partial class HldToolParamsEditForm : Form
    {
        public HldToolParamsEditForm()
        {
            InitializeComponent();
        }


        HldToolBase subject;

        public HldToolBase Subject
        {
            get { return subject; }
            set
            {
                subject = value;
                CopyParams();
                InitParams();
            }
        }

        Dictionary<string, InputParams> inParams;
        Dictionary<string, object> outParams;

        void CopyParams()
        {
            inParams = new Dictionary<string, InputParams>();

            foreach (KeyValuePair<string, InputParams> param in subject.inParams)
            {
                inParams.Add(param.Key, param.Value);
            }

            outParams = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> param in subject.outParams)
            {
                outParams.Add(param.Key, null);
            }

        }

        void InitParams()
        {
            PropertyInfo[] properties = subject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            int iplCnt = 1; int ipNCnt = 1; int oplCnt = 1; int opNCnt = 1;

            foreach (PropertyInfo info in properties)
            {
                Attribute input = info.GetCustomAttribute(typeof(InputParamAttribute));
                if (input != null)
                {
                    if (subject.inParams.ContainsKey(info.Name))
                        lv_IpN.Items.Add(new ListViewItem(new string[] { ipNCnt++.ToString(), info.Name, info.PropertyType.Name }));
                    else
                        lv_Ipl.Items.Add(new ListViewItem(new string[] { iplCnt++.ToString(), info.Name, info.PropertyType.Name }));
                }

                Attribute output = info.GetCustomAttribute(typeof(OutputParamAttribute));
                if (output != null)
                {
                    if (subject.outParams.ContainsKey(info.Name))
                        lv_OpN.Items.Add(new ListViewItem(new string[] { opNCnt++.ToString(), info.Name, info.PropertyType.Name }));
                    else
                        lv_Opl.Items.Add(new ListViewItem(new string[] { oplCnt++.ToString(), info.Name, info.PropertyType.Name }));
                }
            }
        }

        private void btn_Remove_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == btn_InputRemove)
            {
                if (lv_IpN.SelectedItems.Count == 0) return;
                ListViewItem removingItem = lv_IpN.SelectedItems[0];
                lv_IpN.Items.Remove(removingItem);
                lv_Ipl.Items.Add(removingItem);
                lv_Ipl.Sort();

                inParams.Remove(removingItem.SubItems[1].Text);
            }
            else if (btn == btn_OutputRemove)
            {
                if (lv_OpN.SelectedItems.Count == 0) return;
                ListViewItem removingItem = lv_OpN.SelectedItems[0];
                lv_OpN.Items.Remove(removingItem);
                lv_Opl.Items.Add(removingItem);
                lv_Opl.Sort();

                outParams.Remove(removingItem.SubItems[1].Text);
            }
            else
                return;
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == btn_InputAdd)
            {
                if (lv_Ipl.SelectedItems.Count == 0) return;
                ListViewItem addingItem = lv_Ipl.SelectedItems[0];
                lv_Ipl.Items.Remove(addingItem);
                lv_IpN.Items.Add(addingItem);
                lv_IpN.Sort();

                string key = addingItem.SubItems[1].Text;
                InputParams value = null;
                if (subject.inParams.ContainsKey(key))
                    value = subject.inParams[key];

                inParams.Add(addingItem.SubItems[1].Text, value);
            }
            else if (btn == btn_OutputAdd)
            {
                if (lv_Opl.SelectedItems.Count == 0) return;
                ListViewItem addingItem = lv_Opl.SelectedItems[0];
                lv_Opl.Items.Remove(addingItem);
                lv_OpN.Items.Add(addingItem);
                lv_OpN.Sort();

                outParams.Add(addingItem.SubItems[1].Text, null);
            }
            else
                return;
        }

        private void btn_Confirm_Click(object sender, EventArgs e)
        {
            subject.inParams.Clear();
            subject.inParams = inParams;

            subject.outParams.Clear();
            subject.outParams = outParams;

            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            inParams.Clear();
            outParams.Clear();
            this.Close();
        }

        private void btn_Default_Click(object sender, EventArgs e)
        {
            Dictionary<string, InputParams> origin = new Dictionary<string, InputParams>();
            foreach (KeyValuePair<string, InputParams> param in subject.inParams)
            {
                origin.Add(param.Key, param.Value);
            }
            subject.inParams.Clear();
            subject.InitInParmas();

            foreach (KeyValuePair<string, InputParams> param in origin)
            {
                if (subject.inParams.ContainsKey(param.Key))
                    subject.inParams[param.Key] = param.Value;
            }

            origin.Clear();

            subject.outParams.Clear();
            subject.InitOutParmas();

            this.Close();
        }

        private void lv_Add_DoubleClick(object sender, EventArgs e)
        {
            ListView lv = sender as ListView;
            if (lv == lv_Ipl)
            {
                btn_Add_Click(btn_InputAdd, e);
            }
            else if (lv == lv_Opl)
            {
                btn_Add_Click(btn_OutputAdd, e);
            }
            else
                return;
        }

        private void lv_Remove_DoubleClick(object sender, EventArgs e)
        {
            ListView lv = sender as ListView;
            if (lv == lv_IpN)
            {
                btn_Remove_Click(btn_InputRemove, e);
            }
            else if (lv == lv_OpN)
            {
                btn_Remove_Click(btn_OutputRemove, e);
            }
            else
                return;
        }

    }
}
