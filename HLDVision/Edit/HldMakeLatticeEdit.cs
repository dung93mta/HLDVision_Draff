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

namespace HLDVision.Edit
{
    public partial class HldMakeLatticeEdit : HldToolEditBase
    {
        public HldMakeLatticeEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += WarppingEdit_SubjectChanged;

            this.HldDisplayViewEdit.SelectedImageChanged += DisplayViewEdit_SelectedImageChanged;
            this.HldDisplayViewEdit.Display.DrawObjectChanged += Display_OutDrawObjectChanged;
        }


        protected override void InitDisplayViewEdit()
        {

        }

        void DisplayViewEdit_SelectedImageChanged(object sender, HldImageInfo imageInfo)
        {
            if (imageInfo.ImageName.Contains("Output"))
            {
                tabControl.SelectedTab = tb_Out_Pers;
                if (Subject.InputImage == null) return;
                if (cb_OutOption.SelectedValue == null)
                    cb_OutOption.SelectedValue = HldWarpping.Option.Perspective;

                HldDisplayViewEdit.Display.Image = Subject.OutputImage;

                if ((HldWarpping.Option)cb_OutOption.SelectedValue == HldWarpping.Option.Perspective)
                    HldDisplayViewEdit.Display.InteractiveGraphicsCollection.Add(Subject.OutPLineObject);
                else
                    HldDisplayViewEdit.Display.InteractiveGraphicsCollection.Add(Subject.OutRectObject);
                HldDisplayViewEdit.Display.GraphicsCollection.Add(Subject.OutputPoint);

                if (Subject.OutputPoint != null)
                    Subject.OutputPoint.Display = HldDisplayViewEdit.Display;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldWarpping Subject
        {
            get { return base.GetSubject() as HldWarpping; }
            set { base.SetSubject(value); }
        }

        void WarppingEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);

            //Display_InDrawObjectChanged(this, Subject.InPLineObject);
            Display_OutDrawObjectChanged(this, Subject.OutPLineObject);
        }
        
        #region OutputPoints Event

        void Display_OutDrawObjectChanged(object sender, InteractDrawObject drawObject)
        {
            if (Subject.OutPLineObject == null) return;
            Remove_Out_nud_ValueChangedEvent();

            nud_OutP0_X.Value = (decimal)Subject.OutPLineObject[0].X; nud_OutP0_Y.Value = (decimal)Subject.OutPLineObject[0].Y;
            nud_OutP1_X.Value = (decimal)Subject.OutPLineObject[1].X; nud_OutP1_Y.Value = (decimal)Subject.OutPLineObject[1].Y;
            nud_OutP2_X.Value = (decimal)Subject.OutPLineObject[2].X; nud_OutP2_Y.Value = (decimal)Subject.OutPLineObject[2].Y;
            nud_OutP3_X.Value = (decimal)Subject.OutPLineObject[3].X; nud_OutP3_Y.Value = (decimal)Subject.OutPLineObject[3].Y;

            Add_Out_nud_ValueChangedEvent();
        }

        void Add_Out_nud_ValueChangedEvent()
        {
            nud_OutP0_X.ValueChanged += nud_OutputDrawPoint_ValueChanged; nud_OutP0_Y.ValueChanged += nud_OutputDrawPoint_ValueChanged;
            nud_OutP1_X.ValueChanged += nud_OutputDrawPoint_ValueChanged; nud_OutP1_Y.ValueChanged += nud_OutputDrawPoint_ValueChanged;
            nud_OutP2_X.ValueChanged += nud_OutputDrawPoint_ValueChanged; nud_OutP2_Y.ValueChanged += nud_OutputDrawPoint_ValueChanged;
            nud_OutP3_X.ValueChanged += nud_OutputDrawPoint_ValueChanged; nud_OutP3_Y.ValueChanged += nud_OutputDrawPoint_ValueChanged;
        }

        void Remove_Out_nud_ValueChangedEvent()
        {
            nud_OutP0_X.ValueChanged -= nud_OutputDrawPoint_ValueChanged; nud_OutP0_Y.ValueChanged -= nud_OutputDrawPoint_ValueChanged;
            nud_OutP1_X.ValueChanged -= nud_OutputDrawPoint_ValueChanged; nud_OutP1_Y.ValueChanged -= nud_OutputDrawPoint_ValueChanged;
            nud_OutP2_X.ValueChanged -= nud_OutputDrawPoint_ValueChanged; nud_OutP2_Y.ValueChanged -= nud_OutputDrawPoint_ValueChanged;
            nud_OutP3_X.ValueChanged -= nud_OutputDrawPoint_ValueChanged; nud_OutP3_Y.ValueChanged -= nud_OutputDrawPoint_ValueChanged;
        }

