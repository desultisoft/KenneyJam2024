using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Interactable
{
    [SerializeField] float speed;
    DatingProfile datingProfile;
    PostProcessingController postProcessingController;
    Rigidbody2D rb;

    Vector3 target;
    bool shouldMove;

    private void Start()
    {
        datingProfile = DatingProfile.datingProfile;
        postProcessingController = PostProcessingController.PostProcessingSingleton;
        rb = GetComponent<Rigidbody2D>();
        SetTarget(new Vector3(9.5f, -3.8f));
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if (shouldMove)
        {
            Vector3 moveDir = (target - transform.position);
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
        UIFlyAndFlip.uIFlyAndFlip.StartSequence();
    }

    public void SetTarget(Vector3 _target)
    {
        target = _target;
        shouldMove = true;
    }
}
