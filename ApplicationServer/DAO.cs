using System.Collections.Generic;
using System.Data;
using System.Linq;
using ORM;

namespace ApplicationServer
{
    public class DAO
    {
        SQLPersonDBEntities db = new SQLPersonDBEntities();
        DataTable dataTable = new DataTable();

        public Table GetQuery(string data)
        {
            var people = (from p in db.People
                         select p).ToList();
            Table table = new Table();
            table.InitalizeTable(people);
            
            return table;
        }

        public void UpdatePerson(Table table)
        {
            int id = int.Parse(table.ValuesList.First().First().ToString());
            var person = db.People.First(i => i.ID == id);

            List<object> values = table.ValuesList.First();
            for (int i = 0; i < person.Length; i++)
            {
                person[i] = values[i];
            }
            db.SaveChanges();
        }

        public void UpdatePerson(int id, List<object> person)
        {
            var dbPerson = (from p in db.People
                                where p.ID == id
                                select p).ToList();
            for (int i = 0; i < person.Count; i++)
            {
                dbPerson.First()[i] = person[i];
            }
            db.SaveChanges();
        }

        public void DeletePerson(object idObject)
        {
            int id = int.Parse(idObject.ToString());
            var person = db.People.First(i => i.ID == id);
            db.DeleteObject(person);
            db.SaveChanges();
        }

        public void AddPerson(List<List<object>> list)
        {
            Person person = new Person();
            for (int i = 0; i < person.Length; i++)
            {
                person[i] = list.First()[i];
            }
            db.AddToPeople(person);
            db.SaveChanges();
        }

        public void AddPhoto(byte[] photo, int id)
        {
            var person = db.People.First(i => i.ID == id);
            person.Photo = photo;
            db.SaveChanges();
        }

        public byte[] GetPhoto(int id)
        {
            var person = db.People.First(i => i.ID == id);
            return person.Photo;
        }

        public void SaveFile(int id, byte[] file)
        {
            var person = db.People.First(i => i.ID == id);
            person.File = file;
            db.SaveChanges();
        }

        public byte[] GetFile(int id)
        {
            var person = db.People.First(i => i.ID == id);
            return person.File;
        }

        public void SetPhotoLinkName(string FileName, int id)
        {
            var person = db.People.First(i => i.ID == id);
            person.PhotoLink = FileName;
            db.SaveChanges();
        }
    }
}

