using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.IO;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private float threshold = .1f;
    [SerializeField] private float deadZone = .025f;

    private bool _isPressed;
    private Vector3 _startPos;
    private ConfigurableJoint _joint;
    private bool gestuurd = false;
    private int WelkeGeluid = 0;

    public UnityEvent onPressed, onReleased;
    public AudioSource sourceFireSound;
    public AudioClip fireSound;
   

    void Start()
    {

        _startPos = transform.localPosition;
        _joint = GetComponent<ConfigurableJoint>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!_isPressed && GetValue() - threshold > 0)
            Pressed();
        if (_isPressed && GetValue() - threshold <= 0)
            Released();

    }
    private float GetValue()
    {
        var value = Vector3.Distance(_startPos, transform.localPosition) / _joint.linearLimit.limit;

        if (Mathf.Abs(value) < deadZone)
            value = 0;

        return Mathf.Clamp(value, -1f, 1f);
    }
    private void Pressed()
    {
        _isPressed = true;
        onPressed.Invoke();
        //source.PlayOneShot(fireSound);
        Debug.Log("pressed");

       
    }
    private void Released()
    {
        _isPressed = false;
        onReleased.Invoke();
        Debug.Log("Released");
    }

}
