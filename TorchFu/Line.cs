using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TorchFu
{
    public class TorchLine
    {
        protected string line;

        public TorchLine(string text)
        { line = text; }
        public TorchLine(string type, string name, string value)
        { line = '<' + type.ToUpper() + '>' + name.ToUpper() + ':' + value; }

        public override string ToString()
        { return line; }
    }

    public class StringLine : TorchLine
    {
        public StringLine(string name, string value)
            : base("string", name, value)
        { }
    }

    public class FloatLine : TorchLine
    {
        public FloatLine(string name, double value)
            : base("float", name, value.ToString("0.000000"))
        { }
    }

    public class IntLine : TorchLine
    {
        public IntLine(string name, int value)
            : base("integer", name, value.ToString())
        { }
    }

    public class BoolLine : TorchLine
    {
        public BoolLine(string name, bool value)
            : base("bool", name, value.ToString().ToLower())
        { }
    }

    public class LongLine : TorchLine
    {
        public LongLine(string name, long value)
            : base("integer64", name, value.ToString())
        { }
    }

    public class TranslateLine : TorchLine
    {
        public TranslateLine(string name, string value)
            : base("translate", name, value)
        { }
    }

    public class UIntLine : TorchLine
    {
        public UIntLine(string name, uint value)
            : base("unsigned int", name, value.ToString())
        { }
    }

    public class XMLLine : TorchLine
    {
        public XMLLine(int tabs, string tag)
            : base(string.Empty)
        {
            for (int i = 0; i < tabs; ++i)
                line += '\t';
            line += '<' + tag + '>';
        }
    }
}