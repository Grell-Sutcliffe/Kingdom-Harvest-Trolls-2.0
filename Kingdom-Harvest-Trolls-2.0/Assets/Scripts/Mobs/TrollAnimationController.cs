using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollAnimationController : MonoBehaviour
{
    TrollController trollController;

    private Animator _animator;
    private Rigidbody2D _body;

    private void Start()
    {
        trollController = GetComponent<TrollController>();
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _animator.SetFloat("velocity", ((trollController.target == null) || (trollController.is_attacing == true)) ? (0) : (1));
        _animator.SetBool("is_attacking", trollController.is_attacing);
    }
}
