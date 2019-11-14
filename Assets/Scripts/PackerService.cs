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
        private int _count = 0;

        public PackService(int size, IPacker packer)
        {
            _rootNode = new Node();
            _rootNode.Rectangle = new Rect(0, 0, size, size);

            _packer = packer;
        }

        public void PackImages(IPackImage[] images)
        {
            foreach (var img in images)
            {
                Node node = _rootNode.Insert(img);
                node.SetImageID(_count++);

                _packer.Pack(img, node.Rectangle);
            }
        }

        public void PackImage(IPackImage image)
        {
            Node node = _rootNode.Insert(image);
            node.SetImageID(_count++);

            _packer.Pack(image, node.Rectangle);
        }

        public Texture GetPackedImage()
        {
            return _packer.GetPackedImage();
        }
    }
}
