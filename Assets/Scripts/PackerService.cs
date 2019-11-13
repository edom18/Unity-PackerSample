using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleTexturePacker.Domain;

namespace SimpleTexturePacker.Application
{
    public class PackService
    {
        private Node _rootNode = null;
        private IPacker _packer = null;

        public PackService(int size, IPacker packer)
        {
            _rootNode = new Node();
            _rootNode.Rectangle = new Rect(0, 0, size, size);

            _packer = packer;
        }

        public void SetImages(IPackImage[] images)
        {
            int count = 0;
            foreach (var img in images)
            {
                Node node = _rootNode.Insert(img);
                node.SetImage(count++);

                _packer.Pack(img, node.Rectangle);
            }
        }

        public Texture GetPackedImage()
        {
            return _packer.GetPackedImage();
        }
    }
}
