using System;
using UnityEngine;

public struct Complex : IEquatable<Complex>, IEquatable<float> {
    public static readonly Complex I = new Complex(0, 1);
    public static readonly Complex Zero = new Complex(0, 0);

    public readonly float real;
    public readonly float img;

    public Complex(float real, float img) {
        this.real = real;
        this.img = img;
    }

    /* Static Methods */

    public static Complex FromDegrees(float angleInDeg) {
      return Complex.FromRadian(Mathf.Deg2Rad * angleInDeg);
    }

    public static Complex FromRadian(float radian) {
      //Debug.Log("deg = " + radian*Mathf.Rad2Deg);
      return new Complex(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Complex operator +(Complex c1, Complex c2)
    {
       return new Complex(c1.real + c2.real, c1.img + c2.img);
    }

    public static Complex operator *(Complex c1, Complex c2)
    {
       return new Complex(c1.real * c2.real - c1.img * c2.img,
                          c1.real * c2.img + c1.img * c2.real);
    }

    public static implicit operator Complex(float r) {
        return new Complex(r, 0);
    }

    /* Operator Overload */
    public static bool operator ==(Complex term1, Complex term2) {
        return term1.Equals(term2);
    }

    public static bool operator !=(Complex term1, Complex term2) {
        return !term1.Equals(term2);
    }

    /* Properties */

    public float magnitude {
        get {
            return Mathf.Sqrt(real*real + img*img);
        }
    }

    public Complex normalized {
        get {
          float mag = magnitude;
          return new Complex(real/mag, img/mag);
        }
    }

    /* Overrides */
    public override string ToString() {
        return real + "+" + img + "i";
    }

    public bool Equals(Complex other) {
        return Mathf.Approximately(real, other.real) &&
               Mathf.Approximately(img, other.img);
    }

    public bool Equals(float otherReal) {
        return Mathf.Approximately(real, otherReal) &&
               Mathf.Approximately(img, 0);
    }

    public override bool Equals(object other) {
        if (other is Complex) {
            return Equals((Complex)other);
        }
        if (other is double) {
            return Equals((double)other);
        }
        return false;
    }

    public override int GetHashCode() {
        return real.GetHashCode() ^ img.GetHashCode();
    }
}
