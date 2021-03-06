using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ECSharp
{
    public interface IEntity { }

    public class Entity
    {
        public long Index { get; set; }
        public string Name { get; set; }
        public World World { get; set; }

        public Entity(long index, World w, string name = "")
        {
            Index = index;
            World = w;
            Name = "";
        }
    }

}