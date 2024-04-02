using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using HLDVision;
using OpenCvSharp.CPlusPlus;

namespace HLDCalibration
{
    [Serializable]
    public class HldCalibration
    {
        public HldCalibration()
        {
            CalData = new HldCalibrationData();
        }

        HldCalibrationData mCalData;

        public HldCalibrationData CalData
        {
            get { return mCalData; }
            set
            {
                mCalData = value;
                mCalData.IsPossibleNonlinear = CalcNonlinearCalibration();
            }
        }

        public Point2d RotCenter
        {
            get { return CalData.RotCenter; }
        }

        public Point3d RotCenter3d
        {
            get { return new Point3d(CalData.RotCenter.X, CalData.RotCenter.Y, 0); }
        }

        public Mat CalcCalibrationMat(Point2d[] visionPt, Point2d[] robotPt)
        {
            if (visionPt.Length != robotPt.Length)
                throw new Exception("imgPt.Length != robotPt.Length");

            if (robotPt.Length < 3) return null;

            // | a00  a01  a02 |   |Vx[i]|    |Rx[i]|
            // | a10  a11  a12 | * |Vy[i]| =  |Ry[i]| * (a20 * Vx[i] + a21 * Vy[i])
            // | a20  a21   1  |   |  1  |    |  1  |
            //
            // | Vx[0] Vy[0]   1     0     0     0    -Rx[0]Vx[0] -Rx[0]Vy[0] | | a00 |   | Rx[0] |
            // |   0     0     0   Vx[0] Vy[0]   1    -Ry[0]Vx[0] -Ry[0]Vy[0] | | a01 |   | Ry[0] |
            // |   :     :     :     :     :     :         :         :     :  | |  :  | = |   :   |
            // |   :     :     :     :     :     :         :         :     :  | |  :  |   |   :   |
            // | Vx[n] Vy[n]   1     0     0     0    -Rx[n]Vx[n] -Rx[n]Vy[n] | | a20 |   | Rx[n] |
            // |   0     0     0   Vx[n] Vy[n]   1    -Ry[n]Vx[n] -Ry[n]Vy[n] | | a21 |   | Rx[n] |
            //                            A                                        X          B


            //bool isHomo = false;
            //bool isAffine = false;

            if (robotPt.Length > 3) // Homogeneous
            {
                Mat homo = Cv2.FindHomography(visionPt, robotPt);
                double detPerspec = homo.Determinant();
                if (Math.Abs(detPerspec) > Math.Pow(10, -10))
                {
                    //isHomo = true;
                    CalData.VtoRMat.Mat = homo;
                    return CalData.VtoRMat.Mat;
                }
            }

            Point2f[] v2f = new Point2f[visionPt.Length];
            Point2f[] r2f = new Point2f[visionPt.Length];
            for (int i = 0; i < visionPt.Length; i++)
            {
                v2f[i] = new Point2f((float)visionPt[i].X, (float)visionPt[i].Y);
                r2f[i] = new Point2f((float)robotPt[i].X, (float)robotPt[i].Y);
            }
            Mat affine = Cv2.GetAffineTransform(v2f, r2f);

            Mat affine9 = new Mat(3, 3, MatType.CV_64FC1, new double[] {
                affine.At<double>(0, 0), affine.At<double>(0, 1), affine.At<double>(0, 2),
                affine.At<double>(1, 0), affine.At<double>(1, 1), affine.At<double>(1, 2),
                0, 0, 1 });

            double detAffine = affine9.Determinant();
            if (Math.Abs(detAffine) > Math.Pow(10, -10))
            {
                //isAffine = true;
                CalData.VtoRMat.Mat = affine9;
                return CalData.VtoRMat.Mat;
            }

            CalData.VtoRMat.Mat = CalCalibrationMat1D(visionPt, robotPt);

            CalData.IsPossibleNonlinear = CalcNonlinearCalibration();
            return CalData.VtoRMat.Mat;
        }

        public Mat GetAffineTransform(Point2d[] visionPt, Point2d[] robotPt)
        {
            if (visionPt.Length != robotPt.Length)
                throw new Exception("imgPt.Length != robotPt.Length");

            // | a00  a01  a02 |   |Vx[i]|    |Rx[i]|
            // | a10  a11  a12 | * |Vy[i]| =  |Ry[i]|
            //                     |  1  |
            //
            // | Vx[0] Vy[0]   1     0     0     0  | | a00 |   | Rx[0] |
            // |   0     0     0   Vx[0] Vy[0]   1  | | a01 |   | Ry[0] |
            // |   :     :     :     :     :     :  | |  :  | = |   :   |
            // |   :     :     :     :     :     :  | |  :  |   |   :   |
            // | Vx[n] Vy[n]   1     0     0     0  | | a11 |   | Rx[n] |
            // |   0     0     0   Vx[n] Vy[n]   1  | | a12 |   | Rx[n] |
            //                  A                        X          B

            Mat A = new Mat(visionPt.Length * 2, 6, MatType.CV_64FC1);
            Mat B = new Mat(visionPt.Length * 2, 1, MatType.CV_64FC1);

            A.SetTo(0);
            B.SetTo(0);

            for (int i = 0; i < visionPt.Length; i++)
            {
                A.Set<double>(2 * i, 0, visionPt[i].X);
                A.Set<double>(2 * i, 1, visionPt[i].Y);
                A.Set<double>(2 * i, 2, 1);
                B.Set<double>(2 * i, robotPt[i].X);

                A.Set<double>(2 * i + 1, 3, visionPt[i].X);
                A.Set<double>(2 * i + 1, 4, visionPt[i].Y);
                A.Set<double>(2 * i + 1, 5, 1);
                B.Set<double>(2 * i + 1, robotPt[i].Y);
            }

            try
            {
                Mat X = (A.Transpose() * A).Inv() * A.Transpose() * B;

                double[] Aelements = new double[9];
                X.GetArray(0, 0, Aelements);
                X.Dispose();
                Aelements[8] = 1;
                return new Mat(3, 3, MatType.CV_64FC1, Aelements);
            }
            catch
            {
                return null;
            }
            finally
            {
                A.Dispose();
                B.Dispose();
            }
        }

