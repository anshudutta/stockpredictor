// This is the main DLL file.

#include "stdafx.h"
#include "vector"

#include "StockPredictor.ManagedWrapper.h"

using namespace StockPredictorManagedWrapper;
using namespace System::Collections::Generic;

NativeClassWrapper::NativeClassWrapper() 
{ 
	m_nativeClass = new MonteCarloSimulator();
	m_quantFunctions = new QuantFunctions(); 
}

NativeClassWrapper::~NativeClassWrapper() 
{ 
	this->!NativeClassWrapper();
}

NativeClassWrapper::!NativeClassWrapper() 
{ 
	delete m_nativeClass; 
	delete m_quantFunctions;
}

System::Collections::Generic::List<double>^ NativeClassWrapper::SimulateStockPrice(int days, long iterations, double price, double rate, double dividendYield, double volatility)
{
	std::vector<double> stockPrices = m_nativeClass->SimulateStockPrice(days, iterations, price, rate, dividendYield, volatility);
	System::Collections::Generic::List<double> ^iColl = gcnew System::Collections::Generic::List<double>();
	for (std::vector<double>::iterator stockPrice = stockPrices.begin();
		stockPrice != stockPrices.end(); ++stockPrice) {
			iColl->Add(*stockPrice);
	}
	return iColl;
}

System::Collections::Generic::List<double>^ NativeClassWrapper::SimulateStockPrice(int days, long iterations, double price, double volatility)
{
	std::vector<double> stockPrices = m_nativeClass->SimulateStockPrice(days, iterations, price, volatility);
	System::Collections::Generic::List<double> ^iColl = gcnew System::Collections::Generic::List<double>();
	for (std::vector<double>::iterator stockPrice = stockPrices.begin();
		stockPrice != stockPrices.end(); ++stockPrice) {
			iColl->Add(*stockPrice);
	}
	return iColl;
}

double NativeClassWrapper::GetWeightedStandardDeviation(System::Collections::Generic::List<double>^ distribution)
{
	int count = distribution->Count;
	double* arr = new double[count];

	for (int i = 0; i < count; i++)
	{
		arr[i] = distribution[i];
	}

	double result = m_quantFunctions->GetWeightedStandardDeviation(arr, count);
	delete arr;
	return result;
}

