using System;
using System.Collections.Generic;

using System.Text;

namespace Nepy.Core
{
    public class UnicodeBlock
    {
        static Dictionary<string, UnicodeBlock> map = new Dictionary<string, UnicodeBlock>();

        static UnicodeBlock()
        {
        }
        private UnicodeBlock(string idName):this(idName, (string[])null)
        {

        }
        private UnicodeBlock(string idName, string aliasName):this(idName,new string[]{aliasName})
        {
            
        }
        private string idName;
        private UnicodeBlock(string idName, string[] aliasName)
        {
            if (map.ContainsKey(idName.ToUpper()))
            {
                throw new ArgumentException(string.Format("UnicodeBlock name '{0}' already exists", idName));
            }
            this.idName = idName;
            map.Add(idName.ToUpper(), this);
            if (aliasName != null)
            {
                for (int x = 0; x < aliasName.Length; ++x)
                {
                    string name = aliasName[x].ToUpper();
                    if (map.ContainsKey(name))
                    {
                        throw new ArgumentException(string.Format("UnicodeBlock alias name '{0}' already exists", aliasName));
                    }
                    map.Add(name, this);
                }
            }
        }
        public override string ToString()
        {
            return idName;
        }
        public static UnicodeBlock BASIC_LATIN = new UnicodeBlock("BASIC_LATIN", new String[] { "Basic Latin", "BasicLatin" });
        public static UnicodeBlock LATIN_1_SUPPLEMENT = new UnicodeBlock("LATIN_1_SUPPLEMENT", new String[] { "Latin-1 Supplement", "Latin-1Supplement" });
        public static UnicodeBlock LATIN_EXTENDED_A = new UnicodeBlock("LATIN_EXTENDED_A", new String[] { "Latin Extended-A", "LatinExtended-A" });
        public static UnicodeBlock LATIN_EXTENDED_B = new UnicodeBlock("LATIN_EXTENDED_B", new String[] { "Latin Extended-B", "LatinExtended-B" });
        public static UnicodeBlock IPA_EXTENSIONS = new UnicodeBlock("IPA_EXTENSIONS", new String[] { "IPA Extensions", "IPAExtensions" });
        public static UnicodeBlock SPACING_MODIFIER_LETTERS = new UnicodeBlock("SPACING_MODIFIER_LETTERS", new String[] { "Spacing Modifier Letters",
                                                                        "SpacingModifierLetters"});
        public static UnicodeBlock COMBINING_DIACRITICAL_MARKS = new UnicodeBlock("COMBINING_DIACRITICAL_MARKS", new String[] {"Combining Diacritical Marks",
                                                                          "CombiningDiacriticalMarks" });
        public static UnicodeBlock GREEK = new UnicodeBlock("GREEK", new String[] { "Greek and Coptic", "GreekandCoptic" });
        public static UnicodeBlock CYRILLIC = new UnicodeBlock("CYRILLIC");
        public static UnicodeBlock ARMENIAN = new UnicodeBlock("ARMENIAN");
        public static UnicodeBlock HEBREW = new UnicodeBlock("HEBREW");
        public static UnicodeBlock ARABIC = new UnicodeBlock("ARABIC");
        public static UnicodeBlock DEVANAGARI = new UnicodeBlock("DEVANAGARI");
        public static UnicodeBlock BENGALI = new UnicodeBlock("BENGALI");
        public static UnicodeBlock GURMUKHI = new UnicodeBlock("GURMUKHI");
        public static UnicodeBlock GUJARATI = new UnicodeBlock("GUJARATI");
        public static UnicodeBlock ORIYA = new UnicodeBlock("ORIYA");
        public static UnicodeBlock TAMIL = new UnicodeBlock("TAMIL");
        public static UnicodeBlock TELUGU = new UnicodeBlock("TELUGU");
        public static UnicodeBlock KANNADA = new UnicodeBlock("KANNADA");
        public static UnicodeBlock MALAYALAM = new UnicodeBlock("MALAYALAM");
        public static UnicodeBlock THAI = new UnicodeBlock("THAI");
        public static UnicodeBlock LAO = new UnicodeBlock("LAO");
        public static UnicodeBlock TIBETAN =  new UnicodeBlock("TIBETAN");
        public static UnicodeBlock GEORGIAN = new UnicodeBlock("GEORGIAN");

