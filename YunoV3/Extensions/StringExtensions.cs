using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoV3.Extensions
{
    public static class StringExtensions
    {

        public static string Title(this string str, char delimiter = ' ')
            => str.Title(delimiter.ToString());

        public static string Title(this string str, string delimiter = " ")
        {

            var output = "";

            foreach(var word in str.Split(delimiter))
                output += char.ToUpper(word.FirstOrDefault()) + word.Substring(1);

            return output;

        }

    }
}
