#include "stdafx.h"
#include "stdlib.h"
#include "math.h"
#include "string"
#include "vector"
#include "MonteCarloSimulator.h"
#include "QuantFunctions.h"

using namespace StockPredictor::QuantLibraries;
using namespace std;

MonteCarloSimulator::MonteCarloSimulator(void)
{
	/*
	_arrPriceByDay = NULL;*/
}

//void MonteCarloSimulator::Initialize(int days)
//{
//	/*if (_arrPriceByDay != NULL)
//	{
//		delete[] _arrPriceByDay;
//	}*/
//	//_arrPriceByDay = new double[days];
//}

bool MonteCarloSimulator::Validate(int days, long iterations, double currentPrice, double drift, double volatility, std::string &message) //pass by reference message
{
	bool valid = true;
	message = "";
	if (days<= 0)
	{
		message += "Validation Error : Input parameter days must be greater than 0";
		valid = false;
	}

	if (iterations<= 0)
	{
		message += "Validation Error : Input parameter iterations must be greater than 0";
		valid = false;
	}
	
	if (currentPrice <= 0)
	{
		message += "Validation Error : Input parameter currentPrice must be greater than 0";
		valid = false;
	}

	if (drift < 0 || drift > 1)
	{
		message += "Validation Error : Input parameter drift must be greater than 0 and less than one";
		valid = false;
	}

	if (volatility <= 0 || volatility > 1)
	{
		message += "Validation Error : Input parameter drift must be greater than 0 and less than one";
		valid = false;
	}

	return valid;
}

vector<double> MonteCarloSimulator::SimulateStockPrice(int days, long iterations, double currentPrice, double drift, double volatility)
{
	// The stock price is modelled by GBM St = S0 * e^(d+z*sigma)
	// The log-normal return thus varies with a mean  = (drift - sigma^2 /2) * T and std deviation = sigma* sqroot(T)
	// So d = (drift - sigma^  2 /2)

	string validationMessage;
	if (Validate(days, iterations, currentPrice, drift, volatility, validationMessage) == false)
	{
		throw validationMessage;
	}

	const double numberOfDaysInYear = 252;
	double dailyVolatitily = volatility/sqrt(numberOfDaysInYear);
	QuantFunctions quantFunctions;

	//Initialize(days);
	double **arrPaths = new double*[iterations];
	for (int i = 0; i < iterations; i++)
	{
		arrPaths[i] = new double[days];
	}

	double mean = 0;
	if (drift != 0)
	{
		mean = (drift - pow(dailyVolatitily,2)/2);
	}
	vector<double> _arrPriceByDay;
	try
	{
		_arrPriceByDay.reserve(days);

		for (int i = 0; i < iterations; i++)
		{
			double price = currentPrice;
			for (int day = 0; day < days; day++)
			{
				double random = ((double)rand()/(double)RAND_MAX);
				double zValue = quantFunctions.GetInverseCDF(random);
				double rateOfReturn = mean + zValue * dailyVolatitily;
				price = price * exp(rateOfReturn);
				arrPaths[i][day] = price;
			}
		}

		for (int day = 0; day < days; day++)
		{
			double cumulative = 0;
			for (int iteration = 0; iteration < iterations; iteration++)
			{
				cumulative += arrPaths[iteration][day];
			}
			_arrPriceByDay.push_back(cumulative/iterations);
		}
	}
	catch(int ex)
	{
		delete[] arrPaths;
		throw ex;
	}
	
	delete[] arrPaths;
	return _arrPriceByDay;
}

MonteCarloSimulator::~MonteCarloSimulator(void)
{
	//delete[] _arrPriceByDay;
}
