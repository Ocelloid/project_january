using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEffect : MonoBehaviour {
    public float AgeLimit;
    float age;

    void Awake() {
        AgeLimit = 2f;
    }

    void Update() {
        if (age > AgeLimit) {
            Destroy(gameObject);
            return;
        }

        age += Time.deltaTime;
    }
}