        public static UnicodeBlock HANGUL_JAMO = new UnicodeBlock("HANGUL_JAMO", new String[] { "Hangul Jamo", "HangulJamo" });
        public static UnicodeBlock LATIN_EXTENDED_ADDITIONAL = new UnicodeBlock("LATIN_EXTENDED_ADDITIONAL", new String[] {"Latin Extended Additional",
                                                                        "LatinExtendedAdditional"});
        public static UnicodeBlock GREEK_EXTENDED = new UnicodeBlock("GREEK_EXTENDED", new String[] { "Greek Extended", "GreekExtended" });
        public static UnicodeBlock GENERAL_PUNCTUATION = new UnicodeBlock("GENERAL_PUNCTUATION", new String[] { "General Punctuation", "GeneralPunctuation" });
        public static UnicodeBlock SUPERSCRIPTS_AND_SUBSCRIPTS = new UnicodeBlock("SUPERSCRIPTS_AND_SUBSCRIPTS", new String[] {"Superscripts and Subscripts",
                                                                          "SuperscriptsandSubscripts" });
        public static UnicodeBlock CURRENCY_SYMBOLS = new UnicodeBlock("CURRENCY_SYMBOLS", new String[] { "Currency Symbols", "CurrencySymbols" });
        public static UnicodeBlock COMBINING_MARKS_FOR_SYMBOLS = new UnicodeBlock("COMBINING_MARKS_FOR_SYMBOLS", new String[] 
                                                                           {"Combining Diacritical Marks for Symbols",
                                                                          "CombiningDiacriticalMarksforSymbols",
                                                                          "Combining Marks for Symbols",
                                                                          "CombiningMarksforSymbols" });
        public static UnicodeBlock LETTERLIKE_SYMBOLS = new UnicodeBlock("LETTERLIKE_SYMBOLS", new String[] { "Letterlike Symbols", "LetterlikeSymbols" });
        public static UnicodeBlock NUMBER_FORMS = new UnicodeBlock("NUMBER_FORMS", new String[] { "Number Forms", "NumberForms" });
        public static UnicodeBlock ARROWS = new UnicodeBlock("ARROWS");
        public static UnicodeBlock MATHEMATICAL_OPERATORS = new UnicodeBlock("MATHEMATICAL_OPERATORS", new String[] {"Mathematical Operators",
                                                                     "MathematicalOperators"});
        public static UnicodeBlock MISCELLANEOUS_TECHNICAL = new UnicodeBlock("MISCELLANEOUS_TECHNICAL", new String[] {"Miscellaneous Technical",
                                                                      "MiscellaneousTechnical"});
        public static UnicodeBlock CONTROL_PICTURES = new UnicodeBlock("CONTROL_PICTURES", new String[] { "Control Pictures", "ControlPictures" });
        public static UnicodeBlock OPTICAL_CHARACTER_RECOGNITION = new UnicodeBlock("OPTICAL_CHARACTER_RECOGNITION", new String[] {"Optical Character Recognition",
                                                                            "OpticalCharacterRecognition"});
        public static UnicodeBlock ENCLOSED_ALPHANUMERICS = new UnicodeBlock("ENCLOSED_ALPHANUMERICS", new String[] {"Enclosed Alphanumerics",
                                                                     "EnclosedAlphanumerics"});
        public static UnicodeBlock BOX_DRAWING = new UnicodeBlock("BOX_DRAWING", new String[] { "Box Drawing", "BoxDrawing" });
        public static UnicodeBlock BLOCK_ELEMENTS = new UnicodeBlock("BLOCK_ELEMENTS", new String[] { "Block Elements", "BlockElements" });
        public static UnicodeBlock GEOMETRIC_SHAPES = new UnicodeBlock("GEOMETRIC_SHAPES", new String[] { "Geometric Shapes", "GeometricShapes" });
        public static UnicodeBlock MISCELLANEOUS_SYMBOLS = new UnicodeBlock("MISCELLANEOUS_SYMBOLS", new String[] {"Miscellaneous Symbols",
                                                                    "MiscellaneousSymbols"});
        public static UnicodeBlock DINGBATS = new UnicodeBlock("DINGBATS");
        public static UnicodeBlock CJK_SYMBOLS_AND_PUNCTUATION = new UnicodeBlock("CJK_SYMBOLS_AND_PUNCTUATION", new String[] {"CJK Symbols and Punctuation",
                                                                          "CJKSymbolsandPunctuation"});
        public static UnicodeBlock HIRAGANA = new UnicodeBlock("HIRAGANA");
        public static UnicodeBlock KATAKANA = new UnicodeBlock("KATAKANA");
        public static UnicodeBlock BOPOMOFO = new UnicodeBlock("BOPOMOFO");
        public static UnicodeBlock HANGUL_COMPATIBILITY_JAMO = new UnicodeBlock("HANGUL_COMPATIBILITY_JAMO", new String[] {"Hangul Compatibility Jamo",
                                                                        "HangulCompatibilityJamo"});
        public static UnicodeBlock KANBUN = new UnicodeBlock("KANBUN");
        public static UnicodeBlock ENCLOSED_CJK_LETTERS_AND_MONTHS = new UnicodeBlock("ENCLOSED_CJK_LETTERS_AND_MONTHS", new String[] {"Enclosed CJK Letters and Months",
                                                                              "EnclosedCJKLettersandMonths"});
        public static UnicodeBlock CJK_COMPATIBILITY = new UnicodeBlock("CJK_COMPATIBILITY", new String[] { "CJK Compatibility", "CJKCompatibility" });
        public static UnicodeBlock CJK_UNIFIED_IDEOGRAPHS = new UnicodeBlock("CJK_UNIFIED_IDEOGRAPHS", new String[] {"CJK Unified Ideographs",
                                                                     "CJKUnifiedIdeographs"});
        public static UnicodeBlock HANGUL_SYLLABLES = new UnicodeBlock("HANGUL_SYLLABLES", new String[] { "Hangul Syllables", "HangulSyllables" });
        public static UnicodeBlock PRIVATE_USE_AREA = new UnicodeBlock("PRIVATE_USE_AREA", new String[] { "Private Use Area", "PrivateUseArea" });
        public static UnicodeBlock CJK_COMPATIBILITY_IDEOGRAPHS = new UnicodeBlock("CJK_COMPATIBILITY_IDEOGRAPHS",
                             new String[] {"CJK Compatibility Ideographs",
                                           "CJKCompatibilityIdeographs"});
        public static UnicodeBlock ALPHABETIC_PRESENTATION_FORMS = new UnicodeBlock("ALPHABETIC_PRESENTATION_FORMS", new String[] {"Alphabetic Presentation Forms",
                                                                            "AlphabeticPresentationForms"});
        public static UnicodeBlock ARABIC_PRESENTATION_FORMS_A = new UnicodeBlock("ARABIC_PRESENTATION_FORMS_A", new String[] {"Arabic Presentation Forms-A",
                                                                          "ArabicPresentationForms-A"});
        public static UnicodeBlock COMBINING_HALF_MARKS = new UnicodeBlock("COMBINING_HALF_MARKS", new String[] {"Combining Half Marks",
                                                                   "CombiningHalfMarks"});
        public static UnicodeBlock CJK_COMPATIBILITY_FORMS = new UnicodeBlock("CJK_COMPATIBILITY_FORMS", new String[] {"CJK Compatibility Forms",
                                                                      "CJKCompatibilityForms"});
        public static UnicodeBlock SMALL_FORM_VARIANTS = new UnicodeBlock("SMALL_FORM_VARIANTS", new String[] {"Small Form Variants",
                                                                  "SmallFormVariants"});
        public static UnicodeBlock ARABIC_PRESENTATION_FORMS_B = new UnicodeBlock("ARABIC_PRESENTATION_FORMS_B", new String[] {"Arabic Presentation Forms-B",
                                                                          "ArabicPresentationForms-B"});
        public static UnicodeBlock HALFWIDTH_AND_FULLWIDTH_FORMS = new UnicodeBlock("HALFWIDTH_AND_FULLWIDTH_FORMS",
                             new String[] {"Halfwidth and Fullwidth Forms",
                                           "HalfwidthandFullwidthForms"});
        public static UnicodeBlock SPECIALS = new UnicodeBlock("SPECIALS");
        public static UnicodeBlock SYRIAC = new UnicodeBlock("SYRIAC");
        public static UnicodeBlock THAANA = new UnicodeBlock("THAANA");
        public static UnicodeBlock SINHALA = new UnicodeBlock("SINHALA");
        public static UnicodeBlock MYANMAR = new UnicodeBlock("MYANMAR");
        public static UnicodeBlock ETHIOPIC = new UnicodeBlock("ETHIOPIC");
        public static UnicodeBlock CHEROKEE = new UnicodeBlock("CHEROKEE");
        public static UnicodeBlock UNIFIED_CANADIAN_ABORIGINAL_SYLLABICS = new UnicodeBlock("UNIFIED_CANADIAN_ABORIGINAL_SYLLABICS",
                             new String[] {"Unified Canadian Aboriginal Syllabics",
                                           "UnifiedCanadianAboriginalSyllabics"});
        public static UnicodeBlock OGHAM = new UnicodeBlock("OGHAM");
        public static UnicodeBlock RUNIC = new UnicodeBlock("RUNIC");
        public static UnicodeBlock KHMER = new UnicodeBlock("KHMER");
        public static UnicodeBlock MONGOLIAN = new UnicodeBlock("MONGOLIAN");
        public static UnicodeBlock BRAILLE_PATTERNS = new UnicodeBlock("BRAILLE_PATTERNS", new String[] {"Braille Patterns",
                                                               "BraillePatterns"});
        public static UnicodeBlock CJK_RADICALS_SUPPLEMENT = new UnicodeBlock("CJK_RADICALS_SUPPLEMENT", new String[] {"CJK Radicals Supplement",
                                                                       "CJKRadicalsSupplement"});

