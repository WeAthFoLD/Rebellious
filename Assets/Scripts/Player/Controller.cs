using UnityEngine;
using System.Collections;

//The main player controller. handles player jumping and shooting events, also try to move it at a constant force.
//TODO: Shoot delegate to another class?
public class Controller : MonoBehaviour {

    public Animator canvasAnimator;
    public GameObject tutsObj;

    public bool debug;

    Rigidbody2D rb2D;

    Object prefabBullet;

    AudioClip jumpSound;

    public AudioClip shootSound, deathSound;

    private bool jumpPressed;

    private float jumpPressTime = -1000;

    private bool onGround = false;
    private bool onAirJumped = false;

    public float maxSpeed { get { return debug ? 0.0f : (dead ? 0.0f : 8.0f); } }
    public float cameraSpeed { get { return debug ? 0.0f : 8.0f; } }

    //public float maxSpeed { get { return 0.0f; } }
    private bool dead = false;

    public float travelDistance { get { return transform.localPosition.x - startX; } }

    const float OVERLOAD_INCR = 0.2f;
    const float
        OVERLOAD_RECOVER_SPEED = 0.15F, //PER SEC
        OVERLOAD_COOLDOWN = 5;
    public float shootOverload {
        get { return load; }
    }
    private float startCoolTime = -2333f;
    private float load = 0.0f;

    float startX;

    private bool checkedTuts = false, playingTutorial =false;
    
	void Start () {
        prefabBullet = Resources.Load<GameObject>("Prefabs/Player/bullet");

        rb2D = GetComponent<Rigidbody2D>();

        jumpSound = Resources.Load<AudioClip>("Sounds/Player/jump");

        startX = transform.localPosition.x;
	}
	
	void Update () {
        doVelocityUpdate();

        if(dead)
            return;

        doOnGroundUpdate();

        if(!playingTutorial) { 
            doShootUpdate();
            doJumpUpdate();
        } else {
            if(shootKeyDown() || jumpKeyDown()) {
                Time.timeScale = 1f;
                canvasAnimator.Play("Gaming");
                playingTutorial = false;
            }
        }

        doOverloadUpdate();

        if(!checkedTuts) {
            if(transform.localPosition.x > 4.5) {
                checkedTuts = true;
                if(!Utils.played) {
                    Utils.played = true;
                    playingTutorial = true;
                    canvasAnimator.Play("Tutorial");
                    tutsObj.SetActive(true);
                    Time.timeScale = 0.02f;
                }
            }
        }
	}

    float lastShootTime = -1;
    const float SHOOT_INTERVAL = 0.2f;

    void doShootUpdate() {
        if(shootKeyDown() && shootOverload < 1.0f) {
            float dt = Time.time - lastShootTime;
            if(dt > SHOOT_INTERVAL) {
                lastShootTime = Time.time;
                //spawn the bullet
                Vector3 delta = new Vector3(Random.Range(0.4f, 0.5f), Random.Range(0.3f, 0.4f));
                GameObject obj = Instantiate(prefabBullet, gameObject.transform.localPosition + delta, Quaternion.identity) as GameObject;
                obj.transform.parent = transform.parent;
                // Utils.Vibrate();

                GetComponent<AudioSource>().PlayOneShot(shootSound);
                overload();
            }
        }
    }

    void doOnGroundUpdate() {
        float WIDTH = 0.4f, DOWN = 1f;
        Vector2 left = gameObject.transform.position - Vector3.right * WIDTH, right = gameObject.transform.position + Vector3.right * WIDTH;
        RaycastHit2D
            res1 = Physics2D.Raycast(left, -Vector2.up, DOWN, LayerMask.GetMask("Ground")),
            res2 = Physics2D.Raycast(right, -Vector2.up, DOWN, LayerMask.GetMask("Ground"));
        onGround = res1 || res2;
        if(onGround)
            onAirJumped = false;
    }

    void doJumpUpdate() {
        //Check key and update state
        const float KEY_PROCESS_TOL = 0.2f, TOLERANCE = 0.2f, VELOCITY = 16.0f; // Time interval between every jump

        float dt = Time.fixedTime - jumpPressTime;

        if(jumpKeyDown()) {
            if(dt > KEY_PROCESS_TOL) {
                jumpPressed = true;
                jumpPressTime = Time.fixedTime;
            }
        }

        dt = Time.fixedTime - jumpPressTime;

        bool doJump = false;

        if(onGround && jumpPressed) {
            doJump = true;
        }
        if(!onGround && !onAirJumped && jumpPressed) {
            onAirJumped = true;
            doJump = true;
        }

        if(doJump) {
            jumpPressed = false;

            if(dt < TOLERANCE) {
                transform.localPosition += Vector3.up * 0.1f;
                rb2D.velocity = new Vector2(rb2D.velocity.x, VELOCITY);
            }

            GetComponent<AudioSource>().PlayOneShot(jumpSound);
        }
    }

    void doVelocityUpdate() {
        const float ACCEL = 35.0f;
        float velx = rb2D.velocity.x;
        if(Mathf.Abs(velx) < maxSpeed) {
            rb2D.AddForce(Vector2.right * (onGround ? 1f : 0.2f) * ACCEL);
        } else {
            rb2D.velocity = new Vector2(Mathf.Sign(velx) * maxSpeed, rb2D.velocity.y); //Speed fix
        }
    }

    private void overload() {
        load += OVERLOAD_INCR;
        if(load >= 1.0f) {
            load = 1.0f;
            startCoolTime = Time.time;
        }
    }

    void doOverloadUpdate() {
        if(load == 1.0f) {
            if(Time.time - startCoolTime < OVERLOAD_COOLDOWN)
                return;
        }

        load -= Time.deltaTime * OVERLOAD_RECOVER_SPEED;
        if(load < 0f) {
            load = 0f;
        }
    }

    bool jumpKeyDown() {
        float hw = Screen.width / 2f;

        for(int i = 0; i < Input.touchCount; ++i ) {
            Touch t = Input.GetTouch(i);
            if(t.position.x < hw) {
                return true;
            }
        }

        return Input.GetKey(KeyCode.F);
    }

    bool shootKeyDown() {

        float hw = Screen.width / 2f;

        for(int i = 0; i < Input.touchCount; ++i) {
            Touch t = Input.GetTouch(i);
            if(t.position.x > hw) {
                return true;
            }
        }

        return Input.GetKey(KeyCode.J);
    }

    public void Attack(float damage = 0.0f) {
        if(!dead && !playingTutorial) {
            dead = true;
            GameObject.Find("Scene").BroadcastMessage("PlayerDead");
        }
    }

    void PlayerDead() {
        StartCoroutine("PlayDeadAnim");

        Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
        rb2D.gravityScale = 0.0f;
        rb2D.drag = 2.0f;

        rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
        rb2D.AddForce(Vector2.up * 4.0f, ForceMode2D.Impulse);

        GetComponent<AudioSource>().PlayOneShot(deathSound);

        Utils.Vibrate();
    }

    IEnumerator PlayDeadAnim() {
        GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(0.5f);
        FrameAnimation fa = GetComponent<FrameAnimation>();
        fa.Flip();
        fa.enabled = true;
        fa.ResetAndPlay();
    }
    
}
