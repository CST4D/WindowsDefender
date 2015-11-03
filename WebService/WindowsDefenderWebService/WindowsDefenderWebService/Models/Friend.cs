namespace WindowsDefenderWebService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Friend
    {
        [Key]
        [Column(Order = 0)]
        public string UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        public string FriendId { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateFriended { get; set; }

        [StringLength(128)]
        public string AspNetUser_Id { get; set; }

        

        [StringLength(128)]
        public string AspNetUser_Id1 { get; set; }

        [StringLength(128)]
        public string AspNetUser_Id2 { get; set; }


    }
}
