using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player Variables
    public float playerJumpForce;
    private float lastCharacterPosition;
    private bool playerFalling;
    private bool onGround;
    private bool isBendDown;
    private bool leftArm;
    private bool rightArm;
    private Rigidbody2D playerBody;
    private Animator playerAnimator;

    // Ground Variables
    private LayerMask groundMaterial;
    private Transform groundDetection;
    private float groundDetectionRadius;

    // Game Variables
    private GameController game;
    private CoinsController coinsManager;

    // Sounds
    public AudioSource jumpSound;
    public AudioSource drownSound;
    public AudioSource killedSound;
    public AudioSource runSound;

    //Speed
    public float startSpeed;
    private float playerSpeed;
    private float maxSpeed;
    public float PlayerSpeed
    {
        get { return playerSpeed; }
        set { playerSpeed = value; }
    }
    private float startTime;

    //Data
    Dictionary<string, int> data = new Dictionary<string, int>() { { "platform5", 0}, { "platform6", 0 }, { "platform7", 0 }, { "platform10", 0 },
                                                                   { "spikes", 0 }, { "Catcher", 0 }, { "evilBlock", 0 } , { "evilSaw", 0 } };

    /** 
     *  Initialization
     */
    void Start()
    {
        groundMaterial = LayerMask.GetMask("Ground");
        groundDetection = GameObject.Find("GroundCollision").GetComponent<Transform>();

        game = GameObject.Find("GameManager").GetComponent<GameController>();
        coinsManager = GameObject.Find("CoinManager").GetComponent<CoinsController>();

        maxSpeed = 7.5f;
        switch (PlayerPrefs.GetInt("Speed"))
        {
            case 1:
                startSpeed += (maxSpeed - startSpeed) / 2;
                break;
            case 2:
                startSpeed = maxSpeed;
                break;
            default:
                break;
        }

        playerBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        groundDetectionRadius = GetComponent<BoxCollider2D>().size.x / 2;
        lastCharacterPosition = transform.position.x;

        onGround = false;
        isBendDown = false;
        leftArm = false;
        rightArm = false;
        playerFalling = false;

        startTime = Time.time;

        runSound.Play();
    }

    /** 
     *  Update is called once per frame
     */
    void Update()
    {
        playerBody.velocity = new Vector2(playerSpeed, playerBody.velocity.y);
        CheckCollision();
        SetAnimatorBool();
        CheckJump();
        IncreaseSpeed();
    }

    /**
     *  Set the variable in the Animator
     */
    void SetAnimatorBool()
    {
        onGround = Physics2D.OverlapCircle(groundDetection.position, groundDetectionRadius, groundMaterial);
        isBendDown = Input.GetKey(KeyCode.S) && onGround;
        leftArm = Input.GetKey(KeyCode.Q) && onGround && !Input.GetKey(KeyCode.D) && !isBendDown;
        rightArm = Input.GetKey(KeyCode.D) && onGround && !Input.GetKey(KeyCode.Q) && !isBendDown;
        playerFalling = playerBody.velocity.y < -0.1;

        playerAnimator.SetFloat("playerSpeed", playerBody.velocity.x);
        playerAnimator.SetBool("onGround", onGround);
        playerAnimator.SetBool("isBendDown", isBendDown);
        playerAnimator.SetBool("leftArm", leftArm);
        playerAnimator.SetBool("rightArm", rightArm);
        playerAnimator.SetBool("playerFalling", playerFalling);
    }

    /**
     *  Check if the player want to jump 
     */
    void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround && !GameObject.Find("PauseMenu"))
            {
                playerBody.velocity = new Vector2(playerBody.velocity.x, playerJumpForce);
                StartCoroutine(SwitchSoundJump());
            }
        }
    }

    /**
     *  Check if the caracter is blocked 
     */
    void CheckCollision()
    {
        if (lastCharacterPosition == transform.position.x && !GameObject.Find("PauseMenu"))
        {
            transform.position = new Vector2(transform.position.x + 0.01f, transform.position.y);
        }
        lastCharacterPosition = transform.position.x;
    }

    /**
     *  Co-routine to switch jump sound and footsteps sound
     */
    IEnumerator SwitchSoundJump()
    {
        runSound.Pause();
        jumpSound.Play();
        yield return new WaitForSeconds(0.75f);
        runSound.Play();
    }

    /** 
     *  Load the adapted action when the player collides with an object
     */
    void OnTriggerEnter2D(Collider2D item)
    {
        if (item.gameObject.tag == "kill")
        {
            SetData(item.gameObject.name);
            string playerName = PlayerPrefs.GetString("PlayerName");
            //Data are set only during the body mode, for a real player
            if (!playerName.Equals("Default") && PlayerPrefs.GetInt("BodyMode") == 1) SendData(playerName);
            runSound.Stop();
            killedSound.Play();
            game.LoadDeathMenu();
        }
        else if (item.gameObject.tag == "water")
        {
            runSound.Stop();
            drownSound.Play();
        }
        else if (item.gameObject.tag == "coin" || (item.gameObject.tag == "coinL" && leftArm) || (item.gameObject.tag == "coinR" && rightArm) || (item.gameObject.tag == "coinB" && isBendDown))
        {
            item.gameObject.SetActive(false);
            coinsManager.CollectCoin(item.gameObject);
        }
    }

    /** 
     *  Load the adapted action when the player is colliding with an object
     */
    private void OnTriggerStay2D(Collider2D item)
    {
        if (item.gameObject.tag == "coin" || (item.gameObject.tag == "coinL" && leftArm) || (item.gameObject.tag == "coinR" && rightArm) || (item.gameObject.tag == "coinB" && isBendDown))
        {
            item.gameObject.SetActive(false);
            coinsManager.CollectCoin(item.gameObject);
        }
    }

    /** 
     *  Increase the speed of the player
     */
    void IncreaseSpeed()
    {
        if (playerSpeed < maxSpeed)
        {
            playerSpeed = startSpeed + Mathf.Log10(Time.time - startTime + 1) / 10;
        }
    }

    //WARNING! CHANGE startSpeed AND startTime ( ReserSpeed() ) Between INCREASE AND DECREASE

    /** 
     *  Decrease the speed of the player
     */
    void DecreaseSpeed()
    {
        if (playerSpeed > startSpeed)
        {
            playerSpeed = startSpeed - Mathf.Log10(Time.time - startTime + 1) / 10;
        }
    }

    /** 
     *  Reset the speed of the player
     */
    public void ResetSpeed()
    {
        startTime = Time.time;
    }

    /** 
     *  Set the data file of the player
     */
    public void SetData(string platformName)
    {
        if (data.ContainsKey(platformName))
        {
            data[platformName]++;
        }
    }

    /** 
     *  Remove the platform that the player has not yet crossed
     */
    public void ModifyData()
    {
        GameObject[] traps = GameObject.FindGameObjectsWithTag("trapPlatform");
        GameObject[] water = GameObject.FindGameObjectsWithTag("water");
        string trapName = "";

        for (int i = 0; i < traps.Length; i++)
        {
            trapName = traps[i].name;
            trapName = trapName.Remove(trapName.Length - 7);
            if (traps[i].transform.position.x > gameObject.transform.position.x + 3 && data.ContainsKey(trapName)) data[trapName] = data[trapName] - 1;
        }

        for (int i = 0; i < water.Length; i++)
        {
            trapName = water[i].name;
            trapName = trapName.Remove(trapName.Length - 7);
            if (water[i].transform.position.x > gameObject.transform.position.x + 3 && data.ContainsKey(trapName)) data[trapName] = data[trapName] - 1;
        }
    }

    /** 
     *  Set the data file of the player
     */
    public void SendData(string playerName)
    {
        ModifyData();
        PlayerData player = new PlayerData(playerName, data["platform5"], data["platform6"], data["platform7"], data["platform10"], data["spikes"], data["Catcher"], data["evilBlock"], data["evilSaw"], PlayerPrefs.GetInt("RandomMode"));
        DataFileManager.AddNewPlayer(player);
    }
}
