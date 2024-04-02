using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HLDCalibration
{
    public partial class HldCalibrationEdit
    {
        float[][] offsetData = null;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float[][] OffsetData
        {
            get { return offsetData; }
            set
            {
                offsetData = value;
                cb_ManualIndex.Items.Clear();
                gb_Manual.Enabled = false;
                if (offsetData == null) return;

                for (int i = 0; i < offsetData.Length; i++)
                {
                    for (int j = 0; j < offsetData[i].Length; j++)
                    {
                        if (offsetData[i][j] != 0f)
                        {
                            gb_Manual.Enabled = true;
                            for (int k = 0; k < offsetData.Length / 2; k++)
                                cb_ManualIndex.Items.Add(k + 1);
                            return;
                        }
                    }
                }
            }
        }

        float[] GetOffset()
        {
            return new float[3] { Convert.ToSingle(nud_OffsetX.Value), Convert.ToSingle(nud_OffsetY.Value), Convert.ToSingle(nud_OffsetT.Value) };
        }

        float[] GetAlign()
        {
            if (cb_ManualIndex.SelectedItem == null) return null;
            int index = (int)cb_ManualIndex.SelectedItem - 1;

            int toolNo = rb_Tool2.Checked ? 1 : 0;
            float[] alignData = offsetData[index * 2 + toolNo];
            if (alignData.Length != 3) return null;

            return alignData;
        }

        bool ManualMoveStart()
        {
            isStart = true;

            try
            {
                if (!InitRobot()) throw new Exception();

                this.Invoke(new Action(delegate { btn_ManualStart.Text = "Stop"; }));
                this.Invoke(new Action(delegate { gb_ManualMove.Enabled = true; }));
                this.Invoke(new Action(() => { cb_ManualMoveStart.Checked = false; }));

                while (isStart)
                {
                    if (cb_ManualMoveStart.Checked)
                    {
                        this.Invoke(new Action(() => { cb_ManualMoveStart.Enabled = false; cb_MoveTeachingPos.Enabled = false; }));

                        float[] alignData = (float[])this.Invoke(new Func<float[]>(GetAlign));
                        float[] offsetData = GetOffset();

                        if (alignData == null)
                        {
                            MessageBox.Show("Calibration Offset is wrong", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            throw new Exception();
                        }

                        if (MessageBox.Show(string.Format("Move Start?\r\n\r\nalign  :  (X : {0:F3} Y : {1:F3} T : {2:F3})\r\noffset : (X : {3:F3} Y : {4:F3} T : {5:F3})", alignData[0], alignData[1], alignData[2], offsetData[0], offsetData[1], offsetData[2]), "Manual Move", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                            == System.Windows.Forms.DialogResult.OK)
                        {
                            if (!robot.WriteCalOffset(alignData[0] + offsetData[0], alignData[1] + offsetData[1], alignData[2] + offsetData[2]))
                            {
                                MessageBox.Show("Calibration Offset send failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                throw new Exception();
                            }

                            if (robot.MoveCalibration() == false)
                            {
                                MessageBox.Show("Move signal send failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                throw new Exception();
                            }
                        }
                        this.Invoke(new Action(() => { cb_ManualMoveStart.Enabled = true; cb_MoveTeachingPos.Enabled = true; }));
                        this.Invoke(new Action(() => { cb_ManualMoveStart.Checked = false; }));
                    }
                    else if (cb_MoveTeachingPos.Checked)
                    {
                        this.Invoke(new Action(() => { cb_ManualMoveStart.Enabled = false; cb_MoveTeachingPos.Enabled = false; }));
                        if (MessageBox.Show("Move Start?.", "Manual Move", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                            == System.Windows.Forms.DialogResult.OK)
                        {
                            if (!robot.WriteCalOffset(0f, 0f, 0f))
                            {
                                MessageBox.Show("Calibration Offset send failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                throw new Exception();
                            }

                            if (robot.MoveCalibration() == false)
                            {
                                MessageBox.Show("Move signal send failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                throw new Exception();
                            }
                        }
                        this.Invoke(new Action(() => { cb_ManualMoveStart.Enabled = true; cb_MoveTeachingPos.Enabled = true; }));
                        this.Invoke(new Action(() => { cb_MoveTeachingPos.Checked = false; }));
                    }

                    if (!Thread.Yield()) Thread.Sleep(100);
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                this.Invoke(new Action(() => { cb_ManualMoveStart.Enabled = true; cb_MoveTeachingPos.Enabled = true; }));
                this.Invoke(new Action(() => { cb_ManualMoveStart.Checked = false; cb_MoveTeachingPos.Checked = false; }));
                this.Invoke(new Action(() => { gb_ManualMove.Enabled = false; }));
                if (robot != null && robot.IsOpen)
                    robot.EndCalibration();
            }

            return true;
        }

    }
}
