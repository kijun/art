[System.Serializable]
public struct TimeSignature {
    public float beatsPerMinute;
    public int beatUnitLength;
    public int beatsPerMeasure;

    public float beatDuration {
        get {
            return 60f / beatsPerMinute;
        }
    }

    public int MeasureFromTime(float time) {
        return (int)((time / beatDuration) / beatsPerMeasure);
    }

    public int BeatFromTime(float time) {
        // From 1 to beatsPerMeasure
        return (int)(time / beatDuration) - MeasureFromTime(time) * beatsPerMeasure + 1;
    }

    public string TimeToString(float time) {
        return MeasureFromTime(time) + " " +
               BeatFromTime(time) + "/" + beatsPerMeasure;
    }
}

