using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] GameObject explosionSkillEffect;
    [SerializeField] float explosionRadius = 3f;
    [SerializeField] float explosionForce = 100;

    private ThirdPersonCameraController thirdPersonCameraController;
    Collider[] colliders = new Collider[20];

    private void Awake()
    {
        thirdPersonCameraController = GetComponent<ThirdPersonCameraController>();
        StarterAssetsInputs.ExplosionSkillCast += OnExplosionSkill;
    }

    private void OnDestroy()
    {
        StarterAssetsInputs.ExplosionSkillCast -= OnExplosionSkill;
    }

    private void OnExplosionSkill()
    {
        if (thirdPersonCameraController.IsTargetGround())
        {
            GameObject.Instantiate(explosionSkillEffect, thirdPersonCameraController.GetTargetTransform().position, Quaternion.identity);
            int numColliders = Physics.OverlapSphereNonAlloc(thirdPersonCameraController.GetTargetTransform().position, explosionRadius, colliders);
            if (numColliders > 0)
            {
                for (int i = 0; i < numColliders; i++)
                {
                    if (colliders[i].TryGetComponent(out Rigidbody rb))
                    {
                        rb.AddExplosionForce(explosionForce, thirdPersonCameraController.GetTargetTransform().position - Vector3.down * 0.2f, explosionRadius);
                    }
                }
            }
        }
    }
}
