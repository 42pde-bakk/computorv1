using System.Text.RegularExpressions;

namespace Computorv1
{
	internal static class Parsing
	{
		static Dictionary<Int32, Double> ParseSide(String arg)
		{
			Regex allRegex = new Regex(@"^(?<coefficient>[+-]?[\d+]?.?[\d+]?\*?)?(?<x>X)(?<power>\^[+-]?[\d+])?$", RegexOptions.IgnoreCase);
			Regex coeffOnlyRegex = new Regex(@"^([+-]?[\d+].?[\d+]?)$", RegexOptions.IgnoreCase);
			// Regex noCoeff = new Regex(@"([+-]?)X\^([+-]?[\d+])", RegexOptions.IgnoreCase);
			Dictionary<Int32, Double> dict = new()
			{
				{0, 0},
				{1, 0},
				{2, 0}
			};
			String[] parts = Regex.Split(arg, @"(?=[+-])");

			foreach (String part in parts)
			{
				// Console.WriteLine($"trying to match {part}");
				Match allMatch = allRegex.Match(part);
				Match coeffOnlyMatch = coeffOnlyRegex.Match(part);

				// Console.WriteLine($"part = {part}, success? : {allMatch.Success}");

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

					if (!string.IsNullOrEmpty(powerString) && powerString[0] == '^')
					{
						powerString = powerString.Substring(1, powerString.Length - 1);
						power = int.Parse(powerString);
					}
					else
					{
						power = 0;
						// throw new NotImplementedException("no ^ symbol");
					}
					// power = int.Parse(powerString);

					if (!string.IsNullOrEmpty(coefficientString))
					{
						// Console.WriteLine($"coeffString = {coefficientString}");
						if (coefficientString is "+" or "-")
						{
							coefficientString = String.Concat(coefficientString, "1");
						}
						if (coefficientString[^1] == '*')
						{
							coefficientString = coefficientString.Substring(0, coefficientString.Length - 1);
							// Console.WriteLine($"NEW coeffString = {coefficientString}");
						}
						// Console.WriteLine($"coeffString = {coefficientString}");
						// if (allMatch.Groups["times"].Value != "*")
						// 	throw new NotImplementedException("no * symbol");
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
				else if (coeffOnlyMatch.Success)
				{
					// Console.WriteLine($"coeffOnlyMatch = Success, part = {part}");
					Int32 coefficient = int.Parse(part);
					Int32 power = 0;

					dict[power] += coefficient;
				}
				else
				{
					// Console.WriteLine("shitters clogged");
					throw new ArgumentException("cant match part");
				}
			}
			return dict;
		}

		public static Dictionary<Int32, Double> Parse(String arg)
		{
			String arg2 = Regex.Replace(arg, @"\s+", "");
			String[] splitted = arg2.Split('=');
			Dictionary<Int32, Double> lhs = ParseSide(splitted[0]);
			Dictionary<Int32, Double> rhs = ParseSide(splitted[1]);
			if (string.IsNullOrWhiteSpace(splitted[0]) || string.IsNullOrWhiteSpace(splitted[1]))
			{
				throw new ArgumentException("Empty string on either side of =sign");
			}

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
