// StockProjector.ManagedWrapper.h

#pragma once
#include "vector"
#include "MonteCarloSimulator.h"

using namespace System;
using namespace StockPredictor::QuantLibraries;

namespace StockProjectorManagedWrapper {

	public ref class NativeClassWrapper
	{
	public:
		NativeClassWrapper(); //{ m_nativeClass = new MonteCarloSimulator(); }
		~NativeClassWrapper();
	
		// TODO: Add your methods for this class here.
		//public:
		//	NativeClassWrapper(); //{ m_nativeClass = new MonteCarloSimulator(); }
		//	~NativeClassWrapper(); //{ this->!NativeClassWrapper(); }
			//!NativeClassWrapper() { delete m_nativeClass; }
		
		std::vector<double> SimulateStockPrice(int days, long iterations, double price, double rate, double dividendYield, double volatility); 
		/*{
			return m_nativeClass->SimulateStockPrice(days, iterations, price, rate, dividendYield, volatility);
		}*/

		std::vector<double> SimulateStockPrice(int days, long iterations, double price, double volatility);
		/*{
			return m_nativeClass->SimulateStockPrice(days, iterations, price, volatility);
		}*/
	private :
		MonteCarloSimulator* m_nativeClass;
	};
}
