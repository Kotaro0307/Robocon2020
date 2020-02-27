#include "LRF.h"

#define NOT_OPEN		(1)	//開くことができない
#define NOT_GETDISTANCE	(2)	//値を取得できなかった
#define CONNECT			(3)	//接続した
#define CLOSE			(0)	//閉じた

using namespace qrk;
using namespace std;

void GetData(const Urg_driver& urg, const vector<long>& data, long& deg0Len, long& deg90Len, long& degM90Len)
{
	// 全てのデータの X-Y の位置を表示
	long min_distance = urg.min_distance();
	long max_distance = urg.max_distance();
	size_t data_n = data.size();

	//距離を取得
	for (size_t i = 0; i < data_n; i++) {
		//ラジアン
		double radian = urg.index2rad(i);
			
		if (radian == 0) {
			//0度
			if ((data[i] <= min_distance) || (data[i] >= max_distance)) {
				//範囲外
				deg0Len = 7000;
			}
			else {
				//範囲内
				deg0Len = data[i];
			}
		}
		else if (i == (data_n - 1)) {
			//90度
			if ((data[i] <= min_distance) || (data[i] >= max_distance)) {
				//範囲外
				deg90Len = 7000;
			}
			else {
				//範囲内
				deg90Len = data[i];
			}
		}
		else if (i == 0) {
			//-90度
			if ((data[i] <= min_distance) || (data[i] >= max_distance)) {
				//範囲外
				degM90Len = 7000;
			}
			else {
				//範囲内
				degM90Len = data[i];
			}
		}
	}
}

vector<int> myMode;
vector<int> myStats;

int GetDistance(int mode, int argc, string argv[], char* urg_port, long* deg0, long* deg90, long* degM90)
{
	int array_num = 0;
	bool oneFlag = false;

	const char* argv_char[] = { NULL };
	for (int i = 0; i != argc; i++) {
		argv_char[i] = argv[i].c_str();
	}

	if (myMode.size() > 0) {
		//２回目以降
		for (int i = 0; i < myMode.size(); i++) {
			if (myMode[i] == mode) {
				//かぶり有り
				oneFlag = true;
				break;
			}
		}

		if (!oneFlag) {
			array_num = myMode.size();
			//要素の追加
			myMode.push_back(mode);
			myStats.push_back(CONNECT);
		}
		//初期化
		oneFlag = false;
	}
	else {
		//初回
		array_num = myMode.size();
		//要素の追加
		myMode.push_back(mode);
		myStats.push_back(CONNECT);
	}

	Connection_information information(argc, argv_char, urg_port);
	// 接続
	Urg_driver urg;
	if (!urg.open(information.device_or_ip_name(), information.baudrate_or_port_number(), information.connection_type())) {
		//開かなかった
		return (NOT_OPEN);
	}
	// データ取得
	urg.set_scanning_parameter(urg.deg2step(-90), urg.deg2step(+90), 0);
	urg.start_measurement(Urg_driver::Distance, Urg_driver::Infinity_times, 0);

	while (1) {
		vector<long> data;
		
		if (urg.get_distance(data)) {
			//距離を取得
			GetData(urg, data, *deg0, *deg90, *degM90);
		}
		else {
			//距離を取得できなかった
			return (NOT_GETDISTANCE);
		}

		if (!urg.is_open()) {
			//開かなかった
			return (NOT_OPEN);
		}

		if (myStats[array_num] == CLOSE) {
			//LRFを閉じる
			break;
		}
	}
	//閉じる
	urg.close();

	return (CLOSE);
}

void Close(int mode)
{
	//LRFを閉じる
	for (int i = 0; i < myMode.size(); i++) {
		if (myMode[i] == mode) {
			myStats[i] = CLOSE;
			break;
		}
	}
}