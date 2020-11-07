﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingSettings;

public class LightingRender2D {
	public static Mesh preRenderMesh = null;

	public static Mesh GetMesh() {
		if (preRenderMesh == null) {
			Mesh mesh = new Mesh();

			mesh.vertices = new Vector3[]{new Vector3(-1, -1), new Vector3(1, -1), new Vector3(1, 1), new Vector3(-1, 1)};
			mesh.triangles = new int[]{2, 1, 0, 0, 3, 2};
			mesh.uv = new Vector2[]{new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1)};

			preRenderMesh = mesh;
		}
		return(preRenderMesh);
	}

	static public int GetTextureSize(LightingSourceTextureSize textureSize) {
		switch(textureSize) {
			case LightingSourceTextureSize.px2048:
				return(2048);

			case LightingSourceTextureSize.px1024:
				return(1024);

			case LightingSourceTextureSize.px512:
				return(512);

			case LightingSourceTextureSize.px256:
				return(256);
			
			default:
				return(128);
		}
	}

	public static Vector3 GetSize(Camera camera) {
		float sizeY = camera.orthographicSize;

		Vector3 size = new Vector2(sizeY, sizeY);
		
		size.x *= ((float)camera.pixelRect.width / (float)camera.pixelRect.height);
		size.x *= (((float)camera.pixelRect.width + 1f) / camera.pixelRect.width);

		size.y *= (((float)camera.pixelRect.height + 1f) / camera.pixelRect.height);
		
		size.z = 1;

		return(size);
	}

	// Post-Render Mode Drawing
	public static void PostRender(LightingMainBuffer2D mainBuffer) {
		Camera camera = mainBuffer.cameraSettings.GetCamera();

		if (camera == null) {
			return;
		}

		if (mainBuffer.cameraSettings.renderMode == CameraSettings.RenderMode.Disabled) {
			return;
		}

		if (Lighting2D.renderingMode != RenderingMode.OnPostRender) {
			return;
		}

		if (Camera.current != camera) {
			return;
		}

		Rendering.Universal.WithoutAtlas.Texture.Draw(mainBuffer.GetMaterial(), LightingPosition.GetCamera(camera), GetSize(camera), camera.transform.eulerAngles.z, LightingPosition.GetCamera(camera).z);
	}

	// Mesh-Render Mode Drawing
	static public void OnRender(LightingMainBuffer2D mainBuffer) {
		Camera camera = mainBuffer.cameraSettings.GetCamera();

		if (camera == null) {
			return;
		}

		if (mainBuffer.cameraSettings.renderMode == CameraSettings.RenderMode.Disabled) {
			return;
		}

		if (Lighting2D.renderingMode != RenderingMode.OnRender) {
			return;
		}
		
		OnRenderMode onRenderMode = OnRenderMode.Get(mainBuffer);
		if (onRenderMode == null) {
			return;
		}
		
		onRenderMode.UpdatePosition();

		if (onRenderMode.meshRenderer != null) {
			if (mainBuffer.cameraSettings.renderMode == CameraSettings.RenderMode.Hidden) {
				onRenderMode.meshRenderer.enabled = false;
				return;
			}

			onRenderMode.meshRenderer.enabled = true;
			if (onRenderMode.meshRenderer.sharedMaterial != mainBuffer.GetMaterial()) {
				onRenderMode.meshRenderer.sharedMaterial = mainBuffer.GetMaterial();
			}
			
			if (onRenderMode.meshRenderer.sharedMaterial == null) {
				onRenderMode.meshRenderer.sharedMaterial = mainBuffer.GetMaterial();
			}
		}
	}

	// Graphics.Draw() Mode Drawing
	static public void PreRender(LightingMainBuffer2D mainBuffer) {
		Camera camera = mainBuffer.cameraSettings.GetCamera();

		if (camera == null) {
			return;
		}

		if (mainBuffer.cameraSettings.renderMode == CameraSettings.RenderMode.Disabled) {
			return;
		}

		if (Lighting2D.renderingMode != RenderingMode.OnPreRender) {
			return;
		}

		Graphics.DrawMesh(LightingRender2D.GetMesh(), Matrix4x4.TRS(LightingPosition.GetCamera(camera), camera.transform.rotation, GetSize(camera)), mainBuffer.GetMaterial(), 0, camera);
	}
}