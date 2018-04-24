using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kernel {
public class MainApp : MonoBehaviour {

    JM1ProductionTable pt = new JM1ProductionTable();
    JM1NodePropertyGenerator pg = new JM1NodePropertyGenerator();
    Canvas canvas = new Canvas();

    void Start() {
        StartCoroutine(Run());
    }

    IEnumerator Run() {
        for (int i = 0; i < 100000; i++) {
            var root = pt.Produce(new JM1RootNode());
            pg.Reset();
            pg.GenerateProperty(root);
            root.Render(canvas);
            //yield return new WaitForSeconds(1);
            if (Random.value < 0.97f) {
                yield return null;
            } else {
                yield return new WaitForSeconds(Random.Range(3f, 6f));
            }
            DestroyAnims1();
            //yield return AnimateCamera();
            //StartCoroutine(DestroyAnims());
            //yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator AnimateCamera() {
        var rgbd = GetComponent<Rigidbody>();
        rgbd.angularVelocity = new Vector3(0, RandomHelper.Pick(-0.3f, 0.3f), 0);
        //rgbd.velocity = new Vector3(Random.value, 0, 0);
        //transform.eulerAngles = new Vector3(0, 0, 90);
        //transform.position = transform.position.SwapZ(-16);
        yield return new WaitForSeconds(5.3f);
        //transform.eulerAngles = new Vector3(0, 0, 0);
        //transform.position = transform.position.SwapZ(-10);
    }

    IEnumerator DestroyAnims() {
        foreach (var anim in FindObjectsOfType<Animatable2>()) {
            anim.AddAnimationCurve(AnimationKeyPath.Opacity, AnimationCurve.Linear(0, 1, 1, 0));
            //anim.opacity = 0;
            //anim.StopCoroutine(
            //Destroy(anim.gameObject);
        }
        yield return new WaitForSeconds(2.1f);
        foreach (var anim in FindObjectsOfType<Animatable2>()) {
            Destroy(anim.gameObject);
        }
        var rgbd = GetComponent<Rigidbody>();
        rgbd.angularVelocity = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
        yield return new WaitForSeconds(1);
    }

    void DestroyAnims1() {
        foreach (var anim in FindObjectsOfType<Animatable2>()) {
            Destroy(anim.gameObject);
        }
    }
}
}


