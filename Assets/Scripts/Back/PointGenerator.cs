using UnityEngine;
using System.Collections;

public class PointGenerator : MonoBehaviour {

    public int genFrom = 1, genTo = 1;
    public float intervalFrom = 4.0f, intervalTo = 6.0f;
    public float maxDelay = 0.4f;
    public float alpha = 1.0f;

    Object[] prefabs;
    float lastGenTime;
    float timeWait = 0;

    void Start() {
        prefabs = new Object[7];
        for (int i = 0; i < 7; ++i) {
            prefabs[i] = Resources.Load("Prefabs/Back/dot" + i, typeof(GameObject));
        }
        lastGenTime = Time.time;
        timeWait = Random.Range(intervalFrom, intervalTo);
    }
	
	void Update () {
        float dt = Time.time - lastGenTime;
        if (dt >= timeWait) {
            lastGenTime = Time.time;
            timeWait = Random.Range(intervalFrom, intervalTo);
            int gen = Random.Range(genFrom, genTo);
            int genStart = Random.Range(0, prefabs.Length - gen);
            for (int i = genStart; i < genStart + gen; ++i) {
                GameObject go = Instantiate(prefabs[i], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                go.transform.parent = transform;
                DotAnimation dotAnim = go.GetComponent<DotAnimation>();
                dotAnim.lifeTime *= 5f;
                dotAnim.alphaMultiplyer = alpha;
            }
        }
	}


}
