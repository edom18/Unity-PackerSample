using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleTexturePacker.Domain;
using SimpleTexturePacker.Application;
using SimpleTexturePacker.Infrastructure;

public class PackClient : MonoBehaviour
{
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

    [SerializeField] private Material _material = null;

    private PackService _packerService = null;
    private IPacker _packer = null;

    private void Start()
    {
        IPackImage[] imgs = new IPackImage[]
        {
            new DummyPackImage(50, 130, Color.red),
            new DummyPackImage(150, 10, Color.green),
            new DummyPackImage(250, 300, Color.blue),
            new DummyPackImage(350, 80, Color.yellow),
            new DummyPackImage(150, 30, Color.cyan),
        };

        int size = 1024;
        _packer = new Packer(size, _material);
        _packerService = new PackService(size, _packer);
        _packerService.SetImages(imgs);

        _tex = _packerService.GetPackedImage();
    }

    Texture _tex;
    private void OnGUI()
    {
        if (_tex == null)
        {
            return;
        }

        GUI.DrawTexture(new Rect(0, 0, _tex.width, _tex.height), _tex);
    }
}
