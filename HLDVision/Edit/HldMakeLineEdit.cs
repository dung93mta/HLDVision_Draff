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
    public partial class HldMakeLineEdit : HldToolEditBase
    {
        public HldMakeLineEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += LineMakeEdit_SubjectChanged;

            this.HldDisplayViewEdit.SelectedImageChanged += DisplayViewEdit_SelectedImageChanged;
            this.HldDisplayViewEdit.Display.DrawObjectChanged += Display_InDrawObjectChanged;
        }


        protected override void InitDisplayViewEdit()
        { }

        void DisplayViewEdit_SelectedImageChanged(object sender, HldImageInfo imageInfo)
        {
            if (imageInfo.ImageName.Contains("Input"))
            {
                tabControl.SelectedTab = tb_In_Pers;
                if (Subject.InputImage == null) return;

                hldDisplayViewEdit.Display.InteractiveGraphicsCollection.Add(Subject.OutLine);
                Display_InDrawObjectChanged(this, Subject.OutLine);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldMakeLine Subject
        {
            get { return base.GetSubject() as HldMakeLine; }
            set { base.SetSubject(value); }
        }

        void LineMakeEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);


            Display_InDrawObjectChanged(this, Subject.InLine);
        }

        #region InputPoints Event

        void Display_InDrawObjectChanged(object sender, InteractDrawObject drawObject)
        {
            if (Subject.InPoint1 == null || Subject.InPoint2 == null)
                return;
            Remove_In_nud_ValueChangedEvent();

            nud_InP0_X.Value = (decimal)Subject.InLine.SP.X; nud_InP0_Y.Value = (decimal)Subject.InLine.SP.Y;
            nud_InP1_X.Value = (decimal)Subject.InLine.EP.X; nud_InP1_Y.Value = (decimal)Subject.InLine.EP.Y;

            if (!Subject.inParams.ContainsKey("InPoint1")) Subject.InPoint1.Point2d = Subject.InLine.SP;
            if (!Subject.inParams.ContainsKey("InPoint2")) Subject.InPoint2.Point2d = Subject.InLine.EP;
            CalOutPoint();

            Add_In_nud_ValueChangedEvent();
        }

        void Add_In_nud_ValueChangedEvent()
        {
            nud_InP0_X.ValueChanged += nud_InputDrawPoint_ValueChanged; nud_InP0_Y.ValueChanged += nud_InputDrawPoint_ValueChanged;
            nud_InP1_X.ValueChanged += nud_InputDrawPoint_ValueChanged; nud_InP1_Y.ValueChanged += nud_InputDrawPoint_ValueChanged;
        }

        void Remove_In_nud_ValueChangedEvent()
        {
            nud_InP0_X.ValueChanged -= nud_InputDrawPoint_ValueChanged; nud_InP0_Y.ValueChanged -= nud_InputDrawPoint_ValueChanged;
            nud_InP1_X.ValueChanged -= nud_InputDrawPoint_ValueChanged; nud_InP1_Y.ValueChanged -= nud_InputDrawPoint_ValueChanged;
        }

        private void nud_InputDrawPoint_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nud = sender as NumericUpDown;

            if (nud == nud_InP0_X || nud == nud_InP0_Y)
            {
                Subject.InPoint1 = new HldPoint((float)nud_InP0_X.Value, (float)nud_InP0_Y.Value);
            }
            else if (nud == nud_InP1_X || nud == nud_InP1_Y)
            {
                Subject.InPoint2 = new HldPoint((float)nud_InP1_X.Value, (float)nud_InP1_Y.Value);
            }
            CalOutPoint();
            HldDisplayViewEdit.Display.Invalidate();
        }

        void CalOutPoint()
        {
            nud_OutP0_X.Value = (decimal)Subject.OutLine.SP.X; nud_OutP0_Y.Value = (decimal)Subject.OutLine.SP.Y;
            nud_OutP1_X.Value = (decimal)Subject.OutLine.EP.X; nud_OutP1_Y.Value = (decimal)Subject.OutLine.EP.Y;

            nud_OutCP_X.Value = (decimal)Subject.OutLine.CP.X; nud_OutCP_Y.Value = (decimal)Subject.OutLine.CP.Y;
            nud_Out_deg.Value = (decimal)Subject.OutLine.ThetaAngle; nud_Out_rad.Value = (decimal)Subject.OutLine.ThetaRad;
        }

        #endregion

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldMakeLine), null);
            DataBindings.Add("LineResult", source, "Result", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_LCreteria.DataBindings.Add("Value", source, "LCreteria", true, DataSourceUpdateMode.OnPropertyChanged, 3f);
            lbl_isLine.DataBindings.Add("Text", source, "IsLine", true, DataSourceUpdateMode.OnPropertyChanged, ToString());
        }

        public double LineResult
        {
            get { return (double)nud_MakeLine_Result_Length.Value; }
            set
            {
                nud_MakeLine_Result_Length.Value = (decimal)value;
            }
        }

        //HldImageInfo inPointsImageInfo;
        //HldImageInfo outPointsImageInfo;
    }
}
