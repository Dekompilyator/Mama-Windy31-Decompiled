using UnityEngine;

namespace SmartFPController
{
	public static class RaycastHitExtensions
	{
		public static string GetSurface(this RaycastHit hitInfo)
		{
			return SurfaceDetector.GetSurface(hitInfo);
		}

		public static string GetSurface(this Material meshMaterial)
		{
			return SurfaceDetector.GetSurface(meshMaterial);
		}

		public static string GetSurface(this Texture terrainTexture)
		{
			return SurfaceDetector.GetSurface(terrainTexture);
		}

		public static bool TryGetSurface(this RaycastHit hit, out string surface)
		{
			return SurfaceDetector.TryGetSurface(hit, out surface);
		}

		public static bool TryGetSurface(this Material meshMaterial, out string surface)
		{
			return SurfaceDetector.TryGetSurface(meshMaterial, out surface);
		}

		public static bool TryGetSurface(this Texture terrainTexture, out string surface)
		{
			return SurfaceDetector.TryGetSurface(terrainTexture, out surface);
		}

		public static Material GetMaterial(this RaycastHit hitInfo)
		{
			if (hitInfo.collider == null || hitInfo.collider.isTrigger || hitInfo.collider is TerrainCollider)
			{
				return null;
			}
			Renderer component = hitInfo.collider.GetComponent<Renderer>();
			if (component == null)
			{
				return null;
			}
			MeshCollider meshCollider = hitInfo.collider as MeshCollider;
			if (meshCollider == null || meshCollider.convex)
			{
				return component.sharedMaterial;
			}
			Mesh sharedMesh = meshCollider.sharedMesh;
			int num = hitInfo.triangleIndex * 3;
			for (int i = 0; i < sharedMesh.subMeshCount; i++)
			{
				int num2 = sharedMesh.GetTriangles(i).Length;
				if (num < num2)
				{
					return component.sharedMaterials[i];
				}
				num -= num2;
			}
			return null;
		}

		public static Texture GetTerrainTexture(this RaycastHit hitInfo)
		{
			if (hitInfo.collider == null || hitInfo.collider.isTrigger)
			{
				return null;
			}
			TerrainCollider terrainCollider = hitInfo.collider as TerrainCollider;
			if (terrainCollider == null || terrainCollider.terrainData == null)
			{
				return null;
			}
			TerrainData terrainData = terrainCollider.terrainData;
			Vector3 position = hitInfo.transform.position;
			int x = Mathf.RoundToInt((hitInfo.point.x - position.x) / terrainData.size.x * (float)terrainData.alphamapWidth);
			int y = Mathf.RoundToInt((hitInfo.point.z - position.z) / terrainData.size.z * (float)terrainData.alphamapHeight);
			float[,,] alphamaps = terrainData.GetAlphamaps(x, y, 1, 1);
			SplatPrototype[] splatPrototypes = terrainData.splatPrototypes;
			for (int i = 0; i < splatPrototypes.Length; i++)
			{
				if (alphamaps[0, 0, i] > 0.5f)
				{
					return splatPrototypes[i].texture;
				}
			}
			return null;
		}

		public static bool TryGetMaterial(this RaycastHit hitInfo, out Material meshMaterial)
		{
			meshMaterial = hitInfo.GetMaterial();
			return meshMaterial != null;
		}

		public static bool TryGetTerrainTexture(this RaycastHit hitInfo, out Texture terrainTexture)
		{
			terrainTexture = hitInfo.GetTerrainTexture();
			return terrainTexture != null;
		}
	}
}
