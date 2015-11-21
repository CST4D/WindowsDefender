namespace WindowsDefenderWebService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Model for the virus table.
    /// 
    /// Authors: Wilson Carpenter, Gerald Becker
    /// </summary>
    [Table("Viruses")]
    public partial class Virus
    {
        /// <summary>
        /// Primary Key.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VirusId { get; set; }

        /// <summary>
        /// The name of the virus.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// A description of the virus.
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// The movement speed of the virus.
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// The armour rating of the virus.
        /// </summary>
        public int Armour { get; set; }

        /// <summary>
        /// The resistance rating of the virus.
        /// </summary>
        public int Resistance { get; set; }

        /// <summary>
        /// The health of the virus.
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// ID of the virus' special ability, if any.
        /// </summary>
        public int? SpAbilityId { get; set; }

        /// <summary>
        /// The location of the virus' image.
        /// </summary>
        [StringLength(100)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// The virus' special ability.
        /// </summary>
        public virtual SpecialAbility SpecialAbility { get; set; }
    }
}
