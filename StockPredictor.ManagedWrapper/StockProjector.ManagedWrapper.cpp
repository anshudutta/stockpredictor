// This is the main DLL file.

#include "stdafx.h"
#include "vector"

#include "StockPredictor.ManagedWrapper.h"

using namespace StockPredictorManagedWrapper;
using namespace System::Collections::Generic;

NativeClassWrapper::NativeClassWrapper() { m_nativeClass = new MonteCarloSimulator(); }
//{
//	 m_nativeClass = new MonteCarloSimulator();
//}

NativeClassWrapper::~NativeClassWrapper() { this->!NativeClassWrapper(); }
//{
//	 delete m_nativeClass;
//}

NativeClassWrapper::!NativeClassWrapper() { delete m_nativeClass; }

System::Collections::Generic::List<double>^ NativeClassWrapper::SimulateStockPrice(int days, long iterations, double price, double rate, double dividendYield, double volatility)
{
	std::vector<double> stockPrices = m_nativeClass->SimulateStockPrice(days, iterations, price, rate, dividendYield, volatility);
	System::Collections::Generic::List<double> ^iColl;
	for (std::vector<double>::iterator stockPrice = stockPrices.begin();
		stockPrice != stockPrices.end(); ++stockPrice) {
			iColl->Add(*stockPrice);
	}
	return iColl;
}

