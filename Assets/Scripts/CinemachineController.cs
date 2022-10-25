using UnityEngine;
using Cinemachine;

public class CinemachineController : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void StateSwitcher(CinemachineVirtualCamera toThisCamera)
    {
        animator.Play(toThisCamera.name);
    }
}
