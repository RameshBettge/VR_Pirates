using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableCurve", menuName = "ScriptableCurve", order = 1)]
public class ScriptableCurve : ScriptableObject
{
    [SerializeField]
    public AnimationCurve curve;
}
