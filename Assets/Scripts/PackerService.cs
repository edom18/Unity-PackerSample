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

        public Vector2 GetUV(int imageID)
        {
            return _packer.GetUV(imageID);
        }

        public void PackImages(IPackImage[] images)
        {
            _packer.Pack(images);
        }

        public void PackImage(IPackImage image)
        {
            _packer.Pack(image);
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
