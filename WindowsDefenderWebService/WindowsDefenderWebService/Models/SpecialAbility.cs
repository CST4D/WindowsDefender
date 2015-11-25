namespace WindowsDefenderWebService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Model for the SpecialAbilities table.
    /// 
    /// Authors: Wilson Carpenter, Gerald Becker
    /// </summary>
    [Table("SpecialAbility")]
    public partial class SpecialAbility
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SpecialAbility()
        {
            Towers = new HashSet<Tower>();
            Viruses = new HashSet<Virus>();
        }

        /// <summary>
        /// Primary Key.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SpAbilityId { get; set; }

        /// <summary>
        /// The name of the special ability.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// A description of the special ability.
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// A collection of towers that use the special ability.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tower> Towers { get; set; }

        /// <summary>
        /// A collection of viruses that use the special ability.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Virus> Viruses { get; set; }
    }
}
