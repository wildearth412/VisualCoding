using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stairs
{
    [RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
    public class Stairs : MonoBehaviour
    {
        public int stairsId;
        public Vector3[] verticesOrigin = new Vector3[112];
        public float amplitude = 1.0f;

        private Vector3[] vertices = new Vector3[112];
        private Mesh mesh;


        private void Awake()
        {
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "Procedural Stairs";
        }

        private void Update()
        {
            Vector3 vec = new Vector3(0, 0, 0.06f * Mathf.Sin(0.15f * (float)Time.frameCount + (float)stairsId * 0.41887f));    // create Vertexs
            //Debug.Log("                    " + vec);
            for (var i = 0; i < 8; i++)
            {
                this.vertices[i] = this.verticesOrigin[8 * stairsId + i] + vec * amplitude;
            }

            Vector2[] uv = new Vector2[vertices.Length];                      // create UV
            for (var i = 0; i < 8; i++)
            {
                uv[i] = new Vector2(0.25f * (float)(i % 4), 0.5f * (float)(i / 4));
            }

            Vector4[] tangents = new Vector4[vertices.Length];                // create Tangents
            Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
            for (var i = 0; i < 8; i++)
            {
                tangents[i] = tangent;
            }

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.tangents = tangents;

            int[] triangles = new int[36];                     // create triangles
            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;
            triangles[6] = 0;
            triangles[7] = 5;
            triangles[8] = 1;
            triangles[9] = 0;
            triangles[10] = 4;
            triangles[11] = 5;
            triangles[12] = 1;
            triangles[13] = 5;
            triangles[14] = 6;
            triangles[15] = 1;
            triangles[16] = 6;
            triangles[17] = 2;
            triangles[18] = 5;
            triangles[19] = 7;
            triangles[20] = 6;
            triangles[21] = 5;
            triangles[22] = 4;
            triangles[23] = 7;
            triangles[24] = 2;
            triangles[25] = 6;
            triangles[26] = 7;
            triangles[27] = 2;
            triangles[28] = 7;
            triangles[29] = 3;
            triangles[30] = 3;
            triangles[31] = 7;
            triangles[32] = 4;
            triangles[33] = 3;
            triangles[34] = 4;
            triangles[35] = 0;

            mesh.triangles = triangles;

            mesh.RecalculateNormals();                             // create Normals
        }
    }
}
