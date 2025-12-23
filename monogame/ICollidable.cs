using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
namespace monogame
{
    public interface ICollidable
    {
        void OnCollision(IGameObject other);
        Rectangle GetCollisionBounds();
    }
}
