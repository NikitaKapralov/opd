using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkbCulture.AppHost.Dtos
{
    public class UserResponseDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public int Level { get; set; } = 1;

        public int[] VisitedLocations { get; set; } = Array.Empty<int>();

        public string Icon { get; set; } = "";

    }
}
