﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this, 3.0f);
    }
}