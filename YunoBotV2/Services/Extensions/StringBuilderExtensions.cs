using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Services.Extensions
{
    public static class StringBuilderExtensions
    {

        public static void AppendLineHelp(this StringBuilder builder, string str)
        {

            builder.AppendLPaddingChar(str, 30, padFrom: "|", direction: PaddingDirection.Right);

        }

        /// <summary>
        /// Apply padding to string in AppendLine
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="str">The string to pad</param>
        /// <param name="paddingAmount">Amount to pad</param>
        /// <param name="paddingChar">Character to pad with</param>
        /// <param name="direction">Direction to start padding from</param>
        public static void AppendLPaddingChar(this StringBuilder builder,
            string str,
            int paddingAmount,
            char paddingChar = ' ',
            string padFrom = ":",
            int? startFrom = null,
            PaddingDirection direction = PaddingDirection.Left)
        {

            var index = startFrom ?? str.IndexOf(padFrom);
            var firstStr = str.Substring(0, index);
            var secondStr = str.Substring(index);

            switch (direction)
            {

                case PaddingDirection.Left:
                    builder.AppendLine(firstStr.PadLeft(paddingAmount, paddingChar) + secondStr);
                    break;
                case PaddingDirection.Right:
                    builder.AppendLine(firstStr.PadRight(paddingAmount, paddingChar) + secondStr);
                    break;

            }

        }

        /// <summary>
        /// Apply padding to string in AppendLine
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="str">The string to pad</param>
        /// <param name="paddingAmount">Amount to pad</param>
        /// <param name="paddingChar">Character to pad with</param>
        /// <param name="direction">Direction to start padding from</param>
        public static void AppendLinePadding(this StringBuilder builder,
            string str,
            int paddingAmount,
            char paddingChar = ' ',
            PaddingDirection direction = PaddingDirection.Left)
        {

            switch (direction)
            {

                case PaddingDirection.Left:
                    builder.AppendLine(str.PadLeft(paddingAmount));
                    break;
                case PaddingDirection.Right:
                    builder.AppendLine(str.PadRight(paddingAmount));
                    break;

            }

        }

        public enum PaddingDirection
        {

            Left,
            Right

        }

    }
}
