//======= Copyright (c) Viet Kien Nguyen, All rights reserved. ===============
//
// Purpose: The Gun
//
//=============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Valve.VR.InteractionSystem;
using Valve.VR;


//-------------------------------------------------------------------------------------------------
[RequireComponent(typeof(Interactable))]
public class Gun : MonoBehaviour
{
    public enum Handedness { Left, Right };
    public Handedness currentHandGuess = Handedness.Right;
    private Hand hand;
    private HandItemManager handItemManager;

    [Header("GUI Setup")]
    Transform guiTransform;
    [SerializeField] private GunGUI gunGUI;
    [SerializeField] private SevenZonesGUI sevenZonesGUI;

    [Header("Gameplay Setup")]
    private int currentMagSize;
    private float fireReset = 1f;
    private bool isFiring = false;
    [SerializeField] private ParticleFX muzzleFlash, caseEject;
    [SerializeField] private ushort hapticFeedbackStrength = 2000;
    [SerializeField] private int defaultMagSize = 17;  // glock 17
    [SerializeField] private float muzzleVelocity = 1200f;
    [SerializeField] private bool autoFire = false;
    [SerializeField] private float autoFireRate = 20; // shot per second (glock 17)

    [Header("Functionality Setup")]
    private bool caseEjected = false;
    private bool magEjected = false;
    private float recoilLerp = 0;
    private float recoilVal = 0;

    [SerializeField] private GameObject loadedMagazine, ejectedMagazine;
    [SerializeField] private Transform muzzleTransform, cartridgeEjectTransform;
    [SerializeField] private Transform triggerTransform, triggerRestTransform, triggerActionTransform;
    [SerializeField] private Transform slideTransform, slideFeecbackTransform, slideOriginalTransform;
    [SerializeField] private Transform modelTransform, recoilTransform, recoverTransform;
    [SerializeField] private float sliderSpeed = 1f;
    [SerializeField] private bool slideForwardWhenEmpty = false;
    [SerializeField] private bool useRecoilAnimation = false;
    [SerializeField] private float recoilStrength = 0.4f;
    [SerializeField] private float recoverSpeed = 2f;
    [SerializeField] private bool autoEjectEmptyMag = false;

    [SerializeField] private GunAmmoBullet bullet;
    [SerializeField] private GunAmmoCartridge bulletCase;
    [SerializeField] private GunAttachmentLaserSight laserSight;
    [SerializeField] private bool toggleLaserSight = true;

    [SerializeField] private bool autoSpawnMagazineHand = true;
    [SerializeField] private ItemPackage magazineHandItemPackage;
    [SerializeField] private GameObject magazineHandPrefab;

    [SerializeField] private SoundPlayOneshot shotSound;
    // don't play audio oneshot! will cause performance loss, use audio.Play() instead
    //public SoundPlayOneshot slideOpenSound;
    //public SoundPlayOneshot slideCloseSound;

    private SteamVR_Events.Action newPosesAppliedAction;

    bool MagazineIsEmpty()
    {
        if (currentMagSize > 0)
        { return false; }
        else
        { return true; }
    }


