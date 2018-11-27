using UnityEngine;

[System.Serializable]
public class InputAxisPair
{
    [SerializeField]
    InputAxis x;
    [SerializeField]
    InputAxis y;

    public InputAxisPair(InputAxis x, InputAxis y)
    {
        this.x = x;
        this.y = y;
    }

    public float X { get { return x.Value; } }
    public float RawX { get { return x.RawValue; } }

    public float Y { get { return y.Value; } }
    public float RawY { get { return y.RawValue; } }

    public Vector2 Vec2 { get { return new Vector2(x.Value, y.Value); } }
    public Vector2 RawVec2 { get { return new Vector2(x.RawValue, y.RawValue); } }
}
