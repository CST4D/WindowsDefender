namespace WindowsDefenderWebService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    /// <summary>
    /// Model for the Towers table.
    /// 
    /// Authors: Wilson Carpenter, Gerald Becker
    /// </summary>
    public partial class Tower
    {
        /// <summary>
        /// Primary Key.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TowerId { get; set; }

        /// <summary>
        /// The name of the tower.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// A description of the tower.
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// The cost to place the tower.
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// The amount of damage that the tower does.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// The attack speed of the tower.
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// The attack range of the tower.
        /// </summary>
        public int Range { get; set; }

        /// <summary>
        /// The level at which the tower may be unlocked.
        /// </summary>
        public int LevelUnlocked { get; set; }

        /// <summary>
        /// The location of the tower's image.
        /// </summary>
        [StringLength(100)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// The ID of the tower's special ability, if any.
        /// </summary>
        public int? SpAbilityId { get; set; }

        /// <summary>
        /// The special ability of the tower, if any.
        /// </summary>
        public virtual SpecialAbility SpecialAbility { get; set; }
    }
}
