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

    [SerializeField] private GunGUI handGunGUI;
    [SerializeField] private SevenZonesGUI defaultSevenZonesGUIPrefab;
    [SerializeField] private SevenZonesGUI currentSevenZonesGUI;
    [SerializeField] Transform guiTransform;

    [SerializeField] private bool slideForwardWhenEmpty = false;

    [SerializeField] private GunAmmoBullet bullet;
    [SerializeField] private GunAmmoCartridge bulletCase;

    [SerializeField] private GunAttachmentLaserSight laserSight;
    bool toggleLaserSight = true;

    [SerializeField] private Transform slide, slideFeecbackTransform, slideOriginalTransform;
    [SerializeField] private float sliderSpeed = 1f;

    [SerializeField] private Transform trigger, triggerRestTransform, triggerActionTransform;

    [SerializeField] private int defaultMagSize = 17;  // glock 17

    int currentMagSize;
    bool caseEjected = false;
    bool magEjected = false;

    [SerializeField] private float autoFireRate = 20; // shot per second (glock 17)
    [SerializeField] private bool autoFire = false;

    [SerializeField] private float muzzleVelocity = 1200f;

    [SerializeField] private ParticleFX muzzleFlash, caseEject;

    [SerializeField] private ushort hapticFeedbackStrength = 2000;

    private float fireReset = 1f;
    private bool isFiring = false;

    [SerializeField] private Transform muzzleTransform, ejectionPortTransform;

    public GameObject loadedMagazine, ejectedMagazine, loadGuide;

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

        if (currentSevenZonesGUI == null)
        {
            currentSevenZonesGUI = Instantiate(defaultSevenZonesGUIPrefab, guiTransform);
        }
        currentSevenZonesGUI.activeHand = hand;
    }
    //-------------------------------------------------------------------------------------------------
    void Awake()
    {
        newPosesAppliedAction = SteamVR_Events.NewPosesAppliedAction(OnNewPosesApplied);
    }
    //-------------------------------------------------------------------------------------------------
    private void Start()
    {
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

        trigger.localRotation = Quaternion.Lerp(triggerRestTransform.localRotation, triggerActionTransform.localRotation, axisValue);
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

            if (!MagazineIsEmpty())
            {
                autoFire = !autoFire;
            }
            else
            {
                // chamber round and move slide to firing position
                if (!magEjected)
                {
                    EjectMagazine();
                    magEjected = true;
                }
                else
                {
                    ReloadMagazine();
                    magEjected = false;
                }
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
                }
            }
        }
        EvaluateTouchPress(hand);

        UpdateTriggerRotation();
        SlideFeedback(sliderSpeed);
        UpdateGUI();
    }
    //-------------------------------------------------------------------------------------------------
    private void EvaluateTouchPress(Hand hand)
    {
        // divided into 7 sections
        // middle button and 6 outer
        float middleSectionMin = -0.3f;
        float middleSectionMax = 0.3f;

        // touch button is pressed
        if (hand.controller.GetAxis() != Vector2.zero)
        {
            // set the active hand to the GUI           
            handGunGUI.activeHand = hand;
            // show canvas
            handGunGUI.GetComponent<Canvas>().enabled = true;
            // get touch coord
            Vector2 touchInput = hand.controller.GetAxis();
            // radius from middle
            float touchMagFromMiddle = touchInput.magnitude;
            // move touch pointer
            handGunGUI.MoveIndexPointerTo(touchInput);

            // TODO if statement here?
            {
                if (touchMagFromMiddle < middleSectionMax && (touchInput.x > middleSectionMin && touchInput.x < middleSectionMax) && (touchInput.y > middleSectionMin && touchInput.y < middleSectionMax))
                {
                    handGunGUI.highlightedBtnIndex = 0;
                    // middle section check passed
                    if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
                    {
                        // thumb button pressed down
                        toggleLaserSight = !toggleLaserSight;
                        laserSight.gameObject.SetActive(toggleLaserSight);
                        handGunGUI.highlightedBtnPressed = true;
                    }
                }
                else if (touchMagFromMiddle > middleSectionMax && (touchInput.x > middleSectionMin && touchInput.x < middleSectionMax) && (touchInput.y > middleSectionMax))
                {
                    // top middle check passed
                    handGunGUI.highlightedBtnIndex = 1;
                }
                else if (touchMagFromMiddle > middleSectionMax && (touchInput.x > middleSectionMin && touchInput.x < middleSectionMax) && (touchInput.y < middleSectionMin))
                {
                    // bottom middle check passed
                    handGunGUI.highlightedBtnIndex = 2;
                }
                else if (touchMagFromMiddle > middleSectionMax && (touchInput.x < middleSectionMax) && (touchInput.y > middleSectionMax))
                {
                    // top left check passed
                    handGunGUI.highlightedBtnIndex = 3;
                }
                else if (touchMagFromMiddle > middleSectionMax && (touchInput.x > middleSectionMax) && (touchInput.y > middleSectionMax))
                {
                    // top right check passed
                    handGunGUI.highlightedBtnIndex = 4;
                }
                else if (touchMagFromMiddle > middleSectionMax && (touchInput.x < middleSectionMax) && (touchInput.y < middleSectionMax))
                {
                    // bottom left check passed
                    handGunGUI.highlightedBtnIndex = 5;
                }
                else if (touchMagFromMiddle > middleSectionMax && (touchInput.x > middleSectionMax) && (touchInput.y < middleSectionMax))
                {
                    // bottom right check passed
                    handGunGUI.highlightedBtnIndex = 6;
                }
            }
        }
        else
        {
            handGunGUI.GetComponent<Canvas>().enabled = false;
            handGunGUI.touchPointer.SetActive(false);
            handGunGUI.highlightedBtnIndex = -1;
        }
    }

    //-------------------------------------------------------------------------------------------------
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
        ejectedMag.AddComponent<DestroyObjectAfterSeconds>().TimeSecondToDestroy = 10f;
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
        handGunGUI.remainingBulletCount = currentMagSize;
        handGunGUI.autoFire = autoFire;
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
        if (slide.localPosition == slideFeecbackTransform.localPosition)
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
            slide.localPosition = Vector3.MoveTowards(slide.localPosition, slideFeecbackTransform.localPosition, slideForwardRate);
            //slide.localPosition = Vector3.SmoothDamp(slide.localPosition, slideFeecbackTransform.localPosition, ref velocity, slideBackwardRate);
            //slideOpenSound.Play();
        }
        else
        {
            // get back into position to be ready to fire again 
            slide.localPosition = Vector3.MoveTowards(slide.localPosition, slideOriginalTransform.localPosition, slideForwardRate);
            caseEjected = false;
            //slideCloseSound.Play();
        }
    }
    //-------------------------------------------------------------------------------------------------
    private void EjectCase()
    {
        if (!caseEjected)
        {
            bulletCase.EjectFrom(ejectionPortTransform, Random.Range(2f, 4f));
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
