using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderlySystem.BLL.Configurations
{
    public class CloudinarySettings
    {
        public string CloudName { get; set; } = default!;
        public string ApiKey { get; set; } = default!;
        public string ApiSecret { get; set; } = default!;
        public string? Folder { get; set; }
    }
}
