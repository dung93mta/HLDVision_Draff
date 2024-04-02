using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp.CPlusPlus;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Reflection;
using HLDCommon;

namespace HLDVision
{
    [Serializable]
    public class HldDataLog : HldToolBase
    {
        public HldDataLog()
        {
            Count = 5;
        }
        public HldDataLog(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region InParams
        int count;
        public int Count
        {
            get { return count; }
            set
            {
                count = value;
                InitInParmas();
            }
        }

        bool isDisplayType;
        public bool IsDisplayType { get { return isDisplayType; } set { isDisplayType = value; NotifyPropertyChanged("IsDisplayType"); } }

        public override void InitInParmas()
        {
            if (inParams.Count > Count)
                for (int i = inParams.Count - 1; i >= Count; i--)
                    inParams.Remove("Input[" + i + "]");
            else
                for (int i = inParams.Count; i < Count; i++)
                {
                    inParams.Add("Input[" + i + "]", null);
                }
        }

        dynamic[] input;
        public dynamic[] Input
        {
            get
            {
                if (input == null) input = new dynamic[Count];
                return input;
            }
            set { input = value; }
        }

        #endregion

        #region OutParams
        [Serializable]
        public class Valuestruct
        {
            public Type Type;
            public string Name;
            public string Value;
            public Valuestruct(Type t, string n, string v)
            {
                Type = t; Name = n; Value = v;
            }
            public string ToString(bool isDisplayType = false)
            {
                string str = Name;
                if (isDisplayType)
                {
                    if (Type != null)
                        str += "(" + Type.Name + ")";
                }
                return str;

            }
        }

        [NonSerialized]
        List<List<Valuestruct>> gridLog;
        public List<List<Valuestruct>> GridLog
        {
            get
            {
                if (gridLog == null) gridLog = new List<List<Valuestruct>>();
                return gridLog;
            }
            set { }
        }

        public override void InitOutParmas()
        {
        }
        #endregion

        HldImageInfo inputImageInfo;
        public override void InitImageList()
        {
            inputImageInfo = new HldImageInfo(string.Format("[{0}] InputImage", this.ToString()));
            imageList.Add(inputImageInfo);
        }

        public override void InitOutProperty()
        {
            lastRunSuccess = false;
            GridLog.Clear();
        }

        public override void Run(bool isEditMode = false)
        {
            int count = -1;
            string log = null;
            foreach (string str in inParams.Keys)
            {
                List<Valuestruct> Row = new List<Valuestruct>();
                count++;
                if (inParams[str] == null) continue;
                Input[count] = inParams[str].GetValue();

                if (Input[count] == null) continue;

                Type inType = Input[count].GetType();
                string inputToolName = inParams[str].Instance.ToString();
                string inputParamName = inParams[str].FieldName.ToString();

                Row.Add(new Valuestruct(null, inputToolName, null));
                Row.Add(new Valuestruct(inType, inputParamName, null));

                if (!inType.IsClass)
                {
                    GetStructValue(inputParamName, Input[count], Row);
                }
                else
                {
                    if (inType.Name.Contains("[]"))
                    {
                        int length = Input[count].Length;
                        for (int i = 0; i < length; i++)
                        {
                            Row[Row.Count - 1] = new Valuestruct(inType, inputParamName + "[" + i + "]", null);
                            if (Input[count][i].GetType().IsClass)
                                GetClassValue(inputParamName, Input[count][i], Row);
                            else
                                GetStructValue(inputParamName, Input[count][i], Row);
                            //GridLog.Add(Row);
                            List<Valuestruct> Rowtemp = new List<Valuestruct>();
                            Row = Rowtemp;
                            Row.Add(null);
                            Row.Add(null);
                        }
                    }
                    else
                    {
                        GetClassValue(inputParamName, Input[count], Row);
                    }
                }
                log = Convertlog();

            }
            HldLogger.Log.Debug(log);

            NotifyPropertyChanged("GridLog");
            lastRunSuccess = true;
        }

        string Convertlog()
        {
            bool b = IsDisplayType;
            string log = null;
            for (int i = 0; i < GridLog.Count; i++)
            {
                for (int j = 0; j < GridLog[i].Count; j++)
                {
                    if (GridLog[i][j] == null)
                        continue;
                    if (j == 0)
                        log += "\r\n";
                    if (j == 1)
                        continue;
                    //log += "\r\t<"+ GridLog[i][j].ToString(true) + "> ";
                    else
                        log += GridLog[i][j].ToString(true) + ": " + GridLog[i][j].Value + ", ";
                }
            }
            return log;
        }

        void GetClassValue(string name, object inobject, List<Valuestruct> row)
        {
            Type inType = inobject.GetType();
            int rowCount = row.Count;
            System.Reflection.PropertyInfo[] originFields = inType.GetProperties();
            if (inType == typeof(string))
            {
                row.Add(new Valuestruct(inType, inType.Name, inobject.ToString()));
                GridLog.Add(row);
                return;
            }
            else
            {
                foreach (System.Reflection.PropertyInfo info in originFields)
                {
                    // Class의 경우엔 재귀호출은 필요없지 않을까...?
                    if (info.PropertyType.IsClass) continue;
                    try
                    {
                        GetStructValue(info.Name, info.GetValue(inobject), row);
                        List<Valuestruct> rowtemp = new List<Valuestruct>();
                        row = rowtemp;
                        for (int i = 0; i < rowCount; i++)
                            row.Add(null);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        void GetStructValue(string name, object inobject, List<Valuestruct> row, int count = 0)
        {
            if (count == 3) return;
            Type inType = inobject.GetType();
            double value;
            int rowCount = row.Count;
            if (double.TryParse(inobject.ToString(), out value) || inType == typeof(Boolean))
            {
                if (name == "SizeOf") return;
                row.Add(null);
                row[rowCount] = (new Valuestruct(inType, name, inobject.ToString()));
                GridLog.Add(row);
                return;
            }

            FieldInfo[] originFields = inType.GetFields();

            foreach (FieldInfo info in originFields)
            {
                try
                {
                    //log += info.Name + ": " + info.GetValue(inobject).ToString() + ", ";                    
                    //row.Add(new Valuestruct(inType, inType.Name, inobject.ToString()));
                    //Log.Add(row);
                    GetStructValue(info.Name, info.GetValue(inobject), row, ++count);
                    List<Valuestruct> rowtemp = new List<Valuestruct>();
                    row = rowtemp;
                    for (int i = 0; i < rowCount; i++)
                        row.Add(null);
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}
