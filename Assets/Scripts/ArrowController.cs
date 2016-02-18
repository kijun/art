using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PolygonCollider2D), typeof (Rigidbody2D))]

public class ArrowController : MonoBehaviour {
    // every few seconds, turn direction?
    // or turn direction but have the same velocity
    // until few seconds later when it must stop
    //
    public float baseSpeed = 1f;
    public float chaseTime = 3f;
    private Rigidbody2D rgbd;
    private Transform player;

    void Awake() {
        rgbd = GetComponent<Rigidbody2D>();
    }

    public void ChasePlayer(Transform aPlayer) {
        player = aPlayer;
        StartCoroutine(DoChase());
    }

    IEnumerator DoChase() {
        float elapsedTime = 0.0f;
        while (elapsedTime < chaseTime) {
            elapsedTime += Time.deltaTime;
            var direction = player.position - transform.position;
            rgbd.velocity = direction.normalized * baseSpeed;
            //rgbd.rotation = 180f - Mathf.Atan2(-2, 2) / (2 * Mathf.PI) * 360f;
            rgbd.rotation = -90f + Mathf.Atan2(direction.y, direction.x) / (2 * Mathf.PI) * 360f;
            yield return null;
        }
    }
}

