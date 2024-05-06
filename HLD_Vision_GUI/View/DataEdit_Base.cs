using HLD_Vision_GUI.AutoThread;
using HLD_Vision_GUI.Model;
using HLDVision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HLD_Vision_GUI.View
{
    public partial class DataEdit_Base : Form
    {
        public DataEdit_Base()
        {
            InitializeComponent();
        }

        public DataEdit_Base(HLDRecipe _recipeData, App.Process _process, string _jobName)
        {
            if (_recipeData == null) return;

            mRecipeData = _recipeData;
            mProcess = _process;
            mJobName = _jobName;

            mSerializer = new HldSerializer();
            mJobPath = Path.Combine(App.JobPath, _jobName);
            try
            {
                mJob = mSerializer.LoadJob(mJobPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                IsPossible = false;
                return;
            }

            if (mJob == null || mJob.ToolList.Count == 0) return;

            GetJobInfo(mJob);

            InitializeComponent();
        }

        public bool IsPossible = false;
        protected HldSerializer mSerializer;
        protected string mJobPath;
        protected HLDRecipe mRecipeData;
        protected string mJobName;
        protected App.Process mProcess;
        protected AlignResult mAlignResult = new AlignResult();
        protected HldJob mJob;

        //public abstract void btn_Run_Click(object sender, EventArgs e);

        //public abstract void btn_Save_Click(object sender, EventArgs e);

        public int mJobinfoIndex;
        protected bool GetJobInfo(HldJob _job, int _blockindex = 0)
        {
            if (_job == null) return false;
            try
            {
                mAlignResult.JobInfo.Add(new Dictionary<Type, List<sCheckToolInfo>>());
                foreach (HldToolBase toolbase in _job.ToolList)
                {
                    if (!mAlignResult.JobInfo[_blockindex].ContainsKey(toolbase.GetType()))
                        mAlignResult.JobInfo[_blockindex].Add(toolbase.GetType(), new List<sCheckToolInfo>());

                    mAlignResult.JobInfo[_blockindex][toolbase.GetType()].Add(new sCheckToolInfo(toolbase, false));

                    if (toolbase is HldAcquisition)
                    {
                        toolbase.InitOutProperty();
                        (toolbase as HldAcquisition).OpenDevice();

                        mAlignResult.JobInfo[_blockindex][toolbase.GetType()][0].NeedCheck = true;
                    }

                    if (toolbase is HldRegion)
                        (toolbase as HldRegion).IsAutoIndex = false;

                    if (toolbase is HldTemplateMatch)
                        (toolbase as HldTemplateMatch).IsAutoIndex = false;

                    if (toolbase is HldToolBlock)
                    {
                        GetJobInfo((toolbase as HldToolBlock).ToolJob, mAlignResult.JobInfo[_blockindex][typeof(HldToolBlock)].Count);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        protected double NormalizeTheta(double _rad)
        {
            double nomalrad = _rad + 3 / 2 * Math.PI;
            while (nomalrad > Math.PI / 4)
            {
                nomalrad -= Math.PI / 2;
            }
            return nomalrad;
        }
    }
}
