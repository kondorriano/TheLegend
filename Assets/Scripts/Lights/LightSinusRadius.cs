﻿using UnityEngine;
using System.Collections;

public class LightSinusRadius : MonoBehaviour
{

    Light dL;
    float[] randVal;
    float originalRadius;
    void Start()
    {
        dL = GetComponent<Light>();
        originalRadius = dL.range;
        randVal = new float[8];
        for (int i = 0; i < randVal.Length; ++i)
        {
            if (i % 2 == 0) randVal[i] = Random.Range(0f, 0.2f);
            else randVal[i] = Random.Range(15f, 20f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float noise = 0;
        for (int i = 0; i < randVal.Length; i += 2)
        {
            noise += randVal[i] * Mathf.Sin(Time.time * randVal[i + 1]);
        }

        dL.range = originalRadius + noise;
    }
}