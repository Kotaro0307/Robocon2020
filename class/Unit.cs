using System;
using System.Collections.Generic;

namespace Basic
{
    class Unit
    {
        public static string DeleteString(string str)
        {
            //不要な文字列を削除する（\r, \n, \0）
            char[] remove_chars = new char[3] { '\r', '\n', '\0' };
            //文字列削除中
            foreach (char c in remove_chars)
            {
                str = str.Replace(c.ToString(), "");
            }
            //変換後の文字列を返す
            return (str);
        }

        public static string GetReceiveData(string data)
        {
            //受信したデータからセンサの値のみを抜き取る
            if (data.Length > 3)
            {
                return (data.Substring(3));
            }
            return (data);
        }
        public static List<bool> StrToBool(string data)
        {
            //文字列からboolに変換
            List<bool> boolData = new List<bool>();

            for (int i = 0; i < data.Length; i++)
            {
                //末尾まで移動
                if (data[i] == '0')
                {
                    //数値を発見 0
                    boolData.Add(false);
                }
                else if (data[i] == '1')
                {
                    //数値を発見 1
                    boolData.Add(true);
                }
                else
                {
                    break;
                }
            }
            //値を返却
            return (boolData);
        }
        public static double StrToDouble(string data)
        {
            //文字列から小数に変換
            double doubleNo = 0;
            double intNo = 0;

            for (int i = 0; i < data.Length; i++)
            {
                //末尾まで移動
                if ((data[i] >= '0') && (data[i] <= '9'))
                {
                    //数値を発見
                    for (int j = i; j < data.Length; j++)
                    {
                        //末尾まで移動
                        if (data[j] == '.')
                        {
                            //点を発見
                            for (int k = (j + 1); k < data.Length; k++)
                            {
                                //末尾まで移動
                                if ((data[k] < '0') && (data[k] > '9'))
                                {
                                    //数値の終了
                                    break;
                                }
                                //小数部の計算
                                doubleNo += (data[k] - '0') * Math.Pow(0.1, (k - j));
                            }
                            //数値の終了
                            break;
                        }
                        else if ((data[j] < '0') && (data[j] > '9'))
                        {
                            //数値の終了
                            break;
                        }
                        //整数部の計算
                        intNo = (intNo * 10) + (data[j] - '0');
                    }
                    //数値の終了
                    break;
                }
            }
            //値の返却（整数部＋小数部）
            return (intNo + doubleNo);
        }

        public static int Math_limit(int data, int max, int min)
        {
            //最大値、最小値の制限
            if (data > max)
            {
                //大きすぎる
                return (max);
            }
            else if (data < min)
            {
                //小さすぎる
                return (min);
            }
            //範囲内
            return (data);
        }

        public static double RadianToDegree(double radian)
        {
            //ラジアンから度に変更
            return (radian * 180.0 / Math.PI);
        }
        public static double DegreeToRadian(double degree)
        {
            //度からラジアンに変更
            return (degree * Math.PI / 180.0);
        }
    }
}