using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoV3.Objects.Deserializers
{
    public class RandomUser
    {

        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("name")]
        public RandomUserName Name { get; set; }
        [JsonProperty("location")]
        public RandomUserLocation Location { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("login")]
        public RandomUserLogin Login { get; set; }
        [JsonProperty("dob")]
        public string Birthdate { get; set; }
        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }
        [JsonProperty("cell")]
        public string CellNumber { get; set; }
        [JsonProperty("picture")]
        public RandomUserPicture Picture { get; set; }
        [JsonProperty("nat")]
        public string Nationality { get; set; }

    }

    public class RandomUserName
    {

        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("first")]
        public string First { get; set; }
        [JsonProperty("last")]
        public string Last { get; set; }

    }

    public class RandomUserLocation
    {

        [JsonProperty("street")]
        public string Street { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("postcode")]
        public string Postcode { get; set; }

    }

    public class RandomUserLogin
    {

        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }

    }

    public class RandomUserPicture
    {

        [JsonProperty("large")]
        public string Large { get; set; }
        [JsonProperty("medium")]
        public string Medium { get; set; }
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

    }
}
