"# stockpredictor" 
Predicts the Movement of stock prices through Monte Carlo Simulation

Theory

Assuming that the daily return is log normal, the price of a stock at S(t) can be modelled as a Geometric Brownian Motion 

S(t) = S(t-1) e(μ - σ^2/2+Zσ)

where,
- μ = daily drift ( given by r-d or 0)
- σ = daily implied volatility,
- z = random shock or random Inverse CDF of a Normal Distribution Function,
- r = risk free rate,
- d = dividend yield