    //-------------------------------------------------------------------------------------------------
    private void OnAttachedToHand(Hand attachedHand)
    {
        hand = attachedHand;

        if (sevenZonesGUI && hand)
            sevenZonesGUI.activeHand = hand;
    }
    //-------------------------------------------------------------------------------------------------
    void Awake()
    {
        newPosesAppliedAction = SteamVR_Events.NewPosesAppliedAction(OnNewPosesApplied);
    }
    //-------------------------------------------------------------------------------------------------
    private void Start()
    {
        handItemManager = hand.GetComponent<HandItemManager>();
        currentMagSize = defaultMagSize;
    }
    //-------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        newPosesAppliedAction.enabled = true;
    }
    //-------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        newPosesAppliedAction.enabled = false;
    }
    //-------------------------------------------------------------------------------------------------
    void LateUpdate()
    {

    }
    //-------------------------------------------------------------------------------------------------
    private void OnNewPosesApplied()
    {

    }
    //-------------------------------------------------------------------------------------------------
    private void UpdateTriggerRotation()
    {
        float axisValue = hand.controller.GetAxis(EVRButtonId.k_EButton_SteamVR_Trigger).x;

        triggerTransform.localRotation = Quaternion.Lerp(triggerRestTransform.localRotation, triggerActionTransform.localRotation, axisValue);
    }
    //-------------------------------------------------------------------------------------------------
    private void HandAttachedUpdate(Hand hand)
    {
        if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            // check if mag is empty
            // if not check if new mag is loaded 
            // if new mag loaded DO reset slide and set mag to full
            hand.controller.TriggerHapticPulse(hapticFeedbackStrength);

            // chamber round and move slide to firing position
            if (!magEjected)
            {
                EjectMagazine();
                currentMagSize = 0;
                magEjected = true;
            }
            else
            {
                ReloadMagazine();
                magEjected = false;
            }
        }

        if (autoFire)  // autofire when trigger is held down
        {
            fireReset -= autoFireRate * Time.deltaTime;
            if (hand.controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (!isFiring && !MagazineIsEmpty() && fireReset < 0)
                {
                    isFiring = true;
                    Shoot();
                    fireReset = 1;
                    Recoil(recoilStrength);
                }
            }
        }
        else  // pull the trigger to fire single shot
        {
            if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (!isFiring && !MagazineIsEmpty())
                {
                    isFiring = true;
                    Shoot();
                    Recoil(recoilStrength);
                }
            }
        }

        if (sevenZonesGUI.BotLeftBtnPressed())
        {
            toggleLaserSight = !toggleLaserSight;
        }

        if (sevenZonesGUI.BotRightBtnPressed())
        {
            ToggleAutoFire();
        }

        if (sevenZonesGUI.TopRightBtnPressed())
        {
            handItemManager.SwitchToNextGun(hand);
        }

        if (sevenZonesGUI.TopLeftBtnPressed())
        {
            handItemManager.SwitchToPrevGun(hand);
        }

        if (sevenZonesGUI.BotMidBtnPressed())
        {
            handItemManager.EmptyGunHand(hand);
        }
        AutoEjectMag();
        RecoverFromRecoil(recoverSpeed);
        CheckLaserSight();
        UpdateTriggerRotation();
        SlideFeedback(sliderSpeed);
        UpdateGUI();
        CastDroneReaction();
    }
    //-------------------------------------------------------------------------------------------------
    private void CastDroneReaction()
    {
        RaycastHit hit;
        if (Physics.Raycast(muzzleTransform.position, muzzleTransform.forward, out hit))
        {
            if (hit.transform.gameObject.GetComponent<EnemyDrone>())
            {
                hit.transform.gameObject.GetComponent<EnemyDrone>().IsAimedUpon = true;
            }
        }
    }
    //-------------------------------------------------------------------------------------------------
    private void AutoEjectMag()
    {
        if (autoEjectEmptyMag)
        {
            if (MagazineIsEmpty())
            {
                if (!magEjected)
                {
                    EjectMagazine();
                    magEjected = true;
                }
            }
        }
    }
    //-------------------------------------------------------------------------------------------------
    private void Recoil(float strength)
    {
        if (useRecoilAnimation)
        {
            recoilLerp += strength;
            recoilLerp = Mathf.Clamp(recoilLerp, 0, 1);
            modelTransform.localPosition = Vector3.Lerp(recoverTransform.localPosition, recoilTransform.localPosition, recoilLerp);
        }
    }
    //-------------------------------------------------------------------------------------------------
    private void RecoverFromRecoil(float strength)
    {
        if (useRecoilAnimation)
        {
            recoilLerp -= Time.deltaTime * strength;
            recoilLerp = Mathf.Clamp(recoilLerp, 0, 1);
            modelTransform.localPosition = Vector3.Lerp(recoverTransform.localPosition, recoilTransform.localPosition, recoilLerp);
        }
    }
    //-------------------------------------------------------------------------------------------------
    private void CheckLaserSight()
    {
        laserSight.gameObject.SetActive(toggleLaserSight);
    }
    //-------------------------------------------------------------------------------------------------
    private void ToggleAutoFire()
    {
        autoFire = !autoFire;
    }
    private void Shoot()
    {
        bullet.ShootFrom(muzzleTransform, muzzleVelocity);
        hand.controller.TriggerHapticPulse(hapticFeedbackStrength);
        SpawnFX(muzzleFlash, 1f);
        shotSound.Play();
        currentMagSize--;
    }
    //-------------------------------------------------------------------------------------------------
    void SpawnFX(ParticleFX particleFX, float lifeTime)
    {
        ParticleFX newFX = Instantiate(particleFX);
        newFX.transform.SetParent(particleFX.transform.parent);
        newFX.transform.position = particleFX.transform.position;
        newFX.transform.rotation = particleFX.transform.rotation;
        newFX.transform.localScale = particleFX.transform.localScale;
        newFX.LifeTime = lifeTime;
        newFX.gameObject.SetActive(true);
        newFX.GetComponent<ParticleSystem>().Play();
    }
    //-------------------------------------------------------------------------------------------------
    void EjectMagazine()
    {
        // hide loaded mag
        loadedMagazine.SetActive(false);

        // spawn ejected mag
        GameObject ejectedMag = Instantiate(ejectedMagazine, ejectedMagazine.transform.parent);
        ejectedMag.transform.localScale = ejectedMagazine.transform.transform.localScale;
        ejectedMag.transform.localPosition = ejectedMagazine.transform.transform.localPosition;
        ejectedMag.transform.localRotation = ejectedMagazine.transform.localRotation;
        ejectedMag.transform.SetParent(null);
        ejectedMag.SetActive(true);
        ejectedMag.GetComponent<Rigidbody>().velocity = ejectedMagazine.transform.forward * -2f;
        Destroy(ejectedMag.gameObject, 10f);
    }
    //-------------------------------------------------------------------------------------------------
    void ReloadMagazine()
    {
        // show loaded mag
        loadedMagazine.SetActive(true);
        currentMagSize = defaultMagSize;
    }
    //-------------------------------------------------------------------------------------------------
    void UpdateGUI()
    {
        gunGUI.remainingBulletCount = currentMagSize;
        gunGUI.autoFire = autoFire;
    }
    //-------------------------------------------------------------------------------------------------
    void SlideFeedback(float speed)
    {
        float slideForwardRate = speed * Time.deltaTime;
        //float slideBackwardRate = Time.deltaTime / speed;
        //Vector3 velocity = Vector3.zero;

        // TODO make sure that firing is only possible if new round is chambered

        //if (slide.localPosition == slideOriginalTransform.localPosition && caseEjected)
        //{
        //    isFiring = false;
        //}

        // move to position to eject case
        if (slideTransform.localPosition == slideFeecbackTransform.localPosition)
        {
            // position reached
            if (MagazineIsEmpty()) // check if mag is empty
            {
                // if empty, eject and dont move back
                EjectCase();
                caseEjected = true;
                if (slideForwardWhenEmpty)
                {
                    isFiring = false;
                }
            }
            else // else eject and move back
            {
                EjectCase();
                caseEjected = true;
                isFiring = false;
            }
        }

        if (isFiring)
        {
            slideTransform.localPosition = Vector3.MoveTowards(slideTransform.localPosition, slideFeecbackTransform.localPosition, slideForwardRate);
            //slide.localPosition = Vector3.SmoothDamp(slide.localPosition, slideFeecbackTransform.localPosition, ref velocity, slideBackwardRate);
            //slideOpenSound.Play();
        }
        else
        {
            // get back into position to be ready to fire again 
            slideTransform.localPosition = Vector3.MoveTowards(slideTransform.localPosition, slideOriginalTransform.localPosition, slideForwardRate);
            caseEjected = false;
            //slideCloseSound.Play();
        }
    }
    //-------------------------------------------------------------------------------------------------
    private void EjectCase()
    {
        if (!caseEjected)
        {
            bulletCase.EjectFrom(cartridgeEjectTransform, Random.Range(2f, 4f));
            SpawnFX(caseEject, 1f);
        }
    }
    //-------------------------------------------------------------------------------------------------
    private void ShutDown()
    {
        if (hand != null && hand.otherHand.currentAttachedObject != null)
        {
            if (hand.otherHand.currentAttachedObject.GetComponent<ItemPackageReference>() != null)
            {
                if (hand.otherHand.currentAttachedObject.GetComponent<ItemPackageReference>().itemPackage == magazineHandItemPackage)
                {
                    hand.otherHand.DetachObject(hand.otherHand.currentAttachedObject);
                }
            }
        }
    }
    //-------------------------------------------------------------------------------------------------
    private void OnHandFocusLost(Hand hand)
    {
        gameObject.SetActive(false);
    }
    //-------------------------------------------------------------------------------------------------
    private void OnHandFocusAcquired(Hand hand)
    {
        gameObject.SetActive(true);
        OnAttachedToHand(hand);
    }
    //-------------------------------------------------------------------------------------------------
    private void OnDetachedFromHand(Hand hand)
    {
        Destroy(gameObject);
    }
    //-------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        ShutDown();
    }
}
