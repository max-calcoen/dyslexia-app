using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class ConjunctionScript : MonoBehaviour
{
    private TouchMonitor touchMonitor = new TouchMonitor(new Vector2(-1, -1), new Vector2(-1, -1));
    private string[] conjunctions =
    new string[] {
        "for",
        "and",
        "nor",
        "but",
        "or",
        "yet",
        "so",
        "after",
        "although",
        "as soon as",
        "because",
        "before",
        "by the time",
        "in case",
        "now that",
        "since",
        "unless",
        "when",
        "while",
        "whenever"
    };
    private string[] nonconjunctions =
    new string[] {
        "never",
        "ever",
        "in",
        "at",
        "on",
        "for",
        "under",
        "with",
        "from"
    };
    private bool currCorrect;
    public TextMeshProUGUI word;
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip incorrectSound;

    // Start is called before the first frame update
    void Start()
    {
        RenderNewWord();
    }

    public void RenderNewWord()
    {
        string newWord = GetRandomWord();
        word.text = newWord;
    }

    public string GetRandomWord()
    {
        if (Random.Range(1, 3) == 2)
        {
            // generate wrong pair
            currCorrect = false;
            System.Random rand = new System.Random();
            return nonconjunctions[rand.Next(nonconjunctions.Length)];
        }
        else
        {
            // return regular pair
            currCorrect = true;
            System.Random rand = new System.Random();
            return conjunctions[rand.Next(conjunctions.Length)];
        }
    }
    private void HandleTouchStarted()
    {
        Touch touch = Input.GetTouch(0);
        touchMonitor.Start = touch.position;
    }

    private void HandleTouchEnded()
    {
        Touch touch = Input.GetTouch(0);
        touchMonitor.End = touch.position;
        if (
            touchMonitor.End.y > touchMonitor.Start.y // swipe up: user
            &&                                        // thinks word is conjunction
            currCorrect

            ||

            touchMonitor.End.y < touchMonitor.Start.y // swipe down: user
            &&                                        // thinks word isn't conjunction
            !currCorrect
            )
        {
            HandleCorrect();
        }
        else
        {
            HandleIncorrect();
        }
    }

    private void HandleCorrect()
    {
        audioSource.clip = correctSound;
        audioSource.Play();
        RenderNewWord();
    }
    private void HandleIncorrect()
    {
        audioSource.clip = incorrectSound;
        audioSource.Play();
        RenderNewWord();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0) return;
        Touch touch = Input.GetTouch(0);
        // touch started
        if (touch.phase == TouchPhase.Began) HandleTouchStarted();
        // touch ended
        else if (touch.phase == TouchPhase.Ended) HandleTouchEnded();
    }
}
