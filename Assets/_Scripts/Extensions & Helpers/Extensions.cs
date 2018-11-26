using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
	public static float[] ToArray(this Color c)
    {
        float[] array = new float[4];

        array[0] = c.r;
        array[1] = c.g;
        array[2] = c.b;
        array[3] = c.a;

        return array;
    }
}
