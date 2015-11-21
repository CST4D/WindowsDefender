namespace WindowsDefenderWebService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Model for the Threads table.
    /// 
    /// Authors: Wilson Carpenter, Gerald Becker
    /// </summary>
    public partial class Thread
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Thread()
        {
            ThreadPosts = new HashSet<ThreadPost>();
        }
        /// <summary>
        /// Primary Key.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ThreadId { get; set; }

        /// <summary>
        /// The UserId of the thread creator.
        /// </summary>
        [Required]
        [StringLength(128)]
        public string PostedById { get; set; }

        /// <summary>
        /// The date of when the thread was created.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The subject of the thread.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Subject { get; set; }

        /// <summary>
        /// The collection of posts in the thread.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThreadPost> ThreadPosts { get; set; }
    }
}
