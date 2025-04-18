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

//========================================================================
// Based on code provided by Patrick Demian, see copyright notice below
// The above code has been modified, primarily to convert from a class to
// a struct and all of the consequent changes to the code.
// XML comments have also been added to the code
//========================================================================
// Downloaded from GitHub 15 April 2018
// https://github.com/Osinko/BigFloat
//========================================================================
//Copyright(C) 2014 Patrick Demian

//Permission is hereby granted, free of charge, to any person obtaining a copy of
//this software and associated documentation files (the "Software"), to deal in
//the Software without restriction, including without limitation the rights to
//use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//of the Software, and to permit persons to whom the Software is furnished to do
//so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//========================================================================

using Sdcb.Arithmetic.Mpfr;
using System.Globalization;
using System.Numerics;

namespace Badger.Maths.Algebra
{
    /// <summary>
    /// The <see cref="BigFloat"/> struct represents a floating point number of arbitrary precision using 
    /// the <see cref="System.Numerics.BigInteger"/> struct for the numerator and denominator.
    /// </summary>
    [Serializable]
    public readonly struct BigFloat : IComparable, IComparable<BigFloat>, IEquatable<BigFloat>
    {   

        #region Fields

        /// <summary>
        /// The numerator of the BigFloat number.
        /// </summary>
        private readonly BigInteger _numerator;

        /// <summary>
        /// The denominator of the BigFloat number.
        /// </summary>
        private readonly BigInteger _denominator;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="BigFloat"/> struct with a value of 0 (which is the same 
        /// as <see cref="BigFloat.Zero"/>)"/>
        /// </summary>
        public BigFloat()
        {
            this._numerator = BigInteger.Zero;
            this._denominator = BigInteger.One;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BigFloat"/> struct with a value of <paramref name="value"/> 
        /// where <paramref name="value"/> is parsed by the <see cref="BigFloat.Parse(string)"/> function
        /// </summary>
        /// <param name="value">A <see cref="String"/> representing a floating point value</param>
        /// <remarks><paramref name="value"/> is sent to the <see cref="BigFloat.Parse(string)"/> function 
        /// which in turn uses the <see cref="BigInteger.Parse(string)"/> and <see cref="BigFloat.Reduce(BigFloat)"/> functions
        /// to parse the string to a floating point value</remarks> 
        public BigFloat(string value)
        {
            BigFloat bf = Parse(value);
            this._numerator = bf.Numerator;
            this._denominator = bf.Denominator;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BigFloat"/> struct with a value of 
        /// <paramref name="numerator"/> / <paramref name="denominator"/>
        /// </summary>
        /// <param name="numerator">The numerator of the new <see cref="BigFloat"/> struct</param>
        /// <param name="denominator">The denominator of the new <see cref="BigFloat"/> struct</param>
        /// <exception cref="ArgumentException">This exception is thrown if the denominator is set to zero</exception>
        public BigFloat(BigInteger numerator, BigInteger denominator)
        {
            this._numerator = numerator;
            if (denominator == 0)
                throw new ArgumentException("The denominator cannot be zero");
            this._denominator = BigInteger.Abs(denominator);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BigFloat"/> struct with a value of <paramref name="value"/>
        /// </summary>
        /// <param name="value">The <see cref="BigInteger"/> value to be converted to a <see cref="BigFloat"/></param>
        /// <remarks>The numerator is set to <paramref name="value"/> and the denominator is set to 
        /// <see cref="BigInteger.One"/></remarks>
        public BigFloat(BigInteger value)
        {
            this._numerator = value;
            this._denominator = BigInteger.One;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BigFloat"/> struct with a value of <paramref name="value"/>
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> value to be copied</param>
        /// <remarks>If <paramref name="value"/> is <c>Null</c> then the new <see cref="BigFloat"/> is 
        /// set to the equivalent of <see cref="BigFloat.Zero"/> else the numerator and denominator 
        /// are copied from <paramref name="value"/> to the new instance</remarks>
        public BigFloat(BigFloat value)
        {
            if (BigFloat.Equals(value, null))
            {
                this._numerator = BigInteger.Zero;
                this._denominator = BigInteger.One;
            }
            else
            {
                this._numerator = value.Numerator;
                this._denominator = value.Denominator;
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BigFloat"/> struct with a value of <paramref name="value"/>
        /// </summary>
        /// <param name="value">The <see cref="ulong"/> value to be converted to a <see cref="BigFloat"/></param>
        /// <remarks>The numerator is set to <paramref name="value"/> and the denominator is set to 
        /// <see cref="BigInteger.One"/></remarks>
        public BigFloat(ulong value)
        {
            this._numerator = new BigInteger(value);
            this._denominator = BigInteger.One;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BigFloat"/> struct with a value of <paramref name="value"/>
        /// </summary>
        /// <param name="value">The <see cref="long"/> value to be converted to a <see cref="BigFloat"/></param>
        /// <remarks>The numerator is set to <paramref name="value"/> and the denominator is set to 
        /// <see cref="BigInteger.One"/></remarks>
        public BigFloat(long value)
        {
            this._numerator = new BigInteger(value);
            this._denominator = BigInteger.One;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BigFloat"/> struct with a value of <paramref name="value"/>
        /// </summary>
        /// <param name="value">The <see cref="uint"/> value to be converted to a <see cref="BigFloat"/></param>
        /// <remarks>The numerator is set to <paramref name="value"/> and the denominator is set to 
        /// <see cref="BigInteger.One"/></remarks>
        public BigFloat(uint value)
        {
            this._numerator = new BigInteger(value);
            this._denominator = BigInteger.One;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BigFloat"/> struct with a value of <paramref name="value"/>
        /// </summary>
        /// <param name="value">The <see cref="int"/> value to be converted to a <see cref="BigFloat"/></param>
        /// <remarks>The numerator is set to <paramref name="value"/> and the denominator is set to 
        /// <see cref="BigInteger.One"/></remarks>
        public BigFloat(int value)
        {
            this._numerator = new BigInteger(value);
            this._denominator = BigInteger.One;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BigFloat"/> struct with a value of <paramref name="value"/>
        /// </summary>
        /// <param name="value">The <see cref="float"/> value to be converted to a <see cref="BigFloat"/></param>
        /// <remarks><paramref name="value"/> is first converted to a string and then 
        /// <see cref="BigFloat.BigFloat(string)"/> is called</remarks>
        public BigFloat(float value) : this(value.ToString("N99"))
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BigFloat"/> struct with a value of <paramref name="value"/>
        /// </summary>
        /// <param name="value">The <see cref="double"/> value to be converted to a <see cref="BigFloat"/></param>
        /// <remarks><paramref name="value"/> is first converted to a string and then 
        /// <see cref="BigFloat.BigFloat(string)"/> is called</remarks>
        public BigFloat(double value) : this(value.ToString("N99"))
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BigFloat"/> struct with a value of <paramref name="value"/>
        /// </summary>
        /// <param name="value">The <see cref="decimal"/> value to be converted to a <see cref="BigFloat"/></param>
        /// <remarks><paramref name="value"/> is first converted to a string and then 
        /// <see cref="BigFloat.BigFloat(string)"/> is called</remarks>
        public BigFloat(decimal value) : this(value.ToString("N99"))
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="BigFloat"/> struct with a value of <paramref name="value"/>
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to be converted to a <see cref="BigFloat"/></param>
        /// <returns>A new instance of a <see cref="BigFloat"/> struct with a value <paramref name="value"/></returns>
        /// <exception cref="ArgumentNullException">This exception is thrown if <paramref name="value"/> is <c>Null</c>></exception>
        /// <remarks><see cref="BigFloat.Parse(string)"/> uses <see cref="BigInteger.Parse(string)"/> and 
        /// <see cref="BigFloat.Factor()"/> to convert the string to a <see cref="BigFloat"/></remarks>
        public static BigFloat Parse(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            value = value.Trim();
            value = value.Replace(",", "");
            int pos = value.IndexOf('.');
            value = value.Replace(".", "");

            if (pos < 0)
            {
                //no decimal point
                BigInteger numerator = BigInteger.Parse(value);
                return BigFloat.Reduce(new BigFloat(numerator));
            }
            else
            {
                //decimal point (length - pos - 1)
                BigInteger numerator = BigInteger.Parse(value);
                BigInteger denominator = BigInteger.Pow(10, value.Length - pos);

                return BigFloat.Reduce(new BigFloat(numerator, denominator));
            }
        }

        /// <summary>
        /// Attempts to parse a <see cref="string"/> value into a <see cref="BigFloat"/> value
        /// </summary>
        /// <param name="value">The <see cref="string"/> to be parsed into a <see cref="BigFloat"/></param>
        /// <param name="result">The result of the <see cref="TryParse(string, out BigFloat)"/> method</param>
        /// <returns><para>If <paramref name="value"/> is successfully parsed to a <see cref="BigFloat"/> then 
        /// <see cref="TryParse(string, out BigFloat)"/> returns <c>true</c> otherwise <c>false</c></para>
        /// <para>If <see cref="TryParse(string, out BigFloat)"/> succeeds then <paramref name="result"/> 
        /// is the parsed value of <paramref name="value"/></para>
        /// <para>If <see cref="TryParse(string, out BigFloat)"/> fails then <paramref name="result"/>
        /// is <see cref="BigFloat.Zero"/></para></returns>
        public static bool TryParse(string value, out BigFloat result)
        {
            try
            {
                result = BigFloat.Parse(value);
                return true;
            }
            catch (ArgumentNullException)
            {
                result = BigFloat.Zero;
                return false;
            }
            catch (FormatException)
            {
                result = BigFloat.Zero;
                return false;
            }
        }

        #endregion

        #region Public Static Fields

        /// <summary>
        /// Provides a constant value of 1
        /// </summary>
        public static readonly BigFloat One = new(1);

        /// <summary>
        /// Provides a constant value of 0
        /// </summary>
        public static readonly BigFloat Zero = new();

        /// <summary>
        /// Provides a constant value of -1
        /// </summary>
        public static readonly BigFloat MinusOne = new(-1);

        /// <summary>
        /// Provides a constant value of 0.5
        /// </summary>
        public static readonly BigFloat OneHalf = new(1, 2);

        /// <summary>
        /// Provides a constant value of Pi
        /// </summary>
        public static readonly BigFloat Pi = new(new BigInteger(22), new BigInteger(7));

        #endregion

        #region Properties / Accessors

        /// <summary>
        /// Gets the numerator of the <see cref="BigFloat"/> number.
        /// </summary>
        public BigInteger Numerator
        {
            get { return this._numerator; }
        }

        /// <summary>
        /// Gets the denominator of the <see cref="BigFloat"/> number.
        /// </summary>
        public BigInteger Denominator
        {
            get { return this._denominator; }
        }

        /// <summary>
        /// Gets the sign of the <see cref="BigFloat"/> number.
        /// </summary>
        public int Sign
        {
            get
            {
                return (this._numerator.Sign + this._denominator.Sign) switch
                {
                    2 or -2 => 1,
                    0 => -1,
                    _ => 0,
                };
            }
        }

        #endregion

        #region Static Operators

        /// <summary>
        /// Returns the negation of a <see cref="BigFloat"/> number
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> that will be negated</param>
        /// <returns>A new <see cref="BigFloat"/> that is the negative of <paramref name="value"/></returns>
        public static BigFloat operator -(BigFloat value)
        {
            return BigFloat.Negate(value);
        }

        /// <summary>
        /// Subtracts two <see cref="BigFloat"/> numbers and returns the result
        /// </summary>
        /// <param name="left"><paramref name="right"/> will be subtracted frim this <see cref="BigFloat"/></param>
        /// <param name="right">The <see cref="BigFloat"/> that will extracted from <paramref name="left"/></param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the substration of 
        /// <paramref name="right"/> from <paramref name="left"/></returns>
        public static BigFloat operator -(BigFloat left, BigFloat right)
        {
            return BigFloat.Subtract(left, right);
        }

        /// <summary>
        /// Decrements a <see cref="BigFloat"/> number by 1
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> to be decremented by 1</param>
        /// <returns>A new <see cref="BigFloat"/> instance that is <paramref name="value"/> minus <see cref="BigFloat.One"/></returns>
        public static BigFloat operator --(BigFloat value)
        {
            return BigFloat.Decrement(value);
        }

        /// <summary>
        /// Adds two <see cref="BigFloat"/> numbers together and returns the result
        /// </summary>
        /// <param name="left">The <see cref="BigFloat"/> to add to <paramref name="right"/></param>
        /// <param name="right">The <see cref="BigFloat"/> to add to <paramref name="left"/></param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the sum of <paramref name="left"/> and 
        /// <paramref name="right"/></returns>
        public static BigFloat operator +(BigFloat left, BigFloat right)
        {
            return BigFloat.Add(left, right);
        }

        /// <summary>
        /// The unary + operator, returns the value of <paramref name="value"/>, i.e. it is a no-op
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> that will be trated by the unary + operator</param>
        /// <returns><paramref name="value"/></returns>
        /// <remarks>This operator is implemented for consistency with the - operator, it is a no-op, 
        /// it does not change the value of <paramref name="value"/></remarks>
        public static BigFloat operator +(BigFloat value)
        {
            return value;
        }

        /// <summary>
        /// Increments a <see cref="BigFloat"/> number by 1
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> to be incremented by 1</param>
        /// <returns>A new <see cref="BigFloat"/> instance that is <paramref name="value"/> plus <see cref="BigFloat.One"/></returns>
        /// <remarks>Internally uses the <see cref="BigFloat.Increment(BigFloat)"/> method</remarks>
        public static BigFloat operator ++(BigFloat value)
        {
            return BigFloat.Increment(value);
        }

        /// <summary>
        /// Calculates the remainder of the division of two <see cref="BigFloat"/> numbers
        /// </summary>
        /// <param name="left">The <see cref="BigFloat"/> that will be divided by <paramref name="right"/></param>
        /// <param name="right"><paramref name="left"/> will be diveded by this <see cref="BigFloat"/></param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the remainder of the division of 
        /// <paramref name="left"/> by <paramref name="right"/></returns>
        /// <remarks>Internally uses the <see cref="BigFloat.Remainder(BigFloat, BigFloat)"/> method</remarks>
        public static BigFloat operator %(BigFloat left, BigFloat right)
        {
            return BigFloat.Remainder(left, right);
        }

        /// <summary>
        /// Multiplies two <see cref="BigFloat"/> numbers and returns the result
        /// </summary>
        /// <param name="left">The <see cref="BigFloat"/> that will be mutiplied by <paramref name="right"/></param>
        /// <param name="right">The <see cref="BigFloat"/> that will be mutiplied by <paramref name="left"/></param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the multiplication of 
        /// <paramref name="left"/> and <paramref name="right"/></returns>
        /// <remarks>Interally uses the <see cref="BigFloat.Multiply(BigFloat, BigFloat)"/> method</remarks>
        public static BigFloat operator *(BigFloat left, BigFloat right)
        {
            return BigFloat.Multiply(left, right);
        }

        /// <summary>
        /// Divides two <see cref="BigFloat"/> numbers and returns the result
        /// </summary>
        /// <param name="left">The <see cref="BigFloat"/> that will be divided by <paramref name="right"/></param>
        /// <param name="right"><paramref name="left"/> will be diveded by this <see cref="BigFloat"/></param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the division of 
        /// <paramref name="left"/> by <paramref name="right"/></returns>
        /// <remarks>Internally uses the <see cref="BigFloat.Divide(BigFloat, BigFloat)"/> method</remarks>
        public static BigFloat operator /(BigFloat left, BigFloat right)
        {
            return BigFloat.Divide(left, right);
        }

        /// <summary>
        /// Shifts the decimal point of a <see cref="BigFloat"/> number to the right by <paramref name="shift"/>
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> to be shifted right</param>
        /// <param name="shift">The number of places to ShiftRight</param>
        /// <returns>A new <see cref="BigFloat"/> instance that is <paramref name="value"/> right shifted by 
        /// <paramref name="shift"/> places</returns>
        /// <remarks>Internally uses the <see cref="BigFloat.ShiftDecimalRight(BigFloat, int)"/> method</remarks>
        public static BigFloat operator >>(BigFloat value, int shift)
        {
            return BigFloat.ShiftDecimalRight(value, shift);
        }

        /// <summary>
        /// Shifts the decimal point of a <see cref="BigFloat"/> number to the left by <paramref name="shift"/>
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> to be shifted left</param>
        /// <param name="shift">The number of places to ShiftLeft</param>
        /// <returns>A new <see cref="BigFloat"/> instance that is <paramref name="value"/> left shifted by 
        /// <paramref name="shift"/> places</returns>
        /// <remarks>Internally uses the <see cref="BigFloat.ShiftDecimalLeft(BigFloat, int)"/> method</remarks>
        public static BigFloat operator <<(BigFloat value, int shift)
        {
            return BigFloat.ShiftDecimalLeft(value, shift);
        }

        /// <summary>
        /// Raises a <see cref="BigFloat"/> number to the power of an integer exponent
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> that will be raised to the power of <paramref name="exponent"/></param>
        /// <param name="exponent"><paramref name="value"/> will be raised to the power of this <see cref="int"/></param>
        /// <returns>A new <see cref="BigFloat"/> instance that is <paramref name="value"/> raised to the 
        /// power of <paramref name="exponent"/>A new <see cref="BigFloat"/> instance that is 
        /// <paramref name="value"/> raised to the power of <paramref name="exponent"/></returns>
        /// <remarks>Internally uses the <see cref="BigFloat.Pow(BigFloat, int)"/> method</remarks>
        public static BigFloat operator ^(BigFloat left, int right)
        {
            return BigFloat.Pow(left, right);
        }

        /// <summary>
        /// Compares two <see cref="BigFloat"/> numbers for inequality
        /// </summary>
        /// <param name="left">A <see cref="BigFloat"/> to be compared to <paramref name="right"/></param>
        /// <param name="right">A <see cref="BigFloat"/> to be compared to <paramref name="left"/></param>
        /// <returns>Returns <c>true</c> if <paramref name="left"/> is not equal to <paramref name="right"/></returns>
        /// <remarks>Internally uses the <see cref="BigFloat.Compare(BigFloat, BigFloat)"/> method</remarks>
        public static bool operator !=(BigFloat left, BigFloat right)
        {
            return BigFloat.Compare(left, right) != 0;
        }

        /// <summary>
        /// Compares two <see cref="BigFloat"/> numbers for equality
        /// </summary>
        /// <param name="left">A <see cref="BigFloat"/> to be compared to <paramref name="right"/></param>
        /// <param name="right">A <see cref="BigFloat"/> to be compared to <paramref name="left"/></param>
        /// <returns>Returns <c>true</c> if <paramref name="left"/> is equal to <paramref name="right"/></returns>
        /// <example><list type="bullet">
        /// <item><description>1.5 (1/2) will be equal to 1.5 (1/2)</description></item>
        /// <item><description>1.5 (1/2) will be equal to 1.5 (4/8)</description></item>
        /// <item><description>2.3 (23/10) will not be equal to 3.2 (16/5)</description></item>
        /// </list></example>
        /// <remarks>Internally uses the <see cref="BigFloat.Compare(BigFloat, BigFloat)"/> method, this method 
        /// does not require that the two <see cref="BigFloat.Denominator"/> values be the same</remarks>
        public static bool operator ==(BigFloat left, BigFloat right)
        {
            return BigFloat.Compare(left, right) == 0;
        }

        /// <summary>
        /// Conducts a <c>less than</c> comparison on two <see cref="BigFloat"/> values
        /// </summary>
        /// <param name="left">A <see cref="BigFloat"/> to be compared to <paramref name="right"/></param>
        /// <param name="right">A <see cref="BigFloat"/> to be compared to <paramref name="left"/></param>
        /// <returns>Returns <c>true</c> if <paramref name="left"/> is less than <paramref name="right"/></returns>
        /// <remarks>Internally uses the <see cref="BigFloat.Compare(BigFloat, BigFloat)"/> method</remarks>
        public static bool operator <(BigFloat left, BigFloat right)
        {
            return BigFloat.Compare(left, right) < 0;
        }

        /// <summary>
        /// Conducts a <c>less than or equal to</c> comparison on two <see cref="BigFloat"/> values
        /// </summary>
        /// <param name="left">A <see cref="BigFloat"/> to be compared to <paramref name="right"/></param>
        /// <param name="right">A <see cref="BigFloat"/> to be compared to <paramref name="left"/></param>
        /// <returns>Returns <c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/></returns>
        /// <remarks>Internally uses the <see cref="BigFloat.Compare(BigFloat, BigFloat)"/> method</remarks>
        public static bool operator <=(BigFloat left, BigFloat right)
        {
            return BigFloat.Compare(left, right) <= 0;
        }

        /// <summary>
        /// Conducts a <c>greater than</c> comparison on two <see cref="BigFloat"/> values
        /// </summary>
        /// <param name="left">A <see cref="BigFloat"/> to be compared to <paramref name="right"/></param>
        /// <param name="right">A <see cref="BigFloat"/> to be compared to <paramref name="left"/></param>
        /// <returns>Returns <c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/></returns>
        /// <remarks>Internally uses the <see cref="BigFloat.Compare(BigFloat, BigFloat)"/> method</remarks>
        public static bool operator >(BigFloat left, BigFloat right)
        {
            return BigFloat.Compare(left, right) > 0;
        }

        /// <summary>
        /// Conducts a <c>greater than or equal to</c> comparison on two <see cref="BigFloat"/> values
        /// </summary>
        /// <param name="left">A <see cref="BigFloat"/> to be compared to <paramref name="right"/></param>
        /// <param name="right">A <see cref="BigFloat"/> to be compared to <paramref name="left"/></param>
        /// <returns>Returns <c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/></returns>
        /// <remarks>Internally uses the <see cref="BigFloat.Compare(BigFloat, BigFloat)"/> method</remarks>
        public static bool operator >=(BigFloat left, BigFloat right)
        {
            return BigFloat.Compare(left, right) >= 0;
        }

        #endregion

        #region Static Arithmetic Methods

        /// <summary>
        /// Calculates the machine epsilon of the <see cref="BigFloat"/> struct
        /// </summary>
        /// <returns>Returns the machine epsilon of the <see cref="BigFloat"/> struct</returns>
        /// <remarks>Machine epsilon is a concept in numerical analysis that represents the smallest value that can 
        /// be added to 1 to produce a result distinguishable from 1 in the floating-point system. It's essentially 
        /// a measure of the precision or the "gap" between representable floating-point numbers</remarks>
        public static BigFloat GetEpsilon()
        {
            BigFloat epsilon = new(BigInteger.One, BigInteger.One); // Start with 1
            BigFloat one = BigFloat.One;

            while (one + epsilon != one)
            {
                epsilon = new BigFloat(epsilon.Numerator, epsilon.Denominator * 2); // Halve epsilon
            }

            return epsilon;
        }

        /// <summary>
        /// Adds two <see cref="BigFloat"/> numbers together and returns the result
        /// </summary>
        /// <param name="left">The <see cref="BigFloat"/> to add to <paramref name="right"/></param>
        /// <param name="right">The <see cref="BigFloat"/> to add to <paramref name="left"/></param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the sum of <paramref name="left"/> and 
        /// <paramref name="right"/></returns>
        /// <exception cref="ArgumentNullException">This exception is thrown if either 
        /// <paramref name="left"/> or <paramref name="right"/> is <c>null</c></exception>
        public static BigFloat Add(BigFloat left, BigFloat right)
        {
            if (BigFloat.Equals(left, null))
                throw new ArgumentNullException(nameof(left));

            if (BigFloat.Equals(right, null))
                throw new ArgumentNullException(nameof(right));

            BigInteger newNumerator = BigInteger.Add(BigInteger.Multiply(left.Numerator, right.Denominator), BigInteger.Multiply(right.Numerator, left.Denominator));
            BigInteger newDenominator = BigInteger.Multiply(left.Numerator, right.Denominator);
            return new BigFloat(newNumerator, newDenominator);
        }

        /// <summary>
        /// Subtracts two <see cref="BigFloat"/> numbers and returns the result
        /// </summary>
        /// <param name="left"><paramref name="right"/> will be subtracted frim this <see cref="BigFloat"/></param>
        /// <param name="right">The <see cref="BigFloat"/> that will extracted from <paramref name="left"/></param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the substration of 
        /// <paramref name="right"/> from <paramref name="left"/></returns>
        /// <exception cref="ArgumentNullException">This exception is thrown if either 
        /// <paramref name="left"/> or <paramref name="right"/> is <c>null</c></exception>
        public static BigFloat Subtract(BigFloat left, BigFloat right)
        {
            if (BigFloat.Equals(left, null))
                throw new ArgumentNullException(nameof(left));

            if (BigFloat.Equals(right, null))
                throw new ArgumentNullException(nameof(right));

            BigInteger newNumerator = BigInteger.Subtract(BigInteger.Multiply(left.Numerator, right.Denominator), BigInteger.Multiply(right.Numerator, left.Denominator));
            BigInteger newDenominator = BigInteger.Multiply(left.Numerator, right.Denominator);
            return new BigFloat(newNumerator, newDenominator);
        }

        /// <summary>
        /// Multiplies two <see cref="BigFloat"/> numbers and returns the result
        /// </summary>
        /// <param name="left">The <see cref="BigFloat"/> that will be mutiplied by <paramref name="right"/></param>
        /// <param name="right">The <see cref="BigFloat"/> that will be mutiplied by <paramref name="left"/></param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the multiplication of 
        /// <paramref name="left"/> and <paramref name="right"/></returns>
        /// <exception cref="ArgumentNullException">This exception is thrown if either 
        /// <paramref name="left"/> or <paramref name="right"/> is <c>null</c></exception>
        public static BigFloat Multiply(BigFloat left, BigFloat right)
        {
            if (BigFloat.Equals(left, null))
                throw new ArgumentNullException(nameof(left));

            if (BigFloat.Equals(right, null))
                throw new ArgumentNullException(nameof(right));

            BigInteger newNumerator = BigInteger.Multiply(left.Numerator, right.Numerator);
            BigInteger newDenominator = BigInteger.Multiply(left.Denominator, right.Denominator);

            return new BigFloat(newNumerator, newDenominator);
        }

        /// <summary>
        /// Divides two <see cref="BigFloat"/> numbers and returns the result
        /// </summary>
        /// <param name="left">The <see cref="BigFloat"/> that will be divided by <paramref name="right"/></param>
        /// <param name="right"><paramref name="left"/> will be diveded by this <see cref="BigFloat"/></param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the division of 
        /// <paramref name="left"/> by <paramref name="right"/></returns>
        /// <exception cref="ArgumentNullException">This exception is thrown if either 
        /// <paramref name="left"/> or <paramref name="right"/> is <c>null</c></exception>
        /// <exception cref="System.DivideByZeroException">This excdeption is thrown if 
        /// <paramref name="right"/> evalutes to zero (if the numerator of 
        /// <paramref name="right"/> equals zero)</exception>
        public static BigFloat Divide(BigFloat left, BigFloat right)
        {
            if (BigInteger.Equals(left, null))
                throw new ArgumentNullException(nameof(left));

            if (BigInteger.Equals(right, null))
                throw new ArgumentNullException(nameof(right));

            if (right.Numerator.Equals(BigInteger.Zero))
                throw new System.DivideByZeroException(nameof(right));

            BigInteger newNumerator = BigInteger.Multiply(left.Numerator, right.Denominator);
            BigInteger newDenominator = BigInteger.Multiply(left.Denominator, right.Numerator);

            return new BigFloat(newNumerator, newDenominator);
        }

        /// <summary>
        /// Calculates the remainder of the division of two <see cref="BigFloat"/> numbers
        /// </summary>
        /// <param name="left">The <see cref="BigFloat"/> that will be divided by <paramref name="right"/></param>
        /// <param name="right"><paramref name="left"/> will be diveded by this <see cref="BigFloat"/></param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the remainder of the division of 
        /// <paramref name="left"/> by <paramref name="right"/></returns>
        /// <exception cref="ArgumentNullException">This exception is thrown if either 
        /// <paramref name="left"/> or <paramref name="right"/> is <c>null</c></exception>
        public static BigFloat Remainder(BigFloat left, BigFloat right)
        {
            if (BigFloat.Equals(left, null))
                throw new ArgumentNullException(nameof(left));

            if (BigFloat.Equals(right, null))
                throw new ArgumentNullException(nameof(right));

            BigFloat result = BigFloat.Subtract(left, BigFloat.Floor(BigFloat.Multiply(BigFloat.Divide(left, right), right)));
            return new BigFloat(result.Numerator, result.Denominator);
        }

        /// <summary>
        /// Gets the arctangent (inverse tangent function) of the <see cref="BigFloat"/> number.
        /// </summary>
        /// <returns>Returns the arctangent of this <see cref="BigFloat"/></returns>
        public static BigFloat Atan(BigFloat value)
        {
            if (value == BigFloat.Zero) return BigFloat.Zero;

            BigFloat result = BigFloat.Zero;
            BigFloat term = value; // First term in the series
            BigFloat xSquared = BigFloat.Pow(value, 2);

            bool subtract = true;
            int k = 1;

            while (BigFloat.Abs(value) > BigFloat.GetEpsilon())
            {
                result += term;

                // Calculate next term in the series
                k += 2;
                term *= xSquared;
                term /= new BigFloat(k);

                if (subtract)
                    term = -term;

                subtract = !subtract;
            }
            return result;
        }

        /// <summary>
        /// Raises a <see cref="BigFloat"/> number to the power of an integer exponent
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> that will be raised to the power of <paramref name="exponent"/></param>
        /// <param name="exponent"><paramref name="value"/> will be raised to the power of this <see cref="int"/></param>
        /// <returns>A new <see cref="BigFloat"/> instance that is <paramref name="value"/> raised to the 
        /// power of <paramref name="exponent"/>A new <see cref="BigFloat"/> instance that is 
        /// <paramref name="value"/> raised to the power of <paramref name="exponent"/></returns>
        public static BigFloat Pow(BigFloat value, int exponent)
        {
            BigInteger newNumerator = value.Numerator;
            BigInteger newDenominator = value.Denominator;

            if (value.Numerator.IsZero)
            {
                // Nothing to do
            }
            else if (exponent < 0)
            {
                newNumerator = BigInteger.Pow(value.Denominator, -exponent);
                newDenominator = BigInteger.Pow(value.Numerator, -exponent);
            }
            else
            {
                newNumerator = BigInteger.Pow(value.Numerator, exponent);
                newDenominator = BigInteger.Pow(value.Denominator, exponent);
            }

            return new BigFloat(newNumerator, newDenominator);
        }

        /// <summary>
        /// Calculates the absolute value of a <see cref="BigFloat"/> number
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> of which the absolute value will be calculated</param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the absolute value of <paramref name="value"/></returns>
        public static BigFloat Abs(BigFloat value)
        {
            BigInteger newNumerator = BigInteger.Abs(value.Numerator);
            return new BigFloat(newNumerator, value.Denominator);
        }

        /// <summary>
        /// Returns the negation of a <see cref="BigFloat"/> number
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> that will be negated</param>
        /// <returns>A new <see cref="BigFloat"/> that is the negative of <paramref name="value"/></returns>
        public static BigFloat Negate(BigFloat value)
        {
            return new BigFloat(BigInteger.Negate(value.Numerator), value.Denominator);
        }

        /// <summary>
        /// Returns the bitwise inversion of a <see cref="BigFloat"/> number, performing a bitwise inversion of 
        /// <paramref name="value"/>, flipping all the bits, also called a <c>bitwise complement</c> or <c>bitwise not</c>
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> subject to the bitwise inversion</param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the bitwise inversion of <paramref name="value"/></returns>
        public static BigFloat InvertBits(BigFloat value)
        {
            return new BigFloat(~value.Numerator, ~value.Denominator);
        }

        /// <summary>
        /// Calculates the reciprocal of a <see cref="BigFloat"/> number, the numerator and denominator are swapped
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BigFloat Reciprocal(BigFloat value)
        {
            return new BigFloat(value.Denominator, value.Numerator);
        }

        /// <summary>
        /// Increments a <see cref="BigFloat"/> number by 1
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> to be incremented by 1</param>
        /// <returns>A new <see cref="BigFloat"/> instance that is <paramref name="value"/> plus <see cref="BigFloat.One"/></returns>
        public static BigFloat Increment(BigFloat value)
        {
            return BigFloat.Add(value, BigFloat.One);
        }

        /// <summary>
        /// Decrements a <see cref="BigFloat"/> number by 1
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> to be decremented by 1</param>
        /// <returns>A new <see cref="BigFloat"/> instance that is <paramref name="value"/> minus <see cref="BigFloat.One"/></returns>
        public static BigFloat Decrement(BigFloat value)
        {
            return BigFloat.Subtract(value, BigFloat.One);
        }

        /// <summary>
        /// Reduces the <see cref="BigFloat"/> using <see cref="BigInteger.GreatestCommonDivisor(BigInteger, BigInteger)"/>
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> to be reduced</param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the reduced version of <paramref name="value"/></returns>
        public static BigFloat Reduce(BigFloat value)
        {
            if (BigInteger.Equals(value.Denominator, BigInteger.One))
                return new BigFloat(value);

            // Reduce numerator and denominator
            // System.Numerics.BigInteger uses the Euclidean Algirithm to find the GCD
            BigInteger factor = BigInteger.GreatestCommonDivisor(value.Numerator, value.Denominator);

            return new BigFloat(BigInteger.Divide(value.Numerator, factor), BigInteger.Divide(value.Denominator, factor));
        }

        /// <summary>
        /// Calculates the smallest integral value greater than or equal to the specified <see cref="BigFloat"/> value
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> for which the ceiling value will be calculated</param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the smallest integral value greater than 
        /// or equal to <paramref name="value"/></returns>
        public static BigFloat Ceiling(BigFloat value)
        {
            BigInteger newNumerator = value.Numerator;

            if (value.Numerator < BigInteger.Zero)
                newNumerator = BigInteger.Subtract(newNumerator, BigInteger.Remainder(value.Numerator, value.Denominator));
            else
                newNumerator = BigInteger.Add(newNumerator, BigInteger.Subtract(value.Denominator, BigInteger.Remainder(value.Numerator, value.Denominator)));

            return BigFloat.Reduce(new BigFloat(newNumerator, value.Denominator));
        }

        /// <summary>
        /// Calculates the largest integral value less than or equal to the specified <see cref="BigFloat"/> value
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> for which the floor value will be calculated</param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the largest integral value smaller than 
        /// or equal to <paramref name="value"/></returns>
        public static BigFloat Floor(BigFloat value)
        {
            BigInteger newNumerator = value.Numerator;

            if (value.Numerator < BigInteger.Zero)
                newNumerator = BigInteger.Add(newNumerator, BigInteger.Subtract(value.Denominator, BigInteger.Remainder(value.Numerator, value.Denominator)));
            else
                newNumerator = BigInteger.Subtract(newNumerator, BigInteger.Remainder(value.Numerator, value.Denominator));

            return BigFloat.Reduce(new BigFloat(newNumerator, value.Denominator));
        }

        /// <summary>
        /// Rounds <paramref name="value"/> to the nearest integer where 0.5 rounds up
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> to be rounded</param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the nearest integer to 
        /// <paramref name="value"/> with 0.5 rounding up</returns>
        /// <example>3.5 (7/2) will round to 4 (4/1)</example>
        /// <example>-3.5 (-7/2) will round to -3 (-3/1)</example>
        public static BigFloat Round(BigFloat value)
        {
            BigFloat remainder = BigFloat.Decimals(value);

            if (remainder.CompareTo(OneHalf) >= 0)
                return BigFloat.Ceiling(value);
            else
                return BigFloat.Floor(value);
        }

        // TODO Add other rounding methods

        /// <summary>
        /// Truncates <paramref name="value"/> to the nearest smaller integer
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> to be truncated</param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the truncated value of <paramref name="value"/></returns>
        /// <remarks>This overload is more efficient then <see cref="BigFloat.Truncate(BigFloat, bool)"/> but the 
        /// fractional representation is not preserved, the <see cref="BigFloat.Denominator"/> will 
        /// always be 1 (one)</remarks>
        public static BigFloat Truncate(BigFloat value)
        {
            return new BigFloat(BigInteger.Divide(value.Numerator, value.Denominator), BigInteger.One);
        }

        /// <summary>
        /// Truncates <paramref name="value"/> to the nearest smaller integer with the option of maintaining 
        /// the fractional representation
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> to be truncated</param>
        /// <param name="KeepDenominator">A boolean flag indicating if the fractional representation should 
        /// be maintained</param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the truncated value of <paramref name="value"/></returns>
        /// <remarks>If <paramref name="KeepDenominator"/> is set to <c>true</c> then this overload is less 
        /// efficient than <see cref="BigFloat.Truncate(BigFloat)"/></remarks>
        public static BigFloat Truncate(BigFloat value, bool KeepDenominator)
        {
            if (KeepDenominator)
            {
                BigInteger newNumerator = value.Numerator;
                newNumerator = BigInteger.Subtract(newNumerator, BigInteger.Remainder(value.Numerator, value.Denominator));

                return new BigFloat(newNumerator, value.Denominator);
            }
            else
            {
                return BigFloat.Truncate(value);
            }
        }

        /// <summary>
        /// Calculates the decimal part of a <see cref="BigFloat"/> number
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> for which the decimal part is required</param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the decimal part only of <paramref name="value"/></returns>
        public static BigFloat Decimals(BigFloat value)
        {
            BigInteger result = BigInteger.Remainder(value.Numerator, value.Denominator);

            return new BigFloat(result, value.Denominator);
        }

        /// <summary>
        /// Shifts the decimal point of a <see cref="BigFloat"/> number to the left by <paramref name="shift"/>
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> to be shifted left</param>
        /// <param name="shift">The number of places to ShiftLeft</param>
        /// <returns>A new <see cref="BigFloat"/> instance that is <paramref name="value"/> left shifted by 
        /// <paramref name="shift"/> places</returns>
        public static BigFloat ShiftDecimalLeft(BigFloat value, int shift)
        {
            BigInteger newNumerator = value.Numerator;

            if (shift < 0)
                return ShiftDecimalRight(value, -shift);

            newNumerator = BigInteger.Multiply(newNumerator, BigInteger.Pow(10, shift));

            return new BigFloat(newNumerator, value.Denominator);
        }

        /// <summary>
        /// Shifts the decimal point of a <see cref="BigFloat"/> number to the right by <paramref name="shift"/>
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> to be shifted right</param>
        /// <param name="shift">The number of places to ShiftRight</param>
        /// <returns>A new <see cref="BigFloat"/> instance that is <paramref name="value"/> right shifted by 
        /// <paramref name="shift"/> places</returns>
        public static BigFloat ShiftDecimalRight(BigFloat value, int shift)
        {
            BigInteger newDenominator = value.Denominator;

            if (shift < 0)
                return ShiftDecimalLeft(value, -shift);

            newDenominator = BigInteger.Multiply(newDenominator, BigInteger.Pow(10, shift));

            return new BigFloat(value.Numerator, newDenominator);
        }

        /// <summary>
        /// Calculates the square root of a <see cref="BigFloat"/> number
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> of which the square root will be calculated</param>
        /// <returns>A new <see cref="double"/> that is the square root of <paramref name="value"/></returns>
        /// <remarks>Because this method returns a <see cref="double"/> and uses <see cref="double"/> 
        /// values in the intermediate calculations it will not maintain precision. 
        /// <see cref="BigFloat.Sqrt(BigFloat, double)"/> addresses this limitation but 
        /// may result in slower calculation times</remarks>
        public static double Sqrt(BigFloat value)
        {
            return Math.Pow(10, BigInteger.Log10(value.Numerator) / 2) / Math.Pow(10, BigInteger.Log10(value.Denominator) / 2);
        }

        /// <summary>
        /// Calculates the square root of a <see cref="BigFloat"/> number with fractional approximations
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> of which the square root will be calculated</param>
        /// <param name="tolerance">The precision tolerance for the approximation (a default of 1e-20 is suggested)</param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the square root of <paramref name="value"/></returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is negative.</exception>
        public static BigFloat Sqrt(BigFloat value, double tolerance)
        {
            if (value.Numerator < 0)
                throw new ArgumentException("Cannot calculate the square root of a negative BigFloat.", nameof(value));

            if (value.Numerator == 0)
                return BigFloat.Zero;

            // Initial guess: Use the square root of the numerator and denominator as a starting point
            BigFloat guess = new(SqrtBigInteger(value.Numerator), SqrtBigInteger(value.Denominator));

            // Iteratively refine the guess using Newton's Method
            BigFloat previousGuess;
            do
            {
                previousGuess = guess;
                guess = (previousGuess + value / previousGuess) / 2;
            }
            while (BigFloat.Abs(guess - previousGuess) > new BigFloat(tolerance));

            return guess;
        }

        /// <summary>
        /// Calculates the integer square root of a <see cref="BigInteger"/> using the Newton-Raphson method
        /// </summary>
        /// <param name="value">The <see cref="BigInteger"/> to calculate the square root of.</param>
        /// <returns>The integer square root of <paramref name="value"/></returns>
        private static BigInteger SqrtBigInteger(BigInteger value)
        {
            if (value < 0)
                throw new ArgumentException("Cannot calculate the square root of a negative BigInteger.", nameof(value));

            if (value == 0 || value == 1)
                return value;

            // Initial guess (value / 2)
            BigInteger guess = value >> 1;
            BigInteger result = (guess + value / guess) >> 1;

            // Iterate until the result stabilizes
            while (result < guess)
            {
                guess = result;
                result = (guess + value / guess) >> 1;
            }

            return result;
        }

        /// <summary>
        /// Calculates the logarithm of a <see cref="BigFloat"/> number to the specified base and returns the 
        /// result as a <see cref="BigFloat"/>
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> of which the logarithm will be calculated</param>
        /// <param name="baseValue">The base for the logarithm calculation</param>
        /// <returns>A new <see cref="BigFloat"/> instance that is the logarithm of <paramref name="value"/> to 
        /// the base <paramref name="baseValue"/></returns>
        public static BigFloat Log(BigFloat value, double baseValue)
        {
            if (value.Numerator <= 0)
                throw new ArgumentException("Logarithm is undefined for non-positive values.", nameof(value));

            if (baseValue is <= 0 or 1)
                throw new ArgumentException("Logarithm base must be positive and not equal to 1.", nameof(baseValue));

            // Calculate the natural logarithm (log_e) of the numerator and denominator
            BigFloat logNumerator = LogBigInteger(value.Numerator);
            BigFloat logDenominator = LogBigInteger(value.Denominator);

            // Calculate the natural logarithm of the BigFloat
            BigFloat naturalLog = logNumerator - logDenominator;

            // Convert the base using the change of base formula: log_base(value) = log(value) / log(baseValue)
            BigFloat logBase = LogBigInteger((BigInteger)baseValue);
            return naturalLog / logBase;
        }

        /// <summary>
        /// Calculates the natural logarithm (base e) of a <see cref="BigFloat"/> number
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> of which the natural logarithm will be calculated</param>
        /// <returns>A new <see cref="double"/> that is the natural logarithm of <paramref name="value"/></returns>
        public static BigFloat Log(BigFloat value)
        {
            return BigFloat.Log(value, Math.E);
        }

        /// <summary>
        /// Calculates the base 10 logarithm of a <see cref="BigFloat"/> number
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> of which the base 10 logarithm will be calculated</param>
        /// <returns>A new <see cref="double"/> that is the base 10 logarithm of <paramref name="value"/></returns>
        public static BigFloat Log10(BigFloat value)
        {
            return BigFloat.Log(value, (double)10);
        }

        /// <summary>
        /// Calculates the logarithm of a <see cref="BigInteger"/> to the specified base and returns the 
        /// result as a <see cref="BigFloat"/>
        /// </summary>
        /// <param name="value">The <see cref="BigInteger"/> for which the logarithm will be calculated</param>
        /// <param name="baseValue">The base of the logarithm. Defaults to e (natural logarithm) if not specified</param>
        /// <returns>The logarithm of <paramref name="value"/> to the specified base as a <see cref="BigFloat"/></returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is less than or equal to zero, or if <paramref name="baseValue"/> is less than or equal to zero or equal to 1.</exception>
        private static BigFloat LogBigInteger(BigInteger value, BigFloat baseValue = default)
        {
            if (value <= 0)
                throw new ArgumentException("Logarithm is undefined for non-positive values.", nameof(value));
            if (baseValue <= BigFloat.Zero || baseValue == BigFloat.One)
                throw new ArgumentException("Logarithm base must be positive and not equal to 1.", nameof(baseValue));

            // Default base to e (natural logarithm) if not provided
            if (baseValue == default)
                baseValue = new BigFloat(Math.E);

            // Step 1: Estimate the logarithm using the number of digits
            int numberOfDigits = (int)Math.Floor(BigInteger.Log10(value)) + 1;
            BigFloat logEstimate = new BigFloat(numberOfDigits - 1) * BigFloat.Log10(new BigFloat(10));

            // Step 2: Refine the estimate using the first digit
            BigInteger divisor = BigInteger.Pow(10, numberOfDigits - 1);
            BigFloat firstDigit = new(value / divisor);
            logEstimate += BigFloat.Log10(firstDigit);

            // Step 3: Convert to the specified base
            if (baseValue != new BigFloat(Math.E))
            {
                logEstimate /= BigFloat.Log10(baseValue);
            }

            return logEstimate;
        }

        #endregion

        #region IComparable Support

        /// <summary>
        /// Compares this instance to a specified <see cref="BigFloat"/> object and returns an integer 
        /// that indicates their numerical relationship, less than, equal to or greater than
        /// </summary>
        /// <param name="other">The <see cref="BigFloat"/> to be compared to this instance</param>
        /// <returns><list type="bullet">
        /// <item><description>Returns <c><0</c> if this instance is less than <paramref name="other"/></description></item>
        /// <item><description>Returns <c>0</c> if this instance is equal to <paramref name="other"/></description></item>
        /// <item><description>Returns <c>>0</c> if this instance is greater than <paramref name="other"/></description></item>
        /// </list></returns>
        /// <exception cref="ArgumentNullException">This exception is thrown in <paramref name="other"/> is <c>null</c></exception>
        /// <example><list type="bullet">
        /// <item><description>3.5 (7/2) will be greater than 3.4 (17/5)</description></item>
        /// <item><description>1.5 (1/2) will be equal to 1.5 (1/2)</description></item>
        /// <item><description>1.5 (1/2) will be equal to 1.5 (4/8)</description></item>
        /// <item><description>2.3 (23/10) will be less than 3.2 (16/5)</description></item>
        /// </list></example>
        /// <remarks><para>To compare two fractions (this instance and <paramref name="other"/>), the method uses cross multiplication. 
        /// The numerator of this instance is multiplied by the denominator of <paramref name="other"/>, and the numerator 
        /// of <paramref name="other"/> is multiplied by the denominator of this instance. The two resulting 
        /// <see cref="BigInteger"/> values are compared using <see cref="BigInteger.Compare(BigInteger, BigInteger)"/></para>
        /// <para>The <see cref="BigFloat.Equals(BigFloat)"/> can be used to test for equality and requires that 
        /// the two <see cref="BigFloat.Denominator"/> values be equal</para></remarks>
        public int CompareTo(BigFloat other)
        {
            if (BigFloat.Equals(other, null))
                throw new ArgumentNullException(nameof(other));

            //Make copies
            BigInteger one = this.Numerator;
            BigInteger two = other.Numerator;

            //cross multiply
            one *= other.Denominator;
            two *= this.Denominator;

            //test
            return BigInteger.Compare(one, two);
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="object"/> and returns an integer 
        /// that indicates their numerical relationship, less than, equal to or greater than.
        /// </summary>
        /// <param name="other">The <see cref="BigFloat"/> to be compared to this instance</param>
        /// <returns><list type="bullet">
        /// <item><description>Returns <c><0</c> if this instance is less than <paramref name="other"/></description></item>
        /// <item><description>Returns <c>0</c> if this instance is equal to <paramref name="other"/></description></item>
        /// <item><description>Returns <c>>0</c> if this instance is greater than <paramref name="other"/></description></item>
        /// </list></returns>
        /// <exception cref="ArgumentNullException">This exception is thrown if <paramref name="obj"/> is <c>null</c></exception>
        /// <exception cref="System.ArgumentException">This exceptiion is thrown if <paramref name="obj"/> is not an 
        /// instance of a <see cref="BigFloat"/></exception>
        public int CompareTo(object? obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (obj is not BigFloat)
                throw new System.ArgumentException(nameof(obj) + " is not a BigFloat");

            return CompareTo((BigFloat)obj);
        }

        /// <summary>
        /// Compares two <see cref="BigFloat"/> values and returns an integer 
        /// that indicates their numerical relationship, less than, equal to or greater than.
        /// </summary>
        /// <param name="left">The <see cref="BigFloat"/> to be compared to <paramref name="right"/></param>
        /// <param name="right">The <see cref="BigFloat"/> to be compared to <paramref name="left"/></param>
        /// <returns><list type="bullet">
        /// <item><description>Returns <c><0</c> if this instance is less than <paramref name="other"/></description></item>
        /// <item><description>Returns <c>0</c> if this instance is equal to <paramref name="other"/></description></item>
        /// <item><description>Returns <c>>0</c> if this instance is greater than <paramref name="other"/></description></item>
        /// </list></returns>
        /// <exception cref="ArgumentNullException">This exception is thrown if either <paramref name="left"/> or 
        /// <paramref name="right"/> is null</exception>
        public static int Compare(BigFloat left, BigFloat right)
        {
            if (BigFloat.Equals(left, null))
                throw new ArgumentNullException(nameof(left));
            if (BigFloat.Equals(right, null))
                throw new ArgumentNullException(nameof(right));

            return (new BigFloat(left)).CompareTo(right);
        }

        #endregion

        #region Object Methods

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to be compared to this instance for equality</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is equal to this instance; <c>false</c> if 
        /// <paramref name="obj"/> is <c>null</c> or if <paramref name="obj"/> is not equal to 
        /// this instance</returns>
        /// <example><list type="bullet">
        /// <item><description>1.5 (1/2) will be equal to 1.5 (1/2)</description></item>
        /// <item><description>1.5 (1/2) will not be equal to 1.5 (4/8)</description></item>
        /// <item><description>2.3 (23/10) will not be equal to 3.2 (16/5)</description></item>
        /// </list></example>
        /// <remarks>This method checks if the two <see cref="BigFloat.Numerator"/> values are equal and if the 
        /// two <see cref="BigFloat.Denominator"/> values are equal. Hence the same value stored with different 
        /// <see cref="BigFloat.Denominator"/> values will not be considered equal. The 
        /// <see cref="BigFloat.CompareTo(BigFloat)"/> test allows two <see cref="BigFloat"/> values to be 
        /// considered equal even if their <see cref="BigFloat.Denominator"/> values are different</remarks>
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return this.Equals((BigFloat)obj);
        }

        /// <summary>
        /// Determines whether the specified <see cref="BigFloat"/> is equal to this instance
        /// </summary>
        /// <param name="other">The <see cref="BigFloat"/> to be compared to this instance for equality</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to this instance; <c>false</c> if 
        /// <paramref name="other"/> is not equal to this instance</returns>
        /// <example><list type="bullet">
        /// <item><description>1.5 (1/2) will be equal to 1.5 (1/2)</description></item>
        /// <item><description>1.5 (1/2) will not be equal to 1.5 (4/8)</description></item>
        /// <item><description>2.3 (23/10) will not be equal to 3.2 (16/5)</description></item>
        /// </list></example>
        /// <remarks>This method checks if the two <see cref="BigFloat.Numerator"/> values are equal and if the 
        /// two <see cref="BigFloat.Denominator"/> values are equal. Hence the same value stored with different 
        /// <see cref="BigFloat.Denominator"/> values will not be considered equal. The 
        /// <see cref="BigFloat.CompareTo(BigFloat)"/> test allows two <see cref="BigFloat"/> values to be 
        /// considered equal even if their <see cref="BigFloat.Denominator"/> values are different</remarks>
        public bool Equals(BigFloat other)
        {
            return (other.Numerator == this.Numerator && other.Denominator == this._denominator);
        }

        /// <summary>
        /// Returns a hash code for this instance
        /// </summary>
        /// <returns>An integer hash code that uniquely represents this instance</returns>
        /// <remarks>The <see cref="BigFloat"/> is not reduced prior to calculating the hash code for 
        /// consistency with <see cref="BigFloat.Equals(BigFloat)"/>, hence 0.5 (1/20 and 0.5 (2/4) will 
        /// nor return the same hash code</remarks>
        public override int GetHashCode()
        {
            return HashCode.Combine(this._numerator, this._denominator);
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns a string representation of this instance
        /// </summary>
        /// <returns>A string that is a decimal representation of the value of this <see cref="BigFloat"/></returns>
        /// <remarks>Internally this method uses <see cref="BigFloat.ToString(string, IFormatProvider)"/> with 
        /// <see cref="CultureInfo.CurrentCulture"/> as the <see cref="IFormatProvider"/>, and the default 
        /// format "G" (General)</remarks>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture); // Default format
        }

        /// <summary>
        /// Returns a string representation of this instance
        /// </summary>
        /// <param name="format">A numeric format string that specifies how the decimal result of 
        /// the division <see cref="BigFloat.Numerator"/>/<see cref="BigFloat.Denominator"/> will 
        /// be displayed, <see href="https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings"/></param>
        /// <param name="formatProvider">The <see cref="IFormatProvider"/> that specifies how the 
        /// <see cref="string"/> will be formatted</param>
        /// <returns>A string that is a decimal representation of the value of this <see cref="BigFloat"/></returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            // Handle special cases
            if (this._denominator == 0)
                return this._numerator > 0 ? "Infinity" : "-Infinity";
            if (this._numerator == 0)
                return "0";

            // Default to "G" format if none is specified
            format ??= "G";
            var result = (double)this._numerator / (double)this._denominator;

            return result.ToString(format, formatProvider);
        }

        /// <summary>
        /// Returns a string representation of this instance
        /// </summary>
        /// <returns>A string representation of this <see cref="BigFloat"/> in fractional form</returns>
        /// <remarks>The first fraction is the <see cref="BigFloat"/> as stored in memory and the second 
        /// is the simplified form. These two will be the same if the fraction is already in its most 
        /// simple form</remarks>
        public string ToFractionString()
        {
            // Handle special cases
            if (this._denominator == 0)
                return this._numerator > 0 ? "Infinity" : "-Infinity";
            if (Numerator == 0)
                return "0";

            // Simplify the fraction
            BigFloat simplified = BigFloat.Reduce(this);

            return $"{this._numerator}/{this._denominator} {simplified.Numerator}/{simplified.Denominator}";
        }

        /// <summary>
        /// Returns a string representation of this instance
        /// </summary>
        /// <param name="format">A numeric format string that specifies how the decimal result of 
        /// the division <see cref="BigFloat.Numerator"/>/<see cref="BigFloat.Denominator"/> will 
        /// be displayed, <see href="https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings"/></param>
        /// <param name="formatProvider">The <see cref="IFormatProvider"/> that specifies how the 
        /// decimal representation will be formatted</param>
        /// <returns>A string representation of this <see cref="BigFloat"/> in both fractional and 
        /// decimal form</returns>
        /// <remarks>Internally this method uses <see cref="BigFloat.ToString(string, IFormatProvider)"/> 
        /// to provide the decimal representation</remarks>
        public string ToExactPrecisionView(string format, IFormatProvider formatProvider)
        {
            // Handle special cases
            if (Denominator == 0)
                return Numerator > 0 ? "Infinity" : "-Infinity";
            if (Numerator == 0)
                return "0";

            var result = (double)Numerator / (double)Denominator;
            return $"{Numerator}/{Denominator}" + result.ToString(format, formatProvider); // Shows fraction and approximate value
        }

        #endregion

        #region Explicit Type Conversion

        /// <summary>
        /// Converts a <see cref="BigFloat"/> to a <see cref="int"/>
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> to convert to a <see cref="int"/></param>
        /// <exception cref="OverflowException">This exception is thrown if <paramref name="value"/> is 
        /// outside the range of a <see cref="int"/></exception>
        public static explicit operator int(BigFloat value)
        {
            if (new BigFloat(decimal.MinValue) > value) throw new OverflowException("value is less than System.decimal.MinValue.");
            if (new BigFloat(decimal.MaxValue) < value) throw new OverflowException("value is greater than System.decimal.MaxValue.");

            return (int)((decimal)value.Numerator / (decimal)value.Denominator);
        }

        /// <summary>
        /// Converts a <see cref="BigFloat"/> to a <see cref="decimal"/>
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> to convert to a <see cref="decimal"/></param>
        /// <exception cref="OverflowException">This exception is thrown if <paramref name="value"/> is 
        /// outside the range of a <see cref="decimal"/></exception>
        public static explicit operator decimal(BigFloat value)
        {
            if (new BigFloat(decimal.MinValue) > value) throw new OverflowException("value is less than System.decimal.MinValue.");
            if (new BigFloat(decimal.MaxValue) < value) throw new OverflowException("value is greater than System.decimal.MaxValue.");

            return (decimal)value.Numerator / (decimal)value.Denominator;
        }

        /// <summary>
        /// Converts a <see cref="BigFloat"/> to a <see cref="double"/>
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> to convert to a <see cref="double"/></param>
        /// <exception cref="OverflowException">This exception is thrown if <paramref name="value"/> is 
        /// outside the range of a <see cref="double"/></exception>
        public static explicit operator double(BigFloat value)
        {
            if (new BigFloat(double.MinValue) > value) throw new System.OverflowException("value is less than System.double.MinValue.");
            if (new BigFloat(double.MaxValue) < value) throw new System.OverflowException("value is greater than System.double.MaxValue.");

            return (double)value.Numerator / (double)value.Denominator;
        }

        /// <summary>
        /// Converts a <see cref="BigFloat"/> to a <see cref="float"/>
        /// </summary>
        /// <param name="value">The <see cref="BigFloat"/> to convert to a <see cref="float"/></param>
        /// <exception cref="OverflowException">This exception is thrown if <paramref name="value"/> is 
        /// outside the range of a <see cref="float"/></exception>
        public static explicit operator float(BigFloat value)
        {
            if (new BigFloat(float.MinValue) > value) throw new System.OverflowException("value is less than System.float.MinValue.");
            if (new BigFloat(float.MaxValue) < value) throw new System.OverflowException("value is greater than System.float.MaxValue.");

            return (float)value.Numerator / (float)value.Denominator;
        }

        /// <summary>
        /// Converts a <see cref="string"/> to a <see cref="BigFloat"/>
        /// </summary>
        /// <param name="value">The <see cref="string"/> to convert to a <see cref="BigFloat"/></param>
        public static explicit operator BigFloat(string value)
        {
            return new BigFloat(value);
        }

        #endregion

        #region Implicit Type Conversion

        /// <summary>
        /// Converts a <see cref="byte"/> to a <see cref="BigFloat"/>
        /// </summary>
        /// <param name="value">The <see cref="byte"/> to convert to a <see cref="BigFloat"/></param>
        public static implicit operator BigFloat(byte value)
        {
            return new BigFloat((uint)value);
        }

        /// <summary>
        /// Converts a <see cref="sbyte"/> to a <see cref="BigFloat"/>
        /// </summary>
        /// <param name="value">The <see cref="sbyte"/> to convert to a <see cref="BigFloat"/></param>
        public static implicit operator BigFloat(sbyte value)
        {
            return new BigFloat((int)value);
        }

        /// <summary>
        /// Converts a <see cref="short"/> to a <see cref="BigFloat"/>
        /// </summary>
        /// <param name="value">The <see cref="short"/> to convert to a <see cref="BigFloat"/></param>
        public static implicit operator BigFloat(short value)
        {
            return new BigFloat((int)value);
        }

        /// <summary>
        /// Converts a <see cref="ushort"/> to a <see cref="BigFloat"/>
        /// </summary>
        /// <param name="value">The <see cref="ushort"/> to convert to a <see cref="BigFloat"/></param>
        public static implicit operator BigFloat(ushort value)
        {
            return new BigFloat((uint)value);
        }

        /// <summary>
        /// Converts a <see cref="int"/> to a <see cref="BigFloat"/>
        /// </summary>
        /// <param name="value">The <see cref="int"/> to convert to a <see cref="BigFloat"/></param>
        public static implicit operator BigFloat(int value)
        {
            return new BigFloat(value);
        }

        /// <summary>
        /// Converts a <see cref="long"/> to a <see cref="BigFloat"/>
        /// </summary>
        /// <param name="value">The <see cref="long"/> to convert to a <see cref="BigFloat"/></param>
        public static implicit operator BigFloat(long value)
        {
            return new BigFloat(value);
        }

        /// <summary>
        /// Converts a <see cref="uint"/> to a <see cref="BigFloat"/>
        /// </summary>
        /// <param name="value">The <see cref="uint"/> to convert to a <see cref="BigFloat"/></param>
        public static implicit operator BigFloat(uint value)
        {
            return new BigFloat(value);
        }

        /// <summary>
        /// Converts a <see cref="ulong"/> to a <see cref="BigFloat"/>
        /// </summary>
        /// <param name="value">The <see cref="ulong"/> to convert to a <see cref="BigFloat"/></param>
        public static implicit operator BigFloat(ulong value)
        {
            return new BigFloat(value);
        }

        /// <summary>
        /// Converts a <see cref="decimal"/> to a <see cref="BigFloat"/>
        /// </summary>
        /// <param name="value">The <see cref="decimal"/> to convert to a <see cref="BigFloat"/></param>
        public static implicit operator BigFloat(decimal value)
        {
            return new BigFloat(value);
        }

        /// <summary>
        /// Converts a <see cref="double"/> to a <see cref="BigFloat"/>
        /// </summary>
        /// <param name="value">The <see cref="double"/> to convert to a <see cref="BigFloat"/></param>
        public static implicit operator BigFloat(double value)
        {
            return new BigFloat(value);
        }

        /// <summary>
        /// Converts a <see cref="float"/> to a <see cref="BigFloat"/>
        /// </summary>
        /// <param name="value">The <see cref="float"/> to convert to a <see cref="BigFloat"/></param>
        public static implicit operator BigFloat(float value)
        {
            return new BigFloat(value);
        }

        /// <summary>
        /// Converts a <see cref="BigInteger"/> to a <see cref="BigFloat"/>
        /// </summary>
        /// <param name="value">The <see cref="BigInteger"/> to convert to a <see cref="BigFloat"/></param>
        public static implicit operator BigFloat(BigInteger value)
        {
            return new BigFloat(value);
        }

        #endregion

    }
}