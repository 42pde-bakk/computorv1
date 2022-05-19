// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.RegularExpressions;

static Double Abs(Double nb)
{
	return (nb < 0) ? nb * -1 : nb;
}

static Double Sqrt(Double nb)
{
	if (nb <= 0)
	{
		return (double.NaN);
	}
	Double root = nb / 3;
	Int32 i;
	for (i = 0; i < 32; i++)
		root = (root + nb / root) / 2;
	return (root);
}

static Dictionary<Int32, Double> ParseSide(String arg)
{
	Regex r = new Regex(@"([+-]?[\d+]?.?[\d+]?)\*X\^([+-]?[\d+])", RegexOptions.IgnoreCase);
	Dictionary<Int32, Double> dict = new()
	{
		{0, 0},
		{1, 0},
		{2, 0}
	};
	String[] parts = Regex.Split(arg, @"(?=[+-])");

	foreach (String part in parts)
	{
		Match match = r.Match(part);
		if (match.Success)
		{
			// parse it like a * x^p
			Double coeff = double.Parse(match.Groups[1].Value);
			Int32 power = int.Parse(match.Groups[2].Value);
			if (!dict.ContainsKey(power))
				dict[power] = 0;
			dict[power] += coeff;
		}
		else
		{
			// maybe its just a regular int/float value
			Int32 coeff = int.Parse(part);
			dict[0] += coeff;
		}
	}
	return dict;
}

static Int32 GCD(Int32 a, Int32 b)
{
	while (b > 0)
	{
		Int32 rem = a % b;
		a = b;
		b = rem;
	}
	return a;
}

static Int32 Max(Int32 a, Int32 b)
{
	return (a > b ? a : b);
}

static void ShowIrreducableFraction(Int32 up, Int32 down)
{
	Int32 gcd = GCD(up, down);
	up /= gcd;
	down /= gcd;
	if (down is -1 or 1)
	{
		Console.WriteLine($"x = {up / down}");
	}
	else
	{
		String upstr = new String(up.ToString());
		String downstr = new String(down.ToString());
	
		Console.WriteLine($"    {upstr}");
		Console.WriteLine($"x = {new String('\u2014', Max(upstr.Length, downstr.Length))}");
		Console.WriteLine($"    {downstr}");
		Console.WriteLine($"down = {down}");
	}
}

static bool isInteger(Double d)
{
	return (d == (int) d);
}

static void ShowSteps(Double d, Double a, Double b, Double c)
{
	Console.WriteLine();
	Console.OutputEncoding = Encoding.Unicode;
	String upper = $"-{b} ± \u221A({b}\u00B2 - 4*{a}*{c})";
	Console.WriteLine($"    {upper}");
	Console.WriteLine($"x = {new String('\u2014', upper.Length)}");
	String lower = $"2 * {a}";
	Int32 sp = (upper.Length - lower.Length) / 2;
	Console.WriteLine($"    {new String(' ', sp)}{lower}");
	Console.WriteLine();

	Console.WriteLine($"Discriminant = {b}\u00b2 - 4 * {a} * {c} = {b*b} - {4*a*c} = {d}");
	Console.WriteLine();

	upper = $"-{b} ± \u221A({d})";
	Console.WriteLine($"    {upper}");
	Console.WriteLine($"x = {new String('\u2014', upper.Length)}");
	lower = $"{2 * a}";
	sp = (upper.Length - lower.Length) / 2;
	Console.WriteLine($"    {new String(' ', sp)}{lower}");

	Double dSqrt = Sqrt(d);

	if (isInteger(2 * a) && (isInteger(-b + dSqrt) || isInteger(-b - dSqrt)))
	{
		Console.WriteLine($"Here be the irreducable fractions:");
		if (isInteger(-b + dSqrt))
		{
			ShowIrreducableFraction((Int32)(-b + Sqrt(d)), (Int32)(2 * a));
		}

		if (isInteger(-b - dSqrt))
		{
			ShowIrreducableFraction((Int32)(-b - Sqrt(d)), (Int32)(2 * a));
		}
		
	}
}

