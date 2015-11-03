namespace WindowsDefenderWebService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MatchHistory")]
    public partial class MatchHistory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MatchHistory()
        {
            MatchHistoryDetails = new HashSet<MatchHistoryDetail>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MatchId { get; set; }

        public int TimeElapsed { get; set; }

        public DateTime DatePlayed { get; set; }

        [Required]
        [StringLength(128)]
        public string HostId { get; set; }

        [StringLength(128)]
        public string AspNetUser_Id { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MatchHistoryDetail> MatchHistoryDetails { get; set; }
    }
}
