using System.Collections.Generic;
using UnityEngine;

namespace absurdjoy
{
	public class BonesToSkinnedMesh : MonoBehaviour
	{
		public Transform boneRoot;
		public OVRPlugin.MeshType meshType;

		// Recreate hand structure to replicate Oculus
		// This orders all the bones within the list, setting the finger tips last.
		public void OnEnable()
		{
			SetupSkinnedMesh();
		}

		private void SetupSkinnedMesh()
		{
			GenerateMesh();

			var listOfChildren = AddRecursiveChildren(boneRoot);
			
			List<Transform> allBones = new List<Transform>();

			// We need bones to be in the same order as oculus
			// So we add all the bones and keep a reference to 5 finger tips. (OVRSkeleton sets these bone id's last)
			// We then add finger tips back to bones so they are last.
			List<Transform> fingerTips = new List<Transform>();
			foreach (var bone in listOfChildren)
			{
				if (bone.name.Contains("Tip"))
				{
					fingerTips.Add(bone); //Keep reference to finger tips
				}
				else
				{
					allBones.Add(bone);
				}
			}

			// Add finger tips back to bones
			foreach (var bone in fingerTips)
			{
				allBones.Add(bone);
			}

			int numSkinnableBones = allBones.Count;
			var bindPoses = new Matrix4x4[numSkinnableBones];
			var localToWorldMatrix = boneRoot.localToWorldMatrix;
			for (int i = 0; i < numSkinnableBones; ++i)
			{
				bindPoses[i] = allBones[i].worldToLocalMatrix * localToWorldMatrix;
			}

			var skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
			skinnedMeshRenderer.bones = allBones.ToArray();
			skinnedMeshRenderer.sharedMesh.bindposes = bindPoses;
			skinnedMeshRenderer.enabled = true;
		}

		private void GenerateMesh()
		{
			var ovrpMesh = new OVRPlugin.Mesh();
			if (OVRPlugin.GetMesh(meshType, out ovrpMesh))
			{
				var vertices = new Vector3[ovrpMesh.NumVertices];
				for (int i = 0; i < ovrpMesh.NumVertices; ++i)
				{
					vertices[i] = ovrpMesh.VertexPositions[i].FromFlippedXVector3f();
				}

				var mesh = new Mesh();
				mesh.vertices = vertices;

				var uv = new Vector2[ovrpMesh.NumVertices];
				for (int i = 0; i < ovrpMesh.NumVertices; ++i)
				{
					uv[i] = new Vector2(ovrpMesh.VertexUV0[i].x, -ovrpMesh.VertexUV0[i].y);
				}

				mesh.uv = uv;

				var triangles = new int[ovrpMesh.NumIndices];
				for (int i = 0; i < ovrpMesh.NumIndices; ++i)
				{
					triangles[i] = ovrpMesh.Indices[ovrpMesh.NumIndices - i - 1];
				}

				mesh.triangles = triangles;

				var normals = new Vector3[ovrpMesh.NumVertices];
				for (int i = 0; i < ovrpMesh.NumVertices; ++i)
				{
					normals[i] = ovrpMesh.VertexNormals[i].FromFlippedXVector3f();
				}

				mesh.normals = normals;

				var boneWeights = new BoneWeight[ovrpMesh.NumVertices];
				for (int i = 0; i < ovrpMesh.NumVertices; ++i)
				{
					var currentBlendWeight = ovrpMesh.BlendWeights[i];
					var currentBlendIndices = ovrpMesh.BlendIndices[i];

					boneWeights[i].boneIndex0 = (int) currentBlendIndices.x;
					boneWeights[i].weight0 = currentBlendWeight.x;
					boneWeights[i].boneIndex1 = (int) currentBlendIndices.y;
					boneWeights[i].weight1 = currentBlendWeight.y;
					boneWeights[i].boneIndex2 = (int) currentBlendIndices.z;
					boneWeights[i].weight2 = currentBlendWeight.z;
					boneWeights[i].boneIndex3 = (int) currentBlendIndices.w;
					boneWeights[i].weight3 = currentBlendWeight.w;
				}

				mesh.boneWeights = boneWeights;

				GetComponent<MeshFilter>().mesh = mesh;
				GetComponent<SkinnedMeshRenderer>().sharedMesh = mesh;
			}
			else
			{
				Debug.LogError("Failed to generate mesh");
			}
		}

		private List<Transform> AddRecursiveChildren(Transform obj, List<Transform> set = null)
		{
			if (set == null)
			{
				set = new List<Transform>();
			}

			if (obj == null)
			{
				return set;
			}

			for (int i = 0; i < obj.childCount; i++)
			{
				var child = obj.GetChild(i);
				if (child == null)
				{
					continue;
				}

				if (child != obj)
				{
					set.Add(child);
				}

				AddRecursiveChildren(child, set);
			}

			return set;
		}
	}
}