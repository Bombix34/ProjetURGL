using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BasicEvents : MonoBehaviour {
    SpriteRenderer spriteRenderer;

    void Awake() {
        LightingCollider2D collider = GetComponent<LightingCollider2D>();

        collider.AddEventOnEnter(OnEnter);
        collider.AddEventOnExit(OnExit);

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnter(LightingSource2D light) {
        spriteRenderer.color = Color.red;
    }

    void OnExit(LightingSource2D light) {
        spriteRenderer.color = Color.green;
    }

    
}
