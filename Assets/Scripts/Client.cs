using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Packer _packer = null;

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

        _packer = new Packer(1024);
        _packer.SetImages(imgs);
    }
}
