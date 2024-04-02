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
using HLDVision.Display;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;

namespace HLDVision.Edit
{
    public partial class HldTemplateMatchEdit : HldToolEditBase
    {
        public HldTemplateMatchEdit()
        {
            InitializeComponent();
            InitBinding();

            this.SubjectChanged += TMEdit_SubjectChanged;
            this.HldDisplayViewEdit.SelectedImageChanged += DisplayViewEdit_SelectedImageChanged;

            templateRegion = new HldRotationRectangle();

            mPatternDisplayList = new List<HldDisplayViewInteract>();
            mPatternDisplayList.Add(hldSubPattern0);
            mPatternDisplayList.Add(hldSubPattern1);
            mPatternDisplayList.Add(hldSubPattern2);
            mPatternDisplayList.Add(hldSubPattern3);

            mPatternLabelList = new List<Label>();
            mPatternLabelList.Add(lb_patter0);
            mPatternLabelList.Add(lb_patter1);
            mPatternLabelList.Add(lb_patter2);
            mPatternLabelList.Add(lb_patter3);
        }

        HldRotationRectangle templateRegion;

        void DisplayViewEdit_SelectedImageChanged(object sender, HldImageInfo imageInfo)
        {
            if (imageInfo.ImageName.Contains("Train"))
            {
                if (Subject.InputImage == null) return;
                HldDisplayViewEdit.Display.Image = Subject.InputImage;
                HldDisplayViewEdit.Display.InteractiveGraphicsCollection.Add(templateRegion);
            }
        }


        protected override void InitDisplayViewEdit()
        {
            HldDisplayViewEdit.InsertCustomImage(1, new HldImageInfo("[TemplateMatch] Train"));
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldTemplateMatch Subject
        {
            get { return base.GetSubject() as HldTemplateMatch; }
            set { base.SetSubject(value); }
        }

        void TMEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);

            hldTemplate.Image = null;
            InitPatternList();
            SetTemplateImage(Subject.SelectedPatternIndex);

            ResetTepmplateGraphics();
        }

        void ResetTepmplateGraphics()
        {
            hldTemplate.InteractiveGraphicsCollection.Clear();
            hldTemplate.GraphicsCollection.Clear();

            if (Subject.TemplateImage == null)
            {
                return;
            }
            hldTemplate.Image = Subject.TemplateImage;

            if (Subject.RefPoint != null)
            {
                hldTemplate.InteractiveGraphicsCollection.Add(Subject.RefPoint);
            }

            if (Subject.Mask != null && Subject.SelectedPatternIndex > -1 && Subject.mPatternDataList.Count > Subject.SelectedPatternIndex)
            {
                Subject.Mask = Subject.mPatternDataList[Subject.SelectedPatternIndex].Value.MaskImage;
                hldTemplate.GraphicsCollection.Add(Subject.Mask);

                Subject.Mask.DrawMask = true;

                if (chb_TempUseEdge.Checked)
                    nud_Threshold_ValueChanged(null, null);
            }
        }

        BindingSource source;

        int mSelectedIndex;
        public int SelectedIndex
        {
            get { return mSelectedIndex; }
            set
            {
                if (mSelectedIndex == value) return;
                mSelectedIndex = value;
                SetTemplateImage(value);
                int refreshindex = value - 1;

                if (value == 0)
                    refreshindex = 0;

                RefreshPatternList(refreshindex);
            }
        }