        public Mat CalCalibrationMat1D(Point2d[] visionPt, Point2d[] robotPt)
        {
            // 먼저 RtoV를 구한후 VtoR 계산한다.
            //
            // (Kx, Ky) = robotPt 벡터(1차원이므로 하나뿐이다),
            // 
            // |  a00    a01   a02 |   |Rx[i]|    |Vx[i]|
            // |  a01   -a00   a12 | * |Ry[i]| =  |Vy[i]| * ((Kx * Rx[i] + Ky * Ry[i]) * a20 + 1)
            // | Kxa20  Kxa20   1  |   |  1  |    |  1  |
            //
            // |  Rx[0] Ry[0]   1   0  (Kx*Rx[0] + Ky*Ry[0])Vx[0] | | a00 |   | Vx[0] |
            // | -Ry[0] Rx[0]   0   1  (Kx*Rx[0] + Ky*Ry[0])Vy[0] | | a01 |   | Vy[0] |
            // |    :     :     :   :             :               | | a02 | = |   :   |
            // |    :     :     :   :             :               | | a12 |   |   :   |
            // |  Rx[n] Ry[n]   1   0  (Kx*Rx[0] + Ky*Ry[0])Vx[n] | | a20 |   | Vx[n] |
            // | -Ry[n] Rx[n]   0   1  (Kx*Rx[0] + Ky*Ry[0])Vy[n] |           | Vy[n] |
            //                           A^-1                          X          B

            Mat A = new Mat(robotPt.Length * 2, 5, MatType.CV_64FC1);
            Mat B = new Mat(robotPt.Length * 2, 1, MatType.CV_64FC1);
            Point2d K = new Point2d(robotPt[robotPt.Length - 1].X - robotPt[0].X, robotPt[robotPt.Length - 1].Y - robotPt[0].Y);
            K *= (1 / K.DistanceTo(new Point2d()));

            A.SetTo(0); B.SetTo(0);

            for (int i = 0; i < robotPt.Length; i++)
            {
                A.Set<double>(2 * i, 0, robotPt[i].X);
                A.Set<double>(2 * i, 1, robotPt[i].Y);
                A.Set<double>(2 * i, 2, 1);
                A.Set<double>(2 * i, 4, -visionPt[i].X * (K.X * robotPt[i].X + K.Y * robotPt[i].Y));
                B.Set<double>(2 * i, visionPt[i].X);

                A.Set<double>(2 * i + 1, 0, -robotPt[i].Y);
                A.Set<double>(2 * i + 1, 1, robotPt[i].X);
                A.Set<double>(2 * i + 1, 3, 1);
                A.Set<double>(2 * i + 1, 4, -visionPt[i].Y * (K.X * robotPt[i].X + K.Y * robotPt[i].Y));
                B.Set<double>(2 * i + 1, visionPt[i].Y);
            }

            try
            {
                Mat X = (A.Transpose() * A).Inv() * A.Transpose() * B;

                double[] Aelements = new double[6];
                X.GetArray(0, 0, Aelements);

                Mat invMat = new Mat(3, 3, MatType.CV_64FC1, new double[] {
                    Aelements[0], Aelements[1], Aelements[2],
                    Aelements[1], -Aelements[0], Aelements[3],
                    K.X * Aelements[4], K.Y * Aelements[4], 1 });

                double det = invMat.Determinant();
                if (Math.Abs(det) < 0.00001) return null;
                Mat calMat = invMat.Inv();
                calMat *= (1 / calMat.At<double>(2, 2));
                invMat.Dispose();
                return calMat;
            }
            catch
            {
                return null;
            }
            finally
            {
                A.Dispose();
                B.Dispose();
            }
        }

        public Mat GetVtoRMat()
        {
            return CalData.VtoRMat.Mat;
        }

        public Mat GetRtoVMat()
        {
            return CalData.VtoRMat.Mat.Inv();
        }

        public Point3d VtoR(Point3d vision, bool isWarp = false, bool isAngleWarp = false)
        {
            if (CalData.VtoRMat == null)
                throw new Exception("VtoRMat is null");

            Point2d vision2d = new Point2d(vision.X, vision.Y);

            // 로봇 좌표계 T축 보정 시작
            Point2d RobotP0, RobotP1, RobotP2;
            double dT;

            if (isWarp) // [Vision의 수직도 < 로봇의 X,Y 직각도 라고 판단] => 로봇 좌표계에서 각도 측정 (카메라가 기울어진다든지...)
            {
                double L = 1000;
                List<Point2d> X = new List<Point2d>();
                X.Add(RtoV(new Point2d(L, 0)));
                X.Add(RtoV(new Point2d(-L, 0)));
                X.Add(RtoV(new Point2d(0, L)));
                X.Add(RtoV(new Point2d(0, -L)));
                //X.Add(RtoV(new Point2d(0, 0)));

                X.Sort((a, b) => a.X > b.X ? -1 : 1);

                Point2d RobotZeroDirection = VtoR(X[0]);
                //if (X[0].X == 0) RobotZeroDirection = RtoV(new Point2d(L, 0));
                //else if (X[0].X == 1) RobotZeroDirection = RtoV(new Point2d(-L, 0));
                //else if (X[0].X == 2) RobotZeroDirection = RtoV(new Point2d(0, L));
                //else RobotZeroDirection = RtoV(new Point2d(0, -L));

                RobotP0 = VtoR(vision2d);
                //RobotP1 = VtoR(vision2d)/*Vision -> Robot*/ + RobotZeroDirection/*Robot*/;
                RobotP1 = RobotZeroDirection;
                RobotP2 = VtoR(vision2d + new Point2d(L * Math.Cos(vision.Z), L * Math.Sin(vision.Z)));
                RobotP2 = RobotP2 - RobotP0;
                double dsin = (RobotP1.X * RobotP2.Y - RobotP1.Y * RobotP2.X);
                double dcos = (RobotP1.X * RobotP2.X + RobotP1.Y * RobotP2.Y);
                dT = Math.Atan2(dsin, dcos);

                RobotP1 = RobotZeroDirection;
                RobotP2 = VtoR(RtoV(new Point2d(0, 0)) + new Point2d(L, 0)); //Vision 0도일때 좌표
                if (!isAngleWarp) // 0도 보상 원하지 않을경우, 로봇 Tool 좌표계를 연계해서 사용할 경우...
                {
                    dsin = (RobotP1.X * RobotP2.Y - RobotP1.Y * RobotP2.X);
                    dcos = (RobotP1.X * RobotP2.X + RobotP1.Y * RobotP2.Y);
                    dT -= Math.Atan2(dsin, dcos);
                }
            }
            else // [Vision의 수직도 > 로봇의 X,Y 직각도 라고 판단] => 비젼 좌표계에서 각도 측정후 부호 반전
            {
                RobotP0 = VtoR(vision2d);
                dT = ((CalData.AngleTo - CalData.AngleFrom) / (CalData.RotPoints[CalData.RotPoints.Count - 1].Z - CalData.RotPoints[0].Z) > 0) ? vision.Z : -vision.Z;
            }
            return new Point3d(RobotP0.X, RobotP0.Y, dT);
        }

