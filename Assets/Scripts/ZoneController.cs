using UnityEngine;
using System.Collections;

public class ZoneController : MonoBehaviour {

    public Vector2 zoneBaseVelocity = Consts.defaultZoneBaseVelocity;
    public Vector2 maxRelativeSpeed = Consts.defaultZoneMaxRelativeSpeed;

    public static ZoneController ZoneForPosition(Vector2 pos) {
        RaycastHit hit;
        if (Physics.Raycast(pos, new Vector3(0, 0, 1), out hit, 500, Consts.patternBackgroundLayerMask)) {
            return hit.collider.GetComponentInParent<ZoneController>();
        }
        return null;
    }
}
