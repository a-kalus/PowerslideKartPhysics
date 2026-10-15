using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    public enum HapticType
    {
        Idle,
        Jump,
        Landing,
        Speed,
        Brake,
        DriftRight,
        DriftLeft,
        CurveRightStart,
        CurveRightPeak
    }

    public HapticTrigger.HapticType hapticType;
    public float intensityMultiplier = 1;
    public float durationMultiplier = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
