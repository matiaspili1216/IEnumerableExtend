using System;
using System.Collections.Generic;
using System.Linq;

namespace IEnumerableExtend
{
    /// <summary>
    /// Class grouping of Custom IEnumerable Extend Methods
    /// </summary>
    public static class IEnumerableExtend
    {
        /// <summary>
        /// Sobreescritura del metodo Equals, para comparar dos List genericas de tipo T
        /// </summary>
        /// <typeparam name="T">Type que contiene la Lista</typeparam>
        /// <param name="thisList">Lista de referencia</param>
        /// <param name="otherList">Lista que se desea comparar</param>
        /// <returns><see langword="true"/> si cada elemento de una lista es igual al elemento de la otra lista en su misma ubicacion.</returns>
        public static bool Equals<T>(this IEnumerable<T> thisList, IEnumerable<T> otherList)
        {
            if (otherList == null) { return false; }

            int count_thisList = thisList.Count();
            int count_otherList = otherList.Count();

            if (count_thisList != count_otherList) return false;

            for (int i = 0; i < count_thisList; i++)
            {
                if (!thisList.ElementAt(i).Equals(otherList.ElementAt(i)))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Sobreescritura del metodo Equals, para comparar dos List genericas de tipo T
        /// </summary>
        /// <typeparam name="T">Type que contiene la Lista</typeparam>
        /// <param name="thisList">Lista de referencia</param>
        /// <param name="otherList">Lista que se desea comparar</param>
        /// <param name="ignoreOrder"><see langword="true"/> si se desea que la comparacion ignore el orden de los elementos. (Se realiza un orden antes de comparar.)</param>
        /// <returns><see langword="true"/> si cada elemento de una lista es igual al elemento de la otra lista en su misma ubicacion.</returns>
        public static bool Equals<T>(this IEnumerable<T> thisList, IEnumerable<T> otherList, bool ignoreOrder) where T : IComparable<T>
        {
            var lthisList = ignoreOrder ? thisList.OrderBy(x => x).ToList() : thisList.ToList();
            var lotherList = ignoreOrder ? otherList.OrderBy(x => x).ToList() : otherList.ToList();

            return Equals(lthisList, lotherList);
        }

        /// <summary>
        /// Permite comparar dos lista, y obtener las diferencias entre sí
        /// </summary>
        /// <typeparam name="T">Type que contiene la Lista</typeparam>
        /// <param name="thisList">Lista de referencia</param>
        /// <param name="otherList">Lista que se desea comparar</param>
        /// <param name="differencesDetails">Texto donde se detallas las diferencias de entre las Listas.</param>
        /// <returns><see langword="true"/> si cada elemento de una lista es igual al elemento de la otra lista en su misma ubicacion.</returns>
        public static bool Equals<T>(this IEnumerable<T> thisList, IEnumerable<T> otherList, out string differencesDetails, string nameThisList = null, string nameOtherList = null, bool verifyUnbalancedList = true)
        {
            bool Result = true;
            List<string> differencesList = new List<string>();

            string NameThisList = string.IsNullOrEmpty(nameThisList) ? "Actual" : nameThisList;
            string NameOtherList = string.IsNullOrEmpty(nameOtherList) ? "A comparar" : nameOtherList;

            if (!VerifyListLength(thisList, otherList, NameThisList, NameOtherList, out string differencesLength, out bool isNull))
            {
                if (isNull || !verifyUnbalancedList) { differencesDetails = differencesLength; return false; }
                else
                {
                    differencesList.Add(GetDifferencesFromUnbalancedList(thisList, otherList, differencesLength));

                    Result = false;
                }
            }
            else
            {
                for (int i = 0; i < thisList.Count(); i++)
                {
                    if (!thisList.ElementAt(i).Equals(otherList.ElementAt(i)))
                    {
                        differencesList.Add($"Index {i}.\n Valor lista '{NameThisList}': '{thisList.ElementAt(i)}'. Valor lista '{NameOtherList}': '{otherList.ElementAt(i)}'");
                        Result = false;
                    }
                }
            }

            differencesDetails = string.Join("\n", differencesList);
            return Result;
        }

        /// <summary>
        /// Permite comparar dos lista, y obtener las diferencias entre sí
        /// </summary>
        /// <typeparam name="T">Type que contiene la Lista</typeparam>
        /// <param name="thisList">Lista de referencia</param>
        /// <param name="otherList">Lista que se desea comparar</param>
        /// <param name="ignoreOrder"><see langword="true"/>Si se desea ignorar el orden de las listas, al comparar</param>
        /// <param name="differencesdetails">Texto donde se detallas las diferencias de entre las Listas.</param>
        /// <returns><see langword="true"/> si cada elemento de una lista es igual al elemento de la otra lista en su misma ubicacion.</returns>
        public static bool Equals<T>(this IEnumerable<T> thisList, IEnumerable<T> otherList, bool ignoreOrder, out string differencesdetails, string nameThisList = null, string nameOtherList = null) where T : IComparable<T>
        {
            var lthisList = ignoreOrder ? thisList.OrderBy(x => x).ToList() : thisList.ToList();
            var lotherList = ignoreOrder ? otherList.OrderBy(x => x).ToList() : otherList.ToList();

            var Res = Equals(lthisList, lotherList, out differencesdetails, nameThisList, nameOtherList);

            return Res;
        }

        /// <summary>
        /// Sobreescritura del metodo Equals, para comparar dos List genericas de tipo String
        /// </summary>
        /// <param name="thisList">Lista de referencia</param>
        /// <param name="otherList">Lista que se desea comparar</param>
        /// <param name="ignoreOrder"><see langword="true"/> si se desea que la comparacion ignore el orden de los elementos. (Se realiza un orden antes de comparar.)</param>
        /// <param name="caseSensitive"><see langword="true"/> si se desea que la comparacion sea CaseSensitive</param>
        /// <returns><see langword="true"/> si cada elemento de una lista es igual al elemento de la otra lista en su misma ubicacion.</returns>
        public static bool Equals(this IEnumerable<string> thisList, IEnumerable<string> otherList, bool ignoreOrder, bool caseSensitive)
        {
            if (otherList == null) { return false; }
            if (thisList.Count() != otherList.Count()) return false;

            List<string> lthisList = thisList.ToList();
            List<string> lotherList = otherList.ToList();

            if (ignoreOrder)
            {
                lthisList = thisList.OrderBy(x => x).ToList();
                lotherList = otherList.OrderBy(x => x).ToList();
            }

            for (int i = 0; i < lthisList.Count; i++)
            {
                string thisListCS = caseSensitive ? lthisList.ElementAt(i) : lthisList.ElementAt(i).ToLower();
                string otherListCS = caseSensitive ? lotherList.ElementAt(i) : lotherList.ElementAt(i).ToLower();

                if (!thisListCS.Equals(otherListCS))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Sobreescritura del metodo Equals, para comparar dos List  genericas de tipo string
        /// </summary>
        /// <param name="thisList">Lista de referencia</param>
        /// <param name="otherList">Lista que se desea comparar</param>
        /// <param name="ignoreOrder"><see langword="true"/> si se desea que la comparacion ignore el orden de los elementos. (Se realiza un orden antes de comparar.)</param>
        /// <param name="caseSensitive"><see langword="true"/> si se desea que la comparacion sea CaseSensitive</param>
        /// <param name="differencesDetails">Texto donde se detallas las diferencias de entre las Listas.</param>
        /// <returns><see langword="true"/> si cada elemento de una lista es igual al elemento de la otra lista en su misma ubicacion.</returns>
        public static bool Equals(this IEnumerable<string> thisList, IEnumerable<string> otherList, bool ignoreOrder, bool caseSensitive, out string differencesDetails, string nameThisList = null, string nameOtherList = null, bool verifyUnbalancedList = true)
        {
            string NameThisList = string.IsNullOrEmpty(nameThisList) ? "Actual" : nameThisList;
            string NameOtherList = string.IsNullOrEmpty(nameOtherList) ? "A comparar" : nameOtherList;

            var listDifferencesDetails = new List<string>();
            bool Result = true;

            bool isUnbalanced = false;

            if (!VerifyListLength(thisList, otherList, NameThisList, NameOtherList, out string differencesLength, out bool isNull))
            {
                if (isNull || !verifyUnbalancedList) { differencesDetails = differencesLength; return false; }
                else
                {
                    isUnbalanced = true;
                }
            }

            List<string> lthisList = thisList.Select(x => caseSensitive ? x : x.ToLower()).ToList();
            List<string> lotherList = otherList.Select(x => caseSensitive ? x : x.ToLower()).ToList();

            if (ignoreOrder)
            {
                lthisList = lthisList.OrderBy(x => x).ToList();
                lotherList = lotherList.OrderBy(x => x).ToList();
            }

            if (isUnbalanced)
            {
                listDifferencesDetails.Add(GetDifferencesFromUnbalancedList(thisList, otherList, differencesLength));

                Result = false;
            }
            else
            {
                for (int i = 0; i < lthisList.Count; i++)
                {
                    string thisListCS = lthisList.ElementAt(i);
                    string otherListCS = lotherList.ElementAt(i);

                    if (!thisListCS.Equals(otherListCS))
                    {
                        Result = false;
                        listDifferencesDetails.Add($"Index {i + 1}.\nValor lista '{NameThisList}': '{thisListCS}'.\nValor lista '{NameOtherList}': '{otherListCS}'.");
                    }
                }
            }

            differencesDetails = Result ? "" : $"Diferencias:\n{listDifferencesDetails.Join("\n")}";
            return Result;
        }

        private static string GetDifferencesFromUnbalancedList<T>(IEnumerable<T> thisList, IEnumerable<T> otherList, string differencesLength)
        {
            List<string> differencesList = new List<string>
            {
                differencesLength
            };

            int count_thisList = thisList.Count();
            int count_otherList = otherList.Count();

            differencesList.AddRange(count_thisList > count_otherList
                ? GetDifferencesDetailsUnbalancedList(thisList, otherList)
                : GetDifferencesDetailsUnbalancedList(otherList, thisList)
                );

            return differencesList.Join("\n");
        }


        private static bool VerifyListLength<T>(IEnumerable<T> thisList, IEnumerable<T> otherList, string nameThisList, string nameOtherList, out string differencesdetails, out bool isNull)
        {
            if (otherList == null) { differencesdetails = $"La lista '{nameOtherList}' es nula."; isNull = true; return false; }

            int count_thisList = thisList.Count();
            int count_otherList = otherList.Count();

            if (count_thisList != count_otherList)
            {
                differencesdetails = $"El número de items en la lista '{nameThisList}' ({count_thisList}), no coincide con la lista '{nameOtherList}' ({count_otherList})";
                isNull = false; return false;
            }
            else { differencesdetails = ""; isNull = false; return true; }
        }

        public static List<string> GetDifferencesDetailsUnbalancedList<T>(IEnumerable<T> majorList, IEnumerable<T> minorList)
        {
            int count_majorList = majorList.Count();
            int count_minorList = minorList.Count();

            if (count_minorList > count_majorList) { return new List<string>() { "Error en 'GetDifferencesDetailsUnbalancedList', el orden de los parametros no es el correcto." }; }

            List<string> differencesList = new List<string>();

            if (count_minorList == count_majorList) { return differencesList; }

            int index_majorList = 0;
            int index_minorList = 0;

            int countDifLength = count_majorList - count_minorList;
            int unbalancedTimes = 0;

            do
            {
                var majorListElement = majorList.ElementAtOrDefault(index_majorList);
                var minorListElement = minorList.ElementAtOrDefault(index_minorList);

                if (minorListElement == null || minorListElement.Equals(default(T)))
                {
                    differencesList.Add($"Index {index_majorList}.\n Valor lista major: '{majorListElement}'. Valor lista menor: 'Sin registro'");
                    index_majorList++;
                    index_minorList++;
                }
                else if (majorListElement.Equals(minorListElement))
                {
                    index_majorList++;
                    index_minorList++;
                }
                else if (unbalancedTimes < countDifLength)
                {
                    if (majorList.Contains(minorListElement))
                    {
                        differencesList.Add($"Index {index_majorList}.\n Valor lista major: '{majorListElement}'. Valor lista menor: 'Sin registro'");
                        index_majorList++;
                    }
                    else
                    {
                        differencesList.Add($"Index {index_majorList}.\n Valor lista major: '{majorListElement}'. Valor lista menor: '{minorListElement}'");
                        index_majorList++;
                        index_minorList++;
                    }
                }
                else
                {
                    differencesList.Add($"Index {index_majorList}.\n Valor lista major: '{majorListElement}'. Valor lista menor: '{minorListElement}'");
                    index_majorList++;
                    index_minorList++;
                }

            } while (index_majorList < count_majorList);

            do
            {
                var minorListElement = minorList.ElementAtOrDefault(index_minorList);

                differencesList.Add($"Index {index_majorList}.\n Valor lista major: 'Sin registro'. Valor lista menor: '{minorListElement}'");
                index_minorList++;
            } while (index_minorList < count_minorList);

            return differencesList;
        }

        /// <summary>
        /// Permite obtener un valor aleatorio de una lista dada.
        /// </summary>
        /// <typeparam name="T">Type de la lista</typeparam>
        /// <param name="list">Lista de donde se obtendra el elemento</param>
        /// <returns>Elemento aleatorio de la lista</returns>
        public static T GetRandomValue<T>(this IEnumerable<T> list)
        {
            if (list == null) { throw new ArgumentNullException($"'{nameof(list)}' is required for GetRandomValue"); }
            else if (!list.Any()) { throw new ArgumentNullException($"'{nameof(list)}' is empty"); }
            else if (list.Count() == 1) { return list.ElementAt(0); }
            else
            {
                var ret = list.ElementAt(new Random(((int)DateTime.Now.Ticks & 0x0000FFFF)).Next(0, list.Count()));
                return ret;
            }
        }

        /// <summary>
        /// Permite obtener un valor aleatorio de una lista dada. Excluyendo un valor expecifico
        /// </summary>
        /// <typeparam name="T">Type de la lista</typeparam>
        /// <param name="lista">Lista de donde se obtendra el elemento</param>
        /// <param name="excludedValue">Valor a excluir de la seleccion</param>
        /// <returns>Elemento aleatorio de la lista</returns>
        public static T GetRandomValue<T>(this IEnumerable<T> lista, T excludedValue)
        {
            return lista.Remove(excludedValue).GetRandomValue();
        }

        /// <summary>
        /// Permite obtener un valor aleatorio de una lista dada. Excluyendo un valor expecifico
        /// </summary>
        /// <typeparam name="T">Type de la lista</typeparam>
        /// <param name="lista">Lista de donde se obtendra el elemento</param>
        /// <param name="excludedValues">Valores a excluir de la seleccion</param>
        /// <returns>Elemento aleatorio de la lista</returns>
        public static T GetRandomValue<T>(this IEnumerable<T> lista, IEnumerable<T> excludedValues) => lista.Except(excludedValues).GetRandomValue();

        /// <summary>
        /// Quita la primera aparición de un objeto específico
        /// </summary>
        /// <typeparam name="T">Type de la lista</typeparam>
        /// <param name="lista">Lista de donde se obtendra el elemento</param>
        /// <param name="excludedItem">Objeto de tipo generico 'T' a quitar</param>
        /// <returns>IEnumerable generica de tipo T, sin el elemento</returns>
        public static IEnumerable<T> Remove<T>(this IEnumerable<T> lista, T excludedItem)
        {
           lista.ToList().Remove(excludedItem);
            return lista;
        }

        /// <summary>
        /// Permite obtener el primer elemento NO nulo de una lista
        /// </summary>
        /// <typeparam name="T">Type que contiene la Lista</typeparam>
        /// <param name="listT">Lista del cual se desea obtener el primer valor no nulo</param>
        /// <returns>El primer valor no nulo de la lista</returns>
        public static T FirstNotNullOrEmpty<T>(this IEnumerable<T> listT)
        {
            foreach (var item in listT)
            {
                if (item != null && !string.IsNullOrEmpty(item.ToString()))
                    return item;
            }

            return default;
        }

        /// <summary>
        /// Extencion que realiza la llamada a 'String.Join()'
        /// </summary>
        /// <typeparam name="T">Type de la lista</typeparam>
        /// <param name="values">Lista de elementos, que se desea combinar</param>
        /// <param name="separator">String que se usa de Separador</param>
        /// <returns>String con los valores de la lista, separados por el String de separación</returns>
        public static string Join<T>(this IEnumerable<T> values, string separator)
        {
            return string.Join(separator, values);
        }

        /// <summary>
        /// Permite obtener el 'Index' de un valor determinado
        /// </summary>
        /// <typeparam name="T">Type de la lista</typeparam>
        /// <param name="source">Lista de la cual se obtendran el 'Index'</param>
        /// <param name="value">Valor del cual se desea obtener el 'index'</param>
        /// <returns>'Index' de un valor determinado</returns>
        public static int SelectedIndex<T>(this IEnumerable<T> source, T value)
        {
            int index = 0;
            foreach (var item in source)
            {
                if (item.Equals(value))
                {
                    return index;
                }
                index++;
            }

            return -1;
        }

        /// <summary>
        /// Obtiene el valor siguiente de la lista, a partir de un valor especificado.
        /// </summary>
        /// <typeparam name="T">Type de la lista</typeparam>
        /// <param name="source">Lista de la cual se obtendran el 'Index'</param>
        /// <param name="value">Valor a partir del cual buscar el siguente</param>
        /// <param name="whitLoop"><see langword="true"/> si se desea que se haga un Loop en la lista</param>
        /// <returns>El valor siguiente al valor expecificado</returns>
        public static T NextTo<T>(this IEnumerable<T> source, T value, bool whitLoop)
        {
            int index = source.SelectedIndex(value);
            int count = source.Count();
            int newIndex = index + 1;

            return newIndex >= count
                ? whitLoop ? source.ElementAt(0) : throw new IndexOutOfRangeException("The value selected, is least possible value.")
                : source.ElementAt(newIndex);
        }

        /// <summary>
        /// Obtiene el valor anterior de la lista, a partir de un valor especificado.
        /// </summary>
        /// <typeparam name="T">Type de la lista</typeparam>
        /// <param name="source">Lista de la cual se obtendran el 'Index'</param>
        /// <param name="value">Valor a partir del cual buscar el anterior</param>
        /// <param name="whitLoop"><see langword="true"/> si se desea que se haga un Loop en la lista</param>
        /// <returns>El valor anterior al valor expecificado</returns>
        public static T PreviewTo<T>(this IEnumerable<T> source, T value, bool whitLoop)
        {
            int index = source.SelectedIndex(value);
            int count = source.Count();
            int newIndex = index - 1;

            return newIndex < 0
                ? whitLoop ? source.ElementAt(count - 1) : throw new IndexOutOfRangeException("The value selected, is first possible value.")
                : source.ElementAt(newIndex);
        }

        /// <summary>
        /// Metodo que permite generar una Lista de intervalos de la lista parametrizada. Comenzando desde el principio de la lista, y dividiendo la misma según <paramref name="count"/>.
        /// </summary>
        /// <typeparam name="T">Type de la lista</typeparam>
        /// <param name="source">Lista de la cual se obtendran los intervalos</param>
        /// <param name="count">Número de elementos cada intervalo</param>
        /// <returns>Lista de intervalos, que se obtiene a partir de dividir <paramref name="source"/> en intervalos con catidad definida en <paramref name="count"/></returns>
        public static IEnumerable<IEnumerable<T>> GetRanges<T>(this IEnumerable<T> source, int count)
        {
            if (!source.Any()) return new List<List<T>>() { source.ToList() };

            int countSource = source.Count();

            if (countSource < count) throw new IndexOutOfRangeException();

            var listaRet = new List<List<T>>();

            for (int i = 0; ; i++)
            {
                var desde = (count * i);
                count = Math.Min(count, countSource - (count * i));

                listaRet.Add(source.ToList().GetRange(desde, count));
                if (desde + count >= countSource) break;
            }

            return listaRet;
        }

        /// <summary>
        /// Retorna todos los elementos de una Lista, exepto el Primero
        /// </summary>
        /// <typeparam name="TSource">Type de la lista</typeparam>
        /// <param name="source">IEnumerable del cual se desea obtener la lista</param>
        /// <returns>IEnumerable con los elementos de la Lista, exepto el Primero</returns>
        public static IEnumerable<TSource> ExceptFirst<TSource>(this IEnumerable<TSource> source)
        {
            if (!source.Any()) throw new NullReferenceException();

            var first = new List<TSource>() { source.First() };
            return source.Except(first);
        }

        /// <summary>
        /// Retorna todos los elementos de una Lista, exepto el Ultimo
        /// </summary>
        /// <typeparam name="TSource">Type de la lista</typeparam>
        /// <param name="source">IEnumerable del cual se desea obtener la lista</param>
        /// <returns>IEnumerable con los elementos de la Lista, exepto el Ultimo</returns>
        public static IEnumerable<TSource> ExceptLast<TSource>(this IEnumerable<TSource> source)
        {
            if (!source.Any()) throw new NullReferenceException();

            var last = new List<TSource>() { source.Last() };
            return source.Except(last);
        }
    }
}