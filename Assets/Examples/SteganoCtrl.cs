using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;

public class SteganoCtrl : MonoBehaviour
{
    public string secretMsg = "This is some secret stuff!";

    private Texture2D encodedTex;
    private Sprite mySprite;
    private bool isDecoded;
    private SpriteRenderer sr;

    void Start () {
        sr = gameObject.GetComponent<SpriteRenderer>();
        encodedTex = Steganography.Encode(sr.sprite.texture, secretMsg);
        sr.sprite = Sprite.Create(encodedTex, new Rect(0.0f, 0.0f, encodedTex.width, encodedTex.height), new Vector2(0.5f, 0.5f));

        var refPixels = encodedTex.GetPixels32();
        var pixels = encodedTex.GetPixels32();
        for (var i = 8; i < pixels.Length; i++)
        {
            Assert.AreEqual(refPixels[i], pixels[i]);
        }

        var bytes = encodedTex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Encoded.png", bytes);
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        isDecoded = GUILayout.Toggle(isDecoded, "Decode");
        if (isDecoded)
        {
            var decodedMsg = Steganography.DecodeAsString(sr.sprite.texture);
            GUILayout.Label(decodedMsg);
        }
        else
        {
            GUILayout.Label("**********************");
        }

        GUILayout.Button(encodedTex);
        GUILayout.EndHorizontal();
    }

    // Update is called once per frame
    void Update () {

    }
}
