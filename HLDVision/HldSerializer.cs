using HLDCameraDevice;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace HLDVision
{
    public class HldSerializer
    {
        void CreateFilePath(string FilePath)
        {
            if (!File.Exists(FilePath))
            {
                // File Path에서 Directory Path를 받아옮
                string dc = Path.GetDirectoryName(FilePath);

                // Directory가 없을 경우 생성
                if (!Directory.Exists(dc))
                    Directory.CreateDirectory(dc);

                using (File.Create(FilePath)) { };
            }
        }

        /// <summary>
        /// Object를 저장파일로 Serializing
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public bool Serializing(string Path, Object Obj)
        {
            Stream memoryStream = new MemoryStream();

            try
            {
                CreateFilePath(Path);

                // Binary 형태로 serialzie 하기 위한 포맷 생성
                BinaryFormatter formatter = new BinaryFormatter();

                // obj 개체를 스트림 serialize
                formatter.Serialize(memoryStream, Obj);

                // 파일을 쓰기 위해 스트림 생성
                // using 사용시에 구문 끝나면 자동 스트림 삭제
                using (Stream fileStream = new FileStream(Path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    memoryStream.Position = 0;
                    memoryStream.CopyTo(fileStream);
                    fileStream.Dispose();
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("[Serializing] " + ex.ToString());
                HLDCommon.HldLogger.Log.Error("[Serializing] " + ex.ToString());
                return false;
            }
            finally
            {
                if (memoryStream != null) memoryStream.Dispose();
            }
        }

        /// <summary>
        /// 저장파일을 Object로 Deserializing
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public Object Deserializing(string Path)
        {
            if (!File.Exists(Path)) return null;

            // 새로운 Object 생성
            Object Obj = new Object();

            // Binary 형태로 serialzie 하기 위한 포맷 생성
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                // 파일을 읽기 위해 스트림 생성
                // using 사용시에 구문 끝나면 자동 스트림 삭제
                using (Stream stream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    // 스트림을 Deserialize하여 Obj개체 생성
                    Obj = (Object)formatter.Deserialize(stream);
                    stream.Dispose();
                }

                return Obj;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("[Deserializing] " + ex.ToString());
                HLDCommon.HldLogger.Log.Error("[Deserializing] " + ex.ToString());
                return null;
            }
        }

        void ClearToolOutput(HldToolBase visionTool)
        {
            if (visionTool is HldToolBlock)
            {
                HldToolBlock toolBlock = visionTool as HldToolBlock;
                foreach (HldToolBase tool in toolBlock.ToolJob.ToolList)
                {
                    ClearToolOutput(tool);
                }
                ClearToolOutput(toolBlock.inputTool);
                ClearToolOutput(toolBlock.outputTool);
            }

            //output param 안에 들어간 결과값을 날려줘야 Save됨.
            List<string> keyList = visionTool.outParams.Keys.ToList();
            foreach (string key in keyList)
            {
                System.Reflection.PropertyInfo pinfo = visionTool.GetType().GetProperty(key, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (pinfo != null)
                    pinfo.SetValue(visionTool, null);

                if (visionTool.outParams[key] is HldImage)
                    visionTool.outParams[key] = null;
            }
        }

        HldImage GetAcqusitionImage(HldToolBase visionTool)
        {
            HldImage input = null;
            HldAcquisition acqusition = visionTool as HldAcquisition;
            if (acqusition != null)
                input = acqusition.InputImage.Clone(true);
            return input;
        }

        void SetAcqusitionImage(HldToolBase visionTool, HldImage input)
        {
            HldAcquisition acqusition = visionTool as HldAcquisition;
            if (acqusition != null)
                acqusition.InputImage = input;
        }

        public bool SaveTool(HldToolBase visionTool, string toolPath)
        {
            HldImage input = GetAcqusitionImage(visionTool);

            ClearToolOutput(visionTool);

            System.Reflection.FieldInfo finfo = typeof(InputParams).GetField("instance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            Dictionary<string, object> tempInparams = new Dictionary<string, object>();
            List<string> inputKeyList = visionTool.inParams.Keys.ToList();
            foreach (string inputKey in inputKeyList)
            {
                InputParams inputValue = visionTool.inParams[inputKey];
                if (inputValue == null) continue;
                tempInparams.Add(inputKey, finfo.GetValue(inputValue));
                finfo.SetValue(inputValue, null);
            }

            bool isSuccess = Serializing(toolPath, visionTool);

            foreach (KeyValuePair<string, object> inParams in tempInparams)
            {
                finfo.SetValue(visionTool.inParams[inParams.Key], inParams.Value);
            }

            SetAcqusitionImage(visionTool, input);
            visionTool.Run(true);

            return isSuccess;
        }

        public HldToolBase LoadTool(string toolPath)
        {
            HldToolBase tool = null;
            if (Path.GetExtension(toolPath).ToLower() == ".tlf")
            {
                tool = (HldToolBase)Deserializing(toolPath);
                if (tool != null) tool.Run(true);
            }

            return tool;
        }

        public bool SaveJob(HldJob visionJob, string jobPath)
        {
            HldToolBase acqTool = null;
            HldImage input = null;

            foreach (HldToolBase tool in visionJob.ToolList)
            {
                if (tool is HldAcquisition)
                {
                    input = GetAcqusitionImage(tool);
                    acqTool = tool;
                }
                ClearToolOutput(tool);
            }

            bool isSuccess = Serializing(jobPath, visionJob);

            if (acqTool != null && input != null && input.Width != 0 && input.Height != 0)
            {
                SetAcqusitionImage(acqTool, input);
                visionJob.Run(true);
            }

            return isSuccess;
        }

        public HldJob LoadJob(string jobPath)
        {
            DateTime dt = DateTime.Now;

            HldJob job = null;
            if (Path.GetExtension(jobPath).ToLower() == ".job")
                job = (HldJob)Deserializing(jobPath);

            if (job != null) job.Name = System.IO.Path.GetFileNameWithoutExtension(jobPath);

            HldFunc.TimeWatch(dt, "LoadJob Time");

            return job;
        }

        public bool SaveObjectJob(object visionJob, string jobPath)
        {
            bool isSuccess = Serializing(jobPath, visionJob);

            return isSuccess;
        }

        public object LoadObjectJob(string jobPath)
        {
            DateTime dt = DateTime.Now;

            object job = null;
            if (Path.GetExtension(jobPath).ToLower() == ".job")
                job = (object)Deserializing(jobPath);

            HldFunc.TimeWatch(dt, "LoadJob Time");

            return job;
        }

        public static void Serializeing(object sender, SerializationInfo info, StreamingContext context)
        {
            FieldInfo[] finfos = info.ObjectType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (FieldInfo finfo in finfos)
            {
                Attribute nonSerializedAttribute = finfo.GetCustomAttribute(typeof(NonSerializedAttribute));
                Attribute compilerGeneratedAttribute = finfo.GetCustomAttribute(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute));
                if (nonSerializedAttribute != null || compilerGeneratedAttribute != null) continue;

                if (finfo.FieldType == typeof(Mat))
                {
                    Mat mat = finfo.GetValue(sender) as Mat;
                    if (mat != null && !mat.IsDisposed && mat.Width != 0 && mat.Height != 0)
                    {
                        int rows = mat.Rows;
                        int cols = mat.Cols;
                        MatType matType = mat.Type();

                        object data;
                        if (matType == MatType.CV_8UC1)
                        {
                            data = new byte[rows, cols];
                            mat.GetArray(0, 0, (byte[,])data);
                        }
                        else if (matType == MatType.CV_8UC3)
                        {
                            data = new Vec3b[rows, cols];
                            mat.GetArray(0, 0, (Vec3b[,])data);
                        }
                        else if (matType == MatType.CV_8UC4)
                        {
                            data = mat.ImEncode();
                        }
                        else if (matType == MatType.CV_32FC1)
                        {
                            data = new float[rows, cols];
                            mat.GetArray(0, 0, (float[,])data);
                        }
                        else if (matType == MatType.CV_32FC3)
                        {
                            data = new Point3f[rows, cols];
                            mat.GetArray(0, 0, (Point3f[,])data);
                        }
                        else if (matType == MatType.CV_64FC1)
                        {
                            data = new double[rows, cols];
                            mat.GetArray(0, 0, (double[,])data);
                        }
                        else if (matType == MatType.CV_64FC3)
                        {
                            data = new Point3d[rows, cols];
                            mat.GetArray(0, 0, (Point3d[,])data);
                        }
                        else
                        {
                            throw new Exception("this MatType can not be serialized. Modify the code");
                        }

                        info.AddValue(finfo.Name, data);
                        info.AddValue(finfo.Name + "_Rows", rows);
                        info.AddValue(finfo.Name + "_Cols", cols);
                        info.AddValue(finfo.Name + "_Type", matType.Value);
                    }
                    else
                    {
                        info.AddValue(finfo.Name, null);
                    }
                }
                else
                    info.AddValue(finfo.Name, finfo.GetValue(sender));
            }
        }

        public static void Deserializeing(object sender, SerializationInfo info, StreamingContext context)
        {
            FieldInfo[] finfos = info.ObjectType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (FieldInfo finfo in finfos)
            {
                Attribute attribute = finfo.GetCustomAttribute(typeof(NonSerializedAttribute));
                if (attribute == null)
                {
                    if (finfo.FieldType == typeof(Mat))
                    {
                        try
                        {
                            object data = info.GetValue(finfo.Name, typeof(object));

                            if (data == null)
                            {
                                finfo.SetValue(sender, new Mat());
                                continue;
                            }

                            int rows = (int)info.GetValue(finfo.Name + "_Rows", typeof(int));
                            int cols = (int)info.GetValue(finfo.Name + "_Cols", typeof(int));
                            MatType matType = new MatType((int)info.GetValue(finfo.Name + "_Type", typeof(int)));

                            Mat mat;

                            if (matType == MatType.CV_8UC1)
                                mat = new Mat(rows, cols, matType, (byte[,])data);
                            else if (matType == MatType.CV_8UC3)
                                mat = new Mat(rows, cols, matType, (Vec3b[,])data);
                            else if (matType == MatType.CV_8UC4)
                                mat = Mat.ImDecode((byte[])data, LoadMode.Unchanged);
                            else if (matType == MatType.CV_32FC1)
                                mat = new Mat(rows, cols, matType, (float[,])data);
                            else if (matType == MatType.CV_32FC3)
                                mat = new Mat(rows, cols, matType, (Point3f[,])data);
                            else if (matType == MatType.CV_64FC1)
                                mat = new Mat(rows, cols, matType, (double[,])data);
                            else if (matType == MatType.CV_64FC3)
                                mat = new Mat(rows, cols, matType, (Point3d[,])data);
                            else
                                throw new Exception("this MatType can not be serialized. Modify the code");

                            finfo.SetValue(sender, mat);
                        }
                        catch (SerializationException ex)
                        {
                            HLDCommon.HldLogger.Log.Error(ex.ToString());
                        }
                    }
                    else if (!finfo.Attributes.HasFlag(FieldAttributes.NotSerialized))
                    {
                        try
                        {
                            object value = info.GetValue(finfo.Name, finfo.FieldType);
                            finfo.SetValue(sender, value);
                        }
                        catch (SerializationException ex)
                        {
                            HLDCommon.HldLogger.Log.Error(ex.ToString());
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                if (finfo.Name == "cameraType")
                                {
                                    object value = info.GetValue(finfo.Name, typeof(Enum));
                                    CameraType ct = (CameraType)value;
                                    finfo.SetValue(sender, ct);
                                }
                            }
                            catch
                            {
                                System.Windows.Forms.MessageBox.Show("[Deserializing] " + ex.ToString());
                                HLDCommon.HldLogger.Log.Error("[Deserializing] " + ex.ToString());
                            }
                            continue;
                        }
                    }
                }
            }
        }
    }
}
