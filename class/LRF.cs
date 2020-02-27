using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using Constant;

namespace Module
{
    class LRF
    {
        //LRFの関数
        [DllImport("LRF.dll")] private static extern int GetDistance(int mode, int argc, string[] argv, string urg_port, ref long deg0Len, ref long deg90Len, ref long degM90Len);
        [DllImport("LRF.dll")] private static extern void Close(int mode);

        public string message = Flag.PORT_MSG_NOTOPEN;
        public string portNo = Flag.PORT_NOT_SETTING;

        private long deg0   = 7000;
        private long deg90  = 7000;
        private long degM90 = 7000;

        private bool openflag = false;
        private int mode = 0;

        public LRF()
        {
            //初期化関数
        }

        public void SetPortNo(string port, int no)
        {
            //ポート番号の設定
            portNo = port;
            mode = no;
        }

        public void Open()
        {
            //LRFの起動
            Basic.Serial port = new Basic.Serial(portNo);
            //開放
            port.Open();

            //ポート接続されている
            if (port.GetSerialStats().IsOpen)
            {
                //閉鎖
                port.Close();

                if (!openflag)
                {
                    //LRFのデータ取得
                    Task task = new Task(Proc);
                    task.Start();
                }
            }
            //閉鎖
            port.Close();
        }
        public void Close()
        {
            //LRFの停止
            if (openflag)
            {
                //終了
                message = "Closed";
                //コマンドライン引数の取得
                Close(mode);
            }
        }

        public int GetLen(int angle)
        {
            //角度を渡す
            switch(angle)
            {
                case 0:
                    //0度
                    return ((int)deg0);
                case 90:
                    //90度
                    return ((int)deg90);
                case -90:
                    //-90度
                    return ((int)degM90);
                default:
                    //エラー
                    return(7000);
            }
        }

        private void Proc()
        {
            //起動
            openflag = true;
            message = "Opened";

            //コマンドライン引数の取得
            string[] args = Environment.GetCommandLineArgs();
            int ans = GetDistance(mode, args.Length, args, portNo, ref deg0, ref deg90, ref degM90);

            if (ans == Flag.LRF_NOTGETDISTANCE)
            {
                //距離の取得の失敗
                message = "Not Get Distance";
            }
            else if (ans == Flag.LRF_NOTOPEN)
            {
                //接続されていない
                message = "Not Connect";
            }
            openflag = false;
        }
    }
}
