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
using System.Text;

namespace Badger.Maths.Algebra
{
    /// <summary>
    /// The BigComplex struct is a structure which represents a complex number using Badger.Maths BigFloat class 
    /// for the underlying real and imaginary parts
    /// </summary>

    public readonly struct BigComplex : ICloneable, IComparable, IComparable<BigComplex>, IEquatable<BigComplex>
    {
        #region Fields

        /// <summary>
        /// internal private field for the real part of the complex number
        /// </summary>
        private readonly BigFloat _real;

        /// <summary>
        /// internal private field for the imaginary part of the complex number
        /// </summary>
        private readonly BigFloat _imaginary;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new complex number using the supplied real and imaginary parts
        /// </summary>
        /// <param name="realpart">The real part (a) of the complex number a + bi</param>
        /// <param name="imaginarypart">The imaginary part (b) of the complex number a + bi</param>
        /// <remarks></remarks>
        public BigComplex(BigFloat realpart, BigFloat imaginarypart)
        {
            this._real = realpart;
            this._imaginary = imaginarypart;
        }

        /// <summary>
        /// Constructs a new complex number using the supplied BigComplex numer
        /// </summary>
        /// <param name="complex">A BigComplex number</param>
        /// <remarks></remarks>
        public BigComplex(BigComplex complex)
        {
            this._real = complex.Real;
            this._imaginary = complex.Imaginary;
        }

        /// <summary>
        /// Constructs a new complex number using the supplied Complex numer
        /// </summary>
        /// <param name="complex">A Complex number</param>
        /// <remarks></remarks>
        public BigComplex(Complex complex)
        {
            this._real = new BigFloat(complex.Real);
            this._imaginary = new BigFloat(complex.Imaginary);
        }

        #endregion

        #region Properties / Accessors

        /// <summary>
        /// The Real part of this imaginary number
        /// </summary>
        public BigFloat Real
        {
            get { return this._real; }
        }

        /// <summary>
        /// The Imaginary part of this imaginary number
        /// </summary>
        public BigFloat Imaginary
        {
            get { return this._imaginary; }
        }

        /// <summary>
        /// Returns the modulus of this BigComplex number
        /// The modulus is the positive real scalar which measures the distance from the origin
        /// </summary>
        /// <remarks>This method uses <see cref="BigFloat.Sqrt(BigFloat, double)"/> which 
        /// requires a tolerance to be provided, this is set to 1e-20 by this method</remarks>
        public BigFloat Modulus
        {
            get
            {
                BigFloat result = this._real * this._real + this._imaginary * this._imaginary;
                return BigFloat.Sqrt(result, 1e-20);
            }
        }

        /// <summary>
        /// Returns the Argument of this BigComplex structure. The argument measures the angle that the line from the origin to the 
        /// point z makes with the real axis. It is measured in an anticlockwise direction. The argument is returned in radians
        /// </summary>
        public BigFloat Argument
        {
            get { return BigComplex.Atan2(this); }
        }

        /// <summary>
        /// Gets the complex conjugate of this <see cref="ComplexNumber"/>
        /// </summary>
        public BigComplex Conjugate()
        {
            return new BigComplex(this._imaginary, -this._real);
        }
        #endregion

        #region Static Arithmetic Operators

        /// <summary>
        /// Compares the two supplied BigComplex Structures and returns <c>true</c> if the first is less than the second
        /// </summary>
        /// <param name="left">The first BigComplex structure in the equation "left < right"</param>
        /// <param name="right">The second BigComplex structure in the equation "left < right"</param>
        /// <returns></returns>
        /// <remarks>Internally this method uses the <see cref="BigComplex.CompareTo(BigComplex)"> 
        /// method and hence the comparison is based on proximity to the origin which is determined via the Modulus property</remarks>
        public static bool operator <(BigComplex left, BigComplex right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Compares the two supplied BigComplex Structures and returns <c>true</c> if the first is less than or equal to the second
        /// </summary>
        /// <param name="left">The first BigComplex structure in the equation "left <= right"</param>
        /// <param name="right">The second BigComplex structure in the equation "left <= right"</param>
        /// <returns>Internally this method uses the <see cref="BigComplex.CompareTo(BigComplex)"> 
        /// method and hence the comparison is based on proximity to the origin which is determined via the Modulus property</returns>
        public static bool operator <=(BigComplex left, BigComplex right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Compares the two supplied BigComplex Structures and returns <c>true</c> if the first is greater than the second
        /// </summary>
        /// <param name="left">The first BigComplex structure in the equation "left > right"</param>
        /// <param name="right">The second BigComplex structure in the equation "left > right"</param>
        /// <returns></returns>
        /// <remarks>Internally this method uses the <see cref="BigComplex.CompareTo(BigComplex)"> 
        /// method and hence the comparison is based on proximity to the origin which is determined via the Modulus property</remarks>
        public static bool operator >(BigComplex left, BigComplex right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Compares the two supplied BigComplex Structures and returns <c>true</c> if the first is greater than or equal to the second
        /// </summary>
        /// <param name="left">The first BigComplex structure in the equation "left >= right"</param>
        /// <param name="right">The second BigComplex structure in the equation "left >= right"</param>
        /// <returns>Internally this method uses the <see cref="BigComplex.CompareTo(BigComplex)"> 
        /// method and hence the comparison is based on proximity to the origin which is determined via the Modulus property</returns>
        public static bool operator >=(BigComplex left, BigComplex right)
        {
            return left.CompareTo(right) >= 0;
        }

        /// <summary>
        /// Tests if the two supplied BigComplex structure instances have the same properties
        /// </summary>
        /// <param name="left">The first instance of a BigComplex structure for the comparison</param>
        /// <param name="right">The second instance of a BigComplex structure for the comparison</param>
        /// <returns>True if both instances have the same real and imaginary properties, false otherwise</returns>
        public static bool operator ==(BigComplex left, BigComplex right)
        {
            return left.Real == right.Real && left.Imaginary == right.Imaginary;
        }

        /// <summary>
        /// Tests if the two supplied BigComplex structure instances have different properties
        /// </summary>
        /// <param name="left">The first instance of a BigComplex structure for the comparison</param>
        /// <param name="right">The second instance of a BigComplex structure for the comparison</param>
        /// <returns>True if the two instances have the different real or imaginary properties, false otherwise</returns>
        public static bool operator !=(BigComplex left, BigComplex right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Unary negation operator. Negates the given BigComplex
        /// </summary>
        /// <param name="z">The BigComplex structure to be negated</param>
        /// <returns>
        /// Returns a BigComplex structure which is the negative equavalent of the given BigComplex Structure
        /// </returns>
        public static BigComplex operator -(BigComplex value)
        {
            return new BigComplex(-value.Real, -value.Imaginary);
        }

        /// <summary>
        /// Adds two BigComplex structures
        /// </summary>
        /// <param name="left">The first of the two BigComplex structure instances to sum</param>
        /// <param name="right">The second of the two BigComplex structure instances to sum</param>
        /// <returns>Returns a BigComplex structure which represents the sum of left + right</returns>
        public static BigComplex operator +(BigComplex left, BigComplex right)
        {
            return new BigComplex(left.Real + right.Real, left.Imaginary + right.Imaginary);
        }

        /// <summary>
        /// Subtracts two BigComplex structures
        /// </summary>
        /// <param name="left">The first of the two BigComplex structure instances to subtract</param>
        /// <param name="right">The second of the two BigComplex structure instances to subtract</param>
        /// <returns>Returns a BigComplex structure which represents the calculation of left - right</returns>
        public static BigComplex operator -(BigComplex left, BigComplex right)
        {
            return left + (-right);
        }

        /// <summary>
        /// Multiplies two BigComplex structures
        /// </summary>
        /// <param name="left">The first of the two BigComplex structure instances to multiply</param>
        /// <param name="right">The second of the two BigComplex structure instances to multiply</param>
        /// <returns>Returns a BigComplex structure which represents the calculation of left * right</returns>
        public static BigComplex operator *(BigComplex left, BigComplex right)
        {
            BigFloat real = left.Real * right.Real - left.Imaginary * right.Imaginary;
            BigFloat imaginary = left.Imaginary * right.Real + left.Real * right.Imaginary;

            return new BigComplex(real, imaginary);
        }

        /// <summary>
        /// Divides two BigComplex structures
        /// </summary>
        /// <param name="left">The first of the two BigComplex structure instances to divide</param>
        /// <param name="right">The second of the two BigComplex structure instances to divide</param>
        /// <returns>Returns a BigComplex structure which represents the calculation of left / right</returns>
        public static BigComplex operator /(BigComplex left, BigComplex right)
        {
            BigFloat denominator = BigFloat.Pow(right.Real, 2) + BigFloat.Pow(right.Imaginary, 2);
            BigFloat real = (left.Real * right.Real + left.Imaginary * right.Imaginary) / denominator;
            BigFloat imaginary = (left.Imaginary * right.Real - left.Real * right.Imaginary) / denominator;

            return new BigComplex(real, imaginary);
        }

        #endregion

        #region Static Arithmetic Methods

        /// <summary>
        /// Calculates the atan2 value of the supplied BigComplex number
        /// </summary>
        /// <param name="value">The <see cref="BigComplex"/> for which the atan2 will be calculated</param>
        /// <returns>The 2 argument arc tangent of a complex number</returns>
        /// <remarks>This function is particularly useful because it resolves the ambiguity that can arise 
        /// when finding the angle. The standard arctangent function only gives angles in the range -pi/2 to 
        /// pi/2 (quadrants I and IV). However, with ( \text{atan2} ), the angle is calculated for all four 
        /// quadrants by taking the signs of both ( x ) and ( y ) into account. This ensures it outputs the 
        /// correct angle in the range pi to pi</remarks>
        public static BigFloat Atan2(BigComplex value)
        {
            if (value.Real == BigFloat.Zero)
            {
                if (value.Imaginary > BigFloat.Zero)
                {
                    return BigFloat.Divide(BigFloat.Pi, new BigFloat(2)); // π/2
                }
                else if (value.Imaginary < BigFloat.Zero)
                {
                    return -BigFloat.Pi / new BigFloat(2); // -π/2  
                }
                return BigFloat.Zero;
            }

            BigFloat atan = BigFloat.Atan(value.Imaginary / value.Real);

            if (value.Real > BigFloat.Zero)
            {
                return atan;
            }
            else if (value.Imaginary >= BigFloat.Zero)
            {
                return atan + BigFloat.Pi; // π
            }
            else
            {
                return atan - BigFloat.Pi; // -π
            }
        }

        /// <summary>
        /// Tests if the two supplied BigComplex structure instances have the same properties
        /// </summary>
        /// <param name="item1">The first instance of an <see cref="BigComplex"/> struct for the comparison</param>
        /// <param name="item2">The second instance of an <see cref="BigComplex"/> struct for the comparison</param>
        /// <returns><c>True</c> if the two instances have the same <see cref="BigComplex.Real"/> and 
        /// <see cref="BigComplex.Imaginary"/> properties</returns>
        public static bool IsEqual(BigComplex left, BigComplex right)
        {
            return left.Real == right.Real && left.Imaginary == right.Imaginary;
        }

        #endregion

        #region ICloneable Support

        /// <summary>
        /// Creates a new instance of a BigComplex structure which has the same real and imaginary properties as this instance
        /// </summary>
        /// <returns>A deep copy of this instance of a BigComplex structure with identical real and imaginary properties</returns>
        public Object Clone()
        {
            return new BigComplex(this);
        }
        #endregion

        #region IComparable Support

        /// <summary>
        /// Compares this instance of a BigComplex structure to a specified object instance and returns an integer that indicates whether the value of this instance is less than, 
        /// equal to, or greater than the value of the specified object instance
        /// </summary>
        /// The object to compare with this instance of a BigComplex structure<param name="obj"></param>
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
                    return this.CompareTo((BigComplex)obj);
                }
                catch (InvalidCastException ex)
                {
                    throw new InvalidCastException("The object you provided cannot be cast to an BigComplex object.", ex);
                }
            }
            else
            {
                return this.CompareTo((BigComplex)obj);
            }
        }

        /// <summary>
        /// Compares this instance of a BigComplex structure to a specified object instance and returns an integer that indicates whether the value of this instance is less than, 
        /// equal to, or greater than the value of the specified object instance
        /// </summary>
        /// <param name="obj">The object to compare with this instance of a BigComplex structure</param>
        /// <returns>Less than zero if this instance is closer to the origin than the supplied version, zero if they are the same distance, and greater than zero if this 
        /// instance is farther from the origin than the supplied version (the Modulus property is used)</returns>
        public int CompareTo(BigComplex other)
        {
            // Multiply by 100000 in order to increase differentiation
            return Convert.ToInt32((this.Modulus - other.Modulus) * 100000);
        }
        #endregion

        #region IEquatable Support

        /// <summary>
        /// Tests if the supplied object, <paramref name="obj">obj</paramref>, is an instance of a BigComplex structure and if 
        /// so tests whether it has the same properties as this instance of a BigComplex structure
        /// </summary>
        /// <param name="obj">A BigComplex object to test for property equivalence with this instance</param>
        /// <returns><c>True</c> if both instances have the same real and imaginary properties, <c>False</c> otherwise</returns>
        /// <remarks>Internally this method uses the equality operator, ==</remarks>
        public override bool Equals(object? obj)
        {
            if ((obj == null) || (!object.ReferenceEquals(this.GetType(), obj.GetType())))
                return false;
            return (this == (BigComplex)obj);
        }

        /// <summary>
        /// Tests if the supplied object has the same properties as this instance of a BigComplex structure
        /// </summary>
        /// <param name="obj">A BigComplex object to test for property equivalence with this instance</param>
        /// <returns><c>True</c> if both instances have the same real and imaginary properties, <c>False</c> otherwise</returns>
        /// <remarks>Internally this method uses the equality operator, ==</remarks>
        public bool Equals(BigComplex other)
        {
            return this == other;
        }

        #endregion

        #region Object Methods

        /// <summary>
        /// Returns a hash code for this instance of a BigComplex structure
        /// </summary>
        /// <returns>A single hash code calculated from the individual hash codes of the real and imaginary parts</returns>
        public override int GetHashCode() => HashCode.Combine(this._real.GetHashCode(), this._imaginary.GetHashCode());

        /// <summary>
        /// Returns a string representation of this instance of a BigComplex structure
        /// </summary>
        /// <returns>A <see cref="string"/> representation of this <see cref="BigComplex"/></returns>
        public override string ToString()
        {
            // Copied from Science Library SCI, https://sourceforge.net/projects/scinet/?source=typ_redirect

            StringBuilder z = new();
            _ = z.Append('(');
            _ = z.Append(this._real.ToString());

            if (this._imaginary > new BigFloat(0))
                _ = z.Append(" + ");
            else if (this._imaginary < new BigFloat(0))
                _ = z.Append(" - ");

            if (this._imaginary != new BigFloat(0))
            {
                _ = z.Append(BigFloat.Abs(this._imaginary).ToString());
                _ = z.Append('i');
            }
            _ = z.Append(')');

            return z.ToString();
        }

        #endregion
    }
}