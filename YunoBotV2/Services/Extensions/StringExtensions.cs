using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Services.Extensions
{
    public static class StringExtensions
    {

        /// <summary>
        /// Wrap a string with the given string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="wrapper"></param>
        /// <returns></returns>
        public static string Wrap(this string str, string wrapper)
        {

            return wrapper + str + wrapper;

        }

        /// <summary>
        /// Capitalize the first character of the string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Title(this string str)
        {

            if (string.IsNullOrEmpty(str))
                throw new NullReferenceException();
            else if (char.IsUpper(str.First()))
                return str;
            else
                return char.ToUpper(str.First()) + str.Substring(1);

        }

        /// <summary>
        /// Capitalize the first character of each word in the string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Title(this string str, char splitter = ' ')
        {

            if (string.IsNullOrEmpty(str))
                throw new NullReferenceException();
            else
            {

                var array = str.Split(' ');
                
                for(int i = 0; i < array.Length; i++)
                {
                    
                    array[i] = array[i].Title();

                }

                return string.Join(splitter.ToString(), array);

            }

        }

        /// <summary>
        /// Insert spaces before capital letters, ignores the first char
        /// </summary>
        /// <param name="str"></param>
        /// <returns>string</returns>
        public static string InsertSpaces(this string str)
        {

            var newStr = str;

            for (int i = 1; i < str.Length; i++)
            {

                if (char.IsUpper(str[i]))
                    newStr = str.Insert(i, " ");

            }

            return newStr;

        }

        /// <summary>
        /// Get the substring up to a certain string
        /// </summary>
        /// <param name="og"></param>
        /// <param name="str">The string to go up to</param>
        /// <returns>string</returns>
        public static string SubStringTo(this string og, string str)
        {

            var index = og.IndexOf(str);

            return og.Substring(0, index);

        }

    }

    //found from stackoverflow
    //public static class StringWidthHelperExtension
    //{
    //    private const uint LOCALE_SYSTEM_DEFAULT = 0x0800;
    //    private const uint LCMAP_HALFWIDTH = 0x00400000;
    //    private const uint LCMAP_FULLWIDTH = 0x00800000;

    //    /// <summary>
    //    /// Converts full-width string to half-width string
    //    /// </summary>
    //    /// <param name="str"></param>
    //    /// <returns>string</returns>
    //    public static string ToHalfWidth(this string str)
    //    {
    //        StringBuilder sb = new StringBuilder(256);
    //        LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_HALFWIDTH, str, -1, sb, sb.Capacity);
    //        return sb.ToString();
    //    }

    //    /// <summary>
    //    /// Converts half-width string to full-width string
    //    /// </summary>
    //    /// <param name="str"></param>
    //    /// <returns>string</returns>
    //    public static string ToFullWidth(this string str)
    //    {
    //        StringBuilder sb = new StringBuilder(256);
    //        LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_FULLWIDTH, str, -1, sb, sb.Capacity);
    //        return sb.ToString();
    //    }

    //    [DllImport("kernel32", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    //    private static extern int LCMapString(uint Locale, uint dwMapFlags, string lpSrcStr, int cchSrc, StringBuilder lpDestStr, int cchDest);
    //}

}
