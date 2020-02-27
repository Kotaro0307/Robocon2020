#pragma once

#include <string>
#include <vector>
#include "Urg_driver.h"
#include "Connection_information.h"
#include "math_utilities.h"

#define MODE_FRONT	(0)	//‘O
#define MODE_BACK	(1)	//Œã‚ë

#define DllExport	__declspec(dllexport)

extern "C"
{
	DllExport int GetDistance(int mode, int argc, std::string argv[], char* urg_port, long* deg0, long* deg90, long* degM90);
	DllExport void Close(int mode);
}