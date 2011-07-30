namespace CssSpecificityCalculator.LabConsole {
    public class StyleBlock {
        public string[] Selectors { get; set; }
        public string BlockContent { get; set; }

        public StyleBlock(string[] selectors, string blockContent) {
            Selectors = selectors;
            BlockContent = blockContent;
        }
    }
}