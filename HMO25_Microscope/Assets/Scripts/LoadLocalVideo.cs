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
    public RenderTexture rt;
    private int currentVideoIndex = -1;
    [System.NonSerialized] public string latestFile = "";
    [System.NonSerialized] Texture2D loadedLocalImage;
    [SerializeField] Image backgroundImage;
    public TMP_InputField rfid_IF;
    void Start()
    {
        //get data dir 
        Debug.Log($"Local path: {Application.dataPath}");
        GetVideosFromLocal();
        CheckIfAnyBGPhoto();
        StopVideoAnytime();
        //KeepTheIFActive();
        InvokeRepeating("KeepTheIFActive", 0.5f, 0.5f);
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
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayVideo(1);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StopVideoAnytime();
        }
    }

    public void CheckInputField()
    {
        //0014059262
        //0014063020
        //0014058627

        if(rfid_IF.text.Length == 10)
        {
            Debug.Log("rfid: " + rfid_IF.text);
            string rfid = rfid_IF.text;
            if (rfid == "0014059262")
            {
                PlayVideo(0);
            }
            else if(rfid == "0014063020")
            {
                PlayVideo(1);
            }
            else if(rfid == "0014058627")
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

    public void StopVideoAnytime()
    {
        vp.Stop();
        rt.Release();
    }

    public void CheckIfAnyBGPhoto()
    {
        string directoryPath = $"{Application.dataPath}/Videos";
        if (!Directory.Exists(directoryPath))
        {
            Debug.LogError("Photos directory not found: " + directoryPath);
            return;
        }

        latestFile = directoryPath + "/bg.jpg";

        if (string.IsNullOrEmpty(latestFile))
        {
            Debug.LogError("No photo found to process.");
            backgroundImage.gameObject.SetActive(false);
            return;
        }
        else
        {
            backgroundImage.gameObject.SetActive(true);
            byte[] bytes = File.ReadAllBytes(latestFile);
            loadedLocalImage = new Texture2D(2, 2);
            loadedLocalImage.LoadImage(bytes);
            Sprite sprite = Sprite.Create(loadedLocalImage, new Rect(0, 0, loadedLocalImage.width, loadedLocalImage.height), new Vector2(0.5f, 0.5f));
            backgroundImage.sprite = sprite;
            backgroundImage.type = Image.Type.Simple;
            backgroundImage.preserveAspect = true;

        }
    }

    public void KeepTheIFActive()
    {
        if (!rfid_IF.isFocused)
        {
            rfid_IF.text = "";
            rfid_IF.ActivateInputField();
        }
    }
}
