using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CssSpecificityCalculator.LabConsole {
    public class SelectorSpecificityScore : IComparable<SelectorSpecificityScore> {

        /// <param name="scoreA">count the number of ID attributes in the selector</param>
        /// <param name="scoreB">count the number of other attributes and pseudo-classes in the selector</param>
        /// <param name="scoreC">count the number of element names and pseudo-elements in the selector</param>
        public SelectorSpecificityScore(int scoreA, int scoreB, int scoreC) {
            ScoreA = scoreA;
            ScoreB = scoreB;
            ScoreC = scoreC;
        }

        public SelectorSpecificityScore(string selector) {
            Selector = selector;
            CalculateScore(selector);
        }

        public SelectorSpecificityScore() {}

        public string Selector { get; set; }

        /// <summary>
        /// Count the number of ID attributes in the selector.
        /// </summary>
        public int ScoreA { get; set; }
        /// <summary>
        /// Count the number of other attributes and pseudo-classes in the selector.
        /// </summary>
        public int ScoreB { get; set; }
        /// <summary>
        /// Count the number of element names and pseudo-elements in the selector.
        /// </summary>
        public int ScoreC { get; set; }

        public int TotalScore {
            get { return (ScoreA * 10000) + (ScoreB * 100) + ScoreC; }
        }

        public int CompareTo(SelectorSpecificityScore other) {
            if (ScoreA > other.ScoreA)
                return 1;
            if (ScoreA < other.ScoreA)
                return -1;
            
            if (ScoreB > other.ScoreB)
                return 1;
            if (ScoreB < other.ScoreB)
                return -1;
            
            if (ScoreC > other.ScoreC)
                return 1;
            if (ScoreC < other.ScoreC)
                return -1;
            
            return 0;
        }

        public override string ToString() {
            return string.Format("{0},{1},{2}", ScoreA, ScoreB, ScoreC);
        }

        private void CalculateScore(string selector) {
            // normalize CSS3's pseudo-element to CSS version 2.
            // and remove :not as it shouldn't be counted
            selector = selector.Replace("::", ":")
                .Replace(":not(", " ");

            PseudoClassAndPseudoElementCalculation(selector);

            ElementsCalculation(selector);

            IdAndClassCalculation(selector);
        }

        private void PseudoClassAndPseudoElementCalculation(string selector) {
            const string PSEUDO_PATTERN = @":[\w\-]+";
            string[] pseudoElements = new[] { ":first-line", ":first-letter", ":before", ":after", ":selection" };
            Regex regex = new Regex(PSEUDO_PATTERN);

            MatchCollection matches = regex.Matches(selector);
            foreach (Match match in matches) {
                var pseudoSelector = match.Value;

                // is a pseudo-element
                if (pseudoElements.Contains(pseudoSelector))
                    ScoreC++;
                else // assume it's a pseudo-class
                    ScoreB++;
            }
        }

        private void ElementsCalculation(string selector) {
            // split up the selector to be able to count elements
            var parts = selector.Split(new[] {' ', '+', '>', '+', '~', '*'}, StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in parts) {
                // starts with a letter then it's an element
                if (Char.IsLetter(part[0]))
                    ScoreC++;
            }
        }

        private void IdAndClassCalculation(string selector) {
            foreach (char character in selector) {
                // id
                if (character == '#')
                    ScoreA++;

                // class or attribute
                if (character == '.' || character == '[')
                    ScoreB++;
            }
        }
    }
}