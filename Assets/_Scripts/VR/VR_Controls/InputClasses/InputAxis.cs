using UnityEngine;

[System.Serializable]
public class InputAxis
{
    [SerializeField]
    public string name;
    [SerializeField]
    bool inverted;

    public InputAxis(int axisNum, bool inverted)
    {
        this.inverted = inverted;
        name = "Axis" + axisNum;

    }

    public float Value
    {
        get
        {
            float v = Input.GetAxis(name);
            v = inverted ? -v : v;
            return v;
        }
    }
    public float RawValue
    {
        get
        {
            float v = Input.GetAxisRaw(name);
            v = inverted ? -v : v;
            return v;
        }
    }

    public static string FromInt(int num)
    {
        if (num < 0)
        {
            Debug.LogError("Axis" + num + " does not exist!");
            return "";
        }

        return "Axis" + num;
    }

    public static string FromIntBool(int num, bool inverted)
    {
        if (num < 0)
        {
            Debug.LogError("Axis" + num + " does not exist!");
            return "";
        }

        string sign = "";
        if (inverted) { sign = "(inverted) "; }

        return sign + "Axis" + num;
    }

    public static int ToInt(string name)
    {
        char[] chars = name.ToCharArray();
        char last = chars[chars.Length - 1];

        if (!char.IsDigit(last)) { return -1; }

        int num = (int)System.Char.GetNumericValue(last);

        char secondLast = chars[chars.Length - 2];
        if (char.IsDigit(last))
        {
            num += ((int)System.Char.GetNumericValue(last)) * 10;
        }
        return num;
    }
}

public struct AxisParameters
{
    public int num;
    public bool inverted;

    public AxisParameters(int num, bool inverted)
    {
        this.num = num;
        this.inverted = inverted;
    }
}