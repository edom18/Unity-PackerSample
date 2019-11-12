using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleTexturePacker.Domain;
using SimpleTexturePacker.Application;
using SimpleTexturePacker.Infrastructure;

public class Client : MonoBehaviour
{
    public class DummyPackImage : IPackImage
    {
        private int _width = 0;
        private int _height = 0;

        int IPackImage.Width => _width;
        int IPackImage.Height => _height;

        public DummyPackImage(int width, int height)
        {
            _width = width;
            _height = height;
        }
    }

    [SerializeField]
    private Material _material = null;

    private PackService _packerService = null;
    private IPacker _packer = null;

    private void Start()
    {
        IPackImage[] imgs = new IPackImage[]
        {
            new DummyPackImage(50, 130),
            new DummyPackImage(150, 10),
            new DummyPackImage(250, 300),
            new DummyPackImage(350, 80),
            new DummyPackImage(150, 30),
        };

        int size = 1024;
        _packer = new Packer(size, _material);
        _packerService = new PackService(1024, _packer);
        _packerService.SetImages(imgs);
    }
}
