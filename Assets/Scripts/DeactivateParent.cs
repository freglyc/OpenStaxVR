using UnityEngine;

public class DeactivateParent : MonoBehaviour {

	// Use this for initialization
	public void Deactivate() {
		Transform next = GetNextIdx(transform.parent);
		if (!next.gameObject.activeSelf)
		{
			transform.parent.gameObject.SetActive(false);
		}

	}

	private Transform GetNextIdx(Transform current)
	{
		int idx = current.GetSiblingIndex() == 20 ? 0 : current.GetSiblingIndex() + 1;
		
		return current.parent.GetChild(idx);

	}

}
