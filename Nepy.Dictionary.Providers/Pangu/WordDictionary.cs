/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */


using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Nepy.Core;

namespace Nepy.Dictionary.Providers
{
    public class WordDictionaryFile
    {
        public List<IDataNode> Dicts = new List<IDataNode>();
    }

    public struct PositionLength
    {
        public int Level ;
        public int Position;
        public int Length;
        public WordAttribute WordAttr;

        public PositionLength(int position, int length, WordAttribute wordAttr)
        {
            this.Position = position;
            this.Length = length;
            this.WordAttr = wordAttr;
            this.Level = 0;
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", WordAttr.Word, Position);
        }
    }

    /// <summary>
    /// Dictionary for word
    /// </summary>
    public class WordDictionary
    {
        internal List<IDataNode> WordDict = new List<IDataNode>();

        private string _Version = "00";

        public int Count
        {
            get
            {
                if (WordDict == null)
                {
                    return 0;
                }
                else
                {
                    return WordDict.Count;
                }
            }
        }

        #region Private Methods
        private WordDictionaryFile LoadFromTextFile(String fileName)
        {
            WordDictionaryFile dictFile = new WordDictionaryFile();
            dictFile.Dicts = new List<IDataNode>();

            using (StreamReader sr = new StreamReader(fileName, Encoding.UTF8))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    string[] strs = line.Split(new char[] { '|' });

                    if (strs.Length == 3)
                    {
                        string word = strs[0].Trim();

                        POSType pos = (POSType)int.Parse(strs[1].Substring(2, strs[1].Length - 2), System.Globalization.NumberStyles.HexNumber);
                        double frequency = double.Parse(strs[2]);
                        WordAttribute dict = new WordAttribute(word, pos, frequency);

                        dictFile.Dicts.Add(dict);
                    }
                }
            }

            return dictFile;
        }

        Regex verRegex = new Regex("Pan Gu Segment V(.+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private WordDictionaryFile LoadFromBinFile(String fileName, out string verNumStr)
        {
            WordDictionaryFile dictFile = new WordDictionaryFile();
            dictFile.Dicts = new List<IDataNode>();

            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            byte[] version = new byte[32];
            fs.Read(version, 0, version.Length);

            String ver = Encoding.UTF8.GetString(version, 0, version.Length);

            int zeroPosition = ver.IndexOf('\0');
            if (zeroPosition >= 0)
            {
                ver = ver.Substring(0, zeroPosition);
            }

            
            var matches = verRegex.Matches(ver);
            if (matches.Count > 0)
                verNumStr = matches[0].Value;
            else
                verNumStr = null;

            while (fs.Position < fs.Length)
            {
                byte[] buf = new byte[sizeof(int)];
                fs.Read(buf, 0, buf.Length);
                int length = BitConverter.ToInt32(buf, 0);

                buf = new byte[length];

                fs.Read(buf, 0, buf.Length);

                string word = Encoding.UTF8.GetString(buf, 0, length - sizeof(int) - sizeof(double));
                POSType pos = (POSType)BitConverter.ToInt32(buf, length - sizeof(int) - sizeof(double));
                double frequency = BitConverter.ToDouble(buf, length - sizeof(double));

                WordAttribute dict = new WordAttribute(word, pos, frequency);
                string.Intern(dict.Word);

                dictFile.Dicts.Add(dict);
            }

            fs.Close();

            return dictFile;
        }        
        #endregion

        #region Public Methods


        public void Load(String fileName)
        {
            Load(fileName, false, out _Version);
        }

        public void Load(String fileName, out string version)
        {
            Load(fileName, false, out version);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="textFile"></param>
        /// <param name="version">输出字典的版本号</param>
        public void Load(String fileName, bool textFile, out string version)
        {
            version = "";

            WordDict = null;

            if (textFile)
            {
                WordDict = LoadFromTextFile(fileName).Dicts;
            }
            else
            {
                WordDict = LoadFromBinFile(fileName, out version).Dicts;
            }

        }

        #endregion
    }
}
