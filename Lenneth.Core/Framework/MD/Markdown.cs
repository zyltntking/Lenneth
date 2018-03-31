﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using static System.String;

namespace Lenneth.Core.Framework.MD
{
    /// <summary>
    /// Markdown is a text-to-HTML conversion tool for web writers. 
    /// Markdown allows you to write using an easy-to-read, easy-to-write plain text format, 
    /// then convert it to structurally valid XHTML (or HTML).
    /// </summary>
    public class Markdown
    {
        public string Version => "1.13";

        #region Constructors and Options

        /// <inheritdoc />
        /// <summary>
        /// Create a new Markdown instance using default options
        /// </summary>
        public Markdown() : this(false)
        {
        }

        /// <summary>
        /// Create a new Markdown instance and optionally load options from a configuration
        /// file. There they should be stored in the appSettings section, available options are:
        /// 
        ///     Markdown.StrictBoldItalic (true/false)
        ///     Markdown.EmptyElementSuffix (">" or " />" without the quotes)
        ///     Markdown.LinkEmails (true/false)
        ///     Markdown.AutoNewLines (true/false)
        ///     Markdown.AutoHyperlink (true/false)
        ///     Markdown.EncodeProblemUrlCharacters (true/false) 
        ///     
        /// </summary>
        public Markdown(bool loadOptionsFromConfigFile)
        {
            if (!loadOptionsFromConfigFile) return;

            var settings = ConfigurationManager.AppSettings;
            foreach (string key in settings.Keys)
            {
                switch (key)
                {
                    case "Markdown.AutoHyperlink":
                        AutoHyperlink = Convert.ToBoolean(settings[key]);
                        break;
                    case "Markdown.AutoNewlines":
                        AutoNewlines = Convert.ToBoolean(settings[key]);
                        break;
                    case "Markdown.EmptyElementSuffix":
                        EmptyElementSuffix = settings[key];
                        break;
                    case "Markdown.EncodeProblemUrlCharacters":
                        EncodeProblemUrlCharacters = Convert.ToBoolean(settings[key]);
                        break;
                    case "Markdown.LinkEmails":
                        LinkEmails = Convert.ToBoolean(settings[key]);
                        break;
                    case "Markdown.StrictBoldItalic":
                        StrictBoldItalic = Convert.ToBoolean(settings[key]);
                        break;
                }
            }
        }

        /// <summary>
        /// Create a new Markdown instance and set the options from the MarkdownOptions object.
        /// </summary>
        public Markdown(MarkdownOptions options)
        {
            AutoHyperlink = options.AutoHyperlink;
            AutoNewlines = options.AutoNewlines;
            EmptyElementSuffix = options.EmptyElementSuffix;
            EncodeProblemUrlCharacters = options.EncodeProblemUrlCharacters;
            LinkEmails = options.LinkEmails;
            StrictBoldItalic = options.StrictBoldItalic;
        }

        /// <summary>
        /// use ">" for HTML output, or " />" for XHTML output
        /// </summary>
        private string EmptyElementSuffix { get;} = " />";

        /// <summary>
        /// when false, email addresses will never be auto-linked  
        /// WARNING: this is a significant deviation from the markdown spec
        /// </summary>
        private bool LinkEmails { get; } = true;

        /// <summary>
        /// when true, bold and italic require non-word characters on either side  
        /// WARNING: this is a significant deviation from the markdown spec
        /// </summary>
        private bool StrictBoldItalic { get; }

        /// <summary>
        /// when true, RETURN becomes a literal newline  
        /// WARNING: this is a significant deviation from the markdown spec
        /// </summary>
        private bool AutoNewlines { get; }

        /// <summary>
        /// when true, (most) bare plain URLs are auto-hyperlinked  
        /// WARNING: this is a significant deviation from the markdown spec
        /// </summary>
        private bool AutoHyperlink { get; }

        /// <summary>
        /// when true, problematic URL characters like [, ], (, and so forth will be encoded 
        /// WARNING: this is a significant deviation from the markdown spec
        /// </summary>
        private bool EncodeProblemUrlCharacters { get; }

        #endregion

        private enum TokenType { Text, Tag }

        private struct Token
        {
            public Token(TokenType type, string value)
            {
                Type = type;
                Value = value;
            }
            public readonly TokenType Type;
            public readonly string Value;
        }

        /// <summary>
        /// maximum nested depth of [] and () supported by the transform; implementation detail
        /// </summary>
        private const int NestDepth = 6;

        /// <summary>
        /// Tabs are automatically converted to spaces as part of the transform  
        /// this constant determines how "wide" those tabs become in spaces  
        /// </summary>
        private const int TabWidth = 4;

        private const string MarkerUl = @"[*+-]";
        private const string MarkerOl = @"\d+[.]";

        private static readonly Dictionary<string, string> EscapeTable;
        private static readonly Dictionary<string, string> InvertedEscapeTable;
        private static readonly Dictionary<string, string> BackslashEscapeTable;        

        private readonly Dictionary<string, string> _urls = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _titles = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _htmlBlocks = new Dictionary<string, string>();

        private int _listLevel;

