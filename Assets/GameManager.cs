using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public Sprite[] rupertGrintSprites; // Array of Rupert Grint sprites
    public Sprite[] edSheeranSprites;  // Array of Ed Sheeran sprites
    public Sprite[] margotSprites;     // Array of Margot sprites
    public Sprite[] emmaSprites;      // Array of Emma sprites
    public Sprite[] thandieSprites;   // Array of Thandie sprites
    public Sprite[] zoeSprites;       // Array of Zoe sprites
    public Sprite[] danielSprites;    // Array of Daniel sprites
    public Sprite[] elijahSprites;    // Array of Elijah sprites

    public GameObject[] faceObjects;  // Array of GameObjects for faces
    public float moveSpeed = 2f;      // Speed of sprite movement
    public Vector2 boundsX = new Vector2(-5f, 5f); // X-axis movement bounds
    public Vector2 boundsY = new Vector2(-5f, 5f); // Y-axis movement bounds

    [Header("UI Elements")]
    public Text scoreText; // UI Text to display score
    public Text messageText; // UI Text to display messages (optional)
    public Image wantedPosterImage; // UI Image to display the wanted poster
    public Image successImage; // UI Image to show when the correct face is found
    public Sprite rupertWantedPoster; // Rupert Grint wanted poster sprite
    public Sprite edSheeranWantedPoster; // Ed Sheeran wanted poster sprite
    public Sprite margotWantedPoster; // Margot wanted poster sprite
    public Sprite emmaWantedPoster; // Emma wanted poster sprite
    public Sprite thandieWantedPoster; // Thandie wanted poster sprite
    public Sprite zoeWantedPoster; // Zoe wanted poster sprite
    public Sprite danielWantedPoster; // Daniel wanted poster sprite
    public Sprite elijahWantedPoster; // Elijah wanted poster sprite
    public Sprite rupertSuccessImage; // Rupert success image
    public Sprite edSheeranSuccessImage; // Ed Sheeran success image
    public Sprite margotSuccessImage; // Margot success image
    public Sprite emmaSuccessImage; // Emma success image
    public Sprite thandieSuccessImage; // Thandie success image
    public Sprite zoeSuccessImage; // Zoe success image
    public Sprite danielSuccessImage; // Daniel success image
    public Sprite elijahSuccessImage; // Elijah success image
    public GameObject gameOverPanel; // UI Panel to show game over screen
    public Button restartButton; // Button to restart the game
    private int targetIndex; // Index of the current target (Rupert, Ed, Margot, Emma, Thandie, Zoe, Daniel, Elijah)
    private int score = 0; // Player's score
    private int finalScore = 0;
    private Vector2[] directions; // Movement directions for each sprite
    private string currentTargetTag; // Current target tag for comparison (e.g., "Rupert", "EdSheeran", etc.)


    private bool lookingForRupertOrEd = true; // Whether we are looking for Rupert or Ed
    private bool lookingForMargotOrEmma = false; // Whether we are looking for Margot or Emma
    private bool lookingForThandieOrZoe = false; // Whether we are looking for Thandie or Zoe
    private bool lookingForDanielOrElijah = false; // Whether we are looking for Daniel or Elijah
    [Header("Sound Effects")]
    public AudioClip correctSound; // Sound for correct answer
    public AudioClip incorrectSound; // Sound for incorrect answer
    public AudioClip tapSound; // Sound for incorrect answer
    public AudioSource audioSource; // AudioSource to play sounds
    public AudioSource tapaudioSource; // AudioSource to play sounds
    private bool isGameOver = false; // Flag to check if the game is over

    [DllImport("__Internal")]
  private static extern void SendScore(int score, int game);

    void Start()
    {
        directions = new Vector2[faceObjects.Length];
        AssignSprites();
        UpdateScoreUI();
        // Set up restart button action
        restartButton.onClick.AddListener(RestartGame);
        gameOverPanel.SetActive(false); // Hide the game over panel initially
    }

    void Update()
    {
        if (!isGameOver)
        {
            MoveSprites();
            DetectMouseClick();
        }
    }

    // Assign Rupert Grint, Ed Sheeran, Margot, Emma, Thandie, Zoe, Daniel, and Elijah sprites to face objects
    void AssignSprites()
    {
        // Randomly choose between all the groups (Rupert/Ed, Margot/Emma, Thandie/Zoe, Daniel/Elijah)
        int randomGroup = Random.Range(0, 4);

        switch (randomGroup)
        {
            case 0:
                lookingForRupertOrEd = true;
                lookingForMargotOrEmma = false;
                lookingForThandieOrZoe = false;
                lookingForDanielOrElijah = false;
                break;
            case 1:
                lookingForRupertOrEd = false;
                lookingForMargotOrEmma = true;
                lookingForThandieOrZoe = false;
                lookingForDanielOrElijah = false;
                break;
            case 2:
                lookingForRupertOrEd = false;
                lookingForMargotOrEmma = false;
                lookingForThandieOrZoe = true;
                lookingForDanielOrElijah = false;
                break;
            case 3:
                lookingForRupertOrEd = false;
                lookingForMargotOrEmma = false;
                lookingForThandieOrZoe = false;
                lookingForDanielOrElijah = true;
                break;
        }

        // Randomly assign the target face based on the selected group
        targetIndex = Random.Range(0, faceObjects.Length);

        if (lookingForRupertOrEd)
        {
            currentTargetTag = "Rupert"; // Set target tag to Rupert
        }
        else if (lookingForMargotOrEmma)
        {
            currentTargetTag = "Margot"; // Set target tag to Margot
        }
        else if (lookingForThandieOrZoe)
        {
            currentTargetTag = "Thandie"; // Set target tag to Thandie
        }
        else if (lookingForDanielOrElijah)
        {
            currentTargetTag = "Daniel"; // Set target tag to Daniel
        }

        // Assign the sprites for the faces
        for (int i = 0; i < faceObjects.Length; i++)
        {
            var spriteRenderer = faceObjects[i].GetComponent<SpriteRenderer>();

            if (lookingForRupertOrEd)
            {
                if (i == targetIndex)
                {
                    spriteRenderer.sprite = rupertGrintSprites[Random.Range(0, rupertGrintSprites.Length)];
                    faceObjects[i].tag = "Rupert"; // Tag it as Rupert
                }
                else
                {
                    spriteRenderer.sprite = edSheeranSprites[Random.Range(0, edSheeranSprites.Length)];
                    faceObjects[i].tag = "EdSheeran"; // Tag as Ed Sheeran
                }
            }
            else if (lookingForMargotOrEmma)
            {
                if (i == targetIndex)
                {
                    spriteRenderer.sprite = margotSprites[Random.Range(0, margotSprites.Length)];
                    faceObjects[i].tag = "Margot"; // Tag it as Margot
                }
                else
                {
                    spriteRenderer.sprite = emmaSprites[Random.Range(0, emmaSprites.Length)];
                    faceObjects[i].tag = "Emma"; // Tag as Emma
                }
            }
            else if (lookingForThandieOrZoe)
            {
                if (i == targetIndex)
                {
                    spriteRenderer.sprite = thandieSprites[Random.Range(0, thandieSprites.Length)];
                    faceObjects[i].tag = "Thandie"; // Tag it as Thandie
                }
                else
                {
                    spriteRenderer.sprite = zoeSprites[Random.Range(0, zoeSprites.Length)];
                    faceObjects[i].tag = "Zoe"; // Tag as Zoe
                }
            }
            else if (lookingForDanielOrElijah)
            {
                if (i == targetIndex)
                {
                    spriteRenderer.sprite = danielSprites[Random.Range(0, danielSprites.Length)];
                    faceObjects[i].tag = "Daniel"; // Tag it as Daniel
                }
                else
                {
                    spriteRenderer.sprite = elijahSprites[Random.Range(0, elijahSprites.Length)];
                    faceObjects[i].tag = "Elijah"; // Tag as Elijah
                }
            }

            // Assign random initial directions for movement
            directions[i] = RandomDirection();
        }

        // Update the wanted poster based on the target
        UpdateWantedPoster();
    }

    // Random movement within bounds
    void MoveSprites()
    {
        for (int i = 0; i < faceObjects.Length; i++)
        {
            GameObject face = faceObjects[i];
            face.transform.Translate(directions[i] * moveSpeed * Time.deltaTime);

            // Check bounds and reverse direction if necessary
            Vector3 pos = face.transform.position;
            if (pos.x <= boundsX.x || pos.x >= boundsX.y) directions[i].x *= -1;
            if (pos.y <= boundsY.x || pos.y >= boundsY.y) directions[i].y *= -1;

            face.transform.position = new Vector3(
                Mathf.Clamp(pos.x, boundsX.x, boundsX.y),
                Mathf.Clamp(pos.y, boundsY.x, boundsY.y),
                pos.z
            );
        }
    }

    // Randomize a movement direction
    Vector2 RandomDirection()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    // Detect mouse clicks
    void DetectMouseClick()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            tapaudioSource.PlayOneShot(tapSound);
            if (hit.collider != null)
            {
                GameObject clickedFace = hit.collider.gameObject;

                // Check if the player clicked the correct target
                if (clickedFace.CompareTag(currentTargetTag))
                {
                    score++;
                    finalScore = score;
                    DisplayMessage("You found the right face!");
                    ShowSuccessImage(); // Show success image
                    AssignSprites(); // Reset for next round
                    audioSource.PlayOneShot(correctSound); // Play correct sound
                }
                else
                {
                    score--; // Deduct score for wrong click
                    DisplayMessage("That's not the right face! Try again.");
                    audioSource.PlayOneShot(incorrectSound); // Play incorrect sound
                    if (score <= 0)
                    {
                        TriggerGameOver(); // Trigger game over when score is zero or less
                    }
                }

                UpdateScoreUI();
            }
        }
    }
    // Show the success image for the correct face
    void ShowSuccessImage()
    {
        switch (currentTargetTag)
        {
            case "Rupert":
                successImage.sprite = rupertSuccessImage;
                break;
            case "EdSheeran":
                successImage.sprite = edSheeranSuccessImage;
                break;
            case "Margot":
                successImage.sprite = margotSuccessImage;
                break;
            case "Emma":
                successImage.sprite = emmaSuccessImage;
                break;
            case "Thandie":
                successImage.sprite = thandieSuccessImage;
                break;
            case "Zoe":
                successImage.sprite = zoeSuccessImage;
                break;
            case "Daniel":
                successImage.sprite = danielSuccessImage;
                break;
            case "Elijah":
                successImage.sprite = elijahSuccessImage;
                break;
        }

        successImage.gameObject.SetActive(true); // Display the image
        Invoke(nameof(HideSuccessImage), 1f); // Hide the image after 1 second
    }

    // Hide the success image
    void HideSuccessImage()
    {
        successImage.gameObject.SetActive(false);
    }

    // Trigger the Game Over state
    void TriggerGameOver()
    {
        isGameOver = true; // Set game over flag to true
        SendScore(finalScore, 35);
        gameOverPanel.SetActive(true); // Show the game over panel
        messageText.text = "Game Over! Try again."; // Display the game over message
    }


    // Restart the game
    void RestartGame()
    {
        score = 0; // Reset score
        finalScore = 0;
        isGameOver = false; // Reset the game over flag
        gameOverPanel.SetActive(false); // Hide the game over panel
        AssignSprites(); // Reset the sprites for a new round
        UpdateScoreUI(); // Update the score UI
    }

    // Update the score UI
    void UpdateScoreUI()
    {
        scoreText.text = score.ToString();
    }

    // Update the wanted poster UI based on the target
    void UpdateWantedPoster()
    {
        // Set the poster depending on the current target
        if (currentTargetTag == "Rupert")
        {
            wantedPosterImage.sprite = rupertWantedPoster;
        }
        else if (currentTargetTag == "EdSheeran")
        {
            wantedPosterImage.sprite = edSheeranWantedPoster;
        }
        else if (currentTargetTag == "Margot")
        {
            wantedPosterImage.sprite = margotWantedPoster;
        }
        else if (currentTargetTag == "Emma")
        {
            wantedPosterImage.sprite = emmaWantedPoster;
        }
        else if (currentTargetTag == "Thandie")
        {
            wantedPosterImage.sprite = thandieWantedPoster;
        }
        else if (currentTargetTag == "Zoe")
        {
            wantedPosterImage.sprite = zoeWantedPoster;
        }
        else if (currentTargetTag == "Daniel")
        {
            wantedPosterImage.sprite = danielWantedPoster;
        }
        else if (currentTargetTag == "Elijah")
        {
            wantedPosterImage.sprite = elijahWantedPoster;
        }
    }


    // Display a message for a short duration
    void DisplayMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
            CancelInvoke(nameof(ClearMessage));
            Invoke(nameof(ClearMessage), 2f);
        }
    }

    void ClearMessage()
    {
        if (messageText != null) messageText.text = "";
    }
}
