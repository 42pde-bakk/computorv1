// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

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
	Console.WriteLine($"lets parse {arg}");
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
		Console.WriteLine($"part = {part}");
		Match match = r.Match(part);
		if (match.Success)
		{
			foreach (Object? group in match.Groups)
			{
				Console.WriteLine($"group: {group}");
			}
			// parse it like a * x^p
			Double coeff = double.Parse(match.Groups[1].Value);
			Int32 power = int.Parse(match.Groups[2].Value);
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

void Solve(IReadOnlyDictionary<Int32, Double> d)
{
	Double  a = d[2],
			b = d[1],
			c = d[0];
	Double discriminant = (b * b) - (4 * a * c);
	Double sol1 = (-b - Sqrt(discriminant)) / (2 * a);
	Double sol2 = (+b - Sqrt(discriminant)) / (2 * a);
	Console.WriteLine($"The two solutions are:\n{sol1}\n{sol2}");
}

static Dictionary<Int32, Double> parse(String arg)
{
	String arg2 = Regex.Replace(arg, @"\s+", "");
	Console.WriteLine($"arg={arg}, arg2={arg2}");
	String[] splitted = arg2.Split('=');
	Dictionary<Int32, Double> lhs = ParseSide(splitted[0]);
	Dictionary<Int32, Double> rhs = ParseSide(splitted[1]);
	
	return new Dictionary<Int32, Double>
	{
		{0, lhs[0] - rhs[0]},
		{1, lhs[1] - rhs[1]},
		{2, lhs[2] - rhs[2]}
	};
}

Console.WriteLine("Hello, World!");
Console.WriteLine($"args: {args[0]}");
if (args.Length != 1)
{
	Console.WriteLine("Error. Please provide your equation within quotes");
	Environment.Exit(1);
}
Dictionary<Int32, Double> dict = parse(args[0]);
Console.WriteLine($"0: {dict[0]}, 1: {dict[1]}, 2: {dict[2]}");
Solve(dict);
