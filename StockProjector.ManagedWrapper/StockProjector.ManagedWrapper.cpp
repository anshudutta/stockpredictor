// This is the main DLL file.

#include "stdafx.h"
#include "vector"

#include "StockProjector.ManagedWrapper.h"

using namespace StockProjectorManagedWrapper;

NativeClassWrapper::NativeClassWrapper()
{
	 m_nativeClass = new MonteCarloSimulator();
}

NativeClassWrapper::~NativeClassWrapper()
{
	 delete m_nativeClass;
}

std::vector<double> NativeClassWrapper::SimulateStockPrice(int days, long iterations, double price, double rate, double dividendYield, double volatility) 
{
	 return m_nativeClass->SimulateStockPrice(days, iterations, price, rate, dividendYield, volatility);
}

std::vector<double> NativeClassWrapper::SimulateStockPrice(int days, long iterations, double price, double volatility) 
{
	 return m_nativeClass->SimulateStockPrice(days, iterations, price, volatility);
}