        private void nud_OutputDrawPoint_ValueChanged(object sender, EventArgs e)
        {
            if (Subject.OutPLineObject == null) return;
            NumericUpDown nud = sender as NumericUpDown;

            HldPolyLine plinetemp = Subject.OutPLineObject;
            if (nud == nud_OutP0_X || nud == nud_OutP0_Y)
            {
                if ((HldWarpping.Option)cb_OutOption.SelectedValue == HldWarpping.Option.Rectangle)
                {
                    if (nud_OutP3_X.Value != nud_OutP0_X.Value) nud_OutP3_X.Value = nud_OutP0_X.Value;
                    if (nud_OutP1_Y.Value != nud_OutP0_Y.Value) nud_OutP1_Y.Value = nud_OutP0_Y.Value;
                }
                plinetemp[0] = new PointF((float)nud_OutP0_X.Value, (float)nud_OutP0_Y.Value);
            }
            else if (nud == nud_OutP1_X || nud == nud_OutP1_Y)
            {
                if ((HldWarpping.Option)cb_OutOption.SelectedValue == HldWarpping.Option.Rectangle)
                {
                    if (nud_OutP2_X.Value != nud_OutP1_X.Value) nud_OutP2_X.Value = nud_OutP1_X.Value;
                    if (nud_OutP0_Y.Value != nud_OutP1_Y.Value) nud_OutP0_Y.Value = nud_OutP1_Y.Value;
                }
                plinetemp[1] = new PointF((float)nud_OutP1_X.Value, (float)nud_OutP1_Y.Value);
            }
            else if (nud == nud_OutP2_X || nud == nud_OutP2_Y)
            {
                if ((HldWarpping.Option)cb_OutOption.SelectedValue == HldWarpping.Option.Rectangle)
                {
                    if (nud_OutP1_X.Value != nud_OutP2_X.Value) nud_OutP1_X.Value = nud_OutP2_X.Value;
                    if (nud_OutP3_Y.Value != nud_OutP2_Y.Value) nud_OutP3_Y.Value = nud_OutP2_Y.Value;
                }
                plinetemp[2] = new PointF((float)nud_OutP2_X.Value, (float)nud_OutP2_Y.Value);
            }
            else if (nud == nud_OutP3_X || nud == nud_OutP3_Y)
            {
                if ((HldWarpping.Option)cb_OutOption.SelectedValue == HldWarpping.Option.Rectangle)
                {
                    if (nud_OutP0_X.Value != nud_OutP3_X.Value) nud_OutP0_X.Value = nud_OutP3_X.Value;
                    if (nud_OutP2_Y.Value != nud_OutP3_Y.Value) nud_OutP2_Y.Value = nud_OutP3_Y.Value;
                }
                plinetemp[3] = new PointF((float)nud_OutP3_X.Value, (float)nud_OutP3_Y.Value);
            }
            Subject.OutPLineObject = plinetemp;
            HldDisplayViewEdit.Display.Invalidate();
        }

        #endregion

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldWarpping), null);

            cb_OutOption.DisplayMember = "Key"; cb_OutOption.ValueMember = "Value";
            cb_OutOption.DataSource = Enum.GetValues(typeof(HldWarpping.Option)).Cast<HldWarpping.Option>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_OutOption.DataBindings.Add("SelectedValue", source, "OutOption", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        //HldImageInfo inPointsImageInfo;
        //HldImageInfo outPointsImageInfo;

        private void cb_OutOption_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        private void btn_ResetOutQuad_Click(object sender, EventArgs e)
        {
            if (Subject.InputImage == null) return;
            nud_OutP0_X.Value = 0; nud_OutP0_Y.Value = 0;
            nud_OutP1_X.Value = (decimal)Subject.InputImage.Width; nud_OutP1_Y.Value = 0;
            nud_OutP2_X.Value = (decimal)Subject.InputImage.Width; nud_OutP2_Y.Value = (decimal)Subject.InputImage.Height;
            nud_OutP3_X.Value = 0; nud_OutP3_Y.Value = (decimal)Subject.InputImage.Height;

        }

    }
}
