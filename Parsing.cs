using System.Text.RegularExpressions;

namespace Computorv1
{
	internal static class Parsing
	{
		static Dictionary<Int32, Double> ParseSide(String arg)
		{
			Regex allRegex = new Regex(@"(?<coefficient>[+-]?[\d+]?.?[\d+]?)(?<times>\*)(?<x>X)(?<power>\^[+-]?[\d+])", RegexOptions.IgnoreCase);
			Regex r = new Regex(@"([+-]?[\d+]?.?[\d+]?)\*X\^([+-]?[\d+])", RegexOptions.IgnoreCase);
			Regex noCoeff = new Regex(@"([+-]?)X\^([+-]?[\d+])", RegexOptions.IgnoreCase);
			Dictionary<Int32, Double> dict = new()
			{
				{0, 0},
				{1, 0},
				{2, 0}
			};
			String[] parts = Regex.Split(arg, @"(?=[+-])");

			foreach (String part in parts)
			{
				Match allMatch = allRegex.Match(part);
				Console.WriteLine($"allMatch groups: {allMatch.Groups}");

				if (allMatch.Success)
				{
					String coefficientString;
					String powerString;
					Double coefficient;
					Int32 power;

					// foreach (var group in allMatch.Groups)
					// {
					// 	Console.WriteLine($"group: {group}");
					// }

					coefficientString = allMatch.Groups["coefficient"].Value;
					powerString = allMatch.Groups["power"].Value;
					// Console.WriteLine($"powerstring = {powerString}, coeffString = {coefficientString}");
					if (powerString[0] == '^')
					{
						powerString = powerString.Substring(1, powerString.Length - 1);
					}
					else
					{
						throw new NotImplementedException("no ^ symbol");
					}
					power = int.Parse(powerString);

					if (!string.IsNullOrEmpty(coefficientString))
					{
						if (allMatch.Groups["times"].Value != "*")
							throw new NotImplementedException("no * symbol");
						coefficient = double.Parse(coefficientString);
					}
					else
					{
						coefficient = 1;
					}

					// Console.WriteLine($"group: {allMatch.Groups[0].Value}");
					// Console.WriteLine($"power = {power}, coefficient = {coefficient}");
					if (!dict.ContainsKey(power))
						dict[power] = 0;
					dict[power] += coefficient;
				}
				// Match match = r.Match(part);
				// Match noCoeffMatch = noCoeff.Match(part);
				// if (match.Success)
				// {
				// 	// parse it like a * x^p
				// 	Double coeff = double.Parse(match.Groups[1].Value);
				// 	Int32 power = int.Parse(match.Groups[2].Value);
				// 	if (!dict.ContainsKey(power))
				// 		dict[power] = 0;
				// 	dict[power] += coeff;
				// }
				// else if (noCoeffMatch.Success)
				// {
				// 	Double coefficient = noCoeffMatch.Groups[1].Value == "-" ? -1 : 1;
				// 	Int32 power = int.Parse(noCoeffMatch.Groups[2].Value);
				// 	if (!dict.ContainsKey(power))
				// 		dict[power] = 0;
				// 	dict[power] += coefficient;
				// }
				// else
				// {
				// 	if (part == "X" || (part.Length > 1 && part[1] == 'X' && (part[0] == '-' || part[0] == '+')))
				// 	{
				// 		// Just a lonely X
				// 		Int32 coeff = (part[0] == '-') ? -1 : 1;
				// 		dict[1] += coeff;
				// 	}
				// 	else
				// 	{
				// 		// maybe its just a regular int/float value
				// 		Int32 coeff = int.Parse(part);
				// 		dict[0] += coeff;
				// 	}
				// }
			}
			return dict;
		}

		public static Dictionary<Int32, Double> Parse(String arg)
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
	}
}