static void Solve(IReadOnlyDictionary<Int32, Double> d)
{
	Double  a = d[2],
			b = d[1],
			c = d[0];
	Double discriminant = (b * b) - (4 * a * c);
	if (Environment.GetEnvironmentVariable("COMPUTORV1_BONUS") != null)
	{
		ShowSteps(discriminant, a, b, c);
	}

	Double sol1 = (-b + Sqrt(discriminant)) / (2 * a);
	Double sol2 = (-b - Sqrt(discriminant)) / (2 * a);
	
	if (discriminant > 0)
	{
		Console.WriteLine("Discriminant is strictly positive, the two solutions are:");
		Console.WriteLine(sol1);
		Console.WriteLine(sol2);
	}
	else if (discriminant == 0)
	{
		Console.WriteLine("Discriminant is one, so we have only one solution:");
		Console.WriteLine(sol1);
	}
	else
	{
		// no real roots
		Console.WriteLine("Unable to solve, since the discriminant is lower than zero");
	}
}

static void SolveEasy(IReadOnlyDictionary<Int32, Double> d)
{
	Int32 highestPower = GetHighestPolynomialDegree(d);

	if (highestPower == 0)
	{
		Console.WriteLine("Solvable for every single X");
	}
	else
	{
		Double rhs = d.ContainsKey(0) ? d[0] * -1 : 0;
		Double answer = rhs / d[1];
		Console.WriteLine($"The solution is:");
		Console.WriteLine(answer);
	}
}

static Dictionary<Int32, Double> Parse(String arg)
{
	String arg2 = Regex.Replace(arg, @"\s+", "");
	String[] splitted = arg2.Split('=');
	Dictionary<Int32, Double> lhs = ParseSide(splitted[0]);
	Dictionary<Int32, Double> rhs = ParseSide(splitted[1]);

	foreach ((Int32 key, Double value) in rhs)
	{
		if (!lhs.ContainsKey(key))
		{
			lhs[key] = 0;
		}
		lhs[key] -= value;
	}

	foreach ((Int32 key, var _) in lhs.Where(kvp => kvp.Value == 0).ToList())
	{
		if (key != 0)
		{
			lhs.Remove(key);
		}
	}
	return (lhs);
}

static Int32 GetHighestPolynomialDegree(IReadOnlyDictionary<Int32, Double> coeffs)
{
	Int32 highestDegree = 0;

	if (coeffs.Count > 0)
	{
		highestDegree = coeffs.Keys.Max();
	}
	return (highestDegree);
}

static void ShowHighestPolynomialDegree(IReadOnlyDictionary<Int32, Double> coeffs)
{
	Int32 highestPolynomialDegree = GetHighestPolynomialDegree(coeffs);
	Console.WriteLine($"Polynomial degree: {highestPolynomialDegree}");
}

static Char GetSignPrefix(Double nb) => nb >= 0 ? '+' : '-';

static void ShowReducedForm(IReadOnlyDictionary<Int32, Double> coeffs)
{
	Console.Write("Reduced form: ");
	Boolean first = true;
	foreach ((Int32 power, Double coefficient) in coeffs.OrderByDescending(x => x.Key))
	{
		if (first && coefficient < 0)
		{
			Console.Write('-');
		}

		if (coefficient != 0)
		{
			if (!first)
			{
				Console.Write($" {GetSignPrefix(coefficient)} ");
			}
			first = false;
			Console.Write($"{Abs(coefficient)} * X^{power}");
		}
	}

	if (first)
	{
		Console.Write("0");
	}
	Console.WriteLine(" = 0");
}

static void Computorv1(IReadOnlyList<String> args)
{
	Dictionary<Int32, Double> dict;
	if (args.Count != 1)
	{
		Console.WriteLine("Error. Please provide your equation within quotes");
		Environment.Exit(1);
	}

	try
	{
		dict = Parse(args[0]);
	}
	catch
	{
		Console.WriteLine("Could you do me a favour?");
		System.Threading.Thread.Sleep(1000);
		Console.WriteLine("And please give me valid input?!?!?!??!!");
		return ;
	}
	Int32 highestDegree = GetHighestPolynomialDegree(dict);
	ShowReducedForm(dict);
	ShowHighestPolynomialDegree(dict);
	if (highestDegree < 2)
	{
		SolveEasy(dict);
	}
	else if (highestDegree == 2)
	{
		Solve(dict);
	}
	else
	{
		Console.WriteLine("Error. Please provide me with a valid polynomial.");
	}
}


Computorv1(args);
