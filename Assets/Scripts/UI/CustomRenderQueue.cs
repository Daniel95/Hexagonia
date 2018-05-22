using UnityEngine;
using UnityEngine.UI;
 
[ExecuteInEditMode]
public class CustomRenderQueue : MonoBehaviour {

    public UnityEngine.Rendering.CompareFunction comparison = UnityEngine.Rendering.CompareFunction.Always;

    [ContextMenu("Apply")]
    private void Apply() {
        Debug.Log("Updated material val");
        Image image = GetComponent<Image>();
        Material existingGlobalMat = image.materialForRendering;
        Material newMaterial = new Material(existingGlobalMat);
        newMaterial.SetInt("unity_GUIZTestMode", (int)comparison);
        image.material = newMaterial;
    }

}