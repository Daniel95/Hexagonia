using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class SetRendererEnabledOnAwake : MonoBehaviour {

    [SerializeField] private new bool enabled = false;

    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.enabled = enabled;
    }

}
