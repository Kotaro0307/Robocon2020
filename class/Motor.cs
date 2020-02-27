using System;

using Constant;
using Basic;

namespace Module
{
    class Motor : Module
    {
        //モータに関するクラス
        //モータの回転が保存されている変数
        public int[] motorData = new int[Flag.MOTOR_NO];

        public Motor()
        {
            //初期化
            for (int i = 0; i < Flag.MOTOR_NO; i++)
            {
                //モータのデータの初期化
                motorData[i] = 0;
            }
        }

        public void MotorTurn(int no, int pwm)
        {
            //モータの回転
            for (int i = 0; i < Flag.MOTOR_NO; i++)
            {
                if (i == no)
                {
                    //回したいモータ
                    motorData[i] = pwm;
                }
                else
                {
                    //それ以外のモータ
                    motorData[i] = 0;
                }
            }
            //データ送信
            Send();
        }

        public void Send()
        {
            char[] motordata_tochar = new char[Flag.MOTOR_NO * 2 + 1];

            //IDの設定
            motordata_tochar[0] = (char)Flag.DRIVER_ID;

            for (int i = 0; i < Flag.MOTOR_NO; i++)
            {
                //例外処理（数大きいよ）
                if (motorData[i] >= 100)
                {
                    motorData[i] = 100;
                }
                else if (motorData[i] <= -100)
                {
                    motorData[i] = -100;
                }
                //出力を絶対値変換
                motordata_tochar[i + 1] = (char)Math.Abs(motorData[i]);
                //符号の取得
                if (motorData[i] >= 0)
                {
                    //正の数
                    motordata_tochar[i + Flag.MOTOR_NO + 1] = (char)1;
                }
                else
                {
                    //負の数
                    motordata_tochar[i + Flag.MOTOR_NO + 1] = (char)0;
                }
            }
            if (port.GetSerialStats().IsOpen)
            {
                //データ送信
                port.GetSerialStats().Write(motordata_tochar, 0, motordata_tochar.Length);
            }
        }
    }
}
