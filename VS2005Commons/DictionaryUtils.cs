using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace VS2005Commons
{
    public class DictionaryUtils
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static int getValueFromDictionary(Dictionary<long, int> dictionary, long key, int defaultValue)
        {
            int res = defaultValue;

            try
            {
                if (dictionary != null)
                {
                    res = dictionary[key];
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return res;
        }

        public static int getValueFromDictionary(Dictionary<string, int> dictionary, string key, int defaultValue)
        {
            int res = defaultValue;

            try
            {
                if (dictionary != null)
                {
                    res = dictionary[key];
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return res;
        }

    }
}
