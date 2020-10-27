using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightingTextureRenderer2D : MonoBehaviour {
	public LightingLayer nightLayer = LightingLayer.Layer1;
    public Texture texture;
	public Color color = Color.white;
    public Vector2 size = Vector2.one;

	public enum ShaderMode {Additive, Multiply}

	public ShaderMode shaderMode = ShaderMode.Additive;

	public static List<LightingTextureRenderer2D> list = new List<LightingTextureRenderer2D>();

	public void OnEnable() {
		list.Add(this);

		LightingManager2D.Get();
	}

	public void OnDisable() {
		list.Remove(this);
	}

    static public List<LightingTextureRenderer2D> GetList() {
		return(list);
	}

	public bool InCamera(Camera camera) {
		float cameraRadius = CameraHelper.GetRadius(camera);
		float distance = Vector2.Distance(transform.position, camera.transform.position);
		float radius = cameraRadius + Mathf.Sqrt((size.x * size.x) * (size.y * size.y));

		return(distance < radius);
	}
}
