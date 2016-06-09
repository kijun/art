using UnityEngine;

public class Bezier {

    private Vector2[] points;
    private float[] constants;

    public Bezier( Vector2 A, Vector2 C1, Vector2 C2, Vector2 B ) {
        points = new Vector2[4];
        points[0] = A;
        points[1] = C1;
        points[2] = C2;
        points[3] = B;

        constants = new float[6];
        CalcConstants();
    }

    private void CalcConstants() {
        constants[2] = 3 * ( points[1].x - points[0].x );
        constants[1] = 3 * ( points[2].x - points[1].x ) - constants[2];
        constants[0] = points[3].x - points[0].x - constants[2] - constants[1];

        constants[5] = 3 * ( points[1].y - points[0].y );
        constants[4] = 3 * ( points[2].y - points[1].y ) - constants[5];
        constants[3] = points[3].y - points[0].y - constants[5] - constants[4];
    }

    private float GetU(int u, float t) {
        int offset = u*3;
        float a = constants[0+offset];
        float b = constants[1+offset];
        float c = constants[2+offset];
        float u0 = points[0][u];
        return a*(t*t*t) + b*(t*t) + c*t + u0;
    }

    private float GetdU(int u, float t) {
        int offset = u*3;
        float a = constants[0+offset];
        float b = constants[1+offset];
        float c = constants[2+offset];
        return 3*a*(t*t) + 2*b*(t) + c;
    }

    public Vector2 GetPoint( float t ) {
        return new Vector2( GetU(0,t),GetU(1,t) );
    }

    public Vector2 GetTangent( float t ) {
        return (new Vector2( GetdU(0,t),GetdU(1,t) )).normalized;
    }

    public Vector2 GetNormal( float t ) {
        return (new Vector2( -GetdU(1,t),GetdU(0,t) )).normalized;
    }

    public void SetPoint(int index, Vector2 point) {
        if( index >= 0 && index < 4 && point != null ) {
            points[index] = point;
            CalcConstants();
        }
    }
}
