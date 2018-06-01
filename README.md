# Unitils

Library containing multiple Unity utilities function and workflows.

## Steganography

This utility namespace implements a simple [steganography](https://en.wikipedia.org/wiki/Steganography) algorithm allowing a user to "hide" data inside a Unity `Texture2D`. Basically it uses the least significant bits of the color channels to hier data.

A quick example can be found in `SteganoCtrl.cs` (used in the `Steganography` scene):

```csharp
void Start () {
    sr = gameObject.GetComponent<SpriteRenderer>();

    // Encode secretMsg in a texture. This returns a copy of the texture
    encodedTex = Steganography.Encode(sr.sprite.texture, secretMsg);

    // Set the new texture in our Sprite component
    sr.sprite = Sprite.Create(encodedTex, new Rect(0.0f, 0.0f, encodedTex.width, encodedTex.height), new Vector2(0.5f, 0.5f));
}

private void OnGUI()
{
    GUILayout.BeginHorizontal();

    isDecoded = GUILayout.Toggle(isDecoded, "Decode");
    if (isDecoded)
    {
        // Decode the secretMsg from the texture
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
```

### More details

We support multiple ways to encode data depending on how you want to store the data and affect image quality:

```csharp
public enum Format
{
    RGB1,
    RGB2,
    RGBA1,
    RGBA2,
    A1,
    A2
}
```

Each stream of data has a 8 bytes header that contains the format the data is written with and the size of the written data.