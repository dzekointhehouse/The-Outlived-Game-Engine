using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components
{
    class HighScoreComponent : IComponent
    {
        public string[] name = new string[10];
        public string[] score = new string[10];
        public string path;
    }
}
