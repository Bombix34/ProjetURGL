using UnityEngine;

namespace LightingSettings {
	
	[System.Serializable]
	public class QualitySettings {
		public static string[] LightingSourceTextureSizeArray = new string[]{"Custom", "2048", "1024", "512", "256", "128"};

		public bool HDR = true;
	
		public LightingSourceTextureSize lightTextureSize = LightingSourceTextureSize.px2048;
		public FilterMode lightFilterMode = FilterMode.Bilinear;
		public UpdateMethod updateMethod = UpdateMethod.LateUpdate;
		public ManagerInstance managerInstance = ManagerInstance.Static;
		public ManagerInternal managerInternal = ManagerInternal.HideInHierarchy;

		public enum ManagerInternal {
			HideInHierarchy,
			ShowInHierarchy
		}
		public enum ManagerInstance {
			Static,
			DontDestroyOnLoad,
			Dynamic
		}

		public enum UpdateMethod {
			LateUpdate,
			OnPreCull,
			OnRenderObject,
		}
	}

	[System.Serializable]
	public class Layers {
		public LayersList lightLayers = new LayersList();
		public LayersList nightLayers = new LayersList();
		public LayersList dayLayers = new LayersList();

		public Layers() {
			lightLayers.names[0] = "Default";

			nightLayers.names[0] = "Default";

			dayLayers.names[0] = "Default";
		}
	}

	[System.Serializable]
	public class LayersList {
		public string[] names = new string[1];

		public string[] GetNames() {
			string[] layers = new string[names.Length];

			for(int i = 0; i < names.Length; i++) {
				layers[i] = names[i];
			}

			return(layers);
		}
	}

	[System.Serializable]
	public class FogOfWar {
		public enum ShaderMode {Default, WithMask}
		public bool enabled = false;

		public int bufferID = 0;

		[Range(0, 1)]
		public float resolution = 1;

		public ShaderMode shaderMode = ShaderMode.Default;

		public SortingLayer sortingLayer = new SortingLayer();
	}

	[System.Serializable]
	public class EditorViewSettings {
		public bool enable = true;
	}

	[System.Serializable]
	public class DayLightingSettings {
		public bool enable = false;

		[Range(0, 1)]
		public float alpha = 1;

		[Range(0, 360)]
		public float direction = 270;

		[Range(0, 10)]
		public float height = 1;

		public Softness softness = new Softness();

		public BumpMap bumpMap = new BumpMap();

		[System.Serializable]
		public class Softness {
			public bool enable = true;
			public float intensity = 0.5f;
		}


		// Is this only bumpmap settings?
		[System.Serializable]
		public class BumpMap {
			[Range(0, 5)]
			public float height = 1;

			[Range(0, 5)]
			public float strength = 1;
		}
	}

	[System.Serializable]
	public class SortingLayer {
		[SerializeField]
		private string name = "Default";
		public string Name {
			get {

				if (name.Length < 1) {
					name = "Default";
				}

				return(name);
			} 

			set => name = value;
		}

		public int Order = 0;

		public void ApplyToMeshRenderer(MeshRenderer meshRenderer) {
			if (meshRenderer == null) {
				return;
			}
			
			if (meshRenderer.sortingLayerName != Name) {
				meshRenderer.sortingLayerName = Name;
			}

			if (meshRenderer.sortingOrder != Order) {
				meshRenderer.sortingOrder = Order;
			}
		}
	}

	// Batching
	[System.Serializable]
	public class AtlasSettings {
		public static string[] SpriteAtlasSizeArray = new string[]{"2048", "1024", "512", "256"};

		public bool lightingSpriteAtlas = false;
		//public SpriteAtlasSize spriteAtlasSize = SpriteAtlasSize.px1024;

		public int spriteAtlasPreloadFoldersCount = 0;
		public string[] spriteAtlasPreloadFolders = new string[1];
	}

	
	[System.Serializable]
	public class EditorView {
		public enum DrawGizmos {Disabled, Selected, Always}
		public enum GizmosBounds {None, Rectangle, Radius}

		public DrawGizmos drawGizmos = DrawGizmos.Selected;
		public GizmosBounds drawGizmosBounds = GizmosBounds.None; 
		
		public int layer = 0;
	}

	[System.Serializable]
	public class MeshMode {
		public enum MeshModeShader {Additive, Alpha, FogOfWar, Custom}
		public bool enable = false;

		[Range(0, 1)]
		public float alpha = 0.5f;

		public MeshModeShader shader = MeshModeShader.Additive;
		public Material[] materials = new Material[1];

		public LightingSettings.SortingLayer sortingLayer = new LightingSettings.SortingLayer();
	}

	
	[System.Serializable]
	public class BumpMapMode {
		public NormalMapType type = NormalMapType.PixelToLight;
		
		public NormalMapTextureType textureType = NormalMapTextureType.Texture;
		
		public Texture texture;
		public Sprite sprite;

		public Texture GetBumpTexture() {
			switch(textureType) {
				case NormalMapTextureType.Sprite:
					if (sprite == null) {
						return(null);
					}

					return(sprite.texture);

				case NormalMapTextureType.Texture:
					return(texture);
			}
			
			return(null);
		}
	}

	[System.Serializable]
	public class DayNormalMapMode {
		public NormalMapTextureType textureType = NormalMapTextureType.Texture;
		
		public Texture texture;
		public Sprite sprite;

		public Texture GetBumpTexture() {
			switch(textureType) {
				case NormalMapTextureType.Sprite:
					if (sprite == null) {
						return(null);
					}

					return(sprite.texture);

				case NormalMapTextureType.Texture:
					return(texture);
			}
			
			return(null);
		}
	}

	[System.Serializable]
	public class GlowMode {
		public bool enable = false;

		[Range(1, 10)]
		public int glowSize = 1;

		[Range(1, 10)]
		public int glowIterations = 1;
	}
	
}
