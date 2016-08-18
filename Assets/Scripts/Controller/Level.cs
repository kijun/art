using UnityEngine;

public class Level : MonoBehaviour {
    /***** PUBLIC VARIABLES *****/
    public Segment[] segments;


    /***** PRIVATE VARIABLES *****/
    int currentSegmentIndex;


    /***** PUBLIC METHODS *****/
    public void PlayLevel(Player player, CameraController camera) {
        currentSegmentIndex = 0;
        segments[currentSegmentIndex].PlaySegment(player, camera, resetPlayerPosition: true);
    }

    public void ReplaySegment(Player player, CameraController camera) {
        // reset position
        segments[currentSegmentIndex].PlaySegment(player, camera,
                resetPlayerPosition:true);
    }
}


