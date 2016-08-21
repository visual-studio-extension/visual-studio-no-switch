using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;

namespace VisualStudio.NoSwitch.Tag
{
    [Export(typeof(ITaggerProvider))]
    [ContentType("code")]
    [TagType(typeof(NoSwitchTag))]
    class NoSwitchTagProvider : ITaggerProvider
    {

        [Import]
        internal ITextSearchService2 TextSearchService { get; set; }
        
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return new NoSwitchTagger(buffer, TextSearchService) as ITagger<T>;
        }
    }
    
}
