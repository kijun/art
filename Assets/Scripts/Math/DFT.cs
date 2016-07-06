using UnityEngine;
using System.Collections.Generic;

public static class DFT {
    public static Complex DrawSample(Dictionary<float, Complex> coefficients, int n, int N) {
        Complex sum = Complex.Zero;

        foreach(KeyValuePair<float, Complex> coeff in coefficients) {
            float k = coeff.Key;
            Complex Xk = coeff.Value;
            float arg = Mathf.PI*2*k*n/N;

            sum += Xk * new Complex(Mathf.Cos(arg), Mathf.Sin(arg));
        }

        return sum;
    }

    public static Complex[] GenerateSamples(Dictionary<float, Complex> coefficients, int N) {
      Complex[] data = new Complex[N];

      for (int n = 0; n<N; n++) {
          data[n] = DrawSample(coefficients, n, N);
      }

      return data;
    }
}
