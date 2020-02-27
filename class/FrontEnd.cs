using System;

using Constant;
using OpenCvSharp;

namespace Front
{
    class View
    {
        public void ViewStats(bool zone, bool match, Module.Display display, Module.Motor motorDriver, Module.Servo servo, Module.Gyro gyro, Module.LimitSW limitSW, Module.LRF lrfFront, Module.LRF lrfBack, Robot.Stats robot)
        {
            string nowMatch;

            if (match == Flag.MATCH_FINAL)
            {
                //決勝
                nowMatch = "[Final]";
            }
            else
            {
                //予選
                nowMatch = "[Qual]";
            }

            //ロボットの現在の状態を表示
            Mat frame = new Mat(350, 600, MatType.CV_8UC3, new Scalar(255, 255, 255));

            //ロボットの状態
            Cv2.PutText(frame, "Robot : " + robot.message + nowMatch, new OpenCvSharp.Point(0, 30), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 0), 1, LineTypes.AntiAlias);
            //ロボットの移動方向、速度
            Cv2.PutText(frame, "Speed :" + robot.speed + "%, Angle : " + robot.angle + "[deg]", new OpenCvSharp.Point(0, 60), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 0), 1, LineTypes.AntiAlias);
            //ジャイロセンサのデータ
            Cv2.PutText(frame, "Yaw : " + Math.Round(gyro.yaw, 2) + "[deg]", new OpenCvSharp.Point(0, 90), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 0), 1, LineTypes.AntiAlias);
            //リミットスイッチのデータ
            Cv2.PutText(frame, "Limit : " + ViewLimit(limitSW), new OpenCvSharp.Point(0, 120), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 0), 1, LineTypes.AntiAlias);
            
            //LRF右 0度
            Cv2.PutText(frame, "LRF_F[0deg]", new OpenCvSharp.Point(0, 150), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 0), 1, LineTypes.AntiAlias);
            Cv2.PutText(frame, lrfFront.GetLen(0) + "[mm]", new OpenCvSharp.Point(220, 150), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 0), 1, LineTypes.AntiAlias);
            //LRF右 90度
            Cv2.PutText(frame, "LRF_F[90deg]", new OpenCvSharp.Point(0, 180), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 0), 1, LineTypes.AntiAlias);
            Cv2.PutText(frame, lrfFront.GetLen(90) + "[mm]", new OpenCvSharp.Point(220, 180), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 0), 1, LineTypes.AntiAlias);
            //LRF右 -90度
            Cv2.PutText(frame, "LRF_F[-90deg]", new OpenCvSharp.Point(0, 210), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 0), 1, LineTypes.AntiAlias);
            Cv2.PutText(frame, lrfFront.GetLen(-90) + "[mm]", new OpenCvSharp.Point(220, 210), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 0), 1, LineTypes.AntiAlias);
            //LRF左 0度
            Cv2.PutText(frame, "LRF_B[0deg]", new OpenCvSharp.Point(0, 240), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 0), 1, LineTypes.AntiAlias);
            Cv2.PutText(frame, lrfBack.GetLen(0) + "[mm]", new OpenCvSharp.Point(220, 240), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 0), 1, LineTypes.AntiAlias);
            //LRF左 90度
            Cv2.PutText(frame, "LRF_B[90deg]", new OpenCvSharp.Point(0, 270), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 0), 1, LineTypes.AntiAlias);
            Cv2.PutText(frame, lrfBack.GetLen(90) + "[mm]", new OpenCvSharp.Point(220, 270), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 0), 1, LineTypes.AntiAlias);
            //LRF左 -90度
            Cv2.PutText(frame, "LRF_B[-90deg]", new OpenCvSharp.Point(0, 300), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 0), 1, LineTypes.AntiAlias);
            Cv2.PutText(frame, lrfBack.GetLen(-90) + "[mm]", new OpenCvSharp.Point(220, 300), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 0), 1, LineTypes.AntiAlias);
            
            //足回りの描画
            SetRobotModel(frame, zone, motorDriver, new OpenCvSharp.Point(360, 120), 200, 20);

            //画面表示
            Cv2.ImShow("Progress", frame);
            Cv2.MoveWindow("Progress", 750, 0);
            Cv2.ImShow("Stats", ViewConnect(display, motorDriver, servo, gyro, limitSW, lrfFront, lrfBack));
            Cv2.MoveWindow("Stats", 750, frame.Size().Height + 50);

            Cv2.WaitKey(1);
        }

        private string ViewLimit(Module.LimitSW limitSW)
        {
            string str = "";

            for (int i = 0; i < limitSW.sw.Count; i++)
            {
                if (limitSW.sw[i])
                {
                    //1
                    str += "1 ";
                }
                else
                {
                    //0
                    str += "0 ";
                }
            }
            return (str);
        }

        private void SetRobotModel(Mat frame, bool zone, Module.Motor motorDriver, OpenCvSharp.Point robotStart, int robotSize, int wheelSize)
        {
            Scalar wheelColor;
            Scalar robotColor;

            double r;
            double angle;

            //本体色の選択
            if (zone == Flag.ZONE_RED)
            {
                //赤ゾーン
                robotColor = new Scalar(0, 0, 255);
            }
            else
            {
                //青ゾーン
                robotColor = new Scalar(255, 0, 0);
            }
            //本体
            Cv2.Rectangle(frame, robotStart, new OpenCvSharp.Point((robotStart.X + robotSize), (robotStart.Y + robotSize)), robotColor, -1);

            //タイヤ
            for (int i = 0; i < Flag.WHEEL; i++)
            {
                //原点からの長さ
                r = (robotSize / 2) - (wheelSize / 2) - 10;
                //原点からの角度
                angle = (Math.PI / Flag.WHEEL) * (i * 2 + 1) + Basic.Unit.DegreeToRadian(90);

                //ロボットの中心
                OpenCvSharp.Point center = new OpenCvSharp.Point((robotSize / 2) + robotStart.X, (robotSize / 2) + robotStart.Y);

                //タイヤの開始点
                OpenCvSharp.Point start = new OpenCvSharp.Point();
                start.X = (int)(r * Math.Cos(angle)) - (wheelSize / 2) + center.X;
                start.Y = (int)(r * Math.Sin(angle)) - (wheelSize / 2) + center.Y;
                //タイヤの終了点
                OpenCvSharp.Point end = new OpenCvSharp.Point();
                end.X = start.X + wheelSize;
                end.Y = start.Y + wheelSize;

                //タイヤの状態（回転 or 停止）
                if (motorDriver.motorData[Flag.LEG_MOTOR[i]] == 0)
                {
                    //停止
                    wheelColor = new Scalar(0, 0, 0);
                }
                else
                {
                    //回転
                    wheelColor = new Scalar(255, 255, 255);
                }

                //表示
                Cv2.Rectangle(frame, start, end, wheelColor, -1);
            }
        }

        private Mat ViewConnect(Module.Display display, Module.Motor motorDriver,  Module.Servo servo, Module.Gyro gyro, Module.LimitSW limitSW, Module.LRF lrfFront, Module.LRF lrfBack)
        {
            //センサーの接続状況画面

            //センサーのリスト
            string[,] sensorList =
            {
                //名前, ポート番号, 状態
                { "LRF[F]", lrfFront.portNo,    lrfFront.message    },
                { "LRF[B]", lrfBack.portNo,     lrfBack.message     },
                { "Motor",  motorDriver.portNo, motorDriver.message },
                { "Servo",  servo.portNo,       servo.message       },
                { "Gyro",   gyro.portNo,        gyro.message        },
                { "Limit",  limitSW.portNo,     limitSW.message     },
                { "Display",display.portNo,     display.message     }
            };

            //ディスプレイのサイズを作成
            Mat serialStats = new Mat(((sensorList.Length + 3) * 10), 600, MatType.CV_8UC3, new Scalar(255, 255, 255));

            //センサの各情報を表示
            for (int i = 0; i < (sensorList.Length / 3); i++)
            {
                Cv2.PutText(serialStats, sensorList[i, 0], new OpenCvSharp.Point(0, ((i + 1) * 30)), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 0), 1, LineTypes.AntiAlias);
                Cv2.PutText(serialStats, sensorList[i, 1], new OpenCvSharp.Point(100, ((i + 1) * 30)), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 0), 1, LineTypes.AntiAlias);
                Cv2.PutText(serialStats, "[ " + sensorList[i, 2] + " ]", new OpenCvSharp.Point(200, ((i + 1) * 30)), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 0), 1, LineTypes.AntiAlias);
            }

            //結果を返却
            return (serialStats);
        }
    }
}
