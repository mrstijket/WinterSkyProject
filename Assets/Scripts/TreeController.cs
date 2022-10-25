using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TreeController : MonoBehaviour
{
    [SerializeField] float disableTime = 5.5f;
    [SerializeField] AnimationClip animationCrouch;
    BoxCollider2D boxCollider;
    Animation _animation;

    private void Awake()
    {
        _animation = GetComponent<Animation>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (boxCollider.IsTouching(other))
        {
            StartCoroutine(TriggerProcess());
        }
    }
    IEnumerator TriggerProcess()
    {
        Destroy(boxCollider);
        _animation.clip = animationCrouch;
        _animation.Play();
        yield return new WaitForSeconds(disableTime);
        _animation.Stop();
    }
}
