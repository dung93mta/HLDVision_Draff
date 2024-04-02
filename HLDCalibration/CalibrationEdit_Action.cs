using HLDVision;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HLDCalibration
{
    public partial class HldCalibrationEdit
    {
        bool IsPossibleNonlinear { get { return calibration.CalcNonlinearCalibration(); } }
        HldCalibrationData CalData
        {
            get
            {
                if (calibration == null)
                    calibration = new HldCalibration();
                return calibration.CalData;
            }
            set
            {
                if (calibration == null)
                    calibration = new HldCalibration();

                if (value.CalDegree < (int)nud_Degree.Minimum)
                    value.CalDegree = (int)nud_Degree.Minimum;
                if (value.CalDegree > (int)nud_Degree.Maximum)
                    value.CalDegree = (int)nud_Degree.Maximum;

                if (value.UsingArea < (double)nud_UsingArea.Minimum)
                    value.UsingArea = (double)nud_UsingArea.Minimum;
                if (value.UsingArea > (double)nud_UsingArea.Maximum)
                    value.UsingArea = (double)nud_UsingArea.Maximum;

                calibration.CalData = value;

                base.NotifySubjectChanged(this, null);
            }

        }

        bool JobValidation(HldJob currentJob)
        {
            if (currentJob == null)
            {
                MessageBox.Show("There is no Job");
                return false;
            }

            HldAcquisition acquisitionTool = currentJob.ToolList[0] as HldAcquisition;

            if (acquisitionTool == null)
            {
                MessageBox.Show("First Tool must be 'Acquisition'!!");
                return false;
            }

            HldTemplateMatch templateMatchTool = currentJob.GetLastRunTool() as HldTemplateMatch;

            if (templateMatchTool == null)
            {
                MessageBox.Show("Last Tool must be 'PatternMatch' or 'TemplateMatching'");
                return false;
            }

            if (templateMatchTool.TemplateImage == null)
            {
                MessageBox.Show("There is not selected pattern.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        bool InitRobot()
        {
            if (robot == null)
            {
                MessageBox.Show("Selected robot is null", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!robot.StartCalibration(CalData.GPNo))
            {
                MessageBox.Show("Calibration can't start.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                robot.EndCalibration();
                return false;
            }

            if (MessageBox.Show("Attach the Jig, and Click 'OK'", "Info", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                != System.Windows.Forms.DialogResult.OK)
            {
                robot.EndCalibration();
                return false;
            }

            return true;
        }

        public Point3d? Run(HldJob job)
        {
            job.Run(true);

            HldTemplateMatch templateMatchingTool = job.GetLastRunTool() as HldTemplateMatch;

            double score = templateMatchingTool.Score;
            Point3d result = templateMatchingTool.ResultCP;

            if (score < 0.6)
            {
                MessageBox.Show("Find position is failed\r\nScore: " + score, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            HldImageInfo info;

            if (job == calibrationJob)
                info = calibrationImageInfo;
            else if (job == rotationJob)
                info = rotationImageInfo;
            else throw new Exception("Job is failed!");

            info.Image = templateMatchingTool.InputImage;
            HldImageInfo.DrwaingFunc originFunc = info.drawingFunc;
            info.drawingFunc = templateMatchingTool.imageList[1].drawingFunc;
            this.Invoke(new Action(HldDisplayViewEdit.RefreshImage));
            info.drawingFunc = originFunc;

            if (job == calibrationJob)
                CalData.CalImage = info.Image;
            else if (job == rotationJob)
                CalData.RotImage = info.Image;

            return result;
        }

        HldPoint originPoint = new HldPoint();

        bool CalibrationStart()
        {
            isStart = true;
            this.Invoke(new Action(delegate { btn_Cal_Start.Text = "Stop"; }));

            if (!JobValidation(calibrationJob)) return false;
            if (!InitRobot()) return false;

            int H = CalData.PointH;
            int V = CalData.PointV;
            double pitchH = CalData.PitchH;
            double pitchV = CalData.PitchV;

            bool bMoveOrigin2Cener = false;

            if (DialogResult.Yes == MessageBox.Show("Do You Want to Move Origin to Ceter of Display?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                bMoveOrigin2Cener = true;
            }

            if (H % 2 == 0 || V % 2 == 0)
            {
                MessageBox.Show("Points Count must be odd.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (H < 3 || V < 3)
            {
                MessageBox.Show("Points Count too Small.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            CalData.RefPoint = new Point2d[H * V];
            CalData.RobotPoint = new Point2d[H * V];

            this.Invoke(new Action(delegate { pbar_Cal.Maximum = H * V; }));


            try
            {
                for (int i = 0; i < V; i++)
                {
                    for (int j = 0; j < H; j++)
                    {
                        if (isStart == false) throw new Exception();

                        CalData.RobotPoint[i * H + j] = new Point2d((j - (H - 1) / 2) * pitchH, (i - (V - 1) / 2) * pitchV);
                        Point2d MovePoint = CalData.RobotPoint[i * H + j] + new Point2d(CalData.CalShiftH, CalData.CalShiftV);

                        if (!robot.WriteCalOffset((float)MovePoint.X, (float)MovePoint.Y, 0.0f))
                        {
                            MessageBox.Show("Calibration Offset send failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            throw new Exception();
                        }

                        if (robot.MoveCalibration() == false)
                        {
                            MessageBox.Show(string.Format("Can't find mark {0} : Move signal send failed.).", i * H + j), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            throw new Exception();
                        }

                        Point3d? result = Run(calibrationJob);

                        if (result == null)
                            throw new Exception();

                        CalData.RefPoint[i * H + j] = new Point2d(result.Value.X, result.Value.Y);
                        this.Invoke(new Action(pbar_Cal.PerformStep));

                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                robot.EndCalibration();
            }
                        
            CalData.VtoRMat.Mat = Calibration.CalcCalibrationMat(CalData.RefPoint, CalData.RobotPoint);

            if (bMoveOrigin2Cener)
                ChangeOrigin(new Point2d(HldDisplayViewEdit.Display.Image.Width / 2, HldDisplayViewEdit.Display.Image.Height / 2));
            else
                ChangeOrigin(originPoint.Point2d);

            ChangeOrigin(new Point2d(HldDisplayViewEdit.Display.Image.Width / 2, HldDisplayViewEdit.Display.Image.Height / 2));

            return true;
        }

        void ChangeOrigin(Point2d _originPoint)
        {
            Mat calMat = CalData.VtoRMat.Mat;

            Point2d robotCP = calibration.VtoR_Linear(_originPoint);

            for (int i = 0; i < CalData.RobotPoint.Length; i++)
            {
                CalData.RobotPoint[i] -= robotCP;
            }

            CalData.VtoRMat.Mat = Calibration.CalcCalibrationMat(CalData.RefPoint, CalData.RobotPoint);
            originPoint.Point2d = _originPoint;
        }

        bool RotationStart()
        {
            isStart = true;
            this.Invoke(new Action(delegate { btn_Rot_Start.Text = "Stop"; }));

            if (!JobValidation(rotationJob)) return false;
            if (!InitRobot()) return false;

            List<Point3d> resultPts = new List<Point3d>();
            List<double> listAngle = new List<double>();
            Point3d[] resultP = new Point3d[2];

            float angle;
            float angleFrom = CalData.AngleFrom;
            float angleTo = CalData.AngleTo;

            double shiftH = CalData.RotShiftH;
            double shiftV = CalData.RotShiftV;
            int rotCount = CalData.RotCount;

            this.Invoke(new Action(delegate { pbar_Rot.Maximum = rotCount; }));

            try
            {
                for (int i = 0; i < rotCount; i++)
                {
                    if (isStart == false) throw new Exception();

                    angle = angleFrom + (i) * (angleTo - angleFrom) / (rotCount - 1);
                    listAngle.Add(angle);

                    if (!robot.WriteCalOffset((float)shiftH, (float)shiftV, angle))
                    {
                        MessageBox.Show("Rotation Offset send failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new Exception();
                    }

                    if (!robot.MoveCalibration())
                    {
                        MessageBox.Show("Rotation signal send failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new Exception();
                    }

                    Point3d? result = Run(rotationJob);
                    if (result == null)
                        throw new Exception();

                    resultPts.Add(result.Value);
                    this.Invoke(new Action(pbar_Rot.PerformStep));
                }
            }
            catch (Exception ex)
            {
                HLDCommon.HldLogger.Log.Error(ex.Message);
                return false;
            }
            finally
            {
                robot.EndCalibration(); //Escape Robot Loop
            }

            List<double> Length = new List<double>();
            for (int i = 0; i < rotCount - 1; i++)
            {
                double L = Point2d.Distance(new Point2d(resultPts[i].X, resultPts[i].Y), new Point2d(resultPts[i + 1].X, resultPts[i + 1].Y));
                Length.Add(L);
            }

            CalData.RotPoints = resultPts;
            CalData.RotAngles = listAngle;

            if (rotCount == 2)
            {
                double dthetaDeg = (Calibration.VtoR(resultPts[1]).Z - Calibration.VtoR(resultPts[0]).Z) * 180 / Math.PI;
                float fT = angleTo - angleFrom;
                if (Math.Abs(fT - dthetaDeg) > 1)
                {
                    string str = "Difference of Angle is too big\r\n + P0: " + Calibration.VtoR(resultPts[0]).Z + ", P1: " + Calibration.VtoR(resultPts[1]).Z;
                    MessageBox.Show(str, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            else if (rotCount > 2)
            {
                double diff = Length.Max() - Length.Min();
                if (diff > 10)
                {
                    MessageBox.Show("Difference of Angle is too big", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            else
                return false;

            CalData.RotCenter = Calibration.VtoR(Calibration.FindRotationCenter(resultPts)) - new Point2d(shiftH, shiftV); 

            //////////////////////// Save RobotGP Pos. ////////////////////////
            float x = 0; float y = 0; float w = 0;

            robot.ReadPosition(CalData.GPNo, out x, out y, out w);

            CalData.RobotGP = new Point2d(x, y);
            CalData.RobotGPT = w;
            return true;
        }

        void DrawCalibrationResult(HLDVision.Display.HldDisplayView display)
        {
            int H = CalData.RobotPoints_Y.Count;
            int V = CalData.RobotPoints_X.Count;
            double pitchH = CalData.PitchH;
            double pitchV = CalData.PitchV;

            if (CalData.VtoRMat == null || CalData.RefPoint == null)
            {
                return;
            }

            display.ClearImage();
            (display as HLDVision.Display.HldDisplayViewInteract).InteractiveGraphicsCollection.Add(originPoint);
            this.HldDisplayViewEdit.CalibrationMat = CalData.VtoRMat.Mat;

            if (CalData.RobotPoint == null)
            {
                CalData.RobotPoint = new Point2d[CalData.PointV * CalData.PointH];
                for (int i = 0; i < V; i++)
                {
                    for (int j = 0; j < H; j++)
                    {
                        CalData.RobotPoint[i * H + j] = new Point2d((j - (H - 1) / 2) * pitchH, (i - (V - 1) / 2) * pitchV);
                    }
                }
            }

            gv_Cal_Result.Rows.Clear();

            for (int i = 0; i < CalData.RefPoint.Length; i++)
            {
                Point2d v = CalData.RefPoint[i];
                Point2d r = CalData.RobotPoint[i];
                {
                    gv_Cal_Result.Rows.Add(
                        i.ToString(),
                        r.X.ToString("f3"),
                        r.Y.ToString("f3"),
                        Calibration.VtoR(v).X.ToString("f3"),
                        Calibration.VtoR(v).Y.ToString("f3"),
                        (Calibration.VtoR(v) - r).X.ToString("f3"),
                        (Calibration.VtoR(v) - r).Y.ToString("f3"),
                        v.X.ToString("f3"),
                        v.Y.ToString("f3"));
                }
            }


            gv_Diff.Rows.Clear();
            List<double> diffX = new List<double>();
            List<double> diffY = new List<double>();

            for (int i = 0; i < gv_Cal_Result.RowCount - 1; i++)
            {
                diffX.Add(double.Parse(gv_Cal_Result.Rows[i].Cells[5].Value.ToString()));
                diffY.Add(double.Parse(gv_Cal_Result.Rows[i].Cells[6].Value.ToString()));
            }
            gv_Diff.Rows.Add("Diff_X", diffX.Min().ToString("f3"), diffX.Max().ToString("f3"), (3 * GetStdev(diffX)).ToString("f3"));
            gv_Diff.Rows.Add("Diff_Y", diffY.Min().ToString("f3"), diffY.Max().ToString("f3"), (3 * GetStdev(diffY)).ToString("f3"));

            DisplayCordinate(display);

            DrawLattiec(display);

            Point2d refP;
            Font font = new Font("Arial", 8, System.Drawing.FontStyle.Bold);

            if (calibrationImageInfo.Image == null)
            {
                if (CalData.CalImage != null)
                {
                    calibrationImageInfo.Image = CalData.CalImage;
                    HldDisplayViewEdit.RefreshImage();
                }
                else
                {
                    display.DrawString(string.Format("There is No Calibration Image"), font, Brushes.Red, new Point2d(0, 0));
                    return;
                }
            }

            double size = 5 / display.ZoomRatio;

            for (int i = 0; i < CalData.RefPoint.Length; i++)
            {
                refP = CalData.RefPoint[i];
                display.DrawString(string.Format("({0:0.00},{1:0.00})", Calibration.VtoR(refP).X, Calibration.VtoR(refP).Y),
                                        font, Brushes.Red, new Point2d(refP.X + 10.0, refP.Y - font.Size / display.ZoomRatio));
                display.DrawCross(Pens.Red, refP, 4 * size, 4 * size, 0);

                refP = CalData.RobotPoint[i];
                display.DrawString(string.Format("({0:0.00},{1:0.00})", refP.X, refP.Y),
                                        font, Brushes.Cyan, new Point2d(Calibration.RtoV(refP).X + 10.0, Calibration.RtoV(refP).Y + 0.5 * font.Size / display.ZoomRatio));
                display.DrawCross(Pens.Cyan, Calibration.RtoV(refP), 2 * size, 2 * size, 0);
            }
        }

        private double GetStdev(List<double> array)
        {
            if (array == null) return 0;
            double squareSum = 0;
            double average = array.Average();
            for (int i = 0; i < array.Count; i++)
            {
                squareSum += Math.Pow(array[i] - average, 2);
            }
            double stdev = Math.Sqrt(squareSum / (array.Count - 1));

            return stdev;
        }

        void DisplayCordinate(HLDVision.Display.HldDisplayView display)
        {
            int countX = 1; int countY = 1;
            for (int i = 1; i < Calibration.CalData.RobotPoint.Length; i++)
            {
                if (Calibration.CalData.RobotPoint[0].Y != Calibration.CalData.RobotPoint[i].Y)
                {
                    countX = i;
                    countY = Calibration.CalData.RobotPoint.Length / countX;
                    break;
                }
            }

            Font font = new Font("Arial", 10, System.Drawing.FontStyle.Bold);

            if (Calibration.GetVtoRMat() == null)
            {
                display.DrawString("No Calibration Data", font, Brushes.Red, new PointF(display.Width - 100, display.Height - 20));
                return;
            }

            double pitchH = double.MaxValue; double pitchV = double.MaxValue;
            for (int i = 0; i < calibration.CalData.RobotPoint.Length - 1; i++)
            {
                double dh = Calibration.CalData.RobotPoint[i + 1].X - Calibration.CalData.RobotPoint[i].X;
                if (dh < 0.1) continue;
                if (pitchH > dh) pitchH = dh;
            }
            for (int i = 0; i < calibration.CalData.RobotPoint.Length - 1; i++)
            {
                double dh = Calibration.CalData.RobotPoint[i + 1].Y - Calibration.CalData.RobotPoint[i].Y;
                if (dh < 0.1) continue;
                if (pitchV > dh) pitchV = dh;
            }

            if (pitchH == double.MaxValue) pitchH = pitchV;
            if (pitchV == double.MaxValue) pitchV = pitchH;

            {
                PointF HSP = display.PtoP(Calibration.RtoV_Linear(new Point2d(0 * pitchH, 0)));
                PointF HEP = display.PtoP(Calibration.RtoV_Linear(new Point2d(+2 * pitchH, 0)));
                PointF VSP = display.PtoP(Calibration.RtoV_Linear(new Point2d(0, 0 * pitchV)));
                PointF VEP = display.PtoP(Calibration.RtoV_Linear(new Point2d(0, +2 * pitchV)));

                display.DrawArrow(Pens.Yellow, HSP, HEP, 20, 20);
                display.DrawArrow(Pens.Red, VSP, VEP, 20, 20);
            }

            display.DrawString("X", font, Brushes.Yellow, Calibration.RtoV_Linear(new Point2d(2 * pitchH, 0)));
            display.DrawString("Y", font, Brushes.Red, Calibration.RtoV_Linear(new Point2d(0, 2 * pitchV)));

            display.DrawString("X ──", font, Brushes.Yellow, new PointF(0, display.Height / 2 - font.Size / display.ZoomRatio));
            display.DrawString("Y ──", font, Brushes.Red, new PointF(0, display.Height / 2 + font.Size / display.ZoomRatio));
        }

        private void DrawLattiec(HLDVision.Display.HldDisplayView display, bool isNonlinear = false)
        {
            int countX = 1; int countY = 1;
            for (int i = 1; i < Calibration.CalData.RobotPoint.Length; i++)
            {
                if (Calibration.CalData.RobotPoint[0].Y != Calibration.CalData.RobotPoint[i].Y)
                {
                    countX = i;
                    countY = Calibration.CalData.RobotPoint.Length / countX;
                    break;
                }
            }

            int pitchH = (int)Math.Round(Calibration.CalData.RobotPoint[1].X - Calibration.CalData.RobotPoint[0].X);
            int pitchV = (int)Math.Round(Calibration.CalData.RobotPoint[countX].Y - Calibration.CalData.RobotPoint[0].Y);

            Pen pen = new Pen(Brushes.LightGreen, 2 / display.ZoomRatio);
            pen.DashStyle = DashStyle.Dash;

            double size = 1000;
            Point2d originPt = Calibration.RtoV_Linear(new Point2d(0, 0));
            Point2d widthPt = Calibration.RtoV_Linear(new Point2d(size, 0));
            Point2d heightPt = Calibration.RtoV_Linear(new Point2d(0, size));
            double scale = Math.Min(Math.Abs(Calibration.CalData.CalImage.Width / widthPt.DistanceTo(originPt)), Math.Abs(Calibration.CalData.CalImage.Height / heightPt.DistanceTo(originPt)));
            size *= scale * 0.5;

            display.DrawLine(pen, Calibration.RtoV_Linear(new Point2d(-size, size)), Calibration.RtoV_Linear(new Point2d(size, size)));
            display.DrawLine(pen, Calibration.RtoV_Linear(new Point2d(-size, -size)), Calibration.RtoV_Linear(new Point2d(size, -size)));

            display.DrawLine(pen, Calibration.RtoV_Linear(new Point2d(size, -size)), Calibration.RtoV_Linear(new Point2d(size, size)));
            display.DrawLine(pen, Calibration.RtoV_Linear(new Point2d(-size, -size)), Calibration.RtoV_Linear(new Point2d(-size, size)));

            Pen px = new Pen(Brushes.Cyan, 1 / display.ZoomRatio);
            px.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            for (int i = -countY / 2; i <= countY / 2; i++)
                display.DrawLine(px, Calibration.RtoV_Linear(new Point2d(-size, i * pitchV)), Calibration.RtoV_Linear(new Point2d(size, i * pitchV)));

            Pen py = new Pen(Brushes.Red, 1 / display.ZoomRatio);
            py.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            for (int i = -countX / 2; i <= countX / 2; i++)
                display.DrawLine(py, Calibration.RtoV_Linear(new Point2d(i * pitchH, -size)), Calibration.RtoV_Linear(new Point2d(i * pitchH, size)));
            
            if (IsPossibleNonlinear)
            {
                px = new Pen(Brushes.Cyan, 2 / display.ZoomRatio);
                px.EndCap = System.Drawing.Drawing2D.LineCap.Flat;
                for (int i = -countY / 2; i <= countY / 2; i++)
                {
                    for (double x = -size; x < size; x++)
                    {
                        Point2d robotP = new Point2d(x, i * pitchV);
                        Point2d nonlinearP = Calibration.GetNonlinearValue(robotP);
                        display.DrawCross(px, Calibration.RtoV(nonlinearP), 2, 2, 0);
                    }
                }
                py = new Pen(Brushes.Red, 2 / display.ZoomRatio);
                py.EndCap = System.Drawing.Drawing2D.LineCap.Flat;
                for (int i = -countX / 2; i <= countX / 2; i++)
                {
                    for (double y = -size; y < size; y++)
                    {
                        Point2d robotP = new Point2d(i * pitchH, y);
                        Point2d nonlinearP = Calibration.GetNonlinearValue(robotP);
                        display.DrawCross(py, Calibration.RtoV(nonlinearP), 2, 2, 0);
                    }
                }
            }

        }

        void DrawRotationResult(HLDVision.Display.HldDisplayView display)
        {
            if (CalData.RotPoints == null || CalData.RotPoints.Count == 0)
            {
                return;
            }

            if (Calibration.GetVtoRMat() == null)
            {
                return;
            }

            if (CalData.RotAngles == null)
            {
                CalData.RotAngles = new List<double>();

                for (int i = 0; i < CalData.RotCount; i++)
                {
                    double angle = CalData.AngleFrom + (i) * (CalData.AngleTo - CalData.AngleFrom) / (CalData.RotCount - 1);
                    CalData.RotAngles.Add(angle);
                }
            }

            display.ClearImage();

            List<Point3d> resultPts = CalData.RotPoints;

            Point2d robotCP = new Point2d(CalData.RotCenter.X, CalData.RotCenter.Y);
            Point2d visionCP = Calibration.RtoV(robotCP);
            Point2d robotShiftCP = robotCP + new Point2d(CalData.RotShiftH, CalData.RotShiftV);
            Point2d visionShiftCP = Calibration.RtoV(robotShiftCP);

            float radius = (float)Math.Sqrt(Math.Pow(resultPts[0].X - visionShiftCP.X, 2) + Math.Pow(resultPts[0].Y - visionShiftCP.Y, 2));

            lbl_Rot_Origin_RLocation.Text = string.Format("({0:F3}, {1:F3})", robotCP.X, robotCP.Y);
            lbl_Rot_Origin_VLocation.Text = string.Format("({0:F3}, {1:F3})", visionCP.X, visionCP.Y);
            lbl_Rot_Origin_RadiusVision.Text = string.Format("{0:F3} px", radius);
            nud_GP_X.Value = (decimal)CalData.RobotGP.X; nud_GP_Y.Value = (decimal)CalData.RobotGP.Y;

            Point2d P0 = new Point2d(resultPts[0].X, resultPts[0].Y);

            if (CalData.VtoRMat != null)
                lbl_Rot_Origin_RadiusRobot.Text = string.Format("{0:F3} mm", OpenCvSharp.CPlusPlus.Point.Distance(Calibration.VtoR(P0), Calibration.VtoR(visionShiftCP)));

            Font font = new Font("Arial", 8, System.Drawing.FontStyle.Bold);
            display.ClearImage();

            Rect patternRect = CalData.RotPatternRect;
            if (patternRect.Width <= 0 || patternRect.Height <= 0)
                patternRect = new Rect(0, 0, 10, 10);

            if (rotationImageInfo.Image == null)
            {
                if (CalData.RotImage != null)
                {
                    rotationImageInfo.Image = CalData.RotImage;
                    HldDisplayViewEdit.RefreshImage();
                }
                else
                {
                    display.DrawString(string.Format("There is No RotationCenter Image"), font, new SolidBrush(Color.Red), new Point2d(0, 0));
                    return;
                }
            }

            // Display ListView
            gv_Rot_Pts.Rows.Clear();
            for (int i = 0; i < CalData.RotPoints.Count; i++)
            {
                gv_Rot_Pts.Rows.Add(new string[5] { i.ToString(), CalData.RotAngles[i].ToString("f2"), CalData.RotPoints[i].X.ToString("f3"), CalData.RotPoints[i].Y.ToString("f3"), (CalData.RotPoints[i].Z * 180 / Math.PI).ToString("f3") });
            }
          
            DisplayCordinate(display);

            for (int i = 0; i < CalData.RotPoints.Count; i++)
            {
                Point2d resultPt = new Point2d(resultPts[i].X, resultPts[i].Y);
                display.DrawCross(Pens.Red, resultPt, patternRect.Width, patternRect.Height, resultPts[i].Z);
                display.DrawRectangle(Pens.YellowGreen, resultPt, patternRect.Width, patternRect.Height, resultPts[i].Z);

                display.DrawString(string.Format("P" + i + ": ({0:0.00}, {1:0.00})\r\nT" + i + ": {2:0.0}Deg",
                    resultPts[i].X, resultPts[i].Y, resultPts[i].Z * 180 / Math.PI), font, Brushes.Gray, resultPt);
            }
            display.DrawString(string.Format("Robot : ({0:0.00}, {1:0.00})\r\nPixel : ({2:0.00}, {3:0.00})",
                robotShiftCP.X, robotShiftCP.Y, visionShiftCP.X, visionShiftCP.Y), font, Brushes.Gray, visionShiftCP);

            display.DrawString(string.Format("Robot : ({0:0.00}, {1:0.00})\r\nPixel : ({2:0.00}, {3:0.00})",
                robotCP.X, robotCP.Y, visionCP.X, visionCP.Y), font, Brushes.Red, visionCP);

            float size = 3f / display.ZoomRatio;

            Pen p = new Pen(Color.Lime, size);
            p.DashStyle = DashStyle.Dot;
            display.DrawLine(p, visionCP, visionShiftCP);

            p = new Pen(Color.Yellow, 2);
            p.DashPattern = new float[] { 2f, 1f, 1f, 1f };
            display.DrawEllipse(p, visionShiftCP, radius, radius);

            p = new Pen(Color.Red, size);
            p.DashStyle = DashStyle.Solid;
            display.DrawEllipse(p, visionCP, 2 * size, 2 * size);

            p = new Pen(Color.Orange, size);
            p.DashStyle = DashStyle.Solid;
            display.DrawEllipse(p, visionShiftCP, 2 * size, 2 * size);
        }
    }
}
