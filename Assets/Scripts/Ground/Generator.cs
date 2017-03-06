using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generation {

    public delegate void GenerationCallback(GameObject obj);

    class Generator {

        GenPattern lastGenPattern;

        Dictionary<string, SinglePattern> patterns = new Dictionary<string, SinglePattern>();

        float totalPossibility = 0.0f;

        public float targetX = -6;

        public void addPattern(string name, GenPattern pattern, float prob = 1.0f) {
            patterns.Add(name, new SinglePattern(pattern, prob));
            totalPossibility += prob;
        }

        public void generate(GameObject scene, string patternName) {
            generate(scene, patterns[patternName].pattern);
        }

        public void generate(GameObject scene, GenPattern pattern) {
            targetX += pattern.generateAt(scene, targetX);
        }

        public void generateUntil(GameObject scene, double x) {
            while(targetX < x) {
                generate(scene);
            }
        }

        public void generate(GameObject scene) {
            float target = Random.Range(0, totalPossibility);
            float p = 0.0f;

            SinglePattern targPattern = null;
            string patternName = "";
            do {
                foreach(KeyValuePair<string, SinglePattern> pair in patterns) {
                    p += pair.Value.prob;
                    if(p >= target) {
                        targPattern = pair.Value;
                        patternName = pair.Key;
                        break;
                    }
                }
            } while(lastGenPattern != null && !lastGenPattern.acceptsAsNextPattern(patternName));
            lastGenPattern = targPattern.pattern;
            generate(scene, targPattern.pattern);
            //Debug.Log("Generate " + patternName);
        }

        private class SinglePattern {

            public GenPattern pattern;
            public float prob;

            public SinglePattern(GenPattern _pattern, float p) {
                pattern = _pattern;
                prob = p;
            }

        }

    }

}
