#ifdef COMPILING_DLL
	#define DECLSPEC_CLASS __declspec(dllexport)
#else 
	#define DECLSPEC_CLASS __declspec(dllimport)
#endif
#pragma once //preprocessor directive designed to cause the current source file to be included only once in a single compilation
namespace StockPredictor
{
	namespace QuantLibraries
	{
		class DECLSPEC_CLASS MonteCarloSimulator
		{
		public:
			MonteCarloSimulator(void);
			std::vector<double> SimulateStockPrice(int days, long iterations, double price, double drift, double volatility);
			~MonteCarloSimulator(void);

		private:
			//std::vector<double> _arrPriceByDay;
			/*void Initialize(int days);*/
			bool Validate(int days, long iterations, double price, double drift, double volatility, std::string &message);
		};
	}
}

