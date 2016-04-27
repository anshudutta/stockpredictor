#ifdef COMPILING_DLL
#define DECLSPEC_CLASS __declspec(dllexport)
#else
#define DECLSPEC_CLASS __declspec(dllimport)
#endif

#pragma once
namespace StockPredictor{
	namespace QuantLibraries{
		class DECLSPEC_CLASS QuantFunctions
		{
		public:
			QuantFunctions(void);
			double GetInverseCDF(double p);
			double GetDailyRateFromYearlyRate(double rate);
			double GetStandardDeviation(double* points, long size);
			double GetWeightedStandardDeviation(double* distribution, long size);
			double GetMean(double* distribution, long size);
			double GetEWMA(double* distribution, long size);
			~QuantFunctions(void);
		};
	}
}

