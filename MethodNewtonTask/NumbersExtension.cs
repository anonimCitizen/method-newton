using System;

namespace MethodNewtonTask
{
    public static class NumbersExtension
    {
        /// <summary>
        /// Initializes static members of the <see cref="NumbersExtension"/> class.
        /// </summary>
        public static readonly AppSettings AppSettings = new AppSettings { Epsilon = double.Epsilon };

        /// <summary>
        /// Find n-th root of number with the given accuracy.
        /// </summary>
        /// <param name="number">Source number.</param>
        /// <param name="degree">Root degree.</param>
        /// <param name="accuracy">Accuracy value.</param>
        /// <returns> n-th root of number.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when number is negative and n degree is even.
        /// -or- 
        /// degree is less than zero
        /// -or-
        /// number is NaN, double.NegativeInfinity, double.PositiveInfinity.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when accuracy is less than zero.
        /// -or- 
        /// accuracy is more than `Epsilon`.
        /// </exception>
        public static double FindNthRoot(double number, int degree, double accuracy)
        {
            double x = number;
            double xPrev;
            if (degree % 2 == 0 && number < 0)
            {
                throw new ArgumentException();
            }

            if (degree < 0)
            {
                throw new ArgumentException();
            }

            if (double.IsNegativeInfinity(number) || double.IsPositiveInfinity(number) || double.IsNaN(number))
            {
                throw new ArgumentException();  
            }

            if (accuracy < 0 || accuracy > AppSettings.Epsilon)
            {
                throw new ArgumentOutOfRangeException();
            }


            do
            {
                xPrev = x;
                x = ((double)1 / degree) * ((double)(degree - 1) * xPrev + (double)number / (double)Math.Pow(xPrev, degree - 1));
            } while (Math.Abs(x - xPrev) >= AppSettings.Epsilon); 
            return x;
        }
    }
}
