using UnityEngine;

public struct Complex {
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
}
