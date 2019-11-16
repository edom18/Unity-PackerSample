using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SimpleTexturePacker.Domain
{
    public class PackedInfo
    {
        private int _imageID = -1;
        public int ImageID => _imageID;

        private Rect _rectangle;
        public Rect Rectangle => _rectangle;

        public PackedInfo(int imageID, Rect rectangle)
        {
            _imageID = imageID;
            _rectangle = rectangle;
        }
    }

    public interface IPackImage
    {
        int Width { get; }
        int Height { get; }
        Texture Texture { get; }
    }

    public interface IPacker : System.IDisposable
    {
        PackedInfo[] Pack(IPackImage[] images);
        Texture GetPackedImage();
        Vector4 GetScaleAndOffset(PackedInfo info);
    }

    public class Node
    {
        public Node[] Child = new Node[2];
        public Rect Rectangle = default;
        private int _imageID = -1;

        private bool _isLeafNode = true;

        private bool CheckFitInRect(IPackImage image)
        {
            bool isInRect = (image.Width <= Rectangle.width) &&
                            (image.Height <= Rectangle.height);
            return isInRect;
        }

        private bool CheckFitPerfectly(IPackImage image)
        {
            bool isSameBoth = (image.Width == Rectangle.width) &&
                              (image.Height == Rectangle.height);
            return isSameBoth;
        }

        public Node Insert(IPackImage image)
        {
            if (!_isLeafNode)
            {
                Node newNode = Child[0].Insert(image);
                if (newNode != null)
                {
                    return newNode;
                }

                return Child[1].Insert(image);
            }
            else
            {
                if (_imageID != -1)
                {
                    return null;
                }

                if (!CheckFitInRect(image))
                {
                    return null;
                }

                if (CheckFitPerfectly(image))
                {
                    return this;
                }

                _isLeafNode = false;

                Child[0] = new Node();
                Child[1] = new Node();

                float dw = Rectangle.width - image.Width;
                float dh = Rectangle.height - image.Height;

                if (dw > dh)
                {
                    Child[0].Rectangle = new Rect(Rectangle.x, Rectangle.y, image.Width, Rectangle.height);
                    Child[1].Rectangle = new Rect(Rectangle.x + image.Width, Rectangle.y, Rectangle.width - image.Width, Rectangle.height);
                }
                else
                {
                    Child[0].Rectangle = new Rect(Rectangle.x, Rectangle.y, Rectangle.width, image.Height);
                    Child[1].Rectangle = new Rect(Rectangle.x, Rectangle.y + image.Height, Rectangle.width, Rectangle.height - image.Height);
                }

                return Child[0].Insert(image);
            }
        }

        public void SetImageID(int imageID)
        {
            _imageID = imageID;
            _isLeafNode = true;
        }

        public int GetImageID()
        {
            return _imageID;
        }

        public Node Find(int imageID)
        {
            if (imageID == _imageID)
            {
                return this;
            }

            if (_isLeafNode)
            {
                return null;
            }

            Node child = Child[0].Find(imageID);
            if (child != null)
            {
                return child;
            }

            child = Child[1].Find(imageID);

            return child;
        }
    }
}
