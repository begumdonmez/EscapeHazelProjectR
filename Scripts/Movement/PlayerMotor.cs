//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 _playerVelocity;
    
    private bool _isGrounded;
    private bool _crouching ;
    
    public float gravity = -9.8f;
    public float crouchHeight=0.75f;
    public float originalHeight;
    public float speed = 25f;
    public float crouchSpeed = 15f;

//Look
    public Camera cam;
    private float xRotation = 0f;
    public float xSensitivity = 600f;
    public float ySensitivity = 600f;
    
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        originalHeight = _controller.height;
        Debug.Log(originalHeight);
        
    }

    void Update()
    {
        _isGrounded = _controller.isGrounded;
    }
    
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        float characterRotationY = cam.transform.eulerAngles.y;
        Vector3 inputDirection = Quaternion.Euler(0, characterRotationY, 0)*new Vector3(input.x,0,input.y);
        _controller.Move(inputDirection * speed * Time.deltaTime);
        //_controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        _playerVelocity.y += gravity * Time.deltaTime;
        if (_isGrounded && _playerVelocity.y < 0)
            _playerVelocity.y = -2f;
        _controller.Move(_playerVelocity * Time.deltaTime);
        //Debug.Log(_playerVelocity.y);
    }

    public void ProcessCrouch()
    {
        if (_isGrounded)
        {
            if (!_crouching)
            {
                _controller.height = crouchHeight;
                speed = 15f;
                _crouching = true;
            }
            else
            {
                _controller.height = originalHeight;
                speed = 25f;
                _crouching = false;
            }
            
        }
        
    }

    public void ProcessLook(Vector2 lookInput)
    {
        float mouseX = lookInput.x;
        float mouseY = lookInput.y;

        xRotation -= (mouseY*Time.deltaTime)*ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -500f, 500f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        
        transform.Rotate(Vector3.up*(mouseX*Time.deltaTime)*xSensitivity);
    }
}
