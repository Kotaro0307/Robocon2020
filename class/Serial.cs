using System.IO.Ports;

using Constant;

namespace Basic
{
    internal class Serial
    {
        //シリアル通信に関するクラス
        public string portNo = "NULL";
        public string message = "NULL";

        private readonly SerialPort myPort = null;

        public Serial(string portNo)
        {
            //ポート名の更新
            this.portNo = portNo;
            //シリアル通信の設定を行う関数
            myPort = new SerialPort(portNo, 460800, Parity.None, 8, StopBits.One);
        }

        public void Open()
        {
            //ポートの開放
            try
            {
                if (!myPort.IsOpen)
                {
                    //ポート開く
                    myPort.Open();
                    //起動済
                    message = Flag.PORT_MSG_OPEN;
                }
            }
            catch (System.IO.IOException)
            {
                //ポートを開くことができなかった
                message = Flag.PORT_MSG_NOTOPEN;
            }
        }

        public void Close()
        {
            //ポートの閉鎖
            if (myPort.IsOpen)
            {
                myPort.Close();
                message = Flag.PORT_MSG_CLOSE;
            }
        }

        public void Send(char[] data)
        {
            myPort.Write(data, 0, data.Length);
        }

        public int Receive()
        {
            return (myPort.ReadChar());
        }

        public SerialPort GetSerialStats()
        {
            //シリアルポートの設定を返す
            return (myPort);
        }
    }
}