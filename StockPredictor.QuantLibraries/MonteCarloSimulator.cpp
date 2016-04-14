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
}

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

vector<double> MonteCarloSimulator::SimulateStockPrice(int days, long iterations, double currentPrice, double volatility)
{
	return SimulateStockPrice(days, iterations, currentPrice, 0, 0, volatility);
}

vector<double> MonteCarloSimulator::SimulateStockPrice(int days, long iterations, double currentPrice, double rate, double dividendYield, double volatility)
{
	// The stock price is modelled by GBM St = S0 * e^(d+z*sigma)
	// The log-normal return thus varies with a mean  = (drift - sigma^2 /2) * T and std deviation = sigma* sqroot(T)
	// So d = (drift - sigma^  2 /2)

	string validationMessage;
	double drift = GetDrift(rate, dividendYield);
	if (Validate(days, iterations, currentPrice, drift, volatility, validationMessage) == false)
	{
		throw validationMessage;
	}

	const double numberOfDaysInYear = 252;
	double dailyVolatitily = volatility/sqrt(numberOfDaysInYear);
	QuantFunctions quantFunctions;
	vector<vector<double>> arrPaths;

	double mean = 0;
	if (drift != 0)
	{
		mean = (drift - pow(dailyVolatitily,2)/2);
	}
	vector<double> _arrPriceByDay;
	
	_arrPriceByDay.reserve(days);
	
	for (int i = 0; i < iterations; i++)
	{
		vector<double> iteration;
		double price = currentPrice;
		for (int day = 0; day < days; day++)
		{
			double random = ((double)rand()/(double)RAND_MAX);
			double zValue = quantFunctions.GetInverseCDF(random);
			double rateOfReturn = mean + zValue * dailyVolatitily;
			price = price * exp(rateOfReturn);
			iteration.push_back(price);
		}
		arrPaths.push_back(iteration);
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
	return _arrPriceByDay;
}

double MonteCarloSimulator::GetDrift(double rate, double dividendYield)
{
	QuantFunctions quantFunctions;
	double dailyRate = quantFunctions.GetDailyRateFromYearlyRate(rate);
	dividendYield = quantFunctions.GetDailyRateFromYearlyRate(dividendYield);
	double drift = dailyRate - dividendYield;
	return drift > 0 ? drift : 0;;
}

MonteCarloSimulator::~MonteCarloSimulator(void)
{
}
