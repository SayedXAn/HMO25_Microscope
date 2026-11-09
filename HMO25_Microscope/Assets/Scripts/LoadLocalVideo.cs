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
    //public VideoPlayer vp2;
    public RenderTexture rt;
    private int currentVideoIndex = -1;
    [SerializeField] Image backgroundImage;
    public TMP_InputField rfid_IF;
    private bool rfidOn = false;

    public string[] rfidCards;
    void Start()
    {
        InvokeRepeating(nameof(KeepTheIFActive), 1f, 0.5f);
        Debug.Log($"Local path: {Application.dataPath}");

        ReadRFIDFile();
        GetVideosFromLocal();
        StopVideoAnytime();        
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

    void ReadRFIDFile()
    {
        string filePath = Path.Combine(Application.dataPath, "RFID/rfid.txt");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            rfidCards = lines
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Take(6)
                .ToArray();

            Debug.Log($"Loaded {rfidCards.Length} RFID strings.");

            foreach (string rfid in rfidCards)
            {
                Debug.Log(rfid);
            }
        }
        else
        {
            Debug.LogError("RFID file not found: " + filePath);
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

        if(rfid_IF.text.Length == 10)
        {
            Debug.Log("rfid: " + rfid_IF.text);
            string rfid = rfid_IF.text;

            if (rfid == rfidCards[0])
            {
                PlayVideo(0);
            }
            else if(rfid == rfidCards[1])
            {
                PlayVideo(1);
            }
            else if (rfid == rfidCards[2])
            {
                PlayVideo(2);
            }
            else if (rfid == rfidCards[3])
            {
                PlayVideo(3);
            }
            else if (rfid == rfidCards[4])
            {
                PlayVideo2ndScreen();
            }
            else if(rfid == rfidCards[5])
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
        if (vp.isPlaying)
        {
            return;
        }
        rt.Release();
        vp.url = videoURLs[4];
        vp.Play();
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
        if (vp.isPlaying)
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
        rt.Release();
       
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
