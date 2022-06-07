using System.Text.RegularExpressions;

namespace Computorv1
{
	class Parsing
	{
		static Dictionary<Int32, Double> ParseSide(String arg)
		{
			Regex r = new Regex(@"([+-]?[\d+]?.?[\d+]?)\*X\^([+-]?[\d+])", RegexOptions.IgnoreCase);
			Regex no_coeff = new Regex(@"([+-]?)X\^([+-]?[\d+])", RegexOptions.IgnoreCase);
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
				Match no_coeff_match = no_coeff.Match(part);
				if (match.Success)
				{
					// parse it like a * x^p
					Double coeff = double.Parse(match.Groups[1].Value);
					Int32 power = int.Parse(match.Groups[2].Value);
					if (!dict.ContainsKey(power))
						dict[power] = 0;
					dict[power] += coeff;
				}
				else if (no_coeff_match.Success)
				{
					Double coefficient = no_coeff_match.Groups[1].Value == "-" ? -1 : 1;
					Int32 power = int.Parse(no_coeff_match.Groups[2].Value);
					if (!dict.ContainsKey(power))
						dict[power] = 0;
					dict[power] += coefficient;
				}
				else
				{
					if (part == "X" || (part.Length > 1 && part[1] == 'X' && (part[0] == '-' || part[0] == '+')))
					{
						// Just a lonely X
						Int32 coeff = (part[0] == '-') ? -1 : 1;
						dict[1] += coeff;
					}
					else
					{
						// maybe its just a regular int/float value
						Int32 coeff = int.Parse(part);
						dict[0] += coeff;
					}
				}
			}
			return dict;
		}

		static public Dictionary<Int32, Double> Parse(String arg)
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
