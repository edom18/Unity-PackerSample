using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleTexturePacker.Domain;
using SimpleTexturePacker.Application;
using SimpleTexturePacker.Infrastructure;

public class PackClient : MonoBehaviour
{
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
        _packerService.PackImages(imgs);

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
