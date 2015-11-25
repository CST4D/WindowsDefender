namespace WindowsDefenderWebService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Model for the Map table.
    /// 
    /// Authors: Wilson Carpenter, Gerald Becker
    /// </summary>
    public partial class Map
    {
        /// <summary>
        /// Primary Key.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MapId { get; set; }

        /// <summary>
        /// The name of the map.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// A description of the map.
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// The difficulty rating of the map.
        /// </summary>
        public int DifficultyRating { get; set; }

        /// <summary>
        /// The location of an image for the map.
        /// </summary>
        [StringLength(100)]
        public string ImageUrl { get; set; }
    }
}
