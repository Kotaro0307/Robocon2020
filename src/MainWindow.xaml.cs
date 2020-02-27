using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;

using Constant;

namespace AutoRobot_Main
{
    public partial class MainWindow : Window
    {
        //USB機器
        private Device.USB usb = new Device.USB();
        //ディスプレイ
        private Module.Display display = new Module.Display();
        //モータドライバ
        private Module.Motor motorDriver = new Module.Motor();
        //サーボモータ
        private Module.Servo servo = new Module.Servo();
        //ジャイロセンサ
        private Module.Gyro gyro = new Module.Gyro();
        //リミットスイッチ
        private Module.LimitSW limitSW = new Module.LimitSW();
        //前方LRF
        private Module.LRF lrfFront = new Module.LRF();
        //後方LRF
        private Module.LRF lrfBack = new Module.LRF();
        //ロボットのステータス
        private Robot.Stats natsume = new Robot.Stats();
        //ロボットの動き
        private Robot.Operation move = new Robot.Operation();
        //表示
        private Front.View view = new Front.View();
        //表示Windowの状態
        private bool windowStats = false;
        //メインの動きの状態
        private bool mainStats = false;
        //試合
        private bool match = Flag.MATCH_QUALI;
        //ゾーン
        private bool zone = Flag.ZONE_RED;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            //起動ボタンが押された
            //ステータス：起動
            natsume.message = Flag.ROBOT_SETUP;

            //LRFの起動
            lrfFront.Open();
            lrfBack.Open();

            if (!windowStats)
            {
                //USB機器の監視
                Task deviceProc = new Task(Proc_Device);
                deviceProc.Start();
                //描画用Windowの起動
                Task viewProc = new Task(Proc_View);
                viewProc.Start();
                Task moveProc = new Task(Proc_Move);
                moveProc.Start();
                //起動済み
                windowStats = true;
                mainStats = true;
            }

            //ボタンの切り替え
            StartButton.IsEnabled = false;
            EndButton.IsEnabled = true;
        }
        private void EndButton_Click(object sender, RoutedEventArgs e)
        {
            //終了ボタンが押された
            mainStats = false;
            //ステータス：停止
            natsume.message = Flag.ROBOT_FINISH;

            //ロボットの停止
            move.Stop(ref motorDriver, ref natsume);

            //ボタンの切り替え
            StartButton.IsEnabled = true;
            EndButton.IsEnabled = false;
        }
        protected virtual void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Windowの閉じるボタンが押された
            mainStats = false;
            //ロボットの停止
            move.Stop(ref motorDriver, ref natsume);

            //LRFの停止
            lrfFront.Close();
            lrfBack.Close();
        }

        private void Proc_View()
        {
            //表示系
            while (true)
            {
                view.ViewStats(zone, match, display, motorDriver, servo, gyro, limitSW, lrfFront, lrfBack, natsume);
            }
        }

        private void Proc_Move()
        {
            //ロボットのメインの処理
            Robot.PID pid = new Robot.PID();
            pid.Init();

            int progress = Flag.PROGRESS_SETUP;
            
            while (mainStats)
            {
                if (display.RobotReset())
                {
                    //リセット
                    progress = Flag.PROGRESS_SETUP;
                    //ステータス：起動
                    natsume.message = Flag.ROBOT_SETUP;
                    //足回りの停止
                    move.Stop(ref motorDriver, ref natsume);
                    pid.Init();
                }

                switch(progress)
                {
                    case Flag.PROGRESS_SETUP:
                        //スタートボタン待ち
                        if (display.RobotStart())
                        {
                            //スタート
                            progress = Flag.PROGRESS_MOVE;
                            //ステータス：移動中
                            natsume.message = Flag.ROBOT_MOVE;

                            //ゾーンの設定
                            zone = display.NowZone();
                            //試合の決定
                            match = display.NowMatch();
                        }
                        break;
                    case Flag.PROGRESS_MOVE:
                        //行動中
                        progress = Flag.PROGRESS_END;
                        break;
                    case Flag.PROGRESS_END:
                        //終了
                        //ステータス：終了
                        natsume.message = Flag.ROBOT_FINISH;
                        //足回りの停止
                        move.Stop(ref motorDriver, ref natsume);
                        break;
                    default:
                        //エラー
                        //ステータス：エラー
                        natsume.message = Flag.ROBOT_ERROR;
                        //足回りの停止
                        move.Stop(ref motorDriver, ref natsume);
                        break;
                }
            }
        }

        private void Proc_Device()
        {
            //USBの自動接続
            List<string> sensorKind = new List<string>();
            List<string> sensorPortNo = new List<string>();

            while (true)
            {
                int usbCode = usb.CheckUSB(ref sensorPortNo, ref sensorKind);
                if (usbCode != Flag.USB_NONE)
                {
                    if (usbCode == Flag.USB_CONNECT)
                    {
                        //接続
                        for (int i = 0; i < sensorKind.Count; i++)
                        {
                            switch (sensorKind[i])
                            {
                                case Flag.KIND_GYRO:
                                    //ジャイロセンサ
                                    gyro.SetPortNo(sensorPortNo[i]);
                                    gyro.Open();
                                    gyro.StartDataReceive();
                                    break;
                                case Flag.KIND_MOTOR:
                                    //モータドライバ
                                    motorDriver.SetPortNo(sensorPortNo[i]);
                                    motorDriver.Open();
                                    break;
                                case Flag.KIND_SERVO:
                                    //サーボモータ
                                    servo.SetPortNo(sensorPortNo[i]);
                                    servo.Open();
                                    break;
                                case Flag.KIND_LIMIT:
                                    //リミットスイッチ
                                    limitSW.SetPortNo(sensorPortNo[i]);
                                    limitSW.Open();
                                    limitSW.StartDataReceive();
                                    break;
                                case Flag.KIND_DISPLAY:
                                    //ディスプレイ基板
                                    display.SetPortNo(sensorPortNo[i]);
                                    display.Open();
                                    display.StartDataReceive();
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void Port_LRF_Front_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!mainStats)
            {
                //メインの処理が動作していないときのみ実行する
                
            }
        }

        private void Port_LRF_Back_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!mainStats)
            {
                //メインの処理が動作していないときのみ実行する
            }
        }
    }
}
