namespace Constant
{
    class Flag
    {
        //読み取り専用の定数クラス

        /*--------Robot--------*/
        public const string ROBOT_SETUP         = "SetUp";  //起動待ち
        public const string ROBOT_MOVE          = "Move";   //起動中
        public const string ROBOT_FINISH        = "Finish"; //終了
        public const string ROBOT_ERROR         = "Error!"; //終了

        public const int PROGRESS_SETUP         = 0;        //起動準備
        public const int PROGRESS_MOVE          = 1;        //移動
        public const int PROGRESS_END           = 2;        //終了

        public const double ROBOT_GYRO_SPEED    = 1.5;      //ジャイロの補正値

        /*--------PID--------*/
        public const double PID_KP      = 0.1;  //Pゲイン
        public const double PID_KI      = 0.04; //Iゲイン
        public const double PID_KD      = 0.1;  //Dゲイン

        public const int PID_MODE_PLUSE = 0;    //プラス方向
        public const int PID_MODE_MINUS = 1;    //マイナス方向 
        public const int PID_NOTUSE     = 0;    //使用しない

        /*--------Display基板--------*/
        public const string KIND_DISPLAY        = "DSP";    //ディスプレイ基板の識別番号

        public const int DISPLAY_ROBOT_START    = 0;        //ロボットのスタートボタン
        public const int DISPLAY_ROBOT_RESET    = 1;        //ロボットのリセットボタン
        public const int DISPLAY_SWITCH_ZONE    = 2;        //ゾーンの切り替えレバー
        public const int DISPLAY_SWITCH_MATCH   = 3;        //試合の切り替えレバー

        public const bool ZONE_RED              = true;     //青ゾーン
        public const bool ZONE_BLUE             = false;    //赤ゾーン

        public const bool MATCH_FINAL           = true;     //決勝トーナメント
        public const bool MATCH_QUALI           = false;    //予選リーグ

        /*--------Motor--------*/
        public const string KIND_MOTOR  = "MOT";    //モータドライバの識別番号

        public const int MOTOR_NO       = 10;       //モータの数
        public const int WHEEL          = 4;        //足回りのタイヤの数
        public const int MAX_MOTORSPEED = 60;       //モータの速度の限界値

        public const int DRIVER_ID      = 111;      //ドライバID
        private const int MOTOR_LEG1    = 0;        //足回り１
        private const int MOTOR_LEG2    = 1;        //足回り２
        private const int MOTOR_LEG3    = 2;        //足回り３
        private const int MOTOR_LEG4    = 3;        //足回り４
        //足回りのモータ配列
        public static readonly int[] LEG_MOTOR = new int[WHEEL]
        {
            MOTOR_LEG1,
            MOTOR_LEG2,
            MOTOR_LEG3,
            MOTOR_LEG4
        };

        /*--------Servo--------*/
        public const string KIND_SERVO = "SRV";    //サーボモータの識別番号

        /*--------Gyro--------*/
        public const string KIND_GYRO = "GYR";    //ジャイロセンサの識別番号
        
        /*--------Limit--------*/
        public const string KIND_LIMIT  = "LIM";    //リミットスイッチの識別番号

        public const bool SWITCH_ON     = true;     //スイッチオン
        public const bool SWITCH_OFF    = false;    //スイッチオフ

        /*--------LRF--------*/
        //関数の返り値
        public const int LRF_SUCCESS        = 0;        //成功
        public const int LRF_NOTOPEN        = 1;        //開くことができなかった
        public const int LRF_NOTGETDISTANCE = 2;        //距離を取得することができなかった

        /*--------Module全般--------*/
        public const string PORT_MSG_OPEN       = "Opened";     //ポート開いた
        public const string PORT_MSG_NOTOPEN    = "NotOpened";  //ポート開いてない
        public const string PORT_MSG_CLOSE      = "Closed";     //ポート閉じた
        public const string PORT_MSG_SUCCESS    = "Success";    //正常
        public const string PORT_MSG_FAILD      = "Faild";      //異常
        //センサーの種類
        public static readonly string[] SENSOR_KIND =
        {
            KIND_MOTOR,     //モータ
            KIND_SERVO,     //サーボモータ
            KIND_GYRO,      //ジャイロ
            KIND_DISPLAY    //ディスプレイ
        };

        /*--------USB--------*/
        public const int USB_CONNECT            = 1;        //何かしらのUSBが刺さった
        public const int USB_NONE               = 0;        //何も起こっていない
        public const string PORT_NOT_SETTING    = "NONE";   //ポートが未設定
    }
}
