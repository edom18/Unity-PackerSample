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
        private int _size = 0;

        public Packer(int size, Material material)
        {
            _size = size;

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

        void IPacker.Pack(IPackImage image, Rect rect)
        {
            _material.SetTexture("_PackTex", image.Texture);
            // Vector4
            // _material.SetFloat("_Scale", _size / image.Width);

            Vector4 scaleAndOffset = new Vector4();

            scaleAndOffset.x = _size / image.Width;
            scaleAndOffset.y = _size / image.Height;

            scaleAndOffset.z = rect.x;
            scaleAndOffset.w = rect.y;
            scaleAndOffset.z /= _size;
            scaleAndOffset.w /= _size;

            _material.SetVector("_ScaleAndOffset", scaleAndOffset);
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

        private void SwapBuffer()
        {
            RenderTexture temp = _current;
            _current = _next;
            _next = temp;
        }
    }
}