        public Point2d VtoR_Linear(Point2d vision)
        {
            if (CalData.VtoRMat == null)
                throw new Exception("VtoRMat is null");

            Mat visionMat = new Mat(3, 1, MatType.CV_64FC1, new double[] { vision.X, vision.Y, 1.0 });
            visionMat = CalData.VtoRMat.Mat * visionMat;

            Point2d robotP = new Point2d(visionMat.At<double>(0, 0) / visionMat.At<double>(2, 0), visionMat.At<double>(1, 0) / visionMat.At<double>(2, 0));

            return robotP;
        }

        public Point2d RtoV_Linear(Point2d robot)
        {
            if (CalData.VtoRMat == null)
                throw new Exception("VtoRMat is null");

            Mat RtoVMat = GetRtoVMat();
            Mat RobotMat = new Mat(3, 1, MatType.CV_64FC1, new double[] { robot.X, robot.Y, 1.0 });
            RobotMat = RtoVMat * RobotMat;
            RtoVMat.Dispose();
            return new Point2d(RobotMat.At<double>(0, 0) / RobotMat.At<double>(2, 0), RobotMat.At<double>(1, 0) / RobotMat.At<double>(2, 0));
        }

        public Point2d GetRotationCenter()
        {
            return CalData.RotCenter;
        }

        public Point2d FindRotationCenter(Point2d p1, Point2d p2, double t1, double t2)
        {
            double T = t2 - t1;
            Mat P0 = new Mat(2, 1, MatType.CV_64FC1);
            Mat R = new Mat(2, 2, MatType.CV_64FC1, new double[] { Math.Cos(T), Math.Sin(-T), Math.Sin(T), Math.Cos(T) });
            Mat E = Mat.Eye(2, 2, MatType.CV_64FC1);
            Mat P1 = new Mat(2, 1, MatType.CV_64FC1, new double[] { p1.X, p1.Y });
            Mat P2 = new Mat(2, 1, MatType.CV_64FC1, new double[] { p2.X, p2.Y });

            P0 = (R - E).Inv() * (R * P1 - P2);
            Point2d RotCenter = new Point2d(P0.At<double>(0), P0.At<double>(1));
            return RotCenter;
        }

        public Point2d FindRotationCenter(List<Point3d> Pts)
        {
            Point2d P0;
            if (Pts.Count < 2)
                return new Point2d(0, 0);
            else if (Pts.Count == 2)
            {
                Point2d P1 = new Point2d(Pts[0].X, Pts[0].Y);
                Point2d P2 = new Point2d(Pts[1].X, Pts[1].Y);
                P0 = FindRotationCenter(P1, P2, Pts[0].Z, Pts[1].Z);
                return P0;
            }
            else
            {
                Mat A = new Mat(Pts.Count, 3, MatType.CV_64FC1);
                Mat B = new Mat(Pts.Count, 1, MatType.CV_64FC1);
                Mat X = new Mat(3, 1, MatType.CV_64FC1);
                for (int i = 0; i < Pts.Count; i++)
                {
                    A.Set<double>(i, 0, Pts[i].X);
                    A.Set<double>(i, 1, Pts[i].Y);
                    A.Set<double>(i, 2, 1);
                    B.Set<double>(i, 0, -Pts[i].X * Pts[i].X - Pts[i].Y * Pts[i].Y);
                }
                
                X = (A.Transpose() * A).Inv() * (A.Transpose() * B);
                P0 = new Point2d(X.At<double>(0, 0) * -0.5, X.At<double>(1, 0) * -0.5);
                return P0;
            }
        }

        public bool FindRotationCenter(List<Point2d> _listP1, List<Point2d> _listP2, double _length, out Point2d CP1, out Point2d CP2)
        {
            //  || P_r[1] - P_r[0] ||^2 = 2R_r^2 - 2R_r^2 cos(dT)
            //  || P_l[1] - P_l[0] ||^2 = 2R_l^2 - 2R_l^2 cos(dT)
            //
            //  || P_r[1] - P_r[0] ||^2     2R_r^2(1 - cos(dT))     R_r^2        || P_r[1] - P_r[0] ||     R_r
            // ------------------------- = --------------------- = -------  =>  ----------------------- = ----- = K
            //  || P_l[1] - P_l[0] ||^2     2R_l^2(1 - cos(dT))     R_l^2        || P_l[1] - P_l[0] ||     R_l

            CP1 = new Point2d();
            CP2 = new Point2d();

            //Point2d P0 = new Point2d();
            if (_listP1 == null || _listP2 == null) return false;
            if (_listP1.Count != _listP2.Count) return false;
            if (_listP1.Count < 3) return false;

            double length1 = _listP1[_listP1.Count - 1].DistanceTo(_listP1[0]);
            double length2 = _listP2[_listP1.Count - 1].DistanceTo(_listP2[0]);

            Point2d Lvector1 = _listP1[_listP1.Count - 1] - _listP1[0];
            Point2d Rvector1 = new Point2d(-Lvector1.Y, Lvector1.X) * (1 / length1);
            Rvector1 = Rvector1.DotProduct(_listP1[_listP1.Count / 2] - _listP1[0]) > 0 ? -Rvector1 : Rvector1;

            Point2d Lvector2 = _listP2[_listP2.Count - 1] - _listP2[0];
            Point2d Rvector2 = new Point2d(-Lvector2.Y, Lvector2.X) * (1 / length2);
            Rvector2 = Rvector2.DotProduct(_listP2[_listP2.Count / 2] - _listP2[0]) > 0 ? -Rvector2 : Rvector2;

            // L^2 = R_r^2 + R_l^2 - 2R_r * R_l * cosT
            // L^2 = R_l^2 * (K^2 + 1 - 2KcosT)
            //
            // R_l^2 = L^2 / (1 + K^2 - 2KcosT)
            //

            double K = length2 / length1;
            double cosT = Rvector2.DotProduct(Rvector1);

            double R1 = Math.Sqrt((_length * _length) / (1 + K * K - 2 * K * cosT));
            double R2 = K * R1;

            CP1 = (_listP1[_listP1.Count - 1] + _listP1[0]) * 0.5 + Rvector1 * R1 * Math.Sin(Math.Acos(length1 / 2 / R1));
            CP2 = (_listP2[_listP2.Count - 1] + _listP2[0]) * 0.5 + Rvector2 * R2 * Math.Sin(Math.Acos(length2 / 2 / R2));

            return true;
        }


