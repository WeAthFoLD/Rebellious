using UnityEngine;
using System.Collections;

public class MobParticleFactory : MonoBehaviour {

    public Rect spawnArea = new Rect(0, 0, 1, 1);
    public float density = 0.2f;

    Object prefab;

	void Start () {
        prefab = Resources.Load("Prefabs/Mobs/bot_particle");
	}
	
	void Update () {
        //Fix so that particle appears well-distributed over time
        if(Random.value < density * Time.timeScale) { 
            //Do one generation
            float x = Random.Range(spawnArea.x, spawnArea.x + spawnArea.width);
            float y = Random.Range(spawnArea.y, spawnArea.y + spawnArea.height);

            Instantiate(prefab, new Vector3(x, y) + gameObject.transform.position, Quaternion.identity);
        }
	}
}
