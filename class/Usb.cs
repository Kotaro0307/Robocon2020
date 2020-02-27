using System.Collections.Generic;

using Constant;

namespace Device
{
    class USB
    {
        //現在接続されているUSBの数
        private int usbNo = 0;
        //現在の接続されているポート番号
        private string[] portNo;
        
        public USB()
        {
            //初期化
        }

        public int CheckUSB(ref List<string> sensorPortNo, ref List<string> sensorKind)
        {
            //デバイスの監視
            portNo = System.IO.Ports.SerialPort.GetPortNames();

            if (usbNo != portNo.Length)
            {
                if (usbNo < portNo.Length)
                {
                    //センサ関係のモジュールのみを抜き出す
                    ConnectPort(portNo, ref sensorPortNo, ref sensorKind);
                    //更新
                    usbNo = portNo.Length;
                    //接続フラグの送信
                    return (Flag.USB_CONNECT);
                }
            }
            //何もなし
            return (Flag.USB_NONE);
        }

        private void ConnectPort(string[] connectPortNo, ref List<string> sensorPortNo, ref List<string> sensorKind)
        {
            //ポートの接続
            //初期化
            sensorKind.Clear();
            sensorPortNo.Clear();

            //シリアルポートの取得
            for (int i = 0; i < connectPortNo.Length; i++)
            {
                //ポートの設定
                Basic.Serial myPort = new Basic.Serial(connectPortNo[i]);

                //ポート開放
                myPort.Open();

                //識別番号のみを切り出す
                char[] kindCode = new char[3];
                while (true)
                {
                    if (myPort.GetSerialStats().ReadChar() == '!')
                    {
                        myPort.GetSerialStats().Read(kindCode, 0, 3);
                        break;
                    }
                }
                //センサーの識別番号
                sensorKind.Add(new string(kindCode));
                //センサのポート番号
                sensorPortNo.Add(connectPortNo[i]);
                //ポート閉鎖
                myPort.Close();
            }
        }
    }
}