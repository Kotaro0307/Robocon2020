using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Constant;

namespace AutoRobot_Main
{
    class Command
    {
        public static bool Move(string line)
        {
            //移動コマンド
            return (Flag.COMMAND_NOERROR);
        }
        public static bool Turn(string line)
        {
            //回転コマンド
            return (Flag.COMMAND_NOERROR);
        }
        public static bool Stop(string line)
        {
            //停止コマンド
            return (Flag.COMMAND_NOERROR);
        }
        public static bool Start(string line)
        {
            //停止コマンド
            return (Flag.COMMAND_NOERROR);
        }
        public static bool Delete(string line)
        {
            //停止コマンド
            return (Flag.COMMAND_NOERROR);
        }
    }
}
