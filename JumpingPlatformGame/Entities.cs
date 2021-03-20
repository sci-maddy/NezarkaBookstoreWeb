using System;
using System.Data.Common;
using System.Drawing;
using System.Threading;

namespace JumpingPlatformGame {
	class Entity {
		public virtual Color Color => Color.Black;
		public WorldPoint Location;
		public virtual void Update(Seconds sec)
		{

		}
	}

	class MovableEntity : Entity {
		public Horizontal Horizontal = new Horizontal(); //horizontal data - speed and walls
		public void MoveHorizontally(Seconds deltaSeconds)
		{
			Meters deltaMeters = Horizontal.Speed * deltaSeconds;
			Location.X += new Meters(deltaMeters.Value);
			if (Location.X.Value > Horizontal.UpperBound.Value)//bounce, location is behind walls, recalculate position
			{
				Location.X = new Meters(Horizontal.UpperBound.Value - (Location.X.Value - Horizontal.UpperBound.Value)); 
				Horizontal.Speed = new MeterPerSeconds(-Horizontal.Speed.Value); //direction change
			}
			if (Location.X.Value < Horizontal.LowerBound.Value)
			{
				Location.X = new Meters(Horizontal.LowerBound.Value + (Location.X.Value + Horizontal.LowerBound.Value));
				Horizontal.Speed = new MeterPerSeconds(-Horizontal.Speed.Value);
			}
		}
		public override void Update(Seconds deltaSeconds)
		{
			MoveHorizontally(deltaSeconds);
		}
	}

	class MovableJumpingEntity : MovableEntity {
		public Vertical Vertical = new Vertical(); //vertical data - speed and walls
		public void MoveVertically(Seconds deltaSeconds)
		{
			Meters deltaMeters = Vertical.Speed * deltaSeconds;
			Location.Y += new Meters(deltaMeters.Value);
			if (Location.Y.Value > Vertical.UpperBound.Value) //bounce
			{
				Location.Y = new Meters(Vertical.UpperBound.Value - (Location.Y.Value - Vertical.UpperBound.Value));
				Vertical.Speed = new MeterPerSeconds(-Vertical.Speed.Value); // changes direction
			}
			if (Location.Y.Value < Vertical.LowerBound.Value)
			{
				Location.Y = new Meters(Vertical.LowerBound.Value); //sticks to ground
			}
		}
		public override void Update(Seconds deltaSeconds)
		{
			MoveHorizontally(deltaSeconds);
			MoveVertically(deltaSeconds);
		}
	}
	class Horizontal
	{
		public Meters LowerBound;
		public Meters UpperBound;
		public MeterPerSeconds Speed;
	}
	class Vertical
	{
		public Meters LowerBound;
		public Meters UpperBound;
		public MeterPerSeconds Speed;
	}
	class WorldPoint
	{
		public Meters X;
		public Meters Y;
	}

	class Joe : MovableEntity {
		public override string ToString() => "Joe";
		public override Color Color => Color.Blue;
	}

	class Jack : MovableEntity {
		public override string ToString() => "Jack";
		public override Color Color => Color.LightBlue;
	}

	class Jane : MovableJumpingEntity {
		public override string ToString() => "Jane";
		public override Color Color => Color.Red;
	}

	class Jill : MovableJumpingEntity {
		public override string ToString() => "Jill";
		public override Color Color => Color.Pink;
	}

}