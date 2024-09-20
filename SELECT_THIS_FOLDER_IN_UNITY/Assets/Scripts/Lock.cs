using UnityEngine;

public class Lock : MonoBehaviour
{
	public GameObject door;

	public GameObject lockedText;

	public GameObject openText;

	public new string tag;

	public GameObject key;

	private Rigidbody rigid;

	private Locks doorLocks;

	private void Start()
	{
		rigid = GetComponent<Rigidbody>();
		doorLocks = door.GetComponent<Locks>();
	}

	public void removeLock()
	{
		rigid.isKinematic = false;
		doorLocks.blocks.Remove(base.gameObject);
		base.transform.parent = null;
		base.gameObject.tag = "Untagged";
	}
}
