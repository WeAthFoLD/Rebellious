using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Generation;

public class GroundGen : MonoBehaviour {

    const float GEN_ADVANCE = 13.0f;
    const int PASSES = 3;

    GameObject mainCamera;

    Generator[] genPasses = new Generator[PASSES];

    Generator beginPass = new Generator();

    GenPattern lastGenPattern;

    void Start() {
        for(int i = 0; i < PASSES; ++i) {
            genPasses[i] = new Generator();
        }

        //Hardcode: detach all the generation patterns.
        //TODO: Maybe elsewhere?
        beginPass.targetX = -9f;
        beginPass.addPattern("start", new GenStart(-5f), 0.0f);

        //Pass 0: Ground gen
        addPattern(0, "default", new GenDefaultTile(-5f), 0.2f);
        addPattern(0, "nothing1", new GenNothing(), 0.01f);
        addPattern(0, "nothing2", new GenNothing(2), 0.03f);
        addPattern(0, "nothing3", new GenNothing(3), 0.02f);
        addPattern(0, "tileH1", new GenRepeatRandomTimes(new GenDefaultTile(-4.3f), 2, 5), 0.1f);
        
        //Pass 1: ?
        addPattern(1, "default", new GenNothing());
        addPattern(1, "small_robot", new GenSmallRobot(), 0.03f);
        addPattern(1, "medium_robot", new GenMedRobot(), 0.01f);
        addPattern(1, "platform", new GenPlatform(0.5f, 0.5f), 0.08f);

        //Pass 2: Island gen
        addPattern(2, "default", new GenNothing());
        addPattern(2, "platform_high", new GenPlatformDecorative(3f), 0.05f);
        addPattern(2, "platform_higher", new GenPlatformDecorative(3.5f), 0.05f);

        mainCamera = GameObject.FindWithTag("MainCamera");

        StartCoroutine("generateAtStart");
        StartCoroutine("garbageCollection");
    }

    public void addPattern(int pass, string name, GenPattern pattern, float p = 1.0f) {
        genPasses[pass].addPattern(name, pattern, p);
    }

    IEnumerator generator() {
        for(; ; ) {
            generateUntil(mainCamera.transform.localPosition.x + GEN_ADVANCE);
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator generateAtStart() {
        for(int i = 0; i < 12; ++i) {
            beginPass.generate(gameObject, "start");
            yield return new WaitForSeconds(0.05f);
        }
        foreach(Generator g in genPasses) {
            g.targetX = beginPass.targetX;
        }
        StartCoroutine("generator");
    }

    private void generateUntil(float x) {
        foreach(Generator g in genPasses) {
            g.generateUntil(gameObject, x);
        }
    }

    private void generate(string name) {
        foreach(Generator g in genPasses) {
            g.generate(gameObject, name);
        }
    }

    IEnumerator garbageCollection() {
        for(;;) {
            for(int i = 0; i < transform.childCount; ++i) {
                GameObject child = transform.GetChild(i).gameObject;
                //Check if the child is out of range.
                if(child.transform.localPosition.x < mainCamera.transform.localPosition.x - 12) {
                    Destroy(child);
                }
            }
            yield return new WaitForSeconds(3f);
        }
    }

}