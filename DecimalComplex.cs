//========================================================================
// Badger Maths Library
// Copyright (C) 2018-2025  Mike Conroy
// 
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//========================================================================

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Badger.Maths.Algebra
{
    /// <summary>
    /// The BigComplex struct is a structure which represents a complex number using System.Decimal 
    /// for the underlying real and imaginary parts
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public readonly struct DecimalComplex : ICloneable, IComparable, IComparable<DecimalComplex>, IEquatable<DecimalComplex>
    {
        #region Fields

        /// <summary>
        /// internal private field for the real part of the complex number
        /// </summary>
        private readonly decimal _real;

        /// <summary>
        /// internal private field for the imaginary part of the complex number
        /// </summary>
        private readonly decimal _imaginary;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new complex number using the supplied real and imaginary parts
        /// </summary>
        /// <param name="realpart">The real part (a) of the complex number a + bi</param>
        /// <param name="imaginarypart">The imaginary part (b) of the complex number a + bi</param>
        /// <remarks></remarks>
        public DecimalComplex(decimal realpart, decimal imaginarypart)
        {
            this._real = realpart;
            this._imaginary = imaginarypart;
        }

        /// <summary>
        /// Constructs a new complex number using the supplied BigComplex numer
        /// </summary>
        /// <param name="complex">A DecimalComplex number</param>
        /// <remarks></remarks>
        public DecimalComplex(DecimalComplex complex)
        {
            this._real = complex.Real;
            this._imaginary = complex.Imaginary;
        }

        /// <summary>
        /// Constructs a new complex number using the supplied Complex numer
        /// </summary>
        /// <param name="complex">A Complex number</param>
        /// <remarks></remarks>
        public DecimalComplex(Complex complex)
        {
            this._real = (decimal)complex.Real;
            this._imaginary = (decimal)complex.Imaginary;
        }

        #endregion

        #region Properties / Accessors

        /// <summary>
        /// The Real part of this imaginary number
        /// </summary>
        public decimal Real
        {
            get { return this._real; }
        }

        /// <summary>
        /// The Imaginary part of this imaginary number
        /// </summary>
        public decimal Imaginary
        {
            get { return this._imaginary; }
        }

        /// <summary>
        /// Returns the modulus of this DecimalComplex number
        /// The modulus is the positive real scalar which measures the distance from the origin.
        /// </summary>
        public decimal Modulus
        {
            get
            {
                Decimal result = this._real * this._real + this._imaginary * this._imaginary;
                return DecimalComplex.DecimalSqrt(result, 1e-20M);
            }
        }

        /// <summary>
        /// Returns the Argument of this BigComplex structure. The argument measures the angle that the line from the origin to the 
        /// point z makes with the real axis. It is measured in an  anticlockwise direction. The argument is returned in radians
        /// </summary>
        public decimal Argument
        {
            get
            {
                return DecimalComplex.Atan2(this);
            }
        }

        /// <summary>
        /// Gets the complex conjugate of this <see cref="ComplexNumber"/>.
        /// </summary>
        public DecimalComplex Conjugate()
        {
            return new DecimalComplex(this._imaginary, -this._real);
        }
        #endregion

        #region Static Arithmetic Operators

        /// <summary>
        /// Tests if the two supplied DecimalComplex structure instances have the same properties
        /// </summary>
        /// <param name="item1">The first instance of a DecimalComplex structure for the comparison</param>
        /// <param name="item2">The second instance of a DecimalComplex structure for the comparison</param>
        /// <returns>True if both instances have the same real and imaginary properties, false otherwise</returns>
        public static bool operator ==(DecimalComplex item1, DecimalComplex item2)
        {
            return item1.Real == item2.Real && item1.Imaginary == item2.Imaginary;
        }

        /// <summary>
        /// Tests if the two supplied DecimalComplex structure instances have different properties
        /// </summary>
        /// <param name="item1">The first instance of a DecimalComplex structure for the comparison</param>
        /// <param name="item2">The second instance of a DecimalComplex structure for the comparison</param>
        /// <returns>True if the two instances have the different real or imaginary properties, false otherwise</returns>
        public static bool operator !=(DecimalComplex item1, DecimalComplex item2)
        {
            return !(item1 == item2);
        }

        /// <summary>
        /// The unary - operator, negates the value of <paramref name="value"/>
        /// </summary>
        /// <param name="value">The <see cref="DecimalComplex"/> that will be negated</param>
        /// <returns>A negated <see cref="DecimalComplex"/></returns>
        public static DecimalComplex operator -(DecimalComplex value)
        {
            return DecimalComplex.Negate(value);
        }

        /// <summary>
        /// The unary + operator, returns the value of <paramref name="value"/>, i.e. it is a no-op
        /// </summary>
        /// <param name="value">The <see cref="DecimalComplex"/> that will be subject to the unary + operation</param>
        /// <returns><paramref name="value"/></returns>
        /// <remarks>This operator is implemented for consistency with the - operator, it is a no-op, 
        /// it does not change the value of <paramref name="value"/></remarks>
        public static DecimalComplex operator +(DecimalComplex value)
        {
            return value;
        }

        /// <summary>
        /// Adds two DecimalComplex structures
        /// </summary>
        /// <param name="item1">The first of the two DecimalComplex structure instances to sum</param>
        /// <param name="item2">The second of the two DecimalComplex structure instances to sum</param>
        /// <returns>Returns a DecimalComplex structure which represents the sum of item1 + item2</returns>
        public static DecimalComplex operator +(DecimalComplex item1, DecimalComplex item2)
        {
            return new DecimalComplex(item1.Real + item2.Real, item1.Imaginary + item2.Imaginary);
        }

        /// <summary>
        /// Subtracts two DecimalComplex structures
        /// </summary>
        /// <param name="item1">The first of the two DecimalComplex structure instances to subtract</param>
        /// <param name="item2">The second of the two DecimalComplex structure instances to subtract</param>
        /// <returns>Returns a DecimalComplex structure which represents the calculation of item1 - item2</returns>
        public static DecimalComplex operator -(DecimalComplex item1, DecimalComplex item2)
        {
            return item1 + (-item2);
        }

        /// <summary>
        /// Multiplies two DecimalComplex structures
        /// </summary>
        /// <param name="item1">The first of the two DecimalComplex structure instances to multiply</param>
        /// <param name="item2">The second of the two DecimalComplex structure instances to multiply</param>
        /// <returns>Returns a DecimalComplex structure which represents the calculation of item1 * item2</returns>
        public static DecimalComplex operator *(DecimalComplex item1, DecimalComplex item2)
        {
            decimal real = item1.Real * item2.Real - item1.Imaginary * item2.Imaginary;
            decimal imaginary = item1.Imaginary * item2.Real + item1.Real * item2.Imaginary;

            return new DecimalComplex(real, imaginary);
        }

        /// <summary>
        /// Divides two DecimalComplex structures
        /// </summary>
        /// <param name="item1">The first of the two DecimalComplex structure instances to divide</param>
        /// <param name="item2">The second of the two DecimalComplex structure instances to divide</param>
        /// <returns>Returns a DecimalComplex structure which represents the calculation of item1 / item2</returns>
        public static DecimalComplex operator /(DecimalComplex item1, DecimalComplex item2)
        {
            decimal denominator = (item2.Real * item2.Real) + (item2.Imaginary * item2.Imaginary);
            decimal real = (item1.Real * item2.Real + item1.Imaginary * item2.Imaginary) / denominator;
            decimal imaginary = (item1.Imaginary * item2.Real - item1.Real * item2.Imaginary) / denominator;

            return new DecimalComplex(real, imaginary);
        }

        #endregion

        #region Static Arithmetic Methods

        /// <summary>
        /// Negates the supplied <see cref="DecimalComplex"/> structure
        /// </summary>
        /// <param name="value">The <see cref="DecimalComplex"/> to be negated</param>
        /// <returns>A new <see cref="DecimalComplex"/> struct that is the negative of <paramref name="value"/></returns>
        /// 
        public static DecimalComplex Negate(DecimalComplex value)
        {
            return new DecimalComplex(-value.Real, -value.Imaginary);
        }

        /// <summary>
        /// Calculates the atan2 value of the supplied DecimalComplex number
        /// </summary>
        /// <param name="value">The <see cref="DecimalComplex"/> for which the atan2 will be calculated</param>
        /// <returns>The 2 argument arctangent of a complex number</returns>
        /// <remarks>The calculation follows correct quadrant-based angle calculation by using sign checks</remarks>
        public static decimal Atan2(DecimalComplex value)
        {
            if (value.Real == 0 && value.Imaginary == 0)
            {
                throw new ArgumentException("Undefined angle for (0,0i)");
            }

            // Calculate the absolute value of the atan
            decimal absAtan = DecimalComplex.Atan(value.Imaginary / value.Real);
            decimal DecimalPI = 22M / 7M;

            if (value.Real > 0)
            {
                return absAtan; // First and fourth quadrant
            }
            else if (value.Imaginary >= 0)
            {
                return absAtan + DecimalPI; // Second quadrant
            }
            else
            {
                return absAtan - DecimalPI; // Third quadrant
            }
        }

        /// <summary>
        /// Calculates the arctangent of an angle expressed as a decimal number using Taylor series approximation
        /// </summary>
        /// <param name="value">The angle of which the arctangent wil be calculated</param>
        /// <returns>The arctangent of <paramref name="value"/></returns>
        private static decimal Atan(decimal value)
        {
            // Taylor series approximation for arctan(x)
            const int iterations = 50;
            decimal result = value;
            decimal term = value;
            decimal xSquared = value * value;

            for (int i = 1; i < iterations; i++)
            {
                term *= xSquared;
                decimal fraction = term / (2 * i + 1);
                result += (i % 2 == 0) ? fraction : -fraction;
            }

            return result;
        }

        /// <summary>
        /// Calculates the square root of a <see cref="Decimal"/> using the Newton-Raphson method
        /// </summary>
        /// <param name="value">The <see cref="Decimal"/> to calculate the square root of</param>
        /// <param name="precision"></param>
        /// <returns>The square root of <paramref name="value"/></returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is negative</exception>
        /// <remarks>Calculates the square root to within a given precision, if not specified the 
        /// precision is fixed at 0.0000000000000000000001</remarks>
        private static decimal DecimalSqrt(decimal value, decimal precision = 0.0000000000000000000001M)
        {
            if (value < 0)
            {
                throw new ArgumentException("Cannot calculate the square root of a negative number.", nameof(value));
            }
            if (value == 0 || value == 1)
            {
                return value;
            }

            decimal guess = value / 2;
            decimal previousGuess;

            do
            {
                previousGuess = guess;
                guess = (previousGuess + value / previousGuess) / 2;
            }
            while (Math.Abs(previousGuess - guess) > precision);

            return guess;
        }

        /// <summary>
        /// Tests if the two supplied <see cref="DecimalComplex"/> structure instances have the same properties
        /// </summary>
        /// <param name="left">The first instance of an <see cref="DecimalComplex"/> struct for the comparison</param>
        /// <param name="right">The second instance of an <see cref="DecimalComplex"/> struct for the comparison</param>
        /// <returns><c>True</c> if the two instances have the same <see cref="DecimalComplex.Real"/> and 
        /// <see cref="DecimalComplex.Imaginary"/> properties</returns>
        public static bool IsEqual(DecimalComplex left, DecimalComplex right)
        {
            return left == right;
        }

        #endregion

        #region ICloneable Support

        /// <summary>
        /// Creates a new instance of a DecimalComplex structure which has the same real and imaginary properties as this instance
        /// </summary>
        /// <returns>A deep copy of this instance of a DecimalComplex structure with identical real and imaginary properties</returns>
        public Object Clone()
        {
            return new DecimalComplex(this);
        }
        #endregion

        #region IComparable Support

        /// <summary>
        /// Compares this instance of a DecimalComplex structure to a specified object instance and returns an integer that indicates whether the value of this instance is less than, 
        /// equal to, or greater than the value of the specified object instance
        /// </summary>
        /// The object to compare with this instance of a DecimalComplex structure<param name="obj"></param>
        /// <returns>Less than zero if ??????, zero if they are the same, and greater than zero if ?????</returns>
        public int CompareTo(object? obj)
        {
            if ((obj == null))
            {
                throw new ArgumentNullException(nameof(obj), "The object you have tried to compare to this instance is Null (Nothing in VB).");
            }
            else if ((!object.ReferenceEquals(this.GetType(), obj.GetType())))
            {
                try
                {
                    return this.CompareTo((DecimalComplex)obj);
                }
                catch (InvalidCastException ex)
                {
                    throw new InvalidCastException("The object you provided cannot be cast to an DecimalComplex object.", ex);
                }
            }
            else
            {
                return this.CompareTo((DecimalComplex)obj);
            }
        }

        /// <summary>
        /// Compares this instance of a DecimalComplex structure to a specified object instance and returns an integer that indicates whether the value of this instance is less than, 
        /// equal to, or greater than the value of the specified object instance
        /// </summary>
        /// The object to compare with this instance of a DecimalComplex structure<param name="obj"></param>
        /// <returns>Less than zero if this instance is closer to the origin than the supplied version, zero if they are the same distance, and greater than zero if this 
        /// instance is farther from the origin than the supplied version (the Modulus property is used)</returns>
        public int CompareTo(DecimalComplex other)
        {
            // Multiply by 100000 in order to increase differentiation
            return Convert.ToInt32((this.Modulus - other.Modulus) * 100000);
        }
        #endregion

        #region IEquatable Support

        /// <summary>
        /// Tests if the supplied object, <paramref name="obj">obj</paramref>, is an instance of a DecimalComplex structure and if so tests whether it has the same properties as this
        /// instance of a DecimalComplex structure
        /// </summary>
        /// <param name="obj">A DecimalComplex object to test for property equivalence with this instance</param>
        /// <returns><c>True</c> if both instances have the same real and imaginary properties, <c>False</c> otherwise</returns>
        /// <remarks>Internally this method uses the equality operator, ==</remarks>
        public override bool Equals(object? obj)
        {
            if ((obj == null) || (!object.ReferenceEquals(this.GetType(), obj.GetType())))
                return false;
            return (this == (DecimalComplex)obj);
        }

        /// <summary>
        /// Tests if the supplied object has the same properties as this instance of a DecimalComplex structure
        /// </summary>
        /// <param name="obj">A DecimalComplex object to test for property equivalence with this instance</param>
        /// <returns><c>True</c> if both instances have the same real and imaginary properties, <c>False</c> otherwise</returns>
        /// <remarks>Internally this method uses the equality operator, ==</remarks>
        public bool Equals(DecimalComplex other)
        {
            return this == other;
        }

        #endregion

        #region Object Methods

        /// <summary>
        /// Returns a hash code for this instance of a DecimalComplex structure
        /// </summary>
        /// <returns>A single hash code calculated from the individual hash codes of the real and imaginary parts</returns>
        public override int GetHashCode() => HashCode.Combine(this._real.GetHashCode(), this._imaginary.GetHashCode());

        /// <summary>
        /// Returns a string representation of this DecimalComplex structure
        /// </summary>
        /// <returns>A <see cref="string"/> representation of this <see cref="DecimalComplex"/></returns>
        public override string ToString()
        {
            // Copied from Science Library SCI, https://sourceforge.net/projects/scinet/?source=typ_redirect

            StringBuilder z = new();
            z.Append('(');
            z.Append(this._real);

            if (this._imaginary > 0)
                z.Append(" + ");
            else if (this._imaginary < 0)
                z.Append(" - ");

            if (this._imaginary != 0m)
            {
                z.Append(BigFloat.Abs(new BigFloat(this._imaginary)).ToString());
                z.Append('i');
            }
            z.Append(')');

            return z.ToString();
        }
        #endregion

    }
}
