using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    class CameraFollowComponent : IComponent
    {
        public int CameraId { get; set; }  
        public Rectangle CameraCage { get; set; }

    }
}
