using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.IO;

public class PhysicsButtonIntro : MonoBehaviour
{
    [SerializeField] private float threshold = .1f;
    [SerializeField] private float deadZone = .025f;
   
    private bool _isPressed;
    private Vector3 _startPos;
    private ConfigurableJoint _joint;
    private string json;
    private bool gestuurd = false;
    public int WelkeGeluid = 0;

    public int welkeKnop;
    public bool statusKnop;

    public UnityEvent onPressed, onReleased, NextScene, Deel1, Deel2,Deel3;
    public AudioSource sourceFireSound;
    public AudioClip fireSound;
    public AudioSource source1;
    public AudioClip Sound1;
    public AudioSource source2;
    public AudioClip Sound2;
    


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

    public void herhaal(int herhaal)
    {
        WelkeGeluid = WelkeGeluid - herhaal;
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
        if (WelkeGeluid == 0)
        {
            source1.PlayOneShot(Sound1);
            WelkeGeluid = WelkeGeluid + 1;
        }
        if (WelkeGeluid == 2)
        {
            NextScene.Invoke();
            WelkeGeluid = 0;
        }
      
    }
    private void Released()
    {
        _isPressed = false;
        onReleased.Invoke();
        Debug.Log("Released");
        if (WelkeGeluid == 1)
        {
         
            WelkeGeluid = WelkeGeluid + 1;
        }
       
        

    }

}
