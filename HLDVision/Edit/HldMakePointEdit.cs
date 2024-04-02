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
    public partial class HldMakePointEdit : HldToolEditBase
    {
        public HldMakePointEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += PointMakeEdit_SubjectChanged;

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
                hldDisplayViewEdit.Display.InteractiveGraphicsCollection.Add(Subject.OutPoint);
            }
            else if (imageInfo.ImageName.Contains("Output"))
            {
                if (Subject.InputImage == null) return;
                HldDisplayViewEdit.Display.Image = Subject.InputImage;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldMakePoint Subject
        {
            get { return base.GetSubject() as HldMakePoint; }
            set { base.SetSubject(value); }
        }

        void PointMakeEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            Subject.Ran += Subject_Ran;
            source.ResetBindings(true);

            Display_InDrawObjectChanged(this, Subject.InPoint);
        }

        private void Subject_Ran(object sender, HldToolBase tool)
        {
            Display_InDrawObjectChanged(this, Subject.OutPoint);
        }

        #region InputPoints Event

        void Display_InDrawObjectChanged(object sender, InteractDrawObject drawObject)
        {
            if (drawObject == null) return;
            if (Subject.InPoint == null) return;
            Remove_In_nud_ValueChangedEvent();

            nud_InP0_X.Value = (decimal)Subject.InPoint.X;
            nud_InP0_Y.Value = (decimal)Subject.InPoint.Y;
            nud_InP0_T.Value = (decimal)(Subject.InPoint.ThetaRad * 180 / Math.PI);

            Add_In_nud_ValueChangedEvent();
            if (Subject.OutPoint == null) return;
            nud_OutP_X.Value = (decimal)Subject.OutPoint.X;
            nud_OutP_Y.Value = (decimal)Subject.OutPoint.Y;
            nud_OutP_T.Value = (decimal)(Subject.OutPoint.ThetaRad * 180 / Math.PI);


        }

        void Add_In_nud_ValueChangedEvent()
        {
            nud_InP0_X.ValueChanged += nud_InputDrawPoint_ValueChanged;
            nud_InP0_Y.ValueChanged += nud_InputDrawPoint_ValueChanged;
            nud_InP0_T.ValueChanged += nud_InputDrawPoint_ValueChanged;
        }

        void Remove_In_nud_ValueChangedEvent()
        {
            nud_InP0_X.ValueChanged -= nud_InputDrawPoint_ValueChanged;
            nud_InP0_Y.ValueChanged -= nud_InputDrawPoint_ValueChanged;
            nud_InP0_T.ValueChanged -= nud_InputDrawPoint_ValueChanged;
        }

        private void nud_InputDrawPoint_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nud = sender as NumericUpDown;

            Subject.InPoint = new HldPoint((double)nud_InP0_X.Value, (double)nud_InP0_Y.Value, (double)nud_InP0_T.Value * Math.PI / 180);
            nud_OutP_X.Value = (decimal)Subject.OutPoint.X;
            nud_OutP_Y.Value = (decimal)Subject.OutPoint.Y;
            nud_OutP_T.Value = (decimal)(Subject.OutPoint.ThetaRad * 180 / Math.PI);
            HldDisplayViewEdit.Display.Invalidate();
        }
        #endregion

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldWarpping), null);
            cb_Origin.DisplayMember = "Key"; cb_Origin.ValueMember = "Value";
            cb_Origin.DataSource = Enum.GetValues(typeof(OffsetControl.OriginDirection)).Cast<OffsetControl.OriginDirection>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_Origin.DataBindings.Add("SelectedValue", source, "Origin", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        //HldImageInfo inPointsImageInfo;
        //HldImageInfo outPointsImageInfo;
    }
}
