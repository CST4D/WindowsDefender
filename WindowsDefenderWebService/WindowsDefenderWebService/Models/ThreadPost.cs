namespace WindowsDefenderWebService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Model for the ThreadPost table.
    /// 
    /// Authors: Wilson Carpenter, Gerald Becker
    /// </summary>
    public partial class ThreadPost
    {
        /// <summary>
        /// The ID of the post in a thread.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PostId { get; set; }

        /// <summary>
        /// The ThreadId of the thread posted in.
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// The UserId of the poster.
        /// </summary>
        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        /// <summary>
        /// The body of the post.
        /// </summary>
        [Required]
        public string Post { get; set; }

        /// <summary>
        /// The date in which the post was posted.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Duplicate UserId field.
        /// </summary>
        [StringLength(128)]
        public string AspNetUser_Id { get; set; }

        /// <summary>
        /// The thread posted in.
        /// </summary>
        public virtual Thread Thread { get; set; }
    }
}
