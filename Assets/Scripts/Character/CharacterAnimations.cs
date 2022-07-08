using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    private Animator _animator;

    public bool IsFlying { private get; set; }
    public bool IsMoving { private get; set; }

    void Start()
    {
        _animator = GetComponent<Animator>();    
    }

    void FixedUpdate()
    {
        _animator.SetBool("IsMoving", IsMoving);
        _animator.SetBool("IsFlying", IsFlying);
    }

    public void Jump()
    {
        _animator.SetTrigger("Jump");
    }
}
