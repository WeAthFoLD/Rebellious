using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class SliderHandlerBase : MonoBehaviour {

    Slider slider;

    public Sprite iconOn, iconOff;

    public bool on = true;

    private bool changed;
    private float lastChangedTime;

    public abstract void OnValueChanged(bool value);
    public abstract bool GetInitValue();

    public void ValueChanged() {
        var value = slider.value;
        bool bval = value <= 0.5f;
        if(on != bval) {
            on = bval;
            OnValueChanged(on);
            FindHandle().GetComponent<Image>().sprite = on ? iconOn : iconOff;
        }
        changed = true;
        lastChangedTime = Time.time;
    }
    
	public virtual void Start () {
	    slider = GetComponent<Slider>();
        on = GetInitValue();
        slider.value = on ? 0 : 1;
        FindHandle().GetComponent<Image>().sprite = on ? iconOn : iconOff;
    }
	
	void Update () {
	    if(changed && Time.time - lastChangedTime > 0.1f) {
            slider.value = on ? 0 : 1;
            changed = false;
        }
	}

    private GameObject FindHandle() {
        return gameObject.transform.GetChild(2).GetChild(0).gameObject;
    }
}
