using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NLog;

namespace VS2005Commons
{
    /// <summary>
    /// Utils
    /// </summary>
    public class FileUtils
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Genera un nome file valido data la stringa in oggetto
        /// Mantiene solo i caratteri ed i numero e - e _
        /// </summary>
        /// <param name="aString"></param>
        /// <returns></returns>
        public static string encodeFileName(String aString)
        {
            string res = "";
            foreach (char aChar in aString)
            {
                if (Char.IsLetterOrDigit(aChar) || aChar.Equals('-') || aChar.Equals('_'))
                {
                    res += aChar;
                }
            }

            return res;
        }

        /// <summary>
        /// Scrive su un file il testo specificato, crea sempre un file nuovo
        /// </summary>
        /// <param name="nomeFile"></param>
        /// <param name="testo"></param>
        public static void ScriviFileDiTesto(String nomeFile, String testo)
        {
            using (StreamWriter outfile = new StreamWriter(nomeFile))
            {
                outfile.Write(testo);
            }
        }

        /// <summary>
        /// Scrive su un file il testo specificato, si può decidere se accodare o sovrascrivere
        /// </summary>
        /// <param name="nomeFile"></param>
        /// <param name="testo"></param>
        /// <param name="accodaSeEsiste">se true nel caso esiste gia il file accoda il testo</param>
        public static void ScriviFileDiTesto(String nomeFile, String testo, Boolean accodaSeEsiste)
        {
            if (accodaSeEsiste && File.Exists(nomeFile))
            {
                using (FileStream aFile = new FileStream(nomeFile, FileMode.Append))
                {
                    using (StreamWriter outfile = new StreamWriter(aFile))
                    {
                        outfile.Write(testo);
                    }
                }
            }
            else
            {
                ScriviFileDiTesto(nomeFile, testo);
            }
        }

        public static string LeggiFileDiTestoToEnd(String nomeFile)
        {
            string res;

            using (StreamReader inFile = new StreamReader(nomeFile))
            {
                res = inFile.ReadToEnd();
            }

            return res;
        }

        public static List<String> LeggiFileDiTesto(String nomeFile)
        {
            List<String> res = new List<string>();
            TextReader tr = null;

            try
            {
                tr = new StreamReader(nomeFile);

                // read a line of text
                String line;
                while ((line = tr.ReadLine()) != null)
                {
                    res.Add(line);
                }

                //// close the stream
                //tr.Close();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            finally
            {
                if (tr != null)
                {
                    tr.Close();
                }
            }

            return res;
        }

        public static void DeleteIfExist(String fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        public static void DeleteOldFolders(String dirName, int oldAgeInDays)
        {
            // string[] files = Directory.GetFiles(dirName);
            logger.Info("DeleteOldFolders on " + dirName);

            string[] dirs = Directory.GetDirectories(dirName);

            logger.Info(dirs.Length);

            foreach (string dir in dirs)
            {
                DirectoryInfo dirinfo = new DirectoryInfo(dir);
                if (dirinfo.CreationTime < DateTime.Now.AddDays(-oldAgeInDays))
                {
                    logger.Info("DeleteOldFolders " + dir);
                    dirinfo.Delete(true);
                }
            }

        }

        public List<string> filesInFolder(string folder)
        {
            List<string> res = new List<string>();

            string[] files = Directory.GetFiles(folder);

            res = new List<string>(files);

            return res;
        }
    }
}
