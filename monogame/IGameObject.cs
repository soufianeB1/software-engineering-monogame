using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
namespace monogame
{
    public interface IGameObject
    {
        Rectangle Bounds { get; }  // Voor collision detection
        bool IsSolid { get; }
    }
}
