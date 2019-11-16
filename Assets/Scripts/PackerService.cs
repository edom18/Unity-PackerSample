using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleTexturePacker.Domain;

namespace SimpleTexturePacker.Application
{
    public class PackService : System.IDisposable
    {
        private IPacker _packer = null;

        public PackService(int size, IPacker packer)
        {
            _packer = packer;
        }

        public Vector4 GetScaleAndOffset(PackedEntity entity)
        {
            return _packer.GetScaleAndOffset(entity);
        }

        public PackedEntity[] PackImages(IPackImage[] images)
        {
            return _packer.Pack(images);
        }

        public Texture GetPackedImage()
        {
            return _packer.GetPackedImage();
        }

        public void Dispose()
        {
            _packer.Dispose();
        }
    }
}
