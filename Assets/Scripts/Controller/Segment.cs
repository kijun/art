using UnityEngine;

/*
 * Segment is a section of a level.
 * When players die, they respawn at the beginning of the segment.
 */
public class Segment : MonoBehaviour {


    /***** PUBLIC VARIABLES *****/
    public Vector2 segmentBaseVelocity = new Vector2(0, 0.5f);
    public Vector2 playerMaxSpeed = new Vector2(1, 0.5f);
    public Transform defaultStartPosition;


    /***** PRIVATE VARIABLES *****/

    Player player;
    CameraController camera;


    /***** PUBLIC METHODS *****/

    /*
     * Play segment from the top.
     */
    public void PlaySegment(Player player, CameraController camera, bool resetPlayerPosition = false) {
        this.player = player;
        this.camera = camera;
        if (resetPlayerPosition) {
            // TODO use bounds to set position;
            this.player.Position = DefaultPlayerPosition;
            this.camera.Position = DefaultCameraPosition;
        }

        player.BaseVelocity = segmentBaseVelocity;
        player.MaxRelativeSpeed = playerMaxSpeed;
        camera.Velocity = segmentBaseVelocity;
    }

    /*
     * Restart player and camera position.
     */
    public void RestartSegment() {
        PlaySegment(player, camera, true);
    }


    /***** MONOBEHAVIOUR *****/

    void Update() {
        /*
        if (!segmentBounds.contains(player.Position)) {
            Game.Instance.SegmentCompleted(this);
        }
        */
    }

    /***** PRIVATE METHODS *****/

    /*
     * Returns obstacles to their initial state.
     */
    void ResetObstacles() {
        //TODO
    }


    /***** PRIVATE PROPERTIES *****/

    Vector2 DefaultPlayerPosition {
        get {
            return defaultStartPosition.position;
        }
    }

    Vector2 DefaultCameraPosition {
        get {
            return DefaultPlayerPosition;
        }
    }
}
