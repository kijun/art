using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
public class DepartureZone : MonoBehaviour {

    public ShadowShipController shipPrefab;
    public float secondsBetweenShips;
    public int startingShipCount = 30;
    public Range speedRange = new Range(0.3f, 0.7f);
    //public Vector2[] initialSpawnLocation;
    //public BoxCollider2D departureZone;

    private BoxCollider2D initialSpawnZone;
    private bool spawnShip = true;
    private Bounds departureZone;

	// Use this for initialization
	void Start () {
        initialSpawnZone = GetComponent<BoxCollider2D>();
        departureZone = initialSpawnZone.bounds;
        departureZone.center = departureZone.center.SwapY(departureZone.min.y);
        departureZone.size = departureZone.size.SwapY(0.5f);
        StartCoroutine(SpawnShadowShip());
	}

    IEnumerator SpawnShadowShip() {
        // first spawn some
        for (int i = 0; i<startingShipCount; i++) {
            InstantiateShip(initialSpawnZone.bounds);
        }

        while (spawnShip)  {
            yield return new WaitForSeconds(secondsBetweenShips);
            InstantiateShip(departureZone);
        }
    }

    Vector2 RandomShipPosition(Bounds bounds) {
        return bounds.RandomPoint();
    }

    void InstantiateShip(Bounds bounds) {
        var ship = (ShadowShipController)Instantiate(shipPrefab,
                                                     RandomShipPosition(bounds),
                                                     Quaternion.identity);

        ship.GetComponent<Rigidbody2D>().velocity = new Vector2(0, speedRange.RandomValue());
    }


	// Update is called once per frame
	void Update () {

	}

    void OnTriggerExit2D(Collider2D other) {
        // TODO using tag is probably dangerous
        if (other.gameObject.tag.Equals("Player")) {
            //StopAllCoroutines();
            spawnShip = false;
        }
    }
}
