using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ChangeMaterial : MonoBehaviour
{
    private Renderer _myRenderer;

    public Material InactiveMaterial;
    public Material GazedAtMaterial;

    void Start()
    {
        _myRenderer = GetComponent<Renderer>();
        SetGazedAt(false);
    }

    public void SetGazedAt(bool gazedAt)
    {
        if (InactiveMaterial != null && GazedAtMaterial != null)
        {
            _myRenderer.material = gazedAt ? GazedAtMaterial : InactiveMaterial;
        }
    }
}