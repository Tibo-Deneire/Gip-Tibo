using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.IO;

public class PhysicsButtonRed : MonoBehaviour
{
    [SerializeField] private float threshold = .1f;
    [SerializeField] private float deadZone = .025f;
   
    private bool _isPressed;
    private Vector3 _startPos;
    private ConfigurableJoint _joint;
    private string json;
    private bool gestuurd = false;

    public int welkeKnop;
    public bool statusKnop;

    public UnityEvent onPressed, onReleased;
    public AudioSource source;
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
        
         if (gestuurd == false)
        {
            //Save1();
            StartCoroutine(Upload());
            gestuurd = true;
        }
        else if (gestuurd == true)
        {
           Save2();
           StartCoroutine(Upload());
           gestuurd = false;
        }
       
    }
    private void Released()
    {
        _isPressed = false;
        onReleased.Invoke();
        Debug.Log("Released");
        
    }

    
    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        using (UnityWebRequest www = UnityWebRequest.Post("http://gip20212022.gobbin.be/api/tibo/knop", json))
        {
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));

            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Upload complete!");
            }
        }
    }

    [System.Serializable]
    private class JSONObject
    {
        public int welkeKnop;
        public bool statusKnop;
    }
    
    void Save1 ()
    {
        JSONObject stuurjson = new JSONObject();
        stuurjson.welkeKnop = 2;
        stuurjson.statusKnop = true;
        json = JsonUtility.ToJson(stuurjson);
        Debug.Log("json");
        Debug.Log(json);
    }

    void Save2()
    {
        JSONObject stuurjson = new JSONObject();
        stuurjson.welkeKnop = 2;
        stuurjson.statusKnop = false;
        json = JsonUtility.ToJson(stuurjson);
        Debug.Log("json");
        Debug.Log(json);
    }
    
  
}
