using HLD_Vision_GUI.AutoThread;
using HLD_Vision_GUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HLD_Vision_GUI.View
{
    /// <summary>
    /// Interaction logic for SetIODlg.xaml
    /// </summary>
    public partial class SetIODlg : Window
    {
        private App.Process process;
        private Dictionary<App.Process, ProcessData> dictionary;
        private bool inout;//true - in, false - out
        private List<TextBox> enablebox = new List<TextBox>();

        public SetIODlg(Dictionary<App.Process, ProcessData> dictionary, App.Process process, bool inout)
        {
            InitializeComponent();

            this.dictionary = dictionary;
            this.process = process;
            this.inout = inout;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = string.Format("Set {0} Address", inout ? "Input" : "Ouput");
            gbMain.Header = inout ? "Input" : "Output";
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Vertical;
            GroupBox gb;
            UniformGrid ug;
            Label lbl;
            TextBox tb;
            Dictionary<string, int> dic;
            foreach (var kv in dictionary)
            {
                if (!kv.Value.Use) continue;

                gb = new GroupBox();
                gb.Header = kv.Key.ToString();
                gb.IsEnabled = (kv.Key == process);

                dic = inout ? kv.Value.IOParam.Inputs : kv.Value.IOParam.Outputs;

                ug = new UniformGrid();
                ug.Columns = 2;
                ug.Rows = dic.Count;

                if (inout || kv.Key == App.Process.Common)
                {
                    foreach (var addr in dic)
                    {
                        lbl = new Label();
                        lbl.Content = addr.Key;
                        lbl.FontSize = 12;
                        ug.Children.Add(lbl);

                        tb = new TextBox();
                        tb.Text = addr.Value.ToString();
                        tb.FontSize = 12;
                        tb.Tag = addr.Key;

                        ug.Children.Add(tb);

                        if (kv.Key == process) enablebox.Add(tb);
                    }
                }
                else//normal thread's outputs
                {
                    // ready
                    AutoThread_Base.OutSignal tag = AutoThread_Base.OutSignal.Ready;
                    lbl = new Label();
                    lbl.Content = tag;
                    lbl.FontSize = 12;
                    ug.Children.Add(lbl);

                    tb = new TextBox();
                    tb.Text = dic[tag.ToString()].ToString();
                    tb.Tag = tag;

                    ug.Children.Add(tb);
                    if (kv.Key == process) enablebox.Add(tb);

                    // acqcomp
                    if (kv.Value.UseAcqComp)
                    {
                        tag = AutoThread_Base.OutSignal.AcqComp;
                        lbl = new Label();
                        lbl.Content = tag;
                        lbl.FontSize = 12;
                        ug.Children.Add(lbl);

                        tb = new TextBox();
                        tb.Text = dic[tag.ToString()].ToString();
                        tb.Tag = tag;

                        ug.Children.Add(tb);
                        if (kv.Key == process) enablebox.Add(tb);
                    }
                    else ug.Rows -= 1;

                    // ok, ng
                    for (int i = 0; i < 2; i++)
                    {
                        tag = AutoThread_Base.OutSignal.OK + i;
                        lbl = new Label();
                        lbl.Content = tag;
                        lbl.FontSize = 12;
                        ug.Children.Add(lbl);

                        tb = new TextBox();
                        tb.Text = dic[tag.ToString()].ToString();
                        tb.Tag = tag;

                        ug.Children.Add(tb);
                        if (kv.Key == process) enablebox.Add(tb);
                    }

                    // ok2, ng2
                    if (kv.Value.JudgeType == EJudgeType.Dual)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            tag = AutoThread_Base.OutSignal.OK2 + i;
                            lbl = new Label();
                            lbl.Content = tag;
                            lbl.FontSize = 12;
                            ug.Children.Add(lbl);

                            tb = new TextBox();
                            tb.Text = dic[tag.ToString()].ToString();
                            tb.Tag = tag;

                            ug.Children.Add(tb);
                            if (kv.Key == process) enablebox.Add(tb);
                        }
                    }
                    else ug.Rows -= 2;

                    if (kv.Value.ThreadName.Contains("Panel_Insp"))
                    {
                        if (kv.Key.ToString().Contains("Object1"))
                        {
                            tag = AutoThread_Base.OutSignal.InspNG;
                            lbl = new Label();
                            lbl.Content = tag;
                            lbl.FontSize = 12;
                            ug.Children.Add(lbl);

                            tb = new TextBox();
                            tb.Text = dic[tag.ToString()].ToString();
                            tb.Tag = tag;

                            ug.Children.Add(tb);
                            if (kv.Key == process) enablebox.Add(tb);
                        }
                        if (kv.Key.ToString().Contains("Object2"))
                        {
                            tag = AutoThread_Base.OutSignal.InspNG2;
                            lbl = new Label();
                            lbl.Content = tag;
                            lbl.FontSize = 12;
                            ug.Children.Add(lbl);

                            tb = new TextBox();
                            tb.Text = dic[tag.ToString()].ToString();
                            tb.Tag = tag;

                            ug.Children.Add(tb);
                            if (kv.Key == process) enablebox.Add(tb);

                        }
                    }
                }

                gb.Content = ug;
                panel.Children.Add(gb);
            }
            viewer.Content = panel;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            int addr;
            Dictionary<string, int> IOs = inout ? dictionary[process].IOParam.Inputs : dictionary[process].IOParam.Outputs;
            foreach (var tb in enablebox)
            {
                if (int.TryParse(tb.Text, out addr) && addr >= 0)
                {
                    IOs[tb.Tag.ToString()] = addr;
                }
                else
                {
                    MessageBox.Show("Setting value is invalid, Recheck value!!!. (must be higher then 0)");
                    return;
                }
            }
            DialogResult = true;
            Close();
        }
    }
}
