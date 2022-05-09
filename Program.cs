// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

static Dictionary<double, double> parseSide(string arg)
{
	Console.WriteLine($"lets parse {arg}");
	Regex r = new Regex(@"([+-]?[\d+]?.?[\d+]?)\*X\^([+-]?[\d+])", RegexOptions.IgnoreCase);
	Dictionary<double, double> dict = new Dictionary<double, double>
	{
		{0, 0},
		{1, 0},
		{2, 0}
	};
	string[] parts = Regex.Split(arg, @"(?=[+-])");

	foreach (string part in parts)
	{
		Console.WriteLine($"part = {part}");
		Match match = r.Match(part);
		if (match.Success)
		{
			foreach (object? group in match.Groups)
			{
				Console.WriteLine($"group: {group}");
			}
			// parse it like a * x^p
			double coeff = double.Parse(match.Groups[1].Value);
			double power = double.Parse(match.Groups[2].Value);
			dict[power] += coeff;
		}
		else
		{
			// maybe its just a regular int/float value
		}
	}
	return dict;
}

static Dictionary<double, double> parse(string arg)
{
	string arg2 = Regex.Replace(arg, @"\s+", "");
	Console.WriteLine($"arg={arg}, arg2={arg2}");
	var splitted = arg2.Split('=');
	return parseSide(splitted[0]);
}

Console.WriteLine("Hello, World!");
Console.WriteLine($"args: {args[0]}");
if (args.Length != 1)
{
	Console.WriteLine("Error. Please provide your equation within quotes");
	Environment.Exit(1);
}
var dict = parse(args[0]);