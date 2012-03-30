using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORM;

namespace ApplicationServer.Tests
{
    public class DAOMock
    {
        List<Person> dataBase = new List<Person>();

        public Table GetQuery(string data)
        {
            List<Person> people = (from p in dataBase
                                   select p).ToList();
            Table table = new Table();
            table.InitalizeTable(people);

            return table;
        }

        public void UpdatePerson(int id, List<object> person)
        {
            var dbPerson = (from p in dataBase
                            where p.ID == id
                            select p).ToList();
            for (int i = 0; i < person.Count; i++)
            {
                dbPerson.First()[i] = person[i];
            }
        }

        public void DeletePerson(object idObject)
        {
            int id = int.Parse(idObject.ToString());
            var person = dataBase.First(i => i.ID == id);
            dataBase.Remove(person);
        }

        public void AddPerson(List<List<object>> list)
        {
            Person person = new Person();
            for (int i = 0; i < person.Length; i++)
            {
                person[i] = list.First()[i];
            }
            dataBase.Add(person);
        }

        public void AddPhoto(byte[] photo, int id)
        {
            var person = dataBase.First(i => i.ID == id);
            person.Photo = photo;
        }

        public byte[] GetPhoto(int id)
        {
            var person = dataBase.First(i => i.ID == id);
            return person.Photo;
        }

        public void SaveFile(int id, byte[] file)
        {
            var person = dataBase.First(i => i.ID == id);
            person.File = file;
        }

        public byte[] GetFile(int id)
        {
            var person = dataBase.First(i => i.ID == id);
            return person.File;
        }

        public void SetPhotoLinkName(string FileName, int id)
        {
            var person = dataBase.First(i => i.ID == id);
            person.PhotoLink = FileName;
        }
    }
}
