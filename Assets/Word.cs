using System.Collections.Generic;


public class Word
{
    public string value;
    // hashset- only using Contains method
    public HashSet<int> syllableBreakIndecies = new HashSet<int>();
    public Word(string value, int[] syllableBreakIndecies)
    {
        this.value = value;
        foreach (int i in syllableBreakIndecies)
        {
            this.syllableBreakIndecies.Add(i);
        }
    }
    public bool IsCorrectSplit(int breakInd)
    {
        return syllableBreakIndecies.Contains(breakInd);
    }
}