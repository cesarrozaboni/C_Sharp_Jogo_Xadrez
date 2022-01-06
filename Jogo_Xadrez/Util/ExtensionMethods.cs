using System.Linq;
using System.Text;

namespace System
{
    public static class ExtensionMethods
    {
        public static string FormatarData(this string data)
        {
            var dataFormatada = data.Substring(6, 2);
            dataFormatada += "/";
            dataFormatada += data.Substring(4, 2);
            dataFormatada += "/";
            dataFormatada += data.Substring(0, 4);

            return dataFormatada;
        }

        /// <summary>
        /// Concat this string with one Space
        /// </summary>
        /// <param name="str"></param>
        /// <returns>string with space</returns>
        public static string Space(this string str)
        {
            return $"{str}\u00A0";
        }

        /// <summary>
        /// Concat this string with Space setted in parameter
        /// </summary>
        /// <param name="str"></param>
        /// <returns>string with spaces</returns>
        public static string Space(this string str, int spaces)
        {
            var amountSpace = new StringBuilder();
            
            for (int i = 0; i < spaces; i++)
                amountSpace.Append("\u00A0");
            
            return $"{str}{amountSpace}";
        }

        public static string ToFormat(this string texto, params object[] value)
        {
            var stringBuilder = new StringBuilder();

            if (value.Length > 0)
                stringBuilder.AppendFormat(texto, value);

            return stringBuilder.ToString();
        }

        public static byte[] Convert64ToByte(this string ImgBase64)
        {
            return Convert.FromBase64String(ImgBase64);

        }

       #region "String"
        public static string EncrementaEndereco(this string endereco)
        {
            var aux = string.Empty;

            if (endereco.IsEmpty())
            {
                return endereco;
            }

            if (endereco.Trim().Length < 8)
            {
                return endereco;
            }
            if (!string.IsNullOrEmpty(endereco.Trim()))
            {
                if (endereco.Trim().Length <= 8)
                {
                    aux += endereco.Substring(0, 2);
                    aux += "-";
                    aux += endereco.Substring(2, 3);
                    aux += "-";
                    aux += endereco.Substring(5, 1);
                    aux += "-";
                    aux += endereco.Substring(6, 2);
                }
                else
                {
                    aux += endereco.Substring(0, 1);
                    aux += "-";
                    aux += endereco.Substring(1, 2);
                    aux += "-";
                    aux += endereco.Substring(3, 3);
                    aux += "-";
                    aux += endereco.Substring(6, 3);
                }

            }
            else
            {
                aux = "---";
            }
            return aux;
        }

        public static string ReturnUpper(this string value)
        {
            return value.Trim().ToUpper();
        }



        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool IsNotEmpty(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        public static bool In(this string str, params string[] value)
        {
            var ret = false;
            foreach (var i in value)
            {
                if (str.Equals(i))
                {
                    ret = true;
                }
            }

            return ret;
        }

        public static bool NotIn(this string str, params object[] value)
        {
            var ret = true;
            foreach (var i in value)
            {
                if (str.Equals(i))
                {
                    ret = false;
                }
            }

            return ret;
        }

        public static string QuotedStr(this string str)
        {
            return string.Format("'{0}'", str);
        }

        public static string SendParams(this string url, params string[] parameter)
        {
            var count = parameter.Where(x => x.IsNotEmpty()).Count();

            if (count.Equals(0))
                return url;

            string param = url;

            for (int i = 0; i < parameter.Length; i++)
            {
                param += string.Format("{0}p{1}={2}", (i == 0 ? "?" : "&"), (i + 1), parameter[i]);
            }

            return param;
        }

        /// <summary>
        /// Se a primeira expressão for nula retorna a segunda
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns>System.String</returns>
        public static string IfNull(this string str, string value1, string value2)
        {
            return value1.IsEmpty() ? value2 : value1;
        }
        #endregion

        #region "int"
        public static bool In(this int number, params int[] value)
        {
            var ret = false;
            foreach (var i in value.Where(i => number.Equals(i)))
            {
                ret = true;
            }

            return ret;
        }

        public static bool NotIn(this int number, params object[] value)
        {
            var ret = true;
            foreach (var i in value)
            {
                if (number.Equals(i))
                {
                    ret = false;
                }
            }

            return ret;
        }
        #endregion

        #region "Decimal"
        public static bool In(this decimal number, params decimal[] value)
        {
            var ret = false;
            foreach (var i in value.Where(i => number.Equals(i)))
            {
                ret = true;
            }

            return ret;
        }

        public static bool NotIn(this decimal number, params decimal[] value)
        {
            var ret = true;
            foreach (var i in value)
            {
                if (number.Equals(i))
                {
                    ret = false;
                }
            }

            return ret;
        }
        #endregion

        #region "Sbyte"
        public static bool In(this sbyte number, params sbyte[] value)
        {
            var ret = false;
            foreach (var i in value.Where(i => number.Equals(i)))
            {
                ret = true;
            }

            return ret;
        }

        public static bool NotIn(this sbyte number, params sbyte[] value)
        {
            var ret = true;
            foreach (var i in value)
            {
                if (number.Equals(i))
                {
                    ret = false;
                }
            }

            return ret;
        }
        #endregion

        #region "Char"
        public static bool In(this char? caracter, params char[] value)
        {
            var ret = false;
            foreach (var i in value.Where(i => caracter.Equals(i)))
            {
                ret = true;
            }

            return ret;
        }

        public static bool NotIn(this char? caracter, params char[] value)
        {
            var ret = true;
            foreach (var i in value)
            {
                if (caracter.Equals(i))
                {
                    ret = false;
                }
            }

            return ret;
        }
        #endregion
    }
}