        public static UnicodeBlock KANGXI_RADICALS = new UnicodeBlock("KANGXI_RADICALS", new String[] { "Kangxi Radicals", "KangxiRadicals" });
        public static UnicodeBlock IDEOGRAPHIC_DESCRIPTION_CHARACTERS = new UnicodeBlock("IDEOGRAPHIC_DESCRIPTION_CHARACTERS", 
                                                                                new String[] {"Ideographic Description Characters",
                                                                                 "IdeographicDescriptionCharacters"});
        public static UnicodeBlock BOPOMOFO_EXTENDED = new UnicodeBlock("BOPOMOFO_EXTENDED", new String[] {"Bopomofo Extended",
                                                                "BopomofoExtended"});
        public static UnicodeBlock CJK_UNIFIED_IDEOGRAPHS_EXTENSION_A = new UnicodeBlock("CJK_UNIFIED_IDEOGRAPHS_EXTENSION_A", new String[] {"CJK Unified Ideographs Extension A",
                                                                                 "CJKUnifiedIdeographsExtensionA"});
        public static UnicodeBlock YI_SYLLABLES = new UnicodeBlock("YI_SYLLABLES", new String[] { "Yi Syllables", "YiSyllables" });
        public static UnicodeBlock YI_RADICALS = new UnicodeBlock("YI_RADICALS", new String[] { "Yi Radicals", "YiRadicals" });
        public static UnicodeBlock CYRILLIC_SUPPLEMENTARY = new UnicodeBlock("CYRILLIC_SUPPLEMENTARY", new String[] {"Cyrillic Supplementary",
                                                                     "CyrillicSupplementary"});
        public static UnicodeBlock TAGALOG = new UnicodeBlock("TAGALOG");
        public static UnicodeBlock HANUNOO = new UnicodeBlock("HANUNOO");
        public static UnicodeBlock BUHID = new UnicodeBlock("BUHID");
        public static UnicodeBlock TAGBANWA = new UnicodeBlock("TAGBANWA");
        public static UnicodeBlock LIMBU = new UnicodeBlock("LIMBU");
        public static UnicodeBlock TAI_LE = new UnicodeBlock("TAI_LE", new String[] { "Tai Le", "TaiLe" });
        public static UnicodeBlock KHMER_SYMBOLS = new UnicodeBlock("KHMER_SYMBOLS", new String[] { "Khmer Symbols", "KhmerSymbols" });
        public static UnicodeBlock PHONETIC_EXTENSIONS = new UnicodeBlock("PHONETIC_EXTENSIONS", new String[] { "Phonetic Extensions", "PhoneticExtensions" });
        public static UnicodeBlock MISCELLANEOUS_MATHEMATICAL_SYMBOLS_A = new UnicodeBlock("MISCELLANEOUS_MATHEMATICAL_SYMBOLS_A",
                             new String[]{"Miscellaneous Mathematical Symbols-A",
                                          "MiscellaneousMathematicalSymbols-A"});
        public static UnicodeBlock SUPPLEMENTAL_ARROWS_A = new UnicodeBlock("SUPPLEMENTAL_ARROWS_A", new String[] {"Supplemental Arrows-A",
                                                                    "SupplementalArrows-A"});
        public static UnicodeBlock SUPPLEMENTAL_ARROWS_B = new UnicodeBlock("SUPPLEMENTAL_ARROWS_B", new String[] {"Supplemental Arrows-B",
                                                                    "SupplementalArrows-B"});
        public static UnicodeBlock MISCELLANEOUS_MATHEMATICAL_SYMBOLS_B = new UnicodeBlock("MISCELLANEOUS_MATHEMATICAL_SYMBOLS_B",
                                   new String[] {"Miscellaneous Mathematical Symbols-B",
                                                 "MiscellaneousMathematicalSymbols-B"});
        public static UnicodeBlock SUPPLEMENTAL_MATHEMATICAL_OPERATORS = new UnicodeBlock("SUPPLEMENTAL_MATHEMATICAL_OPERATORS",
                                     new String[]{"Supplemental Mathematical Operators",
                                                  "SupplementalMathematicalOperators"});
        public static UnicodeBlock MISCELLANEOUS_SYMBOLS_AND_ARROWS = new UnicodeBlock("MISCELLANEOUS_SYMBOLS_AND_ARROWS", 
                                    new String[] {"Miscellaneous Symbols and Arrows",
                                                                               "MiscellaneousSymbolsandArrows"});
        public static UnicodeBlock KATAKANA_PHONETIC_EXTENSIONS = new UnicodeBlock("KATAKANA_PHONETIC_EXTENSIONS", new String[] {"Katakana Phonetic Extensions",
                                                                           "KatakanaPhoneticExtensions"});
        public static UnicodeBlock YIJING_HEXAGRAM_SYMBOLS = new UnicodeBlock("YIJING_HEXAGRAM_SYMBOLS", new String[] {"Yijing Hexagram Symbols",
                                                                      "YijingHexagramSymbols"});
        public static UnicodeBlock VARIATION_SELECTORS = new UnicodeBlock("VARIATION_SELECTORS", new String[] { "Variation Selectors", "VariationSelectors" });
        public static UnicodeBlock LINEAR_B_SYLLABARY = new UnicodeBlock("LINEAR_B_SYLLABARY", new String[] { "Linear B Syllabary", "LinearBSyllabary" });
        public static UnicodeBlock LINEAR_B_IDEOGRAMS = new UnicodeBlock("LINEAR_B_IDEOGRAMS", new String[] { "Linear B Ideograms", "LinearBIdeograms" });
        public static UnicodeBlock AEGEAN_NUMBERS = new UnicodeBlock("AEGEAN_NUMBERS", new String[] { "Aegean Numbers", "AegeanNumbers" });
        public static UnicodeBlock OLD_ITALIC = new UnicodeBlock("OLD_ITALIC", new String[] { "Old Italic", "OldItalic" });
        public static UnicodeBlock GOTHIC = new UnicodeBlock("GOTHIC");
        public static UnicodeBlock UGARITIC = new UnicodeBlock("UGARITIC");
        public static UnicodeBlock DESERET = new UnicodeBlock("DESERET");

