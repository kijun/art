using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

    public float speed = 0.1f;

    Renderer bgRenderer;

    void Awake() {
        bgRenderer = GetComponent<Renderer>();
    }

	// Update is called once per frame
	void FixedUpdate () {
        var offset = bgRenderer.material.mainTextureOffset;
        offset.y += speed * Time.deltaTime;
        bgRenderer.material.mainTextureOffset = offset;
	}
}
