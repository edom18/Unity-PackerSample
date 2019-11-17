using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleTexturePacker.Domain;

namespace SimpleTexturePacker.Infrastructure
{
    public class PackImage : IPackImage
    {
        private Texture _texture = null;

        int IPackImage.Width { get { return _texture.width; } }
        int IPackImage.Height { get { return _texture.height; } }
        Texture IPackImage.Texture { get { return _texture; } }

        public PackImage(Texture texture)
        {
            _texture = texture;
        }
    }
}
