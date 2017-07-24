using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Animatable2 : MonoBehaviour {
    Dictionary<string, AnimationCurve> animationCurves = new Dictionary<string, AnimationCurve>();

    Vector2 originalScale;
    float originalRotation;
    float startTime;
    float destroyAt = float.MaxValue;

    /*** Lifecycle ***/

    void Start() {
        originalScale = localScale;
        originalRotation = rotation;
        startTime = Time.time;
        UpdateAnimation();
    }

    public virtual void FixedUpdate() {
        if (Mathf.Abs(scaleVelocity.sqrMagnitude) > float.Epsilon) {
            var newLocalScale = transform.localScale + (Vector3)scaleVelocity * Time.deltaTime;
            if (nonNegativeScale) {
                if (localScale.sqrMagnitude > float.Epsilon) {
                    transform.localScale = new Vector2(Mathf.Max(newLocalScale.x, 0), Mathf.Max(newLocalScale.y, 0));
                }
            } else {
                transform.localScale = newLocalScale;
            }
        }

        UpdateAnimation();

        // apply animation curve
        if (Time.time > destroyAt) {
            Destroy(gameObject);
        }
    }

    void UpdateAnimation() {
        if (animationCurves.Count > 0) {
            foreach (var kv in animationCurves) {
                var val = kv.Value.Evaluate(Time.time);
                switch (kv.Key) {
                    case AnimationKeyPath.Opacity:
                        opacity = val;
                        break;
                    case AnimationKeyPath.Rotation:
                        rotation = val;
                        break;
                    case AnimationKeyPath.VelocityX:
                        velocity = velocity.SwapX(val);
                        break;
                    case AnimationKeyPath.VelocityY:
                        velocity = velocity.SwapY(val);
                        break;
                    case AnimationKeyPath.ScaleX:
                        break;
                    case AnimationKeyPath.ScaleY:
                        break;
                    case AnimationKeyPath.ColorR:
                        break;
                    case AnimationKeyPath.ColorG:
                        break;
                    case AnimationKeyPath.ColorB:
                        break;
                    case AnimationKeyPath.RelPosX:
                        position = position.SwapX(val);
                        break;
                    case AnimationKeyPath.RelPosY:
                        position = position.SwapY(val);
                        break;
                    case AnimationKeyPath.RelScaleX:
                        localScale = localScale.SwapX(val);
                        break;
                    case AnimationKeyPath.RelScaleY:
                        localScale = localScale.SwapY(val);
                        break;
                }
            }
        }

        // remove completed animations
        var finishedAnimations = new List<string>();
        foreach (var pair in animationCurves) {
            if (pair.Value.IsDone(Time.time))
            {
                finishedAnimations.Add(pair.Key);
            }
        }

        foreach (var anim in finishedAnimations) {
            animationCurves.Remove(anim);
        }

        // not sure if this should be relative or absolute time
    }

    public void DestroyIn(float time, bool absolute = false) {
        if (!absolute) {
            destroyAt = time + Time.time;
        } else {
            destroyAt = time;
        }
    }

    /*** PUBLIC|METHODS ***/
    public void StopMovement() {
        velocity = Vector2.zero;
        angularVelocity = 0;
        scaleVelocity = Vector2.zero;
    }

    public bool stationary {
        get {
            return velocity.sqrMagnitude < float.Epsilon &&
                   Mathf.Abs(angularVelocity) < float.Epsilon &&
                   scaleVelocity.sqrMagnitude < float.Epsilon;
        }
    }
    /*** PUBLIC|ANIMATABLE PROPERTIES ***/

    public Vector2 position {
        get { return transform.position; }
        set { transform.position = ((Vector3)value).SwapZ(transform.position.z); }
    }

    public float rotation {
        /*
        get { return rigidbody2D.rotation; }
        set { rigidbody2D.rotation = value; }
        */
        get { return transform.rotation.eulerAngles.z; }
        set {
            var currEuler = transform.rotation.eulerAngles.SwapZ(value);
            transform.rotation = Quaternion.Euler(currEuler);
        }
    }

    public Rigidbody2D rigidbody2D {
        get { return GetComponent<Rigidbody2D>(); }
    }

    public Vector2 velocity {
        get { return rigidbody2D.velocity; }
        set { rigidbody2D.velocity = value; }
    }

    public Vector2 localScale {
        get { return transform.localScale; }
        set { transform.localScale = new Vector3(value.x, value.y, transform.localScale.z); }
    }

    public float angularVelocity {
        get { return rigidbody2D.angularVelocity; }
        set { rigidbody2D.angularVelocity = value; }
    }

    public Vector2 scaleVelocity;
    public bool nonNegativeScale = false;

    public Color color {
        set {
            foreach (var rd in renderers) {
                rd.material.color = value;
            }
        }

        get {
            return renderers[0].material.color;
        }
    }

    public float opacity {
        get {
            return renderers[0].material.GetOpacity();
        }
        set {
            foreach (var rd in renderers) {
                rd.material.SetOpacity(value);
            }
        }
    }

    public Vector2 pivot {
        get {
            return ((Vector2)child.localPosition).MultiplyEach(localScale) * -1;
        }
        set {
            child.localPosition = -1 * value.DivideEach(localScale);
        }
    }

    public float level {
        get {
            return transform.position.z * -1;
        }

        set {
            transform.position = transform.position.SwapZ(value * -1);
        }
    }

    /*** Animation ***/
    public void AddAnimationCurve(string keyPath, AnimationCurve curve, bool relativeTime = true) {
        var keyframes = curve.keys;
        if (relativeTime) {
            for (int i = 0; i < keyframes.Length; i++) {
                var kf = keyframes[i];
                kf.time += Time.time;
                keyframes[i] = kf;
            }
        }
        animationCurves.Remove(keyPath);
        animationCurves.Add(keyPath, new AnimationCurve(keyframes));
    }

    /*** PRIVATE ***/
    Transform child {
        get { return transform.GetChild(0); }
    }

    Renderer[] renderers {
        get {
            return GetComponentsInChildren<Renderer>();
        }
    }
}