        public Point3d RToT(Point3d point)
        {
            Point2d ToolOffset = RotCenter;
            Mat RobotPt = new Mat(2, 1, MatType.CV_64FC1, new double[] { point.X, point.Y });
            Mat Rc1 = new Mat(2, 1, MatType.CV_64FC1, new double[] { ToolOffset.X, ToolOffset.Y });
            Mat R1 = new Mat(2, 2, MatType.CV_64FC1, new double[] { Math.Cos(point.Z), -Math.Sin(point.Z), Math.Sin(point.Z), Math.Cos(point.Z) });

            Mat Rresult = R1 * Rc1 - Rc1 + RobotPt;
            Point3d sol1 = new Point3d(Rresult.At<double>(0, 0), Rresult.At<double>(1, 0), point.Z);
            //Point2d ToolOffset = RotCenter;
            //Mat Rc = new Mat(3, 1, MatType.CV_64FC1, new double[] { ToolOffset.X + point.X, ToolOffset.Y + point.Y, 1.0 });
            //Mat R = Cv2.GetRotationMatrix2D(new Point2f((float)point.X, (float)point.Y), -point.Z * (180 / Math.PI), 1);

            //R = R * Rc;
            //Point3d sol2 = new Point3d(R.At<double>(0, 0) - ToolOffset.X, R.At<double>(1, 0) - ToolOffset.Y, point.Z);            
            return sol1;
        }

        public Point3d RToT(Point3d point, Point2d RobotPos = new Point2d())
        {
            //Mat RR = new Mat(2, 1, MatType.CV_64FC1, new double[] { point.X, point.Y });
            //Mat Rc = new Mat(2, 1, MatType.CV_64FC1, new double[] { RotCenter.X, RotCenter.Y });
            //Mat R = new Mat(2, 2, MatType.CV_64FC1, new double[] { Math.Cos(point.Z), -Math.Sin(point.Z), Math.Sin(point.Z), Math.Cos(point.Z) });

            //Mat Rresult = R * Rc - Rc + RR;
            //return new Point3d(Rresult.At<double>(0, 0) + point.X, Rresult.At<double>(1, 0) + point.Y, point.Z);
            if (RobotPos == new Point2d())
                RobotPos = CalData.RobotGP;
            Point2d ToolOffset = RotCenter - CalData.RobotGP + RobotPos;

            Mat Rc = new Mat(3, 1, MatType.CV_64FC1, new double[] { point.X, point.Y, 1.0 });
            Mat R = Cv2.GetRotationMatrix2D(new Point2f((float)ToolOffset.X, (float)ToolOffset.Y), (float)(point.Z * (180 / Math.PI)), 1);
            R = R * Rc;

            return new Point3d(-R.At<double>(0, 0), -R.At<double>(1, 0), -point.Z);
        }

        public Point3d RToT(Point3d Start, Point3d Target, Point2d NowPos = new Point2d())
        {
            Point2d Rotcenter = CalData.RotCenter;
            double dT = Target.Z - Start.Z;
            Mat R = new Mat(2, 2, MatType.CV_64FC1, new double[] { Math.Cos(dT), -Math.Sin(dT), Math.Sin(dT), Math.Cos(dT) });
            Mat S = new Mat(2, 1, MatType.CV_64FC1, new double[] { Start.X, Start.Y });
            Mat T = new Mat(2, 1, MatType.CV_64FC1, new double[] { Target.X, Target.Y });
            Mat Rc = new Mat(2, 1, MatType.CV_64FC1, new double[] { Rotcenter.X, Rotcenter.Y });
            Mat Now = new Mat(2, 1, MatType.CV_64FC1, new double[] { NowPos.X, NowPos.Y });
            Mat GP = new Mat(2, 1, MatType.CV_64FC1, new double[] { mCalData.RobotGP.X, mCalData.RobotGP.Y });

            if (NowPos == new Point2d()) Now = GP.Clone();

            R = T - (R * (S - (Rc + Now - GP)) + (Rc + Now - GP));
            Point3d result = new Point3d(R.At<double>(0, 0), R.At<double>(1, 0), dT);

            return result;
        }
        public Point2d RotCent_convert(Point3d visionPT)
        {
            Point2d ToolOffset_Vision = RtoV(RotCenter);
            if (mCalData.CalImage != null) //예외처리 보안 필요
            {
                ToolOffset_Vision.X -= (mCalData.CalImage.Width / 2);
                ToolOffset_Vision.Y -= (mCalData.CalImage.Height / 2);
            }
            else
                ToolOffset_Vision = new Point2d(0, 0);

            Mat VisionPT = new Mat(2, 1, MatType.CV_64FC1, new double[] { visionPT.X, visionPT.Y }); //RtoT와 다른점 : robot좌표가 아닌 vision좌표
            Mat Rc1 = new Mat(2, 1, MatType.CV_64FC1, new double[] { ToolOffset_Vision.X, ToolOffset_Vision.Y });
            Mat R1 = new Mat(2, 2, MatType.CV_64FC1, new double[] { Math.Cos(visionPT.Z), -Math.Sin(visionPT.Z), Math.Sin(visionPT.Z), Math.Cos(visionPT.Z) }); //radian? degree?

            Mat Result = R1 * Rc1 - Rc1 + VisionPT;
            Point2d sol1 = new Point2d(Result.At<double>(0, 0), Result.At<double>(1, 0));
            return sol1;
        }

