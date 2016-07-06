// could make some default delegates
using UnityEngine;

delegate float FrequencyDelegate(float progress, float elapsedTime);
delegate Complex CoeffcientDelegate(float progress, float elapsedTime);

static class FrequencyDelegates {
    public static FrequencyDelegate Constant(float frequency) {
        return delegate(float progress, float elapsedTime) { return frequency; };
    }
}

static class CoeffcientDelegates {
    public static CoeffcientDelegate Constant(Complex coeff) {
        return delegate(float progress, float elapsedTime) { return coeff; };
    }

    public static CoeffcientDelegate Rotation(float secPerRotation, float scale) {
        return delegate(float progress, float elapsedTime) {
            return Complex.FromRadian((elapsedTime/secPerRotation)*Mathf.PI*2)*scale;
        };
    }
}

public class HarmonicFrequencyGenerator {
    FrequencyDelegate frequencyTransform;
    CoeffcientDelegate coefficientTransform;

    HarmonicFrequencyGenerator(FrequencyDelegate freqDel,
                               CoeffcientDelegate coeffDel) {
        frequencyTransform = freqDel;
        coefficientTransform = coeffDel;
    }

    public HarmonicFrequency GenerateFrequency(float progress, float elapsedTime) {
        return new HarmonicFrequency(frequencyTransform(progress, elapsedTime),
                                     coefficientTransform(progress, elapsedTime));
    }

    public static HarmonicFrequencyGenerator Constant(float frequency, Complex coeff) {
        FrequencyDelegate freqDel = delegate(float progress, float elapsedTime) { return frequency; };
        CoeffcientDelegate coeffDel = delegate(float progress, float elapsedTime) { return coeff; };

        return new HarmonicFrequencyGenerator(freqDel, coeffDel);
    }

    public static HarmonicFrequencyGenerator Rotation(float frequency, float secPerRotation, float scale) {
        FrequencyDelegate freqDel =  FrequencyDelegates.Constant(frequency);
        CoeffcientDelegate coeffDel = CoeffcientDelegates.Rotation(secPerRotation, scale);

        return new HarmonicFrequencyGenerator(freqDel, coeffDel);

    }
}



