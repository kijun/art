using UnityEngine;
using System.Collections;

public class ArrowObstacle : MonoBehaviour {

    public ArrowController arrowPrefab;
    public float secondsToSpawn = 3f;
    private PlayerController playerCtrl;

	// Use this for initialization
	void Awake() {
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        StartCoroutine(SpawnArrow());
	}

	// Update is called once per frame
	void Update () {

	}


    IEnumerator SpawnArrow() {
        while (true) {
            var arrow = (ArrowController)Instantiate(arrowPrefab,
                    transform.position, Quaternion.identity);
            arrow.ChasePlayer(playerCtrl.transform);
            yield return new WaitForSeconds(secondsToSpawn);
        }
        yield return null;
    }
}

