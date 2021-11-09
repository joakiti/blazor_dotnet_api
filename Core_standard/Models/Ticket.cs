using System;
using System.ComponentModel.DataAnnotations;
using Core.ValidationAttributes;

namespace Core.Models
{
    public class Ticket
    {
        public int? TicketId { get; set; }

        [Required]
        public int? ProjectId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        [StringLength(50)]
        public string Owner { get; set; }

        [Ticket_DueDatePresent]
        [Ticket_EnsureDueDateAfterReportDate]
        [Ticket_EnsureFutureDueDateOnCreation]
        public DateTime? DueDate { get; set; }

        [Ticket_EnsureReportDatePresent]
        public DateTime? SubmittedDate { get; set; }

        public Project Project { get; set; }



        /// <summary>
        /// When creating a ticket, if due date is entered, it has to be in the future.
        /// </summary>
        /// <returns></returns>
        public bool ValidateFutureDueDate()
        {
            if (!DueDate.HasValue) return true;

            return (DueDate.Value > DateTime.Now);
        }

        /// <summary>
        /// When owner is assigned to the ticket, the report date has to be present
        /// </summary>
        /// <returns></returns>
        public bool ValidateReportDatePresence()
        {
            if (string.IsNullOrEmpty(Owner)) return true;

            return SubmittedDate.HasValue;
        }

        /// <summary>
        /// When owner is assigned to the ticket, the due date has to be present
        /// </summary>
        /// <returns></returns>
        public bool ValidateDueDatePresence()
        {
            if (string.IsNullOrEmpty(Owner)) return true;

            return DueDate.HasValue;
        }

        /// <summary>
        /// When Due and report date are given, Due date must arrive after submitted date
        /// </summary>
        /// <returns></returns>
        public bool ValidateDueDateAfterReportDate()
        {
            if (!DueDate.HasValue || !SubmittedDate.HasValue) return true;

            return DueDate.Value.Date >= SubmittedDate.Value.Date;

        }
    }
}