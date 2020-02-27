using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Constant;
using Basic;

namespace Module
{
    class Display : Module
    {
        private bool[] sw = new bool[4] { false, false, false, false };
        
        public Display()
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
                Task task = new Task(GetDispSwData);
                task.Start();
            }
        }

        public bool RobotStart()
        {
            //ロボットがスタートするかどうかの結果を返す
            return (sw[Flag.DISPLAY_ROBOT_START]);
        }

        public bool RobotReset()
        {
            //ロボットがリセットするかどうかの結果を返す
            return (sw[Flag.DISPLAY_ROBOT_RESET]);
        }

        public bool NowZone()
        {
            //現在のゾーン
            return (sw[Flag.DISPLAY_SWITCH_ZONE]);
        }

        public bool NowMatch()
        {
            //現在のゾーン
            return (sw[Flag.DISPLAY_SWITCH_MATCH]);
        }

        private void GetDispSwData()
        {
            //ディスプレイ基板のスイッチのデータ受信
            try
            {
                while (true)
                {
                    //先頭文字列を取得
                    while (port.GetSerialStats().ReadChar() != '!') ;
                    //メインデータを取得
                    string receiveData = port.GetSerialStats().ReadLine();
                    //文字列をboolに変換し、代入
                    List<bool> receivedData = Unit.StrToBool(Unit.GetReceiveData(Unit.DeleteString(receiveData)));
                    if (receivedData.Count == 4)
                    {
                        //正常に値が来た
                        sw = receivedData.ToArray();
                        message = Flag.PORT_MSG_SUCCESS;
                    }
                    else
                    {
                        //異常
                        sw = new bool[4] { false, false, false, false };
                        message = Flag.PORT_MSG_FAILD;
                    }
                }
            }
            catch (System.IO.IOException)
            {
                //ポートが抜かれた
                message = Flag.PORT_MSG_NOTOPEN;
                portNo = "NULL";
            }
        }
    }
}