        void InitBinding()
        {
            source = new BindingSource(typeof(HldTemplateMatch), null);
            nud_Template_ScaleFirst.DataBindings.Add("Value", source, "ScaleFirst", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Template_ScaleLast.DataBindings.Add("Value", source, "ScaleLast", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Template_Method_Degree1.DataBindings.Add("Value", source, "AngleNeg", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Template_Method_Degree2.DataBindings.Add("Value", source, "AnglePos", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_TMmaxCount.DataBindings.Add("Value", source, "MaxCount", true, DataSourceUpdateMode.OnPropertyChanged, 1);
            nud_FirstStep.DataBindings.Add("Value", source, "FirstStep", true, DataSourceUpdateMode.OnPropertyChanged);

            cb_TemplPriority.DisplayMember = "Key"; cb_TemplPriority.ValueMember = "Value";
            cb_TemplPriority.DataSource = Enum.GetValues(typeof(HldTemplateMatch.Direction)).Cast<HldTemplateMatch.Direction>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_TemplPriority.DataBindings.Add("SelectedValue", source, "Priority", true, DataSourceUpdateMode.OnPropertyChanged);

            nud_templPCreteria.DataBindings.Add("Value", source, "PCreteria", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_templPrecision.DataBindings.Add("Value", source, "Precision", true, DataSourceUpdateMode.OnPropertyChanged);
            chb_TempUseEdge.DataBindings.Add("Checked", source, "UseEdge", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_TempTHLow.DataBindings.Add("Value", source, "THLow", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_TempTHHigh.DataBindings.Add("Value", source, "THHigh", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_TempScaleMin.DataBindings.Add("Value", source, "TempScaleMin", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_TempScaleMax.DataBindings.Add("Value", source, "TempScaleMax", true, DataSourceUpdateMode.OnPropertyChanged);
            chb_AutoParam.DataBindings.Add("Checked", source, "AutoParam", true, DataSourceUpdateMode.OnPropertyChanged);

            chb_AutoIndex.DataBindings.Add("Checked", source, "IsAutoIndex", true, DataSourceUpdateMode.OnPropertyChanged);
            this.DataBindings.Add("SelectedIndex", source, "SelectedPatternIndex", true, DataSourceUpdateMode.OnPropertyChanged);

            //chb_TempUseEdge.DataBindings.Add("Checked", source, "Mask.OnMask", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void btn_TemplateMatching_Grab_Click(object sender, EventArgs e)
        {
            HldImageInfo imageInfo = (HldImageInfo)HldDisplayViewEdit.imageListComboBox.SelectedItem;
            if (!imageInfo.ImageName.Contains("Train")) return;

            HldImage image = HldDisplayViewEdit.Display.Image;
            if (image == null) return;

            Mat templateImg = templateRegion.GetROIRegion(image.Mat);

            if (templateImg == null)
            {
                //MessageBox.Show("선택된 이미지 영역이 없습니다.");
                MessageBox.Show("There does not select image.");
                return;
            }

            hldTemplate.Image = new HldImage(templateImg);

            PatternData patterndata = new PatternData(hldTemplate.Image);

            Subject.mPatternDataList.Insert(0, new KeyValuePair<string, PatternData>("", patterndata));
            //if (Subject.mPatternDataList.Count > )
            Subject.SelectedPatternIndex = 0;

            SetTemplateImage(0);
            RefreshPatternList();
            nud_Threshold_ValueChanged(this, null);
        }

        private void chb_TempUseEdge_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_TempUseEdge.Checked)
            {
                //btn_ResetMask.Enabled = true;
                //chb_MaskOn.Enabled = true;
                hldTemplate.GraphicsFuncCollection.Add(DrawEdgeImage);
                nud_Threshold_ValueChanged(this, null);
            }
            else
            {
                //chb_MaskOn.Enabled = false;
                //btn_ResetMask.Enabled = false;
                hldTemplate.GraphicsFuncCollection.Clear();
            }
        }

        Mat Edge;

        void DrawEdgeImage(Graphics gdi)
        {
            if (Subject.TemplateImage == null || Edge == null) return;

            Bitmap bitmap;
            Mat maskedEdge = new Mat();

            Mat mask = Subject.Mask.MaskMat;
            Mat colorEdge = new Mat(mask.Size(), MatType.CV_8UC4);
            Mat nullMat = new Mat(mask.Size(), MatType.CV_8UC1, new Scalar(0));

            if (Edge.Type() != MatType.CV_8UC1) return;

            if (mask != null && mask.Width != 0 && mask.Height != 0)
            {
                Edge.CopyTo(maskedEdge, mask);
                Cv2.Merge(new Mat[] { nullMat, maskedEdge, nullMat, maskedEdge }, colorEdge);
                bitmap = colorEdge.ToBitmap();
            }
            else
            {
                Cv2.Merge(new Mat[] { nullMat, Edge, nullMat, Edge }, colorEdge);
                bitmap = colorEdge.ToBitmap();
            }

            gdi.DrawImageUnscaled(bitmap, 0, 0);

            nullMat.Dispose();
            colorEdge.Dispose();
            maskedEdge.Dispose();
            bitmap.Dispose();
        }

        private void btn_RefToCenter_Click(object sender, EventArgs e)
        {
            if (hldTemplate.Image == null) return;

            Subject.RefPoint.PointF = new PointF(hldTemplate.Image.Width / 2, hldTemplate.Image.Height / 2);
        }

        private void btn_ResetMask_Click(object sender, EventArgs e)
        {
            if (hldTemplate.Image == null) return;

            Subject.Mask.ResetMask();
        }

        private void nud_Threshold_ValueChanged(object sender, EventArgs e)
        {
            if (hldTemplate.Image == null) return;
            if (!chb_TempUseEdge.Checked) return;

            Mat src = Subject.TemplateImage.Mat;

            int minSize = Math.Min(Math.Min(src.Width, src.Height), 1000);
            Subject.GetEdge(src, minSize, out Edge, 1);

            hldTemplate.Invalidate();
        }

        private void nud_TempScaleMin_ValueChanged(object sender, EventArgs e)
        {
            if (nud_TempScaleMin.Value > nud_TempScaleMax.Value)
                nud_TempScaleMin.Value = nud_TempScaleMax.Value;
        }

        private void nud_TempScaleMax_ValueChanged(object sender, EventArgs e)
        {
            if (nud_TempScaleMax.Value < nud_TempScaleMin.Value)
                nud_TempScaleMax.Value = nud_TempScaleMin.Value;
        }

        private void chb_MaskOn_CheckedChanged(object sender, EventArgs e)
        {
            //Subject.Mask.OnMask = chb_MaskOn.Checked;

            //if (chb_MaskOn.Checked)
            //    chb_MaskOn.Text = "Mask On";
            //else
            //    chb_MaskOn.Text = "Mask Off";
            if (hldTemplate.Image == null) return;

            hldTemplate.GraphicsCollection.Clear();
            PatternData pattern = Subject.mPatternDataList[Subject.SelectedPatternIndex].Value;
            HldMaskingEdit maskedit = new HldMaskingEdit();
            HldMasking hldMask = new HldMasking();
            HldToolEditForm maskform = new HldToolEditForm(maskedit);

            hldMask.InputImage = pattern.PatternImage;
            hldMask.Mask = pattern.MaskImage;
            pattern.MaskImage.DrawMask = true;

            maskedit.Subject = hldMask;

            hldMask.InitOutProperty();
            hldMask.Run(true);
            maskform.ShowDialog();

            pattern.MaskImage.DrawMask = true;
            hldTemplate.GraphicsCollection.Add(pattern.MaskImage);

            pattern.MakeMaskedPattern();
        }

        private void chb_AutoParam_CheckedChanged(object sender, EventArgs e)
        {
            bool autoCheck = chb_AutoParam.Checked;

            nud_FirstStep.ReadOnly = autoCheck;
            nud_templPrecision.ReadOnly = autoCheck;
            nud_Template_ScaleFirst.ReadOnly = autoCheck;
            nud_Template_ScaleLast.ReadOnly = autoCheck;
            nud_Template_Method_Degree1.ReadOnly = autoCheck;
            nud_Template_Method_Degree2.ReadOnly = autoCheck;
        }

        #region Pattern Save, Multi Pattern
        List<HldDisplayViewInteract> mPatternDisplayList = new List<HldDisplayViewInteract>();
        List<Label> mPatternLabelList = new List<Label>();

        private void btn_PatternSave_Click(object sender, EventArgs e)
        {
            if (hldTemplate == null || hldTemplate.Image == null || hldTemplate.Image.Width == 0 || hldTemplate.Image.Height == 0)
                return;
            //if (Subject.TemplateImage == null || Subject.TemplateImage.Mat == null || Subject.TemplateImage.Width == 0 || Subject.TemplateImage.Height == 0)
            //    return;

            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "Image files (*.pti)|*.pti";
            fd.FilterIndex = 1;
            if (fd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            if (string.IsNullOrEmpty(fd.FileName)) return;

            this.Cursor = Cursors.WaitCursor;
            PatternData patternData = new PatternData(Subject.TemplateImage, Subject.Mask, Subject.RefPoint);

            // File Save
            patternData.SavePattern(fd.FileName);

            // Icon 만들기
            //using (Stream IconStream = System.IO.File.OpenWrite(fd.FileName))
            //{
            //    Bitmap bitmap = Subject.TemplateImage.Mat.ToBitmap(System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //    bitmap.SetResolution(72, 72);
            //    Icon icon = System.Drawing.Icon.FromHandle(bitmap.GetHicon());
            //    icon.Save(IconStream);
            //}
            this.Cursor = Cursors.Default;

            lbl_CurrentJob.Text = System.IO.Path.GetFileNameWithoutExtension(fd.FileName);
        }

        private void btn_PatternLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Image files (*.pti)|*.pti";
            fd.FilterIndex = 1;
            fd.Multiselect = true;
            if (fd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            if (string.IsNullOrEmpty(fd.FileName)) return;

            this.Cursor = Cursors.WaitCursor;

            for (int i = 0; i < Subject.mPatternDataList.Count; i++)
            {
                Subject.mPatternDataList[i].Value.Dispose();
            }

            Subject.mPatternDataList.Clear();
            Subject.mPatternDataList.Clear();

            foreach (string path in fd.FileNames)
            {
                Subject.mPatternDataList.Add(new KeyValuePair<string, PatternData>(path, (PatternData)serializer.Deserializing(path)));
            }

            InitPatternList();

            this.Cursor = Cursors.Default;

            lbl_CurrentJob.Text = System.IO.Path.GetFileNameWithoutExtension(fd.FileName);
        }

        private void btn_Pre_Click(object sender, EventArgs e)
        {
            //if (!lb_patter0.Text.Contains('.')) return; 
            string[] lb = lb_patter0.Text.Split('.');
            int startIndex;
            if (!int.TryParse(lb[0], out startIndex)) return;
            if (startIndex <= 0) return;
            startIndex--;

            RefreshPatternList(startIndex);
        }
        private void btn_Next_Click(object sender, EventArgs e)
        {
            if (!lb_patter0.Text.Contains('.')) return;
            string[] lb = lb_patter3.Text.Split('.');
            int endIndex;
            if (!int.TryParse(lb[0], out endIndex)) return;
            if (endIndex >= Subject.mPatternDataList.Count - 1) return;
            endIndex++;

            RefreshPatternList(endIndex - 3);
        }

        private void btn_PatternAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Image files (*.pti)|*.pti";
            fd.FilterIndex = 1;
            fd.Multiselect = true;
            if (fd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            if (string.IsNullOrEmpty(fd.FileName)) return;

            foreach (string path in fd.FileNames)
            {
                if (Subject.mPatternDataList.FindIndex(x => x.Key == path) >= 0) continue;
                Subject.mPatternDataList.Add(new KeyValuePair<string, PatternData>(path, (PatternData)serializer.Deserializing(path)));
            }

            RefreshPatternList();
        }

        private void btn_PatternRemove_Click(object sender, EventArgs e)
        {
            if (Subject.mPatternDataList.Count > Subject.SelectedPatternIndex && Subject.SelectedPatternIndex >= 0)
            {
                Subject.mPatternDataList[Subject.SelectedPatternIndex].Value.Dispose();
                Subject.TemplateImage.Dispose();
                Subject.mPatternDataList.RemoveAt(Subject.SelectedPatternIndex);
            }

            if (Subject.SelectedPatternIndex >= Subject.mPatternDataList.Count)
                SetTemplateImage(Subject.mPatternDataList.Count - 1, false);

            RefreshPatternList();
        }

        private void InitPatternList()
        {
            for (int i = 0; i < 4; i++)
            {
                mPatternDisplayList[i].Image = null;
            }

            SetTemplateImage(Subject.SelectedPatternIndex, false);
            RefreshPatternList(0);
        }

        private void RefreshPatternList(int _startindex = -1)
        {
            int startindex = 0;
            if (_startindex < 0) // 동일 Index일 경우...-1, -2도 들어올 수 있음!!
            {
                if (!int.TryParse(lb_patter0.Text.Split('.')[0], out startindex))
                    _startindex = 0;
                else
                    _startindex = startindex;
            }

            for (int i = 0; i < mPatternDisplayList.Count; i++)
            {
                string labelName = (_startindex + i).ToString();
                if (_startindex + i < Subject.mPatternDataList.Count)
                    labelName += "." + System.IO.Path.GetFileNameWithoutExtension(Subject.mPatternDataList[_startindex + i].Key);
                mPatternLabelList[i].Text = labelName;
            }

            btn_Selected.Visible = false;

            for (int i = 0; i < mPatternDisplayList.Count; i++)
            {
                //if (!lb_patter0.Text.Contains('.')) return;
                string[] lb = lb_patter0.Text.Split('.');

                int startIndex;
                if (!int.TryParse(lb[0], out startIndex)) break;
                // 패턴이 없으면 패턴리스트에서 지워준다.
                if (startIndex + i >= Subject.mPatternDataList.Count)
                {
                    mPatternDisplayList[i].Image = null;
                    continue;
                }

                mPatternDisplayList[i].Image = Subject.mPatternDataList[startIndex + i].Value.PatternImage;
                mPatternDisplayList[i].FitToWindow();

                // 빨간 박스 표시
                if (startIndex + i == Subject.SelectedPatternIndex)
                {
                    btn_Selected.Visible = true;
                    btn_Selected.Location = new System.Drawing.Point(mPatternDisplayList[i].Location.X - 2, mPatternDisplayList[i].Location.Y - 2);
                }
            }

            hldTemplate.Image = null;

            // 처리할 항목이 없으면 리턴
            if (Subject.mPatternDataList.Count == 0) return;

            // 화면에 현재 Pattern 표시
            if (Subject.SelectedPatternIndex > -1)
                SetTemplateImage(Subject.SelectedPatternIndex);
        }

        private void hld_SubPattern_Click(object sender, EventArgs e)
        {
            int selectedDisplayIndex;
            string lbName = (sender as HldDisplayViewInteract).Name;

            selectedDisplayIndex = int.Parse(lbName.Substring(lbName.Length - 1));
            int selectedIndex = int.Parse(mPatternLabelList[selectedDisplayIndex].Text.Split('.')[0]);

            RefreshPatternList();

            if (selectedIndex == Subject.SelectedPatternIndex) return;


            SetTemplateImage(selectedIndex);
        }

        private void SetTemplateImage(int _index, bool _saveMask = true)
        {
            if (Subject.mPatternDataList.Count <= _index) return;
            if (Subject.mPatternDataList.Count == 0) return;

            Subject.SelectedPatternIndex = _index;

            // 만약 Edge 사용한다면... Edge 표시
            if (chb_TempUseEdge.Checked)
                nud_Threshold_ValueChanged(null, null);

            PatternData pattrnData;
            if (_index == -1)
                pattrnData = new PatternData(); // 모든 멤버가 null임
            else
                pattrnData = Subject.mPatternDataList[_index].Value;

            Subject.TemplateImage = pattrnData.PatternImage;
            Subject.Mask = pattrnData.MaskImage;
            Subject.RefPoint = pattrnData.RefPoint;

            // 이코드가 있어야 현재 Subject.Refpoint를 화면에 뿌려주게 된다.
            //Subject.mPatternDataList[Subject.mSelectedPatternIndex].Value.RefPoint = Subject.RefPoint;


            ResetTepmplateGraphics();
        }

        #endregion

        private void btn_refMark_Up_Click(object sender, EventArgs e)
        {
            if (hldTemplate.Image == null) return;

            Subject.RefPoint.PointF = new PointF(Subject.RefPoint.PointF.X, Subject.RefPoint.PointF.Y - 1);
        }

        private void btn_refMark_Down_Click(object sender, EventArgs e)
        {
            if (hldTemplate.Image == null) return;

            Subject.RefPoint.PointF = new PointF(Subject.RefPoint.PointF.X, Subject.RefPoint.PointF.Y + 1);
        }

        private void btn_refMark_Left_Click(object sender, EventArgs e)
        {
            if (hldTemplate.Image == null) return;

            Subject.RefPoint.PointF = new PointF(Subject.RefPoint.PointF.X - 1, Subject.RefPoint.PointF.Y);
        }

        private void btn_refMark_Right_Click(object sender, EventArgs e)
        {
            if (hldTemplate.Image == null) return;

            Subject.RefPoint.PointF = new PointF(Subject.RefPoint.PointF.X + 1, Subject.RefPoint.PointF.Y);
        }
    }
}