        public Point3d RtoV(Point3d robot)
        {
            if (CalData.VtoRMat == null)
                throw new Exception("VtoRMat is null");

            Mat RtoVMat = GetRtoVMat();
            Mat RobotMat = new Mat(3, 1, MatType.CV_64FC1, new double[] { robot.X, robot.Y, 1.0 });
            RobotMat = RtoVMat * RobotMat;
            RtoVMat.Dispose();
            return new Point3d(RobotMat.At<double>(0, 0) / RobotMat.At<double>(2, 0), RobotMat.At<double>(1, 0) / RobotMat.At<double>(2, 0), -robot.Z * (Math.PI / 180));
        }

        #region Perspective Point
        public bool GetPerspectivePoint(double _focalLength, double _senserSizeW, double _senserSizeH, out Point2d P0, out double H)
        {
            P0 = new Point2d();
            H = 0;
            if (_focalLength <= 0) return false;
            if (mCalData == null) return false;

            // Vision 4Point
            Point2d RT, RB, LT, LB;
            RT = VtoR_Linear(new Point2d(mCalData.Width, 0));
            RB = VtoR_Linear(new Point2d(mCalData.Width, mCalData.Height));
            LT = VtoR_Linear(new Point2d(0, 0));
            LB = VtoR_Linear(new Point2d(0, mCalData.Height));

            // Robot Coor Max, Min
            Point2d right = GetRobotZeroPos(RT, RB);
            Point2d left = GetRobotZeroPos(LT, LB);
            Point2d top = GetRobotZeroPos(LT, RT);
            Point2d bottom = GetRobotZeroPos(LB, RB);
            Point2d O = new Point2d(0, 0);

            // Angle
            // tanT = (CCD Size) / (2*focal length)
            double T_horizent = Math.Atan(_senserSizeW * RtoV_Linear(right).DistanceTo(RtoV_Linear(left)) / 2 / _focalLength);
            double T_vertical = Math.Atan(_senserSizeH * RtoV_Linear(top).DistanceTo(RtoV_Linear(bottom)) / 2 / _focalLength);

            //double T_horizent1 = Math.Atan(_senserSizeW * RtoV_Linear(right).DistanceTo(RtoV_Linear(O)) / _focalLength);
            //double T_vertical1 = Math.Atan(_senserSizeH * RtoV_Linear(top).DistanceTo(RtoV_Linear(O)) / _focalLength);

            //Point2d l = RtoV_Linear(left);
            //Point2d r = RtoV_Linear(right);
            //Point2d o = RtoV_Linear(O);

            // P0 = (B^2 - 1 - A^2 * B^2) / (-2) * L+
            // A = (L- * 2cosT) / (L+ + L-)
            // B = sqrt(1 / (1 + A^2 - 2AcosT))
            // L0 = A * L1
            // L1 = B * L+

            double A_h = (left.DistanceTo(O) * 2 * Math.Cos(T_horizent)) / (right.DistanceTo(left));
            double B_h = Math.Sqrt(1 / (1 + A_h * A_h - 2 * A_h * Math.Cos(T_horizent)));

            double A_v = (bottom.DistanceTo(O) * 2 * Math.Cos(T_vertical)) / (bottom.DistanceTo(top));
            double B_v = Math.Sqrt(1 / (1 + A_v * A_v - 2 * A_v * Math.Cos(T_vertical)));

            double P0_h = (B_h * B_h - 1 - A_h * A_h * B_h * B_h) / -2 * right.DistanceTo(O);
            double P0_v = (B_v * B_v - 1 - A_v * A_v * B_v * B_v) / -2 * top.DistanceTo(O);

            P0 = right * (P0_h / right.DistanceTo(O)) + top * (P0_v / top.DistanceTo(O));
            double L0 = A_h * B_h * right.DistanceTo(O);
            double L_P0 = P0.DistanceTo(O);

            H = Math.Sqrt(L0 * L0 - L_P0 * L_P0);

            return true;
        }

        Point2d GetRobotZeroPos(Point2d P1, Point2d P2)
        {
            // (1 - k) * P1 + k * P2 = P
            // (1 - k) * P1.Y + k * P2.Y = 0 or (1 - k) * P1.X + k * P2.X = 0
            // P1.? = k * (P1.? - P2.?)
            // k = P1.? / (P1.? - P2.?),  0 < k < 1

            Point2d rightDirection = P1 + P2;
            double k = Math.Abs(rightDirection.X) > Math.Abs(rightDirection.Y) ? P1.Y / (P1.Y - P2.Y) : P1.X / (P1.X - P2.X);

            Point2d P = P1 * (1 - k) + P2 * k;
            return P;
        }

