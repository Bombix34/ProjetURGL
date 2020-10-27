using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Day.WithoutAtlas {

    public class Shadow {

        static public void Draw(DayLightingCollider2D id, Vector2 position, float z) {
            if (id.mainShape.shadowType == DayLightingCollider2D.ShadowType.None) {
                return;
            }

            if (id.mainShape.shadowType == DayLightingCollider2D.ShadowType.Sprite) {
                return;
            }

            if (id.mainShape.height < 0) {
                return;
            }
        
            if (id.InAnyCamera() == false) {
                return;
            }

            GL.Color(Color.black);

            foreach(DayLightingColliderShape shape in id.shapes) {
                DayLighting.ShadowMesh shadow = shape.shadowMesh;
                
                if (shadow == null) {
                    continue;
                }
                
                Vector3 pos = new Vector3(shape.transform2D.position.x + position.x, shape.transform2D.position.y + position.y, z);
                Matrix4x4 matrix = Matrix4x4.TRS(pos, Quaternion.Euler(0, 0, 0), Vector3.one);

                foreach(MeshObject mesh in shadow.softMeshes) {
                    //Graphics.DrawMeshNow(mesh, matrix);
                    GLExtended.DrawMeshPass(mesh, pos, Vector3.one, 0);
                }

                foreach(MeshObject mesh in shadow.meshes) {
                    //Graphics.DrawMeshNow(mesh, matrix);
                    GLExtended.DrawMeshPass(mesh, pos, Vector3.one, 0);
                } 
            }

            GL.Color(Color.white);
        }


          static public void DrawTilemap(DayLightingTilemapCollider2D id, Vector2 position, float z) {
            //if (id.mainShape.colliderType == DayLightingCollider2D.ColliderType.None) {
            //    return;
            //}

           // if (id.mainShape.colliderType == DayLightingCollider2D.ColliderType.Sprite) {
            //    return;
            //}

            //if (id.mainShape.height < 0) {
            //    return;
            //}//

            //if (id.InAnyCamera() == false) {
            //     continue;
            //}

            GL.Color(Color.black);

            foreach(DayLightingTile dayTile in id.dayTiles) {
                DayLighting.TilemapShadowMesh shadow = dayTile.shadowMesh;
                
                if (shadow == null) {
                    continue;
                }

                Vector3 pos = new Vector3(position.x, position.y, z);
                Matrix4x4 matrix = Matrix4x4.TRS(pos, Quaternion.Euler(0, 0, 0), Vector3.one);

                foreach(MeshObject mesh in shadow.softMeshes) {
                    GLExtended.DrawMeshPass(mesh, pos, Vector3.one, 0);
                }

                foreach(MeshObject mesh in shadow.meshes) {
                    GLExtended.DrawMeshPass(mesh, pos, Vector3.one, 0);
                } 
            }

            GL.Color(Color.white);
        }
    }
}