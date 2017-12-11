using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDisplay : MonoBehaviour {

    public SphereWithMaterialBlock prefab;

	// Use this for initialization
	void Start () {
        Cursor.visible = false;
	}

	// Update is called once per frame
	void Update () {
      if (Input.GetKey(KeyCode.Alpha8)) {
          Camera.main.orthographicSize = 10;
          CreateSpheres();
      } else if (Input.GetKey(KeyCode.Alpha9)) {
          Camera.main.orthographicSize = 15;
          CreateSpheres();
      } else if (Input.GetKey(KeyCode.Alpha0)) {
          Camera.main.orthographicSize = 20;
          CreateSpheres();
      }
	}

    void CreateSpheres() {
        foreach (var s in GameObject.FindObjectsOfType<SphereWithMaterialBlock>()) {
            Destroy(s.gameObject);
        }
        int width = (int)CameraHelper.Width;
        int height = (int)CameraHelper.Height;

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                var go = GameObject.Instantiate(prefab, new Vector3(x - width/2f+0.5f, y - height/2f+0.5f, 0), Quaternion.Euler(-90, 0, 0));
                //go.transform.localScale = Vector3.one * (0.1f + Random.Range(-0.1f, 0.1f));
                //var go = GameObject.Instantiate(prefab, new Vector3(x - width/2f, y - height/2f, 0), Quaternion.Euler(-90, 0, 0));
                go.transform.localScale = Vector3.one * (0.85f + Random.Range(-0.1f, 0.1f));
            }
        }
        Debug.Log("Created " + width*height);
    }
}
