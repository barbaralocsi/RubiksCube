using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuccesssPanel : MonoBehaviour
{
    [SerializeField] Text timeText;
    [SerializeField] Button shareButton;
    [SerializeField] Button closeButton;

    void Start()
    {
        shareButton.onClick.AddListener(ShareButtonClick);
        closeButton.onClick.AddListener(CloseButtonClick);
    }

    void CloseButtonClick()
    {
        gameObject.SetActive(false);
    }

    public void setTime(string timeString)
    {
        timeText.text = timeString;
    }

    void ShareButtonClick()
    {
        FB.FeedShare(
            link: new Uri("https://cdn.pixabay.com/photo/2018/04/28/01/04/graphic-3356338_960_720.png"),
            callback: FeedCallback);
    }

    private void FeedCallback(IShareResult result)
    {
        if (result.Cancelled || !String.IsNullOrEmpty(result.Error))
        {
            Debug.Log("FeedShare Error: " + result.Error);
        }
        else if (!String.IsNullOrEmpty(result.PostId))
        {
            // Print post identifier of the shared content
            Debug.Log(result.PostId);
        }
        else
        {
            // Share succeeded without postID
            Debug.Log("FeedShare success!");
        }
    }
}
