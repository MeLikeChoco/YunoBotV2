using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Objects.Deserializers
{
    public class Name
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("first")]
        public string First { get; set; }
        [JsonProperty("last")]
        public string Last { get; set; }
    }

    public class Location
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

    public class Login
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public class Id
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class Picture
    {
        [JsonProperty("large")]
        public string Large { get; set; }
        [JsonProperty("medium")]
        public string Medium { get; set; }
        [JsonProperty("thumbnail")]
        public string Thumnail { get; set; }
    }

    public class RandomPerson
    {
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("name")]
        public Name Name { get; set; }
        [JsonProperty("location")]
        public Location Location { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("login")]
        public Login Login { get; set; }
        [JsonProperty("dob")]
        public string DOB { get; set; }
        [JsonProperty("registered")]
        public string Registered { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("cell")]
        public string Cellphone { get; set; }
        [JsonProperty("id")]
        public Id Id { get; set; }
        [JsonProperty("picture")]
        public Picture Picture { get; set; }
        [JsonProperty("nat")]
        public string Nationality { get; set; }
    }
}
