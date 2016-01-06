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
    Sector prevSector;
    HashSet<Sector>renderedSectors = new HashSet<Sector>();

	// Use this for initialization

    void Awake () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = -1; i<=1; i++) {
            for (int j = -1; j<=1; j++) {
                Debug.Log("going to call render sector (" + i + ", " + j + ")");
                RenderSector(i, j);
            }
        }
        prevSector = sys.GetSector(0, 0);
    }

    Sector CurrentSector() {
        var ssize = Consts.sectorSize;
        Vector2 pos = player.position;
        int x = Mathf.FloorToInt((pos.x - ssize/2.0f)/ssize);
        int y = Mathf.FloorToInt((pos.y - ssize/2.0f)/ssize);
        Debug.Log("For pos: " + pos + "Current Sector (" + x + ", " + y + ")");
        return sys.GetSector(x, y);
    }

    HashSet<Sector> AdjacentSectors() {
        var sectors = new HashSet<Sector>();
        var cs = CurrentSector();
        for (int i = -1; i<=1; i++) {
            for (int j = -1; j<=1; j++) {
                sectors.Add(sys.GetSector(cs.col+i, cs.row+j));
            }
        }
        return sectors;
    }

    void RenderSector(int i, int j) {
        Debug.Log("Rendering Sector (" + i + ", " + j + ")");
        Sector sector = sys.GetSector(i, j);
        foreach (Planet p in sector.planets) {
            var pgo = Instantiate(planetPrefab);
            pgo.transform.position = p.origin;
        }
        renderedSectors.Add(sector);
    }

    void RemoveSector(int i, int j) {
    }

	// Update is called once per frame
	void Update () {
	}

    void FixedUpdate () {
        // if current sector is different then unfold, recreate
        var currSector = CurrentSector();
        if (currSector != prevSector) {
            foreach (var s in AdjacentSectors()) {
                if (!renderedSectors.Contains(s)) {
                    RenderSector(s.col, s.row);
                    renderedSectors.Add(s);
                }
            }
            prevSector = currSector;
        }
        // TODO: remove sectors
    }
}
