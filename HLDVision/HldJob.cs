using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDVision
{
    [Serializable]
    public class HldJob
    {
        List<HldToolBase> toolList = new List<HldToolBase>();

        public List<HldToolBase> ToolList
        {
            get { return toolList; }
            set
            {
                toolList.Clear();
                foreach (HldToolBase tool in value)
                {
                    toolList.Add(tool);
                }
            }
        }

        public HldToolBase this[string name]
        {
            get
            {
                HldToolBase selectTool = null;

                foreach (HldToolBase tool in toolList)
                {
                    if (name == tool.ToString())
                        selectTool = tool;
                }
                return selectTool;
            }
        }

        public HldJob()
        {

        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        bool lastRunSuccess;

        public bool LastRunSuccess { get { return lastRunSuccess; } }

        double lastRunTime;

        public double LastRunTime { get { return lastRunTime; } }

        public void RestoreLink()
        {
            foreach (HldToolBase tool in toolList)
            {
                if (tool == null) continue;
                foreach (KeyValuePair<string, InputParams> param in tool.inParams)
                {
                    InputParams dstValue = param.Value;
                    if (dstValue == null) continue;

                    foreach (HldToolBase t in toolList)
                    {
                        if (t == tool)
                            continue;
                        if (t.HashCode == dstValue.InstanceHashcode)
                        {
                            dstValue.Instance = t;
                            break;
                        }
                    }

                }
            }
        }

        public List<KeyValuePair<string, double>> GetLastProcessTimes()
        {
            //System.Collections.Generic.Dictionary<string, double> lastProcessTimes = new System.Collections.Generic.Dictionary<string, double>();
            List<KeyValuePair<string, double>> lastProcessTimes = new List<KeyValuePair<string, double>>();
            double TT = 0;
            foreach (HldToolBase tool in toolList)
            {
                lastProcessTimes.Add(new KeyValuePair<string, double>(tool.ToString(), tool.lastProcessTime));
                TT += tool.lastProcessTime;
            }
            lastProcessTimes.Insert(0, new KeyValuePair<string, double>("Total Time", TT));
            return lastProcessTimes;
        }

        public HldToolBase GetLastRunTool()
        {
            return toolList.Last();
        }

        public Dictionary<string, object> GetLastRunOutParams()
        {
            return toolList.Last().outParams;
        }


        public struct OutImageInfo
        {
            public OutImageInfo(string ToolName, string ImageName, Mat Image, Image overray)
            {
                this.ToolName = ToolName;
                this.ImageName = ImageName;
                this.Image = Image;
                this.overray = overray;
            }

            public readonly string ToolName;
            public readonly string ImageName;
            [NonSerialized]
            public readonly Mat Image;
            public readonly Image overray;

            public override string ToString()
            {
                return string.Format("Tool : {0,-20}Name : {1}", ToolName, ImageName);
            }
        }

        public List<OutImageInfo> GetOutImages()
        {
            List<OutImageInfo> outImages = new List<OutImageInfo>();

            foreach (HldToolBase tool in toolList)
            {
                foreach (KeyValuePair<string, object> param in tool.outParams)
                {
                    if (param.Value == null) continue;
                    //if (param.Value is Mat) ;
                    //outImages.Add(new OutImageInfo(tool.ToString(), param.Key, param.Value as Mat));
                }
            }

            return outImages;
        }

        public void Run(bool isEditMode = false)
        {
            if (toolList.Count == 0) return;

            DateTime runTimeStart = DateTime.Now;
            lastRunSuccess = true;

            RestoreLink();

            foreach (HldToolBase tool in toolList)
            {
                DateTime lastProcessTimeStart = DateTime.Now;

                tool.GetInParams();
                tool.InitOutProperty();
                tool.Run(isEditMode);

                //if (tool.lastRunSuccess)
                {
                    tool.GetOutParams();
                    tool.OnRan();
                }

                tool.lastProcessTime = DateTime.Now.Subtract(lastProcessTimeStart).TotalMilliseconds;
                System.Diagnostics.Debug.WriteLine(tool.ToString() + " Time: " + tool.lastProcessTime);

                lastRunSuccess &= tool.lastRunSuccess;
            }

            lastRunTime = DateTime.Now.Subtract(runTimeStart).TotalMilliseconds;
            System.Diagnostics.Debug.WriteLine("Total VisionJob Time: " + lastRunTime);

            GC.Collect();
        }

        public void RunWithoutAcq()
        {
            if (toolList.Count == 0) return;

            DateTime runTimeStart = DateTime.Now;
            lastRunSuccess = true;

            RestoreLink();

            foreach (HldToolBase tool in toolList)
            {
                if (tool is HldAcquisition) continue;

                DateTime lastProcessTimeStart = DateTime.Now;

                tool.GetInParams();
                tool.InitOutProperty();
                tool.Run();

                tool.GetOutParams();
                tool.OnRan();

                tool.lastProcessTime = DateTime.Now.Subtract(lastProcessTimeStart).TotalMilliseconds;
                System.Diagnostics.Debug.WriteLine(tool.ToString() + " Time: " + tool.lastProcessTime);

                lastRunSuccess &= tool.lastRunSuccess;
            }

            lastRunTime = DateTime.Now.Subtract(runTimeStart).TotalMilliseconds;
            System.Diagnostics.Debug.WriteLine("Total VisionJob Time: " + lastRunTime);

            GC.Collect();
        }

        public void RunBeforeTool(HldToolBase toolbase)
        {
            try
            {
                if (toolList.Count == 0) return;
                if (toolbase == null) { Run(); return; }

                DateTime runTimeStart = DateTime.Now;
                lastRunSuccess = true;

                RestoreLink();

                foreach (HldToolBase tool in toolList)
                {
                    if (tool == toolbase) break;

                    DateTime lastProcessTimeStart = DateTime.Now;

                    tool.GetInParams();
                    tool.InitOutProperty();
                    tool.Run();

                    tool.GetOutParams();
                    tool.OnRan();

                    tool.lastProcessTime = DateTime.Now.Subtract(lastProcessTimeStart).TotalMilliseconds;
                    System.Diagnostics.Debug.WriteLine(tool.ToString() + " Time: " + tool.lastProcessTime);

                    lastRunSuccess &= tool.lastRunSuccess;
                }

                lastRunTime = DateTime.Now.Subtract(runTimeStart).TotalMilliseconds;
                System.Diagnostics.Debug.WriteLine("Total VisionJob Time: " + lastRunTime);

                GC.Collect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }
        }
        public void OnRan()
        {
            NotifyRan(this, this);
        }

        public delegate void RanHandler(object sender, HldJob job);

        [field: NonSerialized]
        public event RanHandler Ran;

        protected void NotifyRan(object sender, HldJob job)
        {
            if (Ran != null)
                Ran(sender, job);
        }

    }
}
