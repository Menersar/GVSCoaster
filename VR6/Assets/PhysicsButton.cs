using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    [SerializeField] private float threshhold = .1f;
    [SerializeField] private float deadzone = .025f;

    private bool _isPressed;
    private Vector3 _startPos;
    private ConfigurableJoint _joint;

    public UnityEvent onPressed, onReleased;

    public bool restrictButton = true;
    public bool playerTouchesButton = false;
    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.localPosition;
        _joint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isPressed && GetValue() + threshhold >= 1)
            Pressed();
        if (_isPressed && GetValue() - threshhold <= 0)
            Released();
    }

    private float GetValue()
    {
        var value = Vector3.Distance(_startPos, transform.localPosition) / _joint.linearLimit.limit;

        if (Mathf.Abs(value) < deadzone)
        {
            value = 0;
        }

        return Mathf.Clamp(value, -1f, 1f);
    }

    private void Pressed()
    {
        _isPressed = true;
        if (playerTouchesButton)
        onPressed.Invoke();
        Debug.Log("Pressed");
    }

    private void Released()
    {

        _isPressed = false;
        if(playerTouchesButton)
        onReleased.Invoke();
        Debug.Log("Released");
        playerTouchesButton = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && restrictButton)
        {
            this.GetComponent<ConfigurableJoint>().yMotion = ConfigurableJointMotion.Limited;
            playerTouchesButton = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && restrictButton)
        {
            this.GetComponent<ConfigurableJoint>().yMotion = ConfigurableJointMotion.Locked;
            playerTouchesButton = false;
        }
    }
}
