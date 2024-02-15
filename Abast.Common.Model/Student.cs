using Newtonsoft.Json;
using System;

namespace Abast.Common.Model
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthday { get; set; }
        public int Age { get; set ; }


        public override bool Equals(object obj)
        {
            if(obj is Student st)
            {
                return st.Name==this.Name && st.Surname==this.Surname && st.Birthday==this.Birthday;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
