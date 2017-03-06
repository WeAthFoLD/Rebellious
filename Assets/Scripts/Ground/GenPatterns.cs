using UnityEngine;
using System.Collections;

namespace Generation {

    public class GenPrefab : GenPattern {

        public float height;
        public float randomHt;
        string resName;

        public GenPrefab(string _resName, float ht = -4.5f, float randHt = 0.0f) {
            height = ht;
            randomHt = randHt;
            resName = _resName;
        }

        Object prefab;

        public override float generateAt(GameObject scene, float x) {
            if(prefab == null) {
                prefab = Resources.Load(resName);
            }
            GameObject gen = GameObject.Instantiate(prefab, new Vector3(x, height + Random.Range(-randomHt, randomHt)), Quaternion.identity) as GameObject;
            gen.transform.parent = scene.transform;
            generated(gen);
            return STEP;
        }

    }

    public class GenDefaultTile : GenPrefab {

        public GenDefaultTile(float ht) : base("Prefabs/Scene/ground_normal", ht) { }
    }

    public class GenStart : GenPrefab {

        public GenStart(float ht) : base("Prefabs/Scene/ground_start", ht) { }

    }

    public class GenPlatform : GenPrefab {

        public GenPlatform(float ht = 0f, float randHt = 0.5f) : base("Prefabs/Scene/platform", ht, randHt) { }

    }

    public class GenPlatformDecorative : GenPrefab {

        public GenPlatformDecorative(float ht = 0f) : base("Prefabs/Scene/platform_decorative", ht) { }

    }

    public class GenSmallRobot : GenPrefab {

        public GenSmallRobot(float ht = 0f) : base("Prefabs/Mobs/small_robot", ht, 0.7f) { }

    }

    public class GenMedRobot : GenPrefab {

        public GenMedRobot(float ht = 0f) : base("Prefabs/Mobs/medium_robot", ht, 0.7f) { }

    }

    public class GenTrap : GenPrefab {

        public GenTrap(float ht) : base("Prefabs/Scene/ground_trap", ht) { }

    }

    public class GenNothing : GenPattern {

        int blocks;

        public GenNothing(int _blocks = 1) {
            blocks = _blocks;
        }

        public override float generateAt(GameObject scene, float x) {
            return STEP * blocks;
        }

        override public bool acceptsAsNextPattern(string name) {
            return !name.Contains("nothing");
        }

    }

    public class GenRepeatRandomTimes : GenPattern {

        GenPattern pattern;

        int from, to;

        public GenRepeatRandomTimes(GenPattern pat, int a, int b) {
            pattern = pat;
            from = a;
            to = b;
        }

        public override float generateAt(GameObject scene, float x) {
            int n = Random.Range(from, to + 1);
            float sum = 0.0f;
            for(int i = 0; i < n; ++i) {
                sum += pattern.generateAt(scene, x + sum);
            }
            return sum;
        }

        override public bool acceptsAsNextPattern(string name) {
            return pattern.acceptsAsNextPattern(name);
        }

    }

    public class GenWithPlatform : GenPattern {

        static GenPrefab genPlatform = new GenPlatform();
        GenPattern pattern;
        double platProb;
        public float dy = 0.0f;

        public GenWithPlatform(GenPattern _pattern, double genProb) {
            pattern = _pattern;
            platProb = genProb;
        }

        public GenWithPlatform setDY(float d) {
            dy = d;
            return this;
        }

        public override float generateAt(GameObject scene, float x) {
            float sum = pattern.generateAt(scene, x);
            if(Random.Range(0f, 1f) < platProb) {
                int i = (int)(sum / STEP);
                genPlatform.height = dy + Random.Range(-1, 0.1f);
                genPlatform.generateAt(scene, x + Random.Range(0, i) * STEP);
            }
            return sum;
        }

        override public bool acceptsAsNextPattern(string name) {
            return pattern.acceptsAsNextPattern(name);
        }
        
    }

}