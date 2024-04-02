using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HLDVision.Display;
using OpenCvSharp.CPlusPlus;

namespace HLDVision.Edit.Base
{
    public partial class HldDisplayViewEdit : UserControl
    {
        public HldDisplayViewEdit()
        {
            InitializeComponent();
            customImageList = new Dictionary<int, HldImageInfo>();
            hldDisplayViewStatusBar.Display = hld_display;
            cb_ImageList.Click += cb_ImageList_Click;
            cb_ImageList.SelectedValueChanged += cb_ImageList_SelectedValueChanged;
        }


        bool IsComboBoxClick = false;

        void cb_ImageList_Click(object sender, EventArgs e)
        {
            IsComboBoxClick = true;
        }

        HldToolBase subject;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldToolBase Subject
        {
            get { return subject; }
            set
            {
                subject = value;
                RefreshImage();

                if (cb_ImageList.Items.Count > 0)
                    cb_ImageList.SelectedIndex = cb_ImageList.Items.Count - 1;
            }
        }

        public HldDisplayViewInteract Display
        {
            get { return hld_display; }
        }

        int currentIndex;

        public void RefreshImage()
        {
            hld_display.Image = null;
            currentIndex = cb_ImageList.SelectedIndex;

            cb_ImageList.Items.Clear();

            if (subject != null)
            {
                foreach (HldImageInfo info in subject.imageList)
                {
                    cb_ImageList.Items.Add(info);
                }
            }

            foreach (KeyValuePair<int, HldImageInfo> pair in customImageList)
            {
                cb_ImageList.Items.Insert(pair.Key, pair.Value);
            }

            cb_ImageList_SelectedValueChanged(this, null);
        }

        public void ClearImage()
        {
            hld_display.Image = null;
        }

        public void InsertCustomImage(int index, HldImageInfo info)
        {
            customImageList.Add(index, info);
        }

        public void RemoveCustomImage(int index)
        {
            customImageList.Remove(index);
        }

        public void ClearCustomImage()
        {
            customImageList.Clear();
        }

        public void SetLastImage()
        {
            if (cb_ImageList.Items.Count > 0)
                cb_ImageList.SelectedIndex = cb_ImageList.Items.Count - 1;
        }

        public Dictionary<int, HldImageInfo> CustomImageList 
        { 
            get 
            {
                if (customImageList == null)
                    customImageList = new Dictionary<int, HldImageInfo>();
                return customImageList; 
            } 
            set 
            { 
                //customImageList = value; 
                customImageList = new Dictionary<int, HldImageInfo>();
            } 
        }
        Dictionary<int, HldImageInfo> customImageList;

        public ComboBox imageListComboBox
        {
            get { return cb_ImageList; }
        }

        void cb_ImageList_SelectedValueChanged(object sender, EventArgs e)
        {
            this.Display.ClearImage();

            float oldZoom = Display.ZoomRatio;
            System.Drawing.Point oldLocation = Display.imageLocation;

            if (cb_ImageList.SelectedItem == null)
            {
                if (currentIndex != -1 && cb_ImageList.Items.Count > currentIndex)
                    cb_ImageList.SelectedIndex = currentIndex;
                else if (cb_ImageList.Items.Count > 0)
                    cb_ImageList.SelectedIndex = cb_ImageList.Items.Count - 1;
                return;
            }

            HldImageInfo imageInfo = (HldImageInfo)cb_ImageList.SelectedItem;
            hld_display.Image = imageInfo.Image;

            if (imageInfo.drawingFunc != null && hld_display.Image != null)
                imageInfo.drawingFunc(hld_display);

            NotifySelectedValueChanged(imageInfo);

            if (IsComboBoxClick)
            {
                Display.ZoomRatio = oldZoom;
                Display.imageLocation = oldLocation;
                IsComboBoxClick = false;
            }
        }

        public delegate void SelectedImageChangedHandler(object sender, HldImageInfo imageInfo);

        public event SelectedImageChangedHandler SelectedImageChanged;

        void NotifySelectedValueChanged(HldImageInfo imageInfo)
        {
            if (SelectedImageChanged != null)
            {
                SelectedImageChanged(this, imageInfo);
            }
        }

        public Mat CalibrationMat
        {
            set
            {
                // Calibration VtoR과 Fixture Image2Fixture Matrix가 서로 반대네... 젠장 바꿔야지...
                Mat transF = null;
                if (value.Type() == MatType.CV_64FC1)
                {
                    transF = new Mat(3, 3, MatType.CV_32FC1);
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            transF.Set(i, j, (float)value.At<double>(i, j));
                        }
                    }
                }
                else if (value.Type() != MatType.CV_32FC1)
                    return;
                
                hldDisplayViewStatusBar.CalibrationMat = transF;
                if (transF != null) transF.Dispose();
            }
        }

        public Mat GetDisplayImage(int _index = 0)
        {
            HldDisplayView display = new HldDisplayView();
            display.Width = Display.Width;
            display.Height = Display.Height;
            try
            {
                display.Image = CustomImageList[_index].Image;
                customImageList[_index].drawingFunc(display);
                return display.GetDisplayImage();
            }
            catch (Exception ex)
            {
                HLDCommon.HldLogger.Log.Error("[image save fail] \r\n" + ex.ToString());
                return null;
            }
            finally
            {
                display.Dispose();
            }
        }
    }
}
