using UnityEngine;
using System.Collections.Generic;

namespace Generation {
    //Base class denoting a generation pattern. Every generation pattern should COMPLETELY tAKE
    //control over a amount of X space.
    public abstract class GenPattern {

        public const float STEP = 1.73f;

        public GenPattern() {}

        //Generate the pattern. Return how much x space this pattern took hold of.
        //Returns: how much space this pattern takes.
        public abstract float generateAt(GameObject scene, float x);

        public void generated(GameObject obj) { }

        virtual public bool acceptsAsNextPattern(string patternName) {
            return true;
        }

    }

    public class GenPatternList : GenPattern {

        List<GenPattern> patterns = new List<GenPattern>();

        public GenPatternList(params GenPattern[] sub) {
            foreach(GenPattern p in sub) {
                patterns.Add(p);
            }
        }

        public GenPatternList(GenPattern sub, int count) {
            for(int i = 0; i < count; ++i) {
                patterns.Add(sub);
            }
        }

        public override float generateAt(GameObject scene, float x) {
            float len = 0.0f;
            foreach(GenPattern p in patterns) {
                len += p.generateAt(scene, x + len);
            }
            return len;
        }

    }
}
