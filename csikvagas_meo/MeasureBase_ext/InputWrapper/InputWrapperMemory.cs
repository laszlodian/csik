using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace e77.MeasureBase.InputWrapper
{
    public class InputWrapperMemory : InputWrapperBase
    {
        List<string>    _Lines;
        int             _NextLine = 0;

        public void InitMemory(List<string> lines_in)
        {
            Trace.TraceInformation("Input Wrapper: Init Memory");

            _Lines = lines_in;
            _NextLine = 0;
        }

        public override string Name
        {
            get { return string.Empty; }
        }

        protected override string ReadLineInternal()
        {
            if (_NextLine > _Lines.Count)
                return null;
            else
                return _Lines[_NextLine++];
        }
    }
}
