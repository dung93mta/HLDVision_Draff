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
using OpenCvSharp.CPlusPlus;

namespace HLDVision.Edit
{
    public partial class HldMakeRegionsEdit : HldToolEditBase
    {
        public HldMakeRegionsEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += MakeRegionsEdit_SubjectChanged;

        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldMakeRegions Subject
        {
            get { return base.GetSubject() as HldMakeRegions; }
            set { base.SetSubject(value); }
        }

        void MakeRegionsEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldMakeRegions), null);
            DataBindings.Add("RectPoint1", source, "Point0", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("RectPoint2", source, "Point1", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("RectPoint3", source, "Point2", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("RectPoint4", source, "Point3", true, DataSourceUpdateMode.OnPropertyChanged);

            nud_Long_TrayToCell.DataBindings.Add("Value", source, "LongTrayToCell", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Long_CellToCell.DataBindings.Add("Value", source, "LongCellToCell", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Long_CellArea.DataBindings.Add("Value", source, "LongCellArea", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Long_CellCount.DataBindings.Add("Value", source, "LongCellCount", true, DataSourceUpdateMode.OnPropertyChanged);

            nud_Short_TrayToCell.DataBindings.Add("Value", source, "ShortTrayToCell", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Short_CellToCell.DataBindings.Add("Value", source, "ShortCellToCell", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Short_CellArea.DataBindings.Add("Value", source, "ShortCellArea", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Short_CellCount.DataBindings.Add("Value", source, "ShortCellCount", true, DataSourceUpdateMode.OnPropertyChanged);

            nud_MakeMask_Left.DataBindings.Add("Value", source, "MaskLeft", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_MakeMask_Right.DataBindings.Add("Value", source, "MaskRight", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_MakeMask_Top.DataBindings.Add("Value", source, "MaskTop", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_MakeMask_Bottom.DataBindings.Add("Value", source, "MaskBottom", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public Point2f RectPoint1
        {
            get { return new Point2f((float)nud_MarkRect_P1_X.Value, (float)nud_MarkRect_P1_Y.Value); }
            set
            {
                nud_MarkRect_P1_X.Value = (decimal)value.X;
                nud_MarkRect_P1_Y.Value = (decimal)value.Y;
            }
        }

        public Point2f RectPoint2
        {
            get { return new Point2f((float)nud_MarkRect_P2_X.Value, (float)nud_MarkRect_P2_Y.Value); }
            set
            {
                nud_MarkRect_P2_X.Value = (decimal)value.X;
                nud_MarkRect_P2_Y.Value = (decimal)value.Y;
            }
        }

        public Point2f RectPoint3
        {
            get { return new Point2f((float)nud_MarkRect_P3_X.Value, (float)nud_MarkRect_P3_Y.Value); }
            set
            {
                nud_MarkRect_P3_X.Value = (decimal)value.X;
                nud_MarkRect_P3_Y.Value = (decimal)value.Y;
            }
        }

        public Point2f RectPoint4
        {
            get { return new Point2f((float)nud_MarkRect_P4_X.Value, (float)nud_MarkRect_P4_Y.Value); }
            set
            {
                nud_MarkRect_P4_X.Value = (decimal)value.X;
                nud_MarkRect_P4_Y.Value = (decimal)value.Y;
            }
        }
    }
}
