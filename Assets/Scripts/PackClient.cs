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
    [SerializeField] private GameObject _target = null;

    private PackService _packerService = null;
    private IPacker _packer = null;
    private int _index = 0;
    private PackedInfo[] _infos = null;
    private Material _targetMaterial = null;

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

        _packer = new Packer(_material);
        _packerService = new PackService(_packer);
        _infos = _packerService.PackImages(imgs);

        // Show a packed texture as preview.
        _tex = _packerService.GetPackedImage();

        _targetMaterial = _target.GetComponent<Renderer>().material;
        _targetMaterial.mainTexture = _tex;

        ShowTex(0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _index = (_index + 1) % _infos.Length;
            ShowTex(_index);
        }
    }

    private void OnDestroy()
    {
        _packerService.Dispose();
    }

    private void ShowTex(int index)
    {
        // Show a texture from packed texture as preview.
        Vector4 scaleAndOffset = _packerService.GetScaleAndOffset(_infos[index]);
        _targetMaterial.SetVector("_ScaleAndOffset", scaleAndOffset);

        float aspect = scaleAndOffset.y / scaleAndOffset.x;
        _target.transform.localScale = new Vector3(aspect, 1f, 1f);
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
