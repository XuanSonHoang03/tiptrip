using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TipTrip.Common.Models
{
    public class LoginDTO
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }   

        [JsonPropertyName("password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
