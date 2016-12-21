using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Sql
{
    /// <summary>
    /// General SQL Table descriptor. Contains table and columns names, the Additional tables.
    /// </summary>
    public class SqlTableDescriptorBase
    {
        /// <summary>
        /// protected function, only for Main and Child descendant classes, where columns will be specified by descendant.
        /// </summary>
        /// <param name="tableName_in"></param>
        protected SqlTableDescriptorBase(string tableName_in)
        {
            TableName = tableName_in;
        }

        public SqlTableDescriptorBase(string tableName_in, IEnumerable<string> columnNames_in)
            : this(tableName_in)
        {
            ColumnNames = columnNames_in;
            CheckColumns(columnNames_in);
        }

        public SqlTableDescriptorBase(string tableName_in, IEnumerable<string> columnNames_in, IEnumerable<SqlTableDescriptorAdditional> additionalTables_in)
            : this(tableName_in, columnNames_in)
        {
            if (additionalTables_in == null)
                AdditionalTables = Enumerable.Empty<SqlTableDescriptorAdditional>();
            else
                AdditionalTables = additionalTables_in;
        }

        public string TableName { get; set; }

        public IEnumerable<string> ColumnNames { get; protected internal set; }

        void CheckColumns(IEnumerable<string> columns_in)
        {
#if DEBUG   //check column names are not SQL reserved:
            foreach (string str in ColumnNames)
            {
                if(SqlLowLevel.SQL_RESERVED_NAMES.Contains(str))
                    throw new ArgumentException(string.Format("SQL Reserved column name founded: '{0}' at table: '{1}'", str, this));
            }
#endif
        }

        /// <summary>
        /// Adds columns dynamically
        /// </summary>
        /// <param name="newColumns_in"></param>
        public void AddColumns(IEnumerable<string> newColumns_in)
        {
            CheckColumns(newColumns_in);
            ColumnNames = ColumnNames.Union(newColumns_in);
        }

        public string SquenceName
        {
            get { return string.Format("{0}_pk_id_seq", TableName); }
        }

        protected internal IEnumerable<SqlTableDescriptorAdditional> AdditionalTables { get; set; }

        public SqlTableDescriptorAdditional GetAdditionalTable(string table_in)
        {
            return AdditionalTables.Where(item => item.TableName == table_in).Single();
        }

        public void AddAdditionalTable(SqlTableDescriptorAdditional table_in)
        {
            List<SqlTableDescriptorAdditional> res = new List<SqlTableDescriptorAdditional>(AdditionalTables);
            res.Add(table_in);
            AdditionalTables = res.ToArray();
        }

        public override string ToString()
        {
            return string.Format("{0} table '{1}', columns= [{2}], Additional Tables= [{3}]",
                GetType().Name, TableName, ColumnNames.ItemsToString(), AdditionalTables.ItemsToString());
        }

        public string FKeyToThis
        {
            get { return string.Format("fk_{0}_id", TableName); }
        }
      
        public override bool Equals(object obj)
        {
            if (obj == null ||
                (obj.GetType() != this.GetType()))
                return false;

            return ((SqlTableDescriptorBase)obj).TableName == this.TableName;
        }

        public override int GetHashCode()
        {
            return TableName.GetHashCode();
        }
    }

}
