using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// does a harmonic pattern include shapes?
public class HarmonicSequenceGenerator {
    public float duration;
    public int N;
    public HarmonicFrequencyGenerator[] frequencies;

    public HarmonicSequenceGenerator(float duration, int N, HarmonicFrequencyGenerator[] frequencies) {
        this.duration = duration;
        this.N = N;
        this.frequencies = frequencies;
    }

    public static HarmonicSequenceGenerator Star(float duration, int N) {
        HarmonicFrequencyGenerator[] freqs = {HarmonicFrequencyGenerator.Constant(1, new Complex(0, 1))};
        return new HarmonicSequenceGenerator(duration, N, freqs);
    }

    // TODO actually might be able to chain all of these
    public static HarmonicSequenceGenerator RotatingStar(float duration, int N) {
        HarmonicFrequencyGenerator[] freqs = {HarmonicFrequencyGenerator.Rotation(
                frequency:1, secPerRotation:3f, scale:1)};
        return new HarmonicSequenceGenerator(duration, N, freqs);
    }

    public Complex[] GenerateSamples(float progress, float elapsedTime) {
        var coeff = new Dictionary<float, Complex>();
        foreach (var fp in frequencies) {
            HarmonicFrequency freq = fp.GenerateFrequency(progress:progress, elapsedTime:elapsedTime);
            coeff.Add(freq.frequency, freq.coefficient);
        }
        return DFT.GenerateSamples(coeff, N);
    }
}


