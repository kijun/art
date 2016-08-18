using UnityEngine;

public class Player : MonoBehaviour {


    /***** PRIVATE VARIABLES *****/
    Vector2 baseVelocity;
    Vector2 maxRelativeSpeed;
    bool moving;


    /***** PUBLIC METHODS *****/
    public void StartMoving() {
        moving = true;
    }

    public void StopMoving() {
        moving = false;
    }


    /***** PUBLIC PROPERTIES *****/
    public Vector2 Position {
        get {
            return transform.position;
        }
        set {
            transform.position = value;
        }
    }


    /***** MONOBEHAVIOUR *****/
    void Update() {
        float xdir = Input.GetAxisRaw("Horizontal");
        float ydir = Input.GetAxisRaw("Vertical");
        Vector2 newPos = transform.position;

        float dx = (xdir * maxRelativeSpeed.x + baseVelocity.x) * Time.deltaTime;
        float dy = (ydir * maxRelativeSpeed.y + baseVelocity.y) * Time.deltaTime;

        newPos.x += dx;
        newPos.y += dy;

        /*
        if (Mathf.Abs(xdir) + Mathf.Abs(ydir) > float.Epsilon) {
            stroke1.angularVelocity = stroke1MaxAngularVelocity;
            stroke2.angularVelocity = stroke2MaxAngularVelocity;
        } else {
            stroke1.angularVelocity = stroke1BaseAngularVelocity;
            stroke2.angularVelocity = stroke2BaseAngularVelocity;
        }
        */

        transform.position = newPos;
    }


    /*
     * Velocity of the current segment.
     */
    public Vector2 BaseVelocity {
        get; set;
    }

    public Vector2 MaxRelativeSpeed{
        get; set;
    }
}



