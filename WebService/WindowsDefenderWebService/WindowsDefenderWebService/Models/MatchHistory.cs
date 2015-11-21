namespace WindowsDefenderWebService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Model for the MatchHistories table.
    /// 
    /// Authors: Wilson Carpenter, Gerald Becker
    /// </summary>
    [Table("MatchHistory")]
    public partial class MatchHistory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MatchHistory()
        {
            MatchHistoryDetails = new HashSet<MatchHistoryDetail>();
        }

        /// <summary>
        /// Primary Key.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MatchId { get; set; }

        /// <summary>
        /// The total amount of the it took for the game to be completed.
        /// </summary>
        public int TimeElapsed { get; set; }

        /// <summary>
        /// The date on which the game was played.
        /// </summary>
        public DateTime DatePlayed { get; set; }

        /// <summary>
        /// The UserId of the host.
        /// </summary>
        [Required]
        [StringLength(128)]
        public string HostId { get; set; }

        /// <summary>
        /// Duplicate UserId field.
        /// </summary>
        [StringLength(128)]
        public string AspNetUser_Id { get; set; }

        /// <summary>
        /// A collection of the MatchHistoryDetails for each user in the game.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MatchHistoryDetail> MatchHistoryDetails { get; set; }
    }
}
