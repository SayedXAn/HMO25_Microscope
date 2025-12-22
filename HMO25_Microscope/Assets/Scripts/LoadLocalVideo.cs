using UnityEngine;
using System.IO;
using UnityEngine.Video;
using System.Linq;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LoadLocalVideo : MonoBehaviour
{
    //Developed by Sayed Anowar, Experience Engineer, Singularity Ltd (https://sayedanowar.xyz/)

    public string[] videoURLs;
    public VideoPlayer vp;
    public VideoPlayer vp2;
    public RenderTexture rt, rt2;
    private int currentVideoIndex = -1;
    //[System.NonSerialized] public string latestFile = "";
    //[System.NonSerialized] Texture2D loadedLocalImage;
    [SerializeField] Image backgroundImage;
    public TMP_InputField rfid_IF;
    private bool rfidOn = false;
    void Start()
    {
        if(Display.displays.Length > 1)
        {
            for(int i = 0; i < Display.displays.Length; i++)
            {
                Display.displays[i].Activate();
            }
        }
        InvokeRepeating(nameof(KeepTheIFActive), 1f, 0.5f);
        //get data dir
        Debug.Log($"Local path: {Application.dataPath}");
        GetVideosFromLocal();
        //CheckIfAnyBGPhoto();
        StopVideoAnytime();
        //KeepTheIFActive();
        
    }

    void GetVideosFromLocal()
    {
        Debug.Log("Videos from local called");
        string localDir = $"{Application.dataPath}/Videos";
        for(int i = 0; i < videoURLs.Length; i++)
        {
            string avDir = $"{Application.dataPath}/Videos/{i+1}.mp4";
            if(File.Exists(avDir))
            {
                videoURLs[i] = avDir;
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayVideo(0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayVideo(1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayVideo(2);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayVideo(3);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayVideo2ndScreen();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopVideoAnytime();
        }
    }

    public void CheckInputField()
    {
        //AV 1 - 0005045699
        //AV 2 - 0005045708
        //AV 3 - 0014051688
        //AV 4 - 0014045036
        //AV 5 - 0014043586
        //Reset - 0014063020

        if(rfid_IF.text.Length == 10)
        {
            Debug.Log("rfid: " + rfid_IF.text);
            string rfid = rfid_IF.text;
            if (rfid == "0005045699")
            {
                PlayVideo(0);
            }
            else if(rfid == "0005045708")
            {
                PlayVideo(1);
            }
            else if (rfid == "0014051688")
            {
                PlayVideo(2);
            }
            else if (rfid == "0014045036")
            {
                PlayVideo(3);
            }
            else if (rfid == "0014043586")
            {
                PlayVideo2ndScreen();
            }
            else if(rfid == "0014063020")
            {
                StopVideoAnytime();
            }
            KeepTheIFActive();
        }
    }

    public void PlayVideo(int ind)
    {
        if(vp.isPlaying && currentVideoIndex == ind)
        {
            return;
        }
        rt.Release();
        vp.url = videoURLs[ind];
        currentVideoIndex = ind;
        vp.Play();
        StartCoroutine(CheckIfVideoStopped());
    }
    public void PlayVideo2ndScreen()
    {
        if (vp2.isPlaying)
        {
            return;
        }
        rt2.Release();
        vp2.url = videoURLs[4];
        vp2.Play();
        StartCoroutine(CheckIfVideoStopped_2nd());
    }

    IEnumerator CheckIfVideoStopped()
    {        
        yield return new WaitForSeconds(1);
        if (vp.isPlaying)
        {
            StartCoroutine(CheckIfVideoStopped());
        }
        else
        {
            StopCoroutine(CheckIfVideoStopped());
            StopVideoAnytime();
        }
    }
    IEnumerator CheckIfVideoStopped_2nd()
    {
        yield return new WaitForSeconds(1);
        if (vp2.isPlaying)
        {
            StartCoroutine(CheckIfVideoStopped_2nd());
        }
        else
        {
            StopCoroutine(CheckIfVideoStopped_2nd());
            StopVideoAnytime();
        }
    }

    public void StopVideoAnytime()
    {
        vp.Stop();
        vp2.Stop();
        rt.Release();
        rt2.Release();
       
    }

    //public void CheckIfAnyBGPhoto()
    //{
    //    string directoryPath = $"{Application.dataPath}/Videos";
    //    if (!Directory.Exists(directoryPath))
    //    {
    //        Debug.LogError("Photos directory not found: " + directoryPath);
    //        return;
    //    }

    //    latestFile = directoryPath + "/bg.jpg";

    //    if (string.IsNullOrEmpty(latestFile))
    //    {
    //        Debug.LogError("No photo found to process.");
    //        backgroundImage.gameObject.SetActive(false);
    //        return;
    //    }
    //    else
    //    {
    //        backgroundImage.gameObject.SetActive(true);
    //        byte[] bytes = File.ReadAllBytes(latestFile);
    //        loadedLocalImage = new Texture2D(2, 2);
    //        loadedLocalImage.LoadImage(bytes);
    //        Sprite sprite = Sprite.Create(loadedLocalImage, new Rect(0, 0, loadedLocalImage.width, loadedLocalImage.height), new Vector2(0.5f, 0.5f));
    //        backgroundImage.sprite = sprite;
    //        backgroundImage.type = Image.Type.Simple;
    //        backgroundImage.preserveAspect = true;

    //    }
    //}

    public void KeepTheIFActive()
    {
        if(rfidOn)
        {
            return;
        }
        if (!rfid_IF.isFocused)
        {
            rfid_IF.text = "";
            rfid_IF.ActivateInputField();
        }
    }
    public void RFID_On(bool nf)
    {
        rfidOn = nf;
        if(nf)
        {
            
        }
    }
}
