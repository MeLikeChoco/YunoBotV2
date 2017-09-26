using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoV3.Extensions
{
    public static class StringExtensions
    {

        public static string Title(this string str)
            => str.Title(' ');

        public static string Title(this string str, char delimiter)
            => str.Title(delimiter.ToString());

        public static string Title(this string str, string delimiter = " ")
        {

            var array = str.Split(delimiter);

            return char.ToUpper(array.First().First()) + array.Aggregate((output, next) => $"{output}{delimiter}{char.ToUpper(next.First())}{next.Substring(1)}").Substring(1);

        }

    }
}
