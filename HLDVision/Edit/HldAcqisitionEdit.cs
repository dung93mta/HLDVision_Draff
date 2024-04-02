using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;
using HLDVision.Edit.Base;
using HLDCameraDevice;

namespace HLDVision.Edit
{
    public partial class HldAcqisitionEdit : HldToolEditBase
    {
        public HldAcqisitionEdit()
        {
            InitializeComponent();

            InitBinding();
            this.SubjectChanged += AcqisitionEdit_SubjectChanged;
            this.HldDisplayViewEdit.SelectedImageChanged += DisplayViewEdit_SelectedImageChanged;
            this.Disposed += AcqisitionEdit_Disposed;
            thumbnailPictureBoxList = new PictureBox[4] { pb_Img1, pb_Img2, pb_Img3, pb_Img4 };
            thumbnailCurrentLabelList = new Label[4] { lbl_Img1C, lbl_Img2C, lbl_Img3C, lbl_Img4C };
            thumbnailPositionLabelList = new Label[4] { lbl_Img1P, lbl_Img2P, lbl_Img3P, lbl_Img4P };
        }


        protected override void tsb_Run_Click(object sender, EventArgs e)
        {
            if (isLive)
            {
                LiveStop();
            }
            base.tsb_Run_Click(sender, e);
        }

        void cb_CamNo_SelectedValueChanged(object sender, EventArgs e)
        {
            LiveStop();
            if (cb_CamNo.Items.Count > 0)
                Subject.CameraNumber = (int)cb_CamNo.SelectedIndex;
        }

        void AcqisitionEdit_Disposed(object sender, EventArgs e)
        {
            LiveStop();
        }

        Label[] thumbnailPositionLabelList;
        Label[] thumbnailCurrentLabelList;
        PictureBox[] thumbnailPictureBoxList;

        protected override void InitDisplayViewEdit()
        {
            HldDisplayViewEdit.InsertCustomImage(0, new HldImageInfo("[Acquisition] LiveView"));
        }

        void DisplayViewEdit_SelectedImageChanged(object sender, HldImageInfo imageInfo)
        {
            if (imageInfo.ImageName.Contains("LiveView") && !chkBox_isHwTrigger.Checked)
                LiveStart();
            else
                LiveStop();
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldAcquisition Subject
        {
            get { return base.GetSubject() as HldAcquisition; }
            set { base.SetSubject(value); }
        }

        void AcqisitionEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            LiveStop();
            source.DataSource = Subject;
            source.ResetBindings(true);
            RefreshThumbnailes(Subject.InputImageList);
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldAcquisition), null);

            cb_CamType.DisplayMember = "Key"; cb_CamType.ValueMember = "Value";
            cb_CamType.DataSource = Enum.GetValues(typeof(CameraType)).Cast<CameraType>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_CamType.DataBindings.Add("SelectedValue", source, "CameraType", true, DataSourceUpdateMode.OnPropertyChanged);

            DataBindings.Add("cameraType", source, "CameraType", true, DataSourceUpdateMode.Never);
            cb_CamNo.DataBindings.Add("SelectedValue", source, "CameraNumber", true, DataSourceUpdateMode.OnPropertyChanged);

            DataBindings.Add("CurretnImageIndex", source, "CurrentImageIndex", true, DataSourceUpdateMode.Never);

            cb_GrayType.DisplayMember = "Key"; cb_GrayType.ValueMember = "Value";
            cb_GrayType.DataSource = Enum.GetValues(typeof(HldAcquisition.GrayType)).Cast<HldAcquisition.GrayType>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_GrayType.DataBindings.Add("SelectedValue", source, "eGrayType", true, DataSourceUpdateMode.OnPropertyChanged);

            chb_ToGray.DataBindings.Add("Checked", source, "IsToGray", true, DataSourceUpdateMode.OnPropertyChanged);
            cb_HorizontalFilp.DataBindings.Add("Checked", source, "IsHorizontalFilp", true, DataSourceUpdateMode.OnPropertyChanged);
            cb_VerticalFilp.DataBindings.Add("Checked", source, "IsVerticalFilp", true, DataSourceUpdateMode.OnPropertyChanged);

            cb_Rotation.DisplayMember = "Key"; cb_Rotation.ValueMember = "Value";
            cb_Rotation.DataSource = Enum.GetValues(typeof(HldAcquisition.Rotation)).Cast<HldAcquisition.Rotation>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_Rotation.DataBindings.Add("SelectedValue", source, "Rotate", true, DataSourceUpdateMode.OnPropertyChanged);

