// See https://aka.ms/new-console-template for more information

using System.Text;
using Computorv1;

static String	TryToRepresentAsFraction(Double nb)
{
	if (nb == 0)
		return ("");
	if (MyMath.isInteger(nb))
		return (nb.ToString());
	foreach (Int32 value in Enumerable.Range(2, 100))
	{
		if (MyMath.isInteger(nb * value))
		{
			Int32 numerator = (Int32)(nb * value);
			Int32 denominator = value;
			return ($"{numerator}/{denominator}");
		}
	}
	return ($"{nb:F5}");
}

static void ShowIrreducableFraction(Int32 up, Int32 down)
{
	Int32 gcd = MyMath.GCD(up, down);
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
		Console.WriteLine($"x = {new String('\u2014', MyMath.Max(upstr.Length, downstr.Length))}");
		Console.WriteLine($"    {downstr}");
		Console.WriteLine($"down = {down}");
	}
}


static void ShowSteps(Double d, Double a, Double b, Double c)
{
	Console.WriteLine();
	// set outputencoding to Unicode if on Windows
	// Console.OutputEncoding = Encoding.Unicode;
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

	Double dSqrt = MyMath.Sqrt(d);

	if (MyMath.isInteger(2 * a) && (MyMath.isInteger(-b + dSqrt) || MyMath.isInteger(-b - dSqrt)))
	{
		Console.WriteLine($"Here be the irreducable fractions:");
		if (MyMath.isInteger(-b + dSqrt))
		{
			ShowIrreducableFraction((Int32)(-b + MyMath.Sqrt(d)), (Int32)(2 * a));
		}

		if (MyMath.isInteger(-b - dSqrt))
		{
			ShowIrreducableFraction((Int32)(-b - MyMath.Sqrt(d)), (Int32)(2 * a));
		}
		
	}
}

static void Solve(IReadOnlyDictionary<Int32, Double> d)
{
	Double	a = d.ContainsKey(2) ? d[2] : 0,
			b = d.ContainsKey(1) ? d[1] : 0,
			c = d.ContainsKey(0) ? d[0] : 0;

	Double discriminant = (b * b) - (4 * a * c);
	if (Environment.GetEnvironmentVariable("COMPUTORV1_BONUS") != null)
	{
		ShowSteps(discriminant, a, b, c);
	}

	if (discriminant > 0)
	{
		Double solution1 = (-b + MyMath.Sqrt(discriminant)) / (2 * a);
		Double solution2 = (-b - MyMath.Sqrt(discriminant)) / (2 * a);
		Console.WriteLine("Discriminant is strictly positive, the two solutions are:");
		Console.WriteLine(TryToRepresentAsFraction(solution1));
		Console.WriteLine(TryToRepresentAsFraction(solution2));
	}
	else if (discriminant == 0)
	{
		Double solution = (-b + 0) / (2 * a);
		Console.WriteLine("Discriminant is zero, so we have only one solution:");
		Console.WriteLine($"{TryToRepresentAsFraction(solution)}");
	}
	else
	{
		// no real roots
		Double realPart = -b / (2 * a);
		Double imaginaryPart = MyMath.Sqrt(-discriminant) / (2 * a);

		Console.WriteLine("Discriminant is negative, solutions are complex:");
		Console.WriteLine($"{TryToRepresentAsFraction(realPart)} - {TryToRepresentAsFraction(imaginaryPart)} * i");
		Console.WriteLine($"{TryToRepresentAsFraction(realPart)} + {TryToRepresentAsFraction(imaginaryPart)} * i");
	}
}

static void SolveEasy(IReadOnlyDictionary<Int32, Double> d)
{
	Int32 highestPower = GetHighestPolynomialDegree(d);

	if (highestPower == 0)
	{
		if (d[0] == 0)
			Console.WriteLine("Solvable for every single X");
		else
		{
			Console.WriteLine("Error. The statement does not seem to be true:");
			Console.WriteLine($"\t {d[0]} != 0");
		}
	}
	else
	{
		Double rhs = d.ContainsKey(0) ? d[0] * -1 : 0;
		Double answer = rhs / d[1];
		Console.WriteLine($"The solution is:");
		Console.WriteLine(TryToRepresentAsFraction(answer));
	}
}


static Int32 GetHighestPolynomialDegree(IReadOnlyDictionary<Int32, Double> coeffs)
{
	foreach ((Int32 key, Double value) in coeffs.OrderByDescending(x => x.Key))
	{
		if (value != 0)
		{
			return (key);
		}
	}
	return (0);
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
			Console.Write($"{MyMath.Abs(coefficient)} * X^{power}");
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
		dict = Parsing.Parse(args[0]);
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
