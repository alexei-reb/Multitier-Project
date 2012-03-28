using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationServer.Tests
{
    public class Person
    {
        public static Person CreatePerson(global::System.Int32 id)
        {
            Person person = new Person();
            person.ID = id;
            return person;
        }

        #region Primitive Properties

        public global::System.Int32 ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                }
            }
        }
        private global::System.Int32 _ID;

        public global::System.String FName
        {
            get
            {
                return _FName;
            }
            set
            {
                _FName = value;
            }
        }
        private global::System.String _FName;

        public global::System.String LName
        {
            get
            {
                return _LName;
            }
            set
            {
                _LName = value;
            }
        }
        private global::System.String _LName;

        public Nullable<global::System.DateTime> BirthDate
        {
            get
            {
                return _BirthDate;
            }
            set
            {
                _BirthDate = value;
            }
        }
        private Nullable<global::System.DateTime> _BirthDate;

        public global::System.Byte[] Photo
        {
            get
            {
                return _Photo;
            }
            set
            {
                _Photo = value;
            }
        }
        private global::System.Byte[] _Photo;

        public global::System.String PhotoLink
        {
            get
            {
                return _PhotoLink;
            }
            set
            {
                _PhotoLink = value;
            }
        }
        private global::System.String _PhotoLink;

        public global::System.String E_mail
        {
            get
            {
                return _E_mail;
            }
            set
            {
                _E_mail = value;
            }
        }
        private global::System.String _E_mail;

        public global::System.String Phone
        {
            get
            {
                return _Phone;
            }
            set
            {
                _Phone = value;
            }
        }
        private global::System.String _Phone;

        public global::System.String Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }
        private global::System.String _Description;

        public global::System.Byte[] File
        {
            get
            {
                return _File;
            }
            set
            {
                _File = value;
            }
        }
        private global::System.Byte[] _File;

        #endregion

        #region Custom methods
        public object this[int index]
        {
            get
            {
                object result = null;
                if (index < 0 || index > Length)
                {
                    throw new IndexOutOfRangeException();
                }
                switch (index)
                {
                    case 0:
                        result = ID;
                        break;
                    case 1:
                        result = FName;
                        break;
                    case 2:
                        result = LName;
                        break;
                    case 3:
                        result = BirthDate;
                        break;
                    case 4:
                        result = Photo;
                        break;
                    case 5:
                        result = PhotoLink;
                        break;
                    case 6:
                        result = E_mail;
                        break;
                    case 7:
                        result = Phone;
                        break;
                    case 8:
                        result = Description;
                        break;
                    case 9:
                        result = File;
                        break;
                }
                return result;
            }
            set
            {
                if (index < 0 || index > Length)
                {
                    throw new IndexOutOfRangeException();
                }
                switch (index)
                {
                    case 0:
                        if (value != null)
                        {
                            ID = int.Parse(value.ToString());
                        }
                        break;
                    case 1:
                        FName = value as string;
                        break;
                    case 2:
                        LName = value as string;
                        break;
                    case 3:
                        if (value != null)
                        {
                            BirthDate = Convert.ToDateTime(value as string);
                        }
                        else
                        {
                            BirthDate = null;
                        }
                        break;
                    case 4:
                        Photo = value != null ? Encoding.UTF8.GetBytes(value.ToString()) : null;
                        break;
                    case 5:
                        PhotoLink = value as string;
                        break;
                    case 6:
                        E_mail = value as string;
                        break;
                    case 7:
                        Phone = value as string;
                        break;
                    case 8:
                        Description = value as string;
                        break;
                    case 9:
                        File = value != null ? Encoding.UTF8.GetBytes(value as string) : null;
                        break;
                }
            }
        }
        public int Length
        {
            get
            {
                return 10;
            }
        }
        #endregion
    }
}
