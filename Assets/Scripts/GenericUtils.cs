using UnityEngine;
using System.Collections;

class Utils {

    // Stored Data interfaces

    public static bool vibration {
        get { return PlayerPrefs.GetInt("Vibration", 1) != 0; }
        set { PlayerPrefs.SetInt("Vibration", value ? 1 : 0); }
    }

    public static bool sound {
        get { return PlayerPrefs.GetInt("Sound", 1) != 0; }
        set { PlayerPrefs.SetInt("Sound", value ? 1 : 0); }
    }

    public static int highScore {
        get { return PlayerPrefs.GetInt("HighScore", 0); }
        set { PlayerPrefs.SetInt("HighScore", value); }
    }

    public static bool played {
        // FOR DEBUG
        // get { return false; }
        get { return PlayerPrefs.GetInt("Played", 0) != 0; }
        set { PlayerPrefs.SetInt("Played", value ? 1 : 0); }
    }

    public static void Vibrate() {
        if(vibration) {
#if !UNITY_STANDALONE
            Handheld.Vibrate();
#endif
        }
    }

    public static GameObject findNearestInFront(GameObject obj, string tag) {
        GameObject[] arr = GameObject.FindGameObjectsWithTag(tag);
        float dist = float.MaxValue;
        GameObject ret = null;
        GameObject player = GameObject.FindWithTag("Player");
        foreach(GameObject g in arr) {
            if(player.transform.localPosition.x > g.transform.localPosition.x) continue;

            float d = (g.transform.localPosition - obj.transform.localPosition).sqrMagnitude;
            if(d < dist) {
                ret = g;
                dist = d;
            }
        }
        return ret;
    }

    public static bool inScene(GameObject obj) {
        GameObject camera = GameObject.FindWithTag("MainCamera");
        return Mathf.Abs(camera.transform.localPosition.x - obj.transform.localPosition.x) < 4;
    }

    public static Vector3 lerp(Vector2 a, Vector2 b, float f) {
        float mf = 1 - f;
        return new Vector3(a.x * mf + b.x * f, a.y * mf + b.y * f);
    }

    public static GameObject player() {
        return GameObject.FindWithTag("Player");
    }

}