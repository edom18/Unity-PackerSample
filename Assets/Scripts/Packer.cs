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

        public Packer(int size, Material material)
        {
            _size = size;

            _rootNode = new Node();
            _rootNode.Rectangle = new Rect(0, 0, size, size);

            _storeTexture = new Texture2D(size, size);
            _material = material;

            _rt1 = CreateRenderTexture(size, size);
            _rt2 = CreateRenderTexture(size, size);

            _current = _rt1;
            _next = _rt2;
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

        void IPacker.Pack(IPackImage[] images)
        {
            foreach (var img in images)
            {
                TryPack(img);
            }
        }

        void IPacker.Pack(IPackImage image)
        {
            TryPack(image);
        }

        private void TryPack(IPackImage image)
        {
            Node node = _rootNode.Insert(image);

            if (node == null)
            {
                Debug.LogError("Packer texture is full. An image won't be packed.");
                return;
            }

            node.SetImageID(_count++);

            Pack(image, node.Rectangle);
        }

        private void Pack(IPackImage image, Rect rect)
        {
            Vector4 scaleAndOffset = new Vector4();

            scaleAndOffset.x = (float)_size / (float)image.Width;
            scaleAndOffset.y = (float)_size / (float)image.Height;

            scaleAndOffset.z = (float)rect.x / (float)_size;
            scaleAndOffset.w = (float)rect.y / (float)_size;

            _material.SetVector("_ScaleAndOffset", scaleAndOffset);
            _material.SetTexture("_PackTex", image.Texture);

            Graphics.Blit(_current, _next, _material);

            SwapBuffer();
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

        Vector2 IPacker.GetUV(int imageID)
        {
            Node target = _rootNode.Find(imageID);
            return Vector2.zero;
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
