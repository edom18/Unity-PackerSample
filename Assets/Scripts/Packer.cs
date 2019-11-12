using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleTexturePacker.Domain;

namespace SimpleTexturePacker.Infrastructure
{
    public class Packer : IPacker
    {
        private Texture2D _storeTexture = null;
        private Material _material = null;

        public Packer(int size, Material material)
        {
            _storeTexture = new Texture2D(size, size);
            _material = material;
        }

        void IPacker.Pack(IPackImage image, Rect rect)
        {

        }

        Texture2D IPacker.GetPackedTexture()
        {
            return _storeTexture;
        }
    }

}
