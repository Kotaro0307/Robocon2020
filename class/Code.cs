using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text;
using System.Threading.Tasks;

using Constant;

namespace AutoRobot_Main
{
    public struct COMMAND_TABLE
    {
        //コマンド名
        public string word;
        //コマンドの種類
        public int kind;
    }

    class Code
    {
        private string[] source = new string[100];
        private int sourceNo = 0;

        public string SourceArrange(string line)
        {
            //ソースを並べる
            int kind = Flag.COMMAND_UNKNOW;

            //空白なくす
            SkipSpace(ref line);
            //小文字を大文字に変換
            ToUpper(ref line);

            if (GetCmd(ref line, ref kind) == Flag.COMMAND_ERROR)
            {
                //コマンドを取得できなかった
                return ("CMD_ERROR");
            }
            //各コマンドの実行
            if (ExecCommand(line, kind, ref source, ref sourceNo) == Flag.COMMAND_ERROR)
            {
                //コマンドを取得できなかった
                return ("CMD_DO_ERROR");
            }

            return (line);
        }

        private bool GetCmd(ref string line, ref int kind)
        {
            //コマンドの取得
            int i = 0;

            COMMAND_TABLE[] commandList = new COMMAND_TABLE[6];
            //移動コマンド
            commandList[0].word = Flag.COMMAND_MOVE_STR;
            commandList[0].kind = Flag.COMMAND_MOVE;
            //回転コマンド
            commandList[1].word = Flag.COMMAND_TURN_STR;
            commandList[1].kind = Flag.COMMAND_TURN;
            //停止コマンド
            commandList[2].word = Flag.COMMAND_STOP_STR;
            commandList[2].kind = Flag.COMMAND_STOP;
            //実行コマンド
            commandList[3].word = Flag.COMMAND_START_STR;
            commandList[3].kind = Flag.COMMAND_START;
            //削除コマンド
            commandList[4].word = Flag.COMMAND_DELETE_STR;
            commandList[4].kind = Flag.COMMAND_DELETE;
            //コマンドなし
            commandList[5].word = Flag.COMMAND_UNKNOW_STR;
            commandList[5].kind = Flag.COMMAND_UNKNOW;

            while (i != Flag.COMMAND_UNKNOW)
            {
                //コマンド名より文字列の長さが長い
                if (commandList[i].word.Length <= line.Length)
                {
                    //コマンド分の文字列を抜き取る
                    string lineCmd = line.Substring(0, commandList[i].word.Length).ToString().Trim();
                    //比較
                    if (lineCmd == commandList[i].word)
                    {
                        //識別番号のコピー
                        kind = commandList[i].kind;
                        //取得成功
                        return (Flag.COMMAND_NOERROR);
                    }
                }
                //次のコマンドへ移動
                i++;
            }
            //取得失敗
            return (Flag.COMMAND_ERROR);
        }

        private bool ExecCommand(string line, int kind, ref string[] source, ref int sourceNo)
        {
            //各コマンドの実行
            bool error = Flag.COMMAND_NOERROR;

            if (kind == Flag.COMMAND_MOVE)
            {
                //移動コマンド
                error = Command.Move(line.Replace(Flag.COMMAND_MOVE_STR, ""));

                if (error == Flag.COMMAND_NOERROR)
                {
                    //ソースに代入
                    source[sourceNo] = line;
                    sourceNo++;
                }
            }
            else if (kind == Flag.COMMAND_TURN)
            {
                //回転コマンド
                error = Command.Turn(line.Replace(Flag.COMMAND_TURN_STR, ""));

                if (error == Flag.COMMAND_NOERROR)
                {
                    //ソースに代入
                    source[sourceNo] = line;
                    sourceNo++;
                }
            }
            else if (kind == Flag.COMMAND_STOP)
            {
                //停止コマンド
                error = Command.Stop(line.Replace(Flag.COMMAND_STOP_STR, ""));

                if (error == Flag.COMMAND_NOERROR)
                {
                    //ソースに代入
                    source[sourceNo] = line;
                    sourceNo++;
                }
            }
            else if (kind == Flag.COMMAND_START)
            {
                //実行コマンド
                error = Command.Start(line.Replace(Flag.COMMAND_START_STR, ""));
            }
            else if (kind == Flag.COMMAND_DELETE)
            {
                //削除コマンド
                error = Command.Delete(line.Replace(Flag.COMMAND_DELETE_STR, ""));

                if (error == Flag.COMMAND_NOERROR)
                {
                    if (sourceNo > 0)
                    {
                        //ソースを削除
                        sourceNo--;
                        source[sourceNo] = null;
                    }
                    else
                    {
                        error = Flag.COMMAND_ERROR;
                    }
                }
            }
            //結果
            return (error);
        }

        private void SkipSpace(ref string str)
        {
            string newStr = "";

            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] != ' ') && (str[i] != '　'))
                {
                    newStr += str[i].ToString();
                }
            }

            str = newStr;
        }
        private void ToUpper(ref string str)
        {
            str = str.ToUpper();
        }
    }
}
