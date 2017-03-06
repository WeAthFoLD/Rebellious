using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Event : MonoBehaviour {

    public void OpenScene() {
        Application.LoadLevelAsync(1);
    }
}
