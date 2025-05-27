using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLBenchmarkTool
{
    public class Nacecode
    {
        public string Code { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string TextFr { get; set; } = string.Empty;
        public string TextEn { get; set; } = string.Empty;
        public string? ParentCode { get; set; }

        public Nacecode()
        { }

        public Nacecode(string code, string text, string textFr, string textEn, string? parentCode = null)
        {
            Code = code;
            Text = text;
            TextFr = textFr;
            TextEn = textEn;
            ParentCode = parentCode;
        }

        public override string ToString()
        {
            return $"Nacecode: Code={Code}, Text={Text}, TextFr={TextFr}, TextEn={TextEn}, ParentCode={ParentCode}";
        }
    }
}
