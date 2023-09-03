using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class WeaponThrower : MonoBehaviour
{
    [SerializeField] Rigidbody weaponRB;
    [SerializeField] BoxCollider weaponCollider;
    [SerializeField] Animator characterAnimator;
    [SerializeField] GameObject weaponParticleEffects;
    [SerializeField] Transform weaponParent;
    [SerializeField] Transform weaponAimPoint, weaponHoldingPoint, rightBezierCurvePoint, leftBezierCurvePoint;
    [SerializeField] float maxThrowForce = 200f;
    [SerializeField] float minThrowForce = 50f;
    [SerializeField] float maxMeleeForce = 200f;
    [SerializeField] float minMeleeForce = 50f;
    [SerializeField] float weaponReturnTime = 1f;
    [SerializeField] float weaponChargeTime = 2f;
    [SerializeField] float upwardOffsetFactor = 0.1f;

    private float throwForce = 0f;
    private float meleeForce = 0f;
    private Vector3 weaponThrowDirection = new Vector3(0, 0, 0);
    private Vector3 weaponReturnStartPoint = new();
    private bool isThrown = false;
    private bool isReturning = false;
    private bool isThrowCharged = false;
    private bool isMeleeCharged = false;
    private float time = 0f;
    private ThirdPersonCameraController thirdPersonCameraController;
    private Animator playerAnimator;
    private Quaternion restingRotation;

    private void Start()
    {
        thirdPersonCameraController = GetComponent<ThirdPersonCameraController>();
        playerAnimator = GetComponent<Animator>();
        restingRotation = weaponRB.transform.localRotation;
    }


    void Update()
    {
        //if (GameManager.Instance.State != GameState.PLAY) { return; }
        if (Input.GetMouseButtonDown(1) && !isThrown && !isReturning)
        {
            StartCoroutine(ChargeThrow());
        }

        if (Input.GetMouseButtonUp(1) && !isThrown && !isReturning)
        {
            playerAnimator.SetTrigger("ThrowCanceled");
            weaponParticleEffects.SetActive(false);
            throwForce = minThrowForce;
            isThrowCharged = false;
            SoundEffectsManager.Instance.ClearSounds();
        }

        if (Input.GetMouseButtonDown(0) && isThrowCharged)
        {
            StartCoroutine(ThrowWeapon());
        }

        if (Input.GetMouseButtonDown(0) && !isThrown && !isThrowCharged)
        {
            StartCoroutine(ChargeMelee());
        }

        if (Input.GetMouseButtonUp(0) && !isThrown && !isThrowCharged)
        {
            playerAnimator.SetTrigger("Slash");
            SoundEffectsManager.Instance.ClearSounds();
            SoundEffectsManager.Instance.PlaySlashingWithWeapon();
            isMeleeCharged = false;
            weaponCollider.enabled = true;
        }

        if (Input.GetMouseButtonDown(1) && !isReturning && isThrown) 
        {
            StartCoroutine(ReturnHammer());
        }
    }

    private IEnumerator ChargeThrow()
    {
        float time = 0f;
        playerAnimator.SetTrigger("GetReadyToThrow");
        isThrowCharged = true;
        weaponParticleEffects.SetActive(true);
        SoundEffectsManager.Instance.PlayHoldingTheChargedWeapon();

        while (time < weaponChargeTime)
        {
            throwForce = Mathf.Lerp(minThrowForce, maxThrowForce, time / weaponChargeTime);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator ChargeMelee()
    {
        float time = 0f;
        playerAnimator.SetTrigger("GetReadyToSlash");
        isMeleeCharged = true;
        weaponParticleEffects.SetActive(true);
        SoundEffectsManager.Instance.PlayHoldingTheChargedWeapon();

        while (time < weaponChargeTime)
        {
            meleeForce = Mathf.Lerp(minMeleeForce, maxMeleeForce, time / weaponChargeTime);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator ThrowWeapon()
    {
        yield return new WaitForSeconds(0.25f);
        SoundEffectsManager.Instance.ClearSounds();
        playerAnimator.SetTrigger("Throw");
        SoundEffectsManager.Instance.PlayThrowingWeapon();
        isThrown = true;
        isThrowCharged = false;
        weaponRB.transform.parent = null; //to let it loose and avoid movement with the player's movement.
        weaponRB.isKinematic = false;
        weaponCollider.enabled = true;
        Vector3 targetPointWithUpwardOffset = thirdPersonCameraController.GetTargetTransform().position + Vector3.up * upwardOffsetFactor * (thirdPersonCameraController.GetTargetTransform().position - transform.position).magnitude;
        weaponThrowDirection = (targetPointWithUpwardOffset - weaponRB.position).normalized;
        weaponRB.AddForce(weaponThrowDirection * throwForce, ForceMode.Impulse);
        
        float time = 0f;
        while (time < 0.4f)
        {
            weaponRB.rotation = Quaternion.Slerp(weaponRB.transform.rotation, weaponAimPoint.rotation, time / 0.4f);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("THROWN");
        yield return null;
    }

    private IEnumerator ReturnHammer()
    {
        playerAnimator.SetTrigger("Recall");
        isReturning = true;
        weaponRB.velocity = Vector3.zero;
        weaponRB.isKinematic = true;
        weaponCollider.enabled = false;
        weaponReturnStartPoint = weaponRB.position;
        //Debug.Log("weapon return start point = "  + weaponReturnStartPoint);
        //Debug.Log("right bezier point = "  + rightBezierCurvePoint.position);
        while (time < weaponReturnTime + 0.025f)
        {
            // Returning calculations with bezier quadratic formula.
            weaponRB.position = GetBezierQuadraticCurvePoint(time / weaponReturnTime, weaponReturnStartPoint,
                                                                rightBezierCurvePoint.position,
                                                                weaponHoldingPoint.position);
            weaponRB.rotation = Quaternion.Slerp(weaponRB.transform.rotation, weaponHoldingPoint.rotation, time / weaponReturnTime);
            time += Time.deltaTime;
            //Debug.Log(" time = " + time.ToString());
            yield return new WaitForEndOfFrame();
        }
        ResetHammer();
        yield return null;
    }

    private void ResetHammer()
    {
        time = 0f;
        isThrown = false;
        isReturning = false;
        //weaponRB.position = weaponHoldingPoint.position;
        //weaponRB.rotation = weaponHoldingPoint.rotation;
        //weaponRB.transform.parent = weaponParent;

        weaponRB.transform.parent = weaponParent;
        weaponRB.transform.localPosition = Vector3.zero;
        weaponRB.transform.localRotation = restingRotation;
        weaponCollider.enabled = false;
        //Debug.Log("RESETTED");
        weaponParticleEffects.SetActive(false);
    }

    private Vector3 GetBezierQuadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        //formula reference = https://en.wikipedia.org/wiki/B%C3%A9zier_curve

        float u = 1 - t;
        Vector3 intermediatePoint = (u * u * p0) + (2 * u * t * p1) + (t * t * p2);
        return intermediatePoint;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider.CompareTag("Enemy"))
        {
            //Debug.Log("ENEMY HIT");
        }
    }
}
