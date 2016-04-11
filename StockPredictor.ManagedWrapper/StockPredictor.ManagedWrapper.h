// StockPredictor.ManagedWrapper.h

#pragma once

#include <MonteCarloSimulator.h>
#include <vector>
using namespace System;
using namespace StockPredictor::QuantLibraries;

namespace StockPredictorManagedWrapper {

	public ref class NativeClassWrapper
	{
	public:
			NativeClassWrapper() ;//{ m_nativeClass = new MonteCarloSimulator(); }
			~NativeClassWrapper();//{ this->!NativeClassWrapper(); }
			!NativeClassWrapper();// { delete m_nativeClass; }
		
		System::Collections::Generic::List<double>^ SimulateStockPrice(int days, long iterations, double price, double rate, double dividendYield, double volatility);
		/*{
			std::vector<double> stockPrices = m_nativeClass->SimulateStockPrice(days, iterations, price, rate, dividendYield, volatility);

			System::Collections::Generic::List<double> ^iColl;
			for (std::vector<double>::iterator stockPrice = stockPrices.begin();
				stockPrice != stockPrices.end(); ++stockPrice) {
					iColl->Add(*stockPrice);
			}
			return iColl;
		}*/

		//System::Collections::Generic::ICollection<double> SimulateStockPrice(int days, long iterations, double price, double volatility);
		/*{
			return m_nativeClass->SimulateStockPrice(days, iterations, price, volatility);
		}*/
	private :
		MonteCarloSimulator* m_nativeClass;
	};
}