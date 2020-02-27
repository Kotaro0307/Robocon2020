using System;
using System.Linq;

using Basic;
using Constant;
using Module;

namespace Robot
{
    class Operation
    {
        public void SetUp(Motor motorDriver, ref Stats parts)
        {
            if (motorDriver != null)
            {
                if (motorDriver.message == Flag.PORT_MSG_OPEN)
                {
                    //データ送信
                    //停止
                    Stop(ref motorDriver, ref parts);
                }
            }
        }

        public void Move(ref Motor motorDriver, ref Stats parts, int speed, int angle, double goal_yaw, double now_yaw)
        {
            //足回りのデータ
            int[] legMotor = new int[Flag.WHEEL];

            //ロボットの移動方向の決定
            double robot_yaw = goal_yaw - now_yaw;

            if (goal_yaw >= 150)
            {
                if (now_yaw < 0)
                {
                    robot_yaw = goal_yaw - (360 + now_yaw);
                }
            }

            for (int i = 0; i < Flag.WHEEL; i++)
            {
                //タイヤの角度
                double wheelAngle = (Math.PI / Flag.WHEEL) * (i * 2 + 1);
                //各タイヤのモータに値を代入
                legMotor[i] = (int)Math.Round(Math.Sin(Unit.DegreeToRadian(angle+180) - wheelAngle) * speed - robot_yaw);
                
                if (Math.Abs(legMotor[i]) >= Flag.MAX_MOTORSPEED)
                {
                    //タイヤの速度が上限値を超えた
                    for (int j = 0; j < Flag.WHEEL; j++)
                    {
                        //停止
                        motorDriver.motorData[Flag.LEG_MOTOR[j]] = 0;
                    }
                    //エラー
                    parts.message = Flag.ROBOT_ERROR;
                }
            }

            //データ送信
            if (motorDriver != null)
            {
                //値の更新
                parts.speed = speed;
                parts.angle = angle;
                //モータのデータの代入
                for (int i = 0; i < Flag.WHEEL; i++)
                {
                    motorDriver.motorData[Flag.LEG_MOTOR[i]] = legMotor[i];
                }

                if (motorDriver.message == Flag.PORT_MSG_OPEN)
                {
                    //送信
                    motorDriver.Send();
                }
            }
        }
        public void Turn(ref Motor motorDriver, ref Stats parts, int speed)
        {
            //ロボットの旋回
            //データ送信
            if (motorDriver != null)
            {
                parts.speed = speed;
                parts.angle = 0;

                //モータのデータの代入
                for (int i = 0; i < Flag.WHEEL; i++)
                {
                    motorDriver.motorData[Flag.LEG_MOTOR[i]] = speed;
                }

                if (motorDriver.message == Flag.PORT_MSG_OPEN)
                {
                    //送信
                    motorDriver.Send();
                }
            }
        }
        public void Stop(ref Motor motorDriver, ref Stats parts)
        {
            //ロボット停止させる。
            if (motorDriver != null)
            {
                parts.speed = 0;
                parts.angle = 0;

                //モータのデータの代入
                for (int i = 0; i < 10; i++)
                {
                    motorDriver.motorData[i] = 0;
                }

                if (motorDriver.message == Flag.PORT_MSG_OPEN)
                {
                    //データ送信
                    motorDriver.Send();
                }
            }
        }
    }
}