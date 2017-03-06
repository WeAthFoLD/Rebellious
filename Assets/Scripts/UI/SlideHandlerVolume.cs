using UnityEngine;
using System.Collections;
using System;

class SlideHandlerVolume : SliderHandlerBase {
    public override bool GetInitValue() {
        return Utils.sound;
    }

    public override void OnValueChanged(bool value) {
        Utils.sound = value;
    }
}
