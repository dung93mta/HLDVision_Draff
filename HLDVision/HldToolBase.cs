using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenCvSharp.Extensions;
using System.Reflection;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HLDVision
{
    [Serializable]
    public class HldImageInfo
    {
        public HldImageInfo(string imageName)
        {
            this.ImageName = imageName;
        }

        public readonly string ImageName;
        [NonSerialized]
        public HldImage Image;
        public delegate void DrwaingFunc(HLDVision.Display.HldDisplayView display);
        public DrwaingFunc drawingFunc;

        public override string ToString()
        {
            return ImageName;
        }
    }

    [Serializable]
    public abstract class HldToolBase : INotifyPropertyChanged, ISerializable
    {
        public string Group { get; set; }

        public Dictionary<string, InputParams> inParams = new Dictionary<string, InputParams>();
        public Dictionary<string, object> outParams = new Dictionary<string, object>();
        public List<HldImageInfo> imageList = new List<HldImageInfo>();

        protected HldToolBase()
        {
            Group = "HLD Misc";

            inParams.Clear();
            outParams.Clear();

            InitInParmas();
            InitOutParmas();
            InitImageList();

            HashCode = this.ToString() + DateTime.Now.Ticks;
        }

        public override string ToString()
        {
            string defaultName = this.GetType().Name;

            if (!string.IsNullOrEmpty(toolName))
                return toolName;
            return defaultName.Substring(3, defaultName.Length - 3);
        }

        public string toolName;

        public readonly string HashCode;

        public double lastProcessTime;

        public bool lastRunSuccess;

        public object GetInParam(string inName)
        {
            object inParam = null;
            foreach (KeyValuePair<string, InputParams> param in inParams)
            {
                if (param.Value == null) continue;
                if (param.Key == inName)
                {
                    inParam = param.Value.GetValue();
                    break;
                }
            }
            return inParam;
        }

        virtual public void GetInParams()
        {
            List<string> deletedKey = new List<string>();

            foreach (KeyValuePair<string, InputParams> param in inParams)
            {
                PropertyInfo property = this.GetType().GetProperty(param.Key);

                if (property == null) continue;

                object value = null;

                if (param.Value == null)
                {
                    if (property.PropertyType.IsValueType)
                        value = Activator.CreateInstance(property.PropertyType);
                }
                else
                {
                    Type type = param.Value.GetType();

                    if (property.PropertyType != type)
                    {
                        if (type == null)
                            System.Windows.Forms.MessageBox.Show("Type is wrong");
                        else
                            System.Windows.Forms.MessageBox.Show(string.Format("Type is not match\r\nsrcType : {0}\r\ndstType : {1}", type.Name, property.PropertyType.Name));
                        deletedKey.Add(param.Key);
                        continue;
                    }

                    value = param.Value.GetValue();
                }

                property.SetValue(this, value);
            }

            foreach (string key in deletedKey)
            {
                inParams[key] = null;
            }
        }

        virtual public void GetOutParams()
        {
            List<string> keys = new List<string>(outParams.Keys);
            foreach (string key in keys)
            {
                PropertyInfo property = this.GetType().GetProperty(key, BindingFlags.Instance | BindingFlags.Public);
                if (property == null) continue;
                object value = property.GetValue(this);
                outParams[key] = value;
            }
            keys.Clear(); keys = null;
        }

        public string[] GetInputParamNames()
        {
            string[] names = new string[inParams.Keys.Count];

            int count = 0;
            bool isOutputImage = false;
            foreach (string name in inParams.Keys)
            {
                if (name == "InputImage")
                {
                    names[count++] = names[0];
                    names[0] = name;
                }
                else
                    names[count++] = name;
            }

            if (isOutputImage)
                names[count++] = "OutputImage";

            return names;
        }

        public string[] GetOutputParamNames()
        {
            string[] names = new string[outParams.Keys.Count];

            int count = 0;
            bool isOutputImage = false;
            foreach (string name in outParams.Keys)
            {
                names[count++] = name;
            }

            if (isOutputImage)
                names[count++] = "OutputImage";

            return names;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected HldToolBase(SerializationInfo info, StreamingContext context)
        {
            HldSerializer.Deserializeing(this, info, context);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            HldSerializer.Serializeing(this, info, context);
        }

        public delegate void RanHandler(object sender, HldToolBase tool);

        [field: NonSerialized]
        public event RanHandler Ran;

        protected void NotifyRan(object sender, HldToolBase tool)
        {
            if (Ran != null)
            {
                System.Windows.Forms.Control control = Ran.Target as System.Windows.Forms.Control;

                if (control != null && control.InvokeRequired)
                    control.Invoke(Ran, sender, tool);
                else
                    Ran(sender, tool);
            }
        }

        public void OnRan()
        {
            NotifyRan(this, this);
        }

        abstract public void InitInParmas();

        abstract public void InitOutParmas();

        abstract public void InitOutProperty();

        abstract public void InitImageList();

        abstract public void Run(bool isEditMode = false);
    }


    [Serializable]
    public class InputParams
    {
        public InputParams(object instance, string fieldName)
        {
            this.instance = instance;
            this.fieldName = fieldName;
            this.instanceHashcode = (instance as HldToolBase).HashCode;
        }

        string instanceHashcode;
        public string InstanceHashcode
        {
            get { return instanceHashcode; }
        }

        object instance;
        public object Instance
        {
            get { return instance; }
            set
            {
                instance = value;
                instanceHashcode = (instance as HldToolBase).HashCode;
            }
        }

        string fieldName;
        public string FieldName
        {
            get { return fieldName; }
            set
            {
                fieldName = value;
            }
        }

        public object GetValue()
        {
            FieldInfo fInputInfo = Instance.GetType().GetField("outParams");

            if (fInputInfo == null)
                return null;

            Dictionary<string, object> outParams = fInputInfo.GetValue(Instance) as Dictionary<string, object>;

            return outParams[FieldName];
        }

        new public Type GetType()
        {
            FieldInfo fInputInfo = Instance.GetType().GetField("outParams");

            if (fInputInfo == null) return null;

            Dictionary<string, object> outParams = fInputInfo.GetValue(Instance) as Dictionary<string, object>;

            if (!outParams.ContainsKey(FieldName)) return null;

            if (outParams[FieldName] == null)
            {
                fInputInfo = Instance.GetType().GetField(FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (fInputInfo == null)
                {
                    PropertyInfo pInputInfo = Instance.GetType().GetProperty(FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (pInputInfo == null)
                    {
                        if (instance is HldToolBlock)
                        {
                            HldToolBlock toolBlock = instance as HldToolBlock;
                            if (toolBlock.outputTool.outputTypeList[FieldName] != null)
                                return toolBlock.outputTool.outputTypeList[FieldName];
                            else
                                return null;
                        }
                        else if (instance is HldToolBlock.HldInnerToolBlock)
                        {
                            HldToolBlock.HldInnerToolBlock innerToolBlock = instance as HldToolBlock.HldInnerToolBlock;
                            if (innerToolBlock.inputTypeList[FieldName] != null)
                                return innerToolBlock.inputTypeList[FieldName];
                            else
                                return null;
                        }
                        else
                            return null;
                    }
                    else
                        return pInputInfo.PropertyType;
                }
                else
                {
                    return fInputInfo.FieldType;
                }
            }

            return outParams[FieldName].GetType();
        }
    }
}
