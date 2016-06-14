using UnityEngine;

// TODO rotation
// TODO bezier
// TODO free form
// TODO curry/partial function?
public interface Interpolator {
    Vector2 Interpolate(Vector2 position, float progress, float elapsedTime);
}

public class ConstantInterpolator : Interpolator {
    public Vector2 Interpolate(Vector2 position, float progress, float elapsedTime) {
        return position;
    }
}

public class LinearInterpolator : Interpolator {
    Vector2 velocity;

    public LinearInterpolator(Vector2 velocity) {
        this.velocity = velocity;
    }

    public Vector2 Interpolate(Vector2 position, float progress, float elapsedTime) {
        return position + velocity*elapsedTime;
    }
}

public class AcceleratedInterpolator : Interpolator {
    Vector2 velocity;
    Vector2 acceleration;

    public AcceleratedInterpolator(Vector2 velocity, Vector2 acceleration) {
        this.velocity = velocity;
        this.acceleration = acceleration;
    }

    public Vector2 Interpolate(Vector2 position, float progress, float elapsedTime) {
        return position + velocity*elapsedTime + 0.5f * acceleration * elapsedTime * elapsedTime;
    }
}
