namespace WindowsDefenderWebService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Model for the Friends table.
    /// 
    /// Authors: Wilson Carpenter, Gerald Becker
    /// </summary>
    public partial class Friend
    {
        /// <summary>
        /// Primary Key. The user's ID.
        /// </summary>
        [Key]
        [Column(Order = 0)]
        public string UserId { get; set; }

        /// <summary>
        /// Friend's UserId.
        /// </summary>
        [Key]
        [Column(Order = 1)]
        public string FriendId { get; set; }

        /// <summary>
        /// The date when the users became friends.
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime DateFriended { get; set; }

        /// <summary>
        /// Duplicate UserId field.
        /// </summary>
        [StringLength(128)]
        public string AspNetUser_Id { get; set; }

        /// <summary>
        /// Duplicate UserId field.
        /// </summary>
        [StringLength(128)]
        public string AspNetUser_Id1 { get; set; }

        /// <summary>
        /// Duplicate UserId field.
        /// </summary>
        [StringLength(128)]
        public string AspNetUser_Id2 { get; set; }


    }
}
