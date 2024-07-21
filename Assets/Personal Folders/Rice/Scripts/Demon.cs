using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Interactable
{
    [SerializeField] float speed;
    DatingProfile datingProfile;
    PostProcessingController postProcessingController;
    Rigidbody2D rb;

    public Transform destination;
    bool shouldMove;

    public Animator hearts;

    private void Start()
    {
        datingProfile = DatingProfile.datingProfile;
        postProcessingController = PostProcessingController.PostProcessingSingleton;
        rb = GetComponent<Rigidbody2D>();

        shouldMove = true;
    }


    private void FixedUpdate()
    {
        if (DatingProfile.datingProfile.Summoned)
        {
            shouldMove = true;
            destination = DatingProfile.datingProfile.Summoned.transform;
            hearts.gameObject.SetActive(true);
        }

        if (shouldMove)
        {
            Vector3 moveDir = (destination.position - transform.position);
            const float targetRange = 0.1f;
            rb.velocity = moveDir.normalized * speed;
            if (moveDir.magnitude < targetRange) shouldMove = false;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public override void Interact(GameObject interactor)
    {
        if (datingProfile.profileObtained == true) return;
        datingProfile.GenerateProfile();
        postProcessingController.StartChromaticEffect(0.2f, 0.7f);
        postProcessingController.StartCameraShake(0.1f, 0.2f);

        UIFlyAndFlip.uIFlyAndFlip.LoadDemon();
    }
}
