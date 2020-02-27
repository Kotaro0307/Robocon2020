using System;

using Constant;
using Basic;

namespace Robot
{
    class PID
    {
        private static int[,] e = new int[2, 3] { { 0, 0, 0 }, { 0, 0, 0 } };
        private static int[] sensor_val_log = new int[2] { 10000, 10000 };

        public PID()
        {
            //初期化関数
        }

        public void Init()
        {
            //初期化関数
            for (int i = 0; i < 2; i++)
            {
                e[i, 0] = 0;
                e[i, 1] = 0;
                e[i, 2] = 0;
                sensor_val_log[i] = 10000;
            }
        }

        public int[] Correction(int[] sensor_val, int[] target_val, int[] lrf_angle, int[] mode, int max, int min)
        {
            //PID制御
            double[] m = new double[2];
            int speed;
            int angle;

            for (int i = 0; i < 2; i++)
            {
                if (sensor_val[i] != Flag.PID_NOTUSE)
                {
                    //更新
                    e[i, 2] = e[i, 1];
                    e[i, 1] = e[i, 0];

                    if (mode[i] == Flag.PID_MODE_PLUSE)
                    {
                        //プラス方向
                        //target_val[i] < sensorval[i]の状態
                        e[i, 0] = sensor_val[i] - target_val[i];
                    }
                    else if (mode[i] == Flag.PID_MODE_MINUS)
                    {
                        //マイナス方向
                        //target_val[i] > sensorval[i]の状態
                        e[i, 0] = target_val[i] - sensor_val[i];
                    }

                    //計算
                    m[i] = Flag.PID_KP * (e[i, 0] - e[i, 1]) + Flag.PID_KI * e[i, 0] + Flag.PID_KD * ((e[i, 0] - e[i, 1]) - (e[i, 1] - e[i, 2]));
                    //更新
                    sensor_val_log[i] = sensor_val[i];
                }
                else
                {
                    //使用しない
                    m[i] = 0;
                }
            }
            if (lrf_angle[1] == -90)
            {
                m[1] *= -1;
            }
            //角度の計算
            angle = (int)Unit.RadianToDegree(Math.Atan2(m[1], m[0]));
            //速度の計算
            speed = Unit.Math_limit((int)Math.Abs(m[0] + m[1]), max, min);

            //値の返却
            return (new int[2] { speed, angle });
        }
    }
}
