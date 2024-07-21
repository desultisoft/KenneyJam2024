using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DateDemon : MonoBehaviour
{
    Vector3 startPosition;
    private bool shouldAnimate;
    private float pauseTime = 1000;
    int currentProgress;
    public GameObject heart;
    [SerializeField] float[] speeds;
    [SerializeField] Vector3[] targets;
    [SerializeField] float[] pauses;

    Rigidbody2D rb;
    GameObject spriteMask;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = new Vector3(0.0f, -0.42f);
        rb = GetComponent<Rigidbody2D>();
        spriteMask = GameObject.Find("Spritemask");
    }

    public void StartAnimate()
    {
        shouldAnimate = true;
        DatingProfile.datingProfile.Summoned = gameObject;
        heart.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shouldAnimate == false)
            return;

        if (pauseTime < pauses[currentProgress])
        {
            pauseTime += Time.fixedDeltaTime;
            return;
        }
        else pauseTime = 1000;
        Vector3 moveDir = (targets[currentProgress] - transform.position);
        const float targetRange = 0.1f;
        rb.velocity = moveDir.normalized * speeds[currentProgress];
        if (moveDir.magnitude < targetRange)
        {
            pauseTime = 0;
            rb.velocity = Vector2.zero;
            currentProgress++;
        }
        if (currentProgress >= targets.Length) SceneManager.LoadScene(0);
        else if (currentProgress == 1) spriteMask.SetActive(false);
    }

    public void ResetAnim()
    {
        shouldAnimate = false;
        spriteMask.SetActive(true);
        transform.position = startPosition;
    }
}