            chkBox_isHwTrigger.DataBindings.Add("Checked", source, "IsHwTrigger", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_ExposureTime.DataBindings.Add("Value", source, "ExposureTime", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_GrabDelay.DataBindings.Add("Value", source, "BeforeGrabDelay", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public CameraType cameraType
        {
            get { throw new Exception("dd"); }
            set
            {
                if (value == CameraType.Image) gb_ImgList.Visible = true;

                else gb_ImgList.Visible = false;

                if (Subject != null && Subject.CameraDeivce != null)
                {
                    cb_CamNo.DisplayMember = "Key"; cb_CamNo.ValueMember = "Value";

                    System.Collections.Generic.Dictionary<string, int> list = new System.Collections.Generic.Dictionary<string, int>();
                    int camCount = Subject.CameraDeivce.GetConnectedCamCount();
                    //camCount = new Random().Next(2) + 1;
                    for (int i = 0; i < camCount; i++)
                    {
                        list.Add(i.ToString() + " : " + Subject.CameraDeivce.GetCameraSeiralNo(i), i);
                    }

                    int beforeCameraNumber = Subject.CameraNumber;
                    cb_CamNo.DataSource = list.ToList();
                    if (beforeCameraNumber >= 0 && beforeCameraNumber < camCount)
                        cb_CamNo.SelectedValue = beforeCameraNumber;
                }
                else
                {
                    cb_CamNo.DataSource = null;
                }
            }
        }

        private void btn_Acqusition_CameraConfigure(object sender, EventArgs e)
        {
            try
            {
                if (Subject != null)
                    if (Subject.CameraDeivce != null)
                        Subject.CameraDeivce.ShowControlDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #region Image
        delegate void SetTextCallback(string text);

        int curretnImageIndex;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurretnImageIndex
        {
            get { throw new Exception(); }
            set
            {
                curretnImageIndex = value;

                int visibleCurretnImageIndex = curretnImageIndex;
                if (thumbnailList.Count != 0) visibleCurretnImageIndex++;

                lbl_CurrentPos.Text = string.Format("{0} / {1}", visibleCurretnImageIndex, thumbnailList.Count);
                //this.Invoke(new Action(delegate { lbl_CurrentPos.Text = string.Format("{0} / {1}", visibleCurretnImageIndex, thumbnailList.Count); }));

                if (!this.Visible) return;

                for (int i = 0; i < 4; i++)
                {
                    if ((thumbnailPictureBoxList[i].Image != null) && (thumbnailPictureBoxList[i].Image == thumbnailList[curretnImageIndex]))
                        thumbnailCurrentLabelList[i].Visible = true;
                    else
                        thumbnailCurrentLabelList[i].Visible = false;
                }
            }
        }

        List<Image> thumbnailList = new List<Image>();

        void RefreshThumbnailes(List<string> inputImageList)
        {
            thumbnailList.Clear();
            //if (inputImageList.Count == 0) return;
            for (int i = 0; i < inputImageList.Count; i++)
            {
                Image thumbnailImg;
                try
                {
                    Mat img = new Mat(inputImageList[i], OpenCvSharp.LoadMode.GrayScale);
                    Mat resizeImg = img.Resize(new OpenCvSharp.CPlusPlus.Size(80, 60), 0, 0, OpenCvSharp.Interpolation.Area);
                    img.Dispose();
                    thumbnailImg = resizeImg.ToBitmap();
                    thumbnailList.Add(thumbnailImg);
                }
                catch
                {
                    thumbnailImg = null;
                }
            }

            int count = thumbnailList.Count;

            for (int i = 0; i < 4; i++)
            {
                if (i < count)
                    thumbnailPictureBoxList[i].Image = thumbnailList[i];
                else
                    thumbnailPictureBoxList[i].Image = null;
            }

            lbl_ImgCount.Text = string.Format("1 / {0}", count);

            tb_ImgSelector.Minimum = 0;
            tb_ImgSelector.Maximum = count - 1;

            oldValue = 0;
            innerPos = 0;

            if (count > 0) tb_ImgSelector.Value = 0;

            Subject.CurrentImageIndex = 0;
            DataBindings["CurretnImageIndex"].ReadValue();
            tb_ImageSelector_ValueChanged(this, null);
        }

        private void btn_ImgLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();

            fd.Filter = "All files (*.*)|*.*|bmp files (*.bmp)|*.bmp|png files (*.png)|*.png|jpg files (*.jpg)|*.jpg";
            fd.FilterIndex = 1;
            fd.Multiselect = true;
            if (fd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            cb_CamType.SelectedValue = CameraType.Image;
            Subject.InputImageList.Clear();

            int imgCount = fd.FileNames.Length;
            if (imgCount <= 0) return;

            for (int i = 0; i < imgCount; i++)
            {
                Subject.InputImageList.Add(fd.FileNames[i]);
            }

            RefreshThumbnailes(Subject.InputImageList);

            tsb_Run_Click(this, null);
            this.HldDisplayViewEdit.Display.FitToWindow();
        }

        int oldValue = 0;
        int innerPos = 0;

        private void tb_ImageSelector_ValueChanged(object sender, EventArgs e)
        {
            bool isChangeList = false;
            int newValue = tb_ImgSelector.Value;


            if (newValue > oldValue)
            {
                if (innerPos < 3) innerPos++;
                else isChangeList = true;
            }
            else if (newValue < oldValue)
            {
                if (innerPos > 0) innerPos--;
                else isChangeList = true;
            }

            int count = thumbnailList.Count;
            if (isChangeList)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i < count && i >= 0)
                        thumbnailPictureBoxList[i].Image = thumbnailList[i + newValue - innerPos];
                    else
                        thumbnailPictureBoxList[i].Image = null;

                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (i == innerPos)
                    thumbnailPositionLabelList[i].Visible = true;
                else
                    thumbnailPositionLabelList[i].Visible = false;
            }

            lbl_ImgCount.Text = string.Format("{0} / {1}", newValue + 1, count);

            CurretnImageIndex = curretnImageIndex;
            oldValue = newValue;
        }
        #endregion

        #region LiveShow

        static bool isLive = false;
        static Thread liveShowThread;

        void LiveStart()
        {
            if (!isLive)
            {
                liveShowThread = new Thread(new ParameterizedThreadStart(LiveShow));
                liveShowThread.IsBackground = true;
                liveShowThread.Start(this);
            }
        }

        void LiveStop()
        {
            if (!isLive) return;
            if (liveShowThread != null)
            {
                liveShowThread.Abort();
                liveShowThread.Join();
                Thread.Sleep(500);
            }
        }

        object liveLock = new object();

        void LiveShow(object sender)
        {
            try
            {
                HLDCommon.HldLogger.Log.Debug("Live Thread Start");
                HldAcqisitionEdit editor = sender as HldAcqisitionEdit;
                HldImage grabImage = new HldImage();
                isLive = true;
                while (true)
                {
                    lock (liveLock)
                    {
                        Thread.Sleep(20);
                        if (!isLive) break;

                        if (!editor.ParentForm.Visible)
                            break;

                        if (Subject.CameraDeivce == null) continue;

                        Mat image = Subject.CameraDeivce.GrabImage();
                        if (Subject.AcqusitionImage.Width == 0 || Subject.AcqusitionImage.Height == 0)
                        {
                            HLDCommon.HldLogger.Log.Error("There is No InputImage");
                            return;
                        }

                        grabImage.Mat = Subject.ConvertImage(image);

                        this.Invoke(new Action(delegate
                        {
                            System.Drawing.Point location = editor.HldDisplayViewEdit.Display.imageLocation;
                            float zoom = editor.HldDisplayViewEdit.Display.ZoomRatio;

                            editor.HldDisplayViewEdit.Display.Image = grabImage;

                            editor.HldDisplayViewEdit.Display.ZoomRatio = zoom;
                            editor.HldDisplayViewEdit.Display.imageLocation = location;
                            editor.HldDisplayViewEdit.Display.ZoomRatio = zoom;

                            if (grabImage != null)
                            {
                                Point2d centerPt = new Point2d(grabImage.Width / 2, grabImage.Height / 2);
                                editor.HldDisplayViewEdit.Display.ClearImage();
                                editor.HldDisplayViewEdit.Display.DrawCross(Pens.Yellow, centerPt, grabImage.Width, grabImage.Height, 0.0);
                            }
                        }
                        ));
                    }
                }
            }
            catch (ThreadAbortException)
            {
                HLDCommon.HldLogger.Log.Debug("Live Thread End");
            }
            catch (Exception ex)
            {
                HLDCommon.HldLogger.Log.Error("Live Thread : " + ex);
            }
            finally
            {
                isLive = false;
            }
        }


        #endregion

        private void chkBox_isHwTrigger_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
