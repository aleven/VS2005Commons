using System;
using System.Collections.Generic;
using System.Text;

namespace VS2005Commons
{
    public class QueryUtils
    {
        const string CARATTERE_JOLLY = "*";

        /// <summary>
        /// Ritorna una stringa con i caratteri sostituiti per essere usata in una query.
        /// Gestisce:
        /// - gli apici singoli
        /// </summary>
        /// <param name="aString"></param>
        /// <returns></returns>
        public static string encodeString(string aString)
        {
            return aString.Replace("'", "''").Trim();
        }

        /// <summary>
        /// Prepara una condizione like da usare nelle query.
        /// Support query del tipo:
        /// 1) *ca
        /// 2) ca*
        /// 3) *ca*
        /// </summary>
        /// <param name="columnName">La colonna</param>
        /// <param name="filter">Il filtro da usare (deve essere filtrato con forQuery)</param>
        /// <returns></returns>
        public static string likeCondition(string columnName, string filter)
        {
            string condition = "";

            if (StringUtils.isNullOrEmpty(filter))
            {
                condition = "TRUE";
            }
            else
            {
                if (filter.StartsWith(CARATTERE_JOLLY) || filter.EndsWith(CARATTERE_JOLLY))
                {
                    // Ricerche per testo che inizia con o finisce con
                    // Esempio:
                    // ca*
                    // *ca
                    condition = columnName + " LIKE '" + filter + "'";
                }
                else
                {
                    // Ricerche per testo che contiene
                    condition = columnName + " LIKE '" + CARATTERE_JOLLY + filter + CARATTERE_JOLLY + "'";
                }
            }
            return condition;
        }


        /// <summary>
        /// Prepara una condizione like da usare nelle query.
        /// Support query del tipo:
        /// 1) *ca
        /// 2) ca*
        /// 3) *ca*
        /// </summary>
        /// <param name="columnName">La colonna</param>
        /// <param name="filter">Il filtro da usare (deve essere filtrato con forQuery)</param>
        /// <returns></returns>
        public static string likeCondition2(string columnName, string filter)
        {
            string condition = "";

            if (StringUtils.isNullOrEmpty(filter))
            {
                condition = "TRUE";
            }
            else
            {
                if (filter.StartsWith(CARATTERE_JOLLY) || filter.EndsWith(CARATTERE_JOLLY))
                {
                    // Ricerche per testo che inizia con o finisce con
                    // Esempio:
                    // ca*
                    // *ca
                    condition = columnName + " LIKE '" + filter + "'";
                }
                else
                {
                    // Ricerche per testo che contiene
                    condition = columnName + " LIKE '" + CARATTERE_JOLLY + filter + CARATTERE_JOLLY + "'";

                    condition = "(";
                    for (int i = 0; i < filter.Length; i++)
                    {
                        char piece = filter[i];
                        String a = columnName + " LIKE '" + CARATTERE_JOLLY + piece + CARATTERE_JOLLY + "'";

                        if (condition.Length == 1)
                        {
                            condition = condition + a;
                        }
                        else
                        {
                            condition = condition + " AND " + a;
                        }

                    }
                    condition = condition + ")";
                }
            }
            return condition;
        }

        /// <summary>
        /// Come likeCondition ma supporta la sostituzione automatica dei caratteri speciali
        /// se non viene fatta dall'utente
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="filter"></param>
        /// <param name="caratteri"></param>
        /// <returns></returns>
        public static string likeCondition(string columnName, string filter, bool sostituisciCaratteri)
        {
            if (sostituisciCaratteri)
            {
                return likeCondition(columnName, encodeString(filter));
            }
            else
            {
                return likeCondition(columnName, filter);
            }
        }
    }
}
