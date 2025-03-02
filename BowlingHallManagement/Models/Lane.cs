namespace BowlingHallManagement.Models
{
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
        
        public void Reserve(TimeSpan duration)
        {
            if (!IsAvailable)
            {
                throw new InvalidOperationException("This lane is reserved");
            }
            
            IsAvailable = false;
            ReservedUntil = DateTime.Now.Add(duration);
        }
        
        public void Release()
        {
            IsAvailable = true;
            ReservedUntil = null;
        }
        
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