using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] 
public class FogOfWarSprite : MonoBehaviour {
    public static List<FogOfWarSprite> list = new List<FogOfWarSprite>();

    private Sprite sprite;
    private SpriteRenderer spriteRenderer;
    private Material material;

    public SpriteMeshObject spriteMeshObject = new SpriteMeshObject();

    public Sprite GetSprite() {
        SpriteRenderer spriteRenderer = GetSpriteRenderer();

        if (spriteRenderer != null) {
            return(spriteRenderer.sprite);
        }

        return(null);
    }

    public int GetSortingOrder() {
        SpriteRenderer spriteRenderer = GetSpriteRenderer();

        if (spriteRenderer != null) {
            return(spriteRenderer.sortingOrder);
        } else {
            return(0);
        }
    }

    public int GetSortingLayer() {
        SpriteRenderer spriteRenderer = GetSpriteRenderer();

        if (spriteRenderer != null) {
            return(SortingLayer.GetLayerValueFromID(spriteRenderer.sortingLayerID));
        } else {
            return(0);
        } 
    }

    public void Update() {
        SpriteRenderer spriteRenderer = GetSpriteRenderer();

        if (spriteRenderer == null) {
            return;
        }

        if (Lighting2D.fogOfWar.useOnlyInPlay) {
            if (Application.isPlaying) {
                spriteRenderer.enabled = false;
            } else {
                spriteRenderer.enabled = true;
            }
        } else {
            spriteRenderer.enabled = false;
        }
    }

    public SpriteRenderer GetSpriteRenderer() {
        if (spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        material = spriteRenderer.sharedMaterial;
        return(spriteRenderer);
    }

    public void OnEnable() {
		list.Add(this);
	}

	public void OnDisable() {
		list.Remove(this);
	}

    static public List<FogOfWarSprite> GetList() {
		return(list);
	}
}
