using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

    public float speed = 0.1f;
    public float lerpDuration = 2.0f;

    Renderer bgRenderer;

    void Awake() {
        bgRenderer = GetComponent<Renderer>();
    }

	// Update is called once per frame
	void FixedUpdate () {
        var offset = bgRenderer.material.mainTextureOffset;
        offset.y += speed * Time.deltaTime;
        bgRenderer.material.mainTextureOffset = offset;
        float lerp = Mathf.PingPong(Time.time, lerpDuration) / lerpDuration;
        //Debug.Log(lerp);
        //bgRenderer.material.Lerp(bg1, bg2, lerp);
        bgRenderer.material.SetFloat( "_Blend", lerp );
	}
}
