namespace Tes.BusinessRules
{
    public class CompareValues
    {
        /// <summary>
        /// Compare int
        /// </summary>
        /// <param name="db_value"></param>
        /// <param name="data_from"></param>
        /// <param name="data_to"></param>
        /// <param name="opr_from"></param>
        /// <param name="opr_to"></param>
        /// <returns>Boolean</returns>
        public bool Compare(int db_value, int? data_from, int? data_to, string? opr_from, string? opr_to)
        {
            bool result = false;
            if (data_from.HasValue && data_to.HasValue)
            {
                result = (CompareValue(opr_from, data_from.Value, db_value) == true) &&
                         (CompareValue(opr_to, db_value, data_to.Value) == true);
            }
            else if (!data_to.HasValue)
            {
                result = CompareValue(opr_from, data_from.Value, db_value);
            }
            else if (!data_from.HasValue)
            {
                result = CompareValue(opr_to, db_value, data_to.Value);
            }
            return result;
        }
        /// <summary>
        /// Compare decimal
        /// </summary>
        /// <param name="db_value"></param>
        /// <param name="data_from"></param>
        /// <param name="data_to"></param>
        /// <param name="opr_from"></param>
        /// <param name="opr_to"></param>
        /// <returns>Boolean</returns>
        public bool Compare(decimal db_value, decimal? data_from, decimal? data_to, string? opr_from, string? opr_to)
        {
            bool result = false;
            if (data_from.HasValue && data_to.HasValue)
            {
                result = (CompareValue(opr_from, data_from.Value, db_value) == true) &&
                         (CompareValue(opr_to, db_value, data_to.Value) == true);
            }
            else if (!data_to.HasValue)
            {
                result = CompareValue(opr_from, data_from.Value, db_value);
            }
            else if (!data_from.HasValue)
            {
                result = CompareValue(opr_to, db_value, data_to.Value);
            }
            return result;
        }
        /// <summary>
        /// Compare Datetime
        /// </summary>
        /// <param name="db_value"></param>
        /// <param name="data_from"></param>
        /// <param name="data_to"></param>
        /// <param name="opr_from"></param>
        /// <param name="opr_to"></param>
        /// <returns>Boolean</returns>
        public bool Compare(DateTime db_value, DateTime? data_from, DateTime? data_to, string? opr_from, string? opr_to)
        {
            bool result = false;
            if (data_from.HasValue && data_to.HasValue)
            {
                result = (CompareValue(opr_from, data_from.Value, db_value) == true) &&
                         (CompareValue(opr_to, db_value, data_to.Value) == true);
            }
            else if (!data_to.HasValue)
            {
                result = CompareValue(opr_from, data_from.Value, db_value);
            }
            else if (!data_from.HasValue)
            {
                result = CompareValue(opr_to, db_value, data_to.Value);
            }
            return result;
        }

        /// <summary>
        /// Compare int
        /// </summary>
        /// <param name="opr"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>Boolean</returns>
        private bool CompareValue(string opr, int from, int to)
        {
            bool result;
            switch (opr)
            {
                case ">=":
                    result = from >= to;
                    break;
                case ">":
                    result = from > to;
                    break;
                case "<=":
                    result = from <= to;
                    break;
                case "<":
                    result = from < to;
                    break;
                case "=":
                    result = from == to;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }
        /// <summary>
        /// Compare decimal
        /// </summary>
        /// <param name="opr"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>Boolean</returns>
        private bool CompareValue(string opr, decimal from, decimal to)
        {
            bool result;
            switch (opr)
            {
                case ">=":
                    result = from >= to;
                    break;
                case ">":
                    result = from > to;
                    break;
                case "<=":
                    result = from <= to;
                    break;
                case "<":
                    result = from < to;
                    break;
                case "=":
                    result = from == to;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }
        /// <summary>
        /// Compare Datetime
        /// </summary>
        /// <param name="opr"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>Boolean</returns>
        private bool CompareValue(string opr, DateTime from, DateTime to)
        {
            bool result;
            switch (opr)
            {
                case ">=":
                    result = from >= to;
                    break;
                case ">":
                    result = from > to;
                    break;
                case "<=":
                    result = from <= to;
                    break;
                case "<":
                    result = from < to;
                    break;
                case "=":
                    result = from == to;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }
    }
}
