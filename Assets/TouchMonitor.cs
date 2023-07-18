using UnityEngine;
public struct TouchMonitor
{
    public Vector2 Start { get; set; }
    public Vector2 End { get; set; }
	public TouchMonitor(Vector2 s, Vector2 e)
	{
		Start = s;
		End = e;
	}
}

