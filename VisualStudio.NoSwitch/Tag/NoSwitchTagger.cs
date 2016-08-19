using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using System.Linq;

namespace VisualStudio.NoSwitch.Tag
{
    class NoSwitchTagger : ITagger<NoSwitchTag>
    {
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        private ITextSearchService2 _searchService;

        private ITextBuffer _buffer;

        public NoSwitchTagger(ITextBuffer buffer, ITextSearchService2 search)
        {
            _buffer = buffer;
            _searchService = search;
        }

        public IEnumerable<ITagSpan<NoSwitchTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (SnapshotSpan currentSpan in spans)
            {
                var containingLine = currentSpan.Start.GetContainingLine();
                var location = containingLine.Start.Position;
                var line = containingLine.GetText();
                var tokens = line.Split(' ');

                foreach (string token in tokens)
                {
                    if (token == "%%")
                    {
                        var index = line.IndexOf(token);
                        var tokenSpan = new SnapshotSpan(currentSpan.Snapshot, new Span(location, token.Length));

                        //var rs = _searchService.FindAllForReplace(currentSpan, "%", "$", FindOptions.MatchCase);
                        var edit = _buffer.CreateEdit();
                        edit.Replace(tokenSpan, "$");
                        edit.Apply();

                        var tag = new NoSwitchTag(NoSwitchTaggerTypes.Start);
                        var newSpan = new TagSpan<NoSwitchTag>(tokenSpan, tag);
                        yield return newSpan;
                    }

                    location += token.Length + 1;
                }
            }
        }
    }
}
