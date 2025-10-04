using System;

namespace FoxholeBot.types
{
    public struct Coordinate
    {
        public UInt16 X = 0;
        public UInt16 Y = 0;

        public Coordinate()
        {
        }

        public static Coordinate operator +(Coordinate a, Coordinate b)
        {
            return new Coordinate() { X = (ushort)(a.X + b.X), Y = (ushort)(a.Y + b.Y) };
        }

        public static Coordinate operator -(Coordinate a, Coordinate b)
        {
            return new Coordinate() { X = (ushort)(a.X - b.X), Y = (ushort)(a.Y - b.Y) };
        }

        public bool IsNull()
        {
            return X == 0 && Y == 0;
        }
    }
    public class CoordsVector
    {
        public Coordinate start { get; set; }
        public Coordinate end { get; set; }

        public static CoordsVector operator +(CoordsVector a, CoordsVector b)
        {
            return new CoordsVector() { start = a.start + b.start, end = a.end + b.end };
        }

        public static CoordsVector operator -(CoordsVector a, CoordsVector b)
        {
            return new CoordsVector()
            {
                start = a.start - b.start,
                end = a.end - b.end
            };
        }


        /// <summary>
        /// This function calculates the vertical distance of the two coordintates
        /// </summary>
        /// <returns></returns>
        public Int32 GetVerticalDistance()
        {
            return (Int32)(end.Y - start.Y);
        }

        public Int32 GetHorizontalDistance()
        {
            return (Int32)(end.X - start.X);
        }

        /// <summary>
        /// This function calculates the distance between the two coordinates
        /// </summary>
        /// <returns>UInt16 with the hypothenuse distance</returns>
        public Int32 GetVectorDistance()
        {
            double VDistanceSquared = Math.Pow(GetVerticalDistance(), 2);
            double HDistanceSquared = Math.Pow(GetHorizontalDistance(),2);
            return (Int32)Math.Sqrt(VDistanceSquared + HDistanceSquared);
        }

        /// <summary>
        /// This function calculates the angle between the vertical axis and the hypothenuse with 0 degrees at the top
        /// </summary>
        /// <returns></returns>
        public float GetAngle()
        {
            double angleRad = Math.Atan2((double)GetHorizontalDistance(), (double)GetVectorDistance());
            double angleDeg = angleRad * (180.0 / Math.PI);
            //we do 360 minus as foxhole 0° is at the top.
            float foxholeAngle = (float)angleDeg * 2;
            if (foxholeAngle < 0)
                foxholeAngle += 360;

            return foxholeAngle;
        }
    }
}
