using UnityEngine;
using System.Collections;

public class CooldownBarUI : MonoBehaviour {

    new public Camera camera;

    float width;
    Sprite sprite;
    Controller player;
    ScoreBoard scoreBoard;

    NumbersDrawer numbers;

    Vector2 
        v0 = new Vector2(),
        v1 = new Vector2(),
        v2 = new Vector2(),
        v3 = new Vector2();

    ushort[] vertices = new ushort[] { 0, 1, 2, 0, 2, 3 };

	void Start () {
        sprite = GetComponent<SpriteRenderer>().sprite;
        width = sprite.rect.width;
        v2.y = v1.y = sprite.rect.height;

        player = GameObject.FindWithTag("Player").GetComponent<Controller>();
        scoreBoard = GameObject.Find("Scene").GetComponent<ScoreBoard>();

        numbers = new NumbersDrawer(NumbersDrawer.Align.RIGHT);
    }

	void Update () {
        float nw = width * (1 - player.shootOverload);
        v2.x = v3.x = nw;

        sprite.OverrideGeometry(new Vector2[] { v0, v1, v2, v3 }, vertices);

        numbers.setNumber(scoreBoard.score);
        var pos = camera.transform.localPosition;
        numbers.draw(9 + pos.x, 4.3f + pos.y, 0.0066f);
	}
}
