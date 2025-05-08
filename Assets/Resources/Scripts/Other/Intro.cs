using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Animator _animator;
    private void Awake()
    {
        _animator.SetTrigger("Waving");
        _playerController.enabled = false;
    }
    private void ActivatePlayer()
    {
        _playerController.enabled = true;
        gameObject.SetActive(false);
    }
}
