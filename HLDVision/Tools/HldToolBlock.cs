using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HLDVision
{

    [Serializable]
    public class HldToolBlock : HldToolBase
    {
        public HldToolBlock()
        {
            toolJob = new HldJob();
            inputTool = new HldInnerToolBlock();
            inputTool.toolName = "Input";
            outputTool = new HldInnerToolBlock();
            outputTool.toolName = "Output";
        }

        public HldToolBlock(SerializationInfo info, StreamingContext context) : base(info, context) { }

        [Serializable]
        public class HldInnerToolBlock : HldToolBase
        {

            public HldInnerToolBlock()
            {
                inputTypeList = new Dictionary<string, Type>();
                outputTypeList = new Dictionary<string, Type>();
            }

            public HldInnerToolBlock(SerializationInfo info, StreamingContext context) : base(info, context) { }

            #region 안씀

            public override void InitInParmas()
            {
                //do nothing this tool
            }

            public override void InitOutParmas()
            {
                //do nothing this tool
            }

            public override void InitImageList()
            {
                //do nothing this tool
            }

            public override void InitOutProperty()
            {
                //do nothing this tool
            }

            #endregion

            public override void GetInParams()
            {
                outParams.Clear();

                lastRunSuccess = false;
                bool isGetValueSuccess = false;
                bool isSuccess = true;

                List<string> deletedKey = new List<string>();

                foreach (KeyValuePair<string, InputParams> param in inParams)
                {
                    Type keyType = outputTypeList[param.Key];

                    if (param.Value == null) continue;

                    isGetValueSuccess = true;

                    Type type = param.Value.GetType();

                    if (keyType != type)
                    {
                        if (type == null)
                            System.Windows.Forms.MessageBox.Show("Type is wrong");
                        else
                            System.Windows.Forms.MessageBox.Show(string.Format("Type is not match\r\nsrcType : {0}\r\ndstType : {1}", type.Name, keyType.Name));
                        deletedKey.Add(param.Key);
                        continue;
                    }

                    outParams.Add(param.Key, param.Value.GetValue());

                    if (outParams[param.Key] == null) isSuccess = false;
                }

                lastRunSuccess = isGetValueSuccess & isSuccess;

                foreach (string key in deletedKey)
                {
                    inParams[key] = null;
                }
            }

            public Dictionary<string, Type> inputTypeList;
            public Dictionary<string, Type> outputTypeList;

            public override void Run(bool isEditMode = false)
            {
            }
        }

        public HldInnerToolBlock inputTool;
        public HldInnerToolBlock outputTool;

        public Dictionary<string, object> innerInput
        { get { return inputTool.outParams; } }

        public Dictionary<string, object> innerOutput
        { get { return outputTool.outParams; } }

        HldJob toolJob;

        public HldJob ToolJob
        {
            get { return toolJob; }
            set { toolJob = value; }
        }

        #region 안씀

        public override void InitInParmas()
        {
            //do nothing this tool
        }

        public override void InitOutParmas()
        {
            //do nothing this tool
        }

        public override void InitImageList()
        {
            //do nothing this tool
        }

        #endregion

        public void RestoreTool()
        {
            foreach (string key in inputTool.outParams.Keys)
            {
                if (!inParams.ContainsKey(key))
                    inParams.Add(key, null);
            }

            foreach (string key in outputTool.inParams.Keys)
            {
                if (!outParams.ContainsKey(key))
                {
                    outParams.Add(key, Activator.CreateInstance(outputTool.outputTypeList[key]));
                    imageList.Add(new HldImageInfo(key));
                }
            }
        }

        public void AddInput(string key, Type type)
        {
            inputTool.inputTypeList.Add(key, type);

            InputParams value = null;
            if (tempInParams != null && tempInParams.ContainsKey(key))
                value = tempInParams[key];

            inParams.Add(key, value);

            inputTool.outParams.Add(key, Activator.CreateInstance(type));

            if (type.Equals(typeof(HldImage)))
                inputTool.imageList.Add(new HldImageInfo(key));

        }

        public void AddOutput(string key, Type type)
        {
            outputTool.outputTypeList.Add(key, type);

            InputParams value = null;
            if (tempOutputToolInParams != null && tempOutputToolInParams.ContainsKey(key))
                value = tempOutputToolInParams[key];

            outputTool.inParams.Add(key, value);

            outParams.Add(key, null);

            if (type.Equals(typeof(HldImage)))
            {
                HldImageInfo outputImageInfo = new HldImageInfo(key);
                outputTool.imageList.Add(outputImageInfo);
                imageList.Add(outputImageInfo);
            }
        }

        public List<object[]> GetInputs()
        {
            List<object[]> param = new List<object[]>();
            foreach (KeyValuePair<string, Type> pair in inputTool.inputTypeList)
            {
                object[] obj = new object[2];
                obj[0] = pair.Key;
                obj[1] = pair.Value.FullName;
                param.Add(obj);
            }
            return param;
        }

        public List<object[]> GetOutputs()
        {
            List<object[]> param = new List<object[]>();
            foreach (KeyValuePair<string, Type> pair in outputTool.outputTypeList)
            {
                object[] obj = new object[2];
                obj[0] = pair.Key;
                obj[1] = pair.Value.FullName;
                param.Add(obj);
            }
            return param;
        }

        [NonSerialized]
        Dictionary<string, InputParams> tempInParams;

        public void ClearInput()
        {
            tempInParams = new Dictionary<string, InputParams>();
            foreach (KeyValuePair<string, InputParams> pair in inParams)
            {
                if (pair.Value != null)
                    tempInParams.Add(pair.Key, pair.Value);
            }

            inputTool.outParams.Clear();
            outputTool.outParams.Clear();
            inputTool.inputTypeList.Clear();
            inParams.Clear();
            inputTool.imageList.Clear();
        }

        [NonSerialized]
        Dictionary<string, InputParams> tempOutputToolInParams;

        public void ClearOutput()
        {
            tempOutputToolInParams = new Dictionary<string, InputParams>();
            foreach (KeyValuePair<string, InputParams> pair in outputTool.inParams)
            {
                if (pair.Value != null)
                    tempOutputToolInParams.Add(pair.Key, pair.Value);
            }

            outputTool.inParams.Clear();
            outputTool.outParams.Clear();
            outputTool.outputTypeList.Clear();
            outParams.Clear();
            outputTool.imageList.Clear();
            imageList.Clear();
        }

        public override void GetInParams()
        {
            List<string> deletedKey = new List<string>();

            inputTool.lastRunSuccess = true;

            foreach (KeyValuePair<string, InputParams> param in inParams)
            {
                Type keyType = inputTool.inputTypeList[param.Key];

                object value = null;

                if (param.Value != null)
                {
                    Type type = param.Value.GetType();

                    if (keyType != type)
                    {
                        if (type == null)
                            System.Windows.Forms.MessageBox.Show("Type is wrong");
                        else
                            System.Windows.Forms.MessageBox.Show(string.Format("Type is not match\r\nsrcType : {0}\r\ndstType : {1}", type.Name, keyType.Name));
                        deletedKey.Add(param.Key);
                        continue;
                    }

                    value = param.Value.GetValue();
                }

                if (value == null) inputTool.lastRunSuccess = false;

                inputTool.outParams[param.Key] = value;
            }

            foreach (string key in deletedKey)
            {
                inParams[key] = null;
            }
        }

        public override void GetOutParams()
        {
            outputTool.GetInParams();
            foreach (KeyValuePair<string, object> item in outputTool.outParams)
            {
                this.outParams[item.Key] = item.Value;
            }
        }

        public override void InitOutProperty()
        {
            lastRunSuccess = false;
            List<string> keys = outParams.Keys.ToList();
            foreach (string key in keys)
            {
                this.outParams[key] = null;
            }
        }

        public override void Run(bool isEditMode = false)
        {
            if (isEditMode)
            {
                foreach (KeyValuePair<string, object> param in inputTool.outParams)
                {
                    HldImageInfo info = inputTool.imageList.Find((item) => { if (item.ImageName == param.Key) return true; return false; });
                    if (info != null)
                    {
                        if (param.Value is HldImage)
                        {
                            info.Image = param.Value as HldImage;
                        }
                    }
                }
            }

            if (toolJob != null)
                toolJob.Run(isEditMode);

            imageList = new List<HldImageInfo>();

            if (isEditMode)
            {
                foreach (KeyValuePair<string, InputParams> param in outputTool.inParams)
                {
                    if (param.Value == null) continue;
                    if (param.Value.Instance as HldToolBase == null) continue;
                    foreach (HldImageInfo imageinfo in (param.Value.Instance as HldToolBase).imageList)
                    {
                        imageList.Add(imageinfo);

                    }
                }
            }

            GetOutParams();
            if (!outputTool.lastRunSuccess) return;

            outputTool.imageList = imageList;

            lastRunSuccess = true;
        }
    }
}
