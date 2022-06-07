namespace Computorv1
{
	class MyMath
	{
		static public Double Abs(Double nb)
		{
			return (nb < 0) ? nb * -1 : nb;
		}

		static public Double Sqrt(Double nb)
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

		static public Int32 GCD(Int32 a, Int32 b)
		{
			while (b > 0)
			{
				Int32 rem = a % b;
				a = b;
				b = rem;
			}
			return a;
		}

		static public Int32 Max(Int32 a, Int32 b)
		{
			return (a > b ? a : b);
		}

		static public bool isInteger(Double d)
		{
			return (d == (int) d);
		}
	}
}
