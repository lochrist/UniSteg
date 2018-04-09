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


    void StenoString(Steganography.Format format, Color32 defaulColor, int mask)
    {
        var tex = new Texture2D(9, 9);
        var initialData = Enumerable.Repeat(defaulColor, tex.width * tex.height).ToArray();
        tex.SetPixels32(initialData);

        var msg = "this is a test";
        var encodedTex = Steganography.Encode(tex, msg, format);
        ValidateEncoding(tex, encodedTex, mask);

        var result = Steganography.DecodeAsString(encodedTex);

        Assert.AreEqual(msg, result);
    }

    void StenoData(Steganography.Format format, Color32 defaulColor, int mask)
    {
        var tex = new Texture2D(24, 24);
        var initialData = Enumerable.Repeat(defaulColor, tex.width * tex.height).ToArray();
        tex.SetPixels32(initialData);

        var data = new byte[255];
        for (byte i = 0; i < 255; i++)
        {
            data[i] = i;
        }
        var encodedTex = Steganography.Encode(tex, data, format);
        ValidateEncoding(tex, encodedTex, mask);

        var result = Steganography.Decode(encodedTex);

        Assert.AreEqual(data, result);
    }

    void ValidateEncoding(Texture2D src, Texture2D encoded, int mask)
    {
        Assert.AreEqual(src.width, encoded.width);
        Assert.AreEqual(src.height, encoded.height);

        var srcPixels = src.GetPixels32();
        var encodedPixels = encoded.GetPixels32();

        for (int i = 0; i < srcPixels.Length; i++)
        {
            var srcPixel = srcPixels[i];
            var encodedPixel = encodedPixels[i];
            Assert.AreEqual(srcPixel.r & mask, encodedPixel.r & mask);
            Assert.AreEqual(srcPixel.g & mask, encodedPixel.g & mask);
            Assert.AreEqual(srcPixel.b & mask, encodedPixel.b & mask);
            Assert.AreEqual(srcPixel.a & mask, encodedPixel.a & mask);
        }
    }

    [Test]
    public void RGBA1_BlackImgStenoString()
    {
        StenoString(Steganography.Format.RGBA1, kBlack, 0xfe);
    }

    [Test]
    public void RGBA1_WhiteImgStenoString()
    {
        StenoString(Steganography.Format.RGBA1, kWhite, 0xfe);
    }

    [Test]
    public void RGBA1_BlackImgStenoData()
    {
        StenoData(Steganography.Format.RGBA1, kBlack, 0xfe);
    }

    [Test]
    public void RGBA1_WhiteImgStenoData()
    {
        StenoData(Steganography.Format.RGBA1, kWhite, 0xfe);
    }

    [Test]
    public void RGBA2_BlackImgStenoString()
    {
        StenoString(Steganography.Format.RGBA2, kBlack, 0xfc);
    }

    [Test]
    public void RGBA2_WhiteImgStenoString()
    {
        StenoString(Steganography.Format.RGBA2, kWhite, 0xfc);
    }

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
        Assert.AreEqual(Steganography.TextureMaxDataSize(tex, Steganography.Format.A2), 3);
        Assert.AreEqual(Steganography.TextureMaxDataSize(tex, Steganography.Format.RGB1), 5);
        Assert.AreEqual(Steganography.TextureMaxDataSize(tex, Steganography.Format.RGB2), 10);
        Assert.AreEqual(Steganography.TextureMaxDataSize(tex, Steganography.Format.RGBA1), 7);
        Assert.AreEqual(Steganography.TextureMaxDataSize(tex, Steganography.Format.RGBA2), 14);

        var tex2 = new Texture2D(1, 1);
        Assert.AreEqual(Steganography.TextureMaxDataSize(tex2, Steganography.Format.RGBA2), 0);
    }

    [Test]
    public void BitwiseTests()
    {
        byte n1 = 0x00;
        n1 = (byte)((n1 & 0xfe) | 0x01);
        Assert.AreEqual(1, n1);

        var n2 = 0x01;
        n2 = (byte)((n2 & 0xfe) | 0x01);
        Assert.AreEqual(1, n2);

        var n3 = 0x00;
        n3 = (byte)((n3 & 0xfc) | 0x03);
        Assert.AreEqual(3, n3);

        var n4 = 0x03;
        n4 = (byte)((n4 & 0xfc) | 0x03);
        Assert.AreEqual(3, n4);

        var n5 = 0x02;
        n5 = (byte)((n5 & 0xfc) | 0x03);
        Assert.AreEqual(3, n5);
    }
}
