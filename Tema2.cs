// implementati o clasa pentru lucrul cu numere complexe.

using System;

namespace NumberFormats
{
    public class Complex
    {
        #region Constructors
        public Complex()
        {
            this.Real = 0;
            this.Imaginary = 0;
        }

        public Complex(double real, double imaginary)
        {
            this.Real = real;
            this.Imaginary = imaginary;
        }
        #endregion

        #region Operators
        public static Complex operator +(Complex A, Complex B)
        {
            return new Complex(A.Real + B.Real, A.Imaginary + B.Imaginary);
        }

        public static Complex operator -(Complex A, Complex B)
        {
            return new Complex(A.Real - B.Real, A.Imaginary - B.Imaginary);
        }

        public static Complex operator *(Complex A, Complex B)
        {
            return new Complex(
                A.Real * B.Real - A.Imaginary * B.Imaginary,
                A.Real * B.Imaginary + A.Imaginary * B.Real
            );
        }

        public static Complex operator /(Complex A, Complex B)
        {
            double denominator = B.Real * B.Real + B.Imaginary * B.Imaginary;
            if (denominator == 0)
                throw new DivideByZeroException("Cannot divide by zero complex number");

            return new Complex(
                (A.Real * B.Real + A.Imaginary * B.Imaginary) / denominator,
                (A.Imaginary * B.Real - A.Real * B.Imaginary) / denominator
            );
        }

        public static bool operator ==(Complex A, Complex B)
        {
            return (A.Real == B.Real) && (A.Imaginary == B.Imaginary);
        }

        public static bool operator !=(Complex A, Complex B)
        {
            return !(A == B);
        }

        public override bool Equals(object obj)
        {
            if (obj is Complex other)
                return this == other;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Real, Imaginary);
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            if (Imaginary == 0)
                return Real.ToString();
            if (Real == 0)
                return $"{Imaginary}i";
            
            string sign = Imaginary > 0 ? "+" : "";
            return $"{Real}{sign}{Imaginary}i";
        }

        public double Magnitude()
        {
            return Math.Sqrt(Real * Real + Imaginary * Imaginary);
        }

        public Complex Conjugate()
        {
            return new Complex(Real, -Imaginary);
        }

        public static Complex FromPolar(double magnitude, double angle)
        {
            return new Complex(
                magnitude * Math.Cos(angle),
                magnitude * Math.Sin(angle)
            );
        }

        public double ToAngle()
        {
            return Math.Atan2(Imaginary, Real);
        }
        #endregion

        #region Properties
        public double Real { get; set; }
        public double Imaginary { get; set; }
        #endregion

        #region Static Properties
        public static Complex Zero => new Complex(0, 0);
        public static Complex One => new Complex(1, 0);
        public static Complex ImaginaryOne => new Complex(0, 1);
        #endregion
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testarea clasei Complex:");
            
            Complex a = new Complex(3, 4);  // 3+4i
            Complex b = new Complex(1, -2); // 1-2i

            Console.WriteLine($"a = {a}");
            Console.WriteLine($"b = {b}");
            Console.WriteLine($"a + b = {a + b}");
            Console.WriteLine($"a - b = {a - b}");
            Console.WriteLine($"a * b = {a * b}");
            Console.WriteLine($"a / b = {a / b}");
            Console.WriteLine($"Modulul lui a: {a.Magnitude()}");
            Console.WriteLine($"Conjugatul lui b: {b.Conjugate()}");

            Console.WriteLine("\nApasă orice tastă pentru a ieși...");
            Console.ReadKey();
        }
    }
}