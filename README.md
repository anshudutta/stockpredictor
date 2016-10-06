"# stockpredictor" 

Predicts the Movement of stock prices through Monte Carlo Simulation

Theory

Assuming that the daily return is log normal, the price of a stock at S(t) can be modelled as a Geometric Brownian Motion 

S(t) = S(t-1) e(μ - σ^2/2+Zσ)

where,
- μ = Daily drift ( given by r-d or 0)
- σ = Daily implied volatility,
- z = Random shock or random Inverse CDF of a Normal Distribution Function,
- r = Risk free rate,
- d = Dividend yield

The service crwals various public website to get relevant information for a security, e.g. spot price, implied volatility, risk free rate, dividend yield etc. It then feeds the values to the above model and runs Monte Carlo simulation for n paths each of which is defined by a random Z value. The outcome is the price of the stock after x days as derived from the simulation.

Note that this is a mathematical simulation process and not the same as technical analysis of a stock. This is simple giving us the mean value of a stock after x days when the above model is simulated over N number of paths.

The simulator function is written in C++ and the service is in .NET, C#
