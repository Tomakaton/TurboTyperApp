using System;
using System.Collections.Generic;

namespace TurboTyper.MVVM.Model;

/// <summary>
/// Generates word lines for typing sessions without exceeding per-line character budgets.
/// </summary>
public static class WordProvider
{
    private static readonly string[] WordPool =
    [
        "about", "above", "across", "after", "again", "against", "almost", "alone", "along", "already",
        "always", "among", "animal", "another", "answer", "around", "arrive", "asked", "away", "back",
        "balance", "beacon", "became", "because", "become", "before", "began", "begin", "behind", "below",
        "better", "between", "beyond", "black", "board", "bottom", "branch", "bread", "bridge", "bright",
        "bring", "brother", "build", "cabin", "called", "camera", "candle", "cannot", "canvas", "carbon",
        "carry", "castle", "catch", "center", "chance", "change", "charge", "circle", "clean", "clear",
        "climb", "close", "cloud", "coffee", "cold", "color", "common", "corner", "could", "course",
        "cover", "craft", "create", "creek", "crown", "crystal", "dance", "danger", "dark", "dawn",
        "decide", "deep", "desert", "design", "detail", "differ", "direct", "doctor", "dollar", "double",
        "dream", "drift", "drive", "eagle", "early", "earth", "east", "echo", "edge", "effort",
        "eight", "either", "ember", "energy", "engine", "enough", "enter", "equal", "even", "evening",
        "event", "every", "exact", "example", "except", "expect", "export", "fabric", "factor", "fallen",
        "family", "famous", "fast", "father", "field", "figure", "filter", "final", "finger", "finish",
        "flame", "flight", "float", "floor", "flower", "focus", "follow", "forest", "forget", "former",
        "found", "frame", "friend", "front", "frozen", "future", "galaxy", "garden", "gather", "gentle",
        "giant", "glance", "glass", "global", "golden", "ground", "group", "growth", "guard", "guess",
        "guide", "hammer", "handle", "happen", "harbor", "hard", "heart", "heavy", "height", "hidden",
        "history", "hollow", "home", "honest", "horizon", "horse", "house", "human", "hundred", "hungry",
        "island", "jacket", "journey", "jungle", "keep", "kernel", "kettle", "keyboard", "kind", "kitchen",
        "knight", "ladder", "lantern", "large", "later", "laugh", "launch", "layer", "learn", "letter"
    ];

    public static string GenerateLine(int maxChars = 52)
    {
        var words = new List<string>();
        int currentLength = 0;
        string? previousWord = null;
        int retries = 0;
        const int maxRetries = 100;

        while (currentLength < maxChars && retries < maxRetries)
        {
            string candidate = PickNextWord(previousWord);
            int addedLength = (words.Count > 0 ? 1 : 0) + candidate.Length;

            if (currentLength + addedLength <= maxChars)
            {
                words.Add(candidate);
                currentLength += addedLength;
                previousWord = candidate;
                retries = 0;
            }
            else
            {
                retries++;
            }
        }

        if (words.Count == 0)
        {
            words.Add(WordPool[Random.Shared.Next(WordPool.Length)]);
        }

        return string.Join(" ", words);
    }

    public static List<string> GenerateLinesForWordBudget(int totalWords, int maxCharsPerLine = 52)
    {
        var lines = new List<string>();
        if (totalWords <= 0) return lines;

        var currentLineWords = new List<string>();
        int currentLength = 0;
        string? previousWord = null;

        for (int i = 0; i < totalWords; i++)
        {
            string word = PickNextWord(previousWord);
            previousWord = word;

            int addedLength = (currentLineWords.Count > 0 ? 1 : 0) + word.Length;
            if (currentLineWords.Count > 0 && currentLength + addedLength > maxCharsPerLine)
            {
                lines.Add(string.Join(" ", currentLineWords));
                currentLineWords.Clear();
                currentLength = 0;
                addedLength = word.Length;
            }

            currentLineWords.Add(word);
            currentLength += addedLength;
        }

        if (currentLineWords.Count > 0)
        {
            lines.Add(string.Join(" ", currentLineWords));
        }

        return lines;
    }

    private static string PickNextWord(string? previousWord)
    {
        string candidate;
        do
        {
            candidate = WordPool[Random.Shared.Next(WordPool.Length)];
        }
        while (candidate == previousWord && WordPool.Length > 1);

        return candidate;
    }
}
