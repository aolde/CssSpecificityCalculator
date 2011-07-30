namespace CssSpecificityTool.LabConsole {
    public class SelectorTest {
        public string Selector { get; set; }
        public SelectorSpecificityScore Score { get; set; }

        public SelectorTest(string selector, int scoreA, int scoreB, int scoreC) {
            Selector = selector;
            Score = new SelectorSpecificityScore(scoreA, scoreB, scoreC);
        }
    }
}