        public Mat GetTray2RobotPerspectiveMatrix(Point2d[] _trayP, Point2d _P0, double _H, double _trayWidth, double _trayHeight)
        {
            // dr0 = r0 * dh / (h + dh)   => dr0 * (h + dh) = r0 * dh   => dh * (dr0 - r0) = -h * dr0 
            // dr2 = r2 * -dh / (h - dh)  => dr2 * (h - dh) = r2 * -dh  => dh * (-dr2 + r2) = -h * dr2 
            // dh / h = -dr0 / (dr0 - r0) = dr2 / (dr2 - r2)

            //         x
            // dr = ------- r
            //      (1 + x)
            // l[0] + x/(1+x)r[0] = l[2] - x/(1-x)r[2]
            //
            // l[2] - l[0] = x/(1+x)r[0] + x/(1-x)r[2]
            //                x(1-x)r[0] + x(1+x)r[2]
            // l[2] - l[0] = ------------------------- => x(1-x)r[0] + x(1+x)r[2] = (r[2] - r[0])x^2 + (r[0] + r[2])x (분모는 대충 1이니깐 일단 근사화하자... 귀찮)
            //                     (1-x)(1+x)
            // 
            // (r[2] - r[0])x^2 + (r[0] + r[2])x + l[0] - l[2] = 0
            double[] r = new double[4];
            double[] L = new double[4];
            Point2d[] e_r = new Point2d[4];
            Point2d[] e_l = new Point2d[4];

            if (_trayP == null) return null;

            Point2d CP = HldFunc.FindCrossPoint(new HldLine(_trayP[0], _trayP[2]), new HldLine(_trayP[1], _trayP[3]));

            for (int i = 0; i < 4; i++)
            {
                L[i] = _trayP[i].DistanceTo(CP);
                r[i] = _trayP[i].DistanceTo(_P0);
                e_l[i] = (_trayP[i] - CP) * (1 / L[i]);
                e_r[i] = (_trayP[i] - _P0) * (1 / r[i]);
            }

            double[] dr = new double[4] { 0, 0, 0, 0 };
            double x1, x2;

            if (r[0] != r[2])
            {
                if (!GetRootEquation2nd(Math.Abs(r[2] * e_r[2].X) - Math.Abs(r[0] * e_r[0].X), Math.Abs(r[0] * e_r[0].X) + Math.Abs(r[2] * e_r[2].X), Math.Abs(L[0] * e_l[0].X) - Math.Abs(L[2] * e_l[2].X), out x1, out x2))
                    return null;
                if (Math.Abs(x1) > 1 && Math.Abs(x2) > 1) return null;
                double x = Math.Abs(x1) < 1 ? x1 : x2;
                dr[0] = x / (1 + x) * r[0];
                dr[2] = -x / (1 - x) * r[2];
            }

            if (r[1] != r[3])
            {
                if (!GetRootEquation2nd(Math.Abs(r[3] * e_r[3].X) - Math.Abs(r[1] * e_r[1].X), Math.Abs(r[1] * e_r[1].X) + Math.Abs(r[3] * e_r[3].X), Math.Abs(L[1] * e_l[1].X) - Math.Abs(L[3] * e_l[3].X), out x1, out x2))
                    return null;
                if (Math.Abs(x1) > 1 && Math.Abs(x2) > 1) return null;
                double x = Math.Abs(x1) < 1 ? x1 : x2;
                dr[1] = x / (1 + x) * r[1];
                dr[3] = -x / (1 - x) * r[3];
            }


            Mat tray = new Mat(600, 600, MatType.CV_8UC1, 0);
            Point2d moveP = new Point2d(300, 300);

            Point2d[] newTray = new Point2d[4];

            for (int i = 0; i < 4; i++)
            {
                tray.Line(_trayP[i] + moveP, _trayP[(i + 1) % 4] + moveP, 50);
            }

            for (int i = 0; i < 4; i++)
            {
                newTray[i] = _trayP[i] + e_r[i] * dr[i];
                r[i] = r[i] + dr[i];
            }

            Point2d CP2 = HldFunc.FindCrossPoint(new HldLine(newTray[0], newTray[2]), new HldLine(newTray[1], newTray[3]));
            for (int i = 0; i < 4; i++)
            {
                L[i] = newTray[i].DistanceTo(CP2);
            }


            for (int i = 0; i < 4; i++)
            {
                tray.Line(newTray[i] + moveP, newTray[(i + 1) % 4] + moveP, 100);
            }

            // Adjust Size
            double maxMeasuredlength = r[0] > r[1] ? newTray[0].DistanceTo(newTray[2]) : newTray[1].DistanceTo(newTray[3]);
            // Check r[1] * 2;


            double traylength = Math.Sqrt(_trayHeight * _trayHeight + _trayWidth * _trayWidth);
            double scale = traylength / maxMeasuredlength;

            Point2f[] tray_old2f = new Point2f[4];
            Point2f[] tray_new2f = new Point2f[4];

            for (int i = 0; i < 4; i++)
            {
                newTray[i] = newTray[i] + e_r[i] * (scale - 1) * r[i];
                r[i] = r[i] * scale;

                tray_old2f[i] = new Point2f((float)_trayP[i].X, (float)_trayP[i].Y);
                tray_new2f[i] = new Point2f((float)newTray[i].X, (float)newTray[i].Y);
            }

            Point2d CP3 = HldFunc.FindCrossPoint(new HldLine(newTray[0], newTray[2]), new HldLine(newTray[1], newTray[3]));
            for (int i = 0; i < 4; i++)
            {
                L[i] = newTray[i].DistanceTo(CP3);
            }
            for (int i = 0; i < 4; i++)
            {
                tray.Line(newTray[i] + moveP, newTray[(i + 1) % 4] + moveP, 250);
            }
            tray.Dispose();

            Mat perspectiveMat = Cv2.GetPerspectiveTransform(tray_old2f, tray_new2f);
            return perspectiveMat;
        }

        public bool GetRootEquation2nd(double a, double b, double c, out double x_plus, out double x_minus)
        {
            x_plus = x_minus = 0;
            double aa = b * b - 4 * a * c;
            if (aa < 0) return false;
            x_plus = (-b + Math.Sqrt(aa)) / (2 * a);
            x_minus = (-b - Math.Sqrt(aa)) / (2 * a);
            return true;
        }
        #endregion

        #region Nonlinear Calibration

