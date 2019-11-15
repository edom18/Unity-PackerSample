using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleTexturePacker.Domain;
using SimpleTexturePacker.Application;
using SimpleTexturePacker.Infrastructure;

public class PackClient : MonoBehaviour
{
    [SerializeField] private Material _material = null;
    [SerializeField] private Texture[] _textures = null;

    private PackService _packerService = null;
    private IPacker _packer = null;

    private void Start()
    {
        IPackImage[] imgs = new IPackImage[_textures.Length];

        for (int i = 0; i < _textures.Length; i++)
        {
            imgs[i] = new PackImage(_textures[i]);
        }

        // int min = 10;
        // int max = 50;
        // int count = 955;
        // IPackImage[] imgs = new IPackImage[count];
        // for (int i = 0; i < imgs.Length; i++)
        // {
        //     imgs[i] = new DummyPackImage(Random.Range(min, max), Random.Range(min, max), Random.ColorHSV());
        // };

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
