// See https://aka.ms/new-console-template for more information

using Computorv1;

static string	TryToRepresentAsFraction(double nb)
{
	if (nb == 0)
		return ("");
	if (MyMath.IsInteger(nb))
		return (nb.ToString());
	foreach (int value in Enumerable.Range(2, 100))
	{
		if (MyMath.IsInteger(nb * value))
		{
			int numerator = (int)(nb * value);
			int denominator = value;
			return ($"{numerator}/{denominator}");
		}
	}
	return ($"{nb:F5}");
}

static void ShowIrreducableFraction(int up, int down)
{
	int gcd = MyMath.Gcd(up, down);
	up /= gcd;
	down /= gcd;
	if (down is -1 or 1)
	{
		Console.WriteLine($"x = {up / down}");
	}
	else
	{
		string upstr = new string(up.ToString());
		string downstr = new string(down.ToString());
	
		Console.WriteLine($"    {upstr}");
		Console.WriteLine($"x = {new string('\u2014', MyMath.Max(upstr.Length, downstr.Length))}");
		Console.WriteLine($"    {downstr}");
		Console.WriteLine($"down = {down}");
	}
}


static void ShowSteps(double d, double a, double b, double c)
{
	Console.WriteLine();
	// set outputencoding to Unicode if on Windows
	// Console.OutputEncoding = Encoding.Unicode;
	string upper = $"-{b} ± \u221A({b}\u00B2 - 4*{a}*{c})";
	Console.WriteLine($"    {upper}");
	Console.WriteLine($"x = {new string('\u2014', upper.Length)}");
	string lower = $"2 * {a}";
	int sp = (upper.Length - lower.Length) / 2;
	Console.WriteLine($"    {new string(' ', sp)}{lower}");
	Console.WriteLine();

	Console.WriteLine($"Discriminant = {b}\u00b2 - 4 * {a} * {c} = {b*b} - {4*a*c} = {d}");
	Console.WriteLine();

	upper = $"{-1 * b} ± \u221A({d})";
	Console.WriteLine($"    {upper}");
	Console.WriteLine($"x = {new string('\u2014', upper.Length)}");
	lower = $"{2 * a}";
	sp = (upper.Length - lower.Length) / 2;
	Console.WriteLine($"    {new string(' ', sp)}{lower}");

	double dSqrt = MyMath.Sqrt(d);

	if (MyMath.IsInteger(2 * a) && (MyMath.IsInteger(-b + dSqrt) || MyMath.IsInteger(-b - dSqrt)))
	{
		Console.WriteLine($"Here be the irreducable fractions:");
		if (MyMath.IsInteger(-b + dSqrt))
		{
			ShowIrreducableFraction((int)(-b + MyMath.Sqrt(d)), (int)(2 * a));
		}

		if (MyMath.IsInteger(-b - dSqrt))
		{
			ShowIrreducableFraction((int)(-b - MyMath.Sqrt(d)), (int)(2 * a));
		}
	}
}

static void Solve(IReadOnlyDictionary<int, double> d)
{
	double	a = d.ContainsKey(2) ? d[2] : 0,
			b = d.ContainsKey(1) ? d[1] : 0,
			c = d.ContainsKey(0) ? d[0] : 0;

	double discriminant = (b * b) - (4 * a * c);
	if (Environment.GetEnvironmentVariable("COMPUTORV1_BONUS") != null)
	{
		ShowSteps(discriminant, a, b, c);
	}

	if (discriminant > 0)
	{
		double solution1 = (-b + MyMath.Sqrt(discriminant)) / (2 * a);
		double solution2 = (-b - MyMath.Sqrt(discriminant)) / (2 * a);
		Console.WriteLine("Discriminant is strictly positive, the two solutions are:");
		Console.WriteLine(TryToRepresentAsFraction(solution1));
		Console.WriteLine(TryToRepresentAsFraction(solution2));
	}
	else if (discriminant == 0)
	{
		double solution = (-b + 0) / (2 * a);
		Console.WriteLine("Discriminant is zero, so we have only one solution:");
		string solutionStr = TryToRepresentAsFraction(solution);
		if (string.IsNullOrEmpty(solutionStr))
			Console.WriteLine(0);
		else
			Console.WriteLine(solutionStr);
		// Console.WriteLine($"{TryToRepresentAsFraction(solution)}");
	}
	else
	{
		// no real roots
		double realPart = -b / (2 * a);
		double imaginaryPart = MyMath.Sqrt(-discriminant) / (2 * a);

		Console.WriteLine("Discriminant is negative, solutions are complex:");
		Console.WriteLine($"{TryToRepresentAsFraction(realPart)} - {TryToRepresentAsFraction(imaginaryPart)} * i");
		Console.WriteLine($"{TryToRepresentAsFraction(realPart)} + {TryToRepresentAsFraction(imaginaryPart)} * i");
	}
}

static void SolveEasy(IReadOnlyDictionary<int, double> d)
{
	int highestPower = GetHighestPolynomialDegree(d);

	if (highestPower == 0)
	{
		if (d[0] == 0)
			Console.WriteLine("Solvable for every single X");
		else
		{
			Console.WriteLine("Error. The statement does not seem to be true:");
			Console.WriteLine($"\t {d[0]} != 0");
			Environment.Exit(1);
		}
	}
	else
	{
		double rhs = d.ContainsKey(0) ? d[0] * -1 : 0;
		double answer = rhs / d[1];
		Console.WriteLine($"The solution is:");
		Console.WriteLine(TryToRepresentAsFraction(answer));
	}
}


static int GetHighestPolynomialDegree(IReadOnlyDictionary<int, double> coeffs)
{
	foreach ((int key, double value) in coeffs.OrderByDescending(x => x.Key))
	{
		if (value != 0)
		{
			return (key);
		}
	}
	return (0);
}

static void ShowHighestPolynomialDegree(IReadOnlyDictionary<int, double> coeffs)
{
	int highestPolynomialDegree = GetHighestPolynomialDegree(coeffs);
	Console.WriteLine($"Polynomial degree: {highestPolynomialDegree}");
}

static char GetSignPrefix(double nb) => nb >= 0 ? '+' : '-';

static void ShowReducedForm(IReadOnlyDictionary<int, double> coeffs)
{
	Console.Write("Reduced form: ");
	bool first = true;
	foreach ((int power, double coefficient) in coeffs.OrderByDescending(x => x.Key))
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
			Console.Write($"{MyMath.Abs(coefficient)} * X^{power}");
		}
	}

	if (first)
	{
		Console.Write("0");
	}
	Console.WriteLine(" = 0");
}

static void Computorv1(IReadOnlyList<string> args)
{
	Dictionary<int, double> dict = new Dictionary<int, double>();
	if (args.Count != 1)
	{
		Console.WriteLine("Error. Please provide your equation within quotes");
		Environment.Exit(1);
	}

	try
	{
		dict = Parsing.Parse(args[0]);
	}
	catch (Exception e)
	{
		Console.WriteLine("Could you do me a favour?");
		Console.WriteLine("And please give me valid input?!?!?!??!!");
		// Console.WriteLine(e.ToString());
		Environment.Exit(1);
	}
	int highestDegree = GetHighestPolynomialDegree(dict);
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
		Environment.Exit(1);
	}
}


Computorv1(args);
