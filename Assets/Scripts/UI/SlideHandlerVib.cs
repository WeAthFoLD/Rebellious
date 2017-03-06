using UnityEngine;
using System.Collections;
using System;

class SlideHandlerVib : SliderHandlerBase {
    public override bool GetInitValue() {
        return Utils.vibration;
    }

    public override void OnValueChanged(bool value) {
        Utils.vibration = value;
    }
}