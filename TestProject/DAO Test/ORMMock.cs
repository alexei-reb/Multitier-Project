using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORM;
using ApplicationServer;
using System.Data.Objects;
namespace TestProject.DAO_Test
{
    class ORMMock
    {
        List<Person> list = new List<Person>();
        public ObjectSet<Person> CreateObjectSet()
        {
            return (from p in list
                    select p);
        }
    }
}
