using UnityEngine;
using System.Collections;

public class FrameAnimation : MonoBehaviour {

    public string path;
    public int count;
    public float frameLen = 0.04f; //25fps by default
    public bool doesLoop = false;
    public bool flip = false;

    SpriteRenderer spriteRenderer;
    Sprite beginSprite;

    Sprite[] sprites;

    float lastSwitchTime;
    int sid = 0;

	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();

        beginSprite = spriteRenderer.sprite;

        Rect rect = beginSprite.rect;
        Vector2 pivot = beginSprite.pivot;


        sprites = new Sprite[count];
        for(int i = 0; i < count; ++i) {
            sprites[flip ? count - i - 1 : i] = Resources.Load("Textures/" + path + "/" + i, typeof(Sprite)) as Sprite;
        }

        
        updateSprite(0);
	}

    void Update() {
        float dt = Time.time - lastSwitchTime;
        if(dt >= frameLen) {
            int next = sid + 1;
            if(next == count) {
                if(doesLoop) {
                    next = 0;
                } else {
                    //Destroy(this);
                    this.enabled = false;
                    return;
                }
            }
            updateSprite(next);
        }
	}

    public void ResetAndPlay() {
        updateSprite(0);
    }

    public void Flip() {
        Sprite[] r = new Sprite[sprites.Length];
        for(int i = 0; i < r.Length; ++i)
            r[r.Length - i - 1] = sprites[i];
        sprites = r;
    }

    private void updateSprite(int id) {
        lastSwitchTime = Time.time;
        sid = id;
        spriteRenderer.sprite = sprites[id];
        //spriteRenderer.sprite = beginSprite;
        //print(spriteRenderer.sprite.rect);
        //print(spriteRenderer.sprite.pivot);
    }
}
