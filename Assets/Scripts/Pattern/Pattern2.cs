using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
public class Pattern2 : MonoBehaviour {
    public int numberOfLines;
    public float lineLength;
    public float lineWidth;
    public float orbitalPeriod; // time for the smallest circle
    public float lineGap;
    public int numberOfArms;

    List<LineProperty> lines = new List<LineProperty>();
    List<LineRenderer> lineRenderers = new List<LineRenderer>();


    // Use this for initialization
    void Start () {
        for (int i=0; i<numberOfLines; i++) {
            var property = new LineProperty(
                center: new Vector2(lineGap*i, 0),
                length: lineLength,
                width: lineWidth,
                color: Color.black
            );

            var lineObj = ResourceLoader.InstantiateLine(property);
            lineRenderers.Add(lineObj);
            lines.Add(property);
        }
        StartCoroutine(Run());
    }

    IEnumerator Run() {
        yield return new WaitForSeconds(1);
        float startTime = Time.time;
        while (true) {
            for (int i = 0; i<numberOfLines; i++) {
                var progress = (Time.time - startTime) / orbitalPeriod;
                var angle = 360 * progress;
                var line = lines[i];
                line.angle = angle/numberOfLines*(i+1);
                //Debug.Log("new angle" + line.angle);
                line.center = InterpolationUtil.UnitVectorWithAngle(line.angle)*(lineGap*i);

                lineRenderers[i].property = line;
            }
            yield return null;
        }
    }
}
*/
