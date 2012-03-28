using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using ORM;
using System.Data.Linq;
using System.Reflection;

namespace ApplicationServer
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

        public void InitalizeTable(ObjectQuery<Person> objectQuery)
        {
            GetPersonTypes();
            GetPersonColumnTitles();
            foreach (Person item in objectQuery)
            {
                List<object> newList = new List<object>();
                for (int i = 0; i < item.Length; i++)
                {
                    if (item[i] is byte[])
                    {
                        newList.Add("File");
                        continue;
                    }
                    newList.Add(item[i]);
                }
                valuesList.Add(newList);
            }
        }

        public void FillPerson(ref Person person)
        {
            List<object> values = ValuesList.First();
            for (int i = 0; i < person.Length; i++)
            {
                person[i] = values[i];
            }
        }

        private void GetPersonTypes()
        {
            typesList.Add("System.Int32");      //ID
            typesList.Add("System.String");     //FName
            typesList.Add("System.String");     //LName
            typesList.Add("System.DateTime");   //BirthDate
            typesList.Add("System.String");     //Photo
            typesList.Add("System.String");     //PhotoLink
            typesList.Add("System.String");     //E-mail
            typesList.Add("System.String");     //Phone
            typesList.Add("System.String");     //Description
            typesList.Add("System.String");     //File
        }

        private void GetPersonColumnTitles()
        {
            columnsList.Add("ID");
            columnsList.Add("FName");
            columnsList.Add("LNsme");
            columnsList.Add("BirthDate");
            columnsList.Add("Photo");
            columnsList.Add("PhotoLink");
            columnsList.Add("E-mail");
            columnsList.Add("Phone");
            columnsList.Add("Description");
            columnsList.Add("File");
        }
    }
}
