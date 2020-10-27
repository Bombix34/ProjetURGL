using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using LightingTilemapCollider;

#if UNITY_2017_4_OR_NEWER

	[System.Serializable]
	public class LightingTile {
		public Vector2Int gridPosition;
		// public TilemapCollider2D or TilemapRoom

		public Vector2? worldPosition = null;
		public float worldRotation = 0;
		public Vector2 worldScale = Vector2.one;
		public float worldRadius = 1.4f;
			
		public Tile.ColliderType colliderType;

		private Sprite originalSprite;
		private Sprite atlasSprite; // remove

		public Rect uv; // STE
		public bool flipX;
		public bool flipY;
		
		public SpriteMeshObject spriteMeshObject = new SpriteMeshObject();

		private MeshObject shapeMesh = null;
		private CustomPhysicsShape customPhysicsShape = null;
		private List<Polygon2D> customPhysicsShapePolygons = null;
		private List<Polygon2D> localPolygons = null;
		private List<Polygon2D> worldPolygons = null;
		private List<Polygon2D> worldPolygonsCache = null;

		public void SetOriginalSprite(Sprite sprite) {
			originalSprite = sprite;
		}

		public Sprite GetOriginalSprite() {
			return(originalSprite);
		}

		// Remove
		public Sprite GetAtlasSprite() {
			return(atlasSprite);
		}

		// Remove
		public void SetAtlasSprite(Sprite sprite) {
			atlasSprite = sprite;
		}

		public bool NotInRange(Vector2 pos, float sourceSize) {
			return(Vector2.Distance(pos, Vector2.zero) > sourceSize + worldRadius);
		}

		public Vector2 GetWorldPosition() {
			if (worldPosition == null) {
				worldPosition = new Vector2();
			}
			
			return(worldPosition.Value);
		}

		public void ResetWorld() {
			worldPolygons = null;
			worldPosition = null;
		}

		// Remove
		public Vector2 GetWorldPosition(LightingTilemapCollider.Base tilemap) {
			if (worldPosition == null) {
				worldRotation = tilemap.transform.eulerAngles.z;

				TilemapProperties properties = tilemap.Properties;

				switch(tilemap.TilemapType()) {
					case MapType.UnityRectangle:
						worldPosition = Rendering.Tilemap.Rectangle.GetTilePosition(this, properties);
						worldRotation = tilemap.transform.eulerAngles.z;

					break;

					#if (SUPER_TILEMAP_EDITOR)

						case MapType.SuperTilemapEditor:
							worldPosition = SuperTilemapEditorSupport.TilemapTransform.GetTilePosition(this, properties);
							worldScale = SuperTilemapEditorSupport.TilemapTransform.GetScale(properties);
						break;

					#endif
				}
			}
			
			return(worldPosition.Value);
		}

		public void UpdatePosition(LightingTilemapCollider.Base tilemap) {
			worldRotation = tilemap.transform.eulerAngles.z;

			TilemapProperties properties = tilemap.Properties;

			switch(tilemap.TilemapType()) {
				#if (SUPER_TILEMAP_EDITOR)
					case MapType.SuperTilemapEditor:
						worldPosition = SuperTilemapEditorSupport.TilemapTransform.GetTilePosition(this, properties);
						worldScale = SuperTilemapEditorSupport.TilemapTransform.GetScale(properties);

					break;
				#endif

				case MapType.UnityRectangle:
					worldPosition = Rendering.Tilemap.Rectangle.GetTilePosition(this, properties);
					worldScale = Rendering.Tilemap.Rectangle.GetScale(properties, true);

				break;

				case MapType.UnityIsometric:
					worldPosition = Rendering.Tilemap.Isometric.GetTilePosition(this, properties);
					worldScale = Rendering.Tilemap.Isometric.GetScale(properties);

				break;

				case MapType.UnityHexagon:
					worldPosition = Rendering.Tilemap.Hexagon.GetTilePosition(this, properties);
					worldScale = Rendering.Tilemap.Hexagon.GetScale(properties);

				break;
			}
		}

		public List<Polygon2D> GetWorldPolygons(LightingTilemapCollider.Base tilemap) {
			if (worldPolygons == null) {
				List<Polygon2D> localPolygons = GetLocalPolygons(tilemap);
				if (worldPolygonsCache == null) {

					worldPolygons = new List<Polygon2D>();
					worldPolygonsCache = worldPolygons;
					
					UpdatePosition(tilemap);
	
					foreach(Polygon2D polygon in localPolygons) {
						Polygon2D worldPolygon = polygon.Copy();

						worldPolygon.ToScaleItself(worldScale);
						worldPolygon.ToRotationItself(tilemap.transform.eulerAngles.z * Mathf.Deg2Rad);
						worldPolygon.ToOffsetItself(worldPosition.Value);

						worldPolygons.Add(worldPolygon);
					}
					
				} else {
					worldPolygons = worldPolygonsCache;

					UpdatePosition(tilemap);

					for(int i = 0; i < localPolygons.Count; i++) {
						Polygon2D polygon = localPolygons[i];
						Polygon2D worldPolygon = worldPolygons[i];

						for(int j = 0; j < polygon.pointsList.Count; j++) {
							worldPolygon.pointsList[j].x = polygon.pointsList[j].x;
							worldPolygon.pointsList[j].y = polygon.pointsList[j].y;
						}

						worldPolygon.ToScaleItself(worldScale);
						worldPolygon.ToRotationItself(tilemap.transform.eulerAngles.z * Mathf.Deg2Rad);
						worldPolygon.ToOffsetItself(worldPosition.Value);
					}

						
				}
			}

			return(worldPolygons);
		}

		public List<Polygon2D> GetLocalPolygons(LightingTilemapCollider.Base tilemap) {
			if (localPolygons == null) {

				if (tilemap.IsCustomPhysicsShape()) {
				
					if (GetCustomPhysicsShapePolygons().Count > 0) {
	
						localPolygons = GetCustomPhysicsShapePolygons(); //poly.ToScaleItself(defaultSize); // scale?
					} else {
						localPolygons = new List<Polygon2D>();
					}

					
				} else {
					localPolygons = new List<Polygon2D>();
					Polygon2D p;

					switch(tilemap.TilemapType()) {
						case MapType.UnityRectangle:
						case MapType.SuperTilemapEditor:

							p = Polygon2D.CreateRect(Vector2.one);
							p.Normalize();

							localPolygons.Add(p);
						break;

						case MapType.UnityIsometric:

							p = Polygon2D.CreateIsometric(new Vector2(1, 0.5f));
							p.Normalize();

							localPolygons.Add(p);
						break;

						case MapType.UnityHexagon:

							p = Polygon2D.CreateHexagon(new Vector2(1, 0.5f));
							p.Normalize();

							localPolygons.Add(p);
						break;

					}
				}
			}
			return(localPolygons);
		}

		public List<Polygon2D> GetCustomPhysicsShapePolygons() {
			if (customPhysicsShapePolygons == null) {
				customPhysicsShapePolygons = new List<Polygon2D>();

				if (originalSprite == null) {
					return(customPhysicsShapePolygons);
				}

				#if UNITY_2017_4_OR_NEWER
					if (customPhysicsShape == null) {
						customPhysicsShape = CustomPhysicsShapeManager.RequesCustomShape(originalSprite);
					}

					if (customPhysicsShape != null) {
						customPhysicsShapePolygons = customPhysicsShape.Get();
					}
				#endif
			}

			return(customPhysicsShapePolygons);
		}

		public MeshObject GetTileDynamicMesh() {
			if (shapeMesh == null) {
				if (customPhysicsShapePolygons != null && customPhysicsShapePolygons.Count > 0) {
					shapeMesh = customPhysicsShape.GetMesh();
				}
			}
			return(shapeMesh);
		}








		public class Rectangle {
			public static MeshObject staticTileMesh = null;

			public static MeshObject GetStaticTileMesh(LightingTilemapCollider2D tilemap) {
				if (staticTileMesh == null) {
					// Can be optimized?
					Mesh mesh = new Mesh();

					float x = 0.5f;
					float y = 0.5f;

					mesh.vertices = new Vector3[]{new Vector2(-x, -y), new Vector2(x, -y), new Vector2(x, y), new Vector2(-x, y)};
					mesh.triangles = new int[]{0, 1, 2, 2, 3, 0};
					mesh.uv = new Vector2[]{new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1)};
			
			
					staticTileMesh = new MeshObject(mesh);	
				}
				return(staticTileMesh);
			}
		}

		public class Isometric {
			public static MeshObject staticTileMesh = null;

			public static MeshObject GetStaticTileMesh(LightingTilemapCollider2D tilemap) {
				if (staticTileMesh == null) {
					Mesh mesh = new Mesh();

					float x = 0.5f;
					float y = 0.5f;

					mesh.vertices = new Vector3[]{new Vector2(0, y), new Vector2(x, y / 2), new Vector2(0, 0), new Vector2(-x, y / 2)};
					mesh.triangles = new int[]{0, 1, 2, 2, 3, 0};
					mesh.uv = new Vector2[]{new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1)};
			
					staticTileMesh = new MeshObject(mesh);	
				}
				return(staticTileMesh);
			}
		}

		public class Hexagon {
			public static MeshObject staticTileMesh = null;

			public static MeshObject GetStaticTileMesh(LightingTilemapCollider2D tilemap) {
				if (staticTileMesh == null) {
					Mesh mesh = new Mesh();

					float x = 0.5f ;
					float y = 0.5f;

					float yOffset = - 0.25f;
					mesh.vertices = new Vector3[]{new Vector2(0, y * 1.5f + yOffset), new Vector2(x, y + yOffset), new Vector2(0, -y * 0.5f + yOffset), new Vector2(-x, y + yOffset), new Vector2(-x, 0 + yOffset), new Vector2(x, 0 + yOffset)};
					mesh.triangles = new int[]{0, 1, 5, 4, 3, 0, 0, 5, 2, 0, 2, 4};
					mesh.uv = new Vector2[]{new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1), new Vector2(0, 1),  new Vector2(0, 1) };
			
			
					staticTileMesh = new MeshObject(mesh);	
				}
				return(staticTileMesh);
			}
		}

		public class STE {
			public static MeshObject staticTileMesh = null;

			public static MeshObject GetStaticTileMesh(LightingTilemapCollider2D tilemap) {
				if (staticTileMesh == null) {
					Mesh mesh = new Mesh();

					float x = 0.5f;
					float y = 0.5f;

					mesh.vertices = new Vector3[]{new Vector2(-x, -y), new Vector2(x, -y), new Vector2(x, y), new Vector2(-x, y)};
					mesh.triangles = new int[]{0, 1, 2, 2, 3, 0};
					mesh.uv = new Vector2[]{new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1)};

					staticTileMesh = new MeshObject(mesh);	
				}

				return(staticTileMesh);
			}
		}
	}

#endif