using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuilder;

public class CssClassEventArgs : EventArgs
{
    public CssClassEventArgs(ICssClassBuilder builder)
    {
        Builder = builder;
    }

    public ICssClassBuilder Builder { get; }
}
