using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour {

    private void OnTriggerEnter(Collider _otherCollider)
    {
        if (_otherCollider.tag == Tags.Obstacle)
        {
            Destroy(gameObject);
            CoroutineHelper.Delay(60, () =>
            {
                SceneManager.LoadScene(0);
            });
        }
        
    }
}