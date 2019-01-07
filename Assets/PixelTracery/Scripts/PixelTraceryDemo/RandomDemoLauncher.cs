using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomDemoLauncher : MonoBehaviour {

  public Material ScreenMaterial;

  public float DelaySec = 5f;

  private List<BaseDemo> demos = new List<BaseDemo> ();

  private BaseDemo lastDemo;

  private void Start () {
    var texture = new Texture2D (240, 136);
    ScreenMaterial.mainTexture = texture;

    addScriptAndDisable<TriangleDemo> ();
    addScriptAndDisable<LineDemo> ();
    addScriptAndDisable<RectDemo> ();
    addScriptAndDisable<CircleDemo> ();
    addScriptAndDisable<CircleDemo2> ();
    addScriptAndDisable<EarthDemo> ();
    addScriptAndDisable<F1BallidsDemo> ();

    StartCoroutine (randomLaunchCoroutine ());
  }

  private void addScriptAndDisable<T> () where T : BaseDemo {
    var demoScript = gameObject.AddComponent<T> ();
    demoScript.Init (ScreenMaterial.mainTexture as Texture2D);
    demoScript.enabled = false;
    demos.Add (demoScript);
  }

  private IEnumerator randomLaunchCoroutine () {
    if (lastDemo != null) lastDemo.enabled = false;

    var randomIx = Random.Range (0, demos.Count);
    lastDemo = demos[randomIx];
    lastDemo.enabled = true;

    yield return new WaitForSeconds (DelaySec);

    StartCoroutine (randomLaunchCoroutine ());
  }
}