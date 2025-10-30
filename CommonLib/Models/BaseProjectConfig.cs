using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Models
{
    public interface BaseProjectConfig
    {
        public string key { get; set; }
        public string label { get; set; }
    }
}
