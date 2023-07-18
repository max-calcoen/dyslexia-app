using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OpenSyllableSplit()
    {
        SceneManager.LoadScene("Syllable Split");
    }
    public void OpenCommonWords()
    {
        SceneManager.LoadScene("Common Words");
    }
    public void OpenConjunctions()
    {
        SceneManager.LoadScene("Conjunctions");
    }
}
