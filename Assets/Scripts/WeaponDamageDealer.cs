using EnemySystem.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamageDealer : MonoBehaviour
{
    [SerializeField] float baseDamage = 20;
    [SerializeField] float relativeVelocityLowerBound = 3f;
    [SerializeField] float relativeVelocityUpperBound = 25;
    [SerializeField] GameObject weaponHitEffect;

    private Rigidbody weaponRB;

    private void Start()
    {
        weaponRB = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameObject.Instantiate(weaponHitEffect, collision.transform.position, collision.transform.rotation);
            SoundEffectsManager.Instance.PlayElectricHit();

            float relativeVelocity = (weaponRB.velocity - collision.gameObject.GetComponent<Rigidbody>().velocity).magnitude;
            //Debug.Log("relative hit velocity: " + relativeVelocity);
            int damageToDeal = (int)Mathf.Lerp(baseDamage, baseDamage * 2f, (relativeVelocity - relativeVelocityLowerBound) / relativeVelocityUpperBound);
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(damageToDeal);
        }
    }
}
