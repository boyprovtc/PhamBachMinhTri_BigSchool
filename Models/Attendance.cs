namespace BigSchools.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("Attendance")]
    //public partial class Attendance
    //{
    //    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    //    public Attendance()
    //    {
    //        Courses = new HashSet<Course>();

    //    }

    //    public int CourseID { get; set; }

    //    public string Attendee { get; set; }



    //    public virtual ICollection<Course> Courses { get; set; }

    //}
    internal class Attendance
    {
        public Attendance()
        {
        }

        public int CourseID { get; set; }
        public string Attendee { get; set; }
    }
}