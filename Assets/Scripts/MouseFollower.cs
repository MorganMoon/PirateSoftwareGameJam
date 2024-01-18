using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PirateSoftwareGameJam.Client
{
    public class MouseFollower : MonoBehaviour
    {
        Rigidbody2D _rigidBody;
        FrictionJoint2D _dragJoint;
 
        bool _active;

        [SerializeField]
        [Range(0f, 100f)]
        private float scale;

        [SerializeField]
        [Range(0f, 50f)]
        private float dragScale;

        private float _movementMultiplier;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _rigidBody = GetComponent<Rigidbody2D>();
            _dragJoint = GetComponentInChildren<FrictionJoint2D>();
            _movementMultiplier = scale;
        }

        private void OnApplicationFocus(bool focus)
        {
            _active = focus;
        }

        void Update()
        {
            if (!_active) { return; }

            if (Input.GetMouseButtonDown(0))
            {
                _rigidBody.freezeRotation = false;
                _dragJoint.enabled = true;
                _movementMultiplier = dragScale;
            }
            if (Input.GetMouseButtonUp(0))
            {
                _rigidBody.freezeRotation = true;
                _dragJoint.enabled = false;
                _movementMultiplier = scale;
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (Input.GetMouseButton(0))
                {
                    _rigidBody.freezeRotation = true;
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                if (Input.GetMouseButton(0))
                {
                    _rigidBody.freezeRotation = false;
                }
            }
            // Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            // mouseDelta = mouseDelta * scale;
            // transform.position += new Vector3(mouseDelta.x, mouseDelta.y, transform.position.z);
        }

        private void FixedUpdate()
        {
            if (!_active) { return; }
                
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            mouseDelta = mouseDelta * _movementMultiplier;
            Vector3 delta = new Vector3(mouseDelta.x, mouseDelta.y);
            _rigidBody.MovePosition(transform.position + delta * Time.deltaTime);
        }
    }
}
