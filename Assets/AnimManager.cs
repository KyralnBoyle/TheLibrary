using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class AnimManager : MonoBehaviour
{
    public Animator[] animators;
    public Vector3[] pageLocations;
    public AudioClip[] audioClips;
    public AudioClip clipRef;
    public AudioSource source;
    public GameObject vrCharacter;
    public float touchTimer = 2f;
    public int currentPage = 0;
    public int lastPage = -1;
    protected bool pressEnabled;
    public VRTK_ControllerEvents controllerEvents;
    protected VRTK_ControllerReference controllerReference;

    private void Start()
    {
        pressEnabled = true;
        vrCharacter.transform.position = pageLocations[currentPage];
        //Camera.main.transform.position = pageLocations[currentPage];
        source.clip = clipRef;
        source.Play();
    }


    public void NextPage()
    {
        currentPage++;

        currentPage = Mathf.Clamp(currentPage, 0, animators.Length - 1);

        if (currentPage <= animators.Length -1)
        {
            clipRef = audioClips[currentPage];
            if (animators[currentPage])
            {
                animators[currentPage].enabled = false;
                animators[currentPage].enabled = true;
                animators[currentPage].Play(0);
            }
            //Debug.Log("AHHH");
            //Debug.Log(clipRef.name);
            source.clip = clipRef;
            source.Play();
        }

        vrCharacter.transform.position = pageLocations[currentPage];
        //Camera.main.transform.position = pageLocations[currentPage];

    }

    public void LastPage()
    {
        currentPage--;

        currentPage = Mathf.Clamp(currentPage, 0, animators.Length - 1);

        if (currentPage <= animators.Length - 1)
        {
            clipRef = audioClips[currentPage];
            if (animators[currentPage])
            {
                animators[currentPage].enabled = false;
                animators[currentPage].enabled = true;
                animators[currentPage].Play(0);
            }
            
            source.clip = clipRef;
            source.Play();
        }

        vrCharacter.transform.position = pageLocations[currentPage];
        //Camera.main.transform.position = pageLocations[currentPage];

    }


    private void Update()
    {
        GameObject rightHand = VRTK_DeviceFinder.GetControllerRightHand(true);
        controllerReference = VRTK_ControllerReference.GetControllerReference(rightHand);

        if (ControllerForward())
        {
            NextPage();
            pressEnabled = false;
            StartCoroutine(EnableTouch());
        }
        else if (ControllerBackward())
        {
            LastPage();
            pressEnabled = false;
            StartCoroutine(EnableTouch());
        }

    }



    protected virtual bool ControllerForward()
    {
        if (!VRTK_ControllerReference.IsValid(controllerReference))
        {
            return false;
        }

        return (pressEnabled && VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Touchpad, SDK_BaseController.ButtonPressTypes.Press, controllerReference));
    }

    protected virtual bool ControllerBackward()
    {
        if (!VRTK_ControllerReference.IsValid(controllerReference))
        {
            return false;
        }

        return (pressEnabled && VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Trigger, SDK_BaseController.ButtonPressTypes.Press, controllerReference));
    }

    IEnumerator EnableTouch()
    {
        yield return new WaitForSeconds(touchTimer);

        pressEnabled = true;
    }

}
