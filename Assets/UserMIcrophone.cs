using UnityEngine;
public class UserMicrophone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var audio = GetComponent<AudioSource>();
        Debug.Log("NAME:" + Microphone.devices[0]);
        audio.clip = Microphone.Start(Microphone.devices[0], true, 10, 44100);
        audio.loop = true;
        while (!(Microphone.GetPosition(null) > 0)){}
        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}