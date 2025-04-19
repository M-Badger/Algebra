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

using System.Numerics;
using System.Text;
using Sdcb.Arithmetic.Mpfr;

namespace Badger.Maths.Algebra
{
    /// <summary>
    /// The MpfrComplex struct is a structure which represents a complex number using an MPFR library (Sdcb.Arithmetic.Mpfr) 
    /// for the underlying real and imaginary parts
    /// </summary>
    /// <seealso href="https://github.com/sdcb/Sdcb.Arithmetic">MpfrFloat</seealso>
    public readonly struct MpfrComplex : ICloneable, IComparable, IComparable<MpfrComplex>, IEquatable<MpfrComplex>
    {
        #region Fields

        /// <summary>
        /// The real part of this MpfrComplex number
        /// </summary>
        private readonly MpfrFloat _real;

        /// <summary>
        /// The imaginary part of this MpfrComplex number
        /// </summary>
        private readonly MpfrFloat _imaginary;

        /// <summary>
        /// Determines the number of bits used to represent the floating-point number.  
        /// The higher the precision, the more accurate the number representation, but also the more memory it requires
        /// </summary>
        private readonly int _precision;

        /// <summary>
        /// The rounding mode used for the MPFR library
        /// </summary>
        private readonly MpfrRounding _roundingMode = MpfrRounding.ToEven;

        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a new MpfrComplex number using the supplied real and imaginary parts
        /// </summary>
        /// <param name="realpart">The real part (a) of the complex number a + bi</param>
        /// <param name="imaginarypart">The imaginary part (b) of the complex number a + bi</param>
        /// <remarks></remarks>
        public MpfrComplex(MpfrFloat realpart, MpfrFloat imaginarypart, int precision = 1000, MpfrRounding rounding = MpfrRounding.ToEven)
        {
            this._real = realpart;
            this._imaginary = imaginarypart;
            this._precision = precision;
            this._roundingMode = rounding;
        }

        /// <summary>
        /// Constructs a new MpfrComplex number using the supplied BigComplex numer
        /// </summary>
        /// <param name="complex">A MpfrComplex number</param>
        /// <remarks></remarks>
        public MpfrComplex(MpfrComplex complex, int precision = 1000, MpfrRounding rounding = MpfrRounding.ToEven)
        {
            this._real = complex.Real;
            this._imaginary = complex.Imaginary;
            this._precision = precision;
            this._roundingMode = rounding;
        }

        /// <summary>
        /// Constructs a new MpfrComplex number using the supplied System.Numerics.Complex number
        /// </summary>
        /// <param name="complex">A System.Numerics.Complex number</param>
        /// <remarks></remarks>
        public MpfrComplex(Complex complex, int precision = 1000, MpfrRounding rounding = MpfrRounding.ToEven)
        {
            this._real = MpfrFloat.From(complex.Real, precision);
            this._imaginary = MpfrFloat.From(complex.Imaginary, precision);
            this._precision = precision;
            this._roundingMode = rounding;
        }

        #endregion

        #region Properties / Accessors
        /// <summary>
        /// The Real part of this imaginary number
        /// </summary>
        public MpfrFloat Real
        {
            get { return this._real; }
        }

        /// <summary>
        /// The Imaginary part of this imaginary number
        /// </summary>
        public MpfrFloat Imaginary
        {
            get { return this._imaginary; }
        }

        /// <summary>
        /// The number of bits used to represent the floating-point number.  
        /// The higher the precision, the more accurate the number representation, but also the more memory it requires
        /// </summary>
        public int Precision
        {
            get { return this._precision; }
        }

        /// <summary>
        /// The rounding mode used for the MPFR library
        /// </summary>
        public MpfrRounding RoundingMode
        {
            get { return this._roundingMode; }
        }

        /// <summary>
        /// Returns the modulus of this MpfrComplex number
        /// The modulus is the positive real scalar which measures the distance from the origin.
        /// </summary>
        public MpfrFloat Modulus()
        {
                MpfrFloat tResult, tImaginary;

                tResult = MpfrFloat.Square(this._real, this._precision, this._roundingMode);
                tImaginary = MpfrFloat.Square(this._imaginary, this._precision, this._roundingMode);

                tResult += tImaginary;

                return MpfrFloat.Sqrt(tResult, this._precision, this._roundingMode);
        }

        /// <summary>
        /// Returns the Argument of this MpfrComplex structure. The argument measures the angle that the line from the origin to the 
        /// point z makes with the real axis. The argument is returned in radians
        /// </summary>
        public MpfrFloat Argument()
        {
                return MpfrFloat.Atan2(this._imaginary, this._real, this._precision, this._roundingMode);
        }

        /// <summary>
        /// Gets the complex conjugate of this <see cref="ComplexNumber"/>.
        /// </summary>
        public MpfrComplex Conjugate()
        {
            return new MpfrComplex(this._real, new MpfrFloat(0) - this._imaginary, this._precision, this._roundingMode);
        }

        #endregion

        #region Static Arithmetic Operators

        /// <summary>
        /// Tests if the two supplied MpfrComplex structure instances have the same properties
        /// </summary>
        /// <param name="item1">The first instance of a MpfrComplex structure for the comparison</param>
        /// <param name="item2">The second instance of a MpfrComplex structure for the comparison</param>
        /// <returns>True if both instances have the same real and imaginary properties, false otherwise</returns>
        /// <remarks><para>The <c>sdcb.Arithmetic.Mpfr</c> library uses the <c>mpfr_equal_p</c> method to determine 
        /// equality. <c>mpfr_equal_p</c> performs a bitwise comparison, meaning that the numbers are considered 
        /// equal only if their entire binary representations (including sign, exponent, and mantissa) match.</para>
        /// <para>Consider using the <see cref="IsEqual(MpfrComplex, MpfrComplex, double)"/> function to 
        /// perform an equality test within a specified precision.</para></remarks>
        public static bool operator ==(MpfrComplex item1, MpfrComplex item2)
        {
            return item1.Real.Equals(item2.Real) && item1.Imaginary.Equals(item2.Imaginary);
        }

        /// <summary>
        /// Tests if the two supplied MpfrComplex structure instances have different properties
        /// </summary>
        /// <param name="item1">The first instance of a MpfrComplex structure for the comparison</param>
        /// <param name="item2">The second instance of a MpfrComplex structure for the comparison</param>
        /// <returns><c>True</c> if the two instances have different real and/or imaginary properties, <c>false</c> otherwise</returns>
        /// <remarks><para>The <c>sdcb.Arithmetic.Mpfr</c> library uses the <c>mpfr_equal_p</c> method to determine 
        /// equality. <c>mpfr_equal_p</c> performs a bitwise comparison, meaning that the numbers are considered 
        /// equal only if their entire binary representations (including sign, exponent, and mantissa) match.</para>
        /// <para>Consider using the <see cref="IsEqual(MpfrComplex, MpfrComplex, double)"/> function to 
        /// perform an equality test within a specified precision.</para></remarks>
        public static bool operator !=(MpfrComplex item1, MpfrComplex item2)
        {
            return !(item1 == item2);
        }

        /// <summary>
        /// Tests if the two supplied MpfrComplex structure instances have the same properties to within a given <paramref name="tolerance"/>
        /// </summary>
        /// <param name="item1">The first instance of an MpfrComplex structure for the comparison</param>
        /// <param name="item2">The second instance of an MpfrComplex structure for the comparison</param>
        /// <param name="tolerance">If two MpfrComplex structures have the same properties to within a defined 
        /// <paramref name="tolerance"/> then they are considered equal.</param>
        /// <returns><c>True</c> if the two instances have the same real or imaginary properties (within the defined 
        /// <paramref name="tolerance"/>, <c>false</c> otherwise</returns>
        public static bool IsEqual(MpfrComplex item1, MpfrComplex item2, double tolerance = 1e-25)
        {
            if (MpfrFloat.Subtract(item1.Real, item2.Real) > tolerance || MpfrFloat.Subtract(item1.Imaginary, item2.Imaginary) > tolerance)
                return false;
            return true;
        }

        /// <summary>
        /// The unary - operator, negates the value of <paramref name="value"/>
        /// </summary>
        /// <param name="value">The <see cref="MpfrComplex"/> that will be negated</param>
        /// <returns>A negated <see cref="MpfrComplex"/></returns>
        public static MpfrComplex operator -(MpfrComplex value)
        {
            return MpfrComplex.Negate(value);
        }

        /// <summary>
        /// The unary + operator, returns the value of <paramref name="value"/>, i.e. it is a no-op
        /// </summary>
        /// <param name="value">The <see cref="MpfrComplex"/> that will be subject to the unary + operation</param>
        /// <returns><paramref name="value"/></returns>
        /// <remarks>This operator is implemented for consistency with the - operator, it is a no-op, 
        /// it does not change the value of <paramref name="value"/></remarks>
        public static MpfrComplex operator +(MpfrComplex value)
        {
            return value;
        }

        /// <summary>
        /// Adds two MpfrComplex structures
        /// </summary>
        /// <param name="item1">The first of the two MpfrComplex structure instances to sum</param>
        /// <param name="item2">The second of the two MpfrComplex structure instances to sum</param>
        /// <remarks>The <see cref="Precision"/> and <see cref="RoundingMode"/> of <paramref name="item1"/> are used for the new MpfrComplex</remarks>
        /// <returns>Returns a MpfrComplex structure which represents the sum of item1 + item2</returns>
        public static MpfrComplex operator +(MpfrComplex item1, MpfrComplex item2)
        {
            return new MpfrComplex(item1.Real + item2.Real, item1.Imaginary + item2.Imaginary, item1.Precision, item1.RoundingMode);
        }

        /// <summary>
        /// Subtracts two MpfrComplex structures
        /// </summary>
        /// <param name="item1">The first of the two MpfrComplex structure instances to subtract</param>
        /// <param name="item2">The second of the two MpfrComplex structure instances to subtract</param>
        /// <remarks>The <see cref="Precision"/> and <see cref="RoundingMode"/> of <paramref name="item1"/> are used for the new MpfrComplex</remarks>
        /// <returns>Returns a MpfrComplex structure which represents the calculation of item1 - item2</returns>
        public static MpfrComplex operator -(MpfrComplex item1, MpfrComplex item2)
        {
            return item1 + (-item2);
        }

        /// <summary>
        /// Multiplies two MpfrComplex structures
        /// </summary>
        /// <param name="item1">The first of the two MpfrComplex structure instances to multiply</param>
        /// <param name="item2">The second of the two MpfrComplex structure instances to multiply</param>
        /// <remarks>The <see cref="Precision"/> and <see cref="RoundingMode"/> of <paramref name="item1"/> are used for the new MpfrComplex</remarks>
        /// <returns>Returns a MpfrComplex structure which represents the calculation of item1 * item2</returns>
        public static MpfrComplex operator *(MpfrComplex item1, MpfrComplex item2)
        {
            MpfrFloat NewReal = (item1.Real * item2.Real) - (item1.Imaginary * item2.Imaginary);
            MpfrFloat NewImaginary = (item1.Real * item2.Imaginary) + (item1.Imaginary * item2.Real);

            return new MpfrComplex(NewReal, NewImaginary, item1.Precision, item1.RoundingMode);
        }

        /// <summary>
        /// Divides two MpfrComplex structures
        /// </summary>
        /// <param name="item1">The first of the two MpfrComplex structure instances to divide</param>
        /// <param name="item2">The second of the two MpfrComplex structure instances to divide</param>
        /// <remarks>The <see cref="Precision"/> and <see cref="RoundingMode"/> of <paramref name="item1"/> are used for the new MpfrComplex</remarks>
        /// <returns>Returns a MpfrComplex structure which represents the calculation of item1 / item2</returns>
        public static MpfrComplex operator /(MpfrComplex item1, MpfrComplex item2)
        {
            MpfrFloat denominator;
            MpfrFloat t1, t2, t3;
            MpfrFloat NewReal, NewImaginary;

            t1 = MpfrFloat.Square(item2.Real, item2.Precision, item2.RoundingMode);
            t2 = MpfrFloat.Square(item2.Imaginary, item2.Precision, item2.RoundingMode);
            denominator = t1 + t2;

            // Calculate real part
            t1 = item1.Real * item2.Real;
            t2 = item1.Imaginary * item2.Imaginary;
            t3 = t1 + t2;
            NewReal = t3 / denominator;

            //Calculate imaginary part
            t1 = item1.Imaginary * item2.Real;
            t2 = item1.Real * item2.Imaginary;
            t3 = t1 - t2;
            NewImaginary = t3 / denominator;

            return new MpfrComplex(NewReal, NewImaginary, item1.Precision, item1.RoundingMode);
        }

        /// <summary>
        /// Tests if the first supplied MpfrComplex structure is less than the second supplied MpfrComplex structure
        /// </summary>
        /// <param name="item1">The MpfrComplex instance to compare with <paramref name="item2"/></param>
        /// <param name="item2">The MpfrComplex instance to compare with <paramref name="item1"/></param>
        /// <returns><c>True</c> if <paramref name="item1"/> is closer to the origin than <paramref name="item2"/>, otherwise <c>false</c></returns>
        public static bool operator <(MpfrComplex item1, MpfrComplex item2)
        {
            if (item1.CompareTo(item2) < 0) return true;
            return false;
        }

        /// <summary>
        /// Tests if the first supplied MpfrComplex structure is greater than the second supplied MpfrComplex structure
        /// </summary>
        /// <param name="item1">The MpfrComplex instance to compare with <paramref name="item2"/></param>
        /// <param name="item2">The MpfrComplex instance to compare with <paramref name="item1"/></param>
        /// <returns><c>True</c> if <paramref name="item1"/> is further from the origin than <paramref name="item2"/>, otherwise <c>false</c></returns>
        public static bool operator >(MpfrComplex item1, MpfrComplex item2)
        {
            if (item1.CompareTo(item2) > 0) return true;
            return false;
        }

        /// <summary>
        /// Tests if the first supplied MpfrComplex structure is less than or equal to the second supplied MpfrComplex structure
        /// </summary>
        /// <param name="item1">The MpfrComplex instance to compare with <paramref name="item2"/></param>
        /// <param name="item2">The MpfrComplex instance to compare with <paramref name="item1"/></param>
        /// <returns><c>True</c> if <paramref name="item1"/> is the same distance or closer to the origin than <paramref name="item2"/>, otherwise <c>false</c></returns>
        public static bool operator <=(MpfrComplex item1, MpfrComplex item2)
        {
            if (item1.CompareTo(item2) <= 0) return true;
            return false;
        }

        /// <summary>
        /// Tests if the first supplied MpfrComplex structure is greater than or equal to the second supplied MpfrComplex structure
        /// </summary>
        /// <param name="item1">The MpfrComplex instance to compare with <paramref name="item2"/></param>
        /// <param name="item2">The MpfrComplex instance to compare with <paramref name="item1"/></param>
        /// <returns></returns>
        public static bool operator >=(MpfrComplex item1, MpfrComplex item2)
        {
            if (item1.CompareTo(item2) >= 0) return true;
            return false;
        }

        #endregion

        #region Static Arithmetic Methods

        /// <summary>
        /// Negates the supplied <see cref="MpfrComplex"/> structure
        /// </summary>
        /// <param name="value">The <see cref="MpfrComplex"/> to be negated</param>
        /// <returns>A new <see cref="MpfrComplex"/> struct that is the negative of <paramref name="value"/></returns>
        /// 
        public static MpfrComplex Negate(MpfrComplex value)
        {
            return new MpfrComplex(-value.Real, -value.Imaginary, value.Precision, value.RoundingMode);
        }

        #endregion

        #region ICloneable Support

        /// <summary>
        /// Creates a new instance of a MpfrComplex structure which has the same real and imaginary properties as this instance
        /// </summary>
        /// <returns>A deep copy of this instance of a MpfrComplex structure with identical real and imaginary properties</returns>
        public Object Clone()
        {
            return new MpfrComplex(this, this._precision, this._roundingMode);
        }
        #endregion

        #region IComparable Support

        /// <summary>
        /// Compares this instance of a MpfrComplex structure to a specified object instance and returns an integer that indicates whether the value of this instance is less than, 
        /// equal to, or greater than the value of the specified object instance
        /// </summary>
        /// <param name="obj">The object to compare with this instance of an MpfrComplex structure</param>
        /// <returns>Less than zero if this instance is closer to the origin than the supplied version, zero if they are the same distance, and greater than zero if this 
        /// instance is farther from the origin than the supplied version (the <see cref="Modulus"/> property is used)</returns>
        /// <exception cref="ArgumentNullException">This exception is thrown if <paramref name="obj"/> is <code>Null</code> (<code>Nothing</code> in VB)</exception>
        /// <exception cref="InvalidCastException">This exception is thrown if <paramref name="obj"/> cannot be cast to an <c cref="MpfrComplex"/></exception>
        public int CompareTo(object? obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "The object you have tried to compare to this instance is Null (Nothing in VB).");
            }
            else if ((!object.ReferenceEquals(this.GetType(), obj.GetType())))
            {
                try
                {
                    return this.CompareTo((MpfrComplex)obj);
                }
                catch (InvalidCastException ex)
                {
                    throw new InvalidCastException("The object you provided cannot be cast to an MPFRComplex object.", ex);
                }
            }
            else
            {
                return this.CompareTo((MpfrComplex)obj);
            }
        }

        /// <summary>
        /// Compares this instance of a MpfrComplex structure to a specified object instance and returns an integer that indicates
        /// whether the value of this instance is less than, equal to, or greater than the value of the specified object instance
        /// </summary>
        /// <param name="obj">The object to compare with this instance of a MpfrComplex structure</param>
        /// <returns>Less than zero if this instance is closer to the origin than the supplied version, zero if they are the same 
        /// distance, and greater than zero if this instance is farther from the origin than the supplied version 
        /// (the <see cref="Modulus"/> property is used)</returns>
        public int CompareTo(MpfrComplex other)
        {
            // Multiply by 100000 in order to increase differentiation
            MpfrFloat tTemp;
            tTemp = this.Modulus() - other.Modulus();
            tTemp *= new MpfrFloat(10000);
            return Convert.ToInt32(tTemp);
        }
        #endregion

        #region IEquatable Support

        /// <summary>
        /// Tests if the supplied object, <paramref name="obj">obj</paramref>, is an instance of a MpfrComplex structure and if
        /// so tests whether it has the same properties as this instance of an MpfrComplex structure
        /// </summary>
        /// <param name="obj">An MpfrComplex object to test for property equivalence with this instance</param>
        /// <returns><c>True</c> if both instances have the same real and imaginary properties, <c>False</c> otherwise</returns>
        /// <remarks>Internally this method uses the equality operator, ==</remarks>
        public override bool Equals(object? obj)
        {
            if ((obj == null) || (!object.ReferenceEquals(this.GetType(), obj.GetType())))
                return false;
            return (this == (MpfrComplex)obj);
        }

        /// <summary>
        /// Tests if the supplied object has the same properties as this instance of a MpfrComplex structure
        /// </summary>
        /// <param name="obj">A MpfrComplex object to test for property equivalence with this instance</param>
        /// <returns><c>True</c> if both instances have the same real and imaginary properties, <c>False</c> otherwise</returns>
        /// <remarks>Internally this method uses the equality operator, ==</remarks>
        public bool Equals(MpfrComplex other)
        {
            return this == other;
        }

        #endregion

        #region Object Methods

        /// <summary>
        /// Returns a hash code for this instance of a MpfrComplex structure
        /// </summary>
        /// <returns>A single hash code calculated from the individual hash codes of the real and imaginary parts</returns>
        public override int GetHashCode() => HashCode.Combine(this._real.GetHashCode(), this._imaginary.GetHashCode());

        /// <summary>
        /// Returns a string representation of this instance of a MpfrComplex structure
        /// </summary>
        /// <returns>A string representation of this instance of a MpfrComplex structure</returns>
        public override string ToString()
        {
            // Copied from Science Library SCI, https://sourceforge.net/projects/scinet/?source=typ_redirect

            StringBuilder z = new();
            z.Append('(');
            z.Append(this._real.ToString());

            if (this._imaginary >= new MpfrFloat(0))
                z.Append(" + ");
            else if (this._imaginary <= new MpfrFloat(0))
                z.Append(" - ");

            if (this._imaginary != new MpfrFloat(0))
            {
                MpfrFloat tTemp = MpfrFloat.Abs(this._imaginary, this._precision, this._roundingMode);
                z.Append(tTemp.ToString());
                z.Append('i');
            }
            z.Append(')');

            return z.ToString();
        }
        #endregion

    }
}
