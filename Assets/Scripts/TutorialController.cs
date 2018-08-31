using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public AudioSource selectSound;
    public AudioSource coinsSound;
    public AudioSource runSound;
    public AudioSource jumpSound;
    public AudioSource congratsSound;

    private bool playerFalling;
    private bool onGround;
    private bool isBendDown;
    private bool leftArm;
    private bool rightArm;

    private int index;

    private float playerSpeed;
    private float playerJumpForce;

    private GameObject player;
    private Rigidbody2D playerBody;
    private Animator playerAnimator;

    private LayerMask groundMaterial;
    private Transform groundDetection;
    private float groundDetectionRadius;

    private GameObject startingPoint;
    private GameObject endingPoint;

    private Text tutorialText;
    private Text orderText;

    private string[] nextObjectTutorial;
    private string[] nextTextTutorial;

    /**
     *  Initialization
     */
    void Start()
    {
        tutorialText = GameObject.Find("TutorialText").GetComponent<Text>();
        orderText = GameObject.Find("OrderText").GetComponent<Text>();

        groundMaterial = LayerMask.GetMask("Ground");
        groundDetection = GameObject.Find("GroundCollision").GetComponent<Transform>();

        player = GameObject.Find("Player");
        playerBody = player.GetComponent<Rigidbody2D>();
        playerAnimator = player.GetComponent<Animator>();
        groundDetectionRadius = player.GetComponent<BoxCollider2D>().size.x / 2;

        startingPoint = GameObject.Find("StartingPoint");
        endingPoint = GameObject.Find("EndingPoint");

        onGround = false;
        isBendDown = false;
        leftArm = false;
        rightArm = false;
        playerFalling = false;

        index = 0;
        playerSpeed = 3f;
        playerJumpForce = 8f;

        nextObjectTutorial = new string[] { "redCoin", "blueCoin", "yellowCoin", "groundCoin" };
        nextTextTutorial = new string[] { "Entend your left arm (Q Key) to collect red coins", "Entend your right arm (D Key) to collect blue coins", "Jump (Space Key) to collect yellow on air coins or avoid on ground enemies", "Bend Down (S Key) to collect on ground coins or avoid on air enemies" };

        runSound.Play();
        NextTutorial(nextTextTutorial[index], nextObjectTutorial[index]);
        orderText.text = "Let's Try !";
    }

    /**
     *  Update is called once per frame
     */
    void Update()
    {
        playerBody.velocity = new Vector2(playerSpeed, playerBody.velocity.y);
        onGround = Physics2D.OverlapCircle(groundDetection.position, groundDetectionRadius, groundMaterial);
        isBendDown = Input.GetKey(KeyCode.S) && onGround;
        leftArm = Input.GetKey(KeyCode.Q) && onGround && !Input.GetKey(KeyCode.D);
        rightArm = Input.GetKey(KeyCode.D) && onGround && !Input.GetKey(KeyCode.Q);
        playerFalling = playerBody.velocity.y < -0.1;

        playerAnimator.SetFloat("playerSpeed", playerBody.velocity.x);
        playerAnimator.SetBool("onGround", onGround);
        playerAnimator.SetBool("isBendDown", isBendDown);
        playerAnimator.SetBool("leftArm", leftArm);
        playerAnimator.SetBool("rightArm", rightArm);
        playerAnimator.SetBool("playerFalling", playerFalling);

        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, playerJumpForce);
            StartCoroutine(SwitchSoundJump());
        }

        if (player.transform.position.x >= endingPoint.transform.position.x)
        {
            ResetPlayerPosition();
            if (index < nextTextTutorial.Length)
            {
                NextTutorial(nextTextTutorial[index], nextObjectTutorial[index]);
            }
            else
            {
                EndingTutorial();
            }
        }

    }

    /**
     *  Load the next step of the tutorial
     */
    private void NextTutorial(string order, string item)
    {
        tutorialText.text = order;
        GameObject.Find("Objects").transform.Find(item).gameObject.SetActive(true);
    }

    /**
     *  Load the end tutorial sreen
     */
    private void EndingTutorial()
    {
        runSound.Stop();
        congratsSound.Play();
        tutorialText.text = "Congratulations ! You can now play the game !";
        orderText.enabled = false;
        player.SetActive(false);
    }

    /** 
     *  Load the next step of the tutorial after detecting the collision with the object and the good movement (when you enter in collision)
     */
    void OnTriggerEnter2D(Collider2D item)
    {
        if (item.gameObject.tag == "coinL" && leftArm || item.gameObject.tag == "coinR" && rightArm || item.gameObject.tag == "coin" && !onGround || item.gameObject.tag == "coinB" && isBendDown)
        {
            coinsSound.Play();
            item.gameObject.SetActive(false);
            index++;
        }
    }

    /** 
     *  Load the next step of the tutorial after detecting the collision with the object and the good movement (when you are in collision)
     */
    private void OnTriggerStay2D(Collider2D item)
    {
        if (item.gameObject.tag == "coinL" && leftArm || item.gameObject.tag == "coinR" && rightArm || item.gameObject.tag == "coin" && !onGround || item.gameObject.tag == "coinB" && isBendDown)
        {
            coinsSound.Play();
            item.gameObject.SetActive(false);
            index++;
        }
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
     *  Reset the player position at the starting point
     */
    private void ResetPlayerPosition()
    {
        player.transform.position = new Vector2(startingPoint.transform.position.x, startingPoint.transform.position.y);
    }

    /**
     *  Return to the main menu
     */
    public void BackToMainMenu()
    {
        selectSound.Play();
        SceneManager.LoadScene("MainMenu");
    }
}
