using System;

using Constant;
using Basic;

namespace Module
{
    class Servo : Module
    {
        //サーボに関するクラス
        public Servo()
        {
            //初期化関数
        }

        public void Send(int[] data)
        {
            //サーボにデータを送信
            char[] servoData = new char[data.Length];

            for (int i = 0; i < servoData.Length; i++)
            {
                servoData[i] = (char)Math.Abs(data[i]);
            }
            //データ送信
            port.Send(servoData);
        }
    }
}
