using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;

namespace VisualStudio.NoSwitch.Tag
{
    [Export(typeof(ITaggerProvider))]
    [ContentType("text")]
    [TagType(typeof(NoSwitchTag))]
    class NoSwitchTagProvider : ITaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return new NoSwitchTagger(buffer) as ITagger<T>;
        }
    }
    
}