        public static UnicodeBlock SHAVIAN = new UnicodeBlock("SHAVIAN");
        public static UnicodeBlock OSMANYA = new UnicodeBlock("OSMANYA");
        public static UnicodeBlock CYPRIOT_SYLLABARY = new UnicodeBlock("CYPRIOT_SYLLABARY", new String[] { "Cypriot Syllabary", "CypriotSyllabary" });
        public static UnicodeBlock BYZANTINE_MUSICAL_SYMBOLS = new UnicodeBlock("BYZANTINE_MUSICAL_SYMBOLS", new String[] {"Byzantine Musical Symbols",
                                                                        "ByzantineMusicalSymbols"});
        public static UnicodeBlock MUSICAL_SYMBOLS = new UnicodeBlock("MUSICAL_SYMBOLS", new String[] { "Musical Symbols", "MusicalSymbols" });
        public static UnicodeBlock TAI_XUAN_JING_SYMBOLS = new UnicodeBlock("TAI_XUAN_JING_SYMBOLS", new String[] {"Tai Xuan Jing Symbols",
                                                                     "TaiXuanJingSymbols"});
        public static UnicodeBlock MATHEMATICAL_ALPHANUMERIC_SYMBOLS = new UnicodeBlock("MATHEMATICAL_ALPHANUMERIC_SYMBOLS",
                             new String[] { "Mathematical Alphanumeric Symbols", "MathematicalAlphanumericSymbols" });
        public static UnicodeBlock CJK_UNIFIED_IDEOGRAPHS_EXTENSION_B = new UnicodeBlock("CJK_UNIFIED_IDEOGRAPHS_EXTENSION_B",
                             new String[] { "CJK Unified Ideographs Extension B", "CJKUnifiedIdeographsExtensionB" });
        public static UnicodeBlock CJK_COMPATIBILITY_IDEOGRAPHS_SUPPLEMENT = new UnicodeBlock("CJK_COMPATIBILITY_IDEOGRAPHS_SUPPLEMENT",
                             new String[]{"CJK Compatibility Ideographs Supplement",
                                          "CJKCompatibilityIdeographsSupplement"});
        public static UnicodeBlock TAGS = new UnicodeBlock("TAGS");
        public static UnicodeBlock VARIATION_SELECTORS_SUPPLEMENT = new UnicodeBlock("VARIATION_SELECTORS_SUPPLEMENT", new String[] {"Variation Selectors Supplement",
                                                                             "VariationSelectorsSupplement"});
        public static UnicodeBlock SUPPLEMENTARY_PRIVATE_USE_AREA_A = new UnicodeBlock("SUPPLEMENTARY_PRIVATE_USE_AREA_A",
                             new String[] {"Supplementary Private Use Area-A",
                                           "SupplementaryPrivateUseArea-A"});
        public static UnicodeBlock SUPPLEMENTARY_PRIVATE_USE_AREA_B = new UnicodeBlock("SUPPLEMENTARY_PRIVATE_USE_AREA_B",
                             new String[] {"Supplementary Private Use Area-B",
                                           "SupplementaryPrivateUseArea-B"});
        public static UnicodeBlock HIGH_SURROGATES = new UnicodeBlock("HIGH_SURROGATES", new String[] { "High Surrogates", "HighSurrogates" });
        public static UnicodeBlock HIGH_PRIVATE_USE_SURROGATES = new UnicodeBlock("HIGH_PRIVATE_USE_SURROGATES", new String[] { "High Private Use Surrogates",
                                                                           "HighPrivateUseSurrogates"});
        public static UnicodeBlock LOW_SURROGATES = new UnicodeBlock("LOW_SURROGATES", new String[] { "Low Surrogates", "LowSurrogates" });
        private static int[] blockStarts = {
            0x0000, // Basic Latin
            0x0080, // Latin-1 Supplement
            0x0100, // Latin Extended-A
            0x0180, // Latin Extended-B
            0x0250, // IPA Extensions
            0x02B0, // Spacing Modifier Letters
            0x0300, // Combining Diacritical Marks
            0x0370, // Greek and Coptic
            0x0400, // Cyrillic
            0x0500, // Cyrillic Supplementary
            0x0530, // Armenian
            0x0590, // Hebrew
            0x0600, // Arabic
            0x0700, // Syriac
            0x0750, // unassigned
            0x0780, // Thaana
            0x07C0, // unassigned
            0x0900, // Devanagari
            0x0980, // Bengali
            0x0A00, // Gurmukhi
            0x0A80, // Gujarati
            0x0B00, // Oriya
            0x0B80, // Tamil
            0x0C00, // Telugu
            0x0C80, // Kannada
            0x0D00, // Malayalam
            0x0D80, // Sinhala
            0x0E00, // Thai
            0x0E80, // Lao
            0x0F00, // Tibetan
            0x1000, // Myanmar
            0x10A0, // Georgian
            0x1100, // Hangul Jamo
            0x1200, // Ethiopic
            0x1380, // unassigned
            0x13A0, // Cherokee
            0x1400, // Unified Canadian Aboriginal Syllabics
            0x1680, // Ogham
            0x16A0, // Runic
            0x1700, // Tagalog
            0x1720, // Hanunoo
            0x1740, // Buhid
            0x1760, // Tagbanwa
            0x1780, // Khmer
            0x1800, // Mongolian
            0x18B0, // unassigned
            0x1900, // Limbu
            0x1950, // Tai Le
            0x1980, // unassigned
            0x19E0, // Khmer Symbols
            0x1A00, // unassigned
            0x1D00, // Phonetic Extensions
            0x1D80, // unassigned
            0x1E00, // Latin Extended Additional
            0x1F00, // Greek Extended
            0x2000, // General Punctuation
            0x2070, // Superscripts and Subscripts
            0x20A0, // Currency Symbols
            0x20D0, // Combining Diacritical Marks for Symbols
            0x2100, // Letterlike Symbols
            0x2150, // Number Forms
            0x2190, // Arrows
            0x2200, // Mathematical Operators
            0x2300, // Miscellaneous Technical
            0x2400, // Control Pictures
            0x2440, // Optical Character Recognition
            0x2460, // Enclosed Alphanumerics
            0x2500, // Box Drawing
            0x2580, // Block Elements
            0x25A0, // Geometric Shapes
            0x2600, // Miscellaneous Symbols
            0x2700, // Dingbats
            0x27C0, // Miscellaneous Mathematical Symbols-A
            0x27F0, // Supplemental Arrows-A
            0x2800, // Braille Patterns
            0x2900, // Supplemental Arrows-B
            0x2980, // Miscellaneous Mathematical Symbols-B
            0x2A00, // Supplemental Mathematical Operators
            0x2B00, // Miscellaneous Symbols and Arrows
            0x2C00, // unassigned
            0x2E80, // CJK Radicals Supplement
            0x2F00, // Kangxi Radicals
            0x2FE0, // unassigned
            0x2FF0, // Ideographic Description Characters
            0x3000, // CJK Symbols and Punctuation
            0x3040, // Hiragana
            0x30A0, // Katakana
            0x3100, // Bopomofo
            0x3130, // Hangul Compatibility Jamo
            0x3190, // Kanbun
            0x31A0, // Bopomofo Extended
            0x31C0, // unassigned
            0x31F0, // Katakana Phonetic Extensions
            0x3200, // Enclosed CJK Letters and Months
            0x3300, // CJK Compatibility
            0x3400, // CJK Unified Ideographs Extension A
            0x4DC0, // Yijing Hexagram Symbols
            0x4E00, // CJK Unified Ideographs
            0xA000, // Yi Syllables
            0xA490, // Yi Radicals
            0xA4D0, // unassigned
            0xAC00, // Hangul Syllables
            0xD7B0, // unassigned
            0xD800, // High Surrogates
            0xDB80, // High Private Use Surrogates
            0xDC00, // Low Surrogates
            0xE000, // Private Use
            0xF900, // CJK Compatibility Ideographs
            0xFB00, // Alphabetic Presentation Forms
            0xFB50, // Arabic Presentation Forms-A
            0xFE00, // Variation Selectors
            0xFE10, // unassigned
            0xFE20, // Combining Half Marks
            0xFE30, // CJK Compatibility Forms
            0xFE50, // Small Form Variants
            0xFE70, // Arabic Presentation Forms-B
            0xFF00, // Halfwidth and Fullwidth Forms
            0xFFF0, // Specials
            0x10000, // Linear B Syllabary
            0x10080, // Linear B Ideograms
            0x10100, // Aegean Numbers
            0x10140, // unassigned
            0x10300, // Old Italic
            0x10330, // Gothic
            0x10350, // unassigned
            0x10380, // Ugaritic
            0x103A0, // unassigned
            0x10400, // Deseret
            0x10450, // Shavian
            0x10480, // Osmanya
            0x104B0, // unassigned
            0x10800, // Cypriot Syllabary
            0x10840, // unassigned
            0x1D000, // Byzantine Musical Symbols
            0x1D100, // Musical Symbols
            0x1D200, // unassigned
            0x1D300, // Tai Xuan Jing Symbols
            0x1D360, // unassigned
            0x1D400, // Mathematical Alphanumeric Symbols
            0x1D800, // unassigned
            0x20000, // CJK Unified Ideographs Extension B
            0x2A6E0, // unassigned
            0x2F800, // CJK Compatibility Ideographs Supplement
            0x2FA20, // unassigned
            0xE0000, // Tags
            0xE0080, // unassigned
            0xE0100, // Variation Selectors Supplement
            0xE01F0, // unassigned
            0xF0000, // Supplementary Private Use Area-A
            0x100000, // Supplementary Private Use Area-B
        };
        private static UnicodeBlock[] blocks = {
            BASIC_LATIN,
            LATIN_1_SUPPLEMENT,
            LATIN_EXTENDED_A,
            LATIN_EXTENDED_B,
            IPA_EXTENSIONS,
            SPACING_MODIFIER_LETTERS,
            COMBINING_DIACRITICAL_MARKS,
            GREEK,
            CYRILLIC,
            CYRILLIC_SUPPLEMENTARY,
            ARMENIAN,
            HEBREW,
            ARABIC,
            SYRIAC,
            null,
            THAANA,
            null,
            DEVANAGARI,
            BENGALI,
            GURMUKHI,
            GUJARATI,
            ORIYA,
            TAMIL,
            TELUGU,
            KANNADA,
            MALAYALAM,
            SINHALA,
            THAI,
            LAO,
            TIBETAN,
            MYANMAR,
            GEORGIAN,
            HANGUL_JAMO,
            ETHIOPIC,
            null,
            CHEROKEE,
            UNIFIED_CANADIAN_ABORIGINAL_SYLLABICS,
            OGHAM,
            RUNIC,
            TAGALOG,
            HANUNOO,
            BUHID,
            TAGBANWA,
            KHMER,
            MONGOLIAN,
            null,
            LIMBU,
            TAI_LE,
            null,
            KHMER_SYMBOLS,
            null,
            PHONETIC_EXTENSIONS,
            null,
            LATIN_EXTENDED_ADDITIONAL,
            GREEK_EXTENDED,
            GENERAL_PUNCTUATION,
            SUPERSCRIPTS_AND_SUBSCRIPTS,
            CURRENCY_SYMBOLS,
            COMBINING_MARKS_FOR_SYMBOLS,
            LETTERLIKE_SYMBOLS,
            NUMBER_FORMS,
            ARROWS,
            MATHEMATICAL_OPERATORS,
            MISCELLANEOUS_TECHNICAL,
            CONTROL_PICTURES,
            OPTICAL_CHARACTER_RECOGNITION,
            ENCLOSED_ALPHANUMERICS,
            BOX_DRAWING,
            BLOCK_ELEMENTS,
            GEOMETRIC_SHAPES,
            MISCELLANEOUS_SYMBOLS,
            DINGBATS,
            MISCELLANEOUS_MATHEMATICAL_SYMBOLS_A,
            SUPPLEMENTAL_ARROWS_A,
            BRAILLE_PATTERNS,
            SUPPLEMENTAL_ARROWS_B,
            MISCELLANEOUS_MATHEMATICAL_SYMBOLS_B,
            SUPPLEMENTAL_MATHEMATICAL_OPERATORS,
            MISCELLANEOUS_SYMBOLS_AND_ARROWS,
            null,
            CJK_RADICALS_SUPPLEMENT,
            KANGXI_RADICALS,
            null,
            IDEOGRAPHIC_DESCRIPTION_CHARACTERS,
            CJK_SYMBOLS_AND_PUNCTUATION,
            HIRAGANA,
            KATAKANA,
            BOPOMOFO,
            HANGUL_COMPATIBILITY_JAMO,
            KANBUN,
            BOPOMOFO_EXTENDED,
            null,
            KATAKANA_PHONETIC_EXTENSIONS,
            ENCLOSED_CJK_LETTERS_AND_MONTHS,
            CJK_COMPATIBILITY,
            CJK_UNIFIED_IDEOGRAPHS_EXTENSION_A,
            YIJING_HEXAGRAM_SYMBOLS,
            CJK_UNIFIED_IDEOGRAPHS,
            YI_SYLLABLES,
            YI_RADICALS,
            null,
            HANGUL_SYLLABLES,
            null,
            HIGH_SURROGATES,
            HIGH_PRIVATE_USE_SURROGATES,
            LOW_SURROGATES,
            PRIVATE_USE_AREA,
            CJK_COMPATIBILITY_IDEOGRAPHS,
            ALPHABETIC_PRESENTATION_FORMS,
            ARABIC_PRESENTATION_FORMS_A,
            VARIATION_SELECTORS,
            null,
            COMBINING_HALF_MARKS,
            CJK_COMPATIBILITY_FORMS,
            SMALL_FORM_VARIANTS,
            ARABIC_PRESENTATION_FORMS_B,
            HALFWIDTH_AND_FULLWIDTH_FORMS,
            SPECIALS,
            LINEAR_B_SYLLABARY,
            LINEAR_B_IDEOGRAMS,
            AEGEAN_NUMBERS,
            null,
            OLD_ITALIC,
            GOTHIC,
            null,
            UGARITIC,
            null,
            DESERET,
            SHAVIAN,
            OSMANYA,
            null,
            CYPRIOT_SYLLABARY,
            null,
            BYZANTINE_MUSICAL_SYMBOLS,
            MUSICAL_SYMBOLS,
            null,
            TAI_XUAN_JING_SYMBOLS,
            null,
            MATHEMATICAL_ALPHANUMERIC_SYMBOLS,
            null,
            CJK_UNIFIED_IDEOGRAPHS_EXTENSION_B,
            null,
            CJK_COMPATIBILITY_IDEOGRAPHS_SUPPLEMENT,
            null,
            TAGS,
            null,
            VARIATION_SELECTORS_SUPPLEMENT,
            null,
            SUPPLEMENTARY_PRIVATE_USE_AREA_A,
            SUPPLEMENTARY_PRIVATE_USE_AREA_B
        };

