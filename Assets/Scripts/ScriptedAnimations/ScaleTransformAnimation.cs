using UnityEngine;
using System.Collections;

public class ScaleTransformAnimation : ScriptedAnimation {

    [SerializeField] private float targetSize = 1.2f;
    [SerializeField] private float speed = 1.2f;
    [SerializeField] private GameObject particlePrefab;

    protected override IEnumerator Animation() {
        while (transform.localScale.x < targetSize && transform.localScale.y < targetSize && transform.localScale.z < targetSize)
        {
            transform.localScale += new Vector3(speed, speed, speed) * Time.deltaTime;
            yield return null;
        }

        Instantiate(particlePrefab, transform.position, new Quaternion(), null);

        StopAnimation(true);
    }

    public override void StopAnimation(bool isCompleted)
    {
        transform.localScale = Vector3.one;
        base.StopAnimation(isCompleted);
    }

}