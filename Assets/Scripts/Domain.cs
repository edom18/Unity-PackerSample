using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SimpleTexturePacker.Domain
{
    public interface IPackImage
    {
        int Width { get; }
        int Height { get; }
        Texture Texture { get; }
    }

    public interface IPacker
    {
        void Pack(IPackImage image, Rect rect);
        Texture GetPackedImage();
    }

    public class Node
    {
        public Node[] Child = new Node[2];
        public Rect Rectangle = default;
        public int ImageID = -1;

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
                if (ImageID != -1)
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
            ImageID = imageID;
            _isLeafNode = true;
        }
    }
}
