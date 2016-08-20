using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using System.Linq;
using VisualStudio.NoSwitch.Mapper;

namespace VisualStudio.NoSwitch.Tag
{
    class State
    {
        public bool ActivateThai { set; get; }
        public int StartPosition { set; get; }
    }

    class NoSwitchTagger : ITagger<NoSwitchTag>
    {
        private State _state = new State();

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        private ITextSearchService2 _searchService;

        private ITextBuffer _buffer;

        public NoSwitchTagger(ITextBuffer buffer, ITextSearchService2 search)
        {
            _buffer = buffer;
            _searchService = search;
        }

        private void Release(SnapshotSpan span)
        {
            //ReplaceWith(span, '~', "");
            _state.ActivateThai = false;
            _state.StartPosition = 0;
        }

        private void Start(int location)
        {
            _state.ActivateThai = true;
            _state.StartPosition = location;
        }

        private void ReplaceWith(SnapshotSpan span, char replace, string with)
        {
            var edit = _buffer.CreateEdit();
            edit.Replace(_state.StartPosition, replace, with);
            edit.Apply();
        }

        private void Replace(SnapshotSpan span, string replaceWith)
        {
            var edit = _buffer.CreateEdit();
            edit.Replace(span, replaceWith);
            edit.Apply();
        }


        public IEnumerable<ITagSpan<NoSwitchTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            try
            {
                return TryGetTags(spans);

            }
            catch (Exception ex)
            {
                return Enumerable.Empty<ITagSpan<NoSwitchTag>>();
            }
        }

        public IEnumerable<ITagSpan<NoSwitchTag>> TryGetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (SnapshotSpan currentSpan in spans)
            {
                var containingLine = currentSpan.Start.GetContainingLine();
                var location = containingLine.Start.Position;
                var line = containingLine.GetText();

                var start = line.IndexOf('~');
                if(start != -1)
                {
                    _state.StartPosition = start + location;
                    _state.ActivateThai = true;
                }

                foreach (var token in line.ToArray())
                {
                    var activateThai = _state.ActivateThai;
                    var startPosition = _state.StartPosition;

                    if (token == '~')
                    {
                        if (activateThai)
                        {
                            if (location > startPosition)
                            {
                                //var span = new SnapshotSpan(currentSpan.Snapshot, new Span(startPosition, location - startPosition));
                                //Release(span);
                            }
                        }
                        else
                        {
                            //Start(location);
                        }
                    }
                    else
                    {
                        if (activateThai && location > startPosition)
                        {
                            var thai = KeyboardMap.GetThaiChar(token);

                            if (thai != token)
                            {
                                var tokenSpan = new SnapshotSpan(currentSpan.Snapshot, new Span(location, 1));
                                Replace(tokenSpan, thai.ToString());

                            }
                            var startSpan = new SnapshotSpan(currentSpan.Snapshot, new Span(startPosition, location));
                            var tag = new NoSwitchTag(NoSwitchTaggerTypes.Start);
                            var newSpan = new TagSpan<NoSwitchTag>(startSpan, tag);
                            yield return newSpan;
                        }
                    }
                    location++;
                }
            }
        }
    }
}
