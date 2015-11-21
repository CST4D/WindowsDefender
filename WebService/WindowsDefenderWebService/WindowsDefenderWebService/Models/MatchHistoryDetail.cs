namespace WindowsDefenderWebService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Model for the MatchHistoryDetails table. Adds extra details to the 
    /// MatchHistory table. There is one of these for each user in a given game.
    /// 
    /// Authors: Wilson Carpenter, Gerald Becker
    /// </summary>
    public partial class MatchHistoryDetail
    {
        /// <summary>
        /// Primary Key.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int HistoryId { get; set; }

        /// <summary>
        /// The ID of the match detailed.
        /// </summary>
        public int MatchId { get; set; }

        /// <summary>
        /// The UserId of the user that relates to this detail.
        /// </summary>
        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        /// <summary>
        /// True if the user won the game, false if the user lost.
        /// </summary>
        public bool WonGame { get; set; }

        /// <summary>
        /// The team that the user was on for this game.
        /// </summary>
        [Required]
        [StringLength(1)]
        public string Team { get; set; }

        /// <summary>
        /// Duplicate UserId field.
        /// </summary>
        [StringLength(128)]
        public string AspNetUser_Id { get; set; }

        /// <summary>
        /// The match history detailed.
        /// </summary>
        public virtual MatchHistory MatchHistory { get; set; }
    }
}
