using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

[Serializable]
public class CardTextData
{
    public string cardTop;
    public string cardMiddle;
    public string cardBottom;
    public string cardImage;
}

public class CardTextController : MonoBehaviour
{
    [SerializeField] private Text CardTopText;
    [SerializeField] private Text CardMiddleText;
    [SerializeField] private Text CardBottomText;
    [SerializeField] private SpriteRenderer cardImageRenderer;
    [SerializeField] private Sprite[] cardSprites;

    public void UpdateCardText(string jsonData)
    {
        Debug.Log($"Received Card text {jsonData}");
        try
        {
            CardTextData textData = JsonUtility.FromJson<CardTextData>(jsonData);
            
            if (!string.IsNullOrEmpty(textData.cardTop) && CardTopText != null)
            {
                CardTopText.text = textData.cardTop;
            }
            
            if (!string.IsNullOrEmpty(textData.cardMiddle) && CardMiddleText != null)
            {
                CardMiddleText.text = textData.cardMiddle;
            }

            if (!string.IsNullOrEmpty(textData.cardBottom) && CardBottomText != null)
            {
                CardBottomText.text = textData.cardBottom;
            }

            if (!string.IsNullOrEmpty(textData.cardImage) && cardImageRenderer != null)
            {
                var url = textData.cardImage;
                if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    StartCoroutine(LoadImageFromUrl(url));
                }
                else
                {
                    Sprite selectedSprite = Array.Find(cardSprites, sprite => sprite.name.Equals(textData.cardImage, StringComparison.OrdinalIgnoreCase));
                    if (selectedSprite != null)
                    {
                        cardImageRenderer.sprite = selectedSprite;
                    }
                    else
                    {
                        Debug.LogWarning($"Sprite for {textData.cardImage} not found.");
                    }
                }
            }

            Debug.Log($"Card text updated - Top: {textData.cardTop}, Middle: {textData.cardMiddle}, Bottom: {textData.cardBottom}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error updating card text: {e.Message}");
            // Set default text if no parameters provided
            if (CardTopText != null) CardTopText.text = "Happy Birthday!";
            if (CardBottomText != null) CardBottomText.text = "Have a wonderful day!";
            if (CardBottomText != null) CardBottomText.text = "Love Jane!";
        }
    }

    private IEnumerator LoadImageFromUrl(string url)
    {
        Debug.Log($"Requesting image from: {url}");

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            // Add the required headers
            uwr.SetRequestHeader("Origin", "http://localhost");

            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogWarning($"Error downloading image: {uwr.error}");
            }
            else
            {
                Texture2D originalTexture = DownloadHandlerTexture.GetContent(uwr);

                // Create a RenderTexture with the desired dimensions
                RenderTexture renderTexture = new RenderTexture(900, 1273, 24);
                RenderTexture.active = renderTexture;

                // Blit the original texture into the RenderTexture
                Graphics.Blit(originalTexture, renderTexture);

                // Create a new Texture2D with the desired dimensions
                Texture2D resizedTexture = new Texture2D(900, 1273, originalTexture.format, false);
                resizedTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                resizedTexture.Apply();

                // Clean up
                RenderTexture.active = null;
                renderTexture.Release();

                // Create the sprite from the resized texture
                Sprite sprite = Sprite.Create(resizedTexture, new Rect(0, 0, resizedTexture.width, resizedTexture.height), new Vector2(0.5f, 0.5f));
                cardImageRenderer.sprite = sprite;
            }
        }
    }
}