        /// <summary>
        /// In the static constuctor we'll initialize what stays the same across all transforms.
        /// </summary>
        static Markdown()
        {
            // Table of hash values for escaped characters:
            EscapeTable = new Dictionary<string, string>();
            InvertedEscapeTable = new Dictionary<string, string>();
            // Table of hash value for backslash escaped characters:
            BackslashEscapeTable = new Dictionary<string, string>();

            var backslashPattern = "";

            foreach (var c in @"\`*_{}[]()>#+-.!")
            {
                var key = c.ToString();
                var hash = GetHashKey(key);
                EscapeTable.Add(key, hash);
                InvertedEscapeTable.Add(hash, key);
                BackslashEscapeTable.Add(@"\" + key, hash);
                backslashPattern += Regex.Escape(@"\" + key) + "|";
            }

            BackslashEscapes = new Regex(backslashPattern.Substring(0, backslashPattern.Length - 1), RegexOptions.Compiled);
        }

        /// <summary>
        /// Transforms the provided Markdown-formatted text to HTML;  
        /// see http://en.wikipedia.org/wiki/Markdown
        /// </summary>
        /// <remarks>
        /// The order in which other subs are called here is
        /// essential. Link and image substitutions need to happen before
        /// EscapeSpecialChars(), so that any *'s or _'s in the a
        /// and img tags get encoded.
        /// </remarks>
        public string Transform(string text)
        {
            if (IsNullOrEmpty(text)) return "";

            Setup();

            text = Normalize(text);
           
            text = HashHtmlBlocks(text);
            text = StripLinkDefinitions(text);
            text = RunBlockGamut(text);
            text = Unescape(text);

            Cleanup();

            return text + "\n";
        }


        /// <summary>
        /// Perform transformations that form block-level tags like paragraphs, headers, and list items.
        /// </summary>
        private string RunBlockGamut(string text)
        {
            text = DoHeaders(text);
            text = DoHorizontalRules(text);
            text = DoLists(text);
            text = DoCodeBlocks(text);
            text = DoBlockQuotes(text);

            // We already ran HashHTMLBlocks() before, in Markdown(), but that
            // was to escape raw HTML in the original Markdown source. This time,
            // we're escaping the markup we've just created, so that we don't wrap
            // <p> tags around block-level tags.
            text = HashHtmlBlocks(text);

            text = FormParagraphs(text);

            return text;
        }


        /// <summary>
        /// Perform transformations that occur *within* block-level tags like paragraphs, headers, and list items.
        /// </summary>
        private string RunSpanGamut(string text)
        {
            text = DoCodeSpans(text);
            text = EscapeSpecialCharsWithinTagAttributes(text);
            text = EscapeBackslashes(text);

            // Images must come first, because ![foo][f] looks like an anchor.
            text = DoImages(text);
            text = DoAnchors(text);

            // Must come after DoAnchors(), because you can use < and >
            // delimiters in inline links like [this](<url>).
            text = DoAutoLinks(text);

            text = EncodeAmpsAndAngles(text);
            text = DoItalicsAndBold(text);
            text = DoHardBreaks(text);

            return text;
        }

        private static readonly Regex NewlinesLeadingTrailing = new Regex(@"^\n+|\n+\z", RegexOptions.Compiled);
        private static readonly Regex NewlinesMultiple = new Regex(@"\n{2,}", RegexOptions.Compiled);
        private static readonly Regex LeadingWhitespace = new Regex(@"^[ ]*", RegexOptions.Compiled);

        /// <summary>
        /// splits on two or more newlines, to form "paragraphs";    
        /// each paragraph is then unhashed (if it is a hash) or wrapped in HTML p tag
        /// </summary>
        private string FormParagraphs(string text)
        {
            // split on two or more newlines
            var grafs = NewlinesMultiple.Split(NewlinesLeadingTrailing.Replace(text, ""));
            
            for (var i = 0; i < grafs.Length; i++)
            {
                if (grafs[i].StartsWith("\x1A"))
                {
                    // unhashify HTML blocks
                    grafs[i] = _htmlBlocks[grafs[i]];
                }
                else
                {
                    // do span level processing inside the block, then wrap result in <p> tags
                    grafs[i] = LeadingWhitespace.Replace(RunSpanGamut(grafs[i]), "<p>") + "</p>";
                }
            }

            return Join("\n\n", grafs);
        }


        private void Setup()
        {
            // Clear the global hashes. If we don't clear these, you get conflicts
            // from other articles when generating a page which contains more than
            // one article (e.g. an index page that shows the N most recent
            // articles):
            _urls.Clear();
            _titles.Clear();
            _htmlBlocks.Clear();
            _listLevel = 0;
        }

        private void Cleanup() => Setup();

        private static string _nestedBracketsPattern;

        /// <summary>
        /// Reusable pattern to match balanced [brackets]. See Friedl's 
        /// "Mastering Regular Expressions", 2nd Ed., pp. 328-331.
        /// </summary>
        private static string GetNestedBracketsPattern() => _nestedBracketsPattern ?? (_nestedBracketsPattern = RepeatString(@"
                    (?>              # Atomic matching
                       [^\[\]]+      # Anything other than brackets
                     |
                       \[
                           ", NestDepth) + RepeatString( @" \]
                    )*" , NestDepth));

        private static string _nestedParensPattern;

        /// <summary>
        /// Reusable pattern to match balanced (parens). See Friedl's 
        /// "Mastering Regular Expressions", 2nd Ed., pp. 328-331.
        /// </summary>
        private static string GetNestedParensPattern() => _nestedParensPattern ?? (_nestedParensPattern = RepeatString(@"
                    (?>              # Atomic matching
                       [^()\s]+      # Anything other than parens or whitespace
                     |
                       \(
                           ", NestDepth) + RepeatString( @" \)
                    )*" , NestDepth));

        private static readonly Regex LinkDef = new Regex($@"
                        ^[ ]{{0,{TabWidth - 1}}}\[(.+)\]:  # id = $1
                          [ ]*
                          \n?                   # maybe *one* newline
                          [ ]*
                        <?(\S+?)>?              # url = $2
                          [ ]*
                          \n?                   # maybe one newline
                          [ ]*
                        (?:
                            (?<=\s)             # lookbehind for whitespace
                            [""(]
                            (.+?)               # title = $3
                            ["")]
                            [ ]*
                        )?                      # title is optional
                        (?:\n+|\Z)", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        /// <summary>
        /// Strips link definitions from text, stores the URLs and titles in hash references.
        /// </summary>
        /// <remarks>
        /// ^[id]: url "optional title"
        /// </remarks>
        private string StripLinkDefinitions(string text) => LinkDef.Replace(text, LinkEvaluator);

        private string LinkEvaluator(Match match)
        {
            var linkId = match.Groups[1].Value.ToLowerInvariant();
            _urls[linkId] = EncodeAmpsAndAngles(match.Groups[2].Value);

            if (match.Groups[3].Length > 0)
                _titles[linkId] = match.Groups[3].Value.Replace("\"", "&quot;");

            return "";
        }

        // compiling this monster regex results in worse performance. trust me.
        private static readonly Regex BlocksHtml = new Regex(GetBlockPattern(), RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);


        /// <summary>
        /// derived pretty much verbatim from PHP Markdown
        /// </summary>
        private static string GetBlockPattern()
        {

            // Hashify HTML blocks:
            // We only want to do this for block-level HTML tags, such as headers,
            // lists, and tables. That's because we still want to wrap <p>s around
            // "paragraphs" that are wrapped in non-block-level tags, such as anchors,
            // phrase emphasis, and spans. The list of tags we're looking for is
            // hard-coded:
            //
            // *  List "a" is made of tags which can be both inline or block-level.
            //    These will be treated block-level when the start tag is alone on 
            //    its line, otherwise they're not matched here and will be taken as 
            //    inline later.
            // *  List "b" is made of tags which are always block-level;
            //
            var blockTagsA = "ins|del";
            var blockTagsB = "p|div|h[1-6]|blockquote|pre|table|dl|ol|ul|address|script|noscript|form|fieldset|iframe|math";

            // Regular expression for the content of a block tag.
            var attr = @"
            (?>				            # optional tag attributes
              \s			            # starts with whitespace
              (?>
                [^>""/]+	            # text outside quotes
              |
                /+(?!>)		            # slash not followed by >
              |
                ""[^""]*""		        # text inside double quotes (tolerate >)
              |
                '[^']*'	                # text inside single quotes (tolerate >)
              )*
            )?	
            ";

            var content = RepeatString(@"
                (?>
                  [^<]+			        # content without tag
                |
                  <\2			        # nested opening tag
                    " + attr + @"       # attributes
                  (?>
                      />
                  |
                      >", NestDepth) +   // end of opening tag
                      ".*?" +             // last level nested tag content
            RepeatString(@"
                      </\2\s*>	        # closing nested tag
                  )
                  |				
                  <(?!/\2\s*>           # other tags with a different name
                  )
                )*", NestDepth);

            var content2 = content.Replace(@"\2", @"\3");

            // First, look for nested blocks, e.g.:
            // 	<div>
            // 		<div>
            // 		tags for inner block must be indented.
            // 		</div>
            // 	</div>
            //
            // The outermost tags must start at the left margin for this to match, and
            // the inner nested divs must be indented.
            // We need to do this before the next, more liberal match, because the next
            // match will start at the first `<div>` and stop at the first `</div>`.
            var pattern = @"
            (?>
                  (?>
                    (?<=\n)     # Starting after a blank line
                    |           # or
                    \A\n?       # the beginning of the doc
                  )
                  (             # save in $1

                    # Match from `\n<tag>` to `</tag>\n`, handling nested tags 
                    # in between.
                      
                        [ ]{0,$less_than_tab}
                        <($block_tags_b_re)   # start tag = $2
                        $attr>                # attributes followed by > and \n
                        $content              # content, support nesting
                        </\2>                 # the matching end tag
                        [ ]*                  # trailing spaces
                        (?=\n+|\Z)            # followed by a newline or end of document

                  | # Special version for tags of group a.

                        [ ]{0,$less_than_tab}
                        <($block_tags_a_re)   # start tag = $3
                        $attr>[ ]*\n          # attributes followed by >
                        $content2             # content, support nesting
                        </\3>                 # the matching end tag
                        [ ]*                  # trailing spaces
                        (?=\n+|\Z)            # followed by a newline or end of document
                      
                  | # Special case just for <hr />. It was easier to make a special 
                    # case than to make the other regex more complicated.
                  
                        [ ]{0,$less_than_tab}
                        <(hr)                 # start tag = $2
                        $attr                 # attributes
                        /?>                   # the matching end tag
                        [ ]*
                        (?=\n{2,}|\Z)         # followed by a blank line or end of document
                  
                  | # Special case for standalone HTML comments:
                  
                      [ ]{0,$less_than_tab}
                      (?s:
                        <!-- .*? -->
                      )
                      [ ]*
                      (?=\n{2,}|\Z)            # followed by a blank line or end of document
                  
                  | # PHP and ASP-style processor instructions (<? and <%)
                  
                      [ ]{0,$less_than_tab}
                      (?s:
                        <([?%])                # $2
                        .*?
                        \2>
                      )
                      [ ]*
                      (?=\n{2,}|\Z)            # followed by a blank line or end of document
                      
                  )
            )";

            pattern = pattern.Replace("$less_than_tab", (TabWidth - 1).ToString());
            pattern = pattern.Replace("$block_tags_b_re", blockTagsB);
            pattern = pattern.Replace("$block_tags_a_re", blockTagsA);
            pattern = pattern.Replace("$attr", attr);
            pattern = pattern.Replace("$content2", content2);
            pattern = pattern.Replace("$content", content);

            return pattern;
        }

        /// <summary>
        /// replaces any block-level HTML blocks with hash entries
        /// </summary>
        private string HashHtmlBlocks(string text) => BlocksHtml.Replace(text, HtmlEvaluator);

        private string HtmlEvaluator(Match match)
        {
            var text = match.Groups[1].Value;
            var key = GetHashKey(text);
            _htmlBlocks[key] = text;

            return Concat("\n\n", key, "\n\n");
        }

        private static string GetHashKey(string s) => "\x1A" + Math.Abs(s.GetHashCode()).ToString() + "\x1A";

        private static readonly Regex HtmlTokens = new Regex(@"
            (<!(?:--.*?--\s*)+>)|        # match <!-- foo -->
            (<\?.*?\?>)|                 # match <?foo?> " +
            RepeatString(@" 
            (<[A-Za-z\/!$](?:[^<>]|", NestDepth) + RepeatString(@")*>)", NestDepth) +
                                       " # match <tag> and </tag>",
            RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        /// <summary>
        /// returns an array of HTML tokens comprising the input string. Each token is 
        /// either a tag (possibly with nested, tags contained therein, such 
        /// as &lt;a href="&lt;MTFoo&gt;"&gt;, or a run of text between tags. Each element of the 
        /// array is a two-element array; the first is either 'tag' or 'text'; the second is 
        /// the actual value.
        /// </summary>
        private IEnumerable<Token> TokenizeHTML(string text)
        {
            var pos = 0;
            var tokens = new List<Token>();

            // this regex is derived from the _tokenize() subroutine in Brad Choate's MTRegex plugin.
            // http://www.bradchoate.com/past/mtregex.php
            foreach (Match m in HtmlTokens.Matches(text))
            {
                var tagStart = m.Index;

                if (pos < tagStart)
                    tokens.Add(new Token(TokenType.Text, text.Substring(pos, tagStart - pos)));

                tokens.Add(new Token(TokenType.Tag, m.Value));
                pos = tagStart + m.Length;
            }

            if (pos < text.Length)
                tokens.Add(new Token(TokenType.Text, text.Substring(pos, text.Length - pos)));

            return tokens;
        }


        private static readonly Regex AnchorRef = new Regex($@"
            (                               # wrap whole match in $1
                \[
                    ({GetNestedBracketsPattern()})                   # link text = $2
                \]

                [ ]?                        # one optional space
                (?:\n[ ]*)?                 # one optional newline followed by spaces

                \[
                    (.*?)                   # id = $3
                \]
            )", RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        private static readonly Regex AnchorInline = new Regex($@"
                (                           # wrap whole match in $1
                    \[
                        ({GetNestedBracketsPattern()})               # link text = $2
                    \]
                    \(                      # literal paren
                        [ ]*
                        ({GetNestedParensPattern()})               # href = $3
                        [ ]*
                        (                   # $4
                        (['""])           # quote char = $5
                        (.*?)               # title = $6
                        \5                  # matching quote
                        [ ]*                # ignore any spaces between closing quote and )
                        )?                  # title is optional
                    \)
                )",
                  RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        private static readonly Regex AnchorRefShortcut = new Regex(@"
            (                               # wrap whole match in $1
              \[
                 ([^\[\]]+)                 # link text = $2; can't contain [ or ]
              \]
            )", RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        /// <summary>
        /// Turn Markdown link shortcuts into HTML anchor tags
        /// </summary>
        /// <remarks>
        /// [link text](url "title") 
        /// [link text][id] 
        /// [id] 
        /// </remarks>
        private string DoAnchors(string text)
        {
            // First, handle reference-style links: [link text] [id]
            text = AnchorRef.Replace(text, AnchorRefEvaluator);

            // Next, inline-style links: [link text](url "optional title") or [link text](url "optional title")
            text = AnchorInline.Replace(text, AnchorInlineEvaluator);

            //  Last, handle reference-style shortcuts: [link text]
            //  These must come last in case you've also got [link test][1]
            //  or [link test](/foo)
            text = AnchorRefShortcut.Replace(text, AnchorRefShortcutEvaluator);
            return text;
        }

        private string AnchorRefEvaluator(Match match)
        {
            var wholeMatch = match.Groups[1].Value;
            var linkText = match.Groups[2].Value;
            var linkId = match.Groups[3].Value.ToLowerInvariant();

            string result;

            // for shortcut links like [this][].
            if (linkId == "")
                linkId = linkText.ToLowerInvariant();

            if (_urls.ContainsKey(linkId))
            {
                var url = _urls[linkId];

                url = EncodeProblemUrlChars(url);
                url = EscapeBoldItalic(url);                
                result = "<a href=\"" + url + "\"";

                if (_titles.ContainsKey(linkId))
                {
                    var title = _titles[linkId];
                    title = EscapeBoldItalic(title);
                    result += " title=\"" + title + "\"";
                }

                result += ">" + linkText + "</a>";
            }
            else
                result = wholeMatch;

            return result;
        }

        private string AnchorRefShortcutEvaluator(Match match)
        {
            var wholeMatch = match.Groups[1].Value;
            var linkText = match.Groups[2].Value;
            var linkId = Regex.Replace(linkText.ToLowerInvariant(), @"[ ]*\n[ ]*", " ");  // lower case and remove newlines / extra spaces

            string result;

            if (_urls.ContainsKey(linkId))
            {
                var url = _urls[linkId];

                url = EncodeProblemUrlChars(url);
                url = EscapeBoldItalic(url);                
                result = "<a href=\"" + url + "\"";

                if (_titles.ContainsKey(linkId))
                {
                    var title = _titles[linkId];
                    title = EscapeBoldItalic(title);
                    result += " title=\"" + title + "\"";
                }

                result += ">" + linkText + "</a>";
            }
            else
                result = wholeMatch;

            return result;
        }


        private string AnchorInlineEvaluator(Match match)
        {
            var linkText = match.Groups[2].Value;
            var url = match.Groups[3].Value;
            var title = match.Groups[6].Value;

            url = EncodeProblemUrlChars(url);
            url = EscapeBoldItalic(url);
            if (url.StartsWith("<") && url.EndsWith(">"))
                url = url.Substring(1, url.Length - 2); // remove <>'s surrounding URL, if present            

            var result = $"<a href=\"{url}\"";

            if (!IsNullOrEmpty(title))
            {
                title = title.Replace("\"", "&quot;");
                title = EscapeBoldItalic(title);
                result += $" title=\"{title}\"";
            }

            result += $">{linkText}</a>";
            return result;
        }

        private static readonly Regex ImagesRef = new Regex(@"
                    (               # wrap whole match in $1
                    !\[
                        (.*?)       # alt text = $2
                    \]

                    [ ]?            # one optional space
                    (?:\n[ ]*)?     # one optional newline followed by spaces

                    \[
                        (.*?)       # id = $3
                    \]

                    )", RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex ImagesInline = new Regex($@"
              (                     # wrap whole match in $1
                !\[
                    (.*?)           # alt text = $2
                \]
                \s?                 # one optional whitespace character
                \(                  # literal paren
                    [ ]*
                    ({GetNestedParensPattern()})           # href = $3
                    [ ]*
                    (               # $4
                    (['""])       # quote char = $5
                    (.*?)           # title = $6
                    \5              # matching quote
                    [ ]*
                    )?              # title is optional
                \)
              )",
                  RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// Turn Markdown image shortcuts into HTML img tags. 
        /// </summary>
        /// <remarks>
        /// ![alt text][id]
        /// ![alt text](url "optional title")
        /// </remarks>
        private string DoImages(string text)
        {
            // First, handle reference-style labeled images: ![alt text][id]
            text = ImagesRef.Replace(text, ImageReferenceEvaluator);

            // Next, handle inline images:  ![alt text](url "optional title")
            // Don't forget: encode * and _
            text = ImagesInline.Replace(text, ImageInlineEvaluator);

            return text;
        }

        private string ImageReferenceEvaluator(Match match)
        {
            var wholeMatch = match.Groups[1].Value;
            var altText = match.Groups[2].Value;
            var linkId = match.Groups[3].Value.ToLowerInvariant();
            string result;

            // for shortcut links like ![this][].
            if (linkId == "")
                linkId = altText.ToLowerInvariant();

            altText = altText.Replace("\"", "&quot;");

            if (_urls.ContainsKey(linkId))
            {
                var url = _urls[linkId];
                url = EncodeProblemUrlChars(url);
                url = EscapeBoldItalic(url);                
                result = $"<img src=\"{url}\" alt=\"{altText}\"";

                if (_titles.ContainsKey(linkId))
                {
                    var title = _titles[linkId];
                    title = EscapeBoldItalic(title);

                    result += $" title=\"{title}\"";
                }

                result += EmptyElementSuffix;
            }
            else
            {
                // If there's no such link ID, leave intact:
                result = wholeMatch;
            }

            return result;
        }

        private string ImageInlineEvaluator(Match match)
        {
            var alt = match.Groups[2].Value;
            var url = match.Groups[3].Value;
            var title = match.Groups[6].Value;

            alt = alt.Replace("\"", "&quot;");
            title = title.Replace("\"", "&quot;");
            
            if (url.StartsWith("<") && url.EndsWith(">"))
                url = url.Substring(1, url.Length - 2);    // Remove <>'s surrounding URL, if present
            url = EncodeProblemUrlChars(url);
            url = EscapeBoldItalic(url);

            var result = $"<img src=\"{url}\" alt=\"{alt}\"";

            if (!IsNullOrEmpty(title))
            {
                title = EscapeBoldItalic(title);
                result += $" title=\"{title}\"";
            }

            result += EmptyElementSuffix;

            return result;
        }

        private static readonly Regex HeaderSetext = new Regex(@"
                ^(.+?)
                [ ]*
                \n
                (=+|-+)     # $1 = string of ='s or -'s
                [ ]*
                \n+",
            RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        private static readonly Regex HeaderAtx = new Regex(@"
                ^(\#{1,6})  # $1 = string of #'s
                [ ]*
                (.+?)       # $2 = Header text
                [ ]*
                \#*         # optional closing #'s (not counted)
                \n+",
            RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        /// <summary>
        /// Turn Markdown headers into HTML header tags
        /// </summary>
        /// <remarks>
        /// Header 1  
        /// ========  
        /// 
        /// Header 2  
        /// --------  
        /// 
        /// # Header 1  
        /// ## Header 2  
        /// ## Header 2 with closing hashes ##  
        /// ...  
        /// ###### Header 6  
        /// </remarks>
        private string DoHeaders(string text)
        {
            text = HeaderSetext.Replace(text, SetextHeaderEvaluator);
            text = HeaderAtx.Replace(text, AtxHeaderEvaluator);
            return text;
        }

        private string SetextHeaderEvaluator(Match match)
        {
            var header = match.Groups[1].Value;
            var level = match.Groups[2].Value.StartsWith("=") ? 1 : 2;
            return Format("<h{1}>{0}</h{1}>\n\n", RunSpanGamut(header), level);
        }

        private string AtxHeaderEvaluator(Match match)
        {
            var header = match.Groups[2].Value;
            var level = match.Groups[1].Value.Length;
            return $"<h{level}>{RunSpanGamut(header)}</h{level}>\n\n";
        }


        private static readonly Regex HorizontalRules = new Regex(@"
            ^[ ]{0,3}         # Leading space
                ([-*_])       # $1: First marker
                (?>           # Repeated marker group
                    [ ]{0,2}  # Zero, one, or two spaces.
                    \1        # Marker character
                ){2,}         # Group repeated at least twice
                [ ]*          # Trailing spaces
                $             # End of line.
            ", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        /// <summary>
        /// Turn Markdown horizontal rules into HTML hr tags
        /// </summary>
        /// <remarks>
        /// ***  
        /// * * *  
        /// ---
        /// - - -
        /// </remarks>
        private string DoHorizontalRules(string text) => HorizontalRules.Replace(text, "<hr" + EmptyElementSuffix + "\n");

        private static readonly string WholeList = $@"
            (                               # $1 = whole list
              (                             # $2
                [ ]{{0,{TabWidth - 1}}}
                ({$"(?:{MarkerUl}|{MarkerOl})"})                       # $3 = first list item marker
                [ ]+
              )
              (?s:.+?)
              (                             # $4
                  \z
                |
                  \n{{2,}}
                  (?=\S)
                  (?!                       # Negative lookahead for another list item marker
                    [ ]*
                    {$"(?:{MarkerUl}|{MarkerOl})"}[ ]+
                  )
              )
            )";

        private static readonly Regex ListNested = new Regex(@"^" + WholeList,
            RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        private static readonly Regex ListTopLevel = new Regex(@"(?:(?<=\n\n)|\A\n?)" + WholeList,
            RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        /// <summary>
        /// Turn Markdown lists into HTML ul and ol and li tags
        /// </summary>
        private string DoLists(string text)
        {
            // We use a different prefix before nested lists than top-level lists.
            // See extended comment in _ProcessListItems().
            text = _listLevel > 0 ? ListNested.Replace(text, ListEvaluator) : ListTopLevel.Replace(text, ListEvaluator);

            return text;
        }

        private string ListEvaluator(Match match)
        {
            var list = match.Groups[1].Value;
            var listType = Regex.IsMatch(match.Groups[3].Value, MarkerUl) ? "ul" : "ol";

            // Turn double returns into triple returns, so that we can make a
            // paragraph for the last item in a list, if necessary:
            list = Regex.Replace(list, @"\n{2,}", "\n\n\n");
            var result = ProcessListItems(list, listType == "ul" ? MarkerUl : MarkerOl);

            result = $"<{listType}>\n{result}</{listType}>\n";
            return result;
        }

        /// <summary>
        /// Process the contents of a single ordered or unordered list, splitting it
        /// into individual list items.
        /// </summary>
        private string ProcessListItems(string list, string marker)
        {
            // The listLevel global keeps track of when we're inside a list.
            // Each time we enter a list, we increment it; when we leave a list,
            // we decrement. If it's zero, we're not in a list anymore.

            // We do this because when we're not inside a list, we want to treat
            // something like this:

            //    I recommend upgrading to version
            //    8. Oops, now this line is treated
            //    as a sub-list.

            // As a single paragraph, despite the fact that the second line starts
            // with a digit-period-space sequence.

            // Whereas when we're inside a list (or sub-list), that line will be
            // treated as the start of a sub-list. What a kludge, huh? This is
            // an aspect of Markdown's syntax that's hard to parse perfectly
            // without resorting to mind-reading. Perhaps the solution is to
            // change the syntax rules such that sub-lists must start with a
            // starting cardinal number; e.g. "1." or "a.".

            _listLevel++;

            // Trim trailing blank lines:
            list = Regex.Replace(list, @"\n{2,}\z", "\n");

            var pattern = $@"(\n)?                      # leading line = $1
                (^[ ]*)                    # leading whitespace = $2
                ({marker}) [ ]+                 # list marker = $3
                ((?s:.+?)                  # list item text = $4
                (\n{{1,2}}))      
                (?= \n* (\z | \2 ({marker}) [ ]+))";

            list = Regex.Replace(list, pattern, ListItemEvaluator, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            _listLevel--;
            return list;
        }

        private string ListItemEvaluator(Match match)
        {
            var item = match.Groups[4].Value;
            var leadingLine = match.Groups[1].Value;

            if (!IsNullOrEmpty(leadingLine) || Regex.IsMatch(item, @"\n{2,}"))
                // we could correct any bad indentation here..
                item = RunBlockGamut(Outdent(item) + "\n");
            else
            {
                // recursion for sub-lists
                item = DoLists(Outdent(item));
                item = item.TrimEnd('\n');
                item = RunSpanGamut(item);
            }

            return $"<li>{item}</li>\n";
        }


        private static readonly Regex CodeBlock = new Regex($@"
                    (?:\n\n|\A\n?)
                    (                        # $1 = the code block -- one or more lines, starting with a space
                    (?:
                        (?:[ ]{{{TabWidth}}})       # Lines must start with a tab-width of spaces
                        .*\n+
                    )+
                    )
                    ((?=^[ ]{{0,{TabWidth}}}\S)|\Z) # Lookahead for non-space at line-start, or end of doc", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        /// <summary>
        /// /// Turn Markdown 4-space indented code into HTML pre code blocks
        /// </summary>
        private string DoCodeBlocks(string text)
        {
            text = CodeBlock.Replace(text, CodeBlockEvaluator);
            return text;
        }

        private string CodeBlockEvaluator(Match match)
        {
            var codeBlock = match.Groups[1].Value;

            codeBlock = EncodeCode(Outdent(codeBlock));
            codeBlock = NewlinesLeadingTrailing.Replace(codeBlock, "");

            return Concat("\n\n<pre><code>", codeBlock, "\n</code></pre>\n\n");
        }

        private static readonly Regex CodeSpan = new Regex(@"
                    (?<!\\)   # Character before opening ` can't be a backslash
                    (`+)      # $1 = Opening run of `
                    (.+?)     # $2 = The code block
                    (?<!`)
                    \1
                    (?!`)", RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// Turn Markdown `code spans` into HTML code tags
        /// </summary>
        private string DoCodeSpans(string text) => CodeSpan.Replace(text, CodeSpanEvaluator);
        //private string DoCodeSpans(string text)
        //{
        //    //    * You can use multiple backticks as the delimiters if you want to
        //    //        include literal backticks in the code span. So, this input:
        //    //
        //    //        Just type ``foo `bar` baz`` at the prompt.
        //    //
        //    //        Will translate to:
        //    //
        //    //          <p>Just type <code>foo `bar` baz</code> at the prompt.</p>
        //    //
        //    //        There's no arbitrary limit to the number of backticks you
        //    //        can use as delimters. If you need three consecutive backticks
        //    //        in your code, use four for delimiters, etc.
        //    //
        //    //    * You can use spaces to get literal backticks at the edges:
        //    //
        //    //          ... type `` `bar` `` ...
        //    //
        //    //        Turns to:
        //    //
        //    //          ... type <code>`bar`</code> ...         
        //    //

        //    return CodeSpan.Replace(text, CodeSpanEvaluator);
        //}

        private string CodeSpanEvaluator(Match match)
        {
            var span = match.Groups[2].Value;
            span = Regex.Replace(span, @"^[ ]*", ""); // leading whitespace
            span = Regex.Replace(span, @"[ ]*$", ""); // trailing whitespace
            span = EncodeCode(span);

            return Concat("<code>", span, "</code>");
        }


        private static readonly Regex Bold = new Regex(@"(\*\*|__) (?=\S) (.+?[*_]*) (?<=\S) \1", RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex StrictBold = new Regex(@"([\W_]|^) (\*\*|__) (?=\S) ([^\r]*?\S[\*_]*) \2 ([\W_]|$)", RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex Italic = new Regex(@"(\*|_) (?=\S) (.+?) (?<=\S) \1",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex StrictItalic = new Regex(@"([\W_]|^) (\*|_) (?=\S) ([^\r\*_]*?\S) \2 ([\W_]|$)",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// Turn Markdown *italics* and **bold** into HTML strong and em tags
        /// </summary>
        private string DoItalicsAndBold(string text)
        {

            // <strong> must go first, then <em>
            if (StrictBoldItalic)
            {
                text = StrictBold.Replace(text, "$1<strong>$3</strong>$4");
                text = StrictItalic.Replace(text, "$1<em>$3</em>$4");
            }
            else
            {
                text = Bold.Replace(text, "<strong>$2</strong>");
                text = Italic.Replace(text, "<em>$2</em>");
            }
            return text;
        }

        /// <summary>
        /// Turn markdown line breaks (two space at end of line) into HTML break tags
        /// </summary>
        private string DoHardBreaks(string text)
        {
            text = Regex.Replace(text, AutoNewlines ? @"\n" : @" {2,}\n", $"<br{EmptyElementSuffix}\n");
            return text;
        }

        private static readonly Regex Blockquote = new Regex(@"
            (                           # Wrap whole match in $1
                (
                ^[ ]*>[ ]?              # '>' at the start of a line
                    .+\n                # rest of the first line
                (.+\n)*                 # subsequent consecutive lines
                \n*                     # blanks
                )+
            )", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// Turn Markdown > quoted blocks into HTML blockquote blocks
        /// </summary>
        private string DoBlockQuotes(string text) => Blockquote.Replace(text, BlockQuoteEvaluator);

        private string BlockQuoteEvaluator(Match match)
        {
            var bq = match.Groups[1].Value;

            bq = Regex.Replace(bq, @"^[ ]*>[ ]?", "", RegexOptions.Multiline);       // trim one level of quoting
            bq = Regex.Replace(bq, @"^[ ]+$", "", RegexOptions.Multiline);           // trim whitespace-only lines
            bq = RunBlockGamut(bq);                                                  // recurse

            bq = Regex.Replace(bq, @"^", "  ", RegexOptions.Multiline);

            // These leading spaces screw with <pre> content, so we need to fix that:
            bq = Regex.Replace(bq, @"(\s*<pre>.+?</pre>)", BlockQuoteEvaluator2, RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);

            return $"<blockquote>\n{bq}\n</blockquote>\n\n";
        }

        private string BlockQuoteEvaluator2(Match match) => Regex.Replace(match.Groups[1].Value, @"^  ", "", RegexOptions.Multiline);

        private static readonly Regex AutolinkBare = new Regex(@"(^|\s)(https?|ftp)(://[-A-Z0-9+&@#/%?=~_|\[\]\(\)!:,\.;]*[-A-Z0-9+&@#/%=~_|\[\]])($|\W)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Turn angle-delimited URLs into HTML anchor tags
        /// </summary>
        /// <remarks>
        /// &lt;http://www.example.com&gt;
        /// </remarks>
        private string DoAutoLinks(string text)
        {

            if (AutoHyperlink)
            {
                // fixup arbitrary URLs by adding Markdown < > so they get linked as well
                // note that at this point, all other URL in the text are already hyperlinked as <a href=""></a>
                // *except* for the <http://www.foo.com> case
                text = AutolinkBare.Replace(text, @"$1<$2$3>$4");
            }

            // Hyperlinks: <http://foo.com>
            text = Regex.Replace(text, "<((https?|ftp):[^'\">\\s]+)>", HyperlinkEvaluator);

            if (!LinkEmails) return text;
            // Email addresses: <address@domain.foo>
            var pattern =
                @"<
                      (?:mailto:)?
                      (
                        [-.\w]+
                        \@
                        [-a-z0-9]+(\.[-a-z0-9]+)*\.[a-z]+
                      )
                      >";
            text = Regex.Replace(text, pattern, EmailEvaluator, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

            return text;
        }

        private string HyperlinkEvaluator(Match match)
        {
            var link = match.Groups[1].Value;
            return $"<a href=\"{link}\">{link}</a>";
        }

        private string EmailEvaluator(Match match)
        {
            var email = Unescape(match.Groups[1].Value);

            //
            //    Input: an email address, e.g. "foo@example.com"
            //
            //    Output: the email address as a mailto link, with each character
            //            of the address encoded as either a decimal or hex entity, in
            //            the hopes of foiling most address harvesting spam bots. E.g.:
            //
            //      <a href="&#x6D;&#97;&#105;&#108;&#x74;&#111;:&#102;&#111;&#111;&#64;&#101;
            //        x&#x61;&#109;&#x70;&#108;&#x65;&#x2E;&#99;&#111;&#109;">&#102;&#111;&#111;
            //        &#64;&#101;x&#x61;&#109;&#x70;&#108;&#x65;&#x2E;&#99;&#111;&#109;</a>
            //
            //    Based by a filter by Matthew Wickline, posted to the BBEdit-Talk
            //    mailing list: <http://tinyurl.com/yu7ue>
            //
            email = "mailto:" + email;

            // leave ':' alone (to spot mailto: later) 
            email = EncodeEmailAddress(email);

            email = $"<a href=\"{email}\">{email}</a>";

            // strip the mailto: from the visible part
            email = Regex.Replace(email, "\">.+?:", "\">");
            return email;
        }


        private static readonly Regex OutDent = new Regex(@"^[ ]{1," + TabWidth + @"}", RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// Remove one level of line-leading spaces
        /// </summary>
        private string Outdent(string block) => OutDent.Replace(block, "");

        #region Encoding and Normalization


        /// <summary>
        /// encodes email address randomly  
        /// roughly 10% raw, 45% hex, 45% dec 
        /// note that @ is always encoded and : never is
        /// </summary>
        private string EncodeEmailAddress(string addr)
        {
            var sb = new StringBuilder(addr.Length * 5);
            var rand = new Random();
            foreach (var c in addr)
            {
                var r = rand.Next(1, 100);
                if ((r > 90 || c == ':') && c != '@')
                    sb.Append(c);                         // m
                else if (r < 45)
                    sb.AppendFormat("&#x{0:x};", (int)c); // &#x6D
                else
                    sb.AppendFormat("&#{0};", (int)c);    // &#109
            }
            return sb.ToString();
        }

        private static readonly Regex CodeEncoder = new Regex(@"&|<|>|\\|\*|_|\{|\}|\[|\]", RegexOptions.Compiled);

        /// <summary>
        /// Encode/escape certain Markdown characters inside code blocks and spans where they are literals
        /// </summary>
        private string EncodeCode(string code) => CodeEncoder.Replace(code, EncodeCodeEvaluator);

        private string EncodeCodeEvaluator(Match match)
        {
            switch (match.Value)
            {
                // Encode all ampersands; HTML entities are not
                // entities within a Markdown code span.
                case "&":
                    return "&amp;";
                // Do the angle bracket song and dance
                case "<":
                    return "&lt;";
                case ">":
                    return "&gt;";
                // escape characters that are magic in Markdown
                default:
                    return EscapeTable[match.Value];
            }
        }


        private static readonly Regex Amps = new Regex(@"&(?!(#[0-9]+)|(#[xX][a-fA-F0-9])|([a-zA-Z][a-zA-Z0-9]*);)", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
        private static readonly Regex Angles = new Regex(@"<(?![A-Za-z/?\$!])", RegexOptions.ExplicitCapture | RegexOptions.Compiled);

        /// <summary>
        /// Encode any ampersands (that aren't part of an HTML entity) and left or right angle brackets
        /// </summary>
        private string EncodeAmpsAndAngles(string s)
        {
            s = Amps.Replace(s, "&amp;");
            s = Angles.Replace(s, "&lt;");
            return s;
        }

        private static readonly Regex BackslashEscapes; 

        /// <summary>
        /// Encodes any escaped characters such as \`, \*, \[ etc
        /// </summary>
        private string EscapeBackslashes(string s) => BackslashEscapes.Replace(s, EscapeBackslashesEvaluator);

        private string EscapeBackslashesEvaluator(Match match) => BackslashEscapeTable[match.Value];
       
        private static readonly Regex Unescapes = new Regex("\x1A\\d+\x1A", RegexOptions.Compiled);

        /// <summary>
        /// swap back in all the special characters we've hidden
        /// </summary>
        private string Unescape(string s) => Unescapes.Replace(s, UnescapeEvaluator);

        private string UnescapeEvaluator(Match match) => InvertedEscapeTable[match.Value];


        /// <summary>
        /// escapes Bold [ * ] and Italic [ _ ] characters
        /// </summary>
        private string EscapeBoldItalic(string s)
        {
            s = s.Replace("*", EscapeTable["*"]);
            s = s.Replace("_", EscapeTable["_"]);
            return s;
        }

        private static readonly char[] ProblemUrlChars = @"""'*()[]$:".ToCharArray();

        /// <summary>
        /// hex-encodes some unusual "problem" chars in URLs to avoid URL detection problems 
        /// </summary>
        private string EncodeProblemUrlChars(string url)
        {
            if (!EncodeProblemUrlCharacters) return url;

            var sb = new StringBuilder(url.Length);

            for (var i = 0; i < url.Length; i++)
            {
                var c = url[i];
                var encode = Array.IndexOf(ProblemUrlChars, c) != -1;
                if (encode && c == ':' && i < url.Length - 1)
                    encode = url[i + 1] != '/' && !(url[i + 1] >= '0' && url[i + 1] <= '9');

                if (encode)
                    sb.Append("%" + $"{(byte) c:x}");
                else
                    sb.Append(c);                
            }

            return sb.ToString();
        }


        /// <summary>
        /// Within tags -- meaning between &lt; and &gt; -- encode [\ ` * _] so they 
        /// don't conflict with their use in Markdown for code, italics and strong. 
        /// We're replacing each such character with its corresponding hash 
        /// value; this is likely overkill, but it should prevent us from colliding 
        /// with the escape values by accident.
        /// </summary>
        private string EscapeSpecialCharsWithinTagAttributes(string text)
        {
            var tokens = TokenizeHTML(text);

            // now, rebuild text from the tokens
            var sb = new StringBuilder(text.Length);

            foreach (var token in tokens)
            {
                var value = token.Value;

                if (token.Type == TokenType.Tag)
                {
                    value = value.Replace(@"\", EscapeTable[@"\"]);
                    value = Regex.Replace(value, "(?<=.)</?code>(?=.)", EscapeTable[@"`"]);
                    value = EscapeBoldItalic(value);
                }

                sb.Append(value);
            }

            return sb.ToString();
        }

        /// <summary>
        /// convert all tabs to _tabWidth spaces; 
        /// standardizes line endings from DOS (CR LF) or Mac (CR) to UNIX (LF); 
        /// makes sure text ends with a couple of newlines; 
        /// removes any blank lines (only spaces) in the text
        /// </summary>
        private string Normalize(string text)
        {            
            var output = new StringBuilder(text.Length);
            var line = new StringBuilder();
            var valid = false;

            for (var i = 0; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case '\n':
                        if (valid) output.Append(line);
                        output.Append('\n');
                        line.Length = 0; valid = false;
                        break;
                    case '\r':
                        if ((i < text.Length - 1) && (text[i + 1] != '\n'))
                        {
                            if (valid) output.Append(line);
                            output.Append('\n');
                            line.Length = 0; valid = false;
                        }
                        break;
                    case '\t':
                        var width = (TabWidth - line.Length % TabWidth);
                        for (var k = 0; k < width; k++)
                            line.Append(' ');
                        break;
                    case '\x1A':
                        break;
                    default:
                        if (!valid && text[i] != ' ') valid = true;
                        line.Append(text[i]);
                        break;
                }
            }

            if (valid) output.Append(line);
            output.Append('\n');

            // add two newlines to the end before return
            return output.Append("\n\n").ToString();
        }

        #endregion

        /// <summary>
        /// this is to emulate what's evailable in PHP
        /// </summary>
        private static string RepeatString(string text, int count)
        {
            var sb = new StringBuilder(text.Length * count);
            for (var i = 0; i < count; i++)
                sb.Append(text);
            return sb.ToString();
        }

    }
}