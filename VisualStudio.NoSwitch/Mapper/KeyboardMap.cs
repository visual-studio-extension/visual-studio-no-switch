using System;
using System.Collections.Generic;
using System.Linq;

namespace VisualStudio.NoSwitch.Mapper
{
    class KeyboardMap
    {
        private static readonly String ThaiKeys =
            "ๅ/-ภถุึคตจขชๆไำพะัีรนยบลฟหกดเ้่าสวงผปแอิืทมใฝ+๑๒๓๔ู฿๕๖๗๘๙๐\"ฎฑธํ๊ณฯญฐ,ฤฆฏโฌ็๋ษศซ.()ฉฮฺ์?ฒฬฦ";
        private static readonly String EnglishKey =
            "1234567890-=qwertyuiop[]asdfghjkl;'zxcvbnm,./!@#$%^&*()_+QWERTYUIOP{}ASDFGHJKL:\"ZXCVBNM<>?";

        private static Dictionary<char, char> CreateDict()
        {
            var dict = new Dictionary<char, char>();
            EnglishKey.ToCharArray().Select((Element, Index) => new { Index, Element }).ToList().ForEach(x =>
                    {
                        dict[x.Element] = ThaiKeys[x.Index];
                    });

            return dict; ;
        }

        public static Dictionary<char, char> Maps { get; } = CreateDict();
        public static char GetThaiChar(char englishChar)
        {
            if (Maps.ContainsKey(englishChar))
            {
                return Maps[englishChar];
            }
            return englishChar;
        }
    }
}
