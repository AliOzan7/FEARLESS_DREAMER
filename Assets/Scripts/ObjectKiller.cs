using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectKiller : MonoBehaviour
{
    [SerializeField] float killDelay = 3f;

    private void Start()
    {
        Invoke(nameof(KillObject), killDelay);
    }

    private void KillObject()
    {
        Destroy(gameObject);
    }
}
