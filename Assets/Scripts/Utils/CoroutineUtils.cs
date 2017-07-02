using UnityEngine;
using System;
using System.Collections;

public static class C {

    /**
     * Usage: StartCoroutine(CoroutineUtils.Chain(...))
     * For example:
     *     StartCoroutine(CoroutineUtils.Chain(
     *         CoroutineUtils.Do(() => Debug.Log("A")),
     *         CoroutineUtils.WaitForSeconds(2),
     *         CoroutineUtils.Do(() => Debug.Log("B"))));
     */
    public static IEnumerator Chain(params IEnumerator[] actions) {
        foreach (IEnumerator action in actions) {
			yield return action;
        }
    }

    /**
     * Usage: StartCoroutine(CoroutineUtils.DelaySeconds(action, delay))
     * For example:
     *     StartCoroutine(CoroutineUtils.DelaySeconds(
     *         () => DebugUtils.Log("2 seconds past"),
     *         2);
     */
    public static IEnumerator WithDelay(Action action, float delay) {
        yield return new WaitForSeconds(delay);
        action();
    }

/*
    public static IEnumerator WaitForSeconds(float time) {
        yield return new WaitForSeconds(time);
    }
*/

    public static IEnumerator Do(Action action) {
        action();
        yield return 0;
    }
}
