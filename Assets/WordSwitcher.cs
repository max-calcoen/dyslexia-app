using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class WordSwitcher : MonoBehaviour
{

    private TouchMonitor touchMonitor = new TouchMonitor(new Vector2(-1, -1), new Vector2(-1, -1));
    private Dictionary<string, string> words = new Dictionary<string, string>
    {
        {"time", "A measure in which events can be ordered"},
        {"year", "A period of 365 or 366 days"},
        {"people", "Plural form of person"},
        {"way", "A method, style or manner of doing something"},
        {"day", "A period of 24 hours"},
        {"man", "An adult human male"},
        {"thing", "An object, fact, or entity"},
        {"woman", "An adult human female"},
        {"life", "The existence of an individual or animal"},
        {"child", "A young human being"},
        {"world", "The earth, together with all of its countries and peoples"},
        {"school", "An institution for educating children"},
        {"state", "A nation or territory considered as an organized political community"},
        {"family", "A group consisting of parents and their children"},
        {"student", "A person who is studying at a school or college"},
        {"group", "A number of people or things that are located, gathered, or classed together"},
        {"country", "A nation with its own government, occupying a particular territory"},
        {"problem", "A matter or situation regarded as unwelcome or harmful and needing to be dealt with"},
        {"hand", "The end part of a person's arm"},
        {"part", "A piece or segment of something"},
        {"place", "A particular position or point in space"},
        {"case", "An instance of a particular situation; an example of something occurring"},
        {"week", "A period of seven days"},
        {"company", "A commercial business"},
        {"system", "A set of things working together as parts of a mechanism or network"},
        {"program", "A planned series of future events or performances"},
        {"question", "A sentence worded to elicit information"},
        {"work", "Activity involving mental or physical effort done to achieve a purpose or result"},
        {"government", "The governing body of a nation, state, or community"},
        {"number", "An arithmetical value representing a particular quantity"},
        {"night", "The period of darkness in each twenty-four hours"},
        {"point", "A particular spot, place, or position in an area or on a map, object, or surface"},
        {"home", "The place where one lives permanently"},
        {"water", "A colorless, transparent, odorless liquid"},
        {"room", "A part or division of a building enclosed by walls, floor, and ceiling"},
        {"mother", "A woman in relation to her child or children"},
        {"area", "A region or part of a town, a country, or the world"},
        {"money", "A current medium of exchange in the form of coins and banknotes"},
        {"story", "An account of imaginary or real people and events"},
        {"fact", "A thing that is known or proved to be true"},
        {"month", "Each of the twelve named periods into which a year is divided"},
        {"lot", "A large number or amount; a great deal"},
        {"right", "Morally good, justified, or acceptable"},
        {"study", "The devotion of time and attention to gaining knowledge"},
        {"book", "A written or printed work consisting of pages"},
        {"eye", "The organ of sight in humans and animals"},
        {"job", "A paid position of regular employment"},
        {"word", "A single distinct meaningful element of speech or writing"},
        {"business", "The practice of making one's living by engaging in commerce"},
        {"issue", "An important topic or problem for debate or discussion"},
        {"side", "A position to the left or right of an object, place, or central point"},
        {"kind", "A class or type of people or things having similar characteristics"},
        {"head", "The upper part of the human body, containing the brain, eyes, and mouth"},
        {"house", "A building for human habitation"},
        {"service", "The action of helping or doing work for someone"},
        {"friend", "A person whom one knows and with whom one has a bond of mutual affection"},
        {"father", "A man in relation to his natural child or children"},
        {"power", "The capacity or ability to direct or influence the behavior of others or the course of events"},
        {"hour", "A period of time equal to a twenty-fourth part of a day and night"},
        {"game", "A form of competitive activity or sport played according to rules"},
        {"line", "A long, narrow mark or band"},
        {"end", "The final part of a place, event, or piece of work"},
        {"member", "A person, animal, or plant belonging to a particular group"},
        {"law", "The system of rules recognized by a country or community"},
        {"car", "A road vehicle powered by an engine, typically with four wheels"},
        {"city", "A large town, in particular the chief town of a country or region"},
        {"community", "A group of people living in the same place or having a particular characteristic in common"},
        {"name", "A word or set of words by which a person, animal, place, or thing is known, addressed, or referred to"},
        {"president", "The elected head of a republican state"},
        {"team", "A group of players forming one side in a competitive game or sport"},
        {"minute", "A period of time equal to sixty seconds or a sixtieth of an hour"},
        {"idea", "A thought or suggestion as to a possible course of action"},
        {"kid", "A child or young person"},
        {"body", "The physical structure of a person or an animal"},
        {"information", "Facts provided or learned about something or someone"},
        {"back", "The rear surface of the human body from the shoulders to the hips"},
        {"parent", "A father or mother"},
        {"face", "The front part of a person's head from the forehead to the chin"},
        {"others", "People or things that are additional or different to those already mentioned"},
        {"level", "A position on a scale of amount, quantity, extent, or quality"},
        {"office", "A room, set of rooms, or building used as a place for commercial, professional, or bureaucratic work"},
        {"door", "A hinged, sliding, or revolving barrier at the entrance to a building, room, or vehicle"},
        {"health", "The state of being free from illness or injury"},
        {"person", "A human being regarded as an individual"},
        {"art", "The expression or application of human creative skill and imagination"},
        {"war", "A state of armed conflict between different countries or different groups within a country"},
        {"history", "The study of past events, particularly in human affairs"},
        {"party", "A social gathering of invited guests"},
        {"result", "A consequence, effect, or outcome of something"},
        {"change", "Make or become different"},
        {"morning", "The period of time from sunrise to noon"},
        {"reason", "A cause, explanation, or justification for an action or event"},
        {"research", "The systematic investigation into and study of materials and sources in order to establish facts and reach new conclusions"},
        {"girl", "A female child"},
        {"guy", "A man"},
        {"moment", "A very brief period of time"},
        {"air", "The invisible gaseous substance surrounding the earth"},
        {"teacher", "A person who teaches, especially in a school"},
        {"force", "Strength or energy as an attribute of physical action or movement"},
        {"education", "The process of receiving or giving systematic instruction, especially at a school or university"}
    };
    private bool currCorrect;
    public TextMeshProUGUI word;
    public TextMeshProUGUI definition;
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
        string[] pair = GetPair();
        word.text = pair[0];
        definition.text = pair[1];
    }

    public string[] GetPair()
    {
        if (Random.Range(1, 4) > 2)
        {
            // generate wrong pair
            currCorrect = false;
            System.Random rand = new System.Random();
            List<string> keys = words.Keys.ToList();
            string randomKey = keys[rand.Next(keys.Count)];
            string randomValue = words[randomKey];
            while (randomValue == words[randomKey])
            {
                string randKey = keys[rand.Next(keys.Count)];
                randomValue = words[randKey];
            }
            string[] rtn = { randomKey, randomValue };
            return rtn;
        } else
        {
            // return regular pair
            currCorrect = true;
            System.Random rand = new System.Random();
            List<string> keys = words.Keys.ToList();
            string randomKey = keys[rand.Next(keys.Count)];
            string randomValue = words[randomKey];
            string[] rtn = { randomKey, randomValue };
            return rtn;
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
            touchMonitor.End.x > touchMonitor.Start.x // swipe right: user
            &&                                        // thinks word matched definition
            currCorrect

            ||

            touchMonitor.End.x < touchMonitor.Start.x // swipe left: user
            &&                                        // thinks word didn't match definition
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
