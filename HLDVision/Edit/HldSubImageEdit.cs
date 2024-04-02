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
    public partial class HldSubImageEdit : HldToolEditBase
    {
        public HldSubImageEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += BlurEdit_SubjectChanged;

            this.HldDisplayViewEdit.SelectedImageChanged += DisplayViewEdit_SelectedImageChanged;
            this.HldDisplayViewEdit.Display.DrawObjectChanged += Display_DrawObjectChanged;
        }

        HldRotationRectangle regionRect;

        protected override void InitDisplayViewEdit()
        {

        }

        void DisplayViewEdit_SelectedImageChanged(object sender, HldImageInfo imageInfo)
        {
            if (imageInfo.ImageName.Contains("Input"))
            {
                if (Subject.InputImage == null) return;
                HldDisplayViewEdit.Display.Image = Subject.InputImage;
                HldDisplayViewEdit.Display.InteractiveGraphicsCollection.Add(Subject.SubImageRegionRect);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldSubImage Subject
        {
            get { return base.GetSubject() as HldSubImage; }
            set { base.SetSubject(value); }
        }

        void BlurEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);

            Subject.SubImageRegionRect.Display = HldDisplayViewEdit.Display;
            regionRect = Subject.SubImageRegionRect;

            Display_DrawObjectChanged(this, Subject.SubImageRegionRect);

        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldSubImage), null);
        }

        //HldImageInfo subImageRegionInfo;


        #region InputPoints Event

        void Display_DrawObjectChanged(object sender, InteractDrawObject drawObject)
        {
            Remove_In_nud_ValueChangedEvent();

            nud_InP0_X.Value = (decimal)regionRect.LeftTop.X; P0.Value = (decimal)regionRect.LeftTop.Y;
            nud_InP1_X.Value = (decimal)regionRect.RightTop.X; nud_InP1_Y.Value = (decimal)regionRect.RightTop.Y;
            nud_InP2_X.Value = (decimal)regionRect.RightBottom.X; nud_InP2_Y.Value = (decimal)regionRect.RightBottom.Y;
            nud_InP3_X.Value = (decimal)regionRect.LeftBottom.X; nud_InP3_Y.Value = (decimal)regionRect.LeftBottom.Y;

            Add_In_nud_ValueChangedEvent();
        }

        void Add_In_nud_ValueChangedEvent()
        {
            nud_InP0_X.ValueChanged += nud_InputDrawPoint_ValueChanged; P0.ValueChanged += nud_InputDrawPoint_ValueChanged;
            nud_InP1_X.ValueChanged += nud_InputDrawPoint_ValueChanged; nud_InP1_Y.ValueChanged += nud_InputDrawPoint_ValueChanged;
            nud_InP2_X.ValueChanged += nud_InputDrawPoint_ValueChanged; nud_InP2_Y.ValueChanged += nud_InputDrawPoint_ValueChanged;
            nud_InP3_X.ValueChanged += nud_InputDrawPoint_ValueChanged; nud_InP3_Y.ValueChanged += nud_InputDrawPoint_ValueChanged;
        }

        void Remove_In_nud_ValueChangedEvent()
        {
            nud_InP0_X.ValueChanged -= nud_InputDrawPoint_ValueChanged; P0.ValueChanged -= nud_InputDrawPoint_ValueChanged;
            nud_InP1_X.ValueChanged -= nud_InputDrawPoint_ValueChanged; nud_InP1_Y.ValueChanged -= nud_InputDrawPoint_ValueChanged;
            nud_InP2_X.ValueChanged -= nud_InputDrawPoint_ValueChanged; nud_InP2_Y.ValueChanged -= nud_InputDrawPoint_ValueChanged;
            nud_InP3_X.ValueChanged -= nud_InputDrawPoint_ValueChanged; nud_InP3_Y.ValueChanged -= nud_InputDrawPoint_ValueChanged;
        }

        private void nud_InputDrawPoint_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nud = sender as NumericUpDown;

            if (nud == nud_InP0_X || nud == P0)
            {
                regionRect.LeftTop = new PointF((float)nud_InP0_X.Value, (float)P0.Value);
            }
            else if (nud == nud_InP1_X || nud == nud_InP1_Y)
            {
                regionRect.RightTop = new PointF((float)nud_InP1_X.Value, (float)nud_InP1_Y.Value);
            }
            else if (nud == nud_InP2_X || nud == nud_InP2_Y)
            {
                regionRect.RightBottom = new PointF((float)nud_InP2_X.Value, (float)nud_InP2_Y.Value);
            }
            else if (nud == nud_InP3_X || nud == nud_InP3_Y)
            {
                regionRect.LeftBottom = new PointF((float)nud_InP3_X.Value, (float)nud_InP3_Y.Value);
            }

            HldDisplayViewEdit.Display.Invalidate();
        }
        #endregion
        
    }
}
