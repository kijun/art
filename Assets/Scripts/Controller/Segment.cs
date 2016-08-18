using UnityEngine;

/*
 * Segment is a section of a level.
 * When players die, they respawn at the beginning of the segment.
 */
public class Segment : MonoBehaviour {


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
            this.player.Position = DefaultPlayerPosition;
            this.camera.Position = DefaultCameraPosition;
        }
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
            return transform.position;
        }
    }

    Vector2 DefaultCameraPosition {
        get {
            return DefaultPlayerPosition;
        }
    }
}
