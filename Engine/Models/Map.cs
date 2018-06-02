using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Map
    {
        public int MapLocation { get; set; }
        public string ImageName { get; set; }

        public Map(int mapLocation, string imageName)
        {
            MapLocation = mapLocation;
            ImageName = $"/Engine;component/Images/Map/{imageName}";
        }
    }
}
