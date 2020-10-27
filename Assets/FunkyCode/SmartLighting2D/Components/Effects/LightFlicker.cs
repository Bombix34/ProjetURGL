﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {
    public float flickersPerSecond = 15f;
    public float flickerRangeMin = -0.1f;
    public float flickerRangeMax = 0.1f;

    LightingSource2D lightSource;
    float lightAlpha;
    TimerHelper timer;

    void Start() {
        lightSource = GetComponent<LightingSource2D>();
        lightAlpha = lightSource.color.a;
        
        timer = TimerHelper.Create();
    }

    void Update() {
        if (timer == null) {
            return;
        }

        if (timer.GetMillisecs() > 1000f / flickersPerSecond) {
            float tempAlpha = lightAlpha;
            tempAlpha = tempAlpha + Random.Range(flickerRangeMin, flickerRangeMax);
            lightSource.color.a = tempAlpha;
            timer.Reset();
        }
    }
}
