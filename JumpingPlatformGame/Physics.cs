namespace JumpingPlatformGame {
	struct Meters
	{
		public double Value { get; }
		public Meters(double f)
		{
			this.Value = f;
		}
		public static MeterPerSeconds operator /(Meters m, Seconds s)
		{
			return new MeterPerSeconds(m.Value / s.Value);
		}
		public static Meters operator +(Meters m1, Meters m2)
		{
			return new Meters(m1.Value + m2.Value);
		}
	}
	struct Seconds
	{
		public double Value { get; }
		public Seconds(double f)
		{
			this.Value = f;
		}
		public static Meters operator * (MeterPerSeconds mps, Seconds s)
		{
			return new Meters(mps.Value * s.Value);
		}
		public static Meters operator *(Seconds s, MeterPerSeconds mps) //commutativity
		{
			return new Meters(mps.Value * s.Value);
		}

	}
	struct MeterPerSeconds
	{
		public double Value { get; }
		public MeterPerSeconds(double f)
		{
			this.Value = f;
		}


	}
	static class Extensions
	{
		public static Meters Meters(this int f)
		{
			return new Meters(f);
		}
		public static Seconds Seconds(this double f)
		{
			return new Seconds(f);
		}
		public static MeterPerSeconds MeterPerSeconds(this double f)
		{
			return new MeterPerSeconds(f);
		}
		public static MeterPerSeconds MeterPerSeconds(this int f)
		{
			return new MeterPerSeconds(f);
		}

	}

}