        public bool CalcNonlinearCalibration()
        {
            CalData.LinearRobotPoints = new List<List<Point2d>>();
            CalData.RobotPoints_X = new List<List<Point2d>>();
            CalData.PixelPoints_X = new List<List<Point2d>>();
            CalData.RobotPoints_Y = new List<List<Point2d>>();
            //CalData.PixelPoints_Y = new List<List<Point2d>>();
            CalData.Cooeff_X = new List<List<double>>();
            CalData.Cooeff_Y = new List<List<double>>();

            try
            {
                Point2d start = new Point2d(double.MinValue, double.MinValue);
                int v = 0;
                int h = 0;

                for (int i = 0; i < mCalData.RobotPoint.Length; i++)
                {
                    Point2d point = mCalData.RobotPoint[i];
                    Point2d pixelP = mCalData.RefPoint[i];
                    Point2d robotP = mCalData.RobotPoint[i];

                    // 만약 Y값이 변화가 발생하면...
                    if (point.Y != start.Y)
                    {
                        start = mCalData.RobotPoint[i];
                        CalData.PixelPoints_X.Add(new List<Point2d>());
                        CalData.RobotPoints_X.Add(new List<Point2d>());
                        CalData.LinearRobotPoints.Add(new List<Point2d>());
                        v = CalData.PixelPoints_X.Count - 1;
                    }

                    CalData.LinearRobotPoints[v].Add(robotP);
                    //CalData.PixelPoints_X[v].Add(pixelP);
                    CalData.RobotPoints_X[v].Add(VtoR_Linear(pixelP));

                    // 만약 X값이 변화가 발생하면...
                    h = CalData.LinearRobotPoints[0].FindIndex(x => x.X == point.X);
                    if (v == 0 || h == -1)
                    {
                        //CalData.PixelPoints_Y.Add(new List<Point2d>());
                        CalData.RobotPoints_Y.Add(new List<Point2d>());
                    }
                    CalData.RobotPoints_Y[h].Add(VtoR_Linear(pixelP));
                }

                for (v = 0; v < CalData.RobotPoints_X.Count; v++)
                    CalData.Cooeff_X.Add(GetCooefficient(CalData.RobotPoints_X[v], v));

                for (h = 0; h < CalData.RobotPoints_Y.Count; h++)
                    CalData.Cooeff_Y.Add(GetCooefficient(CalData.RobotPoints_Y[h], h, true));

                if (CalData.RobotPoints_X.Count < 3 || CalData.RobotPoints_Y.Count < 3)
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        List<double> GetCooefficient(List<Point2d> _robotPs, int index, bool _isY = false, int degree = 3)
        {
            degree = CalData.CalDegree;
            ////////////////////////////////////////////////
            // [ 1 x0 x0^2 x0^3 ][c0] = [y0]
            // [ 1 x1 x1^2 x1^3 ][c1] = [y1]
            // [ 1 x2 x2^2 x2^3 ][c2] = [y2]
            // [ 1 x3 x3^2 x3^3 ][c3] = [y3]
            // [ 1 x4 x4^2 x4^3 ]     = [y4]
            // [ 1 x5 x5^2 x5^3 ]     = [y5]
            //         X         C      Y
            //////////////////////////////////////////////////

            List<double> coeff = new List<double>();

            // 끝부분이 너무 발산하는 것을 막기위해...
            double addPrate = (1 - CalData.UsingArea) / 2;
            Point2d addStart;
            Point2d addEnd;

            if (!_isY)
            {
                //////////////////////////////////////////////////
                // k(p1.X - p0.X) + p0.X = p.x
                // k = (p.x - p0.X) / (p1.X - p0.X)
                // p.y = k(p1.Y - p0.Y) + p0.Y
                //////////////////////////////////////////////////
                double robotY = CalData.LinearRobotPoints[index][0].Y;
                double robotX0 = CalData.LinearRobotPoints[index][0].X;
                double robotX1 = CalData.LinearRobotPoints[index][_robotPs.Count - 1].X;

                Point2d p0 = RtoV_Linear(new Point2d(robotX0, robotY));
                Point2d p1 = RtoV_Linear(new Point2d(robotX1, robotY));
                double r0 = addPrate * CalData.Width;
                double k0 = (r0 - p0.X) / (p1.X - p0.X);
                addStart = VtoR_Linear(new Point2d(r0, k0 * (p1.Y - p0.Y) + p0.Y));
                double r1 = (1 - addPrate) * CalData.Width;
                double k1 = (r1 - p0.X) / (p1.X - p0.X);
                addEnd = VtoR_Linear(new Point2d(r1, k1 * (p1.Y - p0.Y) + p0.Y));
            }
            else
            {
                double robotX = CalData.LinearRobotPoints[0][index].X;
                double robotY0 = CalData.LinearRobotPoints[0][index].Y;
                double robotY1 = CalData.LinearRobotPoints[_robotPs.Count - 1][index].Y;

                Point2d p0 = RtoV_Linear(new Point2d(robotX, robotY0));
                Point2d p1 = RtoV_Linear(new Point2d(robotX, robotY1));
                double r0 = addPrate * CalData.Height;
                double k0 = (r0 - p0.Y) / (p1.Y - p0.Y);
                addStart = VtoR_Linear(new Point2d(k0 * (p1.X - p0.X) + p0.X, r0));
                double r1 = (1 - addPrate) * CalData.Height;
                double k1 = (r1 - p0.Y) / (p1.Y - p0.Y);
                addEnd = VtoR_Linear(new Point2d(k1 * (p1.X - p0.X) + p0.X, r1));
            }

            _robotPs.Add(addStart);
            _robotPs.Add(addEnd);

            Mat C = new Mat(degree + 1, 1, MatType.CV_64FC1);
            Mat X = new Mat(_robotPs.Count, degree + 1, MatType.CV_64FC1);
            Mat Y = new Mat(_robotPs.Count, 1, MatType.CV_64FC1);

            for (int i = 0; i < _robotPs.Count; i++)
            {
                for (int j = 0; j <= degree; j++)
                {
                    if (!_isY)
                        X.Set<double>(i, j, Math.Pow(_robotPs[i].X, j));
                    else
                        X.Set<double>(i, j, Math.Pow(_robotPs[i].Y, j));
                }
                if (!_isY)
                    Y.Set<double>(i, _robotPs[i].Y);
                else
                    Y.Set<double>(i, _robotPs[i].X);
            }

            C = (X.Transpose() * X).Inv() * (X.Transpose() * Y);

            for (int i = 0; i < C.Rows; i++)
            {
                coeff.Add(C.Get<double>(i));
            }

            _robotPs.RemoveAt(_robotPs.Count - 1);
            _robotPs.RemoveAt(_robotPs.Count - 1);

            C.Dispose();
            X.Dispose();
            Y.Dispose();

            return coeff;
        }

        public Point2d VtoR(Point2d _visionP)
        {
            Point2d robot = new Point2d();

            if (CalData.IsPossibleNonlinear && CalData.CalDegree > 1)
                robot = GetNonlinearValue(VtoR_Linear(_visionP));
            else
                robot = VtoR_Linear(_visionP);
            //Point2d vision = RtoV_Nonlinear(robot);
            //Point2d diff = vision - _visionP;
            return robot;
        }

        public Point2d RtoV(Point2d _robotP)
        {
            Point2d vision = new Point2d();
            if (CalData.IsPossibleNonlinear && CalData.CalDegree > 1)
                vision = RtoV_Linear(GetLinearValue(_robotP));
            else
                vision = RtoV_Linear(_robotP);
            return vision;
        }

        public Point2d GetLinearValue(Point2d _nonlinearRobotP)
        {
            int xi, yi;
            GetIndex(_nonlinearRobotP, out xi, out yi);
            ///////////////////////////////////////
            // Y = (1 - k)Y1 + kY2
            // Y - Y1 = k(Y2 - Y1)
            // k = (Y - Y1) / (Y2 - Y1)
            // 
            // Y = k(Y1 - Y2) + Y2
            ///////////////////////////////////////

            try
            {
                // 만약 마지막이 넘어가면 한단계 아래걸로 유추... 이게 맞나 ㅡㅡ;
                if (xi == 0) xi = 1;
                if (yi == 0) yi = 1;
                if (xi > CalData.Cooeff_X.Count - 1) xi = CalData.Cooeff_X.Count - 1;
                if (yi > CalData.Cooeff_Y.Count - 1) yi = CalData.Cooeff_Y.Count - 1;

                Point2d _robotbaseP = CalData.LinearRobotPoints[xi - 1][yi - 1];

                double diffY = CalData.LinearRobotPoints[xi][yi].Y - CalData.LinearRobotPoints[xi - 1][yi].Y;
                double Ky = (_nonlinearRobotP.Y - CalData.LinearRobotPoints[xi - 1][yi].Y) / diffY;
                double Y1 = GetPolynominalValue(CalData.Cooeff_X[xi - 1], _nonlinearRobotP.X);
                double Y2 = GetPolynominalValue(CalData.Cooeff_X[xi], _nonlinearRobotP.X);
                double Y = Y1 + (Y2 - Y1) * Ky;

                double diffX = CalData.LinearRobotPoints[xi][yi].X - CalData.LinearRobotPoints[xi][yi - 1].X;
                double Kx = (_nonlinearRobotP.X - CalData.LinearRobotPoints[xi][yi - 1].X) / diffX;
                double X1 = GetPolynominalValue(CalData.Cooeff_Y[yi - 1], _nonlinearRobotP.Y);
                double X2 = GetPolynominalValue(CalData.Cooeff_Y[yi], _nonlinearRobotP.Y);
                double X = X1 + (X2 - X1) * Kx;

                return new Point2d(X, Y);
            }
            catch
            {
                return _nonlinearRobotP;
            }
        }

        public Point2d GetNonlinearValue(Point2d _robotP)
        {
            int xi, yi;
            GetIndex(_robotP, out xi, out yi);
            ///////////////////////////////////////
            // Y = (1 - k)Y1 + kY2
            // Y - Y1 = k(Y2 - Y1)
            // k = (Y - Y1) / (Y2 - Y1)
            // 
            // Y = k(Y1 - Y2) + Y2
            ///////////////////////////////////////

            // 만약 마지막이 넘어가면 한단계 아래걸로 유추... 이게 맞나 ㅡㅡ;
            try
            {
                if (xi == 0) xi = 1;
                if (yi == 0) yi = 1;
                if (xi > CalData.Cooeff_X.Count - 1) xi = CalData.Cooeff_X.Count - 1;
                if (yi > CalData.Cooeff_Y.Count - 1) yi = CalData.Cooeff_Y.Count - 1;

                Point2d _robotbaseP = CalData.LinearRobotPoints[xi - 1][yi - 1];

                double Y1 = GetPolynominalValue(CalData.Cooeff_X[xi - 1], _robotP.X);
                double Y2 = GetPolynominalValue(CalData.Cooeff_X[xi], _robotP.X);
                double Ky = (_robotP.Y - Y1) / (Y2 - Y1);
                double diffY = CalData.LinearRobotPoints[xi][yi].Y - CalData.LinearRobotPoints[xi - 1][yi].Y;
                double Y = _robotbaseP.Y + diffY * Ky;

                double X1 = GetPolynominalValue(CalData.Cooeff_Y[yi - 1], _robotP.Y);
                double X2 = GetPolynominalValue(CalData.Cooeff_Y[yi], _robotP.Y);
                double Kx = (_robotP.X - X1) / (X2 - X1);
                double diffX = CalData.LinearRobotPoints[xi][yi].X - CalData.LinearRobotPoints[xi][yi - 1].X;
                double X = _robotbaseP.X + diffX * Kx;

                return new Point2d(X, Y);
            }
            catch
            {
                return _robotP;
            }
        }

        void GetIndex(Point2d _robotP, out int _index_X, out int _index_Y)
        {
            // X index
            for (_index_X = 0; _index_X < CalData.Cooeff_X.Count; _index_X++)
            {
                double y = GetPolynominalValue(CalData.Cooeff_X[_index_X], _robotP.X);

                if (y > _robotP.Y) break;
            }

            // Y index
            for (_index_Y = 0; _index_Y < CalData.Cooeff_Y.Count; _index_Y++)
            {
                double x = GetPolynominalValue(CalData.Cooeff_Y[_index_Y], _robotP.Y);

                if (x > _robotP.X) break;
            }
        }

        public double GetPolynominalValue(List<double> _coeff, double _x)
        {
            double y = 0;
            for (int j = 0; j < _coeff.Count; j++)
            {
                y += _coeff[j] * Math.Pow(_x, j);
            }
            return y;
        }
        #endregion

    }
}
