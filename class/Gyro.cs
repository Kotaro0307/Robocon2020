using System.Threading.Tasks;

using Basic;
using Constant;

namespace Module
{
    class Gyro : Module
    {
        //ジャイロセンサのクラス
        public double yaw = 0.0;
        
        private const double MAKE_A_ZERO_POINT  = 500.0;
        private const double SET_UP             = 600.0;
        private const double NOT_CONNECT        = 400.0;

        
        public Gyro()
        {
            //初期化関数
        }

        public void StartDataReceive()
        {
            //受信割り込みの開始
            //ポートが開放されている場合
            if (message == Flag.PORT_MSG_OPEN)
            {
                //受信の開始
                Task task = new Task(GetGyroData);
                task.Start();
            }
        }

        private void GetGyroData()
        {
            //ジャイロセンサのデータ受信
            while (true)
            {
                //先頭文字列を取得
                while (port.GetSerialStats().ReadChar() != '!') ;
                //メインデータを取得
                string receiveData = port.GetSerialStats().ReadLine();
                //文字列を小数に変換し、代入
                double gyroData = Unit.StrToDouble(Unit.GetReceiveData(Unit.DeleteString(receiveData)));

                if (gyroData == MAKE_A_ZERO_POINT)
                {
                    //０点合わせ中
                    message = "Make a ZeroPoint";
                }
                else if (gyroData == NOT_CONNECT)
                {
                    //接続できなかった
                    message = "Not Connect";
                }
                else if (gyroData == SET_UP)
                {
                    //起動中
                    message = "Set Up";
                }
                else
                {
                    //起動済み
                    message = "Starting";
                    //角度データを代入
                    yaw = gyroData;
                }
            }
        }
    }
}