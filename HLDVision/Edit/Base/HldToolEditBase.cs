using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;

namespace HLDVision.Edit.Base
{
    public partial class HldToolEditBase : UserControl, INotifyPropertyChanged
    {
        public string PATH_JOB = "C:\\HLD Vision\\Job";
        public string PATH_CALIB = "C:\\HLD Vision\\Calibration"; 

        public HldToolEditBase()
        {
            InitializeComponent();

            this.tsb_Run.Click += tsb_Run_Click;
            this.tsb_Load.Click += tsb_Load_Click;
            this.tsb_Save.Click += tsb_Save_Click;
            this.tsb_ToolList.Click += tsb_ToolList_Click;

            this.tabControl.KeyDown += ToolEditBase_KeyDown;

            ToolStatusImage = Properties.Resources.FailIcon;
            ToolStatusText = string.Format("{0:F2}ms", 0);

            InitDisplayViewEdit();
        }


        void ToolEditBase_KeyDown(object sender, KeyEventArgs e)
        {

        }

        void subject_Ran(object sender, HldToolBase tool)
        {
            float oldZoom = hldDisplayViewEdit.Display.ZoomRatio;
            Point oldLocation = hldDisplayViewEdit.Display.imageLocation;

            hldDisplayViewEdit.SetLastImage();

            hldDisplayViewEdit.Display.ZoomRatio = oldZoom;
            hldDisplayViewEdit.Display.imageLocation = oldLocation;
        }

        private HldToolBase subject;

        internal void SetSubject(HldToolBase subject)
        {
            if (this.subject != null)
                this.subject.Ran -= subject_Ran;
            this.subject = subject;
            this.subject.Ran += subject_Ran;

            lbl_CurrentJob.Text = subject.ToString();
            UpdateStatus();

            hldDisplayViewEdit.Subject = subject;
            NotifySubjectChanged(this, this.subject);
        }

        internal HldToolBase GetSubject()
        {
            return this.subject;
        }

        protected delegate void SubjectChangedHandler(object sender, HldToolBase tool);
        protected event SubjectChangedHandler SubjectChanged;

        public void NotifySubjectChanged(object sender, HldToolBase tool)
        {
            if (SubjectChanged != null)
                SubjectChanged(sender, tool);
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected HldDisplayViewEdit HldDisplayViewEdit
        {
            get { return this.hldDisplayViewEdit; }
            set { this.hldDisplayViewEdit = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected StatusStrip StatusStrip
        {
            get { return this.statusStrip; }
            set { this.statusStrip = value; }
        }

        virtual protected void InitDisplayViewEdit()
        {

        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected bool IsAutoRunButtonVisible
        {
            get { return tsb_AutoRun.Visible; }
            set { tsb_AutoRun.Visible = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected bool IsToolListButtonVisible
        {
            get { return tsb_ToolList.Visible; }
            set { tsb_ToolList.Visible = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected string ToolStatusText
        {
            get { return tssl_ProcessTime.Text; }
            set { tssl_ProcessTime.Text = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected Image ToolStatusImage
        {
            get { return tssl_SuccessImage.Image; }
            set { tssl_SuccessImage.Image = value; }
        }

        void UpdateStatus()
        {
            if (subject.lastRunSuccess)
                ToolStatusImage = Properties.Resources.SuccessIcon;
            else
                ToolStatusImage = Properties.Resources.FailIcon;

            ToolStatusText = string.Format("{0:F2}ms", subject.lastProcessTime);
        }

        virtual protected void tsb_ToolList_Click(object sender, System.EventArgs e)
        {
        }

        public bool DeepCopy(object origin, object src)
        {
            Type originType = origin.GetType();
            Type srcType = src.GetType();

            if (originType != srcType) return false;

            System.Reflection.FieldInfo[] originFields = originType.GetFields(System.Reflection.BindingFlags.Instance |
                                                                              System.Reflection.BindingFlags.Public |
                                                                              System.Reflection.BindingFlags.NonPublic |
                                                                              System.Reflection.BindingFlags.Static |
                                                                              System.Reflection.BindingFlags.DeclaredOnly);

            foreach (System.Reflection.FieldInfo info in originFields)
            {
                if (info.FieldType == typeof(HldImageInfo)) continue;
                info.SetValue(origin, info.GetValue(src));
            }

            return true;
        }

        protected HldSerializer serializer = new HldSerializer();

        virtual protected void tsb_Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Tool files (*.tlf)|*.tlf|All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.Multiselect = false;
            if (fd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            if (string.IsNullOrEmpty(fd.FileName)) return;

            this.Cursor = Cursors.WaitCursor;
            HldToolBase tool = serializer.LoadTool(fd.FileName);
            this.Cursor = Cursors.Default;

            if (tool == null)
            {
                MessageBox.Show("Tool load fail");
                return;
            }

            if (!DeepCopy(subject, tool))
            {
                MessageBox.Show(string.Format("Tool type not match.\r\nTool file type : {0}", tool.ToString()));
                return;
            }

            SetSubject(subject);
            HldDisplayViewEdit.ClearImage();

            lbl_CurrentJob.Text = System.IO.Path.GetFileNameWithoutExtension(fd.FileName);
        }

        virtual protected void tsb_Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "Tool files (*.tlf)|*.tlf|All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.DefaultExt = "*.tlf";
            if (fd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            this.Cursor = Cursors.WaitCursor;
            bool success = serializer.SaveTool(GetSubject(), fd.FileName);
            this.Cursor = Cursors.Default;

            if (!success)
                MessageBox.Show("Tool save fail");
            else
                MessageBox.Show("Tool save success");

        }

        virtual protected void tsb_Run_Click(object sender, EventArgs e)
        {
            this.Focus();
            subject.GetInParams();
            if (subject != null)
            {
                subject.InitOutProperty();
                DateTime lastProcessTimeStart = DateTime.Now;
                subject.Run(true);
                subject.OnRan();
                subject.lastProcessTime = DateTime.Now.Subtract(lastProcessTimeStart).TotalMilliseconds;
            }

            UpdateStatus();
            float oldZoom = hldDisplayViewEdit.Display.ZoomRatio;
            Point oldLocation = hldDisplayViewEdit.Display.imageLocation;

            hldDisplayViewEdit.RefreshImage();

            hldDisplayViewEdit.Display.ZoomRatio = oldZoom;
            hldDisplayViewEdit.Display.imageLocation = oldLocation;
        }

        bool isAutoRunning = false;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected bool IsAutoRunning
        {
            get { return isAutoRunning; }
            set
            {
                isAutoRunning = value;

                if (!isAutoRunning)
                {
                    tsb_AutoRun.Text = "Auto Run";
                }
                else
                {
                    tsb_AutoRun.Text = "Stop";
                }
            }
        }

        System.Threading.CancellationTokenSource autoRunTokenSource = new System.Threading.CancellationTokenSource();

        virtual protected void tsb_AutoRun_Click(object sender, EventArgs e)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
