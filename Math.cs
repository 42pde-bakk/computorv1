namespace Computorv1
{
	internal static class MyMath
	{
		public static Double Abs(Double nb)
		{
			return (nb < 0) ? nb * -1 : nb;
		}

		public static Double Sqrt(Double nb)
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

		public static Int32 Gcd(Int32 a, Int32 b)
		{
			while (b > 0)
			{
				Int32 rem = a % b;
				a = b;
				b = rem;
			}
			return a;
		}

		public static Int32 Max(Int32 a, Int32 b)
		{
			return (a > b ? a : b);
		}

		public static Boolean IsInteger(Double d)
		{
			return (Math.Abs(d - (Int32) d) < Double.Epsilon);
		}
	}
}
