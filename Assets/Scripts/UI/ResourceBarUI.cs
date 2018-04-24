using UnityEngine;

public class ResourceBarUI : MonoBehaviour
{

    private Vector3 startScale;

    private void Update()
    {
        transform.localScale = new Vector3(startScale.x * ResourceValue.Instance.Value, startScale.y, startScale.z);
        
    }

    private void Awake()
    {
        startScale = transform.localScale;
    }

}
