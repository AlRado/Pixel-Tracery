using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FileLoader : MonoBehaviour {

    /**
      fileName: name with extension, e.g. Screen.scr 
     */
    public void LoadFromStreamingAssets (string fileName, Action<byte[]> callback) {
        var path = getDataPath (fileName);
        Load (path, callback);
    }

    public void Load (string path, Action<byte[]> callback) {
        Debug.Log ("Load from path: " + path);
        StartCoroutine (loadBinaryAssetCoroutine (path, callback));
    }

    private IEnumerator loadBinaryAssetCoroutine (string path, Action<byte[]> callback) {
        using (WWW www = new WWW (path)) {
            yield return www;

            if (!string.IsNullOrEmpty (www.error)) Debug.LogError (www.error + ", path: " + path);

            if (callback != null) callback (www.bytes);
        }
    }

    private string getDataPath (string fileName) {
        var streamingAssetsPath = "";
#if UNITY_IPHONE
        streamingAssetsPath = Application.dataPath + "/Raw";
#endif

#if UNITY_ANDROID
        streamingAssetsPath = "jar:file://" + Application.dataPath + "!/assets";
#endif

#if UNITY_STANDALONE || UNITY_EDITOR
        streamingAssetsPath = Application.dataPath + "/StreamingAssets";
#endif

        return Path.Combine (streamingAssetsPath, fileName);
    }

    public bool IsWellFormedUriString (string uriString, UriKind uriKind = UriKind.Absolute) {
        Uri uriResult;
        return Uri.TryCreate (uriString, UriKind.Absolute, out uriResult) &&
            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
       
    }

}