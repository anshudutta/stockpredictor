#include "stdafx.h"
#include "CppUnitTest.h"
#include "stdlib.h"
#include "math.h"
#include "vector"
#include "iostream"
#include "string"
#include "QuantFunctions.h"
#include "MonteCarloSimulator.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using namespace StockPredictor::QuantLibraries;
using namespace std;

namespace StockPredictorTests
{
	TEST_CLASS(UnitTests)
	{
	public:
		
		TEST_METHOD(InverseCDF_Returns_Correct_Value)
		{
			QuantFunctions quantFunctions;
			double areaUnderCurve;
			double actualZValue;

			areaUnderCurve = 0.25;
			actualZValue = quantFunctions.GetInverseCDF(areaUnderCurve);
			Assert::AreEqual(-0.67, round( actualZValue));

			areaUnderCurve = 0.5;
			actualZValue = quantFunctions.GetInverseCDF(areaUnderCurve);
			Assert::AreEqual(0.0, round( actualZValue));

			areaUnderCurve = 0.75;
			actualZValue = quantFunctions.GetInverseCDF(areaUnderCurve);
			Assert::AreEqual(0.67, round( actualZValue));

			areaUnderCurve = .99;
			actualZValue = quantFunctions.GetInverseCDF(areaUnderCurve);
			Assert::AreEqual(2.32, round( actualZValue));
		}
		
		/*TEST_METHOD(Simulator_Throws_Valid_Error_Messages)
		{
			MonteCarloSimulator simulator;
			int days = -5;
			long iterations = 1000000;
			double price = 10;
			double drift = 0.02;
			double stdDev = 0.25;

			auto func = simulator.SimulateStockPrice(days, iterations, price, drift, stdDev);
			Assert::ExpectException<string>(round);
		}*/

		TEST_METHOD(Simulator)
		{
			MonteCarloSimulator simulator;
			int days = 20;
			long iterations = 5000;
			double price = 10;
			double drift = 0.01;
			double stdDev = 0.25;
			double rate = 0.65;
			double divYield = 0.02;

			std::vector<double> result;
			double proceAtEndOfProjection;



			result = simulator.SimulateStockPrice(days, iterations, price, rate, divYield, stdDev);
			proceAtEndOfProjection = result[days-1];
			Assert::AreNotEqual(0.0, proceAtEndOfProjection);

			days = 1;
			iterations = 500000;
			price = 252.34;
			stdDev = 1.0404;

			result = simulator.SimulateStockPrice(days, iterations, price, stdDev);
			proceAtEndOfProjection = result[days-1];
			Assert::AreNotEqual(0.0, proceAtEndOfProjection);
		}

		TEST_METHOD(Mean_Returns_Mean_Of_Distribution)
		{
			const int size = 5;
			double x[size] = {1,2,3,4,5};
			QuantFunctions quantFunctions;
			Assert::AreEqual((double)3, quantFunctions.GetMean(x, size));
		}

		TEST_METHOD(SD_Returns_SD_Of_Distribution)
		{
			const int size = 7;
			double x[size] = {10,2,38,23,38,23,21};
			QuantFunctions quantFunctions;
			Assert::AreEqual((double)12.299, quantFunctions.GetStandardDeviation(x, size));
		}

		TEST_METHOD(WSD_Returns_Weighted_SD_Of_Distribution)
		{
			const int size = 7;
			double x[size] = {10,2,38,23,38,23,21};
			QuantFunctions quantFunctions;
			double stdDeviation = quantFunctions.GetWeightedStandardDeviation(x, size);
			Assert::IsTrue(stdDeviation > 0);
		}
	private :
		double round(double number)
		{
			int temp = number * 100;
			return (double)temp/100;
		}
	};
}