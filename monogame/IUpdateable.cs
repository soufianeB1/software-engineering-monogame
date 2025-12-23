using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monogame
{
    public interface IUpdateable
    {
        void Update(GameTime gameTime);
    }
}
