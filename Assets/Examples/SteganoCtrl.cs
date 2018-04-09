using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteganoCtrl : MonoBehaviour {

    public string secretMsg;
    public Sprite OriginalSprite;
    public string decodedMsg;

    public Texture2D tex;
    private Sprite mySprite;
    private SpriteRenderer sr;

    private bool isDecoded;

    // Use this for initialization
    void Start () {
        // GetPixel32, SetPixels32

        // Create new sprite according to original texture:
        sr = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);

        transform.position = new Vector3(1.5f, 1.5f, 0.0f);
        transform.localScale = new Vector3(5f, 5f, 1f);

        // var encodedTex = Steganongraphy.Encode(OriginalSprite.texture, secretMsg);
        var encodedTex = OriginalSprite.texture;
        mySprite = Sprite.Create(encodedTex, OriginalSprite.textureRect, new Vector2(0.5f, 0.5f), 100.0f);
        sr.sprite = mySprite;

        var c = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        Color32 c32 = c;
        Color cc = c32;
        Color32 c33 = cc;
        Color ccc = c33;


        c32.a = (byte)(c32.a | 1);
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        isDecoded = GUILayout.Toggle(isDecoded, "Decode");
        if (isDecoded)
        {
            decodedMsg = Steganography.DecodeAsString(mySprite.texture);
            GUILayout.Label(decodedMsg);
        }
        else
        {
            GUILayout.Label("**********************");
        }

        GUILayout.EndHorizontal();



    }

    // Update is called once per frame
    void Update () {

    }
}
