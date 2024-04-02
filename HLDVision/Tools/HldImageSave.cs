using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp.CPlusPlus;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace HLDVision
{

    [Serializable]
    public class HldImageSave : HldToolBase
    {
        public HldImageSave()
        {
            saveFileName = null;
            saveFilePath = null;
        }
        public HldImageSave(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region InParams

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
        }

        string saveFileName;

        public string SaveFileName
        {
            get { return saveFileName; }
            set
            {
                saveFileName = value;
                NotifyPropertyChanged("SaveFileName");
            }
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        #endregion

        #region OutParams

        public override void InitOutParmas()
        {
        }

        string saveFilePath;


        public string SaveFilePath
        {
            get { return saveFilePath; }
            set
            {
                saveFilePath = value;
                NotifyPropertyChanged("SaveFilePath");
            }
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
        }

        public override void Run(bool isEditMode = false)
        {
            inputImageInfo.Image = InputImage;

            if (InputImage == null)
            {
                lastRunSuccess = false;
                return;
            }

            if (!System.IO.Directory.Exists(SaveFilePath))
            {
                lastRunSuccess = false;
                return;
            }

            string filename = SaveFileName + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpg";

            string savefile = System.IO.Path.Combine(SaveFilePath, filename);

            InputImage.Mat.SaveImage(savefile);

            lastRunSuccess = true;
        }
    }
}
