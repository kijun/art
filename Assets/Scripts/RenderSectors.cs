using UnityEngine;
using System.Collections.Generic;

public class Coord {
    public int x, y;

    public Coord() : this(0, 0) { }

    public Coord(int x, int y) {
        this.x = x; this.y = y;
    }
}

public class RenderSectors : MonoBehaviour {

    public GameObject planetPrefab;


    PlanetarySystem sys = new PlanetarySystem();
    Transform player;


	// Use this for initialization

    void Awake () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = -1; i<=1; i++) {
            for (int j = -1; j<=1; j++) {
                Debug.Log("going to call render sector (" + i + ", " + j + ")");
                RenderSector(i, j);
            }
        }
    }

    Coord CurrentSector() {
        var ssize = Consts.sectorSize;
        Vector2 pos = transform.position;
        int x = Mathf.FloorToInt(pos.x - ssize/2.0f);
        int y = Mathf.FloorToInt(pos.y - ssize/2.0f);
        return new Coord(x, y);
    }

    void RenderSector(int i, int j) {
        Debug.Log("Rendering Sector (" + i + ", " + j + ")");
        Sector sector = sys.GetSector(i, j);
        foreach (Planet p in sector.planets) {
            var pgo = Instantiate(planetPrefab);
            pgo.transform.position = p.origin;
        }
    }

	// Update is called once per frame
	void Update () {
        // if current sector is different then unfold, recreate
	}
}
