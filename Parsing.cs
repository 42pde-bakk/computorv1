using System.Text.RegularExpressions;

namespace Computorv1
{
	internal static class Parsing
	{
		static Dictionary<int, double> ParseSide(string arg)
		{
			Regex allRegex = new Regex(@"^(?<coefficient>[+-]?[\d+]?.?[\d+]?\*?)?(?<x>X)(?<power>\^[+-]?[\d+])?$", RegexOptions.IgnoreCase);
			Regex coeffOnlyRegex = new Regex(@"^([+-]?[\d+]?.?[\d+]?)$", RegexOptions.IgnoreCase);
			Dictionary<int, double> dict = new()
			{
				{0, 0},
				{1, 0},
				{2, 0}
			};
			string[] parts = Regex.Split(arg, @"(?=[+-])");

			foreach (string part in parts)
			{
				if (parts.Length > 1 && string.IsNullOrEmpty(part))
				{
					continue;
				}
				Match allMatch = allRegex.Match(part);
				Match coeffOnlyMatch = coeffOnlyRegex.Match(part);

				if (allMatch.Success)
				{
					double coefficient;
					int power;

					string coefficientString = allMatch.Groups["coefficient"].Value;
					string powerString = allMatch.Groups["power"].Value;

					if (!string.IsNullOrEmpty(powerString) && powerString[0] == '^')
					{
						powerString = powerString.Substring(1, powerString.Length - 1);
						power = int.Parse(powerString);
					}
					else
					{
						power = 0;
					}

					if (!string.IsNullOrEmpty(coefficientString))
					{
						if (coefficientString is "+" or "-")
						{
							coefficientString = string.Concat(coefficientString, "1");
						}
						if (coefficientString[^1] == '*')
						{
							coefficientString = coefficientString.Substring(0, coefficientString.Length - 1);
						}
						coefficient = double.Parse(coefficientString);
					}
					else
					{
						coefficient = 1;
					}

					if (!dict.ContainsKey(power))
						dict[power] = 0;
					dict[power] += coefficient;
				}
				else if (coeffOnlyMatch.Success)
				{
					int coefficient = int.Parse(part);
					const int power = 0;

					dict[power] += coefficient;
				}
				else
				{
					throw new ArgumentException("cant match part");
				}
			}
			return dict;
		}

		public static Dictionary<int, double> Parse(string arg)
		{
			string arg2 = Regex.Replace(arg, @"\s+", "");
			string[] splitted = arg2.Split('=');
			Dictionary<int, double> lhs = ParseSide(splitted[0]);
			Dictionary<int, double> rhs = ParseSide(splitted[1]);
			if (string.IsNullOrWhiteSpace(splitted[0]) || string.IsNullOrWhiteSpace(splitted[1]))
			{
				throw new ArgumentException("Empty string on either side of =sign");
			}

			foreach ((int key, double value) in rhs)
			{
				if (!lhs.ContainsKey(key))
				{
					lhs[key] = 0;
				}
				lhs[key] -= value;
			}

			foreach ((int key, var _) in lhs.Where(kvp => kvp.Value == 0).ToList())
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
