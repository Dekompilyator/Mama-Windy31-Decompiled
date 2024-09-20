using UnityEngine;

public class KartinaScript : MonoBehaviour
{
	public Transform kraskaPos;

	public MeshRenderer mesh;

	public Material paint;

	public GameObject KraskaText;

	public GameObject PaintText;

	public bool isPaintThere;

	public bool isPainted;

	public void putPaint(Collider col)
	{
		col.transform.parent = null;
		col.transform.position = kraskaPos.position;
		col.transform.rotation = kraskaPos.rotation;
		isPaintThere = true;
	}

	public void Paint()
	{
		mesh.material = paint;
		isPainted = true;
	}
}