        internal static int MIN_CODE_POINT = 0x000000;
        internal static int MAX_CODE_POINT = 0x10ffff;

        public static bool IsValidCodePoint(int codePoint)
        {
            return codePoint >= MIN_CODE_POINT && codePoint <= MAX_CODE_POINT;
        }
        /// <summary>
        /// Returns the object representing the Unicode block
        /// containing the given character (Unicode code point), or
        /// <code>null</code> if the character is not a member of a
        /// defined block.
        /// </summary>
        /// <param name="c">The character in question</param>
        /// <returns>
        /// The <code>UnicodeBlock</code> instance representing the
        /// Unicode block of which this character is a member, or
        /// <code>null</code> if the character is not a member of any Unicode block
        /// </returns>
        public static UnicodeBlock Of(char c)
        {
            return Of((int)c);
        }
        /// <summary>
        /// Returns the object representing the Unicode block
        /// containing the given character (Unicode code point), or
        /// <code>null</code> if the character is not a member of a
        /// defined block.
        /// </summary>
        /// <param name="codePoint">the character (Unicode code point) in question</param>
        /// <returns>
        /// The <code>UnicodeBlock</code> instance representing the
        /// Unicode block of which this character is a member, or
        /// <code>null</code> if the character is not a member of any Unicode block
        /// </returns>
        public static UnicodeBlock Of(int codePoint)
        {
            if (!IsValidCodePoint(codePoint))
            {
                throw new ArgumentOutOfRangeException();
            }

            int top, bottom, current;
            bottom = 0;
            top = blockStarts.Length;
            current = top / 2;

            // invariant: top > current >= bottom && codePoint >= unicodeBlockStarts[bottom]
            while (top - bottom > 1)
            {
                if (codePoint >= blockStarts[current])
                {
                    bottom = current;
                }
                else
                {
                    top = current;
                }
                current = (top + bottom) / 2;
            }
            return blocks[current];
        }

        public static UnicodeBlock ForName(String blockName) {
            string name = blockName.ToUpper();
            if(!map.ContainsKey(name))
                throw new ArgumentException("invalid block name");

            UnicodeBlock block = map[name];
            return block;
        }
    }
}
