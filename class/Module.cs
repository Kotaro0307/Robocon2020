using Basic;
using Constant;

namespace Module
{
    class Module
    {
        //モジュールのステータス
        public string message = Flag.PORT_MSG_NOTOPEN;
        //モジュールのポート番号
        public string portNo = Flag.PORT_NOT_SETTING;
        //モジュールのシリアル通信クラス
        public Serial port = null;

        public Module()
        {
            //初期化関数
        }

        public void SetPortNo(string portNo)
        {
            //ポート番号のセット
            this.portNo = portNo;
            port = new Serial(portNo);
        }

        public void Open()
        {
            //ポートの開放
            if (port != null)
            {
                port.Open();
                message = port.message;
            }
        }

        public void Close()
        {
            //ポートの閉鎖
            if (port != null)
            {
                port.Close();
            }
        }
    }
}
