// StockPredictor.ManagedWrapper.h

#pragma once

#include <MonteCarloSimulator.h>
#include <QuantFunctions.h>
#include <vector>
using namespace System;
using namespace StockPredictor::QuantLibraries;

namespace StockPredictorManagedWrapper {

	public ref class NativeClassWrapper
	{
	public:
			NativeClassWrapper() ;
			~NativeClassWrapper();
			!NativeClassWrapper();
		
		System::Collections::Generic::List<double>^ SimulateStockPrice(int days, long iterations, double price, double rate, double dividendYield, double volatility);

		System::Collections::Generic::List<double>^ SimulateStockPrice(int days, long iterations, double price, double volatility);
		/*{
			return m_nativeClass->SimulateStockPrice(days, iterations, price, volatility);
		}*/
		double GetWeightedStandardDeviation(System::Collections::Generic::List<double>^ distribution);
	private :
		MonteCarloSimulator* m_nativeClass;
		QuantFunctions* m_quantFunctions;
	};
}