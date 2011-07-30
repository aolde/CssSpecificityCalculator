using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CssSpecificityTool.LabConsole {
    public class StylesheetParser {
        private string _styleSheetContent;

        private StylesheetParser() {}

        public static StylesheetParser FromFile(string file) {
            var stylesheetParser = new StylesheetParser();
            
            TextReader textReader = new StreamReader(file);
            stylesheetParser._styleSheetContent = textReader.ReadToEnd();
            textReader.Close();
            
            return stylesheetParser;
        }

        public static StylesheetParser FromContent(string content) {
            var stylesheetParser = new StylesheetParser { _styleSheetContent = content };
            return stylesheetParser;
        }


        public List<StyleBlock> Parse() {
            const string STYLE_BLOCK_PATTERN = @"\{([^}]*)\}";
            const string COMMENT_PATTERN = @"/\*.+?\*/";

            // remove new lines and tabs
            _styleSheetContent = _styleSheetContent.Replace(Environment.NewLine, String.Empty).Replace("\t", String.Empty);

            // strip comments
            _styleSheetContent = Regex.Replace(_styleSheetContent, COMMENT_PATTERN, String.Empty, RegexOptions.Singleline);

            // remove content in bracets
            List<StyleBlock> selectors = new List<StyleBlock>();
            MatchCollection blockMatches = Regex.Matches(_styleSheetContent, STYLE_BLOCK_PATTERN);

            _styleSheetContent = Regex.Replace(_styleSheetContent, STYLE_BLOCK_PATTERN, Environment.NewLine, RegexOptions.Singleline);

            var selectorLines = _styleSheetContent.Split(new [] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < selectorLines.Length; i++) {
                var selectorLine = selectorLines[i].Split(',').Select(s => s.Trim()).ToArray();
                var block = FormatBlockContent(blockMatches[i].Value);

                selectors.Add(new StyleBlock(selectorLine, block));
            }

            return selectors;
        }

        private static string FormatBlockContent(string blockContent) {
            var rules = blockContent.Trim(' ', '{', '}').Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            foreach (var rule in rules) {
                sb.AppendLine(rule + ";");
            }
            return sb.ToString();
        }
    }
}