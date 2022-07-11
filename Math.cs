// ReSharper disable All
namespace Computorv1
{
	internal static class MyMath
	{
		public static double Abs(double nb)
		{
			return (nb < 0) ? nb * -1 : nb;
		}

		public static double Sqrt(double nb)
		{
			if (nb <= 0)
			{
				return (double.NaN);
			}
			double root = nb / 3;
			int i;
			for (i = 0; i < 32; i++)
			{
				root = (root + nb / root) / 2;
			}
			return (root);
		}

		public static int Gcd(int a, int b)
		{
			while (b > 0)
			{
				int rem = a % b;
				a = b;
				b = rem;
			}
			return a;
		}

		public static int Max(int a, int b)
		{
			return (a > b ? a : b);
		}

		public static bool IsInteger(double d)
		{
			return (Math.Abs(d - (int) d) < double.Epsilon);
		}
	}
}
