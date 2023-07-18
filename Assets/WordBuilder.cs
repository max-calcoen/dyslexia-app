using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordBuilder : MonoBehaviour
{
    private Word currWord;
    //private TextMeshProUGUI text;
    private const float SPACING = 3f;
    private TouchMonitor touchMonitor = new(new Vector2(-1, -1), new Vector2(-1, -1));
    private List<GameObject> letters = new();
    private List<float> breaks = new();
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    // Start is called before the first frame update
    void Start()
    {
        RenderNewWord();
    }
    private void RenderNewWord()
    {
        // clear children
        foreach (Transform child in this.transform)
        {
            // Destroy the child GameObject
            Destroy(child.gameObject);
        }
        // reset lists
        breaks = new List<float>();
        letters = new List<GameObject>();
        // generate random word
        currWord = GetRandomWord();
        // generate children
        StartCoroutine(GenerateChildren());
    }
    // totalWidth: total width of word
    private IEnumerator GenerateChildren()
    {
        // ----generate letter objects----
        // initialize empty letter list
        int i = 0;
        foreach (char ch in currWord.value)
        {
            // create gameobject for each letter
            GameObject newLetter = new("Letter" + ch.ToString().ToUpper() + "_" + i);
            // add to list so can be referenced later
            letters.Add(newLetter);
            // set as child of "Word"
            newLetter.transform.parent = this.transform;
            // add textMeshPro component
            TextMeshProUGUI tmpComponent = newLetter.AddComponent<TextMeshProUGUI>();
            // set text
            tmpComponent.text = ch.ToString();
            // set font size
            tmpComponent.fontSize = 200;
            // get RectTransform component
            RectTransform rectTransform = newLetter.GetComponent<RectTransform>();
            // make sure width is 0 so the letters are center aligned
            rectTransform.sizeDelta = new Vector2(0f, rectTransform.sizeDelta.y);
            i++;
        }
        // ----wait for them to render----
        // coroutine magic, wait until next frame
        yield return null;
        // ----get their rendered widths and use to calculate position on screen----
        // iterate through and get rendered widths, find largest height
        float totalWidth = 0;
        float largestHeight = -1;
        foreach (GameObject letter in letters)
        {
            // get textMeshPro component
            TextMeshProUGUI tmpComponent = letter.GetComponent<TextMeshProUGUI>();
            // add width to total width
            totalWidth += tmpComponent.preferredWidth;
            // check largest hight
            if (tmpComponent.preferredHeight > largestHeight) largestHeight = tmpComponent.preferredHeight;
        }
        // calculate position
        // last X
        float lastX = -totalWidth / 2 - SPACING; // initial x - spacing (first doesn't require spacing)
        // last width
        float lastWidth = 0f;
        foreach (GameObject letter in letters)
        {
            // get textMeshPro component
            TextMeshProUGUI tmpComponent = letter.GetComponent<TextMeshProUGUI>();
            // get width of letter
            float width = tmpComponent.preferredWidth;
            // calculate x position
            float x = lastX + lastWidth + SPACING;
            // get RectTransform component
            RectTransform rectTransform = letter.GetComponent<RectTransform>();
            // set new x
            rectTransform.anchoredPosition = new Vector2(x, largestHeight / 2);
            // remember for next iteration
            lastX = x;
            lastWidth = width;
        }
        // calculate breaks
        CalculateBreaks();
    }

    private void CalculateBreaks()
    {
        // iterate through letters
        for (int i = 0; i < letters.Count; i++)
        {
            GameObject letter = letters[i];
            // get RectTransform object
            RectTransform rect = letter.GetComponent<RectTransform>();
            // get position of letter
            Vector2 anchoredPos = rect.anchoredPosition;
            // add break position to list
            breaks.Add(anchoredPos.x - SPACING / 2.0f);
        }
        // last letter has break after
        GameObject lastLetter = letters[^1];
        breaks.Add(
            lastLetter.GetComponent<RectTransform>().anchoredPosition.x +
            lastLetter.GetComponent<TextMeshProUGUI>().preferredWidth + // right edge
            SPACING / 2
        );

        
    }

    private Word GetRandomWord()
    {
        List<Word> words = new()
        {
            new Word("sample", new int[] { 3 }),
            new Word("water", new int[] { 2 }),
            new Word("simple", new int[] { 3 }),
            new Word("monkey", new int[] { 3 }),
            new Word("temple", new int[] { 3 }),
            new Word("goodnight", new int[] { 4 }),
            new Word("crumple", new int[] { 4 }),
            new Word("passion", new int[] { 3 }),
            new Word("pasta", new int[] { 3 }),
            new Word("asphalt", new int[] { 2 }),
            new Word("Texas", new int[] { 3 }),
            new Word("Denver", new int[] { 3 }),
            new Word("impose", new int[] { 2 }),
            new Word("poster", new int[] { 4 }),
            new Word("minute", new int[] { 3 }),
            new Word("peanut", new int[] { 3 }),
            new Word("walrus", new int[] { 3 }),
            new Word("antman", new int[] { 3 }),
            new Word("wonder", new int[] { 3 }),
            new Word("cancer", new int[] { 3 }),
        };
        Word word = words[UnityEngine.Random.Range(0, words.Count)];
        return word;
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
        float avgX = (touchMonitor.Start.x + touchMonitor.End.x) / 2.0f;
        avgX -= Screen.width / 2;
        // get index of break
        float currentVal = breaks[0];
        int i = 0;
        int index;
        // out of bounds left
        if (avgX < breaks[0]) index = 0;
        // out of bounds right
        else if (avgX > breaks[^1]) index = breaks.Count - 1;
        // search for closest break
        while (avgX > currentVal)
        {
            i++;
            currentVal = breaks[i];
        }
        float largerVal = breaks[i];
        float smallerVal = breaks[i - 1];
        index = largerVal - avgX < avgX - smallerVal ? i : i - 1;
        if (currWord.IsCorrectSplit(index))
        {
            audioSource.clip = correctSound;
            audioSource.Play();
            RenderNewWord();
        } else
        {
            audioSource.clip = incorrectSound;
            audioSource.Play();
        }
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
