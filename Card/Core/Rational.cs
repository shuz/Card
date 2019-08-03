using System;
using System.Collections.Generic;
using System.Text;

namespace Card.Core
{
    public sealed class Rational : IComparable<Rational>
    {
        private int numerator, denominator;

        public Rational(int numerator, int denominator)
        {
            this.numerator = numerator;
            this.denominator = denominator;

            Normalize();
        }

        private void Normalize()
        {
            if (numerator != 0 && denominator != 0)
            {
                int sign = Math.Sign(numerator) * Math.Sign(denominator);
                int gcd = Utility.GCD(numerator, denominator);

                numerator = sign * Math.Abs(numerator) / gcd;
                denominator = Math.Abs(denominator) / gcd;
            }
            else
            if (numerator == 0 && denominator != 0)
            {
                numerator = 0;
                denominator = 1;
            }
            else
            {
                numerator = denominator = 0;
            }
        }

        public int Numerator
        {
            get { return numerator; }
        }

        public int Denominator
        {
            get { return denominator; }
        }

        public bool IsInteger
        {
            get { return denominator == 1; }
        }

        public override bool Equals(object obj)
        {
            if (obj is Rational)
            {
                Rational that = obj as Rational;
                return this.numerator == that.numerator && this.denominator == that.denominator;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return numerator * 65537 + denominator;
        }

        public override string ToString()
        {
            if (this.IsInteger)
            {
                return numerator.ToString();
            }
            else
            {
                return string.Format("{0}/{1}", numerator, denominator);
            }
        }

        public static implicit operator Rational(int value)
        {
            return new Rational(value, 1);
        }

        public static Rational operator -(Rational rational)
        { 
            return new Rational(-rational.numerator, rational.denominator); 
        }

        public static Rational operator +(Rational lhs, Rational rhs)
        { 
            return new Rational(
                lhs.numerator * rhs.denominator + rhs.numerator * lhs.denominator, 
                lhs.denominator * rhs.denominator); 
        }

        public static Rational operator -(Rational lhs, Rational rhs)
        { 
            return new Rational(
                lhs.numerator * rhs.denominator - rhs.numerator * lhs.denominator,
                lhs.denominator * rhs.denominator); 
        }

        public static Rational operator *(Rational lhs, Rational rhs)
        { 
            return new Rational(lhs.numerator * rhs.numerator, lhs.denominator * rhs.denominator); 
        }

        public static Rational operator /(Rational lhs, Rational rhs)
        {
            return new Rational(lhs.numerator * rhs.denominator, lhs.denominator * rhs.numerator); 
        }

        public static bool operator ==(Rational lhs, Rational rhs)
        { 
            return lhs.Equals(rhs); 
        }

        public static bool operator !=(Rational lhs, Rational rhs)
        { 
            return !lhs.Equals(rhs); 
        }

        public static bool operator <(Rational lhs, Rational rhs)
        { 
            return lhs.numerator * rhs.denominator < rhs.numerator * lhs.denominator; 
        }

        public static bool operator >(Rational lhs, Rational rhs)
        {
            return lhs.numerator * rhs.denominator > rhs.numerator * lhs.denominator; 
        }

        public static bool operator <=(Rational lhs, Rational rhs)
        {
            return lhs.numerator * rhs.denominator <= rhs.numerator * lhs.denominator; 
        }

        public static bool operator >=(Rational lhs, Rational rhs)
        {
            return lhs.numerator * rhs.denominator >= rhs.numerator * lhs.denominator; 
        }

        public int CompareTo(Rational that)
        {
            return this.numerator * that.denominator - that.numerator * this.denominator;
        }
    }

}
