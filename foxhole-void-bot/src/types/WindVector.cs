using System;

namespace FoxholeBot.types
{
    /// <summary>
    /// This class represents the foxhole wind vector.
    /// </summary>
    public class WindVector
    {
        ArtyGun ArtyGun { get; set; }
        WindDirection Direction { get; set; }
        WindStrenght WindStrenght { get; set; }
        /// <summary>
        /// This constructor create a windvector
        /// </summary>
        /// <param name="gun"></param>
        /// <param name="direction"></param>
        /// <param name="strenght"></param>
        /// <param name="pixelSize"></param>
        public WindVector(ArtyGun gun, WindDirection direction, WindStrenght strenght)
        {
            this.ArtyGun = gun;
            this.Direction = direction;
            this.WindStrenght = strenght;
        }

        /// <summary>
        /// This function caluculates the wind offset distance for the provided gun model
        /// </summary>
        /// <returns>(double) distance</returns>
        private double GetDistance() => (UInt16)ArtyGun.Bias * (UInt16)WindStrenght;

        /// <summary>
        /// This function calculates and returns the horizontal distance
        /// </summary>
        /// <returns>(double) horizontal distance</returns>
        private double GetHorizontalDistance()
        {
            return Math.Sin((double)Direction) * GetDistance();
        }

        /// <summary>
        /// This function calculates and returns the vertical distance
        /// </summary>
        /// <returns>(double) vertical distance</returns>
        private double GetVerticalDistance()
        {
            return Math.Cos((double)Direction) * GetDistance();
        }

        /// <summary>
        /// This function converts the WindVector into a CoordsVector;
        /// </summary>
        /// <param name="meterToPixel"></param>
        /// <returns></returns>
        public CoordsVector GetCoordsVector(float meterToPixel) 
        {
            Coordinate start = new Coordinate() { X = 0, Y = 0 };

            UInt16 horizontalDistance = (UInt16)(GetHorizontalDistance() * meterToPixel);
            UInt16 verticalDistance = (UInt16)(GetVerticalDistance() * meterToPixel);

            Coordinate end = new Coordinate() { X = horizontalDistance, Y = verticalDistance};

            return new CoordsVector() { start = start, end = end };
        }
    }
}
