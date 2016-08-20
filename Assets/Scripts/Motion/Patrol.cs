using UnityEngine;
using System.Collections;


/*
 * TODO multipath
 */
[ExecuteInEditMode]
[SelectionBase]
[RequireComponent (typeof (Rigidbody2D))]
public class Patrol : MonoBehaviour {


    /***** CONST *****/
    const string PATROL_FROM_GAMEOBJECT_NAME = "Patrol From";
    const string PATROL_TO_GAMEOBJECT_NAME = "Patrol To";


    /***** PUBLIC: VARIABLES *****/
    public float speed;
    public bool repeat = true;


    /***** PRIVATE: VARIABLES *****/
    // TODO: serialized?
    Transform patrolFrom;
    Transform patrolTo;


    /***** INITIALIZER *****/
    void Awake () {
        if (patrolFrom == null) {
            patrolFrom = GetOrCreatePositionMarker(PATROL_FROM_GAMEOBJECT_NAME);
        }
        if (patrolTo == null) {
            patrolTo = GetOrCreatePositionMarker(PATROL_TO_GAMEOBJECT_NAME);
        }
        patrolFrom.position = Vector2.zero;
        patrolTo.position = Vector2.right;
    }

    void Start() {
        StartCoroutine(RunPatrol());
    }

    /***** MONOBEHAVIOUR *****/
	void Update () {
	}

    /***** PRIVATE: METHODS *****/
    IEnumerator RunPatrol() {
        // TODO can't be
        float timeToReachDestination = Vector2.Distance(
                patrolFrom.position,
                patrolTo.position) / speed;

        var velocityVector = (patrolTo.position - patrolFrom.position).normalized * speed;

        while (true) {
            GetComponent<Rigidbody2D>().velocity = velocityVector;
            yield return new WaitForSeconds(timeToReachDestination);

            if (!repeat) break;

            GetComponent<Rigidbody2D>().velocity = -1 * velocityVector;
            yield return new WaitForSeconds(timeToReachDestination);
        }
    }


    /*
     * Create child game object if it doesn't exist
     */
    Transform GetOrCreatePositionMarker(string gameObjectName) {
        var child = transform.FindChild(gameObjectName);
        if (child == null)  {
            var go = new GameObject(gameObjectName);
            go.transform.parent = transform;
            child = go.transform;
        }
        return child;
    }
}
