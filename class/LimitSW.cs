using System.Threading.Tasks;
using System.Collections.Generic;

using Basic;
using Constant;

namespace Module
{
    class LimitSW : Module
    {
        //ジャイロセンサのクラス
        public List<bool> sw = new List<bool>();

        public LimitSW()
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
                Task task = new Task(GetLimitData);
                task.Start();
            }
        }

        private void GetLimitData()
        {
            //リミットスイッチのデータ受信
            while (true)
            {
                //先頭文字列を取得
                while (port.GetSerialStats().ReadChar() != '!') ;
                //メインデータを取得
                string receiveData = port.GetSerialStats().ReadLine();
                //文字列をboolに変換し、代入
                sw = Unit.StrToBool(Unit.GetReceiveData(Unit.DeleteString(receiveData)));
            }
        }
    }
}