using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MobileClient
{
    public class Table
    {
        private List<string> columnsList = new List<string>();
        private List<string> typesList = new List<string>();
        private List<List<object>> valuesList = new List<List<object>>();

        public List<string> ColumnsList
        {
            get
            {
                return columnsList;
            }
            set
            {
                columnsList = value;
            }
        }
        public List<string> TypesList
        {
            get
            {
                return typesList;
            }
            set
            {
                typesList = value;
            }
        }
        public List<List<object>> ValuesList
        {
            get
            {
                return valuesList;
            }
            set
            {
                valuesList = value;
            }
        }
    }
}
