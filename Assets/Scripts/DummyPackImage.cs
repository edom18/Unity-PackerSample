using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleTexturePacker.Domain;

public class DummyPackImage : IPackImage
{
    private int _width = 0;
    private int _height = 0;
    private Texture _texture = null;

    private Color _color = Color.white;

    int IPackImage.Width => _width;
    int IPackImage.Height => _height;
    Texture IPackImage.Texture => _texture;

    public DummyPackImage(int width, int height, Color color)
    {
        _width = width;
        _height = height;
        _color = color;

        GenerateDummyTexture();
    }

    private void GenerateDummyTexture()
    {
        Color[] colors = new Color[_width * _height];

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = _color;
        }

        Texture2D tex = new Texture2D(_width, _height);
        tex.SetPixels(colors);
        tex.Apply();

        _texture = tex;
    }
}
