using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class SteganographyTests {
    private static Color32 kBlack = new Color32(0, 0, 0, 0);
    private static Color32 kWhite = new Color32(255, 255, 255, 255);
    private static int k1BitMask = 0xfe;
    private static int k2BitMask = 0xfc;


    void StenoString(Steganography.Format format, Color32 defaulColor, int mask, int defaultSize = 9)
    {
        var tex = new Texture2D(defaultSize, defaultSize);
        var initialData = Enumerable.Repeat(defaulColor, tex.width * tex.height).ToArray();
        tex.SetPixels32(initialData);

        var msg = "this is a test";
        var encodedTex = Steganography.Encode(tex, msg, format);
        ValidateEncoding(tex, encodedTex, format);

        var result = Steganography.DecodeAsString(encodedTex);

        Assert.AreEqual(msg, result);
    }

    void StenoData(Steganography.Format format, Color32 defaulColor, int mask, int defaultSize = 24)
    {
        var tex = new Texture2D(defaultSize, defaultSize);
        var initialData = Enumerable.Repeat(defaulColor, tex.width * tex.height).ToArray();
        tex.SetPixels32(initialData);

        var data = new byte[255];
        for (byte i = 0; i < 255; i++)
        {
            data[i] = i;
        }
        var encodedTex = Steganography.Encode(tex, data, format);
        ValidateEncoding(tex, encodedTex, format);

        var result = Steganography.Decode(encodedTex);

        Assert.AreEqual(data, result);
    }

    void ValidateEncoding(Texture2D src, Texture2D encoded, Steganography.Format format)
    {
        Assert.AreEqual(src.width, encoded.width);
        Assert.AreEqual(src.height, encoded.height);

        var srcPixels = src.GetPixels32();
        var encodedPixels = encoded.GetPixels32();

        for (int i = Steganography.kHeaderNbPixels; i < srcPixels.Length; i++)
        {
            var srcPixel = srcPixels[i];
            var encodedPixel = encodedPixels[i];

            switch (format)
            {
                case Steganography.Format.A1:
                    Assert.AreEqual(srcPixel.r, encodedPixel.r);
                    Assert.AreEqual(srcPixel.g, encodedPixel.g);
                    Assert.AreEqual(srcPixel.b, encodedPixel.b);
                    Assert.AreEqual(srcPixel.a & k1BitMask, encodedPixel.a & k1BitMask);
                    break;
                case Steganography.Format.A2:
                    Assert.AreEqual(srcPixel.r, encodedPixel.r);
                    Assert.AreEqual(srcPixel.g, encodedPixel.g);
                    Assert.AreEqual(srcPixel.b, encodedPixel.b);
                    Assert.AreEqual(srcPixel.a & k2BitMask, encodedPixel.a & k2BitMask);
                    break;
                case Steganography.Format.RGB1:
                    Assert.AreEqual(srcPixel.r & k1BitMask, encodedPixel.r & k1BitMask);
                    Assert.AreEqual(srcPixel.g & k1BitMask, encodedPixel.g & k1BitMask);
                    Assert.AreEqual(srcPixel.b & k1BitMask, encodedPixel.b & k1BitMask);
                    Assert.AreEqual(srcPixel.a, encodedPixel.a);
                    break;
                case Steganography.Format.RGB2:
                    Assert.AreEqual(srcPixel.r & k2BitMask, encodedPixel.r & k2BitMask);
                    Assert.AreEqual(srcPixel.g & k2BitMask, encodedPixel.g & k2BitMask);
                    Assert.AreEqual(srcPixel.b & k2BitMask, encodedPixel.b & k2BitMask);
                    Assert.AreEqual(srcPixel.a, encodedPixel.a);
                    break;
                case Steganography.Format.RGBA1:
                    Assert.AreEqual(srcPixel.r & k1BitMask, encodedPixel.r & k1BitMask);
                    Assert.AreEqual(srcPixel.g & k1BitMask, encodedPixel.g & k1BitMask);
                    Assert.AreEqual(srcPixel.b & k1BitMask, encodedPixel.b & k1BitMask);
                    Assert.AreEqual(srcPixel.a & k1BitMask, encodedPixel.a & k1BitMask);
                    break;
                case Steganography.Format.RGBA2:
                    Assert.AreEqual(srcPixel.r & k2BitMask, encodedPixel.r & k2BitMask);
                    Assert.AreEqual(srcPixel.g & k2BitMask, encodedPixel.g & k2BitMask);
                    Assert.AreEqual(srcPixel.b & k2BitMask, encodedPixel.b & k2BitMask);
                    Assert.AreEqual(srcPixel.a & k2BitMask, encodedPixel.a & k2BitMask);
                    break;
            }

            
        }
    }

    #region RGBA1
    [Test]
    public void RGBA1_BlackImgStenoString()
    {
        StenoString(Steganography.Format.RGBA1, kBlack, k1BitMask);
    }

    [Test]
    public void RGBA1_WhiteImgStenoString()
    {
        StenoString(Steganography.Format.RGBA1, kWhite, k1BitMask);
    }

    [Test]
    public void RGBA1_BlackImgStenoData()
    {
        StenoData(Steganography.Format.RGBA1, kBlack, k1BitMask);
    }

    [Test]
    public void RGBA1_WhiteImgStenoData()
    {
        StenoData(Steganography.Format.RGBA1, kWhite, k1BitMask);
    }
    #endregion

    #region RGBA2
    [Test]
    public void RGBA2_BlackImgStenoString()
    {
        StenoString(Steganography.Format.RGBA2, kBlack, k2BitMask);
    }

    [Test]
    public void RGBA2_WhiteImgStenoString()
    {
        StenoString(Steganography.Format.RGBA2, kWhite, k2BitMask);
    }

    [Test]
    public void RGBA2_BlackImgStenoData()
    {
        StenoData(Steganography.Format.RGBA2, kBlack, k2BitMask);
    }

    [Test]
    public void RGBA2_WhiteImgStenoData()
    {
        StenoData(Steganography.Format.RGBA2, kWhite, k2BitMask);
    }
    #endregion

    #region A1
    [Test]
    public void A1_BlackImgStenoString()
    {
        StenoString(Steganography.Format.A1, kBlack, k1BitMask, 25);
    }

    [Test]
    public void A1_WhiteImgStenoString()
    {
        StenoString(Steganography.Format.A1, kWhite, k1BitMask, 25);
    }

    [Test]
    public void A1_BlackImgStenoData()
    {
        StenoData(Steganography.Format.A1, kBlack, k1BitMask, 46);
    }

    [Test]
    public void A1_WhiteImgStenoData()
    {
        StenoData(Steganography.Format.A1, kWhite, k1BitMask, 46);
    }
    #endregion

    #region A2
    [Test]
    public void A2_BlackImgStenoString()
    {
        StenoString(Steganography.Format.A2, kBlack, k2BitMask, 25);
    }

    [Test]
    public void A2_WhiteImgStenoString()
    {
        StenoString(Steganography.Format.A2, kWhite, k2BitMask, 25);
    }

    [Test]
    public void A2_BlackImgStenoData()
    {
        StenoData(Steganography.Format.A2, kBlack, k2BitMask, 300);
    }

    [Test]
    public void A2_WhiteImgStenoData()
    {
        StenoData(Steganography.Format.A2, kWhite, k2BitMask, 300);
    }
    #endregion

    [Test]
    public void BufferTooSmall()
    {
        var tex = new Texture2D(12, 1);
        var msg = "this is a test";
        Assert.Throws<Exception>(() => Steganography.Encode(tex, msg, Steganography.Format.RGBA2));
    }

    [Test]
    public void Capacity()
    {
        var tex = new Texture2D(4, 4);
        Assert.AreEqual(Steganography.TextureMaxDataSize(tex, Steganography.Format.A1), 1);
        Assert.AreEqual(Steganography.TextureMaxDataSize(tex, Steganography.Format.A2), 2);
        Assert.AreEqual(Steganography.TextureMaxDataSize(tex, Steganography.Format.RGB1), 3);
        Assert.AreEqual(Steganography.TextureMaxDataSize(tex, Steganography.Format.RGB2), 6);
        Assert.AreEqual(Steganography.TextureMaxDataSize(tex, Steganography.Format.RGBA1), 4);
        Assert.AreEqual(Steganography.TextureMaxDataSize(tex, Steganography.Format.RGBA2), 8);

        var tex2 = new Texture2D(1, 1);
        Assert.AreEqual(Steganography.TextureMaxDataSize(tex2, Steganography.Format.RGBA2), 0);
    }

    [Test]
    public void BitwiseTests()
    {
        byte n1 = 0x00;
        n1 = (byte)((n1 & k1BitMask) | 0x01);
        Assert.AreEqual(1, n1);

        var n2 = 0x01;
        n2 = (byte)((n2 & k1BitMask) | 0x01);
        Assert.AreEqual(1, n2);

        var n3 = 0x00;
        n3 = (byte)((n3 & k2BitMask) | 0x03);
        Assert.AreEqual(3, n3);

        var n4 = 0x03;
        n4 = (byte)((n4 & k2BitMask) | 0x03);
        Assert.AreEqual(3, n4);

        var n5 = 0x02;
        n5 = (byte)((n5 & k2BitMask) | 0x03);
        Assert.AreEqual(3, n5);
    }
}
