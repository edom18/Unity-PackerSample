using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleTexturePacker.Domain;

namespace SimpleTexturePacker.Infrastructure
{
    public class Packer : IPacker
    {
        private RenderTexture _rt1 = null;
        private RenderTexture _rt2 = null;
        private RenderTexture _current = null;
        private RenderTexture _next = null;

        private Texture2D _storeTexture = null;
        private Material _material = null;
        private Node _rootNode = null;
        private int _size = 0;
        private int _count = 0;

        public Packer(Material material)
        {
            _material = material;
        }

        private void Initialize(IPackImage[] images)
        {
            _size = CalcSize(images);

            _rootNode = new Node();
            _rootNode.Rectangle = new Rect(0, 0, _size, _size);

            if (_storeTexture != null) { GameObject.Destroy(_storeTexture); }
            if (_rt1 != null) { _rt1.Release(); }
            if (_rt2 != null) { _rt2.Release(); }

            _storeTexture = new Texture2D(_size, _size);

            _rt1 = CreateRenderTexture(_size, _size);
            _rt2 = CreateRenderTexture(_size, _size);

            _current = _rt1;
            _next = _rt2;
        }

        private int CalcSize(IPackImage[] images)
        {
            float totalArea = 0;

            foreach (var img in images)
            {
                totalArea += img.Width * img.Height;
            }

            for (int i = 1; i <= 13; i++)
            {
                int size = 2 << i;
                int area = size * size;

                if (area >= totalArea)
                {
                    return size;
                }
            }

            Debug.LogError("Target images have too much area. Please sprite the images.");

            return 0;
        }

        private RenderTexture CreateRenderTexture(int width, int height)
        {
            RenderTexture rt = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
            rt.Create();

            RenderTexture back = RenderTexture.active;
            RenderTexture.active = rt;
            GL.Clear(true, true, Color.white);
            RenderTexture.active = back;
            return rt;
        }

        PackedInfo[] IPacker.Pack(IPackImage[] images)
        {
            Initialize(images);

            PackedInfo[] entities = new PackedInfo[images.Length];

            for (int i = 0; i < images.Length; i++)
            {
                if (TryPack(images[i], out var entity))
                {
                    entities[i] = entity;
                }
            }

            return entities;
        }

        private bool TryPack(IPackImage image, out PackedInfo info)
        {
            Node node = _rootNode.Insert(image);

            if (node == null)
            {
                Debug.LogError("Packer texture is full. An image won't be packed.");
                info = null;
                return false;
            }

            int imageID = _count++;
            node.SetImageID(imageID);

            Pack(image, node.Rectangle);

            info = new PackedInfo(imageID, node.Rectangle);
            return true;
        }

        private void Pack(IPackImage image, Rect rect)
        {
            Vector4 scaleAndOffset = GetScaleAndOffset(rect);

            _material.SetVector("_ScaleAndOffset", scaleAndOffset);
            _material.SetTexture("_PackTex", image.Texture);

            Graphics.Blit(_current, _next, _material);

            SwapBuffer();
        }

        private Vector4 GetScaleAndOffset(Rect rect)
        {
            Vector4 scaleAndOffset = new Vector4();

            float size = (float)_size;

            scaleAndOffset.x = size / rect.width;
            scaleAndOffset.y = size / rect.height;

            scaleAndOffset.z = rect.x / size;
            scaleAndOffset.w = rect.y / size;

            return scaleAndOffset;
        }

        Texture IPacker.GetPackedImage()
        {
            RenderTexture back = RenderTexture.active;
            RenderTexture.active = _current;

            _storeTexture.ReadPixels(new Rect(0, 0, _size, _size), 0, 0);
            _storeTexture.Apply();

            RenderTexture.active = back;

            return _storeTexture;
        }

        Vector4 IPacker.GetScaleAndOffset(PackedInfo entity)
        {
            Node target = _rootNode.Find(entity.ImageID);
            return GetScaleAndOffset(target.Rectangle);
        }

        private void SwapBuffer()
        {
            RenderTexture temp = _current;
            _current = _next;
            _next = temp;
        }

        void System.IDisposable.Dispose()
        {
            _rt1.Release();
            _rt2.Release();
            _rt1 = null;
            _rt2 = null;
        }
    }
}
