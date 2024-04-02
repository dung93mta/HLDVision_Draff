using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HLDVision.Edit.Base;
using HLDCameraDevice;
using HLDVision.Core;

namespace HLDVision.Edit
{
    public partial class HldJobEdit : HldToolEditBase
    {
        public event EventHandler OnSaved;
        public HldJobEdit()
        {
            InitializeComponent();

            hldToolBlockTree.ToolSelect += ToolBlockTree_ToolSelect;
            this.Disposed += (a, b) =>
            {
                HldDisplayViewEdit.Display.Dispose();
            };
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            IsToolListButtonVisible = true;

            IsAutoRunButtonVisible = true;
            base.OnLayout(e);
        }

        void ToolBlockTree_ToolSelect(object sender, HldToolBase tool)
        {
            base.HldDisplayViewEdit.Subject = tool;
        }

        protected override void tsb_ToolList_Click(object sender, EventArgs e)
        {
            hldToolBlockTree.ShowToolList();
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldJob Job
        {
            get { return hldToolBlockTree.Job; }
            set { hldToolBlockTree.Job = value; lbl_CurrentJob.Text = value.Name; }
        }

        protected override void tsb_Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.InitialDirectory = PATH_JOB;
            fd.Filter = "Job files (*.job)|*.job|ToolBlock files (*.tlf)|*.tlf|All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.Multiselect = false;
            if (fd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            if (string.IsNullOrEmpty(fd.FileName)) return;
            this.Cursor = Cursors.WaitCursor;
            HldJob job = null;
            HldToolBase tool;
            string extension = System.IO.Path.GetExtension(fd.FileName).ToLower();

            if (extension.Equals(".job"))
                job = serializer.LoadJob(fd.FileName);
            else if (extension.Equals(".tlf"))
            {
                tool = serializer.LoadTool(fd.FileName);
                if (!(tool is HldToolBlock))
                    return;
                job = (tool as HldToolBlock).ToolJob;
            }

            this.Cursor = Cursors.Default;

            if (job == null)
            {
                MessageBox.Show("Job load fail");
                return;
            }

            Job.ToolList = job.ToolList;
            hldToolBlockTree.RedrawAllNode();

            lbl_CurrentJob.Text = System.IO.Path.GetFileNameWithoutExtension(fd.FileName);

            base.HldDisplayViewEdit.ClearImage();
        }

        protected override void tsb_Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.InitialDirectory = PATH_JOB;
            string fname = lbl_CurrentJob.Text;
            fd.FileName = fname;
            fd.Filter = "Job files (*.job)|*.job|All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.DefaultExt = "*.job";
            if (fd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            this.Cursor = Cursors.WaitCursor;
            this.Cursor = Cursors.Default;

            string name = System.IO.Path.GetFileNameWithoutExtension(fd.FileName);
            lbl_CurrentJob.Text = name;
            Job.Name = name;
            bool success = serializer.SaveJob(Job, fd.FileName);

            if (!success)
                MessageBox.Show("Job save fail");
            else
            {
                MessageBox.Show("Job save success");
                if (OnSaved != null) OnSaved(this, null);
            }
        }

        protected override void tsb_Run_Click(object sender, EventArgs e)
        {
            if (Job != null)
                Job.Run(true);

            float zoom = hldDisplayViewEdit.Display.ZoomRatio;
            System.Drawing.Point location = hldDisplayViewEdit.Display.imageLocation;

            UpdateDisplay();

            if (zoom != 1 || location != new System.Drawing.Point(0, 0))
            {
                hldDisplayViewEdit.Display.ZoomRatio = zoom;
                hldDisplayViewEdit.Display.imageLocation = location;
            }
        }
        
        void UpdateDisplay()
        {
            if (Job != null)
            {
                Job.OnRan();

                if (Job.LastRunSuccess)
                    ToolStatusImage = Properties.Resources.SuccessIcon;
                else
                    ToolStatusImage = Properties.Resources.FailIcon;

                ToolStatusText = string.Format("{0:F2}ms", Job.LastRunTime);
            }

            HldDisplayViewEdit.RefreshImage();
            hldToolBlockTree.Refresh();
        }

        System.Threading.CancellationTokenSource autoRunTokenSource = new System.Threading.CancellationTokenSource();

        protected override void tsb_AutoRun_Click(object sender, EventArgs e)
        {
            if (base.IsAutoRunning)
            {
                autoRunTokenSource.Cancel(true);
                autoRunTokenSource.Dispose();
            }
            else
            {
                base.IsAutoRunning = true;
                autoRunTokenSource = new System.Threading.CancellationTokenSource();
                Task autoRunTask = Task.Factory.StartNew(() =>
                {
                    HldAcquisition acq = Job["Acquisition"] as HldAcquisition;
                    while (true)
                    {
                        if (Job != null) Job.Run(true);
                        this.Invoke(new Action(UpdateDisplay));
                        //System.Threading.Thread.Sleep(40);

                        if (autoRunTokenSource.Token == null)
                            break;
                        if (autoRunTokenSource.Token.IsCancellationRequested)
                            autoRunTokenSource.Token.ThrowIfCancellationRequested();

                        if (acq != null && acq.CameraType == CameraType.Image && acq.CurrentImageIndex == acq.InputImageList.Count)
                            break;
                    }
                }, autoRunTokenSource.Token).ContinueWith((task) =>
                {
                    base.IsAutoRunning = false;
                });
            }
        }

        public void Run()
        {
            tsb_Run_Click(null, null);
        }
    }
}
