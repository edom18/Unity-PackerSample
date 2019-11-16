﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleTexturePacker.Domain;
using SimpleTexturePacker.Application;
using SimpleTexturePacker.Infrastructure;

public class PackClient : MonoBehaviour
{
    [SerializeField] private Material _material = null;
    [SerializeField] private Texture[] _textures = null;
    [SerializeField] private GameObject _target = null;

    private PackService _packerService = null;
    private IPacker _packer = null;

    private void Start()
    {
        IPackImage[] imgs = new IPackImage[_textures.Length];

        for (int i = 0; i < _textures.Length; i++)
        {
            imgs[i] = new PackImage(_textures[i]);
        }

        // Create dummy images.
        //
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
        PackedInfo[] infos = _packerService.PackImages(imgs);

        // Show a packed texture as preview.
        _tex = _packerService.GetPackedImage();

        // Show a texture from packed texture as preview.
        Material material = _target.GetComponent<Renderer>().material;
        material.mainTexture = _tex;
        Vector4 scaleAndOffset = _packerService.GetScaleAndOffset(infos[0]);
        material.SetVector("_ScaleAndOffset", scaleAndOffset);
    }

    private void OnDestroy()
    {
        _packerService.Dispose();
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
