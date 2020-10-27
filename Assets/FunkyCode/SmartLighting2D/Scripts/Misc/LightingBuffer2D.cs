using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using LightingSettings;

[ExecuteInEditMode]
public class LightingBuffer2D {
	public string name = "unknown";

	private LightingSource2D light;

	public LightingSource2D Light {
		get => light;
		set {
			light = value;

			Rendering.LightingBuffer.UpdateName(this);
		}
	}

	public bool Free {
		get => light == null;
	}

	public LightTexture renderTexture;

	public bool updateNeeded = false;

	// Lighting Atlas Batching
	public LightingAtlasBatches lightingAtlasBatches = new LightingAtlasBatches();

	public static List<LightingBuffer2D> list = new List<LightingBuffer2D>();

	public LightingBuffer2D() {
		list.Add(this);
	}

	public static void Clear() {
		foreach(LightingBuffer2D buffer in new List<LightingBuffer2D>(list)) {
			if (buffer.light) {
				buffer.light.Buffer = null;
			}

			buffer.DestroySelf();
		}

		list.Clear();
	}

	public void DestroySelf() {
		if (renderTexture != null) {
			if (renderTexture.renderTexture != null) {
				
				if (Application.isPlaying) {
					UnityEngine.Object.Destroy (renderTexture.renderTexture);
				} else {
					UnityEngine.Object.DestroyImmediate (renderTexture.renderTexture);
				}
				
			}
		}

		list.Remove(this);
	}

	static public List<LightingBuffer2D> GetList() {
		return(list);
	}

	public void Initiate (int textureSize) {
		Rendering.LightingBuffer.InitializeRenderTexture(this, textureSize);
	}

	public void Render() {
		if (updateNeeded == false) {
			return;
		}
		
		updateNeeded = false;

		if (light == null) {
			return;
		}

		RenderTexture previous = RenderTexture.active;

		RenderTexture.active = renderTexture.renderTexture;

		// Clear
		if (light.litMode == LightingSource2D.LitMode.Everything) {
			GL.Clear(true, true, Color.white);
		} else {
			GL.Clear(true, true, Color.black);
		}
	
		Rendering.LightingBuffer.Render(light);

		RenderTexture.active = previous;
	}
}