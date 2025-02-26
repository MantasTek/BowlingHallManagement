using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingHallManagement.Models
{
    /// <summary>
    /// Represents a bowling lane in the hall
    /// </summary>
    public class Lane
    {
        public int LaneNumber { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime? ReservedUntil { get; set; }
        
        public Lane(int laneNumber)
        {
            LaneNumber = laneNumber;
            IsAvailable = true;
            ReservedUntil = null;
        }
        
        /// <summary>
        /// Reserves the lane for a specified duration
        /// </summary>
        public void Reserve(TimeSpan duration)
        {
            if (!IsAvailable)
            {
                throw new InvalidOperationException("This lane is reserved");
            }
            
            IsAvailable = false;
            ReservedUntil = DateTime.Now.Add(duration);
        }
        
        /// <summary>
        /// Releases the lane reservation
        /// </summary>
        public void Release()
        {
            IsAvailable = true;
            ReservedUntil = null;
        }
        
        /// <summary>
        /// Updates the lane availability status based on current time
        /// </summary>
        public void UpdateAvailability()
        {
            if (!IsAvailable && ReservedUntil.HasValue && DateTime.Now > ReservedUntil.Value)
            {
                Release();
            }
        }
        
        public override string ToString()
        {
            return $"Lane #{LaneNumber} - {(IsAvailable ? "Available" : $"Reserved until {ReservedUntil?.ToString("HH:mm")}")}";
        }
